using SQLHelper;
using SQLHelper.DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using PagedList;
using Mvc4_Development_Test.UploadDetails;

namespace Mvc4_Development_Test.Customer
{

    public class CustomerDAL
    {
        // Create an sqlCommands object from the helper
        SqlCommands sqlHelper;

        // Initialise a string to hold the connection string
        string connStr;

        public CustomerDAL()
        {
            // Initialise the SQL Helper
            sqlHelper = new SqlCommands();

            // Set the connection string to the default connection
            connStr = ConfigurationManager.ConnectionStrings["Default"].ToString();
        }


        /// <summary>
        /// Get a list of all Statuses
        /// </summary>
        /// <returns></returns>
        public List<TitleBO> GetListOfTitles()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
    Title_ID
,   Title_Desc

FROM Title ti


";

            #endregion


            // Populate and return a list of business objects
            List<TitleBO> titleList = sqlHelper.GetListOfObjects<TitleBO>(connStr, SQLQuery);

            return titleList;

        }

        /// <summary>
        /// Get a list of all Statuses
        /// </summary>
        /// <returns></returns>
        public List<StatusBO> GetListOfStatuses()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
    Status_ID
,   Status_Name

FROM Stat st


";

            #endregion


            // Populate and return a list of business objects
            List<StatusBO> statusList = sqlHelper.GetListOfObjects<StatusBO>(connStr, SQLQuery);

            return statusList;

        }

        /// <summary>
        /// Get a list of all Services
        /// </summary>
        /// <returns></returns>
        public List<CustomerServiceBO> GetListOfCustomerServices()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
    cs.Service_ID
,   cs.Customer_ID
,	s._Service_Name

FROM CustomerServices cs
INNER JOIN _Service s
ON s.Service_ID = cs.Service_ID;

";

            #endregion


            // Populate and return a list of business objects
            List<CustomerServiceBO> customerServiceList = sqlHelper.GetListOfObjects<CustomerServiceBO>(connStr, SQLQuery);

            return customerServiceList;

        }

        /// <summary>
        /// Get a list of all Services
        /// </summary>
        /// <returns></returns>
        public List<ServiceBO> GetListOfServices()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
    Service_ID
,   _Service_Name AS 'Service_Name'

FROM _Service ser



";

            #endregion

            // Populate and return a list of business objects
            List<ServiceBO> serviceList = sqlHelper.GetListOfObjects<ServiceBO>(connStr, SQLQuery);

            return serviceList;

        }

        public List<CustomerBO> GetListOfCustomer()
        {
            return GetListOfCustomer(null, null, null);
        }

        /// <summary>
        /// Get a list of all users
        /// </summary>
        /// <returns></returns>
        public List<CustomerBO> GetListOfCustomer(string SearchName, Status? SearchStatus, int? event_ID)
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
    Customer_ID
,   t.Title_ID
,   t.Title_Desc
,   Customer_Name
,   Customer_School
,   Customer_Email
,   Customer_RegDateT
,   st.Status_ID
,   st.Status_Name
,   Staff_ID
,   Customer_EditDateT
FROM Customer cust

LEFT JOIN Stat st
ON cust.Status_ID = st.Status_ID

LEFT JOIN Title t
ON cust.Title_ID = t.Title_ID

{customer_filter}

