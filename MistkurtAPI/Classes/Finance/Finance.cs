using AutoMapper;
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using MistkurtAPI.Classes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MistkurtAPI
{
    public class Finance
    {
        private readonly IRepositoryWrapper _repository;
        private readonly Guid _userId;
        private readonly IMapper _mapper;

        public Finance(IRepositoryWrapper repositiory, IMapper mapper, Guid userId)
        {
            _repository = repositiory;
            _userId = userId;
            _mapper = mapper;
        }


        #region Public
        public ExpensesDashboardDto GetDailyData()
        {
            IEnumerable < Expenses > expenses = GetExpensesForCurrentMonth();
            ExpensesDashboardDto expensesResult = new();
            expensesResult.Expenses = _mapper.Map<ExpensesDto[]>(expenses);

            IEnumerable<Product> all_products = GetProductsFromExpenses(expenses);

            return ParseExpensesReport(all_products, expensesResult, expenses);
        }

        #endregion

        #region Private
        private IEnumerable<Expenses> GetExpensesForCurrentMonth()
        {
            long startOfMonth = Time.GetStartOfMonthTimestamp();
            long today = Time.GetTodayTimestamp();
            return _repository.Expenses.GetUserExpensesByRangeWithDetails(_userId, startOfMonth, today);
        }

        private IEnumerable<Product> GetProductsFromExpenses(IEnumerable<Expenses> expenses)
        {
            return expenses.SelectMany(elem => elem.Products);
        }

        private ExpensesDashboardDto ParseExpensesReport(IEnumerable<Product> products, ExpensesDashboardDto expensesResult, IEnumerable<Expenses> expenses)
        {
            expensesResult.HighestType = GetHighestProductType(products);
            expensesResult.LowestType = GetLowestProductType(products);
            expensesResult.HighestTag = GetHighestProductTag(products);
            expensesResult.LowestTag = GetLowestProductTag(products);

            expensesResult.Total = expenses.Select(i => i.Total).Max();

            return expensesResult;
        }

        private ProductTotals GetHighestProductType(IEnumerable<Product> products)
        {
            // proper group and select by name;
            IEnumerable<ProductTotals> query = from product in products
                        group product by product.Type into productTypes
                        select new ProductTotals
                        {
                            Key = productTypes.Key,
                            Total = (from product2 in productTypes
                                     select product2.Cost).Max()
                        };
            return query.FirstOrDefault();

        }


        private ProductTotals GetHighestProductTag(IEnumerable<Product> products)
        {
            IEnumerable<ProductTotals> query = from product in products
                        group product by product.Tag into productTags
                        select new ProductTotals
                        {
                            Key = productTags.Key,
                            Total = (from product2 in productTags
                                    select product2.Cost).Max()
                        };
            return query.FirstOrDefault();
        }

        private ProductTotals GetLowestProductType(IEnumerable<Product> products)
        {
            IEnumerable<ProductTotals> query = from product in products
                        group product by product.Type into productTypes
                        select new ProductTotals
                        {
                            Key = productTypes.Key,
                            Total = (from product2 in productTypes
                                     select product2.Cost).Min()
                        };
            return query.FirstOrDefault();
        }

        private ProductTotals GetLowestProductTag(IEnumerable<Product> products)
        {
            IEnumerable<ProductTotals> query = from product in products
                        group product by product.Type into productTags
                        select new ProductTotals
                        {
                            Key = productTags.Key,
                            Total = (from product2 in productTags
                                     select product2.Cost).Min()
                        };
            return query.FirstOrDefault();
        }

        #endregion


    }
}
