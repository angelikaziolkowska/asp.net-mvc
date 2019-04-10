using SQLHelper;
using SQLHelper.DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Mvc4_Development_Test.Home
{

    public class HomeDAL
    {
        // Create an sqlCommands object from the helper
        SqlCommands sqlHelper;

        // Initialise a string to hold the connection string
        string connStr;

        public HomeDAL()
        {

            // Initialise the SQL Helper
            sqlHelper = new SqlCommands();

            // Set the connection string to the default connection
            connStr = ConfigurationManager.ConnectionStrings["Default"].ToString();

        }

        public List<StaffBO> GetListOfStaff()
        {
            return GetListOfStaff(null);
        }

        /// <summary>
        /// Get a list of all users
        /// </summary>
        /// <returns></returns>
        public List<StaffBO> GetListOfStaff(int? id)
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
    Staff_ID
,   Staff_Name
,   Staff_RegDateT
,   d.Department_ID
,   d.Department_Name
,   s.Status_ID

FROM Staff s

LEFT JOIN Department d
ON s.Department_ID = d.Department_ID

{staff_filter}
";

            #endregion

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            string staff_filter = String.Empty;

            // If either string or both are specified            
            if (id.HasValue)
            {
                staff_filter += "WHERE Staff_ID = @Staff_ID";
                paramList.AddSQLParameter("@Staff_ID", SqlDbType.VarChar, id);
            }            

            // Replace in query
            SQLQuery = SQLQuery.Replace("{staff_filter}", staff_filter);

           
            // Populate and return a list of business objects
            List<StaffBO> staffList = sqlHelper.GetListOfObjects<StaffBO>(connStr, SQLQuery, paramList);

            return staffList;

        }

        /// <summary>
        /// Get a list of all departments
        /// </summary>
        /// <returns></returns>
        public List<DepartmentBO> GetListOfDepartments()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
	Department_ID
,	Department_Name
,	Status_ID
FROM dbo.Department

WHERE Status_ID = @Status_ID
";

            #endregion

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            // Example Paramater
            paramList.AddSQLParameter("@Status_ID", SqlDbType.Int, Status.Active);

            // Populate and return a list of business objects
            List<DepartmentBO> departmentList = sqlHelper.GetListOfObjects<DepartmentBO>(connStr, SQLQuery, paramList);

            // Add a blank entry to the list
            departmentList.Insert(0, new DepartmentBO() { Department_ID = null, Department_Name = "Please Select", Status_ID = Status.Active });

            return departmentList;

        }


        /// <summary>
        /// Get a list of all staff filters
        /// </summary>
        /// <returns></returns>
        public List<StaffFilterDetailsBO> GetListOfStaffFilterDetails(int Staff_ID)
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"

SELECT      
           f.StaffFilter_Name 
,          v.StaffFilterVal_Name

FROM Staff s  
  
LEFT JOIN StaffFilterLink l
ON s.Staff_ID = l.Staff_ID

LEFT JOIN StaffFilter f
ON l.StaffFilter_ID = f.StaffFilter_ID
  
RIGHT JOIN StaffFilterVal v
ON v.StaffFilter_ID = l.StaffFilter_ID

WHERE 
        l.StaffFilterVal_ID = v.StaffFilterVal_ID
        
        AND s.Staff_ID = @Staff_ID

GROUP BY 
            f.StaffFilter_Name
,           v.StaffFilterVal_Name
";

            #endregion

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            // Example Paramater

            paramList.AddSQLParameter("@Staff_ID", SqlDbType.Int, Staff_ID);

            // Populate and return a list of business objects
            List<StaffFilterDetailsBO> staffFilterDetailList = sqlHelper.GetListOfObjects<StaffFilterDetailsBO>(connStr, SQLQuery, paramList);
         
            return staffFilterDetailList;

        }




        /// <summary>
        /// Get a list of all staff filters
        /// </summary>
        /// <returns></returns>
        public List<StaffFilterBO> GetListOfStaffFilters()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
	StaffFilter_ID
,	StaffFilter_Name
,	Status_ID
FROM dbo.StaffFilter

