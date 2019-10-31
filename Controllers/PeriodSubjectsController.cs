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
    public class PeriodSubjectsController : Controller
    {
        private readonly DbConn _context;

        public PeriodSubjectsController(DbConn context)
        {
            _context = context;
        }

        // GET: PeriodSubjects
        public async Task<IActionResult> Index()
        {
            return View(await _context.PeriodSubjects.ToListAsync());
        }

        // GET: PeriodSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodSubject = await _context.PeriodSubjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (periodSubject == null)
            {
                return NotFound();
            }

            return View(periodSubject);
        }

        // GET: PeriodSubjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PeriodSubjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] PeriodSubject periodSubject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(periodSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(periodSubject);
        }

        // GET: PeriodSubjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodSubject = await _context.PeriodSubjects.FindAsync(id);
            if (periodSubject == null)
            {
                return NotFound();
            }
            return View(periodSubject);
        }

        // POST: PeriodSubjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] PeriodSubject periodSubject)
        {
            if (id != periodSubject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(periodSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeriodSubjectExists(periodSubject.Id))
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
            return View(periodSubject);
        }

        // GET: PeriodSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodSubject = await _context.PeriodSubjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (periodSubject == null)
            {
                return NotFound();
            }

            return View(periodSubject);
        }

        // POST: PeriodSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var periodSubject = await _context.PeriodSubjects.FindAsync(id);
            _context.PeriodSubjects.Remove(periodSubject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeriodSubjectExists(int id)
        {
            return _context.PeriodSubjects.Any(e => e.Id == id);
        }
    }
}
