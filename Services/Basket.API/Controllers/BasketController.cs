using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Basket.API.Entities;
using Basket.API.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EventBus.Messages.Event;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : Controller
    {
        private readonly IBasketRepository _repository;
        private readonly ILogger<BasketController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository context, ILogger<BasketController> logger, IPublishEndpoint publishEndpoint)
        {
            _repository = context;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Cart>> GetBasket(string userId)
        {
            var cart = await _repository.GetBasket(userId);
            if (cart == null)
            {
                cart = await _repository.CreateNewBasket(userId);
            }
            _logger.LogInformation("GetBasket");
            return Ok(cart);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateBasket([FromBody] Cart basket)
        {
            bool result = await _repository.UpdateBasket(basket);
            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Cart>> DeleteBasket(string userId)
        {
            bool result = await _repository.DeleteBasket(userId);
            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Checkout(string userId)
        {
            //取得Cart狀態
            //發送checkout event給rabbitmq
            //清空Cart

            var userCart = await _repository.GetBasket(userId);

            if (userCart == null || userCart.Items.Count == 0)
            {
                return BadRequest();
            }

            //建立訂單
            BasketCheckoutEvent order = new BasketCheckoutEvent()
            {
                UserName = userId,
                BorrowItems = new List<BorrowItem>()
            };

            foreach (var item in userCart.Items)
            {
                order.BorrowItems.Add(new BorrowItem()
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    ReturnTime = null
                });
            }

            //傳送Event給RabbitMQ
            await _publishEndpoint.Publish(order);
            //清空Cart
            await _repository.DeleteBasket(userId);

            return Accepted();
        }
    }
}
