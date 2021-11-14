using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using covidAPI.DataAccess.@interface;
using covidAPI.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace covidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IcovidDataAccess _icovidDataAccess;


        public WeatherForecastController( IcovidDataAccess icovidDataAccess)
        {
            _icovidDataAccess = icovidDataAccess;
        }


        [HttpPost("api/registerUser")]
        public async Task<IActionResult> registerUser([FromBody]user _user )
        {
            var x = await _icovidDataAccess.registerUser(_user);
            return Ok("return true");
        }


    }
}