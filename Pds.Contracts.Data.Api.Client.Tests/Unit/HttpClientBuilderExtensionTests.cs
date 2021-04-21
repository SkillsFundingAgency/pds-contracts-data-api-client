using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Pds.Contracts.Data.Api.Client.Enumerations;
using Pds.Contracts.Data.Api.Client.Extensions;
using Pds.Contracts.Data.Api.Client.Tests.Unit.Dummy;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pds.Contracts.Data.Api.Client.Tests.Unit
{
    [TestClass, TestCategory("Unit")]
    public class HttpClientBuilderExtensionTests
    {
        [TestMethod]
        public async Task AddHttpClient_WhenPollyRetryPolicyIsSet_NoErrorOccurs_NoRetriesAreAttempted()
        {
            // Arrange
            var expectedStausCode = HttpStatusCode.OK;

            SetupMockDelegate(expectedStausCode);
            IDummyService dummyService = CreateDummyService(_mockDelegatingHandler);

            // Act
            var response = await dummyService.MakeHttpCallAsync();

            // Assert
            response.StatusCode.Should().Be(expectedStausCode);
            VerifyHttpRequests(Times.Once());
        }

        [TestMethod]
        public async Task AddHttpClient_WhenPollyRetryPolicyIsSet_SingleServiceErrorsOccur_OperationIsRetried()
        {
            // Arrange
            int retryCount = 0;
            var expectedStausCode = HttpStatusCode.OK;

            var mockDelegate = Mock.Of<DelegatingHandler>(MockBehavior.Strict);
            Mock.Get(mockDelegate)
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(() =>
                {
                    // First attempt will result in failure
                    retryCount++;
                    if (retryCount > 1)
                    {
                        return Task.FromResult(new HttpResponseMessage(expectedStausCode));
                    }
                    else
                    {
                        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                    }
                });

            IDummyService dummyService = CreateDummyService(mockDelegate);

            // Act
            Func<Task<HttpResponseMessage>> act = async () => await dummyService.MakeHttpCallAsync();

            // By default, first call to Act is not sufficent to trigger circuit breaker
            var response = await act();

            // Assert
            response.StatusCode.Should().Be(expectedStausCode);

            Mock.Get(mockDelegate)
            .Protected()
            .Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task AddHttpClient_When_PollyRetryPolicyIsSet_And_MultipleConsecutiveSericeErrorsOccur_Then_CircuitBreakerIsTriggered()
        {
            // Arrange
            var expectedStausCode = HttpStatusCode.InternalServerError;

            SetupMockDelegate(expectedStausCode);
            IDummyService dummyService = CreateDummyService(_mockDelegatingHandler);

            // Act
            Func<Task<HttpResponseMessage>> act = async () => await dummyService.MakeHttpCallAsync();

            // By default, first call to Act is not sufficent to trigger circuit breaker
            var response = await act();

            // Assert
            response.StatusCode.Should().Be(expectedStausCode);

            // Second call has caused sufficent number of failures to trigger
            act.Should().Throw<BrokenCircuitException<HttpResponseMessage>>();
            VerifyHttpRequests(Times.Exactly(5));
        }

        #region Setup Helpers

        private readonly DelegatingHandler _mockDelegatingHandler
            = Mock.Of<DelegatingHandler>();

        private static IConfigurationRoot GetConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["DummyApiClientOptions:ApiBaseAddress"] = "https://testhost/",
                ["DummyApiClientOptions:Authority"] = "https://testauthority/",
                ["DummyApiClientOptions:TenantId"] = "tenant-id",
                ["DummyApiClientOptions:ClientId"] = "client-id",
                ["DummyApiClientOptions:ClientSecret"] = "secret",
                ["DummyApiClientOptions:AppUri"] = "api-uri"
            });

            var config = configBuilder.Build();
            return config;
        }

        private static IDummyService CreateDummyService(DelegatingHandler httpMessageHandler)
        {
            var serviceCollection = new ServiceCollection();
            IConfigurationRoot config = GetConfiguration();

            var policyRegistry = serviceCollection.AddPolicyRegistry();
            var policies = new PolicyType[] { PolicyType.Retry, PolicyType.CircuitBreaker };

            serviceCollection
                 .AddPolicies<IDummyService>(config, policyRegistry)
                 .AddHttpClient<IDummyService, DummyService, DummyApiClientOptions>(config, policies)
                 .AddHttpMessageHandler(() => httpMessageHandler);

            var provider = serviceCollection.BuildServiceProvider();
            var dummyService = provider.GetRequiredService<IDummyService>();
            return dummyService;
        }

        private void SetupMockDelegate(HttpStatusCode expectedStausCode)
        {
            Mock.Get(_mockDelegatingHandler)
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(() => Task.FromResult(new HttpResponseMessage(expectedStausCode)))
                .Verifiable();
        }

        #endregion Setup Helpers

        #region Verify Helpers

        private void VerifyHttpRequests(Times times)
        {
            Mock.Get(_mockDelegatingHandler)
            .Protected()
            .Verify(
                "SendAsync",
                times,
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        #endregion Verify Helpers
    }
}