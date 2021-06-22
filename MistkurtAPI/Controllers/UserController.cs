using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using MistkurtAPI.Classes;

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

        private readonly JwtGenerator _jwtGenerator;

        public UserController(IConfiguration configuration)
        {
            _jwtGenerator = new JwtGenerator(configuration.GetValue<String>("JwtPrivateSigningKey"));
        }

    

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest data)
        {
            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();
            settings.Audience = new List<string>() { "779499495532-sgb4ddi6eqodamcdlq83bt9i4keib3eo.apps.googleusercontent.com" };
            GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(data.IdToken, settings).Result;
               
            return Ok(new { FullPayload = payload, AuthToken = _jwtGenerator.CreateUserAuthToken(payload.Email) });
        }

    }
}
