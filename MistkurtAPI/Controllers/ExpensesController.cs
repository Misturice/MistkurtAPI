using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MistkurtAPI.Models;
using MistkurtAPI.Classes.Databases;
namespace MistkurtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly MistKurtContext _context;

        public ExpensesController(MistKurtContext context)
        {
            _context = context;
        }


        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expenses>> GetExpenses(Guid id)
        {
            return await Postgres.GetUserExpenses(id, _context);
        }

        // GET: api/Expenses/5/2021-05-01
        [HttpGet("{id}/{startDate}/{endDate}")]
        public async Task<ActionResult<IEnumerable<Expenses>>> GetExpensesByDate(Guid id, int startDate, int endDate)
        {
            return await Postgres.GetUserExpensesByDateAsync(id, startDate, endDate, _context);
        }
    }
}
