using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VilaStella.Data.Common.Repositories;
using VilaStella.Models;
using VilaStella.WebAdminClient.Infrastructure.Contracts;

namespace VilaStella.WebAdminClient.Infrastructure
{
    public class ReservationsEmailManager : IReservationsEmailManager
    {
        private const string SMTP_SERVER_HOST = "vilastella.com";
        private const string EMAIL_ADDRESS = "noreply@vilastella.com";
        private const string EMAIL_PASSWORD = "vilastellabot";

        private SmtpClient mailClient;

        private IDeletableRepository<Email> emailTemplates;

        public ReservationsEmailManager(IDeletableRepository<Email> emailTemplates)
        {
            this.emailTemplates = emailTemplates;

           this.mailClient = new SmtpClient(SMTP_SERVER_HOST);
            this.mailClient.UseDefaultCredentials = false;
            this.mailClient.Credentials = new System.Net.NetworkCredential(EMAIL_ADDRESS, EMAIL_PASSWORD);
            this.mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        public void SendConfirmationEmail(Reservation reservation)
        {
            var message = new MailMessage(EMAIL_ADDRESS, reservation.Email);
            message.IsBodyHtml = true;

            var emailTemplate = GetEmailTypeByReservation(reservation);

            message.Subject = emailTemplate.Subject;
            message.Body = emailTemplate.Body;

            this.mailClient.Send(message);
        }

        private EmailInfo GetEmailTypeByReservation(Reservation reservation)
        {
            if (reservation.PaymentMethod == PaymentMethod.BankTransaction)
            {
                return BankTransactionEmailTemplate(reservation);
            }
            else
            {
                return ByHandEmailTemplate(reservation);
            }
        }

        private EmailInfo BankTransactionEmailTemplate(Reservation reservation)
        {
            var emailTemplate = this.emailTemplates.All()
                                    .Where(x => x.Type == EmailType.BankTransaction)
                                    .ToList()
                                    .LastOrDefault();

            return ParseByTemplate(reservation, emailTemplate);
        }

        private EmailInfo ByHandEmailTemplate(Reservation reservation)
        {
            var emailTemplate = this.emailTemplates.All()
                                    .Where(x => x.Type == EmailType.ByHand)
                                    .ToList()
                                    .LastOrDefault();

            return ParseByTemplate(reservation, emailTemplate);
        }

        private EmailInfo ParseByTemplate(Reservation reservation, Email emailTemplate)
        {
            var parametersList = this.GetParametersFromReservation(reservation);
            var stringBuilderBody = new StringBuilder(emailTemplate.Body);

            for (int i = 0; i < parametersList.Count; i++)
            {
                string currentFormat = "{" + i.ToString() + "}";
                stringBuilderBody.Replace(currentFormat, parametersList[i]);
            }

            return new EmailInfo()
            {
                Body = stringBuilderBody.ToString(),
                Subject = emailTemplate.Subject
            };
        }

        private List<string> GetParametersFromReservation(Reservation reservation)
        {
            var parametersList = new List<string>()
            {
                reservation.From.ToShortDateString().ToString(),
                reservation.To.ToShortDateString().ToString(),
                reservation.Capparo.ToString("0.##"),
                reservation.FullPrice.ToString("0.##"),
                reservation.ID.ToString()
            };

            return parametersList;
        }

        public void Dispose()
        {
            this.mailClient.Dispose();
        }
    }
}