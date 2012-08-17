using AutoMoq;
using NUnit.Framework;
using OAuth2Provider.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider.Tests
{
    [TestFixture]
    public class HttpBasicAuthenticationTest
    {
        [Test]
        public void DecodesBasicAuthentication()
        {
            var header = Convert.ToBase64String(Encoding.ASCII.GetBytes("user:password"));

            var mocker = new AutoMoqer();
            var requestMock = mocker.GetMock<IRequest>();
            requestMock.SetupGet(x => x.Headers).Returns(new Dictionary<string, IList<string>> { {"Authorization", new List<string> { "Basic " + header }}});

            var basicAuth = new HttpBasicAuthenticationScheme(mocker.GetMock<IRequest>().Object);

            Assert.AreEqual("user", basicAuth.Username);
            Assert.AreEqual("password", basicAuth.Password);
        }
    }
}
