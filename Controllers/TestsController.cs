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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace GeradorDeProvas.Controllers
{
    public class TestsController : Controller
    {
        private readonly DbConn _context;

        public TestsController(DbConn context)
        {
            _context = context;
        }

        public async Task<IActionResult> CreateFile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .FirstOrDefaultAsync(m => m.Id == id);

            if (test == null)
            {
                return NotFound();
            }

            /* Criação de provas */
            string fileName = test.Student.Name + "-test";

            Document doc = new Document(PageSize.A4);
            doc.SetMargins(40, 40, 40, 80);
            doc.AddCreationDate();

            string pasta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string dir = pasta + @"\" + fileName + ".pdf";

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(dir, FileMode.Create));

            doc.Open();
            List<Paragraph> paragrafos = new List<Paragraph>();

            Paragraph p1 = new Paragraph("", new Font(Font.FontFamily.HELVETICA, 12));
            p1.Alignment = Element.ALIGN_CENTER;
            p1.Add($"FICTITOUS COLLEGE\n");
            p1.Add($"MULTIDISCIPLINARY EXAM\n\n");
            paragrafos.Add(p1);

            Paragraph p2 = new Paragraph("", new Font(Font.FontFamily.HELVETICA, 12));
            p2.Alignment = Element.ALIGN_JUSTIFIED;
            p2.Add($"NAME: {test.Student.Name.ToUpper()}    |   {test.Period.Name.ToUpper()} PERIOD\n");
            p2.Add($"TEACHER: ______________________________    |  DATE: {test.Data.Date.ToString()}\n");
            paragrafos.Add(p2);

            foreach (var q in test.TestQuestions)
            {
                Paragraph p = new Paragraph("", new Font(Font.FontFamily.HELVETICA, 12));
                p.Alignment = Element.ALIGN_JUSTIFIED;
                p.Add($"\n{q.Question.Id}. {q.Question.Text}\n");
                foreach (var alter in q.Question.Alternatives)
                {
                    p.Add($"( ) {alter.Text}\n");
                }

                paragrafos.Add(p);
            }

            foreach (var x in paragrafos)
            {
                doc.Add(x);
            }

            doc.Close();




            return RedirectToAction(nameof(Index));
        }

        public void fillTests()
        {
            ViewBag.Students = _context.Students.Select(s => new SelectListItem()
            { Text = s.Name, Value = s.Id.ToString() }).ToList();

            ViewBag.Periods = _context.Periods.Select(s => new SelectListItem()
            { Text = s.Name, Value = s.Id.ToString() }).ToList();
        }

        // GET: Tests
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tests.ToListAsync());
        }

        // GET: Tests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // GET: Tests/Create
        public IActionResult Create()
        {
            fillTests();
            return View();
        }

        // POST: Tests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Student,Period,Data,QuestionsNumber")] TestsVM testVM)
        {
            if (ModelState.IsValid)
            {
                Random rnd = new Random();
                Test test = new Test();

                /* Student */
                int selectedStudent = 0;
                int.TryParse(testVM.Student, out selectedStudent);
                var student = _context.Students.Where(w => w.Id == selectedStudent).FirstOrDefault();

                /* Period */
                int selectedPeriod = 0;
                int.TryParse(testVM.Period, out selectedPeriod);
                var period = _context.Periods.Where(w => w.Id == selectedPeriod).FirstOrDefault();

                /* Test */
                test.Student = student;
                test.Period = period;
                test.Data = testVM.Data;

                /* Subjects */
                var periodSubjects = period.PeriodSubjects.FindAll(w => w.Period == period);

                List<Subject> subsList = new List<Subject>();
                foreach(var subject in periodSubjects)
                {
                    subsList.Add(subject.Subject);
                }
                Subject[] subjects = subsList.ToArray();

                int questionBySub = testVM.QuestionsNumber / subjects.Length;

                Question[] questions = new Question[questionBySub];

                List<Question> questionsList = new List<Question>();
                for (var i = 0; i<subjects.Length; i++)
                {
                    for(var j = 0; j<questionBySub; j++)
                    {
                        int r = rnd.Next(subjects[i].Questions.Count);
                        questionsList.Add(subjects[i].Questions[r]);
                    }
                }

                _context.Add(test);

                test.TestQuestions = new List<TestQuestion>();

                foreach (var q in questionsList)
                {
                    test.TestQuestions.Add(new Models.Entity.TestQuestion { Question = q }) ;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testVM);
        }

        // GET: Tests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }
            return View(test);
        }

        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data")] Test test)
        {
            if (id != test.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(test);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestExists(test.Id))
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
            return View(test);
        }

        // GET: Tests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestExists(int id)
        {
            return _context.Tests.Any(e => e.Id == id);
        }
    }
}
