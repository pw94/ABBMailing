using System.Linq;
using ABBMailing.Models;

namespace ABBMailing.Persistance
{
    public class MailingInitializer
    {
        public static void Initialize(MailingContext context)
        {
            context.Database.EnsureCreated();

            if (context.Topics.Any())
            {
                return;
            }

            Seed(context);
        }

        private static void Seed(MailingContext context)
        {
            var topicsNames = new[] {
                "Sport", "Books", "Electronics", "Toys", "Movies", "Automotive", "Clothing", "Beauty", "Games", "Garden"
            };

            var topics = topicsNames.Select(value => new Topic { Name = value });

            context.Topics.AddRange(topics);

            context.SaveChanges();
        }
    }
}