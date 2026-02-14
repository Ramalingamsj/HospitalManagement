using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class LabTechnicianController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
