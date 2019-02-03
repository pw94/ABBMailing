using System;
using System.Linq;
using ABBMailing.Controllers;
using ABBMailing.Interfaces;
using ABBMailing.Models;
using ABBMailing.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ABBMailing.Tests.Controllers
{
    public class UnsubscribeControllerTests : MailingTestBase
    {
        public UnsubscribeControllerTests()
        {
            CreateAddress();
        }

        private void CreateAddress()
        {
            var address = new Address { Email = "user@server.com", UnsubscribeToken = Guid.NewGuid().ToString(), Subscribed = true };
            var addressTopic = new AddressTopic { Address = address, TopicId = 1 };
            _context.Addresses.Add(address);
            _context.AddressTopic.Add(addressTopic);

            _context.SaveChanges();
        }

        [Fact]
        public void Confirm_MakesAddressInactive()
        {
            var lastAddress = _context.Addresses.Last();
            var mailingService = new Mock<IMailingService>();
            mailingService.Setup(m => m.SendUnsubscribeConfirmation(lastAddress.Email));
            var controller = new UnsubscribeController(_context, mailingService.Object);

            controller.Confirm(lastAddress.UnsubscribeToken);

            Assert.Equal(false, _context.Addresses.Last().Subscribed);
        }

        [Fact]
        public void Confirm_SendsUnsubscriptionConfirmation()
        {
            var lastAddress = _context.Addresses.Last();
            var mailingService = new Mock<IMailingService>();
            mailingService.Setup(m => m.SendUnsubscribeConfirmation(lastAddress.Email));
            var controller = new UnsubscribeController(_context, mailingService.Object);

            controller.Confirm(lastAddress.UnsubscribeToken);

            mailingService.Verify(m => m.SendUnsubscribeConfirmation(lastAddress.Email));
        }

        [Fact]
        public void Confirm_RedirectsToPageWhenPassedInvalidToken()
        {
            var mailingService = new Mock<IMailingService>();
            var controller = new UnsubscribeController(_context, mailingService.Object);

            var result = controller.Confirm("Invalid token");

            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}