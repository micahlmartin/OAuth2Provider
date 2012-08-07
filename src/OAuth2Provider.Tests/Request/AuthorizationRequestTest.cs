using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using OAuth2Provider.Issuer;
using OAuth2Provider.Request;
using AutoMoq;
using NUnit.Framework;
using OAuth2Provider.Tests;

namespace OAuth2Provider.Tests.Request
{
    [TestFixture]
    public class AuthorizationRequestTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenRequestIsNull_ThenExceptionIsThrown()
        {
            var mocker = new AutoMoqer();
            new AuthorizationRequest(null, mocker.GetMock<IOAuthServiceLocator>().Object);
        }

        [Test]
        public void GetRedirectUrl()
        {
            var mocker = new AutoMoqer();
            var properties = new Dictionary<string, IList<string>>
                                 {
                                     {OAuthTokens.ResponseType, new[] {ResponseType.Code}},
                                     {OAuthTokens.ClientId, new[]{"1"}},
                                     {OAuthTokens.RedirectUri, new[]{"http://mydomain.com"}}
                                 };
            mocker.GetMock<IRequest>().Setup(x => x.Values).Returns(properties); 
            var request = new AuthorizationRequest(mocker.GetMock<IRequest>().Object, mocker.GetMock<IOAuthServiceLocator>().Object);

            try
            {
                properties[OAuthTokens.RedirectUri] = new[] { "http://wrong.com" };
                request.GetRedirectUri(new ConsumerImpl {Domain = "test.com"});
                Assert.Fail("Exception not thrown");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidRequest, ex.ErrorCode);
                Assert.IsTrue(ex.ErrorDescription.HasValue());
            }

            try
            {
                properties[OAuthTokens.RedirectUri] = new[] { "wrong.com" };
                request.GetRedirectUri(new ConsumerImpl { Domain = "test.com" });
                Assert.Fail("Exception not thrown");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidRequest, ex.ErrorCode);
                Assert.IsTrue(ex.ErrorDescription.HasValue());
            }

            try
            {
                properties[OAuthTokens.RedirectUri] = new[]{"/test.com/test"};
                request.GetRedirectUri(new ConsumerImpl { Domain = "test.com" });
                Assert.Fail("Exception not thrown");
            }
            catch (OAuthException ex)
            {
                Assert.AreEqual(ErrorCode.InvalidRequest, ex.ErrorCode);
                Assert.IsTrue(ex.ErrorDescription.HasValue());
            }

            properties[OAuthTokens.RedirectUri] = new[] { "http://test.com/response" };
            var result = request.GetRedirectUri(new ConsumerImpl { Domain = "test.com" });

            Assert.AreEqual("http://test.com/response", result);

            result = request.GetRedirectUri(new ConsumerImpl { Domain = "test.com", RedirectUrl = "http://test.com/response" });

            Assert.AreEqual("http://test.com/response", result);
        }

        [Test]
        public void GetAuthorizationToken()
        {
            var mocker = new AutoMoqer();
            mocker.MockServiceLocator();
            var properties = new Dictionary<string, IList<string>>
                                 {
                                     {OAuthTokens.ResponseType, new[]{ResponseType.Code}},
                                     {OAuthTokens.ClientId, new[]{"1"}},
                                     {OAuthTokens.RedirectUri, new[]{"http://mydomain.com"}}
                                 };
            mocker.GetMock<IRequest>().Setup(x => x.Values).Returns(properties);
            mocker.GetMock<IConfiguration>().Setup(x => x.AuthorizationTokenExpirationLength).Returns(500);

            mocker.GetMock<IOAuthServiceLocator>().Setup(x => x.Issuer).Returns(new OAuthIssuer());
            var request = new AuthorizationRequest(mocker.GetMock<IRequest>().Object,mocker.GetMock<IOAuthServiceLocator>().Object);

            var token = request.GetAuthorizationToken(1, 5, null);
            Assert.AreEqual(500, token.ExpiresIn);
            Assert.IsTrue(token.AuthorizationToken.HasValue());
        }
    }
}
