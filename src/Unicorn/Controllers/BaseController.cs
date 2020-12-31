using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Controllers
{
    [Route("api/unicorn/{controller}")]
    public abstract class BaseController: Controller
    {
    }
}
