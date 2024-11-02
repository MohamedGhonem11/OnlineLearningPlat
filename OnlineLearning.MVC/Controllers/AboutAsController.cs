using Microsoft.AspNetCore.Mvc;

namespace OnlineLearning.MVC.Controllers
{
    public class AboutAsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
