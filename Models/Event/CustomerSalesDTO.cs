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
    public class CustomerSalesDTO
    {
        /// <summary>
        /// List of users
        /// </summary>
        public List<EventBO> EventList { get; set; }

        public EventBO EventMember { get; set; }
        public List<RegionBO> RegionList { get; internal set; }

        public List<CustomerBO> CustomerList { get; set; }

        public List<MonthBO> MonthList { get; set; }

        public List<CustomerSalesBO> CustomerSalesList { get; set; }

    }

    /// <summary>
    /// Holds all the data to pass to the CreateEventMember view
    /// </summary>
    public class CreateCustomerSalesDTO
    {
        public CreateCustomerSalesDTO()
        {
            EventMember = new EventBO();
            RegionMember = new RegionBO();
            CustomerMember = new CustomerBO();
            CustomerSalesMember = new CustomerSalesBO();

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

        public CustomerSalesBO CustomerSalesMember { get; set; }

        public List<CustomerSalesBO> CustomerSalesList { get; set; }
    }
}