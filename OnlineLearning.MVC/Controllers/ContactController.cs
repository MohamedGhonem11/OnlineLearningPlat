using Microsoft.AspNetCore.Mvc;

namespace OnlineLearning.MVC.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
