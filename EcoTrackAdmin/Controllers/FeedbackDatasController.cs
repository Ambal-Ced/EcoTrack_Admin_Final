﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcoTrackAdmin.Areas.Identity.Data;
using EcoTrackAdmin.Models;
using Microsoft.AspNetCore.Authorization;  // Add this namespace

namespace EcoTrackAdmin.Controllers
{
    // Apply the [Authorize] attribute to restrict access to authenticated users
    [Authorize]
    public class FeedbackDatasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackDatasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FeedbackDatas
        public async Task<IActionResult> Index()
        {
            return View(await _context.FeedbackDatas.ToListAsync());
        }

        // GET: FeedbackDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedbackData = await _context.FeedbackDatas
                .FirstOrDefaultAsync(m => m.cid == id);
            if (feedbackData == null)
            {
                return NotFound();
            }

            return View(feedbackData);
        }

        // POST: FeedbackDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedbackData = await _context.FeedbackDatas.FindAsync(id);
            if (feedbackData != null)
            {
                _context.FeedbackDatas.Remove(feedbackData);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackDataExists(int id)
        {
            return _context.FeedbackDatas.Any(e => e.cid == id);
        }
    }
}
