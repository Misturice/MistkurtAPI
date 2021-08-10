using AutoMapper;
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using MistkurtAPI.Classes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistkurtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : Controller
    {

        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;


        public FinanceController(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        // POST: /api/Finance/AddProduct/5/000000000
        [HttpPost("AddProduct/{userId}/{date}")]
        public IActionResult AddProducts([FromBody] IEnumerable<ProductForCreationDto> products, Guid userId, int date)
        {
            Expenses expense = null;
            bool createdNewExpense = false;
            if (!_repository.Expenses.ExpenseExists(userId, date))
            {
                expense = new();
                expense.Date = date;
                expense.Total = 0f;
                expense.UserId = userId;
                expense.Products = new List<Product>();
                createdNewExpense = true;
            }
            else
                expense = _repository.Expenses.GetUserExpenseByDateWithDetails(userId, date);



            float newTotalCost = 0f;

            foreach (ProductForCreationDto product in products)
            {
                newTotalCost += product.Cost;
                Product productEntity = _mapper.Map<Product>(product);
                productEntity.ExpensesId = expense.Id;
                expense.Products.Add(productEntity);
                // _repository.Product.CreateProduct(productEntity);
            }

            expense.Total += newTotalCost;

            if (createdNewExpense)
                _repository.Expenses.CreateExpense(expense);
            else
                _repository.Expenses.UpdateExpense(expense);

            _repository.Save();

            return NoContent();
        }

        // PUT: /api/Finance/UpdateProduct/123
        [HttpPut("UpdateProduct/{id}")]
        public IActionResult UpdateProduct([FromBody] ProductForCreationDto product, Guid id)
        {
            Product productEntity = _repository.Product.GetProductById(id);

            if (productEntity == null)
                return NotFound("Product not found");

            _mapper.Map(product, productEntity);
            _repository.Product.UpdateProduct(productEntity);
            _repository.Save();
            return NoContent();
        }

        //DELETE: /api/Finance/DeleteProduct/123
        [HttpDelete("DeleteProduct/{id}")]
        public IActionResult DeleteProduct(Guid id)
        {
            Product productEntity = _repository.Product.GetProductById(id);

            if (productEntity == null)
                return NotFound("Product not found");

            Expenses expenseEntity = _repository.Expenses.GetExpenseById(productEntity.ExpensesId);
            expenseEntity.Total -= productEntity.Cost;

            _repository.Expenses.UpdateExpense(expenseEntity);
            _repository.Product.DeleteProduct(productEntity);

            _repository.Save();

            return NoContent();
        }

        //GET: /api/Finance/GetUserData/123
        [HttpGet("GetUserData/{userId}")]
        public IActionResult GetUserData(Guid userId)
        {
            IEnumerable<Expenses> expensesEntity = _repository.Expenses.GetUserExpensesWithDetails(userId);
            IEnumerable<ExpensesDto> expensesResult = (IEnumerable<ExpensesDto>)_mapper.Map<ExpensesDto>(expensesEntity);
            return Ok(expensesResult);
        }

        //GET /api/Finance/GetTodayUserData/123
        [HttpGet("GetTodayUserData/{userId}")]
        public IActionResult GetTodayUserData(Guid userId)
        {
            long timestamp = Time.GetTodayTimestamp();
            Expenses expensesEntity = _repository.Expenses.GetUserExpenseByDateWithDetails(userId, timestamp);
            ExpensesDto expensesResult = _mapper.Map<ExpensesDto>(expensesEntity);
            return Ok(expensesResult);
        }


    }
}
