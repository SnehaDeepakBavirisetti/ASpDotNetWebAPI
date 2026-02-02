using ASpDotNetWebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASpDotNetWebAPI.Data
{
    public class ApplicationDbcontext : DbContext
    {
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base(options)
        {


        }

        public DbSet<Employee> Employees { get; set; }

        internal object Find(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
