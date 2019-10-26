using GeradorDeProvas.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDeProvas.Models
{
    public class DbConn : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=gerator;Username=postgres;Port=5432;Password=root");
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Alternative> Alternatives { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<PeriodStudent> PeriodStudents { get; set; }
        public DbSet<PeriodSubject> PeriodSubjects { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
    }
}
