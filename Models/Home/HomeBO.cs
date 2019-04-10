using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc4_Development_Test.Home
{

    /// <summary>
    /// Hold the details for a Staff Member
    /// </summary>
    public class StaffBO
    {
        /// <summary>
        /// The ID of the Staff Member
        /// </summary>
        [Display(Name = "ID")]
        [Required]
        public int Staff_ID { get; set; }

        /// <summary>
        /// The Name of the Staff Member
        /// </summary>
        
        [StringLength(40, MinimumLength = 2)]
        [Display(Name="Name")]
        public string Staff_Name { get; set; }

        /// <summary>
        /// The ID of the department
        /// </summary>
        [Required]
        [Display(Name = "Department")]
        public int Department_ID { get; set; }


        /// <summary>
        /// The Name of the Staff Member
        /// </summary>

        [StringLength(40, MinimumLength = 2)]
        [Display(Name = "Department")]
        public string Department_Name { get; set; }

        /// <summary>
        /// The Status of the user
        /// </summary>
        [Display(Name = "Status")]
        public Status Status_ID { get; set; }


        /// <summary>
        /// The ID of the filter
        /// </summary>
        [Required]
        [Display(Name = "Filter")]
        public List<int> StaffFilterVal_ID { get; set; }

        /// <summary>
        /// Get the Registration Date of Staff
        /// </summary>
        [Display(Name = "Registered")]
        public DateTime Staff_RegDateT { get; set; }


    }

    /// <summary>
    /// Holds the details for a department
    /// </summary>
    public class DepartmentBO
    {

        /// <summary>
        /// The ID of the department
        /// </summary>
        [Display(Name = "Department")]
        public int? Department_ID { get; set; }

        /// <summary>
        /// The Name of the Staff Member
        /// </summary>
        [Display(Name = "Department")]
        public string Department_Name { get; set; }

        /// <summary>
        /// The Status of the user
        /// </summary>
        [Display(Name = "Status")]
        public Status Status_ID { get; set; }
    }

}