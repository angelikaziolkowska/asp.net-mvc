using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc4_Development_Test.Home
{

    /// <summary>
    /// Hold the details for a Staff Filter Member
    /// </summary>
    public class StaffFilterLinkBO
    {
        /// <summary>
        /// The ID of the Staff Member
        /// </summary>
        [Display(Name = "Staff ID")]
        [Required]
        public int? Staff_ID { get; set; }

        /// <summary>
        /// The ID of the Staff Filter Member
        /// </summary>
        [Display(Name = "Filter ID")]
        [Required]
        public int? StaffFilter_ID { get; set; }

        /// <summary>
        /// The ID of the Staff Filter Member
        /// </summary>
        [Display(Name = "Filter Value ID")]
        [Required]
        public int? StaffFilterVal_ID { get; set; }
    }
}