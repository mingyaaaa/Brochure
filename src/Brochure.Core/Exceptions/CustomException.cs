using System;
using System.Collections.Generic;
using System.Text;

namespace Brochure.Core.Exceptions
{
    public class ParameterException : Exception
    {
        private static string msg = "参数异常";

        public ParameterException() : base(msg)
        {
        }

        public ParameterException(Exception innerException) : base(msg, innerException)
        {
        }
    }
}