using System.Net;
using System.Net.Mail;
using MailerService.Models;

namespace MailerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        //private readonly TaskManagementContext _dbContext;
        // private readonly TextInter _text=new DbHelper();
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service Started at: {time}", DateTimeOffset.Now);
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service Started at: {time}", DateTimeOffset.Now);
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(Mailer() ? "Worker running at: {time}" : "Worker stopped at: {time}",
                    DateTimeOffset.Now);

                await Task.Delay(1000, stoppingToken);
            }
        }

        private  bool Mailer()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<TaskManagementContext>();
                var model = dbContext.EmailTrackers.ToList();

                foreach (var mail in model)
                {
                    if (mail.Statues)
                    {
                        continue;
                    }
                    mail.SendingDate = DateTime.Now;
                    MailSend(mail);
                    _logger.LogInformation("Mail Send at: {time}", DateTimeOffset.Now);
                    _logger.LogInformation("Mail Send To:" + mail.ReceiverId.ToString());
                    
                    mail.Statues = true;
                    dbContext.EmailTrackers.Update(mail);
                    dbContext.SaveChanges();


                }
                
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Mail Send Error:" + ex.Message);
            }
           
           
            return true;
        }

        private static void MailSend(EmailTracker mail1)
        {
            try
            {
                const string smtpAddress = "192.168.1.254";
                const int portNumber = 465;
                const bool enableSsl = false;

                const string subject = "Hello";

                using var mail = new MailMessage();
                mail.From = new MailAddress(mail1.SenderEmailAddress);
                mail.To.Add(mail1.RecieverEmailAddress);
                mail.CC.Add(mail1.SenderEmailAddress);
                mail.Subject = subject;
                mail.Body = mail1.EmailMessage;
                mail.IsBodyHtml = true;
                using var smtp = new SmtpClient(smtpAddress, portNumber);
                smtp.Credentials = new NetworkCredential("itsupport", "it#$s*p7");
                smtp.EnableSsl = enableSsl;
                smtp.Send(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }
    }
}