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
using AutoMapper;
using Entities.DataTransferObjects;

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
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public UserController(IRepositoryWrapper repository, IConfiguration configuration, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _jwtGenerator = new JwtGenerator(configuration.GetValue<String>("JwtPrivateSigningKey"));
            _logger = logger;
            _mapper = mapper;
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

            if (!_repository.User.EmailExists(payload.Email) || token == null)
                return Conflict();
   
            User user = _repository.User.GetUserByEmail(payload.Email);
            user.Token = token;
            _repository.User.UpdateUser(user);
            _repository.Save();
            UserDto userResult = _mapper.Map<UserDto>(user);

            return Ok(userResult);
        }


        [AllowAnonymous]
        [HttpPost("checkToken")]
        public IActionResult CheckToken([FromBody] CheckTokenRequest data)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            
            if(string.IsNullOrEmpty(token) || !_repository.User.EmailExists(data.Email))
              return NotFound();

            token = token.ToString().Split(" ")[1].ToString();
            User user = _repository.User.GetUserByEmail(data.Email);
            if (!user.Token.Equals(token))
                return Conflict();

            UserDto userResult = _mapper.Map<UserDto>(user);
            return Ok(userResult);
        }



        [AllowAnonymous]
        [HttpGet("{id}/account")]
        public IActionResult GetUserWithDetails(Guid id)
        {
            User user = _repository.User.GetUserWithDetails(id);
            if (user == null)
                return NotFound();
            UserDto userResult = _mapper.Map<UserDto>(user);
            return Ok(userResult);
        }
    }
}
