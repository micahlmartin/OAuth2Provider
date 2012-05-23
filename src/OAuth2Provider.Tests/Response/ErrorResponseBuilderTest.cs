using OAuth2Provider.Response;
using System.Web;
using NUnit.Framework;

namespace OAuth2Provider.Tests.Response
{
    [TestFixture]
    public class ErrorResponseBuilderTest
    {
        [Test]
        public void BuildJsonTest()
        {
            var message = new ErrorResponseBuilder()
                .SetErrorCode(ErrorCode.InvalidClient)
                .SetErrorDescription("Bad Client")
                .SetErrorUri("http://error.com")
                .SetLocation("http://mydomain.com")
                .SetState("mystate")
                .BuildJsonMessage();

            Assert.AreEqual(400, message.StatusCode);
            Assert.AreEqual("http://mydomain.com", message.LocationUri);
            Assert.IsTrue(message.Body.Contains("\"" + OAuthTokens.State + "\":\"mystate\""));
            Assert.IsTrue(message.Body.Contains("\"" + OAuthTokens.Error + "\":\"" + ErrorCode.InvalidClient + "\""));
            Assert.IsTrue(message.Body.Contains("\"" + OAuthTokens.ErrorUri + "\":\"http://error.com\""));
            Assert.IsTrue(message.Body.Contains("\"" + OAuthTokens.ErrorDescription + "\":\"Bad Client\""));
            Assert.AreEqual(ContentType.Json, message.ContentType);

            var ex = new OAuthException(ErrorCode.InvalidClient, "Bad Client", "http://error.com", "mystate");
            message = new ErrorResponseBuilder(ex).SetLocation("http://mydomain.com").BuildJsonMessage();

            Assert.AreEqual(400, message.StatusCode);
            Assert.AreEqual("http://mydomain.com", message.LocationUri);
            Assert.IsTrue(message.Body.Contains("\"" + OAuthTokens.State + "\":\"mystate\""));
            Assert.IsTrue(message.Body.Contains("\"" + OAuthTokens.Error + "\":\"" + ErrorCode.InvalidClient + "\""));
            Assert.IsTrue(message.Body.Contains("\"" + OAuthTokens.ErrorUri + "\":\"http://error.com\""));
            Assert.IsTrue(message.Body.Contains("\"" + OAuthTokens.ErrorDescription + "\":\"Bad Client\""));
            Assert.AreEqual(ContentType.Json, message.ContentType);
        }

        [Test]
        public void BuildQueryTest()
        {
            var message = new ErrorResponseBuilder()
                .SetErrorCode(ErrorCode.InvalidClient)
                .SetErrorDescription("Bad Client")
                .SetErrorUri("http://error.com")
                .SetLocation("http://mydomain.com")
                .SetState("mystate")
                .BuildQueryMessage();

            var expected = string.Format("http://mydomain.com?{0}={1}&{2}={3}&{4}={5}&{6}={7}", 
                OAuthTokens.Error, ErrorCode.InvalidClient, 
                OAuthTokens.ErrorDescription, HttpUtility.UrlEncode("Bad Client"), 
                OAuthTokens.ErrorUri, HttpUtility.UrlEncode("http://error.com"), 
                OAuthTokens.State, "mystate");
            Assert.AreEqual(400, message.StatusCode);
            Assert.AreEqual(expected, message.LocationUri);

            var ex = new OAuthException(ErrorCode.InvalidClient, "Bad Client", "http://error.com", "mystate");
            message = new ErrorResponseBuilder(ex).SetLocation("http://mydomain.com").BuildQueryMessage();

            expected = string.Format("http://mydomain.com?{0}={1}&{2}={3}&{4}={5}&{6}={7}",
                OAuthTokens.Error, ErrorCode.InvalidClient,
                OAuthTokens.ErrorDescription, HttpUtility.UrlEncode("Bad Client"),
                OAuthTokens.ErrorUri, HttpUtility.UrlEncode("http://error.com"),
                OAuthTokens.State, "mystate");
            Assert.AreEqual(400, message.StatusCode);
            Assert.AreEqual(expected, message.LocationUri);
        }

        [Test]
        public void BuildBodyTest()
        {
            var message = new ErrorResponseBuilder()
                .SetErrorCode(ErrorCode.InvalidClient)
                .SetErrorDescription("Bad Client")
                .SetErrorUri("http://error.com")
                .SetLocation("http://mydomain.com")
                .SetState("mystate")
                .BuildBodyMessage();

            var expected = string.Format("{0}={1}&{2}={3}&{4}={5}&{6}={7}",
                OAuthTokens.Error, ErrorCode.InvalidClient,
                OAuthTokens.ErrorDescription, HttpUtility.UrlEncode("Bad Client"),  
                OAuthTokens.ErrorUri, HttpUtility.UrlEncode("http://error.com"),
                OAuthTokens.State, "mystate");
            Assert.AreEqual(400, message.StatusCode);
            Assert.AreEqual(expected, message.Body);
            Assert.AreEqual(ContentType.FormEncoded, message.ContentType);

            var ex = new OAuthException(ErrorCode.InvalidClient, "Bad Client", "http://error.com", "mystate");
            message = new ErrorResponseBuilder(ex).SetLocation("http://mydomain.com").BuildBodyMessage();

            expected = string.Format("{0}={1}&{2}={3}&{4}={5}&{6}={7}",
                OAuthTokens.Error, ErrorCode.InvalidClient,
                OAuthTokens.ErrorDescription, HttpUtility.UrlEncode("Bad Client"),
                OAuthTokens.ErrorUri, HttpUtility.UrlEncode("http://error.com"),
                OAuthTokens.State, "mystate");
            Assert.AreEqual(400, message.StatusCode);
            Assert.AreEqual(expected, message.Body);
            Assert.AreEqual(ContentType.FormEncoded, message.ContentType);
        }
    }
}
