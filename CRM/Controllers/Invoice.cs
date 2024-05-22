using Microsoft.AspNetCore.Mvc;
using SuperAdmin.Areas.Identity.Data;
using SuperAdmin.Models;
using System.Linq;

namespace SuperAdmin.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly SuperAdminContext _context; // Replace YourDbContext with your actual DbContext class name

        public InvoiceController(SuperAdminContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var invoices = _context.Invoice.ToList(); // Retrieve all invoices from the database
            return View(invoices);
        }
        [HttpPost]
        public IActionResult PurchaseBalance(Balance balance, int Id)
        {
            var order = _context.Invoice.FirstOrDefault(o => o.InvoiceId == Id);

            if (order != null)
            {
                // Update the status of the order to "approved"
                order.Status = "approved";

                // Save changes to the database
                _context.SaveChanges();

                // Remaining code for balance purchase...

                // Return success response
                return Json(new { success = true, message = "Balance purchased successfully and order status updated." });
            }
            if (ModelState.IsValid)
            {
                var latestBalance = _context.Balance
                    .Where(b => b.UserId == balance.UserId)
                    .OrderByDescending(b => b.Date)
                    .FirstOrDefault();

                double previousBalance = 0;

                if (latestBalance != null)
                {
                    previousBalance = latestBalance.BalanceAmount;
                }

                var totalBalance = previousBalance + (balance.amount ?? 0); // Convert nullable double? to double

                string narration = "Balance purchased";
                string Recharje = "Recharje";
                string Recharjee = "Debit";

                var newBalance = new Balance
                {
                    BalanceAmount = totalBalance,
                    AccountNumber = balance.AccountNumber,
                    UserId = balance.UserId,
                    previousamount = previousBalance,
                    amount = 0,
                    narration = narration,
                    packagename = Recharje,
                    transactiontype = Recharjee,
                    Date = DateTime.Now
                };

                _context.Balance.Add(newBalance);
                _context.SaveChanges();

                return Json(new { success = true, message = "Balance purchased successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to purchase balance. Please check your inputs." });
            }
        }


    }
}
