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
    public class OrderDetailRepository : Repository<OrderDetails>,IOrderDetailRepository
    {
        private ApplicationDbContect _db;
       
        public OrderDetailRepository(ApplicationDbContect db):base(db) 
        {
            _db = db;
            
        }
   
        public void Update(OrderDetails orderDetails)
        {
            _db.OrderDetails.Update(orderDetails);
        }
    }
}
