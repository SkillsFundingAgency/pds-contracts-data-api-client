using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pds.Contracts.Data.Api.Client.Enumerations;
using Pds.Contracts.Data.Api.Client.Interfaces;
using Pds.Contracts.Data.Api.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pds.Contracts.Data.Api.Client.TestApi.Controllers
{
    /// <summary>
    /// The contract (data) Test Api controller.
    /// </summary>
    [Route("api/test")]
    [ApiController]
    public class ContractsDataServiceTestController : ControllerBase
    {
        private readonly IContractsDataService _contractsDataClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractsDataServiceTestController"/> class.
        /// </summary>
        /// <param name="contractsDataClient">Dependancy injected contract data service client.</param>
        public ContractsDataServiceTestController(IContractsDataService contractsDataClient)
        {
            _contractsDataClient = contractsDataClient;
        }

        /// <summary>
        /// Gets contract by id.
        /// </summary>
        /// <param name="id">The unique identifier of contract.</param>
        /// <returns>Returns a contract.</returns>
        [HttpGet("/api/GetById")]
        public async Task<Models.Contract> Get(int id = 129874)
        {
            var result = await _contractsDataClient.GetAsync(id);
            return result;
        }

        /// <summary>
        /// Gets the contract by contract number and version.
        /// </summary>
        /// <param name="contractNumber">The contract number.</param>
        /// <param name="versionNumber">The version number.</param>
        /// <returns>Returns a contract.</returns>
        [HttpGet("/api/GetByNumberAndVersion")]
        public async Task<Models.Contract> GetByContractNumberAndVersionAsync(string contractNumber = "CityDeals-0001", int versionNumber = 1)
        {
            var result = await _contractsDataClient.GetByContractNumberAndVersionAsync(contractNumber, versionNumber);
            return result;
        }

        /// <summary>
        /// Gets a list of unsigned contracts that are past their due date.
        /// </summary>
        /// <param name="reminderInterval">Interval in days.</param>
        /// <param name="page">The page number to return.</param>
        /// <param name="count">The number of records in the page.</param>
        /// <param name="sort">Sort parameters to apply.</param>
        /// <param name="order">The order in which to .</param>
        /// <returns>A list of contracts that are overdue.</returns>
        [HttpGet("/api/GetReminders")]
        public async Task<ContractReminderResponse<IEnumerable<ContractReminderItem>>> GetContractRemindersAsync(int reminderInterval = 14, int page = 1, int count = 10, ContractSortOptions sort = ContractSortOptions.LastUpdatedAt, SortDirection order = SortDirection.Asc)
        {
            var result = await _contractsDataClient.GetContractRemindersAsync(reminderInterval, page, count, sort, order);
            return result;
        }

        /// <summary>
        /// Update the LastEmailReminderSent and LastUpdatedAt for a provided id, contract number
        /// and contract version.
        /// </summary>
        /// <param name="request">
        /// An UpdateLastEmailReminderSentRequest model containing id, contract number and contract version.
        /// </param>
        /// <returns>A list of contracts that are overdue.</returns>
        [HttpPatch("/api/UpdateReminder")]
        public async Task UpdateLastEmailReminderSentAndLastUpdatedAtAsync(UpdateLastEmailReminderSentRequest request)
        {
            await _contractsDataClient.UpdateContractReminderAsync(request);
        }
    }
}