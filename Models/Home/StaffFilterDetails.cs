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
    public class StaffFilterDetailsBO
    {
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "Filter")]
        public string StaffFilter_Name { get; set; }

        /// <summary>
        /// The Name of the Staff Filter Member
        /// </summary>

        [StringLength(30, MinimumLength = 2)]
        [Display(Name="Value")]
        public string StaffFilterVal_Name { get; set; }
    }
}