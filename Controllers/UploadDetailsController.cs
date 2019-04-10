using Mvc4_Development_Test.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc4_Development_Test.Event;
using Mvc4_Development_Test.Customer;
using Mvc4_Development_Test.UploadDetails;
using LinqToExcel;
using System.IO;
using System.Data;
using PagedList;

namespace Mvc4_Development_Test.Controllers
{
    public class UploadDetailsController : Controller
    {
        // Store the home DAL
        EventDAL eDAL;
        UploadDetailsDAL uDAL;
        CustomerDAL cDAL;
        HomeDAL hDAL;

        public UploadDetailsController()
        {
            // Initialise the Home DAL
            eDAL = new EventDAL();
            uDAL = new UploadDetailsDAL();
            cDAL = new CustomerDAL();
            hDAL = new HomeDAL();
        }

        public ActionResult UploadSuccessful()
        {
            return View();
        }

        public ActionResult UploadFailed()
        {
            return View();
        }


        /// <summary>
        /// GET: Create a staff member
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadDetailsMember()
        {

            bool pending = uDAL.IsFilePending();
            if (pending)
            {
                return RedirectToAction("StaffConfirmation");
            }

            // Create a dto to hold the information we need to pass to the view
            UploadDetailsDTO dto = new UploadDetailsDTO
            {

                // Get the list of events
                EventList = eDAL.GetListOfEvents()               

            };

            // Show the view
            return View(dto);
        }


        /// <summary>
        /// POST: Create a staff member
        /// </summary>
        /// <param name="EventMember"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadDetailsMember(UploadDetailsBO UploadDetailsMember)
        {
            const int title_len = 50;
            const int name_len = 40;
            const int school_len = 80;
            const int email_len = 80;
            const int date_len = 10;
            const int staff_ID_len = 2;
            const int serv_len = 40;
            int lineNum = 0;
            int prevOnlyServ = 0;
            string line = String.Empty;
            UploadDetailsMember.ParsedData = new List<List<string>>();
            List<string> row;
            bool emptyFile = true;


            // Check if file in correct .CSV format
            if (!String.Equals(Path.GetExtension(UploadDetailsMember.File.FileName), ".csv", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("UploadDetailsMember.File", "File must have .CSV extension");
            }

            if (ModelState.IsValid)
            {
                using (StreamReader readFile = new StreamReader(UploadDetailsMember.File.InputStream))
                {
                    // Save all lines into the list
                    while ((line = readFile.ReadLine()) != null)
                    {

                        // Skip header row
                        if (lineNum == 0)
                        {
                            lineNum++;
                            continue;
                        }
                        emptyFile = false;
                        // Split the row into array 
                        row = line.Split(',').ToList();

                        if (row.Count() > 7) // removing further unneccessary columns
                        {
                            row.RemoveRange(7, row.Count() - 7);
                        }

                        if (lineNum < 3 && row.Count() == 7) // Validation of first 2 data rows in the spredsheet
                        {
                            // Check if correct amount of columns in data row
                            if (row != null || (row == null && prevOnlyServ < 3))
                            {
                                // Validation of each column 
                                if (row != null)

                                    if ((lineNum == 1 && // if line 1 is incorrect for any column
                                            ValidateFirstLine(title_len, name_len, school_len, email_len, date_len, staff_ID_len, serv_len, row))
                                    || (ValidateSecondLine(title_len, name_len, school_len, email_len, date_len, staff_ID_len, serv_len, lineNum, row))
                                    || ValidateSecondLineServicesOnly(serv_len, lineNum, row))
                                    {
                                        ModelState.AddModelError("UploadDetailsMember.File", "Invalid or empty values in table. Can't save to database.");
                                    }

                                // 'Previosly only service' count is set to 0
                                prevOnlyServ = 0;

                                // Update count of customers
                                UploadDetailsMember.Customer_Count++;
                            }
                            else // if row contains 1 column
                            {
                                // Previous row contained only service so count increases
                                prevOnlyServ++;
                            }

                            // Adds row into the list of rows if Successful
                            UploadDetailsMember.ParsedData.Add(row);
                        }
                        else // No need for further validation after first 2 data rows, we predict the correctness
                        {
                            if (!String.IsNullOrWhiteSpace(line)) // Only update counter if row is not only service of above customer
                            {
                                UploadDetailsMember.Customer_Count++;
                            }

                            // parsing the following rows into the list
                            UploadDetailsMember.ParsedData.Add(row);

                        }
                        lineNum++;
                    }
                    if (emptyFile == true)
                    {
                        ModelState.AddModelError("UploadDetailsMember.File", "Table cannot be empty.");
                    }
                }

                // Details of the file upload
                UploadDetailsMember.File_DateT = DateTime.Now;
                //UploadDetailsMember.Temp_Table_ID = Guid.NewGuid().ToString().TrimEnd();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Call the DAL to save details into the database
                    uDAL.UploadDetailsMember(UploadDetailsMember);
                    //return RedirectToAction("UploadSuccessful");
                    return RedirectToAction("StaffConfirmation");
                }
                catch
                {
                    ModelState.AddModelError("UploadDetailsMember.File", "Uploading to database failed, table is not in correct format.");
                }
            }

            // Create a dto to hold the information we need to pass to the view
            UploadDetailsDTO fdto = new UploadDetailsDTO
            {
                // Get the list of events
                EventList = eDAL.GetListOfEvents(),
            };

            return View(fdto);
        }

