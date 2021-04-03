using CP_2021.Models;
using Microsoft.EntityFrameworkCore;

namespace CP_2021.Data
{
    internal class TestDBContext : DbContext
    {
        public TestDBContext()
        {

        }

        public TestDBContext(DbContextOptions<TestDBContext> options):base(options)
        {

        }

        public DbSet<DBPerson> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-LRSVKUO;Database=Company;Trusted_Connection=True;");
        }
    }
}
