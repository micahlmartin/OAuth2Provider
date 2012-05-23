using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth2Provider.Request;
using OAuth2Provider.Validation;
using AutoMoq;
using NUnit.Framework;

namespace OAuth2Provider.Tests.Validation
{
    [TestFixture]
    public class AuthorizationRequestValidatorTest
    {
        [Test]
        public void RequiresResponseTypeOfCode()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ResponseType).Returns(" ");
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("12345");

            var validator = mocker.Resolve<AuthorizationRequestValidator>();

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.ResponseType).Returns("something_else");

            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.ResponseType).Returns<string>(null);

            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));
        }

        [Test]
        public void RequiresClientId()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ResponseType).Returns(ResponseType.Code);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns(" ");

            var validator = mocker.Resolve<AuthorizationRequestValidator>();

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns<string>(null);

            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));
        }

        [Test]
        public void SucceedsWhenAllParametersAreSet()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ResponseType).Returns(ResponseType.Code);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("12345");

            var validator = mocker.Resolve<AuthorizationRequestValidator>();

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void RequiresValidRedirectUrl()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ResponseType).Returns(ResponseType.Code);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.ClientId).Returns("12345");

            var validator = mocker.Resolve<AuthorizationRequestValidator>();

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result.Success);

            mocker.GetMock<IOAuthRequest>().Setup(x => x.RedirectUri).Returns("");

            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));


            mocker.GetMock<IOAuthRequest>().Setup(x => x.RedirectUri).Returns("/test/whatever");

            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.RedirectUri).Returns("");

            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));


            mocker.GetMock<IOAuthRequest>().Setup(x => x.RedirectUri).Returns("tcp://whatnow.com");

            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorDescription));

            mocker.GetMock<IOAuthRequest>().Setup(x => x.RedirectUri).Returns("http://something.com");

            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result.Success);

            mocker.GetMock<IOAuthRequest>().Setup(x => x.RedirectUri).Returns("https://something.com");

            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result.Success);
        }
    }
}
