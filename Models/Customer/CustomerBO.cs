using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mvc4_Development_Test.Home;
using static Mvc4_Development_Test.Home.StaffBO;

namespace Mvc4_Development_Test.Customer
{


    /// <summary>
    /// Hold the details for a Customer Member
    /// </summary>
    public class CustomerBO
    {

        /// <summary>
        /// The Statuses
        /// </summary>
        [Display(Name = "Status")]
        public string Status_Name { get; set; }

        [Display(Name = "Status")]
        public Status Status_ID { get; set; }

        [Display(Name = "Services")]
        public List<ServiceBO> ServiceList { get; set; }

        [Display(Name = "Titles")]
        public List<TitleBO> TitleList { get; set; }

        [Display(Name = "Staff")]
        public List<StaffBO> StaffList { get; set; }

        [Display(Name = "Services")]
        public List<CustomerServiceBO> CustomerServiceList { get; set; }


        /// <summary>
        /// The ID of the Customer Member
        /// </summary>
        [Display(Name = "ID")]
        public int Customer_ID { get; set; }

        /// <summary>
        /// The Title Name of the Customer
        /// </summary>
        [Display(Name = "Title")]
        public string Title_Desc { get; set; }

        /// <summary>
        /// The Title ID of the Customer
        /// </summary>
        [Display(Name = "Title")]
        [Required]
        public Titles Title_ID { get; set; }

        /// <summary>
        /// The Name of the Customer
        /// </summary>
        [Required(ErrorMessage = "Please enter customer name.")]
        [StringLength(40, MinimumLength = 2)]
        [Display(Name="Name")]
        public string Customer_Name { get; set; }


        /// <summary>
        /// Get the Registration Date of Customer
        /// </summary>
        [Display(Name = "Registration Date")]
        public DateTime Customer_RegDateT { get; set; }

        /// <summary>
        /// Get the Registration Date of Customer
        /// </summary>
        [Display(Name = "Edited")]
        public DateTime? Customer_EditDateT { get; set; }

        /// <summary>
        /// The Name of the Customer's School
        /// </summary>
        [Required(ErrorMessage = "Please enter name of school.")]
        [StringLength(80, MinimumLength = 2)]
        [Display(Name = "School")]
        public string Customer_School { get; set; }

        /// <summary>
        /// The Name of the Customer's Email
        /// </summary>
        [Required(ErrorMessage = "Please enter customer email.")]
        [StringLength(80, MinimumLength = 4)]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Customer_Email { get; set; }

        /// <summary>
        /// The ID of the staff member
        /// </summary>
        [Display(Name = "Staff")]
        [Required]
        public int Staff_ID { get; set; }

        /// <summary>
        /// The Title Name of the Customer
        /// </summary>
        [Display(Name = "Staff")]
        public string Staff_Name { get; set; }




        /// <summary>
        /// Event linked with
        /// </summary>
        [Display(Name = "Event")]
        public int Event_ID { get; set; }

        /// <summary>
        /// Event linked with
        /// </summary>
        [Display(Name = "Event")]
        public int Event_Name { get; set; }




    }   
}