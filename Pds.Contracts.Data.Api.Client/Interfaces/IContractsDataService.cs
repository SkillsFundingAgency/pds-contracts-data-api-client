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
        /// <param name="id">The identifier.</param>
        /// <returns>The hello string.</returns>
        Task<Models.Contract> GetAsync(int id);

        /// <summary>
        /// Gets the by contract number and version asynchronous.
        /// </summary>
        /// <param name="contractNumber">The contract number.</param>
        /// <param name="version">The version.</param>
        /// <returns>A contract <see cref="Models.Contract"/>.</returns>
        Task<Models.Contract> GetByContractNumberAndVersionAsync(string contractNumber, int version);

        /// <summary>
        /// Get contract reminders.
        /// </summary>
        /// <param name="reminderInterval">Interval in days.</param>
        /// <param name="pageNumber">The page number to return.</param>
        /// <param name="pageSize">The number of records in the page.</param>
        /// <param name="sort">Sort parameters to apply.</param>
        /// <param name="order">The order in which to .</param>
        /// <returns>Returns a list of contract reminder response.</returns>
        Task<ContractReminderResponse<IEnumerable<ContractReminderItem>>> GetContractRemindersAsync(int reminderInterval, int pageNumber, int pageSize, ContractSortOptions sort, SortDirection order);

        /// <summary>
        /// Update the LastEmailReminderSent and LastUpdatedAt for a provided id, contract number
        /// and contract version.
        /// </summary>
        /// <param name="request">
        /// An UpdateLastEmailReminderSentRequest model containing id, contract number and contract version.
        /// </param>
        /// <returns>Returns an integer denoting the resultant status code.</returns>
        Task UpdateContractReminderAsync(UpdateLastEmailReminderSentRequest request);
    }
}