using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mvc4_Development_Test.Home;

namespace Mvc4_Development_Test.UploadDetails
{

    /// <summary>
    /// Hold the details for a Event Member
    /// </summary>
    public class TempCustBO
    {
        public int Customer_ID { get; set; }

        public Titles Title_ID { get; set; }

        public string Customer_Name { get; set; }

        public string Customer_School { get; set; }

        public EmailAddressAttribute Customer_Email { get; set; }

        public DateTime Customer_RegDateT { get; set; }

        public Status Status_ID { get; set; } = Status.Active;

        public int Staff_ID { get; set; }

        public DateTime Edit_DateT { get; set; }

        public int Event_ID { get; set; }
    }
}