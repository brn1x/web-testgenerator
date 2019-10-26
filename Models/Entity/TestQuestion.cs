using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDeProvas.Models.Entity
{
    public class TestQuestion
    {
        public int Id { get; set; }
        public virtual Test Test { get; set; }
        public virtual Question Question { get; set; }
    }
}
