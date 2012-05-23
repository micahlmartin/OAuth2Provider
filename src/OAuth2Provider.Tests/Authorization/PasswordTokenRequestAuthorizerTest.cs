using AutoMoq;
using OAuth2Provider.Authorization;
using OAuth2Provider.Request;
using OAuth2Provider.Repository;
using OAuth2Provider.Issuer;
using Moq;
using NUnit.Framework;

namespace OAuth2Provider.Tests.Authorization
{
    [TestFixture]
    public class PasswordTokenRequestAuthorizerTest
    {
        [Test]
        public void WhenClientIdIsInvalid_ThenThrowsException()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("username");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("password");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.Password);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns<ConsumerImpl>(null);

            var authorizer = mocker.Resolve<PasswordTokenRequestAuthorizer>();
            
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
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("username");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("password");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.Password);
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ClientId = "clientid", Secret = "secret" });
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");

            var authorizer = mocker.Resolve<PasswordTokenRequestAuthorizer>();           

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
        public void WhenUsernameIsInvalid_ThenThrowsException()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("password");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.Password);
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ClientId = "clientid", Secret = "clientsecret" });
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("username");
            mocker.GetMock<IResourceOwnerRepository>().Setup(x => x.GetByUsername(1, "user")).Returns<ResourceOwnerImpl>(null);

            var authorizer = mocker.Resolve<PasswordTokenRequestAuthorizer>();

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
        public void WhenPasswordIsInvalid_ThenThrowsException()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("username");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.Password);
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ClientId = "clientid", Secret = "clientsecret" });
            mocker.GetMock<IResourceOwnerRepository>().Setup(x => x.GetByUsername(1, "username")).Returns(new ResourceOwnerImpl { Username = "username", Password = "pass" });
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("password");

            var authorizer = mocker.Resolve<PasswordTokenRequestAuthorizer>();

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
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("username");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.Password);
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ClientId = "clientid", Secret = "clientsecret" });
            mocker.GetMock<IResourceOwnerRepository>().Setup(x => x.GetByUsername(1, "username")).Returns(new ResourceOwnerImpl { Username = "username", Password = "password" });
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("password");

            var authorizer = mocker.Resolve<PasswordTokenRequestAuthorizer>();

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
        public void EnsureApplicationIsApproved()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("username");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.Password);
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ConsumerId = 1, ClientId = "clientid", Secret = "clientsecret" });
            mocker.GetMock<IResourceOwnerRepository>().Setup(x => x.GetByUsername(1, "username")).Returns(new ResourceOwnerImpl { ResourceOwnerId = 2, Username = "username", Password = "password".ToHash() });
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("password");
            mocker.SetInstance<IOAuthIssuer>(new OAuthIssuer());

            var authorizer = mocker.Resolve<PasswordTokenRequestAuthorizer>();

            var token = authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);

            mocker.GetMock<IResourceOwnerRepository>().Verify(x => x.ApproveConsumer(2, 1), Times.Once());
        }

        [Test]
        public void ReturnsAuthorizedToken()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("username");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.Password);
            mocker.GetMock<IConsumerRepository>().Setup(x => x.GetByClientId("clientid")).Returns(new ConsumerImpl { ConsumerId = 1, ClientId = "clientid", Secret = "clientsecret" });
            mocker.GetMock<IResourceOwnerRepository>().Setup(x => x.GetByUsername(1, "username")).Returns(new ResourceOwnerImpl { ResourceOwnerId = 2, Username = "username", Password = "password".ToHash() });
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("password");
            mocker.GetMock<IConfiguration>().Setup(x => x.AccessTokenExpirationLength).Returns(3600);
            mocker.SetInstance<IOAuthIssuer>(new OAuthIssuer());

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
