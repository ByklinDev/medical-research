using MedicalResearch.Domain.DTO;
using MedicalResearch.Domain.Interfaces.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MedicalResearch.Domain.Configurations;
using Microsoft.Extensions.Options;

namespace MedicalResearch.Domain.Services;

public class EmailService: IEmailService
{

    private readonly  EmailConfiguration _emailConfiguration;

    public EmailService(IOptions<EmailConfiguration> configuration)
    {
        _emailConfiguration = configuration.Value;
    }
    public async Task SendAsync(EmailCreateDTO emailCreateDTO)
    {
        SmtpClient smtpClient = new SmtpClient(_emailConfiguration.SmtpServer, Convert.ToInt32(_emailConfiguration.Port)); 

        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new System.Net.NetworkCredential(_emailConfiguration.CompanyEmail, _emailConfiguration.CompanyEmailPassword);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;
        smtpClient.Timeout = 5000;

        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(_emailConfiguration.CompanyEmail, "Medical Research Company");
        mail.To.Add(new MailAddress(emailCreateDTO.Email));
        mail.Subject = emailCreateDTO.Topic;

        StringBuilder message = new StringBuilder();
        message.AppendLine($"My name is {emailCreateDTO.Name}");
        message.AppendLine(emailCreateDTO.Message);
        message.AppendLine($"My phone number: {emailCreateDTO.Phone}");
        message.AppendLine($"My address: {emailCreateDTO.Address}");
        mail.Body = message.ToString();

        await smtpClient.SendMailAsync(mail);
        smtpClient.Dispose();
    }
}
