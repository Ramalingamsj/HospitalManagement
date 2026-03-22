using HospitalManagement.Models;
using HospitalManagement.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace HospitalManagement.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class PharmacyController : Controller
    {
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
        private readonly IPharmacyService _service;

        public PharmacyController(IPharmacyService service)
        {
            _service = service;
        }

        // ================= DOWNLOAD BILL =================
        public IActionResult DownloadBill(int id)
        {
            var patientName = _service.GetPatientNameService(id) ?? "Unknown";
            var total = _service.GetBillAmountService(id);

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);

                    page.Content().Column(col =>
                    {
                        col.Item().Text("HOSPITAL BILL").FontSize(20).Bold();
                        col.Item().Text($"Patient : {patientName}");
                        col.Item().Text($"Consultation ID : {id}");
                        col.Item().Text($"Date : {DateTime.Now:dd-MM-yyyy}");
                        col.Item().Text($"Total Amount : ₹ {total}")
                            .FontSize(16).Bold().FontColor(Colors.Green.Darken2);
                    });
                });
            }).GeneratePdf();

            string safeName = string.Concat(patientName.Split(Path.GetInvalidFileNameChars()));

            return File(pdf, "application/pdf", $"{safeName}_Bill.pdf");
        }

        // ================= PENDING PAGE =================
        public IActionResult PendingList()
        {
            var data = _service.GetPendingMedicinesService();

            ViewBag.GeneratedBills = data
                .Select(x => x.ConsultationId)
                .Distinct()
                .ToDictionary(id => id, id => _service.BillExistsService(id));

            return View(data);
        }

        // ================= ISSUE MEDICINE =================
        public IActionResult Issue(int id)
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Logins");
            }

            int userId = Convert.ToInt32(userIdString);

            if (userId == null)
                return RedirectToAction("Logins", "Login");

            int pharmacyUserId = Convert.ToInt32(userId);

            try
            {
                _service.IssueMedicineService(id, pharmacyUserId);
                TempData["Success"] = "Medicine issued successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(PendingList));
        }

        // ================= MEDICINE LIST =================
        public IActionResult Index()
        {
            return View(_service.GetMedicinesService());
        }

        // ================= ADD MEDICINE =================
        [HttpPost]
        public IActionResult Create(Medicine m)
        {
            _service.AddMedicineService(m);
            TempData["Success"] = "Medicine added successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ================= UPDATE STOCK =================
        [HttpPost]
        public IActionResult UpdateStock(int id, int stock)
        {
            _service.UpdateStockService(id, stock);
            TempData["Success"] = "Stock updated!";
            return RedirectToAction(nameof(Index));
        }

        // ================= GENERATE BILL =================
        public IActionResult GenerateBill(int id)
        {
            var result = _service.GenerateMedicineBillService(id);

            if (result == "SUCCESS")
                TempData["Success"] = "Bill generated successfully!";
            else
                TempData["Error"] = result;

            return RedirectToAction(nameof(PendingList));
        }

        // ================= BILL LIST =================
        public IActionResult Bills()
        {
            return View(_service.GetBillsService());
        }
    }
}