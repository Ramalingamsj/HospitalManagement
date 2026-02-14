using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class ReceptionistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
