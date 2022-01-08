using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brochure.Server.Main.Controllers
{
    /// <summary>
    /// The test v2 controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v2")]
    public class TestV2Controller : ControllerBase
    {
    }
}
