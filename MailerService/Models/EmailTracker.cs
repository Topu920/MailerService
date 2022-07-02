using System;
using System.Collections.Generic;

namespace MailerService.Models
{
    public partial class EmailTracker
    {
        public long EmailId { get; set; }
        public Guid? TaskId { get; set; }
        public long? SenderId { get; set; }
        public long? ReceiverId { get; set; }
        public string? EmailMessage { get; set; }
        public DateTime? SendingDate { get; set; }
        public bool Statues { get; set; }
        public string SenderEmailAddress { get; set; } = null!;
        public string RecieverEmailAddress { get; set; } = null!;
    }
}
