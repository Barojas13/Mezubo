using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace ApiWager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WagerController : ControllerBase
    {
        Wager Admin;
        public WagerController()
        {
            Admin = new Wager();
        }

        [HttpPost]
        [Route("CreateWager")]
        public IActionResult CreateWager([FromBody] WagerModel ObjWager)
        {
            try
            {
                IHeaderDictionary headers = Request.Headers;
                if (headers.ContainsKey("user"))
                {
                    string iduser = headers["user"];
                    string result = Admin.CreateWager(ObjWager, iduser);

                    return Ok(result);
                }
                else
                {
                    return StatusCode(403);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("ClosetWager/{id}")]
        public IActionResult ListWager(string id)
        {
            try
            {
                string result = Admin.ListWager(id);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
