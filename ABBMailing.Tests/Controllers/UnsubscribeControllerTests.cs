using System;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task Confirm_MakesAddressInactive()
        {
            var lastAddress = _context.Addresses.Last();
            var mock = new Mock<IMailingService>();
            mock.Setup(m => m.SendUnsubscribeConfirmation(lastAddress.Email)).Returns(Task.CompletedTask);
            var controller = new UnsubscribeController(_context, mock.Object);

            await controller.Confirm(lastAddress.UnsubscribeToken);

            Assert.Equal(false, _context.Addresses.Last().Subscribed);
        }

        [Fact]
        public async Task Confirm_SendsUnsubscriptionConfirmation()
        {
            var lastAddress = _context.Addresses.Last();
            var mock = new Mock<IMailingService>();
            mock.Setup(m => m.SendUnsubscribeConfirmation(lastAddress.Email)).Returns(Task.CompletedTask);
            var controller = new UnsubscribeController(_context, mock.Object);

            await controller.Confirm(lastAddress.UnsubscribeToken);

            mock.Verify(m => m.SendUnsubscribeConfirmation(lastAddress.Email));
        }

        [Fact]
        public async Task Confirm_RedirectsToPageWhenPassedInvalidToken()
        {
            var mock = new Mock<IMailingService>();
            var controller = new UnsubscribeController(_context, mock.Object);

            var result = await controller.Confirm("Invalid token");

            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}