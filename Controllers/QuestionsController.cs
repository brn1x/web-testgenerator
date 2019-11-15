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
    public class QuestionsController : Controller
    {
        private readonly DbConn _context;

        public QuestionsController(DbConn context)
        {
            _context = context;
        }

        private void FillSubjectSelect()
        {
            ViewBag.Subjects = _context.Subjects.Select(s => new SelectListItem()
            { Text = s.Name, Value = s.Id.ToString() }).ToList();
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Questions.ToListAsync());
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            Alternative[] alternatives = question.Alternatives.ToArray();

            QuestionVM qViewModel = new QuestionVM();
            qViewModel.QuestionId = question.Id;
            qViewModel.Question = question.Text;
            qViewModel.CorrectAnswer = question.CorrectAnswer;
            qViewModel.Subject = question.Subject.Name;
            qViewModel.AlternativeA = alternatives[0].Text;
            qViewModel.AlternativeB = alternatives[1].Text;
            qViewModel.AlternativeC = alternatives[2].Text;
            qViewModel.AlternativeD = alternatives[3].Text;
            qViewModel.AlternativeE = alternatives[4].Text;

            return View(qViewModel);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            FillSubjectSelect();
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionId,Question,Subject,CorrectAnswer,AlternativeA,AlternativeB,AlternativeC,AlternativeD,AlternativeE")] QuestionVM questionVM)
        {
            if (ModelState.IsValid)
            {
                Question question = new Question();
                question.Text = questionVM.Question;
                question.CorrectAnswer = questionVM.CorrectAnswer;

                int selectedSubject = 0;
                int.TryParse(questionVM.Subject, out selectedSubject);

                question.Subject = _context.Subjects.Where(w => w.Id == selectedSubject).FirstOrDefault();
                _context.Add(question);

                question.Alternatives = new List<Alternative>();

                question.Alternatives.Add(new Models.Entity.Alternative { Text = questionVM.AlternativeA });
                question.Alternatives.Add(new Models.Entity.Alternative { Text = questionVM.AlternativeB });
                question.Alternatives.Add(new Models.Entity.Alternative { Text = questionVM.AlternativeC });
                question.Alternatives.Add(new Models.Entity.Alternative { Text = questionVM.AlternativeD });
                question.Alternatives.Add(new Models.Entity.Alternative { Text = questionVM.AlternativeE });

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questionVM);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            QuestionVM qViewModel = new QuestionVM();
            qViewModel.QuestionId = question.Id;
            qViewModel.Question = question.Text;
            qViewModel.CorrectAnswer = question.CorrectAnswer;
            qViewModel.Subject = question.Subject.Name;

            Alternative[] alternatives = question.Alternatives.ToArray();
            qViewModel.AlternativeA = alternatives[0].Text;
            qViewModel.AlternativeB = alternatives[1].Text;
            qViewModel.AlternativeC = alternatives[2].Text;
            qViewModel.AlternativeD = alternatives[3].Text;
            qViewModel.AlternativeE = alternatives[4].Text;

            ViewBag.Subjects = _context.Subjects.Select(s => new SelectListItem()
            { Selected = (s.Id == question.Subject.Id), Text = s.Name, Value = s.Name }).ToList();
            return View(qViewModel);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId,Question,Subject,CorrectAnswer,AlternativeA,AlternativeB,AlternativeC,AlternativeD,AlternativeE")] QuestionVM questionVM)
        {
            if (id != questionVM.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Question question = new Question();
                    question.Id = questionVM.QuestionId;
                    question.Text = questionVM.Question;
                    question.CorrectAnswer = questionVM.CorrectAnswer;

                    var Alternatives = _context.Questions.Where(w => w.Id == question.Id).Select(s => s.Alternatives);
                    
                    foreach(var alternative in Alternatives){
                        foreach(var i in alternative)
                        {
                            _context.Remove(i);
                        }
                    }

                    question.Alternatives = new List<Alternative>();
                    question.Alternatives.Add(new Models.Entity.Alternative { Text = questionVM.AlternativeA });
                    question.Alternatives.Add(new Models.Entity.Alternative { Text = questionVM.AlternativeB });
                    question.Alternatives.Add(new Models.Entity.Alternative { Text = questionVM.AlternativeC });
                    question.Alternatives.Add(new Models.Entity.Alternative { Text = questionVM.AlternativeD });
                    question.Alternatives.Add(new Models.Entity.Alternative { Text = questionVM.AlternativeE });

                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(questionVM.QuestionId))
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
            return View(questionVM);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            QuestionVM qViewModel = new QuestionVM();
            qViewModel.QuestionId = question.Id;
            qViewModel.Question = question.Text;
            qViewModel.CorrectAnswer = question.CorrectAnswer;
            qViewModel.Subject = question.Subject.Name;

            Alternative[] alternatives = question.Alternatives.ToArray();
            qViewModel.AlternativeA = alternatives[0].Text;
            qViewModel.AlternativeB = alternatives[1].Text;
            qViewModel.AlternativeC = alternatives[2].Text;
            qViewModel.AlternativeD = alternatives[3].Text;
            qViewModel.AlternativeE = alternatives[4].Text;

            ViewBag.Subjects = _context.Subjects.Select(s => new SelectListItem()
            { Selected = (s.Id == question.Subject.Id), Text = s.Name, Value = s.Name }).ToList();
            return View(qViewModel);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.FindAsync(id);

            var Alternatives = _context.Questions.Where(w => w.Id == question.Id).Select(s => s.Alternatives);

            foreach (var alternative in Alternatives)
            {
                foreach (var i in alternative)
                {
                    _context.Remove(i);
                }
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
