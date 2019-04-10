using SQLHelper;
using SQLHelper.DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Mvc4_Development_Test.Event
{

    public class CustomerSalesDAL
    {
        // Create an sqlCommands object from the helper
        SqlCommands sqlHelper;

        // Initialise a string to hold the connection string
        string connStr;

        public CustomerSalesDAL()
        {

            // Initialise the SQL Helper
            sqlHelper = new SqlCommands();

            // Set the connection string to the default connection
            connStr = ConfigurationManager.ConnectionStrings["Default"].ToString();

        }

        /// <summary>
        /// Create a new event memeber
        /// </summary>
        /// <param name="eventDetails">The details of the staff memeber to create</param>
        public void CreateEventMember(EventBO eventDetails)
        {
            

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery5 = @"

    INSERT INTO dbo._Event
    (
       Event_Name
    ,   Region_ID
    ,   Event_DateT
    )
    VALUES
    (
        @Event_Name
    ,   @Region_ID
    ,   @Event_DateT
    );
 
";

            #endregion

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            // Example Paramater
            paramList.AddSQLParameter("@Event_Name", SqlDbType.VarChar, eventDetails.Event_Name.Trim());
            paramList.AddSQLParameter("@Region_ID", SqlDbType.Int, eventDetails.Region_ID);
            paramList.AddSQLParameter("@Event_DateT", SqlDbType.DateTime, eventDetails.Event_DateT);
                   
            // Execute the query and return number of rows affected
            int rowsAffected = sqlHelper.ExecuteSqlNonQuery(connStr, SQLQuery5, paramList);
        }


        public List<MonthBO> GetListOfMonths()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"

    SELECT 
        Month_ID
    ,	Month_Name

    FROM dbo.Months
 
";

            #endregion

            // Populate and return a list of business objects
            List<MonthBO> listOfMonths = sqlHelper.GetListOfObjects<MonthBO>(connStr, SQLQuery);
            return listOfMonths;
        }

        public List<CustomerSalesBO> GetListOfCustomerSales()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"

SELECT 
	    m.Month_ID As Month_ID
	,	m.Month_Name As Month_Name
	,	COUNT(DISTINCT e.Event_ID) AS Event_Count
	,	COUNT(c.Customer_ID) AS Customer_Count

FROM dbo.Months m

LEFT JOIN _Event e
ON m.Month_ID = MONTH(e.Event_DateT)

LEFT JOIN Customer c
ON e.Event_ID = c.Event_ID

GROUP BY Month_ID, Month_Name
ORDER BY Month_ID
 
";

            #endregion

            // Populate and return a list of business objects
            List<CustomerSalesBO> listOfCustomerSales = sqlHelper.GetListOfObjects<CustomerSalesBO>(connStr, SQLQuery);
            return listOfCustomerSales;
        }
    }
}