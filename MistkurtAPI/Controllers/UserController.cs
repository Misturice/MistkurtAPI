using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using MistkurtAPI.Classes.Auth;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Contracts;
using Entities;

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

        private readonly IRepositoryWrapper _repository;
        private readonly JwtGenerator _jwtGenerator;
        private readonly ILogger _logger;

        public UserController(IRepositoryWrapper repository, IConfiguration configuration, ILogger logger)
        {
            _repository = repository;
            _jwtGenerator = new JwtGenerator(configuration.GetValue<String>("JwtPrivateSigningKey"));
            _logger = logger;
        }

    

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult AuthenticateAsync([FromBody] AuthenticateRequest data)
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

            if(token != null && _repository.User.Equals(payload.Email))
            {
                User user = _repository.User.GetUserByEmail(payload.Email);
                user.Token = token;
                _repository.User.Update(user);
                _repository.Save();
            }
            else
               return Conflict();

            return Ok(new { AuthToken =  token});
        }


        [AllowAnonymous]
        [HttpPost("checkToken")]
        public IActionResult CheckToken([FromBody] CheckTokenRequest data)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            
            if(string.IsNullOrEmpty(token) || _repository.User.Equals(data.Email))
              return NotFound();

            token = token.ToString().Split(" ")[1];
            User user = _repository.User.GetUserByEmail(data.Email);

            return Ok(new { Authorized = user.Token == token});
        }
    }
}
