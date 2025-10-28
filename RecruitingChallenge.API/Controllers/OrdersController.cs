using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using RecruitingChallenge.API.DTOs.Order;
using RecruitingChallenge.API.Filters;
using RecruitingChallenge.Common.Models;
using RecruitingChallenge.Domain.Enums;
using RecruitingChallenge.Service.Orders;

namespace RecruitingChallenge.API.Controllers
{
    [ApiController]
    [AuthorizeCustom]
    [Route("api/v1/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<GetOrderResponse>>> GetOrders([FromQuery] GetOrderFilterRequest request)
        {
            var orders = await _orderService.GetAllOrders(request.ToServiceModel());

            var listOrdersResponse = new List<GetOrderResponse>();
            foreach (var order in orders.Items)
            {
                listOrdersResponse.Add(GetOrderResponse.FromDomain(order));
            }

            var response = new PagedResult<GetOrderResponse>(
                listOrdersResponse,
                orders.NextCursorValue,
                orders.NextCursor,
                orders.HasNextPage);

            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetOrderById")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);

            var orderResponse = GetOrderResponse.FromDomain(order);

            return Ok(orderResponse);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderRequest dto)
        {
            await _orderService.UpdateOrderStatus(id, dto.NewStatus);

            return NoContent();
        }
    }
}
