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

            QuestionSubjectAlternativeVM qViewModel = new QuestionSubjectAlternativeVM();
            qViewModel.Id = question.Id;
            qViewModel.Text = question.Text;
            qViewModel.CorrectAnswer = question.CorrectAnswer;
            qViewModel.Subject = question.Subject.Name;

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
        public async Task<IActionResult> Create([Bind("Id,Text,CorrectAnswer,Subject,AlternativeQtt")] QuestionSubjectAlternativeVM questionSubjectAlternative)
        {
            if (ModelState.IsValid)
            {
                var question = new Question();
                question.Text = questionSubjectAlternative.Text;
                question.CorrectAnswer = questionSubjectAlternative.CorrectAnswer;

                int selectedSubject = 0;
                int.TryParse(questionSubjectAlternative.Subject, out selectedSubject);

                question.Subject = _context.Subjects.Where(w => w.Id == selectedSubject).FirstOrDefault();
                _context.Add(question);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questionSubjectAlternative);
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

            QuestionSubjectAlternativeVM qViewModel = new QuestionSubjectAlternativeVM();
            qViewModel.Id = question.Id;
            qViewModel.Text = question.Text;
            qViewModel.CorrectAnswer = question.CorrectAnswer;
            qViewModel.Subject = question.Subject.Name;

            ViewBag.Subjects = _context.Subjects.Select(s => new SelectListItem()
            { Selected = (s.Id == question.Subject.Id), Text = s.Name, Value = s.Name }).ToList();
            return View(qViewModel);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,CorrectAnswer,Subject")] QuestionSubjectAlternativeVM questionSubjectAlternative)
        {
            if (id != questionSubjectAlternative.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Question question = new Question();
                    question.Id = questionSubjectAlternative.Id;
                    question.Text = questionSubjectAlternative.Text;
                    question.CorrectAnswer = questionSubjectAlternative.CorrectAnswer;

                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(questionSubjectAlternative.Id))
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
            return View(questionSubjectAlternative);
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

            QuestionSubjectAlternativeVM qViewModel = new QuestionSubjectAlternativeVM();
            qViewModel.Id = question.Id;
            qViewModel.Text = question.Text;
            qViewModel.CorrectAnswer = question.CorrectAnswer;
            qViewModel.Subject = question.Subject.Name;

            return View(qViewModel);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.FindAsync(id);
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
