using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class PharmacyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
