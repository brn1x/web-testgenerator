using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GeradorDeProvas.Models;
using GeradorDeProvas.Models.Entity;
using GeradorDeProvas.Models.ViewModel;

namespace GeradorDeProvas.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly DbConn _context;

        public SubjectsController(DbConn context)
        {
            _context = context;
        }

        private void fillPeriods()
        {
            ViewBag.Periods = _context.Periods.Select(s => new SelectListItem()
            { Text = s.Name, Value = s.Id.ToString() }).ToList();
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            return View(await _context.PeriodSubjects.ToListAsync());
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.PeriodSubjects
                .FirstOrDefaultAsync(m => m.Subject.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            fillPeriods();
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Subject,Period")] SubjectPeriodVM subjectPeriod)
        {
            if (ModelState.IsValid)
            {
                var subject = new Subject();
                subject.Name = subjectPeriod.Subject;

                int selectedPeriod = 0;
                int.TryParse(subjectPeriod.Period, out selectedPeriod);

                var period = _context.Periods.Where(s => s.Id == selectedPeriod).FirstOrDefault();

                _context.Add(subject);
                period.PeriodSubjects.Add(new Models.Entity.PeriodSubject { Subject = subject });
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subjectPeriod);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PeriodSubject subjectPer = new PeriodSubject();
            subjectPer = await _context.PeriodSubjects.Where(w => w.Subject.Id == id).FirstAsync();
            
            if (subjectPer == null)
            {
                return NotFound();
            }

            ViewBag.Periods = _context.Periods.Select(s => new SelectListItem()
            { Selected = (s.Id == subjectPer.Period.Id), Text = s.Name, Value = s.Name }).ToList();
            return View(subjectPer);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,Period")] PeriodSubject periodSubject)
        {
            if (id != periodSubject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Subject subject = new Subject();
                    subject.Id = periodSubject.Id;
                    subject.Name = periodSubject.Subject.Name;

                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(periodSubject.Id))
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

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.PeriodSubjects
                .FirstOrDefaultAsync(m => m.Subject.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.PeriodSubjects.Where(w => w.Subject.Id == id).FirstAsync();
            _context.PeriodSubjects.Remove(subject);
            _context.Subjects.Remove(subject.Subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}
