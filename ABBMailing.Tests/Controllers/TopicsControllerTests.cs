using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ABBMailing.Controllers;
using ABBMailing.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ABBMailing.Tests.Controllers
{
    public class TopicsControllerTests : MailingTestBase
    {
        [Fact]
        public void List_ReturnsCorrectType()
        {
            var controller = new TopicsController(_context);

            var result = controller.List();

            var objectResult = Assert.IsType<JsonResult>(result);
            Assert.IsAssignableFrom<IEnumerable>(objectResult.Value);
        }

        [Fact]
        public void List_ReturnsAllTopics()
        {
            var controller = new TopicsController(_context);

            var result = controller.List();

            var objectResult = Assert.IsType<JsonResult>(result);
            var topics = Assert.IsAssignableFrom<IEnumerable<object>>(objectResult.Value);
            Assert.Equal(10, topics.Count());
        }
    }
}