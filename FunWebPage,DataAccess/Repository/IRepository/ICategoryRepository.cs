﻿using FunWebPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunWebPage_DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<CategoryModel>
    {
        void Update(CategoryModel category);
      
    }
}
