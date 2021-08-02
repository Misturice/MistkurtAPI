using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MistkurtAPI;
using MistkurtAPI.Models;
using MistkurtAPI.Classes.Databases;

namespace MistkurtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly Postgres _postgres;

        public AdminController(MistKurtContext context)
        {
            _postgres = new(context);
        }

        // GET: api/Admin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return _postgres.ReturnAllUsers();
        }


        // POST: api/Admin
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
