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
        public IActionResult AddProduct([FromBody] ProductForCreationDto product, Guid userId, int date)
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



            float newTotalCost = (float)product.Cost;
            Product productEntity = _mapper.Map<Product>(product);
            productEntity.ExpensesId = expense.Id;
            expense.Products.Add(productEntity);
            

            expense.Total += newTotalCost;

            if (createdNewExpense)
                _repository.Expenses.CreateExpense(expense);
            else
                _repository.Expenses.UpdateExpense(expense);

            _repository.Save();

            return Ok(productEntity.Id);
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

        //GET /api/Finance/GetDailyUserData/123
        [HttpGet("GetDailyUserData/{userId}/{startDate}/{endDate}")]
        public IActionResult GetDailyUserData(Guid userId, long startDate, long endDate)
        {
            Finance finance = new(_repository, _mapper, userId);
            ExpensesDashboardDto expensesResult = finance.GetDailyData(startDate, endDate);
            return Ok(expensesResult);
        }

        //GET /api/Finance/GetExpenseDetails/123/321
        [HttpGet("GetExpenseDetails/{expenseId}")]
        public IActionResult GetExpenseDetails(Guid expenseId)
        {
            Finance finance = new(_repository, _mapper);
            ExpenseDetailsDto expenseResult = finance.GetExpenseDetails(expenseId);
            return Ok(expenseResult);
        }

        //GET: /api/Finance/GetRangeUserData/123/123/132
        [HttpGet("GeRangetUserData/{userId}/{startDate}/{endDate}")]
        public IActionResult GetRangeUserData(Guid userId, long startDate, long endDate)
        {
            IEnumerable<Expenses> expensesEntity = _repository.Expenses.GetUserExpensesByRangeWithDetails(userId, startDate, endDate);
            IEnumerable<ExpensesDto> expensesResult = (IEnumerable<ExpensesDto>)_mapper.Map<ExpensesDto>(expensesEntity);
            return Ok(expensesResult);
        }

    }
}
