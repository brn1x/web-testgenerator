using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GeradorDeProvas.Models;
using GeradorDeProvas.Models.Entity;

namespace GeradorDeProvas.Controllers
{
    public class PeriodStudentsController : Controller
    {
        private readonly DbConn _context;

        public PeriodStudentsController(DbConn context)
        {
            _context = context;
        }

        // GET: PeriodStudents
        public async Task<IActionResult> Index()
        {
            return View(await _context.PeriodStudents.ToListAsync());
        }

        // GET: PeriodStudents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodStudent = await _context.PeriodStudents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (periodStudent == null)
            {
                return NotFound();
            }

            return View(periodStudent);
        }

        // GET: PeriodStudents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PeriodStudents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] PeriodStudent periodStudent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(periodStudent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(periodStudent);
        }

        // GET: PeriodStudents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodStudent = await _context.PeriodStudents.FindAsync(id);
            if (periodStudent == null)
            {
                return NotFound();
            }
            return View(periodStudent);
        }

        // POST: PeriodStudents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] PeriodStudent periodStudent)
        {
            if (id != periodStudent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(periodStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeriodStudentExists(periodStudent.Id))
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
            return View(periodStudent);
        }

        // GET: PeriodStudents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodStudent = await _context.PeriodStudents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (periodStudent == null)
            {
                return NotFound();
            }

            return View(periodStudent);
        }

        // POST: PeriodStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var periodStudent = await _context.PeriodStudents.FindAsync(id);
            _context.PeriodStudents.Remove(periodStudent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeriodStudentExists(int id)
        {
            return _context.PeriodStudents.Any(e => e.Id == id);
        }
    }
}
