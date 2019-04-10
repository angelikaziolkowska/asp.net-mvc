using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc4_Development_Test.Home
{
    /// <summary>
    /// Holds all the data to pass to the Home Index
    /// </summary>
    public class HomeDTO
    {
        /// <summary>
        /// List of users
        /// </summary>
        public List<StaffBO> StaffList { get; set; }

        public StaffBO StaffMember { get; set; }

    }

    public class StaffDetailsDTO
    {
        public StaffBO StaffMember { get; set; }

        public List<StaffFilterDetailsBO> StaffFilterDetailList { get; set; }

    }

    /// <summary>
    /// Holds all the data to pass to the CreateStaffMember view
    /// </summary>
    public class CreateStaffDTO
    {
        public CreateStaffDTO()
        {
            StaffMember = new StaffBO();
            StaffFilterMember = new StaffFilterBO();
            StaffFilterLinkMember = new StaffFilterLinkBO();
            StaffFilterValMember = new StaffFilterValBO();
        }

        /// <summary>
        /// List of departments
        /// </summary>
        public List<DepartmentBO> DepartmentList { get; set; }


        public List<StaffFilterBO> StaffFilterList { get; set; }

        public List<StaffFilterLinkBO> StaffFilterLinkList { get; set; }

        public List<StaffFilterValBO> StaffFilterValList { get; set; }

        /// <summary>
        /// Staff member to create
        /// </summary>
        public StaffBO StaffMember { get; set; }

        public StaffFilterBO StaffFilterMember { get; set; }

        public StaffFilterLinkBO StaffFilterLinkMember { get; set; }

        public StaffFilterValBO StaffFilterValMember { get; set; }


    }
}