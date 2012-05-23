using System;
using System.Collections.Generic;
using OAuth2Provider.Response;
using OAuth2Provider.MessageWriters;
using NUnit.Framework;

namespace OAuth2Provider.Tests.MessageWriters
{
    [TestFixture]
    public class QueryStringOAuthMessageWriterTest
    {
        [Test]
        public void WritesLocation()
        {
            var writer = new QueryStringOAuthMessageWriter();

            var message = new OAuthResponse { LocationUri = "http://mydomain.com" };
            var parameters = new Dictionary<string, object> { { "param1", "12345" }, { "param2", 5678 } };

            writer.Write(message, parameters);

            Assert.AreEqual("http://mydomain.com?param1=12345&param2=5678", message.LocationUri);

            message = new OAuthResponse { LocationUri = "http://mydomain.com?test=789" };
            parameters = new Dictionary<string, object> { { "param1", "12345" }, { "param2", 5678 } };

            writer.Write(message, parameters);

            Assert.AreEqual("http://mydomain.com?test=789&param1=12345&param2=5678", message.LocationUri);

            message = new OAuthResponse { LocationUri = "http://mydomain.com?test=789" };
            parameters = new Dictionary<string, object> { { "p aram1", "1 2345" }, { "p aram2", 5678 } };

            writer.Write(message, parameters);

            Assert.AreEqual("http://mydomain.com?test=789&p+aram1=1+2345&p+aram2=5678", message.LocationUri);


            message = new OAuthResponse { LocationUri = "http://mydomain.com?test=789" };
            parameters = new Dictionary<string, object>();

            writer.Write(message, parameters);

            Assert.AreEqual("http://mydomain.com?test=789", message.LocationUri);
        }

        [Test]
        public void DoesNotWriteEmptyParameters()
        {
            var writer = new QueryStringOAuthMessageWriter();

            var message = new OAuthResponse { LocationUri = "http://mydomain.com" };
            var parameters = new Dictionary<string, object> { { "param1", null }, { "param2", "" } };

            writer.Write(message, parameters);

            Assert.AreEqual("http://mydomain.com", message.LocationUri);

            message = new OAuthResponse { LocationUri = "http://mydomain.com?test=234" };
            parameters = new Dictionary<string, object> { { "param1", null }, { "param2", "" } };

            writer.Write(message, parameters);

            Assert.AreEqual("http://mydomain.com?test=234", message.LocationUri);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IfMessageIsNull_ThenThrowsException()
        {
            var writer = new QueryStringOAuthMessageWriter();
            writer.Write(null, new Dictionary<string, object>());
        }

        [Test]
        public void WhenParametersIsNull_ThenNothingIsWritten()
        {
            var writer = new QueryStringOAuthMessageWriter();

            var message = new OAuthResponse { LocationUri = "http://mydomain.com" };

            writer.Write(message, null);

            Assert.AreEqual("http://mydomain.com", message.LocationUri);
        }
    }
}
