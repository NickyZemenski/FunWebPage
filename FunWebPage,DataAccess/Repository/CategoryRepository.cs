using FunWebPage.DataAccess.Data;
using FunWebPage.Models;
using FunWebPage_DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FunWebPage_DataAccess.Repository
{
    public class CategoryRepository : Repository<CategoryModel>,ICategoryRepository
    {
        private ApplicationDbContect _db;
       
        public CategoryRepository(ApplicationDbContect db):base(db) 
        {
            _db = db;
            
        }
        public void Save()
        {
           _db.SaveChanges();
        }

        public void Update(CategoryModel category)
        {
            _db.Categories.Update(category);
        }
    }
}
