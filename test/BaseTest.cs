using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace test
{
    public abstract class BaseTest
    {
        protected ITestOutputHelper OutputHelper;
        public BaseTest(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }
    }
}
