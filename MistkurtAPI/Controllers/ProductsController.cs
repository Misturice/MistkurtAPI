﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MistkurtAPI;

namespace MistkurtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public ProductsController(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        // POST: /api/Products/5/000000000
        [HttpPost("{userId}/{date}")]
        public IActionResult AddProducts([FromBody] IEnumerable<ProductForCreationDto> products, Guid userId, int date)
        {
            Expenses expense = null;
            bool createdNewExpense = false;
            if(!_repository.Expenses.ExpenseExists(userId, date))
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

        // PUT: /api/Products/123
        [HttpPut("{id}")]
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

        //DELETE: /api/Products/123
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(Guid id)
        {
            Product productEntity = _repository.Product.GetProductById(id);

            if (productEntity == null)
                return NotFound("Product not found");
            _repository.Product.DeleteProduct(productEntity);
            _repository.Save();

            return NoContent();
        }
    }
}
