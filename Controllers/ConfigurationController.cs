using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ApiConfiguration globalConfiguration;

        public ConfigurationController(IOptions<ApiConfiguration> configuration)
        {
            globalConfiguration = configuration.Value;
        }

        [HttpGet]
        public ActionResult<ApiConfiguration> GetApiConfiguration()
        {
            var config = new ApiConfiguration
            {
                MaxTrainingPerDay = globalConfiguration.MaxTrainingPerDay,
                LoggedInUser = this.HttpContext.User.Identity.Name
            };
            return config;
        }
    }
}
