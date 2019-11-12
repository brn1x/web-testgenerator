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
    public class StudentsController : Controller
    {
        private readonly DbConn _context;

        public StudentsController(DbConn context)
        {
            _context = context;
        }

        private void fillPeriods()
        {
            ViewBag.Periods = _context.Periods.Select(s => new SelectListItem()
            { Text = s.Name, Value = s.Id.ToString() }).ToList();
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.PeriodStudents.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.PeriodStudents
                .FirstOrDefaultAsync(m => m.Student.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            fillPeriods();
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Student,Period")] PeriodStudentVM periodStudent)
        {
            if (ModelState.IsValid)
            {
                var student = new Student();
                student.Name = periodStudent.Student;

                int selectedPeriod = 0;
                int.TryParse(periodStudent.Period, out selectedPeriod);

                var period = _context.Periods.Where(s => s.Id == selectedPeriod).FirstOrDefault();

                _context.Add(student);
                period.PeriodStudent.Add(new Models.Entity.PeriodStudent { Student = student });

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(periodStudent);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            PeriodStudent periodStudent = new PeriodStudent();
            periodStudent = await _context.PeriodStudents.Where(w => w.Student.Id == id).FirstAsync();

            PeriodStudentVM periodStudentVM = new PeriodStudentVM();
            periodStudentVM.Id = periodStudent.Student.Id;
            periodStudentVM.Student = periodStudent.Student.Name;
            periodStudentVM.Period = periodStudent.Period.Name;

            if (periodStudent == null)
            {
                return NotFound();
            }

            ViewBag.Periods = _context.Periods.Select(s => new SelectListItem()
            { Selected = (s.Id == periodStudent.Period.Id), Text = s.Name, Value = s.Name }).ToList();
            return View(periodStudentVM);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Student,Period")] PeriodStudentVM periodStudent)
        {
            if (id != periodStudent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var student = new Student();
                    student.Id = periodStudent.Id;
                    student.Name = periodStudent.Student;

                    _context.Update(student);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(periodStudent.Id))
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

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.PeriodStudents
                .FirstOrDefaultAsync(m => m.Student.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.PeriodStudents.Where(w => w.Student.Id == id).FirstAsync();
            _context.PeriodStudents.Remove(student);
            _context.Students.Remove(student.Student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
