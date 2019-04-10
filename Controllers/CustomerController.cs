using Mvc4_Development_Test.Customer;
using Mvc4_Development_Test.Home;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace Mvc4_Development_Test.Controllers
{
    public class CustomerController : Controller
    {
        // Store the customer DAL
        CustomerDAL cDAL;
        HomeDAL hDAL;

        public CustomerController()
        {
            // Initialise the Customer DAL
            cDAL = new CustomerDAL();
            hDAL = new HomeDAL();
        }






        /// <summary>
        /// GET: INDEX
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerIndex(string SearchName, Status? SearchStatus, int? page = 1)
        {

            //IPagedList<CustomerBO> cust = null;
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            // Create a data transfer object to pass all data to the view
            CustomerDTO dto = new CustomerDTO
            {
                // Populate the customer service list
                CustomerServiceList = cDAL.GetListOfCustomerServices(),

                // Populate the stutus list
                StatusList = cDAL.GetListOfStatuses(),

                // Populate the list of users
                CustomerList = cDAL.GetListOfCustomer(SearchName, SearchStatus, null).Where(c => c.Status_ID != Status.Deleted).ToPagedList(pageIndex,pageSize),

                // Populate the service list
                ServiceList = cDAL.GetListOfServices(),

                // Populate title list
                TitleList = cDAL.GetListOfTitles(),

                // Populate staff list
                StaffList = hDAL.GetListOfStaff(),

            };

            return View(dto);
        }


        /// <summary>
        /// GET: Create Customer Member
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateCustomerMember()
        {
            CreateCustomerDTO dto = GetCreateCustomerDTO();

            // Show the view
            return View(dto);
        }

        /// <summary>
        /// Get create customer DTO
        /// </summary>
        /// <returns></returns>
        private CreateCustomerDTO GetCreateCustomerDTO()
        {
            // Create a dto to hold the information we need to pass to the view
            CreateCustomerDTO dto = new CreateCustomerDTO();

            //Populate titles
            dto.TitleList = PopulateTitleList();
            dto.CustomerMember.TitleList = PopulateTitleList();

            // Populate Staff List
            dto.StaffList = PopulateStaffList();
            dto.CustomerMember.StaffList = PopulateStaffList();

            // Populate Service List
            dto.CustomerMember.ServiceList = PopulateServiceList();

            // Populate Status List
            dto.StatusList = PopulateStatusList();

            // Populate Service List
            dto.ServiceList = PopulateServiceList();

            // Populate Customer Service List
            dto.CustomerServiceList = PopulateCustomerServiceList();

            return dto;
        }

        /// <summary>
        /// GET: Redirect to Edit Details page
        /// </summary>
        /// <param name="CustomerMember"></param>
        /// <returns></returns>        
        public ActionResult EditDetails(int id)
        {
            CustomerBO customer = cDAL.GetListOfCustomer().Where(i => i.Customer_ID.Equals(id)).ToList().FirstOrDefault();

            return View(customer);
        }


        /// <summary>
        /// POST: Edit Customer Index deatails
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomerIndex(List<CustomerBO> CustomerList, int page)
        {
            //bool exist = true;
            List<CustomerBO> actualList = cDAL.GetListOfCustomer().ToList();

            for (int i = 0; i < CustomerList.Count(); i++)
            {
                CustomerBO actual = actualList.Where(m => m.Customer_ID.Equals(CustomerList[i].Customer_ID)).ToList().FirstOrDefault();

                // Compares if any changes were made and edits database accordingly
                if (CustomerList[i].Title_ID != (actual.Title_ID)
                    || CustomerList[i].Customer_Name != (actual.Customer_Name)
                    || CustomerList[i].Customer_School != (actual.Customer_School)
                    || CustomerList[i].Customer_Email != (actual.Customer_Email))
                {
                    // Updates edit date to now
                    CustomerList[i].Customer_EditDateT = DateTime.Now;

                    // Updates customer details with edited information
                    cDAL.UpdateCustomerDetails(CustomerList[i]);
                }
            }

            return RedirectToAction("CustomerIndex", new { page  });
        }


        /// <summary>
        /// POST: Edit customer details
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditDetails(CustomerBO customer)
        {
            // Checks if customer already in database
            bool exist = DoesCustomerExist(customer, customer.Customer_ID);

            // Gets the details before changes
            CustomerBO actual = cDAL.GetListOfCustomer().Where(i => i.Customer_ID.Equals(customer.Customer_ID)).ToList().FirstOrDefault();

            // Compares if any changes were made and edits database accordingly
            if (!exist)
            {
                // Updates edit date to now
                customer.Customer_EditDateT = DateTime.Now;

                // Updates customer details with edited information
                cDAL.UpdateCustomerDetails(customer);                   
            }
            
            return RedirectToAction("CustomerIndex");
        }

        /// <summary>
        /// Edit the status of the customer
        /// </summary>
        /// <param name="CustomerMember"></param>
        /// <returns></returns>       
        public ActionResult EditStatus(CustomerBO CustomerMember)
        {
            // If customer status is active change to blocked
            if(CustomerMember.Status_ID == Status.Active)
            {
                cDAL.UpdateCustomerMember(CustomerMember.Customer_ID, Status.Blocked, false);
            }
            // If customer status is blocked change to active
            else if (CustomerMember.Status_ID == Status.Blocked)
            {
                cDAL.UpdateCustomerMember(CustomerMember.Customer_ID, Status.Active, false);
            }
           
            // Redirect to the Index page
            return RedirectToAction("CustomerIndex");
        }
   
        /// <summary>
        /// Change customer status to deleted
        /// </summary>
        /// <param name="CustomerMember"></param>
        /// <returns></returns>
        public ActionResult Delete(CustomerBO CustomerMember)
        {
            // Set customer status to deleted
            cDAL.UpdateCustomerMember(CustomerMember.Customer_ID, Status.Deleted, false);
            
            // Redirect to the Index page
            return RedirectToAction("CustomerIndex");
        }

        /// <summary>
        /// Create Customer Member
        /// </summary>
        /// <param name="CustomerMember"></param>
        /// <param name="ServiceList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateCustomerMember(CustomerBO CustomerMember, List<ServiceBO> ServiceList)
        {
            // Add current date for when customer is created
            CustomerMember.Customer_RegDateT = DateTime.Now;

            bool exist = DoesCustomerExist(CustomerMember);

            // Check if the model is valid
            if (ModelState.IsValid && !exist)
            {
                // Set the status as active
                CustomerMember.Status_ID = Status.Active;
                
                // Submit the entry to the database
                cDAL.CreateCustomerMember(CustomerMember, ServiceList);                
            }
            else if (!exist)
            {
                CreateCustomerDTO dto = this.GetCreateCustomerDTO();

                dto.CustomerMember = CustomerMember;

                return View(dto);
            }

            // Redirect to the Customer Index page
            return RedirectToAction("CustomerIndex");

        }

        /// <summary>
        /// Checks if customer is in the database
        /// </summary>
        /// <param name="CustomerMember"></param>
        /// <returns></returns>
        public bool DoesCustomerExist(CustomerBO CustomerMember, int? Customer_ID = null, int page = 1)
        {
            bool exist = false;

            // Gets list of customers
            IPagedList<CustomerBO> customerList = cDAL.GetListOfCustomer().ToPagedList(page,10);

            // Filters list of customers by id if specified
            if (Customer_ID.HasValue)
            {
                customerList = customerList.Where(c => c.Customer_ID == Customer_ID).ToPagedList(page,10);
            }
            foreach (var customer in customerList)
            {
                if ((CustomerMember.Title_ID.Equals((int)customer.Title_ID))
                    && (CustomerMember.Customer_Name.Equals(customer.Customer_Name))
                    && (CustomerMember.Customer_School.Equals(customer.Customer_School))
                    && CustomerMember.Customer_Email.Equals(customer.Customer_Email)
                    )
                {
                    exist = true;                   
                    break;
                }
                else
                {
                    exist = false;
                }
            }
            return exist;
        }

        /// <summary>
        /// Populate list of staff
        /// </summary>
        /// <returns>staffList</returns>
        public List<StaffBO> PopulateStaffList()
        {
            HomeDAL homeDAL = new HomeDAL();
            return homeDAL.GetListOfStaff();
        }

        /// <summary>
        /// Populate list of statuses
        /// </summary>
        /// <returns>statusList</returns>
        public List<StatusBO> PopulateStatusList()
        {
            CustomerDAL customerDAL = new CustomerDAL();
            return customerDAL.GetListOfStatuses();
        }

        /// <summary>
        /// Populate list of titles
        /// </summary>
        /// <returns>titleList</returns>
        public List<TitleBO> PopulateTitleList()
        {
            CustomerDAL customerDAL = new CustomerDAL();
            return customerDAL.GetListOfTitles();
        }

        /// <summary>
        /// Populate list of customer ids with service ids
        /// </summary>
        /// <returns>customerServiceList</returns>
        public List<CustomerServiceBO> PopulateCustomerServiceList()
        {
            CustomerDAL customerDAL = new CustomerDAL();
            return customerDAL.GetListOfCustomerServices();
        }

        /// <summary>
        /// Populate list of services
        /// </summary>
        /// <returns>serviceList</returns>
        public List<ServiceBO> PopulateServiceList()
        {
            CustomerDAL customerDAL = new CustomerDAL();
            return customerDAL.GetListOfServices();
        }
    }
}
