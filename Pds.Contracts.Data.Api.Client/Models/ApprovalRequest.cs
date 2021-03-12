using System.ComponentModel.DataAnnotations;

namespace Pds.Contracts.Data.Api.Client.Models
{
    /// <summary>
    /// Request object for update last email reminder sent.
    /// </summary>
    public class ApprovalRequest : ContractIdentifier
    {
        /// <summary>
        /// Gets or sets the blob file name.
        /// </summary>
        [Required]
        public string FileName { get; set; }
    }
}