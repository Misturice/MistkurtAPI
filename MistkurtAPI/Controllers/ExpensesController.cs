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
        private readonly Postgres _postgres;

        public ExpensesController(MistKurtContext context)
        {
            _postgres = new(context);
        }


        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public IEnumerable<Expenses> GetExpenses(Guid id)
        {
            return _postgres.GetUserExpensesAsync(id);
        }

        // GET: api/Expenses/5/2021-05-01
        [HttpGet("{id}/{startDate}/{endDate}")]
        public async Task<ActionResult<IEnumerable<Expenses>>> GetExpensesByDate(Guid id, int startDate, int endDate)
        {
            return await _postgres.GetUserExpensesByDateAsync(id, startDate, endDate);
        }
    }
}
