using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDeProvas.Models.ViewModel
{
    public class QuestionVM
    {
        public int QuestionId { get; set; }
        public string Subject { get; set; }
        public string Question { get; set; }
        public int CorrectAnswer { get; set; }
        public string AlternativeA { get; set; }
        public string AlternativeB { get; set; }
        public string AlternativeC { get; set; }
        public string AlternativeD { get; set; }
        public string AlternativeE { get; set; }
    }
}
