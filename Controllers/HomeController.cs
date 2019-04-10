using Mvc4_Development_Test.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc4_Development_Test.Controllers
{
    public class HomeController : Controller
    {
        // Store the home DAL
        HomeDAL hDAL;

        public HomeController()
        {
            // Initialise the Home DAL
            hDAL = new HomeDAL();
        }


        /// <summary>
        /// Index Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string sortOrder, string currentSort)
        {

            // Create a data transfer object to pass all data to the view
            HomeDTO dto = new HomeDTO
            {

                // Populate the list of users
                StaffList = hDAL.GetListOfStaff()
            };


            ViewBag.CurrentSort = sortOrder;

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "Staff_ID" : sortOrder;
           

            switch (sortOrder)
            {
                case "Staff_ID":
                    if (sortOrder.Equals(currentSort))
                        dto.StaffList = dto.StaffList.OrderByDescending
                                (m => m.Staff_ID).ToList();
                    else
                        dto.StaffList = dto.StaffList.OrderBy
                                (m => m.Staff_ID).ToList();
                    break;
                case "Staff_Name":
                    if (sortOrder.Equals(currentSort))
                        dto.StaffList = dto.StaffList.OrderByDescending
                                (m => m.Staff_Name).ToList();
                    else
                        dto.StaffList = dto.StaffList.OrderBy
                                (m => m.Staff_Name).ToList();
                    break;

                case "Status_ID":
                    if (sortOrder.Equals(currentSort))
                        dto.StaffList = dto.StaffList.OrderByDescending
                                (m => m.Status_ID).ToList();
                    else
                        dto.StaffList = dto.StaffList.OrderBy
                                (m => m.Status_ID).ToList();
                    break;
                case "Default":
                    dto.StaffList = dto.StaffList.OrderBy
                            (m => m.Staff_ID).ToList();
                    break;
            }


            // Pass the data to the view
            return View(dto);
        }

        /// <summary>
        /// GET Staff Details Page
        /// </summary>
        /// <returns></returns>
        public ActionResult StaffDetails(int id)
        {            
            // Create a data transfer object to pass all data to the view
            StaffDetailsDTO dto = new StaffDetailsDTO
            {
                StaffMember = hDAL.GetListOfStaff().Where(m => m.Staff_ID == id).FirstOrDefault(),
                // Populate the list of filter details for the staff
                StaffFilterDetailList = hDAL.GetListOfStaffFilterDetails(id),
            };

            // Pass the data to the view
            return View(dto);
        }

        /// <summary>
        /// GET: Create a staff member
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateStaffMember()
        {
            // Create a dto to hold the information we need to pass to the view
            CreateStaffDTO dto = new CreateStaffDTO
            {

                // Get the list of departments
                DepartmentList = hDAL.GetListOfDepartments(),

                // Get the list of staff filters
                StaffFilterList = hDAL.GetListOfStaffFilters(),

                // Get the list of staff filter links
                StaffFilterLinkList = hDAL.GetListOfStaffFilterLinks(),

                // Get the list of staff filter values
                StaffFilterValList = hDAL.GetListOfStaffFilterVals()
    };

            // Show the view
            return View(dto);
        }

        /// <summary>
        /// POST: Create a staff member
        /// </summary>
        /// <param name="StaffMember"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateStaffMember(StaffBO StaffMember)
        {

            // Add current date for when customer is created
            StaffMember.Staff_RegDateT = DateTime.Now;

            // Create a dto to hold the information we need to pass to the view
            CreateStaffDTO dto = new CreateStaffDTO
            {
                // Get the list of departments
                DepartmentList = hDAL.GetListOfDepartments(),

                // populate the staff member bo
                StaffMember = StaffMember,

                // Populater Staff Filter BO
                StaffFilterList = hDAL.GetListOfStaffFilters(),

                // Populate the staff filter link BO
                StaffFilterLinkList = hDAL.GetListOfStaffFilterLinks(),

                // Populate the staff filter val member
                StaffFilterValList = hDAL.GetListOfStaffFilterVals(),
            };

            // Check if the model is valid
            if (ModelState.IsValid)
            {
                // Set the status as active
                StaffMember.Status_ID = Status.Active;

                dto.StaffMember = StaffMember;
                                  

                // Submit the entry to the database
                hDAL.CreateStaffMember(StaffMember, dto.StaffFilterList);
            }
            else
            {
                // Return the failed model to the page
                return View(dto);
            }

            // Redirect to the Index page
            return RedirectToAction("Index");
        }

    }
}
