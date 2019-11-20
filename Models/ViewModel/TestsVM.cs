using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDeProvas.Models.ViewModel
{
    public class TestsVM
    {
        public int TestId { get; set; }
        public string Student { get; set; }
        public string Period { get; set; }
        public DateTime Data { get; set; }
        public int QuestionsNumber { get; set; }
    }
}
