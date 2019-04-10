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
    public class TitleBO
    {
        /// <summary>
        /// ID of Service
        /// </summary>
        [Required]
        [Display(Name = "Title")]
        public Titles Title_ID { get; set; }

        [Display(Name = "Title")]
        public string Title_Desc { get; set; }

    }
}