using SQLHelper;
using SQLHelper.DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Mvc4_Development_Test.Event;
using Mvc4_Development_Test.Customer;

namespace Mvc4_Development_Test.UploadDetails
{

    public class UploadDetailsDAL
    {
        // Create an sqlCommands object from the helper
        SqlCommands sqlHelper;

        // Initialise a string to hold the connection string
        string connStr;

        public UploadDetailsDAL()
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
        /// Create a new event memeber
        /// </summary>
        /// <param name="eventDetails">The details of the staff memeber to create</param>
        public bool UploadDetailsMember(UploadDetailsBO UploadDetailsMember)
        {

            //INSERT INTO dbo._File
            //(
            //    _File_Name
            //,   File_DateT
            //,   Customer_Count
            //,   Event_ID
            //,   Status_ID
            //,   Temp_Table_ID
            //)
            //VALUES
            //(
            //    @_File_Name
            //,   @File_DateT
            //,   @Customer_Count
            //,   @Event_ID
            //,   5
            //,   @Temp_Table_ID
            //)
            //;

            #region CreateCustTable


            string CreateCustTable = @"
SET XACT_ABORT ON;

BEGIN TRANSACTION

        SET DATEFORMAT dmy
        ;
       
        CREATE TABLE {temp_cust_table}
        (
            Customer_ID smallint NOT NULL
        ,   Title_ID int NOT NULL
        ,   Customer_Name varchar(40) NOT NULL
        ,   Customer_School varchar(80) NOT NULL
	    ,   Customer_Email varchar(80) NOT NULL
	    ,   Customer_RegDateT datetime NOT NULL
	    ,   Status_ID smallint NOT NULL
	    ,   Staff_ID smallint NOT NULL
	    ,   Customer_EditDateT datetime NULL
	    ,   Event_ID tinyint NULL
        )
        ;

        CREATE TABLE {temp_cust_serv_table}
        (
            Customer_ID smallint NOT NULL
        ,   Service_ID smallint NOT NULL
        )       
        ;

COMMIT TRANSACTION;

SET XACT_ABORT OFF

";

            #endregion






            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"



    

    INSERT INTO {temp_cust_table}
    (
        Customer_ID
    ,   Title_ID
    ,   Customer_Name
    ,   Customer_School
    ,   Customer_Email
    ,   Customer_RegDateT
    ,   Status_ID
    ,   Staff_ID
    ,   Customer_EditDateT
    ,   Event_ID
    )
    VALUES
    {cust}
    ;    



    INSERT INTO {temp_cust_serv_table}
    (
        Customer_ID
    ,   Service_ID
    )
    VALUES
    {servi}


 
";

            #endregion

            // TGet last ID so far
            #region LastCustID

            string LastCustID = @"

SELECT TOP 1 Customer_ID 
FROM dbo.Customer 
ORDER BY Customer_ID DESC

";
            #endregion

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            string servi = String.Empty;            
            string staff = String.Empty;
            string cust = String.Empty;           
            int cus_ID = 1;
            int lastID = sqlHelper.GetSingleValue<int>(connStr, LastCustID) + 1;
            string guid_dashless = RemoveDashes(UploadDetailsMember.Temp_Table_ID);

            paramList.AddSQLParameter("@Customer_EditDateT", SqlDbType.DateTime, null);            
            paramList.AddSQLParameter("@Temp_Table_ID", SqlDbType.VarChar, guid_dashless);

            //Assign a unique name to temporary tables
            string temp_cust_serv_table = "dbo.TempCustomerServices";
            string temp_cust_table = "dbo.TempCustomer";
            temp_cust_table += guid_dashless;
            temp_cust_serv_table += guid_dashless;
          

            // INSERT INTO _File TABLE
            // ---------------------------------------------------------------------------
            // Create a list for file details 
            List<_FileBO> _FileBO = new List<_FileBO>();
            _FileBO.First()._File_Name = UploadDetailsMember.File.FileName.Trim();
            _FileBO.First().File_DateT = UploadDetailsMember.File_DateT;
            _FileBO.First().Customer_Count = UploadDetailsMember.Customer_Count;
            _FileBO.First().Event_ID = UploadDetailsMember.Event_ID;
            _FileBO.First().Temp_Table_ID = guid_dashless;


            // Insert file details into db
            sqlHelper.BulkInsert<_FileBO>(connStr, "_FileBO", _FileBO);


            // CREATE TEMPORARY CUSTOMER TABLES
            // ---------------------------------------------------------------------------
            CreateCustTable = CreateCustTable.Replace("{temp_cust_table}", temp_cust_table);
            CreateCustTable = CreateCustTable.Replace("{temp_cust_serv_table}", temp_cust_serv_table);
            sqlHelper.ExecuteSqlNonQuery(connStr, CreateCustTable);


            // INSERT INTO TEMPORARY CUSTOMER TABLE
            // ---------------------------------------------------------------------------
            // Create a list of customers
            List<TempCustBO> TempCustBO = new List<TempCustBO>();
            List<TempCustServBO> TempCustServBO = new List<TempCustServBO>();

            int k = 0;
            int skip_rows = 0;
            // Go through every row in the list
            for (int i = 0; i < UploadDetailsMember.ParsedData.Count(); i++)
            {
               
                TempCustBO[i].Customer_ID = lastID + k;
                TempCustBO[i].Title_Name = UploadDetailsMember.ParsedData[i][0].Trim();
                TempCustBO[i].Customer_Name = UploadDetailsMember.ParsedData[i][1].Trim();
                TempCustBO[i].Customer_School = UploadDetailsMember.ParsedData[i][2].Trim();
                TempCustBO[i].Customer_Email = UploadDetailsMember.ParsedData[i][3].Trim();

                // The date string is saved as datetime which can be easily inserted into db
                DateTime dt = DateTime.Parse(UploadDetailsMember.ParsedData[i][4], null, System.Globalization.DateTimeStyles.RoundtripKind);
                TempCustBO[i].Customer_RegDateT = dt;

                TempCustBO[i].Status_ID = (int)UploadDetailsMember.Status_ID;
                TempCustBO[i].Staff_ID = Int16.Parse(UploadDetailsMember.ParsedData[i][5].Trim());
                TempCustBO[i].Event_ID = UploadDetailsMember.Event_ID;





                paramList.AddSQLParameter("@Service_Name" + i, SqlDbType.VarChar, UploadDetailsMember.ParsedData[i][6].Trim());
                                
                servi += "(@Customer_ID" + i + ", (SELECT Service_ID FROM _Service WHERE _Service_Name = @Service_Name" + i + ")),";

                

                // Check whether they are new customers or just service of the current, if so save those into link table
                int j = i + 1;
                while (j < (UploadDetailsMember.ParsedData.Count() - 1) && UploadDetailsMember.ParsedData[j][1].Equals(""))
                {
                    paramList.AddSQLParameter("@Service_Name" + j, SqlDbType.VarChar, UploadDetailsMember.ParsedData[j][6].Trim());
                    servi += "(@Customer_ID" + i + ", (SELECT Service_ID FROM _Service WHERE _Service_Name = @Service_Name" + j + ")),";
                    skip_rows++;
                    j++;
                }


                cust += $@"(    @Customer_ID{i}
                    ,      (SELECT Title_ID FROM Title WHERE Title_Desc = @Title_Name" + i + ")"
                    + ",      @Customer_Name" + i
                    + ",      @Customer_School" + i
                    + ",      @Customer_Email" + i
                    + ",      @Customer_RegDateT" + i
                    + ",      @Status_ID" + i
                    + ",      @Staff_ID" + i
                    + ",      @Customer_EditDateT"
                    + ",      @Event_ID ),";
              
                cus_ID++;
                i += skip_rows;
                k++;
            }
            

            // Removing last unnecessary comma
            servi = servi.TrimEnd(',');
            cust = cust.TrimEnd(',');

            // Replacing in query
            SQLQuery = SQLQuery.Replace("{cust}", cust);
            SQLQuery = SQLQuery.Replace("{servi}", servi);
            

            // Execute the query and return number of rows affected
            int rowsAffected = sqlHelper.ExecuteSqlNonQuery(connStr, SQLQuery, paramList);
            return true;
        }

