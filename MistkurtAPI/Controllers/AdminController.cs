using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace MistkurtAPI.Controllers
{
    [Route("api/admin")]
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


        // POST: api/Admin
        [HttpPost]
        public async Task<ActionResult> PostUser(User user)
        {
            if(_postgres.UserExistsByEmail(user.Email))
                return Conflict();

            await _postgres.AddNewUserAsync(user);

            return Ok();
        }

        // DELETE: api/Admin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            bool status = await _postgres.DeleteUser(id);

            if (status)
                return NoContent();
            else
                return NotFound();
        }


    }
}
