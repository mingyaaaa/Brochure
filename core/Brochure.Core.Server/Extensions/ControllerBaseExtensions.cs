using Brochure.Abstract.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Brochure.Core.Server.Extensions
{
    /// <summary>
    /// The controller base extensions.
    /// </summary>
    public static class ControllerBaseExtensions
    {
        /// <summary>
        /// Jsons the data.
        /// </summary>
        /// <param name="controllerBase">The controller base.</param>
        /// <param name="data">The data.</param>
        /// <returns>An IActionResult.</returns>
        public static IActionResult JsonData<T>(this ControllerBase controllerBase, T data)
        {
            var result = new Result<T>(data);
            return new JsonResult(result) { StatusCode = 200 };
        }

        /// <summary>
        /// Jsons the error.
        /// </summary>
        /// <param name="controllerBase">The controller base.</param>
        /// <param name="data">The data.</param>
        /// <param name="code">The code.</param>
        /// <param name="msg">The msg.</param>
        /// <returns>An IActionResult.</returns>
        public static IActionResult JsonError<T>(this ControllerBase controllerBase, T data, int code, string msg)
        {
            var result = new Result<T>(data, code, msg);
            return new JsonResult(result);
        }

        /// <summary>
        /// Jsons the error.
        /// </summary>
        /// <param name="controllerBase">The controller base.</param>
        /// <param name="code">The code.</param>
        /// <param name="msg">The msg.</param>
        /// <returns>An IActionResult.</returns>
        public static IActionResult JsonError(this ControllerBase controllerBase, int code, string msg)
        {
            var result = new Result(code, msg);
            return new JsonResult(result);
        }

        /// <summary>
        /// Jsons the exception.
        /// </summary>
        /// <param name="controllerBase">The controller base.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>An IActionResult.</returns>
        public static IActionResult JsonException(this ControllerBase controllerBase, Exception exception)
        {
            var result = new Result(500, exception.Message + exception.StackTrace);
            return new JsonResult(result);
        }
    }
}