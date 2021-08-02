using MistkurtAPI.Classes.Databases;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MistkurtAPI.Classes.Common;
using System.Linq;

namespace MistkurtAPI.Classes.Finance
{
    public class Expense
    {
        private IEnumerable<Product> _productsList;
        private readonly Postgres _postgres;
        private Models.Expenses _expense;
        private Guid _userID;

        public Models.Expenses ExpenseInfo { get => _expense; set => _expense = value; }
        public Postgres Postgres => _postgres;

        public IEnumerable<Product> ProductsList { get => _productsList; set => _productsList = value; }

        public Expense(Postgres context, int expenseID, Guid userID)
        {
            _postgres = context;
            _userID = userID;
            ProductsList = new List<Product>();
            if (expenseID == -1)
                CheckIfExpenseExists();
            else
                FindExpense(expenseID);
        }

        public async Task AddProducts(IEnumerable<Models.Product> products)
        {
            float newTotalCost = 0f;
            foreach(Models.Product product in products)
            {
                product.ExpensesID = ExpenseInfo.ID;
                ProductsList.Append(new Product(product));
                Postgres.AddProduct(product);
                newTotalCost += product.Cost;
            }
            ExpenseInfo.Total += newTotalCost;
            await Postgres.SaveDatabase();
        }

        private async Task FindProductsAsync()
        {
            IEnumerable<Models.Product> products = await Postgres.FindProductsByExpenseAsync(ExpenseInfo.ID);
            foreach(Models.Product product in products)
            {
                ProductsList.Append(new Product(product));
            }
        }

        private void CheckIfExpenseExists()
        {
            long startOfDayTimestamp = Time.GetTodayTimestamp();
            Models.Expenses expense = Postgres.GetUserExpenseForGivenDay(_userID, startOfDayTimestamp);
            if (expense == null)
                CreateNewExpense();
            else
                ExpenseInfo = expense;
        }

        private void CreateNewExpense()
        {
            Models.Expenses expense = new();
            expense.Date = Time.GetTodayTimestamp();
            expense.Total = 0.0f;
            expense.UserID = _userID;
            ExpenseInfo = expense;
            Postgres.AddNewExpense(expense);
        }

        private async void FindExpense(int id)
        {
            ExpenseInfo = await Postgres.GetExpenseAsync(id);
            await FindProductsAsync();
        }
    }
}