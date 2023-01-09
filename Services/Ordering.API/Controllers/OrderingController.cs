using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ordering.API.Repositories;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderingController : ControllerBase
    {
        private readonly IOrderingRepository _repository;
        private readonly ILogger<OrderingController> _logger;

        public OrderingController(IOrderingRepository orderingRepository, ILogger<OrderingController> logger)
        {
            _repository = orderingRepository;
            _logger = logger;
        }

    }
}
