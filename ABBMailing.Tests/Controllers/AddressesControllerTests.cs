using System.Collections.Generic;
using System.Threading.Tasks;
using ABBMailing.Controllers;
using ABBMailing.Interfaces;
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

        public AddressesControllerTests()
        {
            var mock = new Mock<IMailingService>();
            mock.Setup(m => m.SendTopicsMail(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _controller = new AddressesController(_context, mock.Object);
            var request = new Mock<HttpRequest>();
            request.Setup(r => r.Scheme).Returns("https");
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(c => c.Request).Returns(request.Object);
            _controller.ControllerContext.HttpContext = httpContext.Object;
            Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
            var unsubscribeAddress = "https://localhost:5001/api/Unsubscribe/Confirm/19b07e18-20d9-4039-a43e-a8a44c250451";
            urlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns(unsubscribeAddress);
            _controller.Url = urlHelper.Object;
        }
        
        [Fact]
        public async Task Create_ReturnsCorrectResult()
        {
            var vm = new AddressViewModel { Email = "test@test.com", Topics = new[] { 1, 2 } };

            var result = await _controller.Create(vm);

            Assert.IsType<OkResult>(result);
        }
    }
}