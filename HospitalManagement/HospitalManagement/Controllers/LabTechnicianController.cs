using HospitalManagement.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace HospitalManagement.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LabTechnicianController : Controller
    {
        private readonly ILabTechnicianService _service;

        public LabTechnicianController(ILabTechnicianService service)
        {
            _service = service;
        }

        public IActionResult Pending()
        {
            return View(_service.GetPendingTests());
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            context.HttpContext.Response.Headers["Expires"] = "0";

            var userId = HttpContext.Session.GetString("UserId");
            var roleId = HttpContext.Session.GetString("RoleId");

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleId))
            {
                context.Result = RedirectToAction("Login", "Logins");
                return;
            }

            base.OnActionExecuting(context);
        }
        [HttpPost]
        public IActionResult UpdateResult(int id, string result)
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Logins");
            }

            int userId = Convert.ToInt32(userIdString);

            _service.UpdateResult(id, result, userId);

            return RedirectToAction("Pending");
        }
        [HttpPost]
        public IActionResult GenerateBill(int id)
        {
            try
            {
                _service.GenerateLabBillService(id);
                TempData["Success"] = "Bill Generated Successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Pending");
        }
        public IActionResult Bills()
        {
            return View(_service.GetBills());
        }

        // ===================== BILL PDF =====================
        public IActionResult PrintBill(int id)
        {
            var bill = _service.GetBills().First(x => x.BillId == id);

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("Hospital Lab Bill")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Bill ID: {bill.BillId}");
                        col.Item().Text($"Patient: {bill.PatientName}");
                        col.Item().Text($"Date: {bill.CreatedAt:dd MMM yyyy}");

                        col.Item().PaddingVertical(10);

                        col.Item().Text($"Total Amount: ₹ {bill.TotalAmount}")
                            .FontSize(16)
                            .Bold();
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("Thank you for visiting")
                        .FontSize(10);
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Bill_{id}.pdf");
        }
    }
}
