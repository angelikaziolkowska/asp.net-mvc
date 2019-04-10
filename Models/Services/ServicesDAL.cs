using Mvc4_Development_Test.Services;
using SQLHelper;
using SQLHelper.DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Mvc4_Development_Test.Services
{

    public class ServicesDAL
    {
        // Create an sqlCommands object from the helper
        SqlCommands sqlHelper;

        // Initialise a string to hold the connection string
        string connStr;

        public ServicesDAL()
        {

            // Initialise the SQL Helper
            sqlHelper = new SqlCommands();

            // Set the connection string to the default connection
            connStr = ConfigurationManager.ConnectionStrings["Default"].ToString();

        }

        /// <summary>
        /// Get a list of all Services
        /// </summary>
        /// <returns></returns>
        public List<ServicesBO> GetListOfServices()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
    Service_ID
,   Service_Name

FROM ServiceOfInterest ser


";

            #endregion


            // Populate and return a list of business objects
            List<ServicesBO> serviceList = sqlHelper.GetListOfObjects<ServicesBO>(connStr, SQLQuery);

            return serviceList;

        }
    }
}