using System.Collections.Generic;
using System.Linq;
using ABBMailing.Controllers;
using ABBMailing.Interfaces;
using ABBMailing.Models;
using ABBMailing.Tests.Infrastructure;
using ABBMailing.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Xunit;

namespace ABBMailing.Tests.Controllers
{
    public class AddressesControllerTests : MailingTestBase
    {
        private readonly AddressesController _controller;
        private readonly Mock<IMailingService> _mailingService;

        public AddressesControllerTests()
        {
            _mailingService = new Mock<IMailingService>();
            MockMailingService();
            _controller = new AddressesController(_context, _mailingService.Object);
            MockHttpContext();
            MockUrlHelper();
        }

        private void MockMailingService()
        {
            _mailingService.Setup(m => m.SendTopicsMail(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>()));
        }
        private void MockHttpContext()
        {
            var request = new Mock<HttpRequest>();
            request.Setup(r => r.Scheme).Returns("https");
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(c => c.Request).Returns(request.Object);

            _controller.ControllerContext.HttpContext = httpContext.Object;
        }

        private void MockUrlHelper()
        {
            var urlHelper = new Mock<IUrlHelper>();
            var unsubscribeAddress = "https://localhost:5001/api/Unsubscribe/Confirm/19b07e18-20d9-4039-a43e-a8a44c250451";
            urlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns(unsubscribeAddress);

            _controller.Url = urlHelper.Object;
        }

        [Fact]
        public void Create_ReturnsCorrectResult()
        {
            var vm = new AddressViewModel { Email = "test@test.com", Topics = new[] { 1, 2 } };

            var result = _controller.Create(vm);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Create_SendsConfirmationMail()
        {
            const string email = "test@test.com";
            var vm = new AddressViewModel { Email = email, Topics = new[] { 1, 2 } };

            _controller.Create(vm);

            _mailingService.Verify(m => m.SendTopicsMail(email, It.IsAny<IEnumerable<string>>(), It.IsAny<string>()));
        }

        [Fact]
        public void Create_ReturnsBadRequestObjectResult_WhenModelStateIsInvalid()
        {
            var vm = new AddressViewModel { Email = "test.invalid.com", Topics = new[] { 1, 2 } };
            _controller.ModelState.AddModelError("Email", "Email address format is invalid");

            var result = _controller.Create(vm);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Create_ReturnsCorrectResult_WhenEmailAddressAlreadyExists()
        {
            const string email = "test@server.com";
            CreateAddress(email);
            var vm = new AddressViewModel { Email = email, Topics = new[] { 1, 2 } };

            var result = _controller.Create(vm);

            Assert.IsType<OkResult>(result);
        }

        private void CreateAddress(string email)
        {
            var address = new Address { Email = email, Subscribed = true, UnsubscribeToken = "test" };
            _context.Addresses.Add(address);
            _context.SaveChanges();
        }

        [Fact]
        public void Create_DoesNotSaveNewAddress_WhenEmailAddressAlreadyExists()
        {
            const string email = "test@server.com";
            CreateAddress(email);
            var vm = new AddressViewModel { Email = email, Topics = new[] { 1, 2 } };
            var addresesCount = _context.Addresses.Count();

            _controller.Create(vm);

            Assert.Equal(addresesCount, _context.Addresses.Count());
        }

        [Fact]
        public void Create_DoesNotSendConfirmationMail_WhenEmailAddressAlreadyExists()
        {
            const string email = "test@server.com";
            CreateAddress(email);
            var vm = new AddressViewModel { Email = email, Topics = new[] { 1, 2 } };

            _controller.Create(vm);

            _mailingService.Verify(
                m => m.SendTopicsMail(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>()),
                Times.Never());
        }
    }
}