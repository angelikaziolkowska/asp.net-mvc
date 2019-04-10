using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mvc4_Development_Test.Home;

namespace Mvc4_Development_Test.UploadDetails
{

    /// <summary>
    /// Hold the details for a _File table in db
    /// </summary>
    public class _FileBO
    {
        public string _File_Name { get; set; }

        public DateTime File_DateT { get; set; }

        public int Customer_Count { get; set; }

        /// <summary>
        /// The ID of the Event Member
        /// </summary>
        public int Event_ID { get; set; }

        public Status Status_ID { get; set; } = 5;

        public string Temp_Table_ID { get; set; }
    }
}