using System;
using System.Collections.Generic;
using OAuth2Provider.Response;
using OAuth2Provider.MessageWriters;
using NUnit.Framework;

namespace OAuth2Provider.Tests.MessageWriters
{
    [TestFixture]
    public class JsonOAuthMessageWriterTest
    {
        [Test]
        public void WritesBody()
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("test", "myval");
            parameters.Add("test2", 5);

            var writer = new JsonOAuthMessageWriter();
            var message = new OAuthResponse();
            writer.Write(message, parameters);

            Assert.IsTrue(message.Body.Contains("\"test\":\"myval\""));
            Assert.IsTrue(message.Body.Contains("\"test2\":5"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenMessageIsNull_ThenExceptionIsThrown()
        {
            var writer = new JsonOAuthMessageWriter();
            writer.Write(null, new Dictionary<string, object>());
        }

        [Test]
        public void WhenParametersAreNull_ThenWriteEmptyBody()
        {
            var writer = new JsonOAuthMessageWriter();
            var message = new OAuthResponse();
            writer.Write(message, null);

            Assert.AreEqual(string.Empty, message.Body);
        }

        [Test]
        public void WhenParameterIsNull_ThenParameterIsNotWrittenToBody()
        {
            var writer = new JsonOAuthMessageWriter();
            var message = new OAuthResponse();
            writer.Write(message, new Dictionary<string, object> { { "test", null } });

            Assert.IsFalse(message.Body.Contains("test"));
        }
    }
}
