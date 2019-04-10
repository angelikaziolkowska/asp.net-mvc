using SQLHelper;
using SQLHelper.DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Mvc4_Development_Test.StaffReport
{

    public class StaffReportDAL
    {
        // Create an sqlCommands object from the helper
        SqlCommands sqlHelper;

        // Initialise a string to hold the connection string
        string connStr;

        public StaffReportDAL()
        {

            // Initialise the SQL Helper
            sqlHelper = new SqlCommands();

            // Set the connection string to the default connection
            connStr = ConfigurationManager.ConnectionStrings["Default"].ToString();

        }
        public List<Staff_ReportBO> GetStaffReportList()
        {
            return GetStaffReportList(null, null);
        }

        /// <summary>
        /// Get a list of all users
        /// </summary>
        /// <returns></returns>
        public List<Staff_ReportBO> GetStaffReportList(int? SearchFilter, int? SearchDepartment)
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"

SELECT      s.Staff_Name As Staff_Name
      ,     s.Staff_ID As Staff_ID
      ,	    d.Department_Name As Department
	  ,     CONVERT(varchar, s.Staff_RegDateT, 103) As Registered
      ,	    st.Status_Name As 'Status'
	  ,     COUNT(c.Customer_ID) As 'Customer_Count'
      ,     MAX(v.StaffFilterVal_Order) As StaffFilterVal_Order

FROM dbo.Staff s

INNER JOIN Department d
ON s.Department_ID = d.Department_ID

INNER JOIN Stat st
ON s.Status_ID = st.Status_ID

LEFT JOIN Customer c
ON s.Staff_ID = c.Staff_ID

LEFT JOIN StaffFilterLink l 
ON s.Staff_ID = l.Staff_ID 

LEFT JOIN StaffFilter f 
ON l.StaffFilter_ID = f.StaffFilter_ID 

RIGHT JOIN StaffFilterVal v 
ON v.StaffFilter_ID = l.StaffFilter_ID 

AND l.StaffFilterVal_ID = v.StaffFilterVal_ID 

{filters}
{dept}

GROUP BY s.Staff_ID, s.Staff_Name, d.Department_Name, s.Staff_RegDateT, st.Status_Name

UNION ALL

SELECT      MAX(v.StaffFilterVal_Name) AS StaffFilterVal_Name
    ,       999 AS Staff_ID
    ,       '' AS Department
    ,       null AS Registered
    ,       '' AS 'Status'
    ,       COUNT(c.Customer_ID)
    ,       MAX(v.StaffFilterVal_Order) As StaffFilterVal_Order

FROM dbo.Staff s

INNER JOIN Department d
ON s.Department_ID = d.Department_ID

INNER JOIN Stat st
ON s.Status_ID = st.Status_ID

LEFT JOIN Customer c
ON s.Staff_ID = c.Staff_ID

LEFT JOIN StaffFilterLink l 
ON s.Staff_ID = l.Staff_ID 

LEFT JOIN StaffFilter f 
ON l.StaffFilter_ID = f.StaffFilter_ID 

RIGHT JOIN StaffFilterVal v 
ON v.StaffFilter_ID = l.StaffFilter_ID 

AND l.StaffFilterVal_ID = v.StaffFilterVal_ID 

{filters}
{dept2}
{group}


ORDER BY    Staff_ID ASC
    {order}
";

            #endregion

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();
           
            string filters = String.Empty;
            string dept = String.Empty; // 
            string dept2 = String.Empty; //
            string group = String.Empty;
            string order = String.Empty;

            if (SearchFilter.HasValue)           
            {               
                filters += " WHERE v.StaffFilter_ID = @SearchFilter";
                group += " GROUP BY    f.StaffFilter_Name, StaffFilterVal_Name, StaffFilterVal_Order ";
                order = ", StaffFilterVal_Order ASC";
                paramList.AddSQLParameter("@SearchFilter", SqlDbType.Int, SearchFilter);               
            }
            if (SearchDepartment.HasValue)
            {
                if (!SearchFilter.HasValue)
                {
                    dept += " WHERE";
                    dept2 += " WHERE";
                    order += ", StaffFilterVal_Order ASC";
                }
                else 
                {
                    dept += " AND";
                    dept2 += " AND";
                    order += ", StaffFilterVal_Order ASC";
                }
                dept += " d.Department_ID = @SearchDepartment";
                dept2 += " d.Department_ID = @SearchDepartment";
                order = ", StaffFilterVal_Order ASC";
                paramList.AddSQLParameter("@SearchDepartment", SqlDbType.Int, SearchDepartment);
            }


            // Replace in query            
            SQLQuery = SQLQuery.Replace("{filters}", filters);       
            SQLQuery = SQLQuery.Replace("{dept}", dept);
            SQLQuery = SQLQuery.Replace("{dept2}", dept2);
            SQLQuery = SQLQuery.Replace("{group}", group);
            SQLQuery = SQLQuery.Replace("{order}", order);


            // Populate and return a list of business objects
            List<Staff_ReportBO> staff_reportList = sqlHelper.GetListOfObjects<Staff_ReportBO>(connStr, SQLQuery, paramList);

            return staff_reportList;
        }
    }
}