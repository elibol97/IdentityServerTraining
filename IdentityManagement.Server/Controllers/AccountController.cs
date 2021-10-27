using IdentityManagement.Infrastructure.Persistance;
using IdentityManagement.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityManagement.Server.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> usrmngr;

        public AccountController(UserManager<AppUser> usrmngr)
        {
            this.usrmngr = usrmngr;
        }

        [HttpPost("/api/user/register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequestViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new AppUser
            {
                UserName = request.UserName,
                Name = request.Name,
                Email = request.Email
            };
            var result = await usrmngr.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await usrmngr.AddClaimAsync(user, new Claim("userName", user.UserName));
            await usrmngr.AddClaimAsync(user, new Claim("name", user.Name));
            await usrmngr.AddClaimAsync(user, new Claim("email", user.Email));
            await usrmngr.AddClaimAsync(user, new Claim("role", "user"));
            return Ok();
        }
    }
}
