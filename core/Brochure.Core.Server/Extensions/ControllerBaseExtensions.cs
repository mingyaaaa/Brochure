using Brochure.Abstract.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brochure.Core.Server.Extensions
{
    public static class BaseControllerExtensions
    {
        public static IActionResult Ok<T>(this ControllerBase controllerBase, T data) 
        {
            var result = new Result<T>(data);
            return new JsonResult(result);
        }

        public static IActionResult Error<T>(this ControllerBase controllerBase, T data, int code, string msg) 
        {
            var result = new Result<T>(data,code,msg);
            return new JsonResult(result);
        }

        public static IActionResult Error(this ControllerBase controllerBase, int code, string msg)
        {
            var result = new Result(code, msg);
            return new JsonResult(result);
        }
    }
}
