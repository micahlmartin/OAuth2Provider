using System.Linq;
using OAuth2Provider.Response;
using NUnit.Framework;

namespace OAuth2Provider.Tests.Response
{
    [TestFixture]
    public class ResponseBuilderTest
    {
        [Test]
        public void BuildJsonTest()
        {
            var message = new ResponseBuilder()
                .SetHeader("header", "val1")
                .SetLocation("http://me.com")
                .SetParam("param1","val")
                .SetStatusCode(200)
                .BuildJsonMessage();

            Assert.AreEqual(ContentType.Json, message.ContentType);
            Assert.AreEqual(1, message.Headers.Count);
            Assert.AreEqual("header", message.Headers.ElementAt(0).Key);
            Assert.AreEqual("val1", message.Headers.ElementAt(0).Value);
            Assert.AreEqual("http://me.com", message.LocationUri);
            Assert.AreEqual(200, message.StatusCode);
            Assert.IsTrue(message.Body.Contains("\"param1\":\"val\""));
        }

        [Test]
        public void BuildBodyTest()
        {
            var message = new ResponseBuilder()
                .SetHeader("header", "val1")
                .SetLocation("http://me.com")
                .SetParam("param1", "val")
                .SetStatusCode(200)
                .BuildBodyMessage();

            Assert.AreEqual(ContentType.FormEncoded, message.ContentType);
            Assert.AreEqual(1, message.Headers.Count);
            Assert.AreEqual("header", message.Headers.ElementAt(0).Key);
            Assert.AreEqual("val1", message.Headers.ElementAt(0).Value);
            Assert.AreEqual("http://me.com", message.LocationUri);
            Assert.AreEqual(200, message.StatusCode);
            Assert.AreEqual("param1=val", message.Body);
        }

        [Test]
        public void BuildQueryTest()
        {
            var message = new ResponseBuilder()
                .SetHeader("header", "val1")
                .SetLocation("http://me.com")
                .SetParam("param1", "val")
                .SetStatusCode(200)
                .BuildQueryMessage();

            Assert.AreEqual(null, message.ContentType);
            Assert.AreEqual(1, message.Headers.Count);
            Assert.AreEqual("header", message.Headers.ElementAt(0).Key);
            Assert.AreEqual("val1", message.Headers.ElementAt(0).Value);
            Assert.AreEqual(200, message.StatusCode);
            Assert.AreEqual("http://me.com?param1=val", message.LocationUri);
        }
    }
}
