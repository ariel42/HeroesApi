using log4net;
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
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ApiConfiguration globalConfiguration;
        private readonly ILog logger;
        public ConfigurationController(IOptions<ApiConfiguration> configuration, ILoggerManager loggerManager)
        {
            globalConfiguration = configuration.Value;
            logger = loggerManager.Logger;
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