WHERE Status_ID = @Status_ID
";

            #endregion

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            // Example Paramater
            paramList.AddSQLParameter("@Status_ID", SqlDbType.Int, Status.Active);

            // Populate and return a list of business objects
            List<StaffFilterBO> staffFilterList = sqlHelper.GetListOfObjects<StaffFilterBO>(connStr, SQLQuery, paramList);

            // Add a blank entry to the list
            //staffFilterList.Insert(0, new StaffFilterBO() { StaffFilter_ID = null, StaffFilter_Name = "Please Select", Status_ID = Status.Active });

            return staffFilterList;

        }

        /// <summary>
        /// Get a list of all staff filter links
        /// </summary>
        /// <returns></returns>
        public List<StaffFilterLinkBO> GetListOfStaffFilterLinks()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
	Staff_ID
,	StaffFilter_ID
,	StaffFilterVal_ID
FROM dbo.StaffFilterLink

";

            #endregion


            // Populate and return a list of business objects
            List<StaffFilterLinkBO> staffFilterLinkList = sqlHelper.GetListOfObjects<StaffFilterLinkBO>(connStr, SQLQuery);

            // Add a blank entry to the list
            // MUST FIX
            //staffFilterLinkList.Insert(0, new StaffFilterLinkBO() { Staff_ID = null, StaffFilter_ID = null, StaffFilterVal_ID = null });


            return staffFilterLinkList;

        }

        /// <summary>
        /// Get a list of all staff filter values
        /// </summary>
        /// <returns></returns>
        public List<StaffFilterValBO> GetListOfStaffFilterVals()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
	StaffFilterVal_ID
,	StaffFilter_ID
,	StaffFilterVal_Name
,   StaffFilterVal_Order
FROM dbo.StaffFilterVal

";

            #endregion


            // Populate and return a list of business objects
            List<StaffFilterValBO> staffFilterValList = sqlHelper.GetListOfObjects<StaffFilterValBO>(connStr, SQLQuery);

            // Add a blank entry to the list
            //staffFilterValList.Insert(0, new StaffFilterValBO() { StaffFilterVal_ID = null, StaffFilter_ID = null, StaffFilterVal_Name = "Please Select", StaffFilterVal_Order = null });

            return staffFilterValList;

        }



        /// <summary>
        /// Create a new staff memeber
        /// </summary>
        /// <param name="userDetails">The details of the staff memeber to create</param>
        public void CreateStaffMember(StaffBO userDetails, List<StaffFilterBO> staffFilters)
        {
            

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery5 = @"
SET XACT_ABORT ON;

BEGIN TRANSACTION


    INSERT INTO dbo.Staff
    (
        Staff_Name
    ,   Staff_RegDateT
    ,   Department_ID
    ,   Status_ID
    )
    VALUES
    (
        @Staff_Name
    ,   @Staff_RegDateT
    ,   @Department_ID
    ,   @Status_ID
    );

    INSERT INTO dbo.StaffFilterLink
    (
        Staff_ID
    ,   StaffFilter_ID
    ,   StaffFilterVal_ID
    )
    VALUES       
    {links}    

COMMIT TRANSACTION;

SET XACT_ABORT OFF
";

            #endregion

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            // Example Paramater
            paramList.AddSQLParameter("@Staff_Name", SqlDbType.VarChar, userDetails.Staff_Name.Trim());
            paramList.AddSQLParameter("@Staff_RegDateT", SqlDbType.DateTime, userDetails.Staff_RegDateT);
            paramList.AddSQLParameter("@Department_ID", SqlDbType.Int, userDetails.Department_ID);
            paramList.AddSQLParameter("@Status_ID", SqlDbType.Int, userDetails.Status_ID);


            string links = String.Empty;
            // Input PayGrade, Location, Gender into linking table
            for (int i = 1; i <= userDetails.StaffFilterVal_ID.Count(); i++)
            {
                paramList.AddSQLParameter("@StaffFilter_ID" + i, SqlDbType.Int, i);
                paramList.AddSQLParameter("@StaffFilterVal_ID" + i, SqlDbType.Int, userDetails.StaffFilterVal_ID[i-1]);
                links += "(SCOPE_IDENTITY(), @StaffFilter_ID" + i + ", @StaffFilterVal_ID" + i + "),";
            }

            links = links.TrimEnd(',');
            SQLQuery5 = SQLQuery5.Replace("{links}", links);


            // Execute the query and return number of rows affected
            int rowsAffected = sqlHelper.ExecuteSqlNonQuery(connStr, SQLQuery5, paramList);

        }
    }
}