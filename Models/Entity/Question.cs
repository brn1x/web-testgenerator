using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDeProvas.Models.Entity
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int CorrectAnswer { get; set; }
        public virtual List<Alternative> Alternatives { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
