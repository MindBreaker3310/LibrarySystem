using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using EventBus.Messages.Event;
using Ordering.API.Entities;
using Ordering.API.Repositories;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IOrderingRepository _repository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BasketCheckoutConsumer> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketCheckoutConsumer(IOrderingRepository orderingRepository, IHttpClientFactory httpClientFactory, ILogger<BasketCheckoutConsumer> logger, IConfiguration configuration, IPublishEndpoint publishEndpoint)
        {
            _repository = orderingRepository;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            //var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
            //var result = await _mediator.Send(command);

            var checkoutData = new OrderRecord()
            {
                Id = $"{context.Message.Id}_OrderRecord",//訂單編號
                Borrower = context.Message.UserName,//FC5233
                BorrowItems = context.Message.BorrowItems,//["SKL-2022-MT-99", "XX維護合約", 歸還時間null]
                BorrowTime = DateTime.Now,
                Completed = false,//完成訂單 當BorrowItems裡的歸還時間都不是null >> true
                CompletedTime = null
            };
            //建立訂單
            var recordResult = await _repository.CreateOrderRecord(checkoutData);


            //讀取catalog狀態
            string docId = context.Message.BorrowItems[0].ItemId;
            //string CatalogUrl = "http://host.docker.internal:8000/api/Catalog/GetDocumentItemById/";
            string CatalogUrl = $"{_configuration["UrlStrings:CatalogUrl"]}/GetDocumentItemById/";
            var client = _httpClientFactory.CreateClient();
            var currentResult = await client.GetAsync($"{CatalogUrl}{docId}");
            if (!currentResult.IsSuccessStatusCode) { return; }


            string responseBody = await currentResult.Content.ReadAsStringAsync();
            var currentDoc = JsonConvert.DeserializeObject<DocumentItem>(responseBody);
            if(currentDoc.FileStatus != "Y")
            {
                _logger.LogWarning($"合約狀態不可外借");
                return;
            }

            //更新catalog狀態
            currentDoc.FileStatus = "N";

            string jsonString = JsonConvert.SerializeObject(currentDoc);
            var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
            //var updateResult = await client.PutAsync("http://host.docker.internal:8000/api/Catalog", payload);
            var updateResult = await client.PutAsync(_configuration["UrlStrings:CatalogUrl"], payload);
            if (!updateResult.IsSuccessStatusCode) { return; }
            var updateResponseString = await updateResult.Content.ReadAsStringAsync();
            if (updateResponseString == "true")
            {
                _logger.LogInformation($"BasketCheckoutEvent consumed successfully. Created Order Id : {context.Message.UserName}");
            }
            _logger.LogInformation($"BasketCheckoutEvent consumed failed. Created Order Id : {context.Message.UserName}. Response = {updateResponseString}");
        }

    }
}
