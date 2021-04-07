using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.Server.Core
{
    /// <summary>
    /// The rsp error.
    /// </summary>
    public class RspError
    {
        /// <summary>
        /// Jsons the error.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="jsonObject">The json object.</param>
        /// <returns>A JsonResult.</returns>
        public JsonResult JsonError(object jsonObject, int code = 500)
        {
            var r = new JsonResult(jsonObject);
            r.StatusCode = code;
            return r;
        }

        /// <summary>
        /// Contents the error.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="content">The content.</param>
        /// <returns>A ContentResult.</returns>
        public ContentResult ContentError(string content, int code = 500)
        {
            var r = new ContentResult();
            r.Content = content;
            r.StatusCode = code;
            return r;
        }
    }
}
