using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMoq;
using OAuth2Provider.Authorization;
using OAuth2Provider.Issuer;
using OAuth2Provider.Repository;
using OAuth2Provider.Request;
using Moq;
using NUnit.Framework;

namespace OAuth2Provider.Tests.Authorization
{
    [TestFixture]
    public class ClientCredentialsTokenRequestAuthorizerTest
    {
        [Test]
        public void WhenClientDoesNotExist_ThenThrowsException()
        {
            var mocker = new AutoMoqer();
            mocker.MockServiceLocator();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns<IConsumer>(null);

            var authorizer = mocker.Resolve<ClientCredentialsTokenRequestAuthorizer>();

            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidClient, ex.ErrorCode);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(ex.ErrorDescription));
            }
        }

        [Test]
        public void WhenClientSecretIsInvalid_ThenThrowsException()
        {
            var mocker = new AutoMoqer();
            mocker.MockServiceLocator();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ClientId = "clientid", Secret = "secret" });
            
            var authorizer = mocker.Resolve<ClientCredentialsTokenRequestAuthorizer>();

            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidClient, ex.ErrorCode);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(ex.ErrorDescription));
            }
        }

        [Test]
        public void ReturnsAuthorizedToken()
        {
            var mocker = new AutoMoqer();
            mocker.MockServiceLocator();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ConsumerId = 1, ClientId = "clientid", Secret = "clientsecret" });
            mocker.GetMock<IConfiguration>().Setup(x => x.AccessTokenExpirationLength).Returns(3600);
            mocker.GetMock<IOAuthServiceLocator>().Setup(x => x.Issuer).Returns(new OAuthIssuer());

            var authorizer = mocker.Resolve<ClientCredentialsTokenRequestAuthorizer>();

            var token = authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(token);
            Assert.IsNotNull(token.AccessToken);
            Assert.AreEqual(3600, token.ExpiresIn);
            Assert.IsNotNull(token.RefreshToken);
        }
    }
}
