using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;  // Add this namespace
using EcoTrackAdmin.Areas.Identity.Data;
using EcoTrackAdmin.Models;

namespace EcoTrackAdmin.Controllers
{
    // Add the [Authorize] attribute here to require authentication for all actions in this controller
    [Authorize]
    public class EmailRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmailRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmailRequests
        public async Task<IActionResult> Index()
        {
            return View(await _context.EmailRequests.ToListAsync());
        }

        // GET: EmailRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailRequest = await _context.EmailRequests
                .FirstOrDefaultAsync(m => m.requestid == id);
            if (emailRequest == null)
            {
                return NotFound();
            }

            return View(emailRequest);
        }

        // GET: EmailRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailRequest = await _context.EmailRequests
                .FirstOrDefaultAsync(m => m.requestid == id);
            if (emailRequest == null)
            {
                return NotFound();
            }

            return View(emailRequest);
        }

        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailRequest = await _context.EmailRequests
                .FirstOrDefaultAsync(m => m.requestid == id);
            if (emailRequest == null)
            {
                return NotFound();
            }

            return View(emailRequest);
        }

        // POST: EmailRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emailRequest = await _context.EmailRequests.FindAsync(id);
            if (emailRequest != null)
            {
                // Create a new notification
                var notification = new Notification
                {
                    Nid = emailRequest.userId, // Set Nid to the userId of the emailRequest
                    Message = "Your Email Request has been Denied"
                };

                // Add the notification to the database
                _context.Notifications.Add(notification);

                // Remove the email request
                _context.EmailRequests.Remove(emailRequest);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: EmailRequests/Approve/5
        [HttpPost, ActionName("Approve")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveConfirmed(int id)
        {
            var emailRequest = await _context.EmailRequests.FindAsync(id);
            if (emailRequest != null)
            {
                // Update the user's email details in AspNetUsers
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == emailRequest.userId);
                if (user != null)
                {
                    user.Email = emailRequest.newEmail;
                    user.NormalizedEmail = emailRequest.newEmail.ToUpper();
                    user.UserName = emailRequest.newEmail;
                    user.NormalizedUserName = emailRequest.newEmail.ToUpper();
                }

                // Create a notification
                var notification = new Notification
                {
                    Nid = emailRequest.userId, // Set Nid to the userId of the emailRequest
                    Message = "Your Email Request has been Approved"
                };

                // Add the notification to the database
                _context.Notifications.Add(notification);

                // Remove the email request
                _context.EmailRequests.Remove(emailRequest);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EmailRequestExists(int id)
        {
            return _context.EmailRequests.Any(e => e.requestid == id);
        }
    }
}