        /// <summary>
        /// Get a list of all Services
        /// </summary>
        /// <returns></returns>
        public List<CustomerServiceBO> GetListOfTempCustomerServices()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
    tcs.Service_ID
,   tcs.Customer_ID
,	s._Service_Name

FROM {temp_cust_serv_table} tcs
INNER JOIN _Service s
ON s.Service_ID = tcs.Service_ID;

";

            #endregion
            string temp_cust_serv_table = "dbo.TempCustomerServices";
            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            // Paramater
            paramList.AddSQLParameter("@Temp_Table_ID", SqlDbType.VarChar, (temp_cust_serv_table + GetGUID()));
            temp_cust_serv_table += GetGUID();
            SQLQuery = SQLQuery.Replace("{temp_cust_serv_table}", temp_cust_serv_table);

            // Populate and return a list of business objects
            List<CustomerServiceBO> tempCustomerServiceList = sqlHelper.GetListOfObjects<CustomerServiceBO>(connStr, SQLQuery, paramList);

            return tempCustomerServiceList;

        }

        /// <summary>
        /// Get a list of all Services
        /// </summary>
        /// <returns></returns>
        public string GetGUID()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
    Temp_Table_ID

FROM dbo._File
WHERE Status_ID = 5;

";

            #endregion


            // Populate and return a list of business objects
            string guid = sqlHelper.GetSingleValue<string>(connStr, SQLQuery).ToString();
            
