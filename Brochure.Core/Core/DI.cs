using Brochure.Interface;
using System;

namespace Brochure.Core
{
    public static class DI
    {
        public static IDIServiceManager ServerManager { get; set; }
        public static IServiceProvider ServiceProvider { get; set; }
    }
}
