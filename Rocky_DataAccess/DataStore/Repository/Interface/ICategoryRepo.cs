using Rocky_Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky_DataAccess.DataStore.Repository.Interface
{
    public interface ICategoryRepo :IRepository<Category>
    {
        public void Update(Category obj);
    }
}
