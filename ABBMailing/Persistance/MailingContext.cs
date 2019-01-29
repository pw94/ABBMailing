using ABBMailing.Models;
using Microsoft.EntityFrameworkCore;

namespace ABBMailing.Persistance
{
    public class MailingContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<AddressTopic> AddressTopic { get; set; }

        public MailingContext() { }

        public MailingContext(DbContextOptions<MailingContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().HasIndex(a => a.Email).IsUnique();
            modelBuilder.Entity<Address>().Property(a => a.Subscribed).HasDefaultValue(true);
            modelBuilder.Entity<Address>().HasIndex(a => a.UnsubscribeToken).IsUnique();

            modelBuilder.Entity<AddressTopic>().HasKey(at => at.Id);
            modelBuilder.Entity<AddressTopic>().HasOne(at => at.Address).WithMany(a => a.AddressTopics).HasForeignKey(at => at.AddressId);
            modelBuilder.Entity<AddressTopic>().HasOne(at => at.Topic).WithMany(t => t.AddressTopics).HasForeignKey(at => at.TopicId);
        }
    }
}