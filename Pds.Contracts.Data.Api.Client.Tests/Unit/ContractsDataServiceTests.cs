using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Contracts.Data.Api.Client.ConfigurationOptions;
using Pds.Contracts.Data.Api.Client.Enumerations;
using Pds.Contracts.Data.Api.Client.Exceptions;
using Pds.Contracts.Data.Api.Client.Implementations;
using Pds.Contracts.Data.Api.Client.Models;
using Pds.Core.ApiClient.Exceptions;
using Pds.Core.ApiClient.Interfaces;
using Pds.Core.Logging;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Pds.Contracts.Data.Api.Client.Tests.Unit
{
    [TestClass, TestCategory("Unit")]
    public class ContractsDataServiceTests
    {
        private const string TestBaseAddress = "http://test-api-endpoint";
        private const string TestFakeAccessToken = "AccessToken";

        private readonly MockHttpMessageHandler _mockHttpMessageHandler = new MockHttpMessageHandler();
        private readonly ILoggerAdapter<ContractsDataService> _contractsDataLogger = Mock.Of<ILoggerAdapter<ContractsDataService>>(MockBehavior.Strict);

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
        public async Task TryGetContractAsyncTestAsync()
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
            var result = await contractsDataService.TryGetContractAsync(contractNumber, version);

            // Assert
            result.Should().BeEquivalentTo(expectedContract);
            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        [TestMethod]
        public async Task TryGetContractAsync_ShoulNotThrowNotFoundException_TestAsync()
        {
            // Arrange
            string contractNumber = "Test";
            int version = 1;

            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()));

            ContractsDataService contractsDataService = CreateContractsDataService();
            _mockHttpMessageHandler.Expect(TestBaseAddress + $"/api/contract?contractNumber={contractNumber}&versionNumber={version}").Respond(HttpStatusCode.NotFound);

            //Act
            var result = await contractsDataService.TryGetContractAsync(contractNumber, version);

            // Assert
            result.Should().BeNull();
            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        [TestMethod]
        public async Task TryGetContractAsyncTestAsync()
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
            var result = await contractsDataService.TryGetContractAsync(contractNumber, version);

            // Assert
            result.Should().BeEquivalentTo(expectedContract);
            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        [TestMethod]
        public async Task TryGetContractAsync_ShoulNotThrowNotFoundException_TestAsync()
        {
            // Arrange
            string contractNumber = "Test";
            int version = 1;

            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()));

            ContractsDataService contractsDataService = CreateContractsDataService();
            _mockHttpMessageHandler.Expect(TestBaseAddress + $"/api/contract?contractNumber={contractNumber}&versionNumber={version}").Respond(HttpStatusCode.NotFound);

            //Act
            var result = await contractsDataService.TryGetContractAsync(contractNumber, version);

            // Assert
            result.Should().BeNull();
            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        [DataTestMethod]
        [DataRow("test", 1)]
        [DataRow("test+24", 1)]
        [DataRow("test!£$%^&*()-_=+{}@#~?><./\\\"¬`24", 0)]
        public async Task GetByContractNumberAndVersionAsync_MockHttp(string contractNumber, int version)
        {
            // Arrange
            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            var expectedContract = new Contract() { Id = 1, ContractVersion = 1, ContractNumber = "Test1" };

            string jsonString = JsonSerializer.Serialize(expectedContract);

            _mockHttpMessageHandler.Expect(TestBaseAddress + $"/api/contract?contractNumber={HttpUtility.UrlEncode(contractNumber)}&versionNumber={version}").Respond("application/json", jsonString);

            ContractsDataService contractsDataService = CreateContractsDataService();

            //Act
            var result = await contractsDataService.GetContractAsync(contractNumber, version);

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
            var expectedContractReminders = new ContractReminders { Contracts = contractList };
            expectedContractReminders.Paging = new Paging() { CurrentPage = pageNumber, PageSize = pageSize, TotalCount = 2, TotalPages = 1, HasNextPage = false, HasPreviousPage = false, NextPageUrl = string.Empty, PreviousPageUrl = string.Empty };

            string jsonString = JsonSerializer.Serialize(expectedContractReminders);

            _mockHttpMessageHandler.Expect(TestBaseAddress + $"/api/contractReminders?reminderInterval={reminderInterval}&page={pageNumber}&count={pageSize}&sort={sort}&order={order}").Respond("application/json", jsonString);

            ContractsDataService contractsDataService = CreateContractsDataService();

            //Act
            var result = await contractsDataService.GetContractRemindersAsync((uint)reminderInterval, (uint)pageNumber, (uint)pageSize, sort, order);

            // Assert
            result.Should().BeEquivalentTo(expectedContractReminders);
            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK)]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.NotFound)]
        public void UpdateLastEmailReminderSentAndLastUpdatedAtAsync_Test(HttpStatusCode httpStatusCode)
        {
            // Arrange
            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            if (httpStatusCode != HttpStatusCode.OK)
            {
                Mock.Get(_contractsDataLogger)
                    .Setup(p => p.LogError(It.IsAny<ApiGeneralException>(), It.IsAny<string>(), It.IsAny<object[]>()));
            }

            var expectedContractRequest = new ContractReminderItem { Id = 1, ContractVersion = 1, ContractNumber = "Test1" };
            ContractsDataService contractsDataService = CreateContractsDataService();

            SetUpHttpMessageHandler(expectedContractRequest, httpStatusCode, $"/api/contractReminder", HttpMethod.Patch);

            //Act
            Func<Task> action = async () => await contractsDataService.UpdateContractReminderAsync(expectedContractRequest);

            // Assert
            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                    action.Should().NotThrow();
                    break;

                case HttpStatusCode.BadRequest:
                    action.Should().Throw<ContractBadRequestClientException>();
                    break;

                case HttpStatusCode.NotFound:
                    action.Should().Throw<ContractNotFoundClientException>();
                    break;

                case HttpStatusCode.PreconditionFailed:
                    action.Should().Throw<ContractStatusClientException>();
                    break;

                case HttpStatusCode.Conflict:
                    action.Should().Throw<ContractUpdateConcurrencyClientException>();
                    break;

                default:
                    throw new NotImplementedException();
            }

            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK)]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.PreconditionFailed)]
        [DataRow(HttpStatusCode.Conflict)]
        public void CreateContractAsync_Test(HttpStatusCode httpStatusCode)
        {
            // Arrange
            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            if (httpStatusCode != HttpStatusCode.OK)
            {
                Mock.Get(_contractsDataLogger)
                    .Setup(p => p.LogError(It.IsAny<ApiGeneralException>(), It.IsAny<string>(), It.IsAny<object[]>()));
            }

            var expectedContractRequest = new CreateRequest { ContractNumber = "Test" };
            ContractsDataService contractsDataService = CreateContractsDataService();

            SetUpHttpMessageHandler(expectedContractRequest, httpStatusCode, $"/api/contract", HttpMethod.Post);

            //Act
            Func<Task> action = async () => await contractsDataService.CreateContractAsync(expectedContractRequest);

            // Assert
            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                    action.Should().NotThrow();
                    break;

                case HttpStatusCode.BadRequest:
                    action.Should().Throw<ContractBadRequestClientException>();
                    break;

                case HttpStatusCode.NotFound:
                    action.Should().Throw<ContractNotFoundClientException>();
                    break;

                case HttpStatusCode.PreconditionFailed:
                    action.Should().Throw<ContractWithHigherVersionAlreadyExistsClientException>();
                    break;

                case HttpStatusCode.Conflict:
                    action.Should().Throw<DuplicateContractClientException>();
                    break;

                default:
                    throw new NotImplementedException();
            }

            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK)]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.PreconditionFailed)]
        [DataRow(HttpStatusCode.Conflict)]
        public void ManualApproveAsyncTest(HttpStatusCode httpStatusCode)
        {
            // Arrange
            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            if (httpStatusCode != HttpStatusCode.OK)
            {
                Mock.Get(_contractsDataLogger)
                    .Setup(p => p.LogError(It.IsAny<ApiGeneralException>(), It.IsAny<string>(), It.IsAny<object[]>()));
            }

            var expectedContractRequest = new ApprovalRequest { ContractNumber = "Test", ContractVersion = 1, FileName = "sample-blob-file.xml", Id = 1 };
            ContractsDataService contractsDataService = CreateContractsDataService();

            SetUpHttpMessageHandler(expectedContractRequest, httpStatusCode, $"/api/contract/manualApprove", HttpMethod.Patch);

            //Act
            Func<Task> action = async () => await contractsDataService.ManualApproveAsync(expectedContractRequest);

            // Assert
            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                    action.Should().NotThrow();
                    break;

                case HttpStatusCode.BadRequest:
                    action.Should().Throw<ContractBadRequestClientException>();
                    break;

                case HttpStatusCode.NotFound:
                    action.Should().Throw<ContractNotFoundClientException>();
                    break;

                case HttpStatusCode.PreconditionFailed:
                    action.Should().Throw<ContractStatusClientException>();
                    break;

                case HttpStatusCode.Conflict:
                    action.Should().Throw<ContractUpdateConcurrencyClientException>();
                    break;

                default:
                    throw new NotImplementedException();
            }

            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK)]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.PreconditionFailed)]
        [DataRow(HttpStatusCode.Conflict)]
        public void ConfirmApprovalAsyncTest(HttpStatusCode httpStatusCode)
        {
            // Arrange
            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            if (httpStatusCode != HttpStatusCode.OK)
            {
                Mock.Get(_contractsDataLogger)
                    .Setup(p => p.LogError(It.IsAny<ApiGeneralException>(), It.IsAny<string>(), It.IsAny<object[]>()));
            }

            var expectedContractRequest = new ApprovalRequest { ContractNumber = "Test", ContractVersion = 1, FileName = "sample-blob-file.xml", Id = 1 };
            ContractsDataService contractsDataService = CreateContractsDataService();

            SetUpHttpMessageHandler(expectedContractRequest, httpStatusCode, $"/api/contract/confirmApproval", HttpMethod.Patch);

            //Act
            Func<Task> action = async () => await contractsDataService.ConfirmApprovalAsync(expectedContractRequest);

            // Assert
            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                    action.Should().NotThrow();
                    break;

                case HttpStatusCode.BadRequest:
                    action.Should().Throw<ContractBadRequestClientException>();
                    break;

                case HttpStatusCode.NotFound:
                    action.Should().Throw<ContractNotFoundClientException>();
                    break;

                case HttpStatusCode.PreconditionFailed:
                    action.Should().Throw<ContractStatusClientException>();
                    break;

                case HttpStatusCode.Conflict:
                    action.Should().Throw<ContractUpdateConcurrencyClientException>();
                    break;

                default:
                    throw new NotImplementedException();
            }

            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK, ContractStatus.WithdrawnByAgency)]
        [DataRow(HttpStatusCode.BadRequest, ContractStatus.WithdrawnByAgency)]
        [DataRow(HttpStatusCode.NotFound, ContractStatus.WithdrawnByAgency)]
        [DataRow(HttpStatusCode.PreconditionFailed, ContractStatus.WithdrawnByAgency)]
        [DataRow(HttpStatusCode.Conflict, ContractStatus.WithdrawnByAgency)]
        [DataRow(HttpStatusCode.OK, ContractStatus.WithdrawnByProvider)]
        [DataRow(HttpStatusCode.BadRequest, ContractStatus.WithdrawnByProvider)]
        [DataRow(HttpStatusCode.NotFound, ContractStatus.WithdrawnByProvider)]
        [DataRow(HttpStatusCode.PreconditionFailed, ContractStatus.WithdrawnByProvider)]
        [DataRow(HttpStatusCode.Conflict, ContractStatus.WithdrawnByProvider)]
        public void WithdrawAsyncTest(HttpStatusCode httpStatusCode, ContractStatus withdrawalType)
        {
            // Arrange
            Mock.Get(_contractsDataLogger)
                .Setup(p => p.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            if (httpStatusCode != HttpStatusCode.OK)
            {
                Mock.Get(_contractsDataLogger)
                    .Setup(p => p.LogError(It.IsAny<ApiGeneralException>(), It.IsAny<string>(), It.IsAny<object[]>()));
            }

            var expectedContractRequest = new WithdrawalRequest
            {
                ContractNumber = "Test",
                ContractVersion = 1,
                FileName = "sample-blob-file.xml",
                Id = 1,
                WithdrawalType = withdrawalType
            };

            ContractsDataService contractsDataService = CreateContractsDataService();

            SetUpHttpMessageHandler(expectedContractRequest, httpStatusCode, $"/api/contract/withdraw", HttpMethod.Patch);

            //Act
            Func<Task> action = async () => await contractsDataService.WithdrawAsync(expectedContractRequest);

            // Assert
            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                    action.Should().NotThrow();
                    break;

                case HttpStatusCode.BadRequest:
                    action.Should().Throw<ContractBadRequestClientException>();
                    break;

                case HttpStatusCode.NotFound:
                    action.Should().Throw<ContractNotFoundClientException>();
                    break;

                case HttpStatusCode.PreconditionFailed:
                    action.Should().Throw<ContractStatusClientException>();
                    break;

                case HttpStatusCode.Conflict:
                    action.Should().Throw<ContractUpdateConcurrencyClientException>();
                    break;

                default:
                    throw new NotImplementedException();
            }

            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();
            VerifyAllMocks();
        }

        #endregion ContractDataService Tests


        #region Setup Helpers

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

        private void SetUpHttpMessageHandler<T>(T expectedContractRequest, HttpStatusCode httpStatusCode, string endpoint, HttpMethod httpMethod)
        {
            _mockHttpMessageHandler
                .Expect(httpMethod, TestBaseAddress + endpoint)
                .With(m =>
                {
                    var input = m.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var createContractRequest = JsonSerializer.Deserialize<T>(input);
                    createContractRequest.Should().BeEquivalentTo(expectedContractRequest);
                    return true;
                })
                .Respond(httpStatusCode);
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