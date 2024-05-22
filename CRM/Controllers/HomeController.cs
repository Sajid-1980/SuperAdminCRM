using SuperAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using SuperAdmin.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using SuperAdmin.Models.Masterleads;
using OfficeOpenXml;
using System.Globalization;
using System.ComponentModel;


namespace SuperAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<SuperAdminUser> _signInManager; // Declare SignInManager
        private readonly UserManager<SuperAdminUser> _userManager; // Declaring UserManager might be necessary if you plan to use it elsewhere in the class
        private readonly SuperAdminContext _context;

        public HomeController(ILogger<HomeController> logger, SuperAdminContext context, SignInManager<SuperAdminUser> signInManager, UserManager<SuperAdminUser> userManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;


        }
        public IActionResult Mask()
        {
            List<MaskOrder> orders = _context.MaskOrder.ToList();
            return View(orders);
        }
        // Action method to update approved option
        [HttpPost]
        public IActionResult UpdateApprovedOption(int orderId, string approvedOption, string maskId, string smsUser, string smsPassword)
        {
            var order = _context.MaskOrder.Find(orderId);
            if (order != null)
            {
                order.Approvedoption = approvedOption;
                order.SmsMaskId = maskId;
                order.SmsUser = smsUser;
                order.SmsPassword = smsPassword;
                _context.SaveChanges();
                return RedirectToAction("Mask");
            }
            return NotFound();
        }

        public IActionResult Index()
        {
            var clients = _context.Register.ToList();
            var clientCount = clients?.Count ?? 0; // Using null-conditional operator and null-coalescing operator
            ViewBag.ClientCount = clientCount; // Pass the count to ViewBag


            var Leads = _context.Masterleads.ToList();
            var LeadsCount = Leads?.Count ?? 0; // Using null-conditional operator and null-coalescing operator
            ViewBag.Leads = LeadsCount;

            var Services = _context.Services.ToList();
            var ServiceCount = Services?.Count ?? 0; // Using null-conditional operator and null-coalescing operator
            ViewBag.ServiceCount = ServiceCount;


            var Subscriptions = _context.SubmitOrderModel.ToList();
            var SubscriptionsCount = Subscriptions?.Count ?? 0; // Using null-conditional operator and null-coalescing operator
            ViewBag.SubscriptionsCount = SubscriptionsCount;

            var MaskOrder = _context.MaskOrder.ToList();
            var MaskOrderCount = MaskOrder?.Count ?? 0; // Using null-conditional operator and null-coalescing operator
            ViewBag.MaskOrderCount = MaskOrderCount;

            var ClientOrder = _context.Orders.ToList();
            var ClientOrderCount = ClientOrder?.Count ?? 0; // Using null-conditional operator and null-coalescing operator
            ViewBag.ClientOrderCount = ClientOrderCount;

            var AdminOrder = _context.SubmitOrderModel.ToList();
            var AdminOrderCount = AdminOrder?.Count ?? 0; // Using null-conditional operator and null-coalescing operator
            ViewBag.AdminOrderCount = AdminOrderCount;
            //var Invoice = _context.Invoice.ToList();
            //var InvoiceCount = AdminOrder?.Count ?? 0; // Using null-conditional operator and null-coalescing operator
            //ViewBag.InvoiceCount = InvoiceCount;

            return View();
        }
        public IActionResult clients()
        {
            var clients = _context.Register.ToList(); // Fetch all clients from the database
            return View(clients);

        }
        [HttpPost]
        public IActionResult Suspend(Guid clientId)
        {
            try
            {
                var client = _context.Register.FirstOrDefault(c => c.ClientId == clientId);
                if (client != null)
                {
                    // Set IsActive to false or perform your suspend logic
                    client.IsActive = false;
                    _context.SaveChanges();
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetClientDetails(Guid clientId)
        {
            try
            {
                var client = _context.Register.FirstOrDefault(c => c.ClientId == clientId);
                if (client != null)
                {
                    // Return client details as JSON
                    return Json(new
                    {
                        clientId = client.ClientId,
                        firstName = client.FirstName,
                        lastName = client.LastName,
                        company = client.Company,
                        phone1 = client.Phone1,
                        phone2 = client.Phone2,
                        email = client.Email,
                        password = client.Password,

                    });
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public IActionResult SaveClientDetails([FromBody] Register editedClient)
        {
            try
            {
                var client = _context.Register.FirstOrDefault(c => c.ClientId == editedClient.ClientId);
                if (client != null)
                {
                    // Update other properties as needed
                    client.FirstName = editedClient.FirstName;
                    client.LastName = editedClient.LastName;
                    client.Company = editedClient.Company;
                    client.Phone1 = editedClient.Phone1;
                    client.Phone2 = editedClient.Phone2;
                    client.Email = editedClient.Email;
                    client.Password = editedClient.Password;


                    _context.SaveChanges();
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        public IActionResult LeadForm()
        {
            var newLead = new Masterleads();
            return View(newLead);
        }

        [HttpPost]
        public IActionResult AddMasterleads(Masterleads newLead)
        {
            if (ModelState.IsValid)
            {
                newLead.CreatedDate = DateTime.Now; // Set the creation date
                _context.Masterleads.Add(newLead);
                _context.SaveChanges();
                // You may add a success message or redirect to another page
                return RedirectToAction("AllLeads");
            }

            // If the model is not valid, return the form with validation errors
            return View(newLead);
        }

        [HttpGet]
        public IActionResult AllLeads()
        {
            var allLeads = _context.Masterleads.ToList(); // Assuming Masterleads is your DbSet for leads
            return View(allLeads);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult BulkUploadLeads()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitOrder(int orderId, string userId, string service, int quantity, string Message, SubmitOrderModel submitOrderModel)
        {
            try
            {
                // Check if the model state is valid
                if (ModelState.IsValid)
                {
                    // Retrieve the existing order based on clientOrderId
                    var existingOrder = _context.Orders.SingleOrDefault(o => o.OrderId == orderId);

                    if (existingOrder != null)
                    {
                        // Update the existing order properties
                        existingOrder.AverageTime = submitOrderModel.ExpectedTime;
                        existingOrder.OrderStatus = "Inprogress"; // Assuming 't' is "Inprogress"
                        existingOrder.PreviousQuantity = submitOrderModel.PreviousQuantity;
                        existingOrder.Message = Message;
                        // Add other properties as needed

                        // Save the changes to the existing order
                        _context.SaveChanges();
                    }
                    else
                    {
                        // If no matching order is found, create a new order
                        var order = new Order
                        {
                            // Set other order properties
                            AverageTime = submitOrderModel.ExpectedTime,
                            OrderStatus = "Inprogress",
                            PreviousQuantity = submitOrderModel.PreviousQuantity,
                            Message = Message,
                            // Add other properties as needed
                        };

                        // Save the new order to the Orders table
                        _context.Orders.Add(order);
                        _context.SaveChanges();
                    }

                    // Save the order details to the SubmitOrderModel table
                    var submitOrderModelEntity = new SubmitOrderModel
                    {
                        clientOrderId = orderId,
                        UserId = submitOrderModel.UserId,
                        Service = submitOrderModel.Service,
                        VendorName = submitOrderModel.VendorName,
                        VendorOrderId = submitOrderModel.VendorOrderId,
                        Quantity = submitOrderModel.Quantity,
                        ExpectedTime = submitOrderModel.ExpectedTime,
                        PreviousQuantity = submitOrderModel.PreviousQuantity,
                        Message = Message,
                        // Add other properties as needed
                    };

                    _context.SubmitOrderModel.Add(submitOrderModelEntity);
                    _context.SaveChanges();

                    // Return a success response
                    return RedirectToAction("AdminOrders");
                }
                else
                {
                    // If the model state is not valid, return an error response
                    return Json(new { success = false, error = "Invalid form data." });
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return Json(new { success = false, error = ex.Message });
            }
        }

        public IActionResult AdminOrders()
        {
            // Retrieve data from the database
            var submitOrders = _context.SubmitOrderModel.ToList();

            return View(submitOrders);
        }
        [HttpPost]

        public IActionResult BulkUploadLeads(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var leads = ParseExcel(file);
                    if (leads.Count > 0)
                    {
                        _context.Masterleads.AddRange(leads);
                        _context.SaveChanges();
                        return RedirectToAction("AllLeads");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No valid leads found in the uploaded file.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Please select a file to upload.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error uploading file. " + ex.Message);
            }

            return View();
        }

        private List<Masterleads> ParseExcel(IFormFile file)
        {
            var leads = new List<Masterleads>();

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 1; row <= rowCount; row++)
                    {
                        var newLead = new Masterleads
                        {

                            FirstName = worksheet.Cells[row, 1].Value?.ToString(),
                            PhoneNumber = worksheet.Cells[row, 2].Value?.ToString(),
                            LastName = worksheet.Cells[row, 3].Value?.ToString(),
                            Gender = worksheet.Cells[row, 4].Value?.ToString(),
                            Area = worksheet.Cells[row, 5].Value?.ToString(),
                            profession = worksheet.Cells[row, 6].Value?.ToString(),
                            Email = worksheet.Cells[row, 7].Value?.ToString(),
                            Address = worksheet.Cells[row, 8].Value?.ToString(),
                            City = worksheet.Cells[row, 9].Value?.ToString(),
                            Country = worksheet.Cells[row, 10].Value?.ToString(),



                        };

                        leads.Add(newLead);
                    }
                }
            }

            return leads;
        }
        private bool TryParseBoolean(string value)
        {
            if (bool.TryParse(value, out bool result))
            {
                return result;
            }

            // If parsing fails, return false as a default value.
            return false;
        }

        public IActionResult AddServices()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SaveService(ServiceModel service)
        {
            if (ModelState.IsValid)
            {
                // Save service to the database
                // Assuming ServiceModel is the model representing your service
                _context.Services.Add(service);
                _context.SaveChanges();

                return RedirectToAction("ViewServices");
            }

            return View("AddServices", service); // Return to the AddServices view with validation errors
        }
        public IActionResult ViewServices()
        {
            var services = _context.Services.ToList(); // Assuming you have a DbSet<ServiceModel> in your context
            return View(services);
        }
        public IActionResult Orders()
        {
            var orders = _context.Orders.OrderByDescending(o => o.OrderId).ToList();
            // Assuming you have a DbSet<Orders> in your context
            return View(orders);
        }

        // Action method to retrieve all LeadRates and render the LeadRate view
        public IActionResult LeadRate()
        {
            try
            {
                var leadRates = _context.LeadRate.ToList();
                return View(leadRates);
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateLeadRate(int leadRateId, double unitLead)
        {
            try
            {
                var leadRate = _context.LeadRate.Find(leadRateId);
                if (leadRate != null)
                {
                    leadRate.UnitLead = unitLead;
                    _context.SaveChanges();
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetLeadRateDetails(int leadRateId)
        {
            try
            {
                var leadRate = _context.LeadRate.FirstOrDefault(lr => lr.LeadRateId == leadRateId);
                if (leadRate != null)
                {
                    // Return lead rate details as JSON
                    return Json(new
                    {
                        leadRateId = leadRate.LeadRateId,
                        unitLead = leadRate.UnitLead
                        // Add other properties as needed
                    });
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult ApproveOrder(int orderId, string Message)
        {
            try
            {
                // Find the order by ID
                Order order = _context.Orders.Find(orderId);

                // Check if the order exists
                if (order != null)
                {
                    // Update the order status in the database
                    order.Message = Message;
                    order.OrderStatus = "Cancelled";
                    _context.SaveChanges();

                    return Json(new { success = true });
                }
                else
                {
                    // Handle the case where the order is not found
                    return Json(new { success = false, error = "Order not found." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine(ex.Message);
                return Json(new { success = false, error = "An error occurred while approving the order." });
            }
        }

        public IActionResult subscribe()
        {
            List<Subscription> subscriptions = _context.Subscription.ToList();
            return View(subscriptions);
        }
        [HttpPost]
        public IActionResult UpdateSubscription(int subscriptionId, string startDate, string endDate)
        {
            try
            {
                // Parse dates
                DateTime parsedStartDate = DateTime.ParseExact(startDate, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
                DateTime parsedEndDate = DateTime.Parse(endDate);

                // Find the subscription by ID
                Subscription subscription = _context.Subscription.Find(subscriptionId);

                // Check if the subscription exists
                if (subscription != null)
                {
                    // Update the subscription in the database
                    subscription.StartDate = parsedStartDate;
                    subscription.EndDate = parsedEndDate;
                    subscription.IsActive = true;
                    subscription.Status = "Approved";
                    _context.SaveChanges();

                    return Json(new { success = true });
                }
                else
                {
                    // Handle the case where the subscription is not found
                    return Json(new { success = false, error = "Subscription not found." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine(ex.Message);
                return Json(new { success = false, error = "An error occurred while updating the subscription." });
            }
        }
        [HttpPost]
        public IActionResult MarkAllAsRead()
        {
            try
            {
                // Get all notifications that are not read
                var unreadNotifications = _context.Notificationscs.Where(n => !n.IsRead).ToList();

                // Mark each notification as read
                foreach (var notification in unreadNotifications)
                {
                    notification.IsRead = true;
                }

                // Save changes to the database
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception (you might want to use a logging framework here)
                Console.Error.WriteLine($"Error marking all notifications as read: {ex.Message}");

                return Json(new { success = false, error = "An error occurred while marking notifications as read." });
            }
        }

        [HttpGet]
        public IActionResult GetServiceDetails(int id)
        {
            try
            {
                var service = _context.Services.Find(id);
                if (service != null)
                {
                    return Json(service);
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine(ex.Message);
                return Json(null);
            }
        }

        [HttpPost]
        public IActionResult DeleteService(int id)
        {
            try
            {
                var service = _context.Services.Find(id);

                if (service != null)
                {
                    _context.Services.Remove(service);
                    _context.SaveChanges();

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = "Service not found." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, error = "An error occurred while deleting the service." });
            }
        }
        [HttpPost]
        public IActionResult UpdateService(ServiceModel editedService)
        {
            try
            {
                var existingService = _context.Services.Find(editedService.Id);

                if (existingService != null)
                {
                    // Update service details
                    existingService.ServiceName = editedService.ServiceName;
                    existingService.Description = editedService.Description;
                    existingService.Quantity = editedService.Quantity;
                    existingService.Price = editedService.Price;
                    existingService.Priceperunit = editedService.Priceperunit;
                    // Update other fields as needed

                    _context.SaveChanges();

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = "Service not found." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, error = "An error occurred while updating the service." });
            }
        }

        [HttpPost]
        public IActionResult CompleteOrder(int orderId, string completionMessage)
        {
            try
            {
                var order = _context.Orders.Find(orderId);

                if (order != null)
                {
                    order.Message = completionMessage;
                    order.OrderStatus = "Completed";
                    _context.SaveChanges();

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = "Order not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "An error occurred while completing the order: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DismissSubscription(int subscriptionId)
        {
            try
            {
                // Find the subscription by ID
                Subscription subscription = _context.Subscription.Find(subscriptionId);

                // Check if the subscription exists
                if (subscription != null)
                {
                    // Update the subscription in the database
                    subscription.IsActive = false;
                    subscription.StartDate = DateTime.MinValue; // Set to minimum date or 0 as needed
                    subscription.EndDate = DateTime.MinValue;   // Set to minimum date or 0 as needed
                    _context.Subscription.Remove(subscription);// Remove the subscription from the context
                    _context.SaveChanges();

                    return Json(new { success = true });
                }
                else
                {
                    // Handle the case where the subscription is not found
                    return Json(new { success = false, error = "Subscription not found." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine(ex.Message);
                return Json(new { success = false, error = "An error occurred while dismissing the subscription." });
            }
        }
        [HttpGet]
        public IActionResult AllNotifications()
        {
            var notifications = _context.Notificationscs.OrderByDescending(n => n.Timestamp).ToList();
            return View(notifications);
        }
        public IActionResult Show()
        {
            var smsBalanceList = _context.SmsBalancecs.ToList();
            return View(smsBalanceList);
        }
        [HttpGet]
        public IActionResult GetDetails(int id)
        {
            var smsBalance = _context.SmsBalancecs.Find(id);
            return Json(smsBalance);
        }
        [HttpPost]
        public IActionResult Update(SmsBalancecs smsBalance)
        {
            if (ModelState.IsValid)
            {
                var existingEntity = _context.SmsBalancecs.Find(smsBalance.SmsBalancecsId);
                if (existingEntity != null)
                {
                    // Update the existing entity with the new values
                    _context.Entry(existingEntity).CurrentValues.SetValues(smsBalance);
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound(); // Or return a meaningful response if the entity with the given ID is not found
                }
            }
            return BadRequest();
        }

        public IActionResult Detuct()
        {
            var detuctionTypes = _context.DetuctionType.ToList();
            return View(detuctionTypes);
        }
        // GET: DetuctionTypes/GetDetuctionType/5
        [HttpGet]
        public IActionResult GetDetuctionType(int id)
        {
            var detuctionType = _context.DetuctionType.Find(id);
            if (detuctionType == null)
            {
                return NotFound();
            }
            return Json(detuctionType);
        }

        // GET: Home/UpdateDetuctionType
        [HttpPost]
        public IActionResult UpdateDetuctionType(int id, int detuction)
        {
            var detuctionType = _context.DetuctionType.FirstOrDefault(d => d.DetuctionTypeId == id);
            if (detuctionType == null)
            {
                return NotFound();
            }

            detuctionType.Detuction = detuction;
            _context.SaveChanges();

            return View("detuctionTypes");
        }
    }
}