using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth2Provider.Authorization;
using OAuth2Provider.Issuer;
using OAuth2Provider.Request;
using AutoMoq;
using NUnit.Framework;

namespace OAuth2Provider.Tests.Authorization
{
    [TestFixture]
    public class AuthorizationCodeAuthorizerTest
    {
        [Test]
        public void RequiresAuthorizationCodeGrantType()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns<string>(null);

            var authorizer = mocker.Resolve<AuthorizationCodeAuthorizer>();

            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown.");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidRequest, ex.ErrorCode);
                Assert.IsTrue(ex.ErrorDescription.HasValue());
            }

            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns("");

            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown.");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidRequest, ex.ErrorCode);
                Assert.IsTrue(ex.ErrorDescription.HasValue());
            }

            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns("   ");

            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown.");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidRequest, ex.ErrorCode);
                Assert.IsTrue(ex.ErrorDescription.HasValue());
            }

            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns("asdf");

            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown.");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidGrant, ex.ErrorCode);
                Assert.IsTrue(ex.ErrorDescription.HasValue());
            }
        }

        [Test]
        public void RequiresAuthorizationCode()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.AuthorizationCode);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.AuthorizationCode).Returns<string>(null);

            var authorizer = mocker.Resolve<AuthorizationCodeAuthorizer>();

            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown.");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidRequest, ex.ErrorCode);
                Assert.IsTrue(ex.ErrorDescription.HasValue());
            }

            mocker.GetMock<IOAuthRequest>().Setup(x => x.AuthorizationCode).Returns("");

            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown.");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidRequest, ex.ErrorCode);
                Assert.IsTrue(ex.ErrorDescription.HasValue());
            }

            mocker.GetMock<IOAuthRequest>().Setup(x => x.AuthorizationCode).Returns("   ");

            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown.");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidRequest, ex.ErrorCode);
                Assert.IsTrue(ex.ErrorDescription.HasValue());
            }
        }

        [Test]
        public void WhenAuthorizationCodeHasExpired_ThenThrowException()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.AuthorizationCode);
            mocker.GetMock<IConfiguration>().Setup(x => x.AuthorizationTokenExpirationLength).Returns(300);
            mocker.SetInstance<IOAuthIssuer>(new OAuthIssuer());
            var issuer = new OAuthIssuer();
            var token = issuer.GenerateAuthorizationToken(new TokenData { ConsumerId = 1, Timestamp = DateTime.UtcNow.AddHours(-1).Ticks });
            mocker.GetMock<IOAuthRequest>().Setup(x => x.AuthorizationCode).Returns(token);

            var authorizer = mocker.Resolve<AuthorizationCodeAuthorizer>();

            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidRequest, ex.ErrorCode);
                Assert.IsTrue(ex.ErrorDescription.HasValue());
            }
        }

        [Test]
        public void WhenRedirectUriDoesNotMatch_ThenExceptionIsThrown()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.AuthorizationCode);
            mocker.GetMock<IConfiguration>().Setup(x => x.AuthorizationTokenExpirationLength).Returns(300);
            mocker.GetMock<IConfiguration>().Setup(x => x.AccessTokenExpirationLength).Returns(500);
            var issuer = new OAuthIssuer();
            mocker.SetInstance<IOAuthIssuer>(issuer);
            var token = issuer.GenerateAuthorizationToken(new TokenData { ConsumerId = 1, Timestamp = DateTime.UtcNow.Ticks, RedirectUri = "http://test.com" });
            mocker.GetMock<IOAuthRequest>().Setup(x => x.AuthorizationCode).Returns(token);

            var authorizer = mocker.Resolve<AuthorizationCodeAuthorizer>();


            try
            {
                authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);
                Assert.Fail("Exception not thrown");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidRequest, ex.ErrorCode);
                Assert.IsTrue(ex.ErrorDescription.HasValue());
            }

            mocker.GetMock<IOAuthRequest>().Setup(x => x.RedirectUri).Returns("http://test.com");
            var result = authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.AccessToken.HasValue());
            Assert.AreEqual(500, result.ExpiresIn);
            Assert.IsTrue(result.RefreshToken.HasValue());
        }

        [Test]
        public void ReturnsAccessToken()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.AuthorizationCode);
            mocker.GetMock<IConfiguration>().Setup(x => x.AuthorizationTokenExpirationLength).Returns(300);
            mocker.GetMock<IConfiguration>().Setup(x => x.AccessTokenExpirationLength).Returns(500);
            var issuer = new OAuthIssuer();
            mocker.SetInstance<IOAuthIssuer>(issuer);
            var token = issuer.GenerateAuthorizationToken(new TokenData {ConsumerId = 1, Timestamp = DateTime.UtcNow.Ticks});
            mocker.GetMock<IOAuthRequest>().Setup(x => x.AuthorizationCode).Returns(token);

            var authorizer = mocker.Resolve<AuthorizationCodeAuthorizer>();
            var result = authorizer.Authorize(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.AccessToken.HasValue());
            Assert.AreEqual(500, result.ExpiresIn);
            Assert.IsTrue(result.RefreshToken.HasValue());
        }
    }
}
