using System;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using VilaStella.Data;
using VilaStella.Models;
using VilaStella.WebAdminClient.Models;

namespace VilaStella.WebAdminClient
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<DefaultDbContext>()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is: {0}"
            });

            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });

            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public SmtpClient EmailClient { get; private set; }

        public EmailService()
        {
            this.EmailClient = new SmtpClient("vilastella.com");
            this.EmailClient.UseDefaultCredentials = false;
            this.EmailClient.Credentials = new NetworkCredential("noreply@vilastella.com", "vilastellabot");
            this.EmailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        public Task SendAsync(IdentityMessage message)
        {
            MailMessage generatedEmail = GenerateMailMessage(message);

            try
            {
                EmailClient.Send(generatedEmail);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private MailMessage GenerateMailMessage(IdentityMessage message)
        {
            MailMessage email = new MailMessage("noreply@vilastella.com", message.Destination, message.Subject, message.Body);

            email.IsBodyHtml = true;

            return email;
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
