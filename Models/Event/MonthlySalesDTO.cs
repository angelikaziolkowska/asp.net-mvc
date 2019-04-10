using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc4_Development_Test.Customer;

namespace Mvc4_Development_Test.Event
{
    /// <summary>
    /// Holds all the data to pass to the Event Index
    /// </summary>
    public class MonthlySalesDTO
    {
        /// <summary>
        /// List of users
        /// </summary>
        public List<EventBO> EventList { get; set; }

        public EventBO EventMember { get; set; }
        public List<RegionBO> RegionList { get; internal set; }

        public List<CustomerBO> CustomerList { get; set; }

        public List<MonthBO> MonthList { get; set; }

        public List<MonthlySalesBO> MonthlySalesList { get; set; }

        public int Month_ID { get; set; }

        public int? Event_ID { get; set; }

    }

    /// <summary>
    /// Holds all the data to pass to the CreateEventMember view
    /// </summary>
    public class CreateMonthlySalesDTO
    {
        public CreateMonthlySalesDTO()
        {
            EventMember = new EventBO();
            RegionMember = new RegionBO();
            CustomerMember = new CustomerBO();
            MonthlySalesMember = new MonthlySalesBO();

        }

        /// <summary>
        /// Event member to create
        /// </summary>
        public EventBO EventMember { get; set; }

        public RegionBO RegionMember { get; set; }
        public List<EventBO> EventList { get; internal set; }

        public List<RegionBO> RegionList { get; set; }

        public CustomerBO CustomerMember { get; set; }
        public List<CustomerBO> CustomerList { get; set; }

        public MonthlySalesBO MonthlySalesMember { get; set; }

        public List<MonthlySalesBO> MonthlySalesList { get; set; }
    }
}