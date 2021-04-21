using System.Collections.Generic;

namespace Pds.Contracts.Data.Api.Client.Models
{
    /// <summary>
    /// ApiResponse.
    /// </summary>
    /// <typeparam name="T">Response data type.</typeparam>
    public class ContractReminders
    {
        /// <summary>
        /// Gets or sets contract reminders.
        /// </summary>
        public IEnumerable<ContractReminderItem> Contracts { get; set; }

        /// <summary>
        /// Gets or sets paging.
        /// </summary>
        public Paging Paging { get; set; }
    }
}