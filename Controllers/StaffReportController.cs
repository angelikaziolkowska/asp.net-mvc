using Mvc4_Development_Test.StaffReport;
using Mvc4_Development_Test.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc4_Development_Test.Controllers
{
    public class StaffReportController : Controller
    {
        // Store the home DAL
        StaffReportDAL rDAL;
        HomeDAL hDAL;

        public StaffReportController()
        {
            // Initialise the Home DAL
            rDAL = new StaffReportDAL();
            hDAL = new HomeDAL();
        }

        /// <summary>
        /// Index Page
        /// </summary>
        /// <returns></returns>
        public ActionResult StaffReportIndex(int? SearchFilter, int? SearchDepartment)
        {

            // Create a data transfer object to pass all data to the view
            StaffReportDTO dto = new StaffReportDTO
            {
                // Populate the list of users
                StaffReportList = rDAL.GetStaffReportList(SearchFilter, SearchDepartment),

                StaffFilterList = hDAL.GetListOfStaffFilters(),

                StaffFilterValList = hDAL.GetListOfStaffFilterVals(),

                StaffList = hDAL.GetListOfStaff(),

                DepartmentList = hDAL.GetListOfDepartments(),

                SearchDepartment = SearchDepartment,

                SearchFilter = SearchFilter,

            };

            // Pass the data to the view
            return View(dto);
        }
    }
}
