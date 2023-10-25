using Blog_API.Modules.Blog;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog_API.Filters
{
    public class ModelStateValidatorForSpeceficController : IAsyncActionFilter, IOrderedFilter
    {
        private readonly ILogger<ModelStateValidatorForSpeceficController> _logger;
        private readonly string _key;
        private readonly string _value;
        public int Order { get; set; }

        public ModelStateValidatorForSpeceficController(ILogger<ModelStateValidatorForSpeceficController> logger , string Key , string Value , int order)
        {
            _logger = logger;
            _key = Key;
            _value = Value;
            Order = order;
        }
    
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Before Execution Of Action Method
            //Contorllelr Name is example
            if (context.Controller is BlogController blogController)
            {
                if(blogController.ModelState.IsValid == false)
                {
                    //context.ActionArguments["argumentName"]  //Access to Argument of passing to Action method
                    //context.Result = "Return"   // **short-circute
                    //**When we short-circute we should not use next() beacuse next delegate calling the next subciquent filter 
                }
                await next();
            }
            await next();
            //After Execution Of Action Method
        }
    }
}

//Order of execution filters(OnActionExecuting Or Before next) => Global -> Controller -> Method  ASC
//Order of execution filters(OnActionExecuted Or After next) => Method -> Controller -> Global   DESC