        /// <summary>
        /// GET: Staff Confirmation Page
        /// </summary>
        /// <returns></returns>
        public ActionResult StaffConfirmation(string temp_table_ID = null, int? page = 1)
        {
            //IPagedList<CustomerBO> cust = null;
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page ?? 1;
            if(temp_table_ID == null)
            {
                temp_table_ID = uDAL.GetGUID();
            }
            

            // Create a data transfer object to pass all data to the view
            CustomerDTO dto = new CustomerDTO
            {

                // Populate the customer service list
                CustomerServiceList = uDAL.GetListOfTempCustomerServices(),

                // Populate the list of users
                CustomerList = uDAL.GetListOfTempCustomer().Where(i => i.Status_ID != Status.Deleted).ToPagedList(pageIndex, pageSize),

                // Populate the service list
                ServiceList = cDAL.GetListOfServices(),
                
                // Populate staff list
                StaffList = hDAL.GetListOfStaff(),                

            };
                                 
            return View(dto);
        }

        /// <summary>
        /// POST: Edit Customer Index deatails
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StaffConfirmation(List<CustomerBO> CustomerList, Guid? temp_table_ID, int? page = 1)
        {

            //bool exist = true;
            var actualList = uDAL.GetListOfTempCustomer().ToList();

            // Changing values in temp database if staff ID did not exist in Staff table
            for (int i = 0; i < CustomerList.Count(); i++)
            {
                CustomerBO actual = actualList.Where(m => m.Customer_ID.Equals(CustomerList[i].Customer_ID)).ToList().FirstOrDefault();

                // Compares if any changes were made and edits database accordingly
                if (CustomerList[i].Staff_ID != (actual.Staff_ID))
                {
                    // Updates edit date to now
                    CustomerList[i].Customer_EditDateT = DateTime.Now;

                    // Updates customer details with edited information
                    uDAL.UpdateCustomerDetails(CustomerList[i]);
                }
            }

            bool success = false;

            // Insert temp tables into real ones
            success = uDAL.AddIntoRealTables();
            
            if(success)
            {
                return RedirectToAction("UploadSuccessful");                
            }
           
            // Redirect to Upload Failed
            return RedirectToAction("UploadFailed");

        }

        private static bool ValidateSecondLineServicesOnly(int serv_len, int lineNum, List<string> row)
        {
            return (lineNum == 2 && // not all values are empty and service has a value
                                                        row[0].Length == 0
                                                        && !(row[1] == ("")
                                                        && row[2] == ("")
                                                        && row[3] == ("")
                                                        && row[4] == ("")
                                                        && row[5] == ("")
                                                        && row[6] != ("") && row[6].Length <= serv_len)
                                                        );
        }

        private static bool ValidateSecondLine(int title_len, int name_len, int school_len, int email_len, int date_len, int staff_ID_len, int serv_len, int lineNum, List<string> row)
        {
            return lineNum == 2 // if any of the values is incorrect on line 2
                                                        && row[0].Length > 0
                                                        && row[1].Length > 0
                                                        && row[2].Length > 0
                                                        && row[3].Length > 0
                                                        && row[4].Length > 0
                                                        && row[5].Length > 0
                                                        && row[6].Length > 0
                                                        &&
                                                        (row[0].Length > title_len
                                                        || row[1].Length > name_len
                                                        || row[2].Length > school_len
                                                        || row[3].Length > email_len || !row[3].Contains("@")
                                                        || !row[4].Contains('/')
                                                        || row[4].Length > date_len
                                                        || row[5].Length > staff_ID_len
                                                        || row[6].Length > serv_len
                                                        );
        }

        private static bool ValidateFirstLine(int title_len, int name_len, int school_len, int email_len, int date_len, int staff_ID_len, int serv_len, List<string> row)
        {
            return (row[0].Length > title_len
                                                        || row[0].Length <= 1
                                                        || row[1].Length > name_len
                                                        || row[1].Length <= 2
                                                        || row[2].Length > school_len
                                                        || row[2].Length <= 2
                                                        || row[3].Length > email_len
                                                        || row[3].Length <= 2
                                                        || !row[3].Contains("@")
                                                        || !row[4].Contains('/')
                                                        || row[4].Length > date_len
                                                        || row[4].Length != 10
                                                        || row[5].Length > staff_ID_len
                                                        || row[5].Length < 1
                                                        || row[6].Length > serv_len
                                                        || row[6].Length <= 6
                                                        );
        }

        /// <summary>
        /// Change customer status to deleted
        /// </summary>
        /// <param name="CustomerMember"></param>
        /// <returns></returns>
        public ActionResult Delete(CustomerBO CustomerMember)
        {
            // Set customer status to deleted
            cDAL.UpdateCustomerMember(CustomerMember.Customer_ID, Status.Deleted, true);

            // Redirect to the Index page
            return RedirectToAction("StaffConfirmation");
        }


        /// <summary>
        /// Change customer status to deleted
        /// </summary>
        /// <param name="CustomerMember"></param>
        /// <returns></returns>
        public ActionResult DeleteFile(Guid temp_table_ID)
        {
            uDAL.DeleteFile();

            // Redirect to the 'Success' page- reused this page and instead of Upload Successful it displays 'Success'
            return RedirectToAction("UploadSuccessful");
        }

    }
}
