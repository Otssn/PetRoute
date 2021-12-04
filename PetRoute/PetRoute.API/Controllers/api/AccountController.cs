using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetRoute.API.Data;
using PetRoute.API.Data.Entities;
using PetRoute.API.Helpers;
using PetRoute.API.Models;
using PetRoute.API.Models.Request;
using PetRoute.commons.Enums;
using System;
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
                        return CreateToken(user);
                    }
                }
            }

            return BadRequest();
        }

        private IActionResult CreateToken(User user)
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
        [HttpPost]
        [Route("SocialLogin")]
        public async Task<IActionResult> SocialLogin([FromBody] SocialLoginRequest model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Email);
                if (user != null)
                {
                    if (user.logerType != model.LogerType)
                    {
                        return BadRequest("El usuario ya inicio secion previamente por Email o por otra red social");
                    }
                    Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.ValidatePasswordAsync(user, model.Id);

                    if (result.Succeeded)
                    {
                        await UpdateUserAsync(user, model);
                        return CreateToken(user);
                    }
                }
                else
                {
                    await CreateUserAsync(model);
                    user = await _userHelper.GetUserAsync(model.Email);
                    return CreateToken(user);
                }
            }

            return BadRequest();
        }

        private async Task UpdateUserAsync(User user, SocialLoginRequest model)
        {
            user.SocialImageURL = model.PhotoURL;
            if (string.IsNullOrEmpty(model.FirstName))
            {
                user.FirstName = model.FirstName;
            }
            if (string.IsNullOrEmpty(model.LastName))
            {
                user.LastName = model.LastName;
            }
            await _userHelper.UpdateUserAsync(user);
        }

        private async Task CreateUserAsync(SocialLoginRequest model)
        {
            FirsLastName firsLastName = SeparateFirsLastName(model.FullName);
            if (string.IsNullOrEmpty(model.FirstName))
            {
                model.FirstName = firsLastName.FirstName;
            }
            if (string.IsNullOrEmpty(model.LastName))
            {
                model.LastName = firsLastName.LastName;
            }
            User user = new()
            {
                Address = "Pendiente",
                Document = "Pendiente",
                DocumentType = _context.documentTypes.FirstOrDefault(),
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                logerType = model.LogerType,
                PhoneNumber = "Pendiente",
                SocialImageURL = model.PhotoURL,
                UserName = model.Email,
                userType = UserType.User
            };
            await _userHelper.AddUserAsync(user, model.Id);
            await _userHelper.AddUserToRoleAsync(user, user.userType.ToString());
            string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            await _userHelper.ConfirmEmailAsync(user, token);
        }

        private FirsLastName SeparateFirsLastName(string fullName)
        {
            int pos = fullName.IndexOf(' ');
            FirsLastName firsLastName = new();
            if(pos == -1)
            {
                firsLastName.FirstName = fullName;
                firsLastName.LastName = fullName;
            }
            else
            {
                firsLastName.FirstName = fullName.Substring(0, pos);
                firsLastName.LastName = fullName.Substring(pos+1, fullName.Length-pos-1);
            }
            return firsLastName;
        }
    }
}
