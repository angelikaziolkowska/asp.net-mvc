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
    public class StaffFilterValBO
    {
        /// <summary>
        /// The ID of the Staff Filter Member
        /// </summary>
        [Display(Name = "Value ID")]
        [Required]
        public int? StaffFilterVal_ID { get; set; }

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
        [Display(Name = "Value Name")]
        public string StaffFilterVal_Name { get; set; }

        /// <summary>
        /// The ID of the Staff Filter Member
        /// </summary>
        [Display(Name = "Value Order")]
        [Required]
        public int? StaffFilterVal_Order { get; set; }






    }
}