using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperAdmin.Areas.Identity.Data;
using SuperAdmin.Models; // Replace with your actual namespace

public class NotificationsController : Controller
{
    private readonly SuperAdminContext _context; // Replace with your actual DbContext

    public NotificationsController(SuperAdminContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetUnreadNotifications()
    {
        var unreadNotifications = _context.Notificationscs
            .Where(n => !n.IsRead)
            .OrderByDescending(n => n.Timestamp)
            .Take(5) // Adjust the number of notifications to be fetched as needed
            .ToList();

        return Json(unreadNotifications);
    }

    [HttpPost]
    public IActionResult MarkAllAsRead()
    {
        var unreadNotifications = _context.Notificationscs
            .Where(n => !n.IsRead)
            .ToList();

        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true;
        }

        _context.SaveChanges();

        return Ok();
    }
}
