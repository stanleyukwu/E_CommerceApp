using Rocky_DataAccess.DataStore.Repository.Interface;
using Rocky_Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rocky_DataAccess.DataStore.Repository.Implementation
{
    public class Categoryrepository : Repository<Category>, ICategoryRepo
    {
        private readonly AppDbContext _context;

        public Categoryrepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
       

        public void Update(Category obj)
        {
            var objTupdate = base.FirstOrDefault(u => u.Id == obj.Id);
            if(objTupdate != null)
            {
                objTupdate.Name = obj.Name;
                objTupdate.DisplayOrder = obj.DisplayOrder;
            }
        }
    }
}
