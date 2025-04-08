using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcoTrackAdmin.Areas.Identity.Data;
using EcoTrackAdmin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;  // Add this namespace

namespace EcoTrackAdmin.Controllers
{
    // Apply the [Authorize] attribute to restrict access to authenticated users
    [Authorize]
    public class UserProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfilesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserProfiles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserProfiles.Include(u => u.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserProfiles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfiles
                .Include(u => u.ApplicationUser)
                .FirstOrDefaultAsync(m => m.UniqueUserName == id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // GET: UserProfiles/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserProfiles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UniqueUserName,ApplicationUserId")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", userProfile.ApplicationUserId);
            return View(userProfile);
        }

        // GET: UserProfiles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfiles
                .Include(u => u.ApplicationUser)
                .FirstOrDefaultAsync(u => u.UniqueUserName == id);

            if (userProfile == null)
            {
                return NotFound();
            }

            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", userProfile.ApplicationUserId);
            return View(userProfile);
        }

        // POST: UserProfiles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserProfile userProfile)
        {
            if (id != userProfile.UniqueUserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUserProfile = await _context.UserProfiles
                        .Include(up => up.ApplicationUser)
                        .FirstOrDefaultAsync(up => up.UniqueUserName == id);

                    if (existingUserProfile == null)
                    {
                        return NotFound();
                    }

                    existingUserProfile.ApplicationUserId = userProfile.ApplicationUserId;

                    var applicationUser = existingUserProfile.ApplicationUser;
                    if (applicationUser != null)
                    {
                        applicationUser.FirstName = userProfile.ApplicationUser.FirstName;
                        applicationUser.LastName = userProfile.ApplicationUser.LastName;
                        applicationUser.Type = userProfile.ApplicationUser.Type;

                        if (string.IsNullOrEmpty(applicationUser.PhoneNumber))
                        {
                            applicationUser.PhoneNumber = userProfile.ApplicationUser.PhoneNumber;
                        }

                        _context.Users.Update(applicationUser);
                    }

                    _context.UserProfiles.Update(existingUserProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProfileExists(userProfile.UniqueUserName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", userProfile.ApplicationUserId);
            return View(userProfile);
        }

        // GET: UserProfiles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfiles
                .Include(u => u.ApplicationUser)
                .FirstOrDefaultAsync(m => m.UniqueUserName == id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // POST: UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userProfile = await _context.UserProfiles
                .Include(up => up.ApplicationUser)
                .FirstOrDefaultAsync(up => up.UniqueUserName == id);

            if (userProfile != null)
            {
                var user = userProfile.ApplicationUser;
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }

                _context.UserProfiles.Remove(userProfile);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProfileExists(userProfile.UniqueUserName))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UserProfileExists(string id)
        {
            return _context.UserProfiles.Any(e => e.UniqueUserName == id);
        }
    }
}
