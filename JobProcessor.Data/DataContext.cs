using JobProcessor.Data.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace JobProcessor.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        { }

        public DbSet<Job> Jobs { get; set; }
    }
}
