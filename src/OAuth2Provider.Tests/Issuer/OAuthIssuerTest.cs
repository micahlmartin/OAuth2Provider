using System;
using OAuth2Provider.Issuer;
using NUnit.Framework;

namespace OAuth2Provider.Tests.Issuer
{
    [TestFixture]
    public class OAuthIssuerTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateAccessToken_ThrowsExceptionWhenDataIsNull()
        {
            var issuer = new OAuthIssuer();
            issuer.GenerateAccessToken(null);
        }

        [Test]
        public void GenerateAccessToken_GeneratesToken()
        {
            var ticks = DateTime.Now.Ticks;
            var data = new TokenData { ConsumerId = 1, ResourceOwnerId = 2, Timestamp = ticks };
            var issuer = new OAuthIssuer();

            var token1 = issuer.GenerateAccessToken(data);
            var token2 = issuer.GenerateAccessToken(data);

            data.Timestamp = DateTime.Now.Ticks + 150;
            var token3 = issuer.GenerateAccessToken(data);

            Assert.AreEqual(token1, token2);
            Assert.AreNotEqual(token1, token3);
            Assert.AreNotEqual(token2, token3);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecodeAccessToken_ThrowsExceptionWhenDataIsNull()
        {
            var issuer = new OAuthIssuer();
            issuer.DecodeAccessToken(null);
        }

        [Test]
        public void AccessTokenAndRefreshTokenAreNotEqual()
        {
            var issuer = new OAuthIssuer();

            var data = new TokenData {ConsumerId = 12345, ResourceOwnerId = 12345, Timestamp = DateTime.Now.Ticks};

            var accessToken = issuer.GenerateAccessToken(data);
            var refreshToken = issuer.GenerateRefreshToken(data);

            Assert.AreNotEqual(accessToken, refreshToken);
        }

        [Test]
        public void DecodeAccessToken_DecodesToken()
        {
            var ticks = DateTime.Now.Ticks;
            var data = new TokenData { ConsumerId = 1, ResourceOwnerId = 2, Timestamp = ticks };
            var issuer = new OAuthIssuer();

            var token1 = issuer.GenerateAccessToken(data);

            var token = issuer.DecodeAccessToken(token1);

            Assert.AreEqual(2, token.ResourceOwnerId);
            Assert.AreEqual(ticks, token.Timestamp);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateRefreshToken_ThrowExceptionWhenDataIsNull()
        {
            var issuer = new OAuthIssuer();
            issuer.GenerateRefreshToken(null);
        }

        [Test]
        public void GenerateRefreshToken_GeneratesToken()
        {
            var ticks = DateTime.Now.Ticks;
            var data = new TokenData { ConsumerId = 1, ResourceOwnerId = 2, Timestamp = ticks };
            var issuer = new OAuthIssuer();

            var token1 = issuer.GenerateRefreshToken(data);
            var token2 = issuer.GenerateRefreshToken(data);
            var token3 = issuer.GenerateRefreshToken(data);

            Assert.AreNotEqual(token1, token2);
            Assert.AreNotEqual(token1, token3);
            Assert.AreNotEqual(token2, token3);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecodeRefreshtoken_ThrowsExceptionWhenDataIsNull()
        {
            var issuer = new OAuthIssuer();
            issuer.DecodeRefreshToken(null);
        }

        [Test]
        public void DecodeRefreshToken_DecodesToken()
        {
            var ticks = DateTime.Now.Ticks;
            var data = new TokenData { ConsumerId = 1, ResourceOwnerId = 2, Timestamp = ticks };
            var issuer = new OAuthIssuer();

            var token1 = issuer.GenerateRefreshToken(data);

            var token = issuer.DecodeRefreshToken(token1);

            Assert.AreEqual(1, token.ConsumerId);
            Assert.AreEqual(2, token.ResourceOwnerId);
            Assert.AreEqual(ticks, token.Timestamp);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateAuthorizationToken_ThrowsExceptionWhenDataIsNull()
        {
            var issuer = new OAuthIssuer();
            issuer.GenerateAuthorizationToken(null);
        }

        [Test]
        public void GenerateAuthorizationToken_GeneratesCode()
        {
            var ticks = DateTime.Now.Ticks;
            var data = new TokenData { ConsumerId = 1, ResourceOwnerId = 2, Timestamp = ticks, RedirectUri = "http://www.test.com" };
            var issuer = new OAuthIssuer();

            var token1 = issuer.GenerateAuthorizationToken(data);

            data.RedirectUri = null;
            var token2 = issuer.GenerateAuthorizationToken(data);

            data.RedirectUri = "http://test.com";
            var token3 = issuer.GenerateAuthorizationToken(data);

            Assert.AreNotEqual(token1, token2);
            Assert.AreNotEqual(token1, token3);
            Assert.AreNotEqual(token2, token3);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecodeAuthorizationToken_ThrowsExceptionWhenDataIsNull()
        {
            var issuer = new OAuthIssuer();
            issuer.DecodeAuthorizationToken(null);
        }

        [Test]
        public void DecodeAuthorizationToken_DecodesToken()
        {
            var ticks = DateTime.Now.Ticks;
            var data = new TokenData { ConsumerId = 1, Timestamp = ticks, ResourceOwnerId = 3 };
            var issuer = new OAuthIssuer();

            var token1 = issuer.GenerateAuthorizationToken(data);

            var token = issuer.DecodeAuthorizationToken(token1);

            Assert.AreEqual(1, token.ConsumerId);
            Assert.AreEqual(ticks, token.Timestamp);
            Assert.AreEqual(3, token.ResourceOwnerId);
            Assert.IsNull(token.RedirectUri);

            data = new TokenData { ConsumerId = 1, Timestamp = ticks, ResourceOwnerId = 3, RedirectUri = "http://test.com" };

            token1 = issuer.GenerateAuthorizationToken(data);

            token = issuer.DecodeAuthorizationToken(token1);

            Assert.AreEqual(1, token.ConsumerId);
            Assert.AreEqual(ticks, token.Timestamp);
            Assert.AreEqual(3, token.ResourceOwnerId);
            Assert.AreEqual("http://test.com", token.RedirectUri);
        }
    }
}
