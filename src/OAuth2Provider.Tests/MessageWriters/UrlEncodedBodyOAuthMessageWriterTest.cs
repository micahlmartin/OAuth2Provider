using System;
using System.Collections.Generic;
using OAuth2Provider.Response;
using OAuth2Provider.MessageWriters;
using NUnit.Framework;

namespace OAuth2Provider.Tests.MessageWriters
{
    [TestFixture]
    public class UrlEncodedBodyOAuthMessageWriterTest
    {
        [Test]
        public void WritesBody()
        {
            var writer = new UrlEncodedBodyOAuthMessageWriter();
            var message = new OAuthResponse();

            var parameters = new Dictionary<string, object> { { "param1", "12345" }, { "param2", 5678 } };
            writer.Write(message, parameters);

            Assert.AreEqual("param1=12345&param2=5678", message.Body);

            parameters = new Dictionary<string, object> { { "p aram1", "1 2345" }, { "p aram2", 5678 } };
            writer.Write(message, parameters);

            Assert.AreEqual("p+aram1=1+2345&p+aram2=5678", message.Body);            
        }

        [Test]
        public void DoesNotWriteEmptyParameters()
        {
            var writer = new UrlEncodedBodyOAuthMessageWriter();
            var message = new OAuthResponse();

            var parameters = new Dictionary<string, object> { { "param1", null }, { "param2", "" } };
            writer.Write(message, parameters);

            Assert.AreEqual(string.Empty, message.Body);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IfMessageIsNull_ThenThrowsException()
        {
            var writer = new UrlEncodedBodyOAuthMessageWriter();
            writer.Write(null, new Dictionary<string, object>());
        }

        [Test]
        public void WhenParametersIsNull_ThenNothingIsWritten()
        {
            var writer = new UrlEncodedBodyOAuthMessageWriter();

            var message = new OAuthResponse();

            writer.Write(message, null);

            Assert.AreEqual("", message.Body);
        }
    }
}
