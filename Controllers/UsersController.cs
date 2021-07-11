using HeroesApi.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class UsersController : ControllerBase
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;

        public UsersController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateUser([FromBody] Credentials credentials)
        {
            var result = await userManager.CreateAsync(new IdentityUser(credentials.Username), credentials.Password);
            if (result.Succeeded)
            {
                return await Login(credentials);
            }
            else
            {
                return Conflict(result.Errors.Select(e => e.Description));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] Credentials credentials)
        {
            var result = await signInManager.PasswordSignInAsync(credentials.Username, credentials.Password, true, false);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return Conflict("Invalid username or password");
            }
        }

        [HttpPost("logout")]
        public async Task<ActionResult<string>> Logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
