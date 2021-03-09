using Microsoft.Extensions.Options;
using Pds.Contracts.Data.Api.Client.ConfigurationOptions;
using Pds.Contracts.Data.Api.Client.Enumerations;
using Pds.Contracts.Data.Api.Client.Interfaces;
using Pds.Contracts.Data.Api.Client.Models;
using Pds.Core.ApiClient;
using Pds.Core.ApiClient.Exceptions;
using Pds.Core.ApiClient.Interfaces;
using Pds.Core.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pds.Contracts.Data.Api.Client.Implementations
{
    /// <summary>
    /// Service wrapper to allow calls to be made to the Contracts Data Api.
    /// </summary>
    public class ContractsDataService : BaseApiClient<ContractsDataApiConfiguration>, IContractsDataService
    {
        private readonly ILoggerAdapter<ContractsDataService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractsDataService"/> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configurationOptions">Contract data api configuration options.</param>
        /// <param name="logger">ILogger reference to log output.</param>
        public ContractsDataService(
            IAuthenticationService<ContractsDataApiConfiguration> authenticationService,
            HttpClient httpClient,
            IOptions<ContractsDataApiConfiguration> configurationOptions,
            ILoggerAdapter<ContractsDataService> logger)
            : base(authenticationService, httpClient, Options.Create(configurationOptions.Value))
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Models.Contract> GetContractByIdAsync(int id)
        {
            _logger.LogInformation($"Retrieving a contract for id : {id}");
            return await Get<Contract>($"/api/contract/{id}");
        }

        /// <inheritdoc/>
        public async Task<Models.Contract> GetContractAsync(string contractNumber, int version)
        {
            _logger.LogInformation($"Retrieving a contract for contract number : {contractNumber} and version : {version}");
            return await Get<Contract>($"/api/contract?contractNumber={contractNumber}&versionNumber={version}");
        }

        /// <inheritdoc/>
        public async Task<ContractReminders> GetContractRemindersAsync(
            uint reminderInterval = 14,
            uint page = 1,
            uint count = 10,
            ContractSortOptions sort = ContractSortOptions.LastUpdatedAt,
            SortDirection order = SortDirection.Asc)
        {
            _logger.LogInformation($"Retrieving a contract reminders for reminderInterval : {reminderInterval}, pageNumber : {page}, pageSize : {count}, sort : {sort} and order : {order}");
            return await Get<ContractReminders>($"/api/contractReminders?reminderInterval={reminderInterval}&page={page}&count={count}&sort={sort}&order={order}");
        }

        /// <inheritdoc/>
        public async Task UpdateContractReminderAsync(ContractReminderItem contractReminderItem)
        {
            _logger.LogInformation($"Updating LastEmailReminderSent and LastUpdatedAt.");
            await Patch($"/api/contractReminder", new ContractIdentifier
            {
                Id = contractReminderItem.Id,
                ContractNumber = contractReminderItem.ContractNumber,
                ContractVersion = contractReminderItem.ContractVersion
            });
        }

        /// <inheritdoc/>
        protected override Action<ApiGeneralException> FailureAction
            => exception =>
            {
                _logger.LogError(exception, exception.Message);
                throw exception;
            };
    }
}