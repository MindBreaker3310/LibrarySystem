using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IDocumentItemRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IDocumentItemRepository context, ILogger<CatalogController> logger)
        {
            _repository = context;
            _logger = logger;
        }

        /// <summary>
        /// 取得所有檔案
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DocumentItem>>> GetDocumentItems()
        {
            var documentItems = await _repository.GetDocumentItems();

            return Ok(documentItems);
        }

        /// <summary>
        /// 依照合約歸檔編號查詢(單筆)
        /// </summary>
        /// <param name="id">合約歸檔編號</param>
        /// <returns></returns>
        [HttpGet("[action]/{id}", Name = "GetDocumentItemById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DocumentItem>> GetDocumentItemById(string id)
        {
            var documentItem = await _repository.GetDocumentItem(id);
            if (documentItem == null)
            {
                _logger.LogError($"文件Id: {id}, 查無資料。");
                return NotFound();
            }
            return Ok(documentItem);
        }

        /// <summary>
        /// 依照合約名稱查詢(多筆)
        /// </summary>
        /// <param name="name">合約名稱</param>
        /// <returns></returns>
        [Route("[action]/{name}", Name = "GetDocumentItemsByName")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DocumentItem>>> GetDocumentItemsByName(string name)
        {
            var documentItems = await _repository.GetDocumentItemsByName(name);

            return Ok(documentItems);
        }

        /// <summary>
        /// 依照合約原始編號查詢(多筆)
        /// </summary>
        /// <param name="number">原始號碼</param>
        /// <returns></returns>
        [Route("[action]/{category}", Name = "GetDocumentItemsByCategory")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DocumentItem>>> GetDocumentItemsByCategory(string number)
        {
            var documentItems = await _repository.GetDocumentItemsByNumber(number);

            return Ok(documentItems);
        }

        /// <summary>
        /// 建立新檔案
        /// </summary>
        /// <param name="documentItem"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DocumentItem>>> CreateDocumentItem([FromBody] DocumentItem documentItem)
        {
            bool result = await _repository.CreateDocumentItem(documentItem);
            if (result)
            {
                return CreatedAtRoute("GetDocumentItemById", new { id = documentItem.FileId }, documentItem);
            }
            return BadRequest();
        }


        /// <summary>
        /// 更新檔案狀態
        /// </summary>
        /// <param name="documentItem"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DocumentItem>>> UpdateDocumentItem([FromBody] DocumentItem documentItem)
        {
            bool updateResult = await _repository.UpdateDocumentItem(documentItem);

            return Ok(updateResult);
        }
    }
}