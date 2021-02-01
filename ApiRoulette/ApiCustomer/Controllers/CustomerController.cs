using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace ApiCustomer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        Customer Admin;
        public CustomerController()
        {
            Admin = new Customer();
        }

        [HttpPost]
        [Route("CreateCustomer")]
        public IActionResult CreateCustomer([FromBody] User customer)
        {
            try
            {
                int result = Admin.CreateCustomer(customer);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
