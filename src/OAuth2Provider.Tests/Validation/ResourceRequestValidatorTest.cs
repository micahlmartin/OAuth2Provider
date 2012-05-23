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
    public class ResourceRequestValidatorTest
    {
        [Test]
        public void WhenAccessTokenIsMissing_ThenValidationFails()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.AccessToken).Returns("");
            var validator = mocker.Resolve<ResourceRequestValidator>();

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsFalse(result.Success);

            mocker.GetMock<IOAuthRequest>().Setup(x => x.AccessToken).Returns<string>(null);

            result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void WhenAccessTokenIsIncluded_ThenValidationSucceeds()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOAuthRequest>().Setup(x => x.AccessToken).Returns("accesstoken");

            var validator = mocker.Resolve<ResourceRequestValidator>();

            var result = validator.ValidateRequest(mocker.GetMock<IOAuthRequest>().Object);

            Assert.IsTrue(result.Success);
        }
    }
}
