using System;
using System.Collections.Generic;
using AutoMapper;
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace MistkurtAPI.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public AdminController(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/admin/getUsers
        [HttpGet("getUsers")]
        public  ActionResult GeAllUsers()
        {
            IEnumerable<User> users = _repository.User.GetAllUsers();
            IEnumerable<UserDto> usersResult = _mapper.Map<IEnumerable<UserDto>>(users);

            return Ok(usersResult);
        }

        // GET: api/admin/getUser/{id}
        [HttpGet("getUser/{id}")]
        public IActionResult GetUserById(Guid id)
        {
            User user = _repository.User.GetUserById(id);

            if(user == null)
            {
                _logger.LogError($"Owner with id: {id} not found");
                return NotFound();
            }

            UserDto userResult = _mapper.Map<UserDto>(user);
            return Ok(userResult);
        }


        // POST: api/admin
        [HttpPost]
        public ActionResult CreateUser([FromBody] UserForCreationDto user)
        {
            if(_repository.User.EmailExists(user.Email))
                return BadRequest();

            User userEntity = _mapper.Map<User>(user);

            _repository.User.CreateUser(userEntity);
            _repository.Save();

            UserDto createdUser = _mapper.Map<UserDto>(userEntity);

            return Created("createUser", createdUser);
        }

        //PUT: api/admin/5
        [HttpPut("{id}")]
        public ActionResult UpdateUser(Guid id, [FromBody] UserForUpdateDto user)
        {
            User userEntity = _repository.User.GetUserById(id);

            if (userEntity == null)
                return NotFound("User not found");

            _mapper.Map(user, userEntity);
            _repository.User.UpdateUser(userEntity);
            _repository.Save();
            return NoContent();
        }

        // DELETE: api/admin/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            User user = _repository.User.GetUserById(id);
            if (user == null)
                return NotFound("User not found");

            _repository.User.DeleteUser(user);
            _repository.Save();

            return NoContent();
        }


    }
}