            // Return a call to a method that removes dashes
            return RemoveDashes(guid);

        }

        /// <summary>
        /// Remove dashes from Guid
        /// </summary>
        /// <param name="ourString"></param>
        /// <returns></returns>
        public string RemoveDashes(string ourString)
        {
            ourString = ourString.Remove(8, 0);
            ourString = ourString.Remove(12, 0);
            ourString = ourString.Remove(16, 0);
            ourString = ourString.Remove(20, 0);
            ourString = ourString.TrimEnd();

            return ourString;

        }

        /// <summary>
        /// Get a list of all users
        /// </summary>
        /// <returns></returns>
        public List<CustomerBO> GetListOfTempCustomer()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SELECT 
    cust.Customer_ID
,   t.Title_ID
,   t.Title_Desc
,   Customer_Name
,   Customer_School
,   Customer_Email
,   Customer_RegDateT
,   st.Status_ID
,   st.Status_Name
,   Staff_ID
,   Customer_EditDate_T

FROM {temp_table_ID} cust

LEFT JOIN Stat st
ON cust.Status_ID = st.Status_ID

LEFT JOIN Title t
ON cust.Title_ID = t.Title_ID

";

            #endregion
            string temp_cust_table = "dbo.TempCustomer";
            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            // Paramater
            paramList.AddSQLParameter("@Temp_Table_ID", SqlDbType.VarChar, GetGUID().TrimEnd());
            temp_cust_table += GetGUID().TrimEnd();
            SQLQuery = SQLQuery.Replace("{temp_table_ID}", temp_cust_table);

            // Populate and return a list of business objects
            List<CustomerBO> customerList = sqlHelper.GetListOfObjects<CustomerBO>(connStr, SQLQuery, paramList).ToList();

            return customerList;

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

UPDATE dbo.TempCustomer{temp_table_ID}
SET     Customer_EditDateT = @Customer_EditDateT
    ,   Staff_ID = @Staff_ID 

WHERE Customer_ID = @Customer_ID;

