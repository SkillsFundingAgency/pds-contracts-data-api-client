using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pds.Contracts.Data.Api.Client.Enumerations
{
    /// <summary>
    /// Enumeration to detemine the sort order.
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// Ascending sort order.
        /// </summary>
        [Display(Name = "Asc", Description = "Ascending")]
        Asc = 0,

        /// <summary>
        /// Descending sort order.
        /// </summary>
        [Display(Name = "Desc", Description = "Descending")]
        Desc = 1
    }
}
