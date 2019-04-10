using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc4_Development_Test.StaffReport
{

    /// <summary>
    /// Hold the details for a Staff Member
    /// </summary>
    public class Staff_ReportBO
    {
        /// <summary>
        /// The Name of the Staff Member
        /// </summary>

        [StringLength(40, MinimumLength = 2)]
        [Display(Name = "Name (ID)")]
        public string Staff_Name { get; set; }

        /// <summary>
        /// The ID of the Staff Member
        /// </summary>
        [Display(Name = "ID")]
        public int Staff_ID { get; set; }

        /// <summary>
        /// The Name of the Staff Member
        /// </summary>
        [Display(Name = "Department")]
        public string Department { get; set; }

        /// <summary>
        /// The ID of the Staff Member
        /// </summary>
        [Display(Name = "Registered")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? Registered { get; set; }

        /// <summary>
        /// The Status name of the user
        /// </summary>
        [Display(Name = "Status")]
        public string Status { get; set; }

        /// <summary>
        /// The ID of the Staff Member
        /// </summary>
        [Display(Name = "Customer Count")]
        public int Customer_Count { get; set; }

    }

}