using AutoMoq;
using OAuth2Provider.Issuer;
using OAuth2Provider.Repository;

namespace OAuth2Provider.Tests
{
    public static class MockHelper
    {
        public static void MockServiceLocator(this AutoMoqer mocker)
        {
            mocker.GetMock<IOAuthServiceLocator>().Setup(x => x.Issuer).Returns(mocker.GetMock<IOAuthIssuer>().Object);
            mocker.GetMock<IOAuthServiceLocator>().Setup(x => x.Configuration).Returns(mocker.GetMock<IConfiguration>().Object);
            mocker.GetMock<IOAuthServiceLocator>().Setup(x => x.ConsumerRepository).Returns(mocker.GetMock<IConsumerRepository>().Object);
            mocker.GetMock<IOAuthServiceLocator>().Setup(x => x.ResourceOwnerRepository).Returns(mocker.GetMock<IResourceOwnerRepository>().Object);
        }
    }
}
