using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Contracts.Data.Api.Client.ConfigurationOptions;
using Pds.Contracts.Data.Api.Client.Enumerations;
using Pds.Contracts.Data.Api.Client.Extensions;
using Polly.CircuitBreaker;
using Polly.Registry;
using Polly.Retry;
using System.Net.Http;

namespace Pds.Contracts.Data.Api.Client.Tests.Unit
{
    [TestClass, TestCategory("Unit")]
    public class PolicyBuilderExtensionTests
    {
        [TestMethod]
        public void AddPolicies_VerifyPoliciesAreAddedCorrectly()
        {
            // Arrange
            var dummyServiceCollection = new ServiceCollection();
            var mockConfig = Mock.Of<IConfiguration>();
            var mockRegistry = Mock.Of<IPolicyRegistry<string>>();
            var mockIConfigurationSection = Mock.Of<IConfigurationSection>();

            Mock.Get(mockConfig)
                .Setup(c => c.GetSection(nameof(HttpPolicyOptions)))
                .Returns(mockIConfigurationSection)
                .Verifiable();

            Mock.Get(mockRegistry)
                .Setup(r => r.Add($"{nameof(PolicyBuilderExtensionTests)}_{PolicyType.Retry}", It.IsAny<AsyncRetryPolicy<HttpResponseMessage>>()))
                .Verifiable();

            Mock.Get(mockRegistry)
                .Setup(r => r.Add($"{nameof(PolicyBuilderExtensionTests)}_{PolicyType.CircuitBreaker}", It.IsAny<AsyncCircuitBreakerPolicy<HttpResponseMessage>>()))
                .Verifiable();

            // Act
            dummyServiceCollection.AddPolicies<PolicyBuilderExtensionTests>(mockConfig, mockRegistry);

            // Assert
            Mock.Verify(Mock.Get(mockConfig), Mock.Get(mockRegistry));
        }
    }
}