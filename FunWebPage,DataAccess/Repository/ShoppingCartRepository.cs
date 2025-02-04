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
    public class ShoppingCartRepository : Repository<ShoppingCart>,IShoppingCartRepository
    {
        private ApplicationDbContect _db;
       
        public ShoppingCartRepository(ApplicationDbContect db):base(db) 
        {
            _db = db;
            
        }
   
        public void Update(ShoppingCart shoppingCart)
        {
            _db.ShoppingCarts.Update(shoppingCart);
        }
    }
}
