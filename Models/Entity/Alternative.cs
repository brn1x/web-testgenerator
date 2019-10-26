using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDeProvas.Models.Entity
{
    public class Alternative
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int CorrectAnswer { get; set; }
    }
}