";

            #endregion

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            // Example Paramater
            paramList.AddSQLParameter("@Customer_ID", SqlDbType.Int, customer.Customer_ID);
            paramList.AddSQLParameter("@Customer_EditDateT", SqlDbType.DateTime, customer.Customer_EditDateT);
            paramList.AddSQLParameter("@Staff_ID", SqlDbType.Int, customer.Staff_ID);
            paramList.AddSQLParameter("@Temp_Table_ID", SqlDbType.UniqueIdentifier, GetGUID());
            SQLQuery = SQLQuery.Replace("{temp_table_ID}", "@Temp_Table_ID");

            // Execute the query and return number of rows affected
            int rowsAffected = sqlHelper.ExecuteSqlNonQuery(connStr, SQLQuery, paramList);

        }


        /// <summary>
        /// Insert from temp tables into real ones
        /// </summary>
        /// <param name="userDetails">The details of the customer member to create</param>
        public bool AddIntoRealTables()
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
    ,   Customer_EditDateT
    ,   Event_ID
    )
    SELECT 
        
        t.Title_ID
    ,   t.Customer_Name
    ,   t.Customer_School
    ,   t.Customer_Email
    ,   t.Customer_RegDateT
    ,   t.Status_ID
    ,   t.Staff_ID
    ,   t.Customer_EditDateT
    ,   t.Event_ID

    FROM dbo.TempCustomer{temp_table_ID} t    

    WHERE t.Status_ID = 1;



    INSERT INTO dbo.CustomerServices
    (
        Customer_ID
    ,   Service_ID
    )
    SELECT * 
    FROM dbo.TempCustomerServices{temp_table_ID}    
    ;

    UPDATE dbo._File
    SET         Status_ID = 1
    WHERE       _File_ID = (SELECT TOP 1 _File_ID 
                            FROM dbo._File
                            ORDER BY _File_ID DESC);

    DROP TABLE dbo.TempCustomer{temp_table_ID};
    DROP TABLE dbo.TempCustomerServices{temp_table_ID}

COMMIT TRANSACTION;

SET XACT_ABORT OFF
";

            #endregion
            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            // Paramater
            paramList.AddSQLParameter("@Temp_Table_ID", SqlDbType.UniqueIdentifier, GetGUID());
            SQLQuery = SQLQuery.Replace("{temp_table_ID}", "@Temp_Table_ID");

            // Execute the query and return number of rows affected
            int rowsAffected = sqlHelper.ExecuteSqlNonQuery(connStr, SQLQuery, paramList);

            return true;
        }

        /// <summary>
        /// Check if there is a file pending
        /// </summary>
        /// <param name=""></param>
        public bool IsFilePending()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"

    SELECT Status_ID
    FROM dbo._File
    WHERE Status_ID = 5;

";

            #endregion

            if (sqlHelper.GetSingleValue<int>(connStr, SQLQuery) == 5)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if there is a file pending
        /// </summary>
        /// <param name=""></param>
        public bool DeleteFile()
        {

            #region SQLQuery

            // The SQL Query to execute
            string SQLQuery = @"
SET XACT_ABORT ON;

BEGIN TRANSACTION

    DELETE FROM dbo._File
    WHERE Status_ID = 5;

    DROP TABLE dbo.TempCustomer{temp_table_ID};
    DROP TABLE dbo.TempCustomerServices{temp_table_ID}
    
COMMIT TRANSACTION;

SET XACT_ABORT OFF
";

            #endregion

            // Create a parameter list to hold all of the parameters
            SqlParameterList paramList = new SqlParameterList();

            // Paramater
            paramList.AddSQLParameter("@Temp_Table_ID", SqlDbType.UniqueIdentifier, GetGUID());
            SQLQuery = SQLQuery.Replace("{temp_table_ID}", "@Temp_Table_ID");

            int rowsAffected = sqlHelper.ExecuteSqlNonQuery(connStr, SQLQuery, paramList);

            return true;
            
        }
    }
}