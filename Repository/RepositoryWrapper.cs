using Contracts;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        // TODO finish this, add find by key method;
        private RepositoryContext _repoContext;
        private IUserRepository _user;
        private IExpensesRepository _expenses;
        private IProductRepository _product;


        public IUserRepository User
        {
            get
            {
                if(_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }

                return _user;
            }
        }

        public IExpensesRepository Expenses
        {
            get
            {
                if (_expenses == null)
                {
                    _expenses = new ExpensesRepository(_repoContext);
                }

                return _expenses;
            }
        }
        public IProductRepository Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductRepository(_repoContext);
                }

                return _product;
            }
        }

        public RepositoryWrapper(RepositoryContext context)
        {
            _repoContext = context;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
