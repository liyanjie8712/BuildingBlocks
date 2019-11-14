using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Liyanjie.AspNetCore.Mvc.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionLoggingFilterAttribute : ExceptionFilterAttribute
    {
        readonly ILogger logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public ExceptionLoggingFilterAttribute(ILogger<ExceptionLoggingFilterAttribute> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            logger.LogError(default, context.Exception, "捕获到未知异常");
            base.OnException(context);
        }
    }
}
