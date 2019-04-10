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
    public class StaffFilterBO
    {
        /// <summary>
        /// The ID of the Staff Filter Member
        /// </summary>
        [Display(Name = "Filter ID")]
        [Required]
        public int? StaffFilter_ID { get; set; }

        /// <summary>
        /// The Name of the Staff Filter Member
        /// </summary>
        
        [StringLength(30, MinimumLength = 2)]
        [Display(Name="Filter")]
        public string StaffFilter_Name { get; set; }

        /// <summary>
        /// The Status of the Staff Filter
        /// </summary>
        [Display(Name = "Filter Status")]
        public Status Status_ID { get; set; }

    }



}