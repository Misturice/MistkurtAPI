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
        private readonly MistKurtContext _context;

        public AdminController(MistKurtContext context )
        {
            _context = context;
        }

        // GET: api/Admin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await Postgres.ReturnAllUsers(_context);
        }


        // POST: api/Admin
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostUser(User user)
        {
            if(Postgres.UserExistsByEmail(user.Email, _context))
                return Conflict();

            await Postgres.AddNewUserAsync(user, _context);

            return Ok();
        }

        // DELETE: api/Admin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            bool status = await Postgres.DeleteUser(id, _context);

            if (status)
                return NoContent();
            else
                return NotFound();
        }


    }
}
