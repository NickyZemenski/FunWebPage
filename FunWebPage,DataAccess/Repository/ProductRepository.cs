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
    public class ProductRepository : Repository<ProductModel>, IProductRepository
    {
        private ApplicationDbContect _db;

        public ProductRepository(ApplicationDbContect db) : base(db)
        {
            _db = db;

        }


        public void Update(ProductModel product)
        {
            _db.Products.Update(product);
        }
    }
}
