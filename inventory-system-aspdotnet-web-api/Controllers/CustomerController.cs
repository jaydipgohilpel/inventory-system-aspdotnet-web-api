using inventory_system_aspdotnet_web_api.Models;
using inventory_system_aspdotnet_web_api.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace inventory_system_aspdotnet_web_api.Controllers
{
    [Route("api/customer")]
    [ApiController]
    

    public class CustomerController : ControllerBase
    {
        public static int _ERRORCODE = 500;
        private readonly CustomerRepository _customerRepository;

        public CustomerController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GetCustomer>> Get()
        {
            try
            {
                List<GetCustomer> customer = new List<GetCustomer>();
                customer = _customerRepository.GetAllCustomer().ToList();
                return Ok(new { data = customer, success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(_ERRORCODE, ex.Message);
            }
        }


        // POST api/customer
        [HttpPost]
        public ActionResult<IEnumerable<Customer>> Post(Customer customer)
        {
            if (customer == null) return BadRequest();

            try
            {
                var rowsAffected = _customerRepository.AddOrUpdateCustomer(null,customer);
                if (rowsAffected == 1)
                    return Ok(new { message = "Customer added successfully.", success = true });
                else return BadRequest();

            }
            catch (Exception ex)
            {
                return StatusCode(_ERRORCODE, ex.Message);;
            }
        }

        // PUT api/customer/5
        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        public ActionResult<IEnumerable<Customer>> Put(int id, Customer customer)
        {
            if (customer == null || id == null) return BadRequest();

            try
            {
                var rowsAffected = _customerRepository.AddOrUpdateCustomer(id, customer);
                if (rowsAffected == 1)
                    return Ok(new{ message = "Customer Updated successfully.", success = true });
                else return BadRequest();

            }
            catch (Exception ex)
            {
                return StatusCode(_ERRORCODE, ex.Message);;
            }
        }

        // DELETE api/customer/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if (id == null) return BadRequest();

            try
            {
                var rowsAffected = _customerRepository.DeleteCustomer(id);
                if (rowsAffected == 1)
                    return Ok(new { message = "Customer Deleted successfully.", success = true });
                else return NotFound();

            }
            catch (Exception ex)
            {
                return StatusCode(_ERRORCODE, ex.Message);;
            }
        }
    }
}
