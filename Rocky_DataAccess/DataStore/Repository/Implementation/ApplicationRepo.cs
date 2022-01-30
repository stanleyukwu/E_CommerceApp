using Microsoft.EntityFrameworkCore;
using Rocky_DataAccess.DataStore.Repository.Interface;
using Rocky_Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky_DataAccess.DataStore.Repository.Implementation
{
   public class ApplicationRepo: Repository<Application>, IApplicationRepo
    {
        private readonly AppDbContext _context;
        public ApplicationRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Application app)
        {
            var appToUpdate = _context.Applications.FirstOrDefault(u => u.Id == app.Id);
            if(appToUpdate != null)
            {
                appToUpdate.Name = app.Name;
            }
        }
    }
}