";

            #endregion

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            string customer_filter = String.Empty;

            // If either string or both are specified            
            if (!String.IsNullOrEmpty(SearchName))
            {
                customer_filter += "WHERE(cust.Customer_Name LIKE '%' + @SearchName + '%')";
                paramList.AddSQLParameter("@SearchName", SqlDbType.VarChar, SearchName);
            }
            if (SearchStatus.HasValue)
            {
                paramList.AddSQLParameter("@SearchStatus", SqlDbType.Int, SearchStatus);

                if (!String.IsNullOrEmpty(SearchName))
                {
                    customer_filter += " AND(cust.Status_ID = @SearchStatus)";
                }
                else
                {
                    customer_filter += "WHERE(cust.Status_ID = @SearchStatus)";
                }               
            }
            // 
            if (event_ID.HasValue)
            {
                paramList.AddSQLParameter("@Event_ID", SqlDbType.Int, event_ID);
                customer_filter += "WHERE(cust.Event_ID = @Event_ID)";

            }

            // Replace in query
            SQLQuery = SQLQuery.Replace("{customer_filter}", customer_filter);
                                 

            // Populate and return a list of business objects
            List<CustomerBO> customerList = sqlHelper. GetListOfObjects<CustomerBO>(connStr, SQLQuery, paramList).ToList();

            return customerList;

        }






        /// <summary>
        /// Create a new customer member
        /// </summary>
        /// <param name="userDetails">The details of the customer member to create</param>
        public bool CreateCustomerMember(CustomerBO userDetails, List<ServiceBO> serviceList = null)
        {
            

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SET XACT_ABORT ON;

BEGIN TRANSACTION
    INSERT INTO dbo.Customer
    (
        Title_ID
    ,   Customer_Name
    ,   Customer_School
    ,   Customer_Email
    ,   Customer_RegDateT
    ,   Status_ID
    ,   Staff_ID
    ,   Event_ID
    )
    VALUES
    (
        {title}
    ,   @Customer_Name
    ,   @Customer_School
    ,   @Customer_Email
    ,   @Customer_RegDateT
    ,   @Status_ID
    ,   {staff}
    ,   @Event_ID
    );    

    INSERT INTO dbo.CustomerServices
    (
        Customer_ID
    ,   Service_ID
    )
    VALUES
    {servi}

COMMIT TRANSACTION;

SET XACT_ABORT OFF
";

            #endregion

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();
            

            string servi = String.Empty;
            string servi2 = String.Empty;
            string staff = String.Empty;

                paramList.AddSQLParameter("@Title_ID", SqlDbType.Int, (int)userDetails.Title_ID);
                paramList.AddSQLParameter("@Customer_Name", SqlDbType.VarChar, userDetails.Customer_Name.Trim());
                paramList.AddSQLParameter("@Customer_School", SqlDbType.VarChar, userDetails.Customer_School.Trim());
                paramList.AddSQLParameter("@Customer_Email", SqlDbType.VarChar, userDetails.Customer_Email.Trim());
                paramList.AddSQLParameter("@Customer_RegDateT", SqlDbType.DateTime, userDetails.Customer_RegDateT);
                paramList.AddSQLParameter("@Status_ID", SqlDbType.Int, (int)userDetails.Status_ID);
                paramList.AddSQLParameter("@Staff_ID", SqlDbType.Int, userDetails.Staff_ID);
                paramList.AddSQLParameter("@Event_ID", SqlDbType.Int, null);

                for (int i = 0; i < serviceList.Count(); i++)
                {
                    if (serviceList[i].IsChecked)
                    {
                        paramList.AddSQLParameter("@Service_ID" + i, SqlDbType.Int, serviceList[i].Service_ID);
                        servi += "(SCOPE_IDENTITY(), @Service_ID" + i + "),";
                    }
                }
                servi = servi.TrimEnd(',');
                SQLQuery = SQLQuery.Replace("{servi}", servi);
                SQLQuery = SQLQuery.Replace("{title}", "@Title_ID");
                SQLQuery = SQLQuery.Replace("{staff}", "@Staff_ID");

                // Execute the query and return number of rows affected
                int rowsAffected = sqlHelper.ExecuteSqlNonQuery(connStr, SQLQuery, paramList);
                       
            return true;
        }

        /// <summary>
        /// Create a new customer member
        /// </summary>
        /// <param name="userDetails">The details of the customer member to create</param>
        public void UpdateCustomerMember(int id, Status s, bool tempTable = false)
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"

UPDATE dbo.{temp}Customer
SET     Status_ID = @Status_ID, 
        Customer_EditDateT = @Customer_EditDateT
WHERE Customer_ID = @Customer_ID;

";

            #endregion
           
            string temp = String.Empty;

            if (tempTable)
            {
                temp += "Temp";
            }
            SQLQuery = SQLQuery.Replace("{temp}", temp);

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            // Example Paramater
            paramList.AddSQLParameter("@Customer_ID", SqlDbType.Int, id);
            paramList.AddSQLParameter("@Status_ID", SqlDbType.Int, (int)s);
            paramList.AddSQLParameter("@Customer_EditDateT", SqlDbType.DateTime, DateTime.Now);



            // Execute the query and return number of rows affected
            int rowsAffected = sqlHelper.ExecuteSqlNonQuery(connStr, SQLQuery, paramList);

        }

        /// <summary>
        /// Create a new customer member
        /// </summary>
        /// <param name="userDetails">The details of the customer member to create</param>
        public void UpdateCustomerDetails(CustomerBO customer)
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"

UPDATE dbo.Customer
SET     Customer_EditDateT = @Customer_EditDateT
    ,   Title_ID = @Title_ID 
        {cust_name_school_title}

WHERE Customer_ID = @Customer_ID;

";

            #endregion

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            // Example Paramater
            paramList.AddSQLParameter("@Customer_ID", SqlDbType.Int, customer.Customer_ID);            
            paramList.AddSQLParameter("@Customer_EditDateT", SqlDbType.DateTime, customer.Customer_EditDateT);
            paramList.AddSQLParameter("@Title_ID", SqlDbType.Int, (int)customer.Title_ID);

            // Update database columns where there was input
            string cust_name_school_title = String.Empty;               

            if (customer.Customer_Name != null)
            {
                cust_name_school_title += ",   Customer_Name = @Customer_Name";
                paramList.AddSQLParameter("@Customer_Name", SqlDbType.VarChar, customer.Customer_Name.Trim());
            }
            if (customer.Customer_School != null)
            {
                cust_name_school_title += ",   Customer_School = @Customer_School";
                paramList.AddSQLParameter("@Customer_School", SqlDbType.VarChar, customer.Customer_School.Trim());
            }
            SQLQuery = SQLQuery.Replace("{cust_name_school_title}", cust_name_school_title);

            // Execute the query and return number of rows affected
            int rowsAffected = sqlHelper.ExecuteSqlNonQuery(connStr, SQLQuery, paramList);

        }
    }
}