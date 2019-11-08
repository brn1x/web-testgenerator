using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDeProvas.Models.Entity
{
    public class PeriodSubject
    {
        public int Id { get; set; }
        public virtual Period Period { get; set; }
        public virtual Subject Subject { get; set; }
    }
}