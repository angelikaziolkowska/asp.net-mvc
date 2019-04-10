using Mvc4_Development_Test.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc4_Development_Test.Event;
using Mvc4_Development_Test.Customer;
using LinqToExcel;
using System.IO;

namespace Mvc4_Development_Test.Controllers
{
    public class EventController : Controller
    {
        // Store the home DAL
        EventDAL eDAL;
        CustomerDAL cDAL;
        CustomerSalesDAL csDAL;
        MonthlySalesDAL msDAL;

        public EventController()
        {
            // Initialise the Home DAL
            eDAL = new EventDAL();
            cDAL = new CustomerDAL();
            csDAL = new CustomerSalesDAL();
            msDAL = new MonthlySalesDAL();
        }


        /// <summary>
        /// Index Page
        /// </summary>
        /// <returns></returns>
        public ActionResult EventIndex(string sortOrder, string currentSort)
        {

            // Create a data transfer object to pass all data to the view
            EventDTO dto = new EventDTO
            {

                // Populate the list of events
                EventList = eDAL.GetListOfEvents(),

                // Populate the list of regions
                RegionList = eDAL.GetListOfRegions()

            };


            ViewBag.CurrentSort = sortOrder;

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "Event_ID" : sortOrder;
           

            switch (sortOrder)
            {
                case "Event_ID":
                    if (sortOrder.Equals(currentSort))
                        dto.EventList = dto.EventList.OrderByDescending
                                (m => m.Event_ID).ToList();
                    else
                        dto.EventList = dto.EventList.OrderBy
                                (m => m.Event_ID).ToList();
                    break;
                case "Event_Name":
                    if (sortOrder.Equals(currentSort))
                        dto.EventList = dto.EventList.OrderByDescending
                                (m => m.Event_Name).ToList();
                    else
                        dto.EventList = dto.EventList.OrderBy
                                (m => m.Event_Name).ToList();
                    break;

                case "Region_ID":
                    if (sortOrder.Equals(currentSort))
                        dto.EventList = dto.EventList.OrderByDescending
                                (m => m.Region_ID).ToList();
                    else
                        dto.EventList = dto.EventList.OrderBy
                                (m => m.Region_ID).ToList();
                    break;

                case "Event_DateT":
                    if (sortOrder.Equals(currentSort))
                        dto.EventList = dto.EventList.OrderByDescending
                                (m => m.Event_DateT).ToList();
                    else
                        dto.EventList = dto.EventList.OrderBy
                                (m => m.Event_DateT).ToList();
                    break;

                default:
                    goto case "Event_ID";
            }


            // Pass the data to the view
            return View(dto);
        }

        /// <summary>
        /// CUstomer Sales Report Index Page
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerSalesIndex()
        {

            // Create a data transfer object to pass all data to the view
            CustomerSalesDTO dto = new CustomerSalesDTO
            {
                // Populate the list of Customer Sales BOs
                CustomerSalesList = csDAL.GetListOfCustomerSales()   
            };

            // Pass the data to the view
            return View(dto);
        }

        /// <summary>
        /// Monthly Sales Report Index Page
        /// </summary>
        /// <returns></returns>
        public ActionResult MonthlySalesIndex(int month, int? event_ID)
        {

            // Create a data transfer object to pass all data to the view
            MonthlySalesDTO dto = new MonthlySalesDTO
            {
                // Populate the list of Months
                MonthList = eDAL.GetListOfMonths(),

                // Populate the list of Customer Sales BOs
                MonthlySalesList = msDAL.GetListOfMonthlySales(month),

                // Pass in the chosen month
                Month_ID = month,

                Event_ID = event_ID,

                CustomerList = cDAL.GetListOfCustomer(null, null, event_ID)
            };

            //if (event_ID != null)
            //{
            //    // Populate the list of Customers
                
            //}

            // Pass the data to the view
            return View(dto);
        }

        /// <summary>
        /// GET: Create an event member
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateEventMember()
        {
            // Create a dto to hold the information we need to pass to the view
            CreateEventDTO dto = new CreateEventDTO
            {

                // Get the list of events
                EventList = eDAL.GetListOfEvents(),

                // Get the list of regions
                RegionList = eDAL.GetListOfRegions(),

            };

            // Show the view
            return View(dto);
        }

        /// <summary>
        /// POST: Create an event member
        /// </summary>
        /// <param name="EventMember"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateEventMember(EventBO EventMember)
        {
            try
            {
               EventMember.Event_DateT = new DateTime(EventMember.Year, EventMember.Month, EventMember.Day);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("EventMember.Event_DateT", e.Message);
            }

            // Check if the model is valid           
            if (ModelState.IsValid)
            {
                // Submit the entry to the database
                eDAL.CreateEventMember(EventMember);
            }
            else
            {
                // Create a dto to hold the information we need to pass to the view
                CreateEventDTO fdto = new CreateEventDTO
                {

                    // Get the list of events
                    EventList = eDAL.GetListOfEvents(),

                    // Get the list of regions
                    RegionList = eDAL.GetListOfRegions(),

                    // Get the event
                    EventMember = EventMember,
                };

                // Return the failed model to the page
                return View(fdto);
            }

            // Redirect to the Index page
            return RedirectToAction("EventIndex");
        }

    }
}
