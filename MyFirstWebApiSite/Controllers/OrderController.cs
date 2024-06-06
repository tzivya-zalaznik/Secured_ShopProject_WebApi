using AutoMapper;
using DTOs;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyFirstWebApiSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService _orderService;
        private IMapper _mapper;
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        // POST api/<OrderController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> Post([FromBody] OrderDTO order)
        {
            Order orderToAdd = _mapper.Map<OrderDTO, Order>(order);
            Order theAddOrder = await _orderService.CreateOrder(orderToAdd);
            OrderDTO newAddOrder = _mapper.Map<Order, OrderDTO>(theAddOrder);
            if (newAddOrder != null)
                return Ok(newAddOrder);
            return BadRequest();
        }
    }
}
