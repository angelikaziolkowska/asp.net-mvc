using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Mvc4_Development_Test.Home.StaffBO;

namespace Mvc4_Development_Test.Customer
{


    /// <summary>
    /// Hold the details for a Service Member
    /// </summary>
    public class StatusBO
    {
        /// <summary>
        /// The Service ID
        /// </summary>
        [Display(Name = "Statuses of Interest")]
        public string Status_Name { get; set; }

        /// <summary>
        /// ID of Service
        /// </summary>
        [Required]
        public Status Status_ID { get; set; }

    }   
}