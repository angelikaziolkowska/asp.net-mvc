using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mvc4_Development_Test.Home;
using PagedList;
using PagedList.Mvc;

namespace Mvc4_Development_Test.Customer
{
    /// <summary>
    /// Holds all the data to pass to the Customer Index
    /// </summary>
    /// 

    public class CustomerDTO
    {
        /// <summary>
        /// List of services
        /// </summary>
        public List<ServiceBO> ServiceList { get; set; }

        /// <summary>
        /// List of statuses
        /// </summary>
        public List<StatusBO> StatusList { get; set; }

        /// <summary>
        /// List of users
        /// </summary>
        public IPagedList<CustomerBO> CustomerList { get; set; }

        /// <summary>
        /// List of customer services
        /// </summary>
        [Display(Name="Services")]
        public List<CustomerServiceBO> CustomerServiceList { get; set; } 

        public List<TitleBO> TitleList { get; set; }

        public List<StaffBO> StaffList { get; set; }


        public int? Page { get; set; }

        public int PageSize { get; set; }

    }

    /// <summary>
    /// Holds all the data to pass to the CreateCustomerMember view
    /// </summary>
    public class CreateCustomerDTO
    {
        public CreateCustomerDTO()
        {
            CustomerMember = new CustomerBO();
            ServiceMember = new ServiceBO();
            StatusMember = new StatusBO();
            CustomerServiceMember = new CustomerServiceBO();
            TitleMember = new TitleBO();
            StaffMember = new StaffBO();

        }

        /// <summary>
        /// List of staff
        /// </summary>
        public List<StaffBO> StaffList { get; set; }

        public StaffBO StaffMember { get; set; }

        /// <summary>
        /// Titles
        /// </summary>
        public List<TitleBO> TitleList { get; set; }

        /// <summary>
        /// Customer member to create
        /// </summary>
        public CustomerBO CustomerMember { get; set; }


        /// <summary>
        /// Customer Statuses to create
        /// </summary>
        public StatusBO StatusMember { get; set; }
       
        public List<StatusBO> StatusList { get; set; }

        /// <summary>
        /// Customer services to create
        /// </summary>
        
        public ServiceBO ServiceMember { get; set; }

        /// <summary>
        /// Service
        /// </summary>
        public List<ServiceBO> ServiceList { get; set; }


        public CustomerServiceBO CustomerServiceMember { get; set; }

        public TitleBO TitleMember { get; set; }

        /// <summary>
        /// Service
        /// </summary>
        public List<CustomerServiceBO> CustomerServiceList { get; set; }
    }
}