using System.Collections.Generic;
using OAuth2Provider.Request;
using System;
using System.Web;
using AutoMoq;
using NUnit.Framework;
using Moq;

namespace OAuth2Provider.Tests.Request
{
    [TestFixture]
    public class TokenRequestTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenRequestIsNull_ThenThrowException()
        {
            new TokenRequest(null, new Mock<IOAuthServiceLocator>().Object);
        }

        [Test]
        [ExpectedException(typeof(OAuthException))]
        public void WhenRequestIsNotValid_ThenThrowException()
        {
            var mocker = new AutoMoqer();
            var properties = new Dictionary<string, string>
                                 {
                                     {OAuthTokens.GrantType, "InvalidGrant"},
                                 };
            mocker.GetMock<IRequest>().Setup(x => x.Properties).Returns(properties);

            try
            {
                new TokenRequest(mocker.GetMock<IRequest>().Object, new Mock<IOAuthServiceLocator>().Object);
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.UnsupportedGrantType, ex.ErrorCode);
                Assert.IsFalse(string.IsNullOrWhiteSpace(ex.ErrorDescription));
                throw;
            }
        }

        [Test]
        public void WhenRequestIsValid_ThenAllRequiredPropertiesAreSet()
        {
            var mocker = new AutoMoqer();
            var properties = new Dictionary<string, string>
                                 {
                                     {OAuthTokens.ClientId, "clientid"},
                                     {OAuthTokens.ClientSecret, "clientsecret"},
                                     {OAuthTokens.Username, "username"},
                                     {OAuthTokens.Password, "password"},
                                     {OAuthTokens.GrantType, GrantType.Password},
                                 };
            mocker.GetMock<IRequest>().Setup(x => x.Properties).Returns(properties); 
            mocker.GetMock<IRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            mocker.GetMock<IRequest>().Setup(X => X.Headers).Returns(new Dictionary<string, string>());
            var request = new TokenRequest(mocker.GetMock<IRequest>().Object, mocker.GetMock<IOAuthServiceLocator>().Object);

            Assert.AreEqual("clientid", request.ClientId);
            Assert.AreEqual("clientsecret", request.ClientSecret);
            Assert.AreEqual("username", request.Username);
            Assert.AreEqual("password", request.Password);
            Assert.AreEqual(GrantType.Password, request.GrantType);
            Assert.AreEqual(ContentType.FormEncoded, request.ContentType);
        }
    }
}
