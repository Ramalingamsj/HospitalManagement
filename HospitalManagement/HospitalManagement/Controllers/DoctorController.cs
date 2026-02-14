using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
