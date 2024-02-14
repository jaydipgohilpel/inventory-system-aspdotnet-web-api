using inventory_system_aspdotnet_web_api.Models;
using inventory_system_aspdotnet_web_api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace inventory_system_aspdotnet_web_api.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase   
    {
        private readonly OrderRepository _orderRepository;

        public OrderController(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: api/order
        [HttpGet]
        public ActionResult<IEnumerable<GetOrder>> Get()
        {
            try
            {
                var orders = _orderRepository.GetAllOrders();
                return Ok(new { data = orders, success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/order
        [HttpPost]
        public ActionResult<int> Post(AddOrder order)
        {

            if (order.OrderDetailQuantity==0||order.OrderDetailUnitPrice==0) return BadRequest("all fields are required");

            try
            {
                _orderRepository.AddOrUpdateOrder(null,order);
                return Ok(new { message = "Order added successfully.", success = true });
            }
            catch (SqlException ex)
            {
                // Handle SQL exceptions
                if (ex.Number == 50000)
                {
                    // Custom error message from the stored procedure
                    return BadRequest(ex.Message);
                }
                else
                {
                    // Other SQL exceptions
                    return StatusCode(500, ex.Message);
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/order/5
        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        public IActionResult Put(int id, AddOrder order)
        {

            if (order.OrderDetailQuantity == 0 || order.OrderDetailUnitPrice == 0) return BadRequest();

           
            try
            {
                _orderRepository.AddOrUpdateOrder(id, order);
                return Ok(new { message = "Order updated successfully.", success = true });
            }
            catch (SqlException ex)
            {
                // Handle SQL exceptions
                if (ex.Number == 50000)
                {
                    // Custom error message from the stored procedure
                    return BadRequest(ex.Message);
                }
                else
                {
                    // Other SQL exceptions
                    return StatusCode(500, ex.Message);
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/order/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id == null) return BadRequest();

            try
            {
                var rowsAffected = _orderRepository.DeleteOrder(id);
                if (rowsAffected > 0)
                    return Ok(new { message = "Order deleted successfully.", success = true });
                else return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);;
            }
        }

    }
}
