using Microsoft.AspNetCore.Mvc.Filters;

namespace FirstProject.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Console.WriteLine(context.Exception);
        }
    }
}
