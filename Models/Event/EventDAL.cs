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

    public class EventDAL
    {
        // Create an sqlCommands object from the helper
        SqlCommands sqlHelper;

        // Initialise a string to hold the connection string
        string connStr;

        public EventDAL()
        {

            // Initialise the SQL Helper
            sqlHelper = new SqlCommands();

            // Set the connection string to the default connection
            connStr = ConfigurationManager.ConnectionStrings["Default"].ToString();

        }

        /// <summary>
        /// Get a list of all users
        /// </summary>
        /// <returns></returns>
        public List<EventBO> GetListOfEvents()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
    Event_ID
,   Event_Name
,   r.Region_ID
,   r.Region_Name
,   Event_DateT

FROM _Event e

LEFT JOIN Region r
ON e.Region_ID = r.Region_ID

";

            #endregion
                    
            // Populate and return a list of business objects
            List<EventBO> eventList = sqlHelper.GetListOfObjects<EventBO>(connStr, SQLQuery);

            return eventList;

        }


        /// <summary>
        /// Get a list of all departments
        /// </summary>
        /// <returns></returns>
        public List<RegionBO> GetListOfRegions()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
	Region_ID
,	Region_Name

FROM dbo.Region
";

            #endregion


            // Populate and return a list of business objects
            List<RegionBO> regionList = sqlHelper.GetListOfObjects<RegionBO>(connStr, SQLQuery);

            return regionList;

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

        /// <summary>
        /// Getting months table
        /// </summary>
        /// <returns></returns>
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
    }
}