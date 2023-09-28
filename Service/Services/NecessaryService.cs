using AutoMapper;
using Entities;
using Entities.DomainEntities;
using Interface.Services;
using Interface.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Request.DomainRequests;
using Service.Services.DomainServices;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Service.Services
{
    public class NecessaryService : DomainService<tbl_Necessary, BaseSearch>, INecessaryService
    {
        private IConfiguration configuration;
        public NecessaryService(IAppUnitOfWork unitOfWork,
            IConfiguration configuration,
            IMapper mapper) : base(unitOfWork, mapper)
        {
            this.configuration = configuration;
        }
        public async Task SendMail(SendMailModel model)
        {
            try
            {
                await Task.Run(() =>
                {
                    string fromAddress = configuration.GetSection("MySettings:Email").Value.ToString();
                    string mailPassword = configuration.GetSection("MySettings:PasswordMail").Value.ToString(); 
                    SmtpClient client = new SmtpClient();
                    client.Port = 587;//outgoing port for the mail.
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    client.Timeout = 1000000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(fromAddress, mailPassword);

                    // Fill the mail form.
                    var send_mail = new MailMessage();
                    send_mail.IsBodyHtml = true;
                    //address from where mail will be sent.
                    send_mail.From = new MailAddress(fromAddress);
                    //address to which mail will be sent.           
                    send_mail.To.Add(new MailAddress(model.to));
                    //subject of the mail.
                    send_mail.Subject = model.title;
                    send_mail.Body = model.content;
                    client.Send(send_mail);
                });
            }
            catch {}
        }
    }
}
