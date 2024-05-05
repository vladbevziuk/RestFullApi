using Microsoft.EntityFrameworkCore;
using RestFullApi.Models;

namespace RestFullApi.Data
{
    public class ApplicationDb : DbContext
    {
        public ApplicationDb(DbContextOptions<ApplicationDb> options)
            : base(options) 
        { 

        }
        public DbSet<Villa> Villas { get; set; }
    }
}
