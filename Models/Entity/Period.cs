using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDeProvas.Models.Entity
{
    public class Period
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<PeriodSubject> PeriodSubjects { get; set; }
    }
}