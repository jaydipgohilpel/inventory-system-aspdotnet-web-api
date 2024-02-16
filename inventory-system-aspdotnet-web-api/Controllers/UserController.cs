using inventory_system_aspdotnet_web_api.Models;
using inventory_system_aspdotnet_web_api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace inventory_system_aspdotnet_web_api.Controllers
{
    [AllowAnonymous]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        ResponseHelper Responce = new ResponseHelper();
        GetDataResponseHelper GetDataResponce = new GetDataResponseHelper();
        LoginResponseHelper LoginResponce = new LoginResponseHelper();

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        [Route("register")]
        public ActionResult<IEnumerable<Users>> Post(Users users)
        {
            if (users.Name == null || users.Password == null || users.Email == null)
                return BadRequest("all fields are required");
            try
            {
                _userRepository.AddOrUpdateUser(null, users);
                Responce.Message = "Registration successfully.";
                return Ok(Responce);
            }
            catch (SqlException ex)
            {
                // Handle SQL exceptions
                if (ex.Number == 50000)
                {
                    Responce.Message = ex.Message;
                    Responce.Success = false;
                    return BadRequest(Responce);

                }
                else
                {
                    return StatusCode(Constants._ERRORCODE, "An error occurred while processing your request.");
                }

            }
           
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(Constants._ERRORCODE, ex.Message);
            }
        }

        // POST api/<UserController>
        [HttpPost]
        [Route("login")]
        public IActionResult login(LoginUsers users)
        {
            if (users.Password == null || users.Email == null)
                return BadRequest("all fields are required");
            try
            {
                var token = _userRepository.LoginUser(users);
                LoginResponce.token = token;
                return Ok(LoginResponce);
            }
            catch (SqlException ex)
            {
                // Handle SQL exceptions
                if (ex.Number == 50000)
                {
                    Responce.Message = ex.Message;
                    Responce.Success = false;
                    return BadRequest(Responce);

                }
                else
                {
                    return StatusCode(Constants._ERRORCODE, "An error occurred while processing your request.");
                }

            }

            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(Constants._ERRORCODE, ex.Message);
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
