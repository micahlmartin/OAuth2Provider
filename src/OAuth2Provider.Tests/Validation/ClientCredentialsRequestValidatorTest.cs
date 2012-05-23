using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMoq;
using OAuth2Provider.Request;
using OAuth2Provider.Validation;
using NUnit.Framework;

namespace OAuth2Provider.Tests.Validation
{
    [TestFixture]
    public class ClientCredentialsRequestValidatorTest
    {
        [Test]
        public void RequiresGrantTypeOfClientCredentials()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Method).Returns(HttpMethod.Post);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns<string>(null);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);

            var validator = mocker.Resolve<ClientCredentialsRequestValidator>();

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

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void RequiresClientId()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Method).Returns(HttpMethod.Post);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.ClientCredentials);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns<string>(null);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);

            var validator = mocker.Resolve<ClientCredentialsRequestValidator>();

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
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Method).Returns(HttpMethod.Post);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.ClientCredentials);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns<string>(null);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);

            var validator = mocker.Resolve<ClientCredentialsRequestValidator>();

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
        public void RequiresPostMethod()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Method).Returns(HttpMethod.Post);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.ClientCredentials);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Method).Returns(HttpMethod.Get);

            var validator = mocker.Resolve<ClientCredentialsRequestValidator>();

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.Method).Returns(HttpMethod.Post);
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void RequiresFormContentType()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Method).Returns(HttpMethod.Post);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.ClientCredentials);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("clientid");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientSecret).Returns("clientsecret");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.Json);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.Method).Returns(HttpMethod.Post);

            var validator = mocker.Resolve<ClientCredentialsRequestValidator>();

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.ContentType).Returns(ContentType.FormEncoded);
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result.Success);
        }
    }
}
