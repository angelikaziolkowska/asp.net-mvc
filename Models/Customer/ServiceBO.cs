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
    public class ServiceBO
    {

        /// <summary>
        /// ID of Service
        /// </summary>
        [Required]
        public Service Service_ID { get; set; }

        public string Service_Name { get; set; }

        public bool IsChecked { get; set; }
    }

    //public class Service_Names
    //{
    //    /// <summary>
    //    /// Customer's Services
    //    /// </summary>
    //    public int Analytics { get; set; }
    //    public int Observe { get; set; }
    //    public int Influence { get; set; }
    //}

    //public class CheckedService : ServiceBO
    //{

    //}
}