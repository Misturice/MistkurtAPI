using Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistkurtAPI
{
    public class ValidateUserExistsAttribute : IActionFilter
    {

        private readonly IRepositoryWrapper _repository;
        private readonly string _key;

        public ValidateUserExistsAttribute(IRepositoryWrapper repository, string key)
        {
            _repository = repository;
            _key = key;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            dynamic value = null;

            if (context.ActionArguments.ContainsKey(_key))
            {
                value = context.ActionArguments[_key];
            }
            else
            {
                context.Result = new BadRequestObjectResult("Bad parameters");
                return;
            }

            User user = null;

            if (_key == "email")
                user = _repository.User.GetUserByEmail(value);
            else
                user = _repository.User.GetUserById(value);

            if(user == null)
            {
                context.Result = new NotFoundResult();
                return;
            }
            else
            {
                context.HttpContext.Items.Add("user", user);
            }

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
