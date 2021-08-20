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

        public Finance(IRepositoryWrapper repositiory, IMapper mapper, Guid userId = default)
        {
            _repository = repositiory;
            if(userId != Guid.Empty)
                _userId = userId;
            _mapper = mapper;
        }


        #region Public
        public ExpensesSummaryDto GetDailyData(long startDate, long endDate)
        {
            IEnumerable < Expenses > expenses = GetExpensesDailyByDate(startDate, endDate);

            ExpensesSummaryDto expensesResult = new();

            expensesResult.Expenses = _mapper.Map<ExpensesDto[]>(expenses);

            IEnumerable<Product> all_products = GetProductsFromExpenses(expenses);

            (expensesResult.HighestType, expensesResult.LowestType, expensesResult.HighestTag, expensesResult.LowestTag) = GetProductsReport(all_products);
            
            expensesResult.Total = expenses.Select(elem => elem.Total).Sum();

            return expensesResult;
        }

        public ExpensesSummaryDto GetExpenseDetails(Guid expenseId)
        {
            Expenses expense = _repository.Expenses.GetExpenseWithDetailsById(expenseId);

            ExpensesSummaryDto expensesResult = new();

            expensesResult.ExpenseDetails = _mapper.Map<ExpensesDto>(expense);

            //expensesResult.Products = _mapper.Map<ProductDto[]>(expense.Products);

            IEnumerable<Product> all_products = expense.Products;

            expensesResult.Total = expense.Total;

            (expensesResult.HighestType, expensesResult.LowestType, expensesResult.HighestTag, expensesResult.LowestTag) = GetProductsReport(all_products);
            
            return expensesResult;
        }



        #endregion

        #region Private

        private IEnumerable<Expenses> GetExpensesDailyByDate(long startDate, long endDate)
        {
            return _repository.Expenses.GetUserExpensesByRangeWithDetails(_userId, startDate, endDate);
        }

        private IEnumerable<Product> GetProductsFromExpenses(IEnumerable<Expenses> expenses)
        {
            return expenses.SelectMany(elem => elem.Products);
        }

        private (ProductTotals, ProductTotals, ProductTotals, ProductTotals) GetProductsReport(IEnumerable<Product> products)
        {
            return (GetHighestProductType(products), GetLowestProductType(products), GetHighestProductTag(products), GetLowestProductTag(products));
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
