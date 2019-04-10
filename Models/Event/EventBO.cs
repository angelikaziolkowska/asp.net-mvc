using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc4_Development_Test.Event
{

    /// <summary>
    /// Hold the details for a Event Member
    /// </summary>
    public class EventBO
    {
        /// <summary>
        /// The ID of the Event Member
        /// </summary>
        [Display(Name = "ID")]
        [Required]
        public int Event_ID { get; set; }

        /// <summary>
        /// The Name of the Event Member
        /// </summary>
        
        [StringLength(60, MinimumLength = 2)]
        [Display(Name="Name")]
        [Required]
        public string Event_Name { get; set; }

        /// <summary>
        /// The ID of the Region
        /// </summary>
        [Required]
        [Display(Name = "Region")]
        public int Region_ID { get; set; }


        /// <summary>
        /// The Name of the Region
        /// </summary>

        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "Region")]
        public string Region_Name { get; set; }

        /// <summary>
        /// Get the Event Date
        /// </summary>
        [Display(Name = "Date")]
        public DateTime Event_DateT { get; set; }

        
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }




    }

    /// <summary>
    /// Holds the details for a department
    /// </summary>
    public class RegionBO
    {
        /// <summary>
        /// The ID of the department
        /// </summary>
        [Display(Name = "Department")]
        public int? Region_ID { get; set; }

        /// <summary>
        /// The Name of the Staff Member
        /// </summary>
        [Display(Name = "Department")]
        public string Region_Name { get; set; }

    }

    /// <summary>
    /// Holds the details for a month
    /// </summary>
    public class MonthBO
    {
        /// <summary>
        /// The ID of the month
        /// </summary>
        [Display(Name = "Month")]
        public int Month_ID { get; set; }

        /// <summary>MonthStaff Member
        /// </summary>
        [Display(Name = "Month")]
        public string Month_Name { get; set; }

    }

    /// <summary>
    /// Holds the details for a month
    /// </summary>
    public class CustomerSalesBO
    {
        /// <summary>
        /// The ID of the month
        /// </summary>
        [Display(Name = "Month")]
        public int Month_ID { get; set; }

        /// <summary>MonthStaff Member
        /// </summary>
        [Display(Name = "Month")]
        public string Month_Name { get; set; }

        [Display(Name = "Event Count")]
        public int Event_Count { get; set; }

        [Display(Name = "Customer Count")]
        public int Customer_Count { get; set; }

    }

    /// <summary>
    /// Holds the details for a month
    /// </summary>
    public class MonthlySalesBO
    {
        /// <summary>
        /// The ID of the month
        /// </summary>
        [Display(Name = "Event")]
        public int Event_ID { get; set; }

        /// <summary>MonthStaff Member
        /// </summary>
        [Display(Name = "Events")]
        public string Event_Name { get; set; }


        [Display(Name = "Customer Count")]
        public int Customer_Count { get; set; }

    }

}