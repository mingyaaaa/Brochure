using Brochure.Abstract.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Brochure.Core.Server.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static IActionResult JsonData<T>(this ControllerBase controllerBase, T data) 
        {
            var result = new Result<T>(data);
            return new JsonResult(result) { StatusCode=200};
        }

        public static IActionResult JsonError<T>(this ControllerBase controllerBase, T data, int code, string msg) 
        {
            var result = new Result<T>(data,code,msg);
            return new JsonResult(result) ;
        }

        public static IActionResult JsonError(this ControllerBase controllerBase, int code, string msg)
        {
            var result = new Result(code, msg);
            return new JsonResult(result);
        }

        public static IActionResult JsonException(this ControllerBase controllerBase, Exception exception) 
        {
            var result = new Result(500, exception.Message+exception.StackTrace);
            return new JsonResult(result);
        }
    }
}
