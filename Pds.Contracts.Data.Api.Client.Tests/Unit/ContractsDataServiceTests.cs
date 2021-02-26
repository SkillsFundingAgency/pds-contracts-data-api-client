using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Contracts.Data.Api.Client.ConfigurationOptions;
using Pds.Contracts.Data.Api.Client.Enumerations;
using Pds.Contracts.Data.Api.Client.Implementations;
using Pds.Contracts.Data.Api.Client.Models;
using Pds.Core.ApiClient.Exceptions;
using Pds.Core.ApiClient.Interfaces;
using Pds.Core.Logging;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pds.Contracts.Data.Api.Client.Tests.Unit
{
    [TestClass, TestCategory("Unit")]
    public class ContractsDataServiceTests
    {
        private const string TestBaseAddress = "http://test-api-endpoint";

        private const string TestFakeAccessToken = "AccessToken";

        #region ContractDataService Tests

        [TestMethod]
        public async Task GetAsync_MockHttp()
        {
            // Arrange
            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            var expectedContract = new Contract() { Id = 1, ContractVersion = 1, ContractNumber = "Test12345" };

            string jsonString = JsonSerializer.Serialize(expectedContract);

            _mockHttpMessageHandler.Expect(TestBaseAddress + "/api/contract/1").Respond("application/json", jsonString);

            ContractsDataService contractsDataService = CreateContractsDataService();
            int contractId = 1;

            //Act
            var result = await contractsDataService.GetContractByIdAsync(contractId);

            // Assert
            result.Should().BeEquivalentTo(expectedContract);
            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        [TestMethod]
        public async Task GetByContractNumberAndVersionAsync_MockHttp()
        {
            // Arrange
            string contractNumber = "Test";
            int version = 1;

            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            var expectedContract = new Contract() { Id = 1, ContractVersion = 1, ContractNumber = "Test1" };

            string jsonString = JsonSerializer.Serialize(expectedContract);

            _mockHttpMessageHandler.Expect(TestBaseAddress + $"/api/contract?contractNumber={contractNumber}&versionNumber={version}").Respond("application/json", jsonString);

            ContractsDataService contractsDataService = CreateContractsDataService();

            //Act
            var result = await contractsDataService.GetContractByContractNumberAndVersionAsync(contractNumber, version);

            // Assert
            result.Should().BeEquivalentTo(expectedContract);
            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        [TestMethod]
        public async Task GetContractRemindersAsync_MockHttp()
        {
            // Arrange
            int reminderInterval = 14;
            int pageNumber = 1;
            int pageSize = 2;
            ContractSortOptions sort = ContractSortOptions.LastUpdatedAt;
            SortDirection order = SortDirection.Asc;
            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            var contractList = new List<ContractReminderItem>()
            {
                new ContractReminderItem() { Id = 1, ContractVersion = 1, ContractNumber = "Test1" },
                new ContractReminderItem() { Id = 2, ContractVersion = 1, ContractNumber = "Test2" }
            };
            ContractReminderResponse<List<ContractReminderItem>> expectedContractReminders = new ContractReminderResponse<List<ContractReminderItem>>(contractList);
            expectedContractReminders.Paging = new Metadata() { CurrentPage = pageNumber, PageSize = pageSize, TotalCount = 2, TotalPages = 1, HasNextPage = false, HasPreviousPage = false, NextPageUrl = string.Empty, PreviousPageUrl = string.Empty };

            string jsonString = JsonSerializer.Serialize(expectedContractReminders);

            _mockHttpMessageHandler.Expect(TestBaseAddress + $"/api/contractReminders?reminderInterval={reminderInterval}&page={pageNumber}&count={pageSize}&sort={sort}&order={order}").Respond("application/json", jsonString);

            ContractsDataService contractsDataService = CreateContractsDataService();

            //Act
            var result = await contractsDataService.GetContractRemindersAsync((uint) reminderInterval, (uint)pageNumber, (uint)pageSize, sort, order);

            // Assert
            result.Should().BeEquivalentTo(expectedContractReminders);
            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        [TestMethod]
        public void UpdateLastEmailReminderSentAndLastUpdatedAtAsync_MockHttp()
        {
            // Arrange
            UpdateLastEmailReminderSentRequest request = new UpdateLastEmailReminderSentRequest() { Id = 1, ContractVersion = 1, ContractNumber = "Test1" };

            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            _mockHttpMessageHandler.Expect(TestBaseAddress + $"/api/contractReminder").Respond(HttpStatusCode.OK);

            ContractsDataService contractsDataService = CreateContractsDataService();

            //Act
            Func<Task> act = async () => await contractsDataService.UpdateContractReminderAsync(request);

            // Assert
            act.Should().NotThrow();
            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        [TestMethod]
        public void UpdateLastEmailReminderSentAndLastUpdatedAtAsync_Mock404Http()
        {
            // Arrange
            UpdateLastEmailReminderSentRequest request = new UpdateLastEmailReminderSentRequest() { Id = 1, ContractVersion = 1, ContractNumber = "Test1" };

            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogError(It.IsAny<Exception>(), It.IsAny<string>()));

            ContractsDataService contractsDataService = CreateContractsDataService();

            //Act
            Func<Task> act = async () => await contractsDataService.UpdateContractReminderAsync(request);

            // Assert
            act.Should().Throw<ApiGeneralException>().Where(e => e.ResponseStatusCode == HttpStatusCode.NotFound);
            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        #endregion ContractDataService Tests


        #region Setup Helpers

        private readonly MockHttpMessageHandler _mockHttpMessageHandler
            = new MockHttpMessageHandler();

        private readonly ILoggerAdapter<ContractsDataService> _contractsDataLogger
            = Mock.Of<ILoggerAdapter<ContractsDataService>>(MockBehavior.Strict);

        private ContractsDataService CreateContractsDataService()
        {
            var httpClient = _mockHttpMessageHandler.ToHttpClient();
            var authenticationService = GetAuthenticationService();
            var contractsDataConfiguration = Options.Create(GetServicesConfiguration());

            return new ContractsDataService(authenticationService, httpClient, contractsDataConfiguration, _contractsDataLogger);
        }

        private IAuthenticationService<ContractsDataApiConfiguration> GetAuthenticationService()
        {
            var mockAuthenticationService = new Mock<IAuthenticationService<ContractsDataApiConfiguration>>(MockBehavior.Strict);
            mockAuthenticationService
                .Setup(x => x.GetAccessTokenForAAD())
                .Returns(Task.FromResult(TestFakeAccessToken));
            return mockAuthenticationService.Object;
        }

        private ContractsDataApiConfiguration GetServicesConfiguration()
            => new ContractsDataApiConfiguration()
            {
                ApiBaseAddress = TestBaseAddress
            };

        #endregion Setup Helpers


        #region Verify Helpers

        private void VerifyAllMocks()
        {
            Mock.Get(_contractsDataLogger).VerifyAll();
        }

        #endregion Verify Helpers
    }
}