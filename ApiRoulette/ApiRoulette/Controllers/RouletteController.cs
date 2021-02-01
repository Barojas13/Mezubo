using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiRoulette.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        Roulette Admin;

        public RouletteController()
        {
            Admin = new Roulette();
        }

        [HttpGet]
        [Route("Insert")]
        public IActionResult Insert()
        {
            try
            {
                int result = Admin.CreateRoulette();

                return Ok(result);

            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }

        [HttpGet("Open/{id}")]
        public IActionResult Open(string id)
        {
            try
            {
                bool result = Admin.OpenRoulette(id);
                if (result)
                {
                    return Ok("La ruleta fue actualizada exitosamente");
                }
                else
                {
                    return Ok("Error intentar abrir la ruleta");
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("List")]
        public IActionResult List()
        {
            try
            {
                Dictionary<string, string> result = Admin.ListRoulette();

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
