using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly AppConfiguration configuration;
        public AppController(IOptions<AppConfiguration> configuration)
        {
            this.configuration = configuration.Value;
        }

        [HttpGet("details")]
        public AppConfiguration GetAppDetails()
        {
            return this.configuration;
        }
    }
}
