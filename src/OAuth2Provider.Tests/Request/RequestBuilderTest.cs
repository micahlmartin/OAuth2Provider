using System.Linq;
using OAuth2Provider.Request;
using NUnit.Framework;

namespace OAuth2Provider.Tests.Request
{
    [TestFixture]
    public class RequestBuilderTest
    {
        [Test]
        public void BuildJsonTest()
        {
            var message = new RequestBuilder()
                .SetMethod(HttpMethod.Post)
                .SetHeader("header", "val1")
                .SetLocation("http://me.com")
                .SetParam("param1","val")
                .SetScope("scope")
                .BuildJsonMessage();

            Assert.AreEqual(ContentType.Json, message.ContentType);
            Assert.AreEqual(1, message.Headers.Count);
            Assert.AreEqual("header", message.Headers.ElementAt(0).Key);
            Assert.AreEqual("val1", message.Headers.ElementAt(0).Value);
            Assert.AreEqual("http://me.com", message.LocationUri);
            Assert.IsTrue(message.Body.Contains("\"param1\":\"val\""));
            Assert.IsTrue(message.Body.Contains("\"" + OAuthTokens.Scope + "\":\"scope\""));
            Assert.AreEqual(HttpMethod.Post, message.Method);
        }

        [Test]
        public void BuildBodyTest()
        {
            var message = new RequestBuilder()
                .SetMethod(HttpMethod.Post)
                .SetHeader("header", "val1")
                .SetLocation("http://me.com")
                .SetParam("param1", "val")
                .SetScope("scope")
                .BuildBodyMessage();

            Assert.AreEqual(ContentType.FormEncoded, message.ContentType);
            Assert.AreEqual(1, message.Headers.Count);
            Assert.AreEqual("header", message.Headers.ElementAt(0).Key);
            Assert.AreEqual("val1", message.Headers.ElementAt(0).Value);
            Assert.AreEqual("http://me.com", message.LocationUri);
            Assert.AreEqual("param1=val&scope=scope", message.Body);
            Assert.AreEqual(HttpMethod.Post, message.Method);
        }

        [Test]
        public void BuildQueryTest()
        {
            var message = new RequestBuilder()
                .SetMethod(HttpMethod.Post)
                .SetHeader("header", "val1")
                .SetLocation("http://me.com")
                .SetParam("param1", "val")
                .SetScope("scope")
                .BuildQueryMessage();

            Assert.AreEqual(null, message.ContentType);
            Assert.AreEqual(1, message.Headers.Count);
            Assert.AreEqual("header", message.Headers.ElementAt(0).Key);
            Assert.AreEqual("val1", message.Headers.ElementAt(0).Value);
            Assert.AreEqual("http://me.com?param1=val&scope=scope", message.LocationUri);
            Assert.AreEqual(HttpMethod.Post, message.Method);
        }
    }
}
