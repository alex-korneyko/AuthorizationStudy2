using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Orders.Api.Controllers
{
    [Route("[controller]")]
    public class SiteController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("[action]")]
        [Authorize]
        public string Secret()
        {
            return "Secret string from orders API";
        }
    }
}