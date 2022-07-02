using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MailerService.Models
{
    public class DbHelper :TextInter
    {
        public  TaskManagementContext _context; 


        private DbContextOptions<TaskManagementContext> GetAllOptions()
        {
            var options = new DbContextOptionsBuilder<TaskManagementContext>();
            options.UseSqlServer(AppSettings.ConnectionString);
            return options.Options;
        }

        public async Task<List<EmailTracker>> GeTracker()
        {
            try
            {
                using (_context=new TaskManagementContext())
                {
                    return await _context.EmailTrackers.ToListAsync();
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}
