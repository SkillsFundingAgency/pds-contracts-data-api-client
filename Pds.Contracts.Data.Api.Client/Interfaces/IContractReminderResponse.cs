using Pds.Contracts.Data.Api.Client.Models;

namespace Pds.Contracts.Data.Api.Client.Interfaces
{
    /// <summary>
    /// Contract reminder response interface.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public interface IContractReminderResponse<T>
    {
        /// <summary>
        /// Gets or sets data.
        /// </summary>
        T Contracts { get; set; }

        /// <summary>
        /// Gets or sets meta.
        /// </summary>
        Metadata Paging { get; set; }
    }
}