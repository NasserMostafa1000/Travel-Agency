using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalamaTravelBL;
using SalamaTravelDTA;

namespace SalamaTravelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        [HttpGet("GetOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrders( string Token,  int ClientId)
        {
            if (string.IsNullOrEmpty(Token))
                return BadRequest("Token is required.");
            if (ClientId <= 0)
                return BadRequest("Invalid ClientId.");

            try
            {
                var orders = await SalamaTravelBL.OrdersBL.GetOrdersAsync(Token, ClientId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("AddOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrder(  OrdersBL.AddOrderRequest request)
        {
            
            if (string.IsNullOrEmpty(request.Token))
                return BadRequest("Token is required.");
            if (request == null)
                return BadRequest("Request body is required.");
            if (request.ClientId <= 0 || request.VisaId <= 0 || request.OrderTypeId <= 0)
                return BadRequest("Invalid input values.");

            try
            {
                bool result = await SalamaTravelBL.OrdersBL.AddOrderASync(request.Token, request.ClientId, request.VisaId, request.OrderTypeId);
                if (result)
                    return Ok("Order added successfully.");
                return BadRequest("Failed to add order.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("AcceptOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AcceptOrder( int OrderId,int OwnerId)
        {
        
            if (OrderId <= 0)
                return BadRequest("Invalid OrderId.");

            try
            {
                bool result = await SalamaTravelBL.OrdersBL.AcceptOrderAsync(OrderId, OwnerId);
                if (result)
                    return Ok("Order accepted successfully.");
                return BadRequest("Failed to accept order.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("RejectOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RejectOrder( string Token, OrdersBL.RejectOrBadOrderRequest request)
        {
            if (string.IsNullOrEmpty(Token))
                return BadRequest("Token is required.");
            if (request == null)
                return BadRequest("Request body is required.");
            if (request.OrderId <= 0 || string.IsNullOrEmpty(request.Reason))
                return BadRequest("Invalid input values.");

            try
            {
                bool result = await SalamaTravelBL.OrdersBL.RejectOrderAsync(Token, request.OrderId, request.Reason);
                if (result)
                    return Ok("Order rejected successfully.");
                return BadRequest("Failed to reject order.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("BadOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BadOrder(string Token, OrdersBL.RejectOrBadOrderRequest request)
        {
            if (string.IsNullOrEmpty(Token))
                return BadRequest("Token is required.");
            if (request == null)
                return BadRequest("Request body is required.");
            if (request.OrderId <= 0 || string.IsNullOrEmpty(request.Reason))
                return BadRequest("Invalid input values.");

            try
            {
                bool result = await SalamaTravelBL.OrdersBL.BadOrderAsync(Token, request.OrderId, request.Reason);
                if (result)
                    return Ok("Order rejected successfully.");
                return BadRequest("Failed to reject order.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("FindById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderById( string token, int clientId,int orderId)
        {
            try
            {
                // استدعاء طبقة الأعمال
                var order = await OrdersBL.FindOrderByIdAsync(token, clientId, orderId);

                if (order == null)
                    return NotFound(new { Message = "Order not found." });

                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                // معالجة الأخطاء الناتجة عن المدخلات
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء العامة
                return StatusCode(500, new { Error = ex.Message });
            }
        }


        [HttpPost("FindOrdersBelongsToSpecificClient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FindOrdersBelongsToSpecificClient([FromBody] OrdersBL.FindOrdersRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Token) || request.ClientId <= 0)
            {
                return BadRequest("Invalid request parameters.");
            }

            try
            {
                var orders = await OrdersDAL.FindOrdersBelongsToSpecificClient(request.Token, request.ClientId);

                if (orders == null || !orders.Any())
                {
                    return NotFound("No orders found for the specified client.");
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
    }


    }


