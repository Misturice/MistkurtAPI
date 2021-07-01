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

namespace MistkurtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
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

        public UserController(IConfiguration configuration, ILogger<UserController> logger)
        {
            _jwtGenerator = new JwtGenerator(configuration.GetValue<String>("JwtPrivateSigningKey"));
            _logger = logger;
        }

    

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest data)
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

            if(token != null)
            {
                Redis.cli.Set($"auth:{payload.Email}", token);
            }

            return Ok(new { AuthToken =  token});
        }

        [AllowAnonymous]
        [HttpPost("checkToken")]
        public IActionResult CheckToken([FromBody] CheckTokenRequest user)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            
            if(string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            token = token.ToString().Split(" ")[1];

            string savedToken = Redis.cli.Get($"auth:{user.Email}");
            _logger.LogInformation($"saved token: {savedToken}");

            return Ok(new { Authorized = savedToken == token});
        }
    }
}
