using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc4_Development_Test.Home;

namespace Mvc4_Development_Test.Services
{
    /// <summary>
    /// Holds all the data to pass to the Customer Index
    /// </summary>
    public class ServiceDTO
    {
        public List<ServicesBO> ServiceList;

    }

    /// <summary>
    /// Holds all the data to pass to the CreateCustomerMember view
    /// </summary>
    public class CreateServiceDTO
    {
        public CreateServiceDTO()
        {
            ServiceMember = new ServicesBO();
            ServiceList = PopulateServiceList();
        }

        /// <summary>
        /// Service
        /// </summary>
        public List<ServicesBO> ServiceList { get; set; }


        private List<ServicesBO> PopulateServiceList()
        {
            servicesDAL serviceDAL = new ServicesDAL();
            return servicesDAL.GetListOfServices();
        }
    }
}