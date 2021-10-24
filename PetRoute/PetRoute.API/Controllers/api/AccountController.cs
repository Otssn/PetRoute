using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetRoute.API.Data;
using PetRoute.API.Data.Entities;
using PetRoute.API.Helpers;
using PetRoute.API.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetRoute.API.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IMailHelper _mailHelper;
        private readonly IBlobHelper _blobHelper;
        public AccountController(IUserHelper userHelper, IConfiguration configuration, DataContext context, IMailHelper mailHelper, IBlobHelper blobHelper)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _context = context;
            _mailHelper = mailHelper;
            _blobHelper = blobHelper;
        }
        [HttpPost]
        [Route("CreateToken")]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Username);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.ValidatePasswordAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        Claim[] claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        JwtSecurityToken token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(99),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            user
                        };

                        return Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }
    }
}
