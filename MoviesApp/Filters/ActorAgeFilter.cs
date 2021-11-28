using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MoviesApp.Filters
{
    public class ActorAgeFilter: Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var birthdayDate = DateTime.Parse(context.HttpContext.Request.Form["BirthdayDate"]);
            var dateMoreThan = DateTime.Now.AddYears(-7);
            var dateLessThan = DateTime.Now.AddYears(-99);
            if (birthdayDate.CompareTo(dateLessThan) <= 0 || birthdayDate.CompareTo(dateMoreThan) >= 0)
            {
                context.Result = new BadRequestResult();
                Console.WriteLine($"User {context.HttpContext.Request.Form["Name"]} {context.HttpContext.Request.Form["LastName"]} tried to enter the wrong date of birth.");
            }
        }
    }
}