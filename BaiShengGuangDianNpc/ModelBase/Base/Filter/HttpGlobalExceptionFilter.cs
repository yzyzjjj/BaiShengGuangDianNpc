using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ModelBase.Base.Logger;
using ModelBase.Base.ServerConfig.Enum;
using ModelBase.Models.Result;

namespace ModelBase.Base.Filter
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;
        public HttpGlobalExceptionFilter(IHostingEnvironment env)
        {
            _env = env;
        }
        public void OnException(ExceptionContext context)
        {
            var err = context.Exception;
            Log.ErrorFormat("GlobalException:{0}\n{1}", err.Message, err.StackTrace);

            var result = new Result
            {
                errno = Error.ExceptionHappen
            };

            if (_env.IsDevelopment())
            {
                //堆栈信息
                result.errmsg = context.Exception.StackTrace;
            }
            //返回异常数据
            context.Result = new BadRequestObjectResult(result);
        }
    }
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
