using System;
using ABBMailing.Persistance;
using Microsoft.EntityFrameworkCore;

namespace ABBMailing.Tests.Infrastructure
{
    public class MailingTestBase : IDisposable
    {
        protected readonly MailingContext _context;

        public MailingTestBase()
        {
            var options = new DbContextOptionsBuilder<MailingContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new MailingContext(options);

            _context.Database.EnsureCreated();

            MailingInitializer.Initialize(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();

            _context.Dispose();
        }
    }
}