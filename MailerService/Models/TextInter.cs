using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailerService.Models
{
    public interface TextInter
    {
        Task<List<EmailTracker>> GeTracker();
    }
}
