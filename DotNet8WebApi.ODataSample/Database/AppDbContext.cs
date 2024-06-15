using DotNet8WebApi.ODataSample.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet8WebApi.ODataSample.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BlogModel> Blogs { get; set; }
    }
}
