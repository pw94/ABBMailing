using System;
using System.Linq;
using System.Threading.Tasks;
using ABBMailing.Controllers;
using ABBMailing.Interfaces;
using ABBMailing.Models;
using ABBMailing.Tests.Infrastructure;
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
        public async Task Confirm_MakesAddressInactive()
        {
            var mock = new Mock<IMailingService>();
            mock.Setup(m => m.SendUnsubscribeConfirmation(It.IsAny<string>())).Returns(Task.CompletedTask);
            var controller = new UnsubscribeController(_context, mock.Object);
            var lastAddress = _context.Addresses.Last();
            var token = lastAddress.UnsubscribeToken;
            Assert.Equal(true, lastAddress.Subscribed);

            await controller.Confirm(token);

            Assert.Equal(false, _context.Addresses.Last().Subscribed);
        }
    }
}