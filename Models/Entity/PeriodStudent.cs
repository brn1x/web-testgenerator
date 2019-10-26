using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDeProvas.Models.Entity
{
    public class PeriodStudent
    {
        public int Id { get; set; }
        public virtual Student Student { get; set; }
        public virtual Period Period { get; set; }
    }
}
