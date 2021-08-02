using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MistkurtAPI;
using MistkurtAPI.Classes.Databases;
using MistkurtAPI.Classes.Finance;

namespace MistkurtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly Postgres _postgres;

        public ProductsController(MistKurtContext context)
        {
            _postgres = new(context);
        }


        // POST : api/Products/asd123
        [HttpPost("{id}")]
        public async Task<IActionResult> AddProducts(IEnumerable<Models.Product> products, Guid id)
        {
            Expense expense = new(_postgres, -1, id);
            await expense.AddProducts(products);
            return Ok();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            bool status = await _postgres.DeleteProductByID(id);
            if (!status)
                return NotFound();
            else
                return NoContent();
        }


    }
}
