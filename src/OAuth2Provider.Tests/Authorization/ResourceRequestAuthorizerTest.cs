using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth2Provider.Authorization;
using OAuth2Provider.Issuer;
using OAuth2Provider.Repository;
using OAuth2Provider.Request;
using AutoMoq;
using Moq;
using NUnit.Framework;

namespace OAuth2Provider.Tests.Authorization
{
    [TestFixture]
    public class ResourceRequestAuthorizerTest
    {
        [Test]
        public void WhenAccessTokenIsMissing_ThenReturnFalse()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.AccessToken).Returns("");
            var validator = mocker.Resolve<ResourceRequestAuthorizer>();

            var result = validator.Authorize(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsFalse(result);

            mocker.GetMock<IOAuthRequest>().Setup(x => x.AccessToken).Returns<string>(null);

            result = validator.Authorize(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsFalse(result);
        }

        [Test]
        public void WhenAccessTokenIsExpired_ThenReturnFalse()
        {
            var mocker = new AutoMoqer();
            mocker.MockServiceLocator();
            
            var issuer = new OAuthIssuer();
            mocker.GetMock<IOAuthServiceLocator>().Setup(x => x.Issuer).Returns(issuer);
            mocker.GetMock<IConfiguration>().Setup(x => x.AccessTokenExpirationLength).Returns(3600);

            
            var validator = mocker.Resolve<ResourceRequestAuthorizer>();

            var token =
                issuer.GenerateAccessToken(new TokenData
                                               {
                                                   ConsumerId = 1,
                                                   ResourceOwnerId = 5,
                                                   Timestamp = DateTimeOffset.UtcNow.AddMinutes(-65).Ticks
                                               });
            
            

            mocker.GetMock<IOAuthRequest>().Setup(x => x.AccessToken).Returns(token);

            var result = validator.Authorize(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsFalse(result);
        }

        [Test]
        public void WhenAccessTokenIsValid_ThenReturnTrue()
        {
            var mocker = new AutoMoqer();
            mocker.MockServiceLocator();

            var issuer = new OAuthIssuer();
            mocker.GetMock<IOAuthServiceLocator>().Setup(x => x.Issuer).Returns(issuer);
            mocker.GetMock<IConfiguration>().Setup(x => x.AccessTokenExpirationLength).Returns(3600);
            var validator = mocker.Resolve<ResourceRequestAuthorizer>();

            var token =
                issuer.GenerateAccessToken(new TokenData
                {
                    ConsumerId = 1,
                    ResourceOwnerId = 5,
                    Timestamp = DateTimeOffset.UtcNow.AddMinutes(-5).Ticks
                });


            mocker.GetMock<IOAuthRequest>().Setup(x => x.AccessToken).Returns(token);

            var result = validator.Authorize(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result);
        }
    }
}
