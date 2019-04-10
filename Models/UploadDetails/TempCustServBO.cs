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
    public class TempCustServBO
    {
        //[Display(Name = "Staff")]
        //public List<StaffBO> StaffList { get; set; }


        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please select a file to upload.")]
        public HttpPostedFileBase File { get; set; }

        public DateTime File_DateT { get; set; }

        public int Customer_Count { get; set; } = 0;

        public Status Status_ID { get; set; } = Status.Active;

        /// <summary>
        /// The ID of the Event Member
        /// </summary>
        [Display(Name = "Select Event")]
        [Required(ErrorMessage = "Please select an event.")]
        public int Event_ID { get; set; }

        public List<List<string>> ParsedData { get; set; }

        public int Customer_ID { get; set; }

        public string Temp_Table_ID { get; set; } = Guid.NewGuid().ToString().TrimEnd();

    }



}