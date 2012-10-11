using AutoMoq;
using OAuth2Provider.Request;
using OAuth2Provider.Repository;
using OAuth2Provider.Authorization;
using OAuth2Provider.Issuer;
using Moq;
using NUnit.Framework;
using System;

namespace OAuth2Provider.Tests.Authorization
{
    [TestFixture]
    public class RefreshTokenRequestAuthorizerTest
    {
        [Test]
        public void WhenClientIdIsInvalid_ThenThrowsException()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.RefreshToken);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns<ConsumerImpl>(null);

            var authorizer = mocker.Resolve<RefreshTokenRequestAuthorizer>();

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
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.RefreshToken);
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ClientId = "clientid", Secret = "secret" });
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");

            var authorizer = mocker.Resolve<RefreshTokenRequestAuthorizer>();

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
        public void WhenContentTypeIsInvalid_ThenThrowsException()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.Json);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.RefreshToken);
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ClientId = "clientid", Secret = "clientsecret" });

            var authorizer = mocker.Resolve<RefreshTokenRequestAuthorizer>();

            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidRequest, ex.ErrorCode);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(ex.ErrorDescription));
            }
        }

        [Test]
        public void WhenRefreshTokenIsMissing_ThenThrowsException()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.RefreshToken);
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ConsumerId = 12, ClientId = "clientid", Secret = "clientsecret" });
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.RefreshToken).Returns("");

            var authorizer = mocker.Resolve<RefreshTokenRequestAuthorizer>();

            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidRequest, ex.ErrorCode);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(ex.ErrorDescription));
            }
        }

        [Test]
        public void WhenRefreshTokenConsumerDoesNotMatch_ThenThrowsException()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.RefreshToken);
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ConsumerId = 12, ClientId = "clientid", Secret = "clientsecret" });
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.SetInstance<IOAuthIssuer>(new OAuthIssuer());
            var issuer = new OAuthIssuer();

            var authorizer = mocker.Resolve<RefreshTokenRequestAuthorizer>();
            
            var token = issuer.GenerateRefreshToken(new TokenData {ConsumerId = 11, ResourceOwnerId = 10, Timestamp = 1});
            mocker.GetMock<IOAuthRequest>().Setup(x => x.RefreshToken).Returns(token);

            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.UnauthorizedClient, ex.ErrorCode);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(ex.ErrorDescription));
            }
        }

        [Test]
        public void WhenConsumerIsNolongerApproved_ThenThrowsException()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.RefreshToken);
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ConsumerId = 12, ClientId = "clientid", Secret = "clientsecret" });
            mocker.GetMock<IResourceOwnerRepository>().Setup(x => x.IsConsumerApproved(10, 12)).Returns(false);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.SetInstance<IOAuthIssuer>(new OAuthIssuer());
            var issuer = new OAuthIssuer();

            var authorizer = mocker.Resolve<RefreshTokenRequestAuthorizer>();

            var token = issuer.GenerateRefreshToken(new TokenData { ConsumerId = 12, ResourceOwnerId = 10, Timestamp = 1 });
            mocker.GetMock<IOAuthRequest>().Setup(x => x.RefreshToken).Returns(token);

            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.UnauthorizedClient, ex.ErrorCode);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(ex.ErrorDescription));
            }
        }

        [Test]
        public void WhenDataIsValid_ThenNewTokenIsCreated()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.RefreshToken);
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ConsumerId = 12, ClientId = "clientid", Secret = "clientsecret" });
            mocker.GetMock<IResourceOwnerRepository>().Setup(x => x.IsConsumerApproved(10, 12)).Returns(true);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.SetInstance<IOAuthIssuer>(new OAuthIssuer());
            var issuer = new OAuthIssuer();

            var authorizer = mocker.Resolve<RefreshTokenRequestAuthorizer>();

            var token = issuer.GenerateRefreshToken(new TokenData { ConsumerId = 12, ResourceOwnerId = 10, Timestamp = 1 });
            mocker.GetMock<IOAuthRequest>().Setup(x => x.RefreshToken).Returns(token);


            var newToken = authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(newToken);

            var accessTokenData = issuer.DecodeAccessToken(newToken.AccessToken);
            Assert.IsNotNull(accessTokenData);
            Assert.AreEqual(10, accessTokenData.ResourceOwnerId);
            Assert.IsTrue(accessTokenData.Timestamp > DateTimeOffset.UtcNow.AddMinutes(-5).Ticks);

            var refreshTokenData = issuer.DecodeRefreshToken(newToken.RefreshToken);
            Assert.IsNotNull(refreshTokenData);
            Assert.AreEqual(12, refreshTokenData.ConsumerId);
            Assert.AreEqual(10, refreshTokenData.ResourceOwnerId);
            Assert.IsTrue(refreshTokenData.Timestamp > DateTimeOffset.UtcNow.AddMinutes(-5).Ticks);
        }

        [Test]
        public void ReturnsAuthorizedToken()
        {
            var mocker = new AutoMoqer();
            mocker.MockServiceLocator();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("username");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.Password);
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ConsumerId = 1, ClientId = "clientid", Secret = "clientsecret" });
            mocker.GetMock<IResourceOwnerRepository>().Setup(x => x.GetByUsername(1, "username")).Returns(new ResourceOwnerImpl { ResourceOwnerId = 2, Username = "username", Password = "password".ToHash() });
            mocker.GetMock<IPasswordHasher>().Setup(x => x.CheckPassword("password", "password".ToHash())).Returns(true);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("password");
            mocker.GetMock<IConfiguration>().Setup(x => x.AccessTokenExpirationLength).Returns(3600);
            mocker.GetMock<IOAuthServiceLocator>().Setup(x => x.Issuer).Returns(new OAuthIssuer());

            var authorizer = mocker.Resolve<PasswordTokenRequestAuthorizer>();

            var token = authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);

            mocker.GetMock<IResourceOwnerRepository>().Verify(x => x.ApproveConsumer(2, 1), Times.Once());
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.AccessToken);
            Assert.AreEqual(3600, token.ExpiresIn);
            Assert.IsNotNull(token.RefreshToken);
        }
    }
}
