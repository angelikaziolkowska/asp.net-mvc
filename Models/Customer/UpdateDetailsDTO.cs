using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc4_Development_Test.Home;

namespace Mvc4_Development_Test.Customer
{
    /// <summary>
    /// Holds all the data to pass to the Customer Index
    /// </summary>
    public class UpdateDetailsDTO
    {


        /// <summary>
        /// List of users
        /// </summary>
        public List<CustomerBO> CustomerList { get; set; }

        /// <summary>
        /// List of titles
        /// </summary>
        public List<TitleBO> TitleList { get; set; }

        public CustomerBO CustomerMember { get; set; }

        public TitleBO TitleMember { get; set; }




    }

    /// <summary>
    /// Holds all the data to pass to the CreateCustomerMember view
    /// </summary>
    public class UpdateCustomerDTO
    {
        public UpdateCustomerDTO()
        {
            CustomerMember = new CustomerBO();
        }

        /// <summary>
        /// Customer to update
        /// </summary>
        public CustomerBO CustomerMember { get; set; }

        /// <summary>
        /// Title to update
        /// </summary>
        public TitleBO TitleMember { get; set; }

        /// <summary>
        /// List of titles
        /// </summary>
        public List<TitleBO> TitleList { get; set; }


    }
}
