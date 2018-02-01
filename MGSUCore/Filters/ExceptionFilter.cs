using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver;
using Common.Exceptions;

namespace MGSUCore.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            if(exception.GetType() == typeof(PolicyException))
            {
                context.Result = new BadRequestObjectResult(exception.Message);
            }
            else if(exception.GetType() == typeof(MongoWriteException))
            {
                var mongoWriteEx = exception as MongoWriteException;
                context.Result = new BadRequestObjectResult(mongoWriteEx.WriteError.Category.ToString());
            }
        }
    }
}
