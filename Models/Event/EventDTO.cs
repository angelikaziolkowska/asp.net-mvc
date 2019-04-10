using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc4_Development_Test.Event
{
    /// <summary>
    /// Holds all the data to pass to the Event Index
    /// </summary>
    public class EventDTO
    {
        /// <summary>
        /// List of users
        /// </summary>
        public List<EventBO> EventList { get; set; }

        public EventBO EventMember { get; set; }
        public List<RegionBO> RegionList { get; internal set; }
    }



    /// <summary>
    /// Holds all the data to pass to the CreateEventMember view
    /// </summary>
    public class CreateEventDTO
    {
        public CreateEventDTO()
        {
            EventMember = new EventBO();
            RegionMember = new RegionBO();

        }

        /// <summary>
        /// Event member to create
        /// </summary>
        public EventBO EventMember { get; set; }

        public RegionBO RegionMember { get; set; }
        public List<EventBO> EventList { get; internal set; }

        public List<RegionBO> RegionList { get; set; }
    }
}