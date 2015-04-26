using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VilaStella.Models;
using VilaStella.Web.Common.Classes;

namespace VilaStella.Web.Common.Helpers
{
    public class RandomReservationGenerator : IRandomReservationGenerator
    {
        private const int PHONE_NUMBER_LENGTH = 8;
        private readonly DateTime START_DATE = new DateTime(2015, 1, 1);

        private Random randomGenerator = new Random();
        private DateTime currentStartDate;

        public RandomReservationGenerator()
        {
            this.currentStartDate = this.START_DATE;
        }

        private readonly string[] emailExtensions = new string[] 
        {
            ".com", ".bg", ".eu", ".net", ".org"
        };

        private readonly char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

        public IEnumerable<Reservation> Generate(int count)
        {
            List<Reservation> reservations = new List<Reservation>();

            for (int i = 0; i < count; i++)
            {
                reservations.Add(this.MakeRandomReservation());
            }

            return reservations;
        }

        private DateTime MakeRandomDate()
        {
            var startDate = this.currentStartDate;
            startDate = startDate.AddDays(this.randomGenerator.Next(0, 7));

            this.currentStartDate = startDate;
            return startDate;
        }

        private DateTime MakeRandomDate(DateTime comparer)
        {
            var randomDate = this.MakeRandomDate();

            while (randomDate < comparer)
            {
                randomDate = this.MakeRandomDate();
            }

            return randomDate;
        }

        private string Email()
        {
            int nameLength = this.randomGenerator.Next(5, 11);
            int domainLength = this.randomGenerator.Next(5, 11);
            int extensionIndex = this.randomGenerator.Next(0, this.emailExtensions.Length);

            string name = this.RandomString(nameLength);
            string domain = this.RandomString(domainLength);
            string extension = this.emailExtensions[extensionIndex];

            return name + "@" + domain + extension;
        }

        private string RandomPhoneNumber(int stringLength)
        {
            StringBuilder randomPhoneNumber = new StringBuilder();

            for (int i = 0; i < stringLength; i++)
            {
                int digit = this.randomGenerator.Next(0, 10);
                randomPhoneNumber.Append(digit);
            }

            return randomPhoneNumber.ToString();
        }

        private string RandomString(int stringLength)
        {
            StringBuilder randomString = new StringBuilder();

            for (int i = 0; i < stringLength; i++)
            {
                int randomCharNumber = this.randomGenerator.Next(0, alphabet.Length);
                char character = alphabet[randomCharNumber];
                randomString.Append(character);
            }

            return randomString.ToString();
        }

        private Reservation MakeRandomReservation()
        {
            int firstNameLength = this.randomGenerator.Next(3, 11);
            int lastNameLength = this.randomGenerator.Next(3, 11);

            string firsName = this.RandomString(firstNameLength);
            string lastName = this.RandomString(lastNameLength);
            string phoneNumber = this.RandomPhoneNumber(PHONE_NUMBER_LENGTH);

            string email = this.Email();

            var fromDate = this.MakeRandomDate();
            var toDate = this.MakeRandomDate(fromDate);

            int partySize = this.randomGenerator.Next(5, 21);

            var reservation = new Reservation()
            {
                FirstName = firsName,
                LastName = lastName,
                Phone = phoneNumber,
                Email = email,
                From = fromDate,
                To = toDate,
                PartySize = partySize,
                Status = Status.Pending
            };

            return reservation;
        }
    }
}
