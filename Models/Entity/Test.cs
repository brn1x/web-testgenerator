using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDeProvas.Models.Entity
{
    public class Test
    {
        public int Id { get; set; }
        public virtual Period Period { get; set; }
        public virtual List<TestQuestion> TestQuestions { get; set; }
    }
}
