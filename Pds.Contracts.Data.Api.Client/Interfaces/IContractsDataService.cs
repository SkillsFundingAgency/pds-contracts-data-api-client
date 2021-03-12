using Pds.Contracts.Data.Api.Client.Enumerations;
using Pds.Contracts.Data.Api.Client.Models;
using System;
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
        /// Creates a contract asynchronously.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="Exceptions.ContractBadRequestClientException">Will be thrown if there are any invalid inputs.</exception>
        /// <exception cref="UnauthorizedAccessException">Will be thrown if the credentials supplied to access the API is invalid.</exception>
        /// <exception cref="Exceptions.DuplicateContractClientException">Will be thrown if there is already an existing contract with same contract number and version.</exception>
        /// <exception cref="Exceptions.ContractWithHigherVersionAlreadyExistsClientException">Will be thrown if there is already an existing contract for the same contract number with higher version.</exception>
        /// <exception cref="Core.ApiClient.Exceptions.ApiGeneralException">
        /// An ApiGeneralException will be thrown
        /// - if there are internal server exceptions.
        /// </exception>
        Task CreateContractAsync(CreateRequest contract);

        /// <summary>
        /// Approve contract manually.
        /// </summary>
        /// <param name="approvalRequest">The approval request.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Exceptions.ContractBadRequestClientException">Will be thrown if there are any invalid inputs.</exception>
        /// <exception cref="Exceptions.ContractNotFoundClientException">Will be thrown if a contract or its PDF cannot be found for the given input.</exception>
        /// <exception cref="UnauthorizedAccessException">Will be thrown if the credentials supplied to access the API is invalid.</exception>
        /// <exception cref="Exceptions.ContractUpdateConcurrencyClientException">Will be thrown if the contract may have been modified or deleted since Contract were loaded.</exception>
        /// <exception cref="Exceptions.ContractStatusClientException">Will be thrown if the contract is not in correct status for Manual approval.</exception>
        /// <exception cref="Core.ApiClient.Exceptions.ApiGeneralException">
        /// An ApiGeneralException will be thrown
        /// - if there are internal server exceptions.
        /// </exception>
        Task ManualApproveAsync(ApprovalRequest approvalRequest);

        /// <summary>
        /// Confirms approval of the contract.
        /// </summary>
        /// <param name="approvalRequest">The approval request.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Exceptions.ContractBadRequestClientException">Will be thrown if there are any invalid inputs.</exception>
        /// <exception cref="Exceptions.ContractNotFoundClientException">Will be thrown if a contract or its PDF cannot be found for the given input.</exception>
        /// <exception cref="UnauthorizedAccessException">Will be thrown if the credentials supplied to access the API is invalid.</exception>
        /// <exception cref="Exceptions.ContractStatusClientException">Will be thrown if the contract is not in correct status for approval confirmation.</exception>
        /// <exception cref="Core.ApiClient.Exceptions.ApiGeneralException">
        /// An ApiGeneralException will be thrown
        /// - if there are internal server exceptions.
        /// </exception>
        Task ConfirmApprovalAsync(ApprovalRequest approvalRequest);

        /// <summary>
        /// Withdraws the contract.
        /// </summary>
        /// <param name="withdrawalRequest">The withdrawal request.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Exceptions.ContractBadRequestClientException">Will be thrown if there are any invalid inputs.</exception>
        /// <exception cref="Exceptions.ContractNotFoundClientException">Will be thrown if a contract or its PDF cannot be found for the given input.</exception>
        /// <exception cref="UnauthorizedAccessException">Will be thrown if the credentials supplied to access the API is invalid.</exception>
        /// <exception cref="Exceptions.ContractUpdateConcurrencyClientException">Will be thrown if the contract may have been modified or deleted since Contract were loaded.</exception>
        /// <exception cref="Exceptions.ContractStatusClientException">Will be thrown if the contract is not in correct status for Manual approval.</exception>
        /// <exception cref="Core.ApiClient.Exceptions.ApiGeneralException">
        /// An ApiGeneralException will be thrown
        /// - if there are internal server exceptions.
        /// </exception>
        Task WithdrawAsync(WithdrawalRequest withdrawalRequest);

        /// <summary>
        /// Updates the contract as reminded by id, contract number and version number combination.
        /// </summary>
        /// <param name="contractReminderItem">
        /// An <see cref="ContractReminderItem"/> model with id, contract number and contract version to be used for reminder update.
        /// </param>
        /// <returns>Returns async task completion.</returns>
        /// <exception cref="Exceptions.ContractNotFoundClientException">Will be thrown if a contract or its PDF cannot be found for the given input.</exception>
        /// <exception cref="UnauthorizedAccessException">Will be thrown if the credentials supplied to access the API is invalid.</exception>
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