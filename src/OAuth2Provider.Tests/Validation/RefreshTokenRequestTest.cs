using AutoMoq;
using OAuth2Provider.Validation;
using OAuth2Provider.Request;
using NUnit.Framework;

namespace OAuth2Provider.Tests.Validation
{
    [TestFixture]
    public class RefreshRequestValidatorTest
    {
        [Test]
        public void RequiresGrantTypeOfRefreshToken()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientId");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns<string>(null);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.RefreshToken).Returns("refreshtoken");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns("application/x-www-form-urlencoded");

            var validator = mocker.Resolve<RefreshTokenRequestValidator>();

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns("");
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.ClientCredentials);
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidGrant, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.RefreshToken);
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void RequiresContentType()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.RefreshToken);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("username");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("password");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.RefreshToken).Returns("refreshtoken");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns("");

            var validator = mocker.Resolve<RefreshTokenRequestValidator>();

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns<string>(null);
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns("application/json");
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns("application/x-www-form-urlencoded");
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result.Success);
        }
    }
}
