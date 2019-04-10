using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc4_Development_Test.Event;

namespace Mvc4_Development_Test.UploadDetails
{


    /// <summary>
    /// Holds all the data to pass to the CreateEventMember view
    /// </summary>
    public class UploadDetailsDTO
    {
        public UploadDetailsDTO()
        {
            UploadDetailsMember = new UploadDetailsBO();

        }

        /// <summary>
        /// Event member to create
        /// </summary>
        public UploadDetailsBO UploadDetailsMember { get; set; }


        public List<UploadDetailsBO> UploadDetailsList { get; internal set; }
        public List<EventBO> EventList { get; internal set; }
    }
}