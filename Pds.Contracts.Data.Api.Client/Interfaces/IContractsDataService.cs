using Pds.Contracts.Data.Api.Client.Enumerations;
using Pds.Contracts.Data.Api.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pds.Contracts.Data.Api.Client.Interfaces
{
    /// <summary>
    /// Interface for the Contracts Data API client.
    /// </summary>
    public interface IContractsDataService
    {
        /// <summary>
        /// Gets a contract by Id.
        /// </summary>
        /// <param name="id">The unique identifier should be used to find a contract.</param>
        /// <returns>An instance of <see cref="Models.Contract"/> corresponding to the id.</returns>
        /// <exception cref="Core.ApiClient.Exceptions.ApiGeneralException">
        /// An ApiGeneralException will be thrown with appropriate <see cref="System.Net.HttpStatusCode"/> with a reason for failure
        /// - when a contract cannot be found based on the id.
        /// - if there are internal server exceptions.
        /// - if the client cannot be authenticated.
        /// </exception>
        Task<Contract> GetContractByIdAsync(int id);

        /// <summary>
        /// Gets the contract by contract number and version number.
        /// </summary>
        /// <param name="contractNumber">The contract number.</param>
        /// <param name="version">The version number of contract.</param>
        /// <returns>An instance of <see cref="Models.Contract"/> corresponding to the contract number and version number.</returns>
        /// <exception cref="Core.ApiClient.Exceptions.ApiGeneralException">
        /// An ApiGeneralException will be thrown
        /// - when a contract cannot be found based on the contract number and version number combination.
        /// - if there are internal server exceptions.
        /// - if the client cannot be authenticated.
        /// </exception>
        Task<Contract> GetContractAsync(string contractNumber, int version);

        /// <summary>
        /// Gets contract reminders.
        /// </summary>
        /// <param name="reminderInterval">Interval in days.</param>
        /// <param name="page">The page number to return.</param>
        /// <param name="count">The number of records in the page.</param>
        /// <param name="sort">Sort parameters to apply.</param>
        /// <param name="order">The order in which to .</param>
        /// <returns>Returns a list of contract reminder response <see cref="ContractReminders"/>.</returns>
        /// <exception cref="Pds.Core.ApiClient.Exceptions.ApiGeneralException">
        /// An ApiGeneralException will be thrown
        /// - if there are any invalid inputs.
        /// - if there are internal server exceptions.
        /// - if the client cannot be authenticated.
        /// </exception>
        Task<ContractReminders> GetContractRemindersAsync(
            uint reminderInterval = 14,
            uint page = 1,
            uint count = 10,
            ContractSortOptions sort = ContractSortOptions.LastUpdatedAt,
            SortDirection order = SortDirection.Asc);

        /// <summary>
        /// Updates the contract as reminded by id, contract number and version number combination.
        /// </summary>
        /// <param name="contractReminderItem">
        /// An <see cref="ContractReminderItem"/> model with id, contract number and contract version to be used for reminder update.
        /// </param>
        /// <returns>Returns async task completion.</returns>
        /// <exception cref="Core.ApiClient.Exceptions.ApiGeneralException">
        /// An ApiGeneralException will be thrown
        /// - when a contract cannot be found based on the id, contract number and version number combination.
        /// - if there are any invalid inputs.
        /// - if there are internal server exceptions.
        /// - if the client cannot be authenticated.
        /// </exception>
        Task UpdateContractReminderAsync(ContractReminderItem contractReminderItem);
    }
}