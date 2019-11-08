using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDeProvas.Models.ViewModel
{
    public class QuestionSubjectAlternativeVM
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int CorrectAnswer { get; set; }
        public string Subject { get; set; }
        public int AlternativeQtt { get; set; }
    }
}
