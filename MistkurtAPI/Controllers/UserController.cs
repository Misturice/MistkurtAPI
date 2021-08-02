using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using MistkurtAPI.Classes.Auth;
using Microsoft.Extensions.Logging;
using MistkurtAPI.Classes.Databases;
using System.Threading.Tasks;

namespace MistkurtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly Postgres _postgres;

        public class AuthenticateRequest
        {
            [Required]
            public string IdToken { get; set; }
        }

        public class CheckTokenRequest
        {
            [Required]
            [FromBody]
            public string Email { get; set; }
        }

        private readonly JwtGenerator _jwtGenerator;
        private readonly ILogger _logger;

        public UserController(IConfiguration configuration, ILogger<UserController> logger, MistKurtContext context)
        {
            _jwtGenerator = new JwtGenerator(configuration.GetValue<String>("JwtPrivateSigningKey"));
            _logger = logger;
            _postgres = new(context);
        }

    

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateRequest data)
        {
            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();
            settings.Audience = new List<string>() { "779499495532-sgb4ddi6eqodamcdlq83bt9i4keib3eo.apps.googleusercontent.com" }; // TODO Add to env variables
            GoogleJsonWebSignature.Payload payload = null;
            try
            {
                payload = GoogleJsonWebSignature.ValidateAsync(data.IdToken, settings).Result;
            }
            catch (Exception)
            {
                payload = null;
            }

            string token = null;
            if(payload != null)
                token = _jwtGenerator.CreateUserAuthToken(payload.Email);

            if(token != null && _postgres.UserExistsByEmail(payload.Email))
            {
                Models.User user = await _postgres.FindUserByEmailAsync(payload.Email);
                user.Token = token;
                await _postgres.UpdateUser(user);
            }
            else
               return Conflict();

            return Ok(new { AuthToken =  token});
        }


        [AllowAnonymous]
        [HttpPost("checkToken")]
        public async Task<IActionResult> CheckToken([FromBody] CheckTokenRequest data)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            
            if(string.IsNullOrEmpty(token) || _postgres.UserExistsByEmail(data.Email))
              return NotFound();

            token = token.ToString().Split(" ")[1];
            Models.User user = await _postgres.FindUserByEmailAsync(data.Email);

            return Ok(new { Authorized = user.Token == token});
        }
    }
}
