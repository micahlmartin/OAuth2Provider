using AutoMoq;
using OAuth2Provider.Validation;
using OAuth2Provider.Request;
using NUnit.Framework;

namespace OAuth2Provider.Tests.Validation
{
    [TestFixture]
    public class PasswordRequestValidatorTest
    {
        [Test]
        public void RequiresGrantTypeOfPassword()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("username");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("password");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns<string>(null);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns("application/x-www-form-urlencoded");

            var validator = mocker.Resolve<PasswordRequestValidator>();

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

            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.Password);
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void RequiresUsername()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.Password);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("password");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns<string>(null);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns("application/x-www-form-urlencoded");

            var validator = mocker.Resolve<PasswordRequestValidator>();

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("  ");
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("asdffa");
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void RequiresPassword()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.Password);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("username");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns<string>(null);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns("application/x-www-form-urlencoded");

            var validator = mocker.Resolve<PasswordRequestValidator>();

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("  ");
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("asdffa");
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void RequiresClientId()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.Password);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("username");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("password");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns<string>(null);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns("application/x-www-form-urlencoded");

            var validator = mocker.Resolve<PasswordRequestValidator>();

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("  ");
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("asdffa");
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void RequiresClientSecret()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.Password);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("username");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("password");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns<string>(null);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns("application/x-www-form-urlencoded");

            var validator = mocker.Resolve<PasswordRequestValidator>();

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("  ");
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("asdffa");
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void RequiresContentType()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.Password);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Username).Returns("username");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Password).Returns("password");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns("");

            var validator = mocker.Resolve<PasswordRequestValidator>();

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
