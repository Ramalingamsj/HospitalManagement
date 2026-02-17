using HospitalManagement.Models;
using HospitalManagement.Repository;
using HospitalManagement.Service;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace HospitalManagement.Controllers
{
    public class PharmacyController : Controller
    {
        private readonly IPharmacyService _service;

        public PharmacyController(IPharmacyService service)
        {
            _service = service;
        }
        public IActionResult DownloadBill(int id)
        {
            var patientName = _service.GetPatientNameService(id) ?? "Unknown";
            var total = _service.GetBillAmountService(id);   // 👈 ADD THIS

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
                        col.Item().Text($"Total Amount : ₹ {total}")  // 👈 ADD
                            .FontSize(16).Bold().FontColor(Colors.Green.Darken2);
                    });
                });
            }).GeneratePdf();

            string safeName = string.Concat(patientName.Split(Path.GetInvalidFileNameChars()));

            return File(pdf, "application/pdf", $"{safeName}_Bill.pdf");
        }
        public IActionResult PendingList()
        {
            var data = _service.GetPendingMedicinesService();

            ViewBag.GeneratedBills = data
                .Select(x => x.ConsultationId)
                .Distinct()
                .ToDictionary(id => id, id => _service.BillExistsService(id));

            return View(data);
        }

        public IActionResult Issue(int id)
        {
            var cookie = Request.Cookies["UserId"];

            if (cookie == null)
                return RedirectToAction("Login", "Auth");

            int pharmacyUserId = Convert.ToInt32(cookie);

            try
            {
                _service.IssueMedicineService(id, pharmacyUserId);

                TempData["Success"] = "Medicine issued successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("PendingList");
        }
        public IActionResult Index()
        {
            return View(_service.GetMedicinesService());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Medicine m)
        {
            _service.AddMedicineService(m);
            TempData["Success"] = "Medicine Added Successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateStock(int id, int stock)
        {
            _service.UpdateStockService(id, stock);
            TempData["Success"] = "Stock Updated";
            return RedirectToAction("Index");
        }
        public IActionResult GenerateBill(int id)
        {
            var result = _service.GenerateMedicineBillService(id);

            if (result == "SUCCESS")
                TempData["Success"] = "Bill generated successfully!";
            else
                TempData["Error"] = result;

            return RedirectToAction("PendingList");
        }
        public IActionResult Bills()
        {
            var bills = _service.GetBillsService();
            return View(bills);
        }
    }
}
