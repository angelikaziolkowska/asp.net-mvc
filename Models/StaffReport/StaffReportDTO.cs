using System;
using System.Collections.Generic;
using Mvc4_Development_Test.Home;
using System.Linq;
using System.Web;



namespace Mvc4_Development_Test.StaffReport
{
    /// <summary>
    /// Holds all the data to pass to the Report Index
    /// </summary>
    public class StaffReportDTO
    {
        /// <summary>
        /// List of users
        /// </summary>
        public List<Staff_ReportBO> StaffReportList { get; set; }

        public List<StaffFilterBO> StaffFilterList { get; set; }

        public List<StaffFilterValBO> StaffFilterValList { get; set; }

        public List<StaffBO> StaffList { get; set; }

        public List<DepartmentBO> DepartmentList { get; set; }

        public int? SearchDepartment { get; set; }

        public int? SearchFilter { get; set; }
    }
}