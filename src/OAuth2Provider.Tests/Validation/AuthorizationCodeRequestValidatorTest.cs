using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth2Provider.Authorization;
using OAuth2Provider.Request;
using OAuth2Provider.Validation;
using AutoMoq;
using NUnit.Framework;

namespace OAuth2Provider.Tests.Validation
{
    [TestFixture]
    public class AuthorizationCodeRequestValidatorTest
    {
        [Test]
        public void RequiresAuthorizationCodeGrantType()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns<string>(null);

            var validator = mocker.Resolve<AuthorizationCodeRequestValidator>();
            
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns("");

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsTrue(result.ErrorDescription.HasValue());

            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns("   ");

            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsTrue(result.ErrorDescription.HasValue());

            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns("bad");

            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);
            Assert.AreEqual(ErrorCode.InvalidGrant, result.ErrorCode);
            Assert.IsTrue(result.ErrorDescription.HasValue());

            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.AuthorizationCode);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.AuthorizationCode).Returns("authcode");
            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void RequiresAuthorizationCode()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.GrantType).Returns(GrantType.AuthorizationCode);
            mocker.GetMock<IOAuthRequest>().Setup(x => x.AuthorizationCode).Returns<string>(null);

            var validator = mocker.Resolve<AuthorizationCodeRequestValidator>();

            mocker.GetMock<IOAuthRequest>().Setup(x => x.AuthorizationCode).Returns("");

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsTrue(result.ErrorDescription.HasValue());

            mocker.GetMock<IOAuthRequest>().Setup(x => x.AuthorizationCode).Returns("   ");

            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);
            Assert.AreEqual(ErrorCode.InvalidRequest, result.ErrorCode);
            Assert.IsTrue(result.ErrorDescription.HasValue());

            mocker.GetMock<IOAuthRequest>().Setup(x => x.AuthorizationCode).Returns("auth_code");

            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);
            Assert.IsTrue(result.Success);
        }
    }
}
