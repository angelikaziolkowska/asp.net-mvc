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

    public class MonthlySalesDAL
    {
        // Create an sqlCommands object from the helper
        SqlCommands sqlHelper;

        // Initialise a string to hold the connection string
        string connStr;

        public MonthlySalesDAL()
        {

            // Initialise the SQL Helper
            sqlHelper = new SqlCommands();

            // Set the connection string to the default connection
            connStr = ConfigurationManager.ConnectionStrings["Default"].ToString();

        }        

        public List<MonthlySalesBO> GetListOfMonthlySales(int month)
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"

 SELECT 
        e.Event_ID As Event_ID
	,	e.Event_Name As Event_Name
	,	COUNT(c.Customer_ID) AS Customer_Count

FROM dbo._Event e

LEFT JOIN Customer c
ON e.Event_ID = c.Event_ID

WHERE MONTH(Event_DateT) = @Month_ID

GROUP BY e.Event_ID, Event_Name
ORDER BY e.Event_ID
 
";

            #endregion

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            // Insert month into parameter list
            paramList.AddSQLParameter("@Month_ID", SqlDbType.Int, month);


            // Populate and return a list of business objects
            List<MonthlySalesBO> listOfMonthlySales = sqlHelper.GetListOfObjects<MonthlySalesBO>(connStr, SQLQuery, paramList);
            return listOfMonthlySales;
        }
    }
}