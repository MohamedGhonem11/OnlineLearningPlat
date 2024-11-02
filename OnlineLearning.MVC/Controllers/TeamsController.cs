using Microsoft.AspNetCore.Mvc;

namespace OnlineLearning.MVC.Controllers
{
    public class TeamsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
