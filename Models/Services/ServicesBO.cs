using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Mvc4_Development_Test.Home.StaffBO;

namespace Mvc4_Development_Test.Services
{


    /// <summary>
    /// Hold the details for a Customer Member
    /// </summary>
    public class ServicesBO
    {
        /// <summary>
        /// The Service ID
        /// </summary>
        [Display(Name = "Services of Interest")]
        public string Service_Name { get; set; }

        /// <summary>
        /// ID of Service
        /// </summary>
        [Required]
        public Service Service_ID { get; set; }

    }
    
    
}