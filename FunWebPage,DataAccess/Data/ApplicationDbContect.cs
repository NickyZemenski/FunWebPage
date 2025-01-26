using FunWebPage.Models;
using Microsoft.EntityFrameworkCore;

namespace FunWebPage.DataAccess.Data
{
    public class ApplicationDbContect : DbContext
    {
        public ApplicationDbContect(DbContextOptions<ApplicationDbContect> options) :base(options)
        {
                
        }

        public DbSet<CategoryModel> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryModel>().HasData(
                new CategoryModel { CategoryId=1, Name= "Action", DisplayOrder=1},
                new CategoryModel { CategoryId=2, Name= "Romance", DisplayOrder=2},
                new CategoryModel { CategoryId=3, Name= "Fantasy", DisplayOrder=3}
                );
        }

    }
}
