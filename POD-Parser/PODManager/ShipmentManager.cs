using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.IO;


using PODBusinessObjects;
//using PODMessaging;
using Permissions;
using PODUtils;
using PODBusinessObjects.DataTransferObjects;


namespace PODManager
{
    public class ShipmentManager
    {
        //TODO:: move to a config file
        public static int BATCH_SIZE = 10;
        
        //TODO:: get user code from intitializing code
        private String _userCode;
        private String _compayCode;
        private long _clientId;
        private String _configFilesRoot;
        private String[] _configFiles;

        private SqlConnection _dbConn;

        #region Constructors
        private ShipmentManager() { }

        public ShipmentManager(String companyCode, String userCode)
        {
            _userCode = userCode;
            _compayCode = companyCode;
            _clientId = -1;
            _configFilesRoot = "";
            _configFiles = new String[] { };
        }

        public ShipmentManager(String companyCode, String userCode, String configFilesRoot)
        {
            _userCode = userCode;
            _compayCode = companyCode;
            _clientId = -1;
            _configFilesRoot = configFilesRoot;
            _configFiles = new String[] { };
        }

        #endregion Constructors

        #region Properties
        public String UserCode
        {
            get { return _userCode; }
            set { _userCode = value; }
        }
        public String CompanyCode
        {
            get { return _compayCode; }
            set { _compayCode = value; }
        }
        public SqlConnection DBConnection
        {
            get { return _dbConn;}
            set { _dbConn = value; }
        }

        public long ClientID
        {
            get { return getClientId(); }
        }
        #endregion Properties


        public enum ASSIGNMENT_STATUS { ALL, UN_ASSIGNED, ASSIGNED, DELIVERED}

        public bool setConfigFilesRoot(String configFilesRoot, String sRetMsgs)
        {
            //checked if file path exists
            bool bRetVal = false;
            sRetMsgs = "";
            DirectoryInfo diConfig = new DirectoryInfo(configFilesRoot);
            if (diConfig.Exists)
            {
                _configFilesRoot = configFilesRoot;
                int nTotalFiles = diConfig.GetFiles().Length;
                int ncurrFile = 0;
                _configFiles = new String[nTotalFiles];

                foreach (FileInfo finfo in diConfig.GetFiles())
                {
                    _configFiles[ncurrFile++] = finfo.Name;
                }

                bRetVal = true;
            }
            else
            {
                sRetMsgs = "Directory Path " + configFilesRoot + " not found. Please check and reset.";
            }

            return bRetVal;
        }

        public String[] getConfigFiles()
        {
            return _configFiles;
        }

        public DataTable getDropdownList(String pageCode, String listName)
        {
            /*Need to put it in DB. Below data for testing only*/
            //String[] dropdownList = new String[] { "Source", "Reference", "Assigned To", "Status" };

            //return dropdownList;
            //Dictionary<int, string> result= new Dictionary<int,string>();
            DataTable datatable = new DataTable();
            try
            {
                if (pageCode == "DataLoad" && listName == "FileType")
                {
                    SqlCommand cmd = _dbConn.CreateCommand();
                    if (_dbConn.State == ConnectionState.Closed)
                        _dbConn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetdropdownListByTypeCode";
                    SqlParameter paramTypeCode = new SqlParameter("@TypeCode", "DATA_TYPE");
                    paramTypeCode.DbType = DbType.String;
                    cmd.Parameters.Add(paramTypeCode);
                    SqlParameter paramErrors = new SqlParameter("@Errors", "");
                    paramErrors.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(paramErrors);

                    datatable = PODManagerDBHelper.getdropdownList(cmd);
                }
            }
            catch (SqlException sqlEx)
            {
                System.Console.WriteLine("Exception : " + sqlEx.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }

            return datatable;
        }

        public DataTable getDropdownList(String pageCode, String listName, String listType)
        {
            DataTable datatable = new DataTable();
            try
            {
                if (pageCode == "DataLoad" && listName == "DataSource" && listType == "SHIPMENTS")
                {
                    SqlCommand cmd = _dbConn.CreateCommand();
                    if (_dbConn.State == ConnectionState.Closed)
                        _dbConn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetdropdownListDataSourceByTypeId";
                    SqlParameter paramTypeCode = new SqlParameter("@TypeId", 1101);
                    paramTypeCode.DbType = DbType.Int32;
                    cmd.Parameters.Add(paramTypeCode);
                    SqlParameter paramClientId = new SqlParameter("@Client_Id", getClientId());
                    paramClientId.DbType = DbType.Int32;
                    cmd.Parameters.Add(paramClientId);
                    SqlParameter paramErrors = new SqlParameter("@Errors", "");
                    paramErrors.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(paramErrors);

                    datatable = PODManagerDBHelper.getdropdownListByListType(cmd);
                }
            }
            catch (SqlException sqlEx)
            {
                System.Console.WriteLine("Exception : " + sqlEx.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }

            return datatable;
        }


        public DataTable getDropdownListForShipment(int typeId)
        {
            DataTable datatable = new DataTable();
            try
            {
                SqlCommand cmd = _dbConn.CreateCommand();
                if (_dbConn.State == ConnectionState.Closed)
                    _dbConn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetdropdownListForShipmentFilterAndSort";
                SqlParameter paramTypeCode = new SqlParameter("@TypeId", typeId);
                paramTypeCode.DbType = DbType.Int32;
                cmd.Parameters.Add(paramTypeCode);
                SqlParameter paramErrors = new SqlParameter("@ClientId", getClientId());
                paramErrors.DbType = DbType.Int32;
                cmd.Parameters.Add(paramErrors);

                datatable = PODManagerDBHelper.getDropdownListForShipment(cmd);
            }
            catch (SqlException sqlEx)
            {
                System.Console.WriteLine("Exception : " + sqlEx.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }

            return datatable;
        }


        private long getClientId()
        {
            if (_clientId == -1)
            {

                SqlCommand cmd = _dbConn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetClientID";
                SqlParameter paramCompanyCode = new SqlParameter("@CompanyCode", _compayCode);
                paramCompanyCode.DbType = DbType.String;
                cmd.Parameters.Add(paramCompanyCode);
                SqlParameter paramUserCode = new SqlParameter("@UserCode", _userCode);
                paramUserCode.DbType = DbType.String;
                cmd.Parameters.Add(paramUserCode);
                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                _clientId = Convert.ToInt64(cmd.ExecuteScalar());

            }
            return _clientId;
        }


        public List<string> validateUser(string CompanyCode, string UserCode, string Password, out string errors)
        {
            errors = string.Empty;
            List<string> userInfo = new List<string>();
            try{

                SqlCommand cmd = _dbConn.CreateCommand();
                if (_dbConn.State != ConnectionState.Open)
                    _dbConn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spValidateUser";
                SqlParameter paramCompanyCode = new SqlParameter("@CompanyCode", CompanyCode);
                paramCompanyCode.DbType = DbType.String;
                cmd.Parameters.Add(paramCompanyCode);
                SqlParameter paramUserCode = new SqlParameter("@UserCode", UserCode);
                paramUserCode.DbType = DbType.String;
                cmd.Parameters.Add(paramUserCode);
                SqlParameter paramPassword = new SqlParameter("@Password", Password);
                paramUserCode.DbType = DbType.String;
                cmd.Parameters.Add(paramPassword);
                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.DbType = DbType.String;
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                userInfo = PODManagerDBHelper.validateUserFromCmd(cmd);

                errors = paramErrors.Value.ToString();
            }
            catch (SqlException sqlEx)
            {
                System.Console.WriteLine("Exception : " + sqlEx.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }

            return userInfo;
        }

        public Dictionary<long, Shipment> getShipmentsByStatus(ASSIGNMENT_STATUS shipmentStatus)
        {
            Dictionary<long, Shipment> shipments = new Dictionary<long, Shipment>();

            try
            {

                SqlCommand cmd = _dbConn.CreateCommand();
                if (_dbConn.State != ConnectionState.Open)
                    _dbConn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetShipmentsByStatusTest";
                SqlParameter paramClientId = new SqlParameter("@ClientID", getClientId());
                paramClientId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramClientId);
                SqlParameter paramShipmentStatus = new SqlParameter("@ShipmentStatus", shipmentStatus);
                paramShipmentStatus.DbType = DbType.Int16;
                cmd.Parameters.Add(paramShipmentStatus);
                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                shipments = PODManagerDBHelper.getShipmentsFromCmd(cmd);

            }
            catch (SqlException sqlEx)
            {
                System.Console.WriteLine("Exception : " + sqlEx.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }

            return shipments;
        }

        public Dictionary<long, Shipment> getShipmentsByStatusFilter(ASSIGNMENT_STATUS shipmentStatus, string ColumnName, string ColumnValue)
        {
            Dictionary<long, Shipment> shipments = new Dictionary<long, Shipment>();

            try
            {

                SqlCommand cmd = _dbConn.CreateCommand();
                if (_dbConn.State != ConnectionState.Open)
                    _dbConn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetShipmentsByStatusFilter";
                SqlParameter paramClientId = new SqlParameter("@ClientID", getClientId());
                paramClientId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramClientId);
                SqlParameter paramShipmentStatus = new SqlParameter("@ShipmentStatus", shipmentStatus);
                paramShipmentStatus.DbType = DbType.Int16;
                cmd.Parameters.Add(paramShipmentStatus);
                SqlParameter paramColumnName = new SqlParameter("@col", ColumnName);
                paramColumnName.DbType = DbType.String;
                cmd.Parameters.Add(paramColumnName);
                SqlParameter paramColumnValue = new SqlParameter("@colVal", ColumnValue);
                paramColumnValue.DbType = DbType.String;
                cmd.Parameters.Add(paramColumnValue);
                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                shipments = PODManagerDBHelper.getShipmentsFromCmd(cmd);

            }
            catch (SqlException sqlEx)
            {
                System.Console.WriteLine("Exception : " + sqlEx.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }

            return shipments;
        }

        public Shipment getShipmentByID(long shipmentId)
        {
            Shipment shipment = new Shipment(getClientId(), shipmentId);
            try
            {

                SqlCommand cmd = _dbConn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetShipmentByID";
                SqlParameter paramClientId = new SqlParameter("@ClientID", getClientId());
                paramClientId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramClientId);
                SqlParameter paramShipmentId = new SqlParameter("@ShipmentID", shipmentId);
                paramShipmentId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramShipmentId);
                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                Dictionary<long, Shipment> shipments = PODManagerDBHelper.getShipmentsFromCmd(cmd);
                if (shipments.Count == 1)
                {
                    shipment = shipments.Values.ElementAt(0);
                }
                else
                {
                    String strError = "Expected on Shipment instead of  " + shipments.Count;
                    throw new Exception(strError);
                }
            }
            catch (SqlException sqlEx)
            {
                System.Console.WriteLine("Exception : " + sqlEx.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }

            return shipment;

        }

        public Contact getContactByID(long contactId)
        {
            Contact retContact= new Contact(getClientId(), contactId);

            try
            {
                SqlCommand cmdCntByID = _dbConn.CreateCommand();
                cmdCntByID.CommandType = System.Data.CommandType.StoredProcedure;
                cmdCntByID.CommandText = "spContact_R";
                SqlParameter paramContactId = new SqlParameter("@ContactID", contactId);
                paramContactId.DbType = DbType.Int64;
                cmdCntByID.Parameters.Add(paramContactId);
                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmdCntByID.Parameters.Add(paramErrors);

                Dictionary<long, Contact> contacts = PODManagerDBHelper.getContactsFromCmd(cmdCntByID);
                if (contacts.Count != 1)
                {
                    System.Console.WriteLine("Exception (getContactByID) : Not a unique Contact found for Contact ID = " + contactId + ", expected one, found " + contacts.Count + ".");

                }
                else
                {
                    retContact = contacts[contactId];
                }
            }
            catch (SqlException sqlEx)
            {
                System.Console.WriteLine("Exception : " + sqlEx.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }

            return retContact;

        }

        public Dictionary<long, Contact> getEmployeesForAssignment()
        {
            Dictionary<long, Contact> employees = new Dictionary<long, Contact>();
            try
            {
                SqlCommand cmd = _dbConn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetContactsByTypeId";
                SqlParameter paramClientId = new SqlParameter("@ClientID", getClientId());
                paramClientId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramClientId);
                SqlParameter paramContactTypeId = new SqlParameter("@ContactTypeID", 101 /*EMP*/);
                paramContactTypeId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramContactTypeId);
                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                employees = PODManagerDBHelper.getContactsFromCmd(cmd);
            }
            catch (SqlException sqlEx)
            {
                System.Console.WriteLine("Exception : " + sqlEx.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }

            return employees;
        }

        public bool assignShipments(List<ShipmentVersion> shipmentVersions, Contact employee, bool bOverride, string errors)
        {
            bool bRetValue = true;
            StringBuilder strErrorBuilder = new StringBuilder();
            strErrorBuilder.AppendLine("Starting Assignment.");

            // change the list of Shipments to a list of ShipmentVersionId's
            List<long> shipmentVersionIds = new List<long>(shipmentVersions.Count);
            for (int i = 0; i < shipmentVersions.Count; ++i)
            {
                shipmentVersionIds.Add(shipmentVersions[i].ShipmentVersionID);
            }

            string strAssignErrors = "";
            bRetValue = assignShipments(shipmentVersionIds, employee, bOverride, strAssignErrors);


            errors += strErrorBuilder.ToString() + strAssignErrors;

            return bRetValue;
        }

        public bool assignShipments(List<long> shipmentVersionIds, Contact employee, bool bOverride, string errors)
        {
            bool bRetValue = false;
            StringBuilder strErrorBuilder = new StringBuilder();
            strErrorBuilder.AppendLine("Starting Assignment.");

            // begin a DB transaction here 
            if (_dbConn.State != ConnectionState.Open)
                _dbConn.Open();

            SqlTransaction tranAssign = _dbConn.BeginTransaction("Assign Transaction");
            try
            {

                if (employee.ContactId < 0) //new employee added on the fly in the assignemnt window
                {
                    PODManagerDBHelper.addContact(_dbConn, tranAssign, employee);
                }
                else if (bOverride) // Employee Phone number changed on the the fly in assignment window
                {
                    PODManagerDBHelper.updateContactPhoneNum(_dbConn, tranAssign, employee);
                }

                string strAssignErrors = "";
                bRetValue = assignShipments(shipmentVersionIds, employee.ContactId, tranAssign, strAssignErrors);


                errors += strErrorBuilder.ToString() + strAssignErrors;
            }
            catch (SqlException sqlExp)
            {
                System.Console.WriteLine("Exception : " + sqlExp.ToString());
                tranAssign.Rollback();
                bRetValue = false;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
                tranAssign.Rollback();
                bRetValue = false;
            }

            finally
            {
                if ( bRetValue == true )
                    tranAssign.Commit();
                else
                    tranAssign.Rollback();

                if (_dbConn.State == ConnectionState.Open)
                    _dbConn.Close();
            }

            return bRetValue;
        }

        public bool addShipmentsDataLoad(DataTable shipmentDataLoadTable, string ShipmentSrc, string LoggedInUser, out string errors)
        {
            bool bRetVal = false;
            string dError = string.Empty;
            try
            {
                SqlCommand cmd = _dbConn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = "spAddShipmentDetail";
                SqlParameter paramShipmentData = cmd.Parameters.AddWithValue("@ShipmentData", shipmentDataLoadTable);
                SqlParameter paramClientId = new SqlParameter("@ClientID", getClientId());
                paramClientId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramClientId);
                SqlParameter paramShipmentSrc = new SqlParameter("@ShipmentSrcRef", ShipmentSrc);
                paramClientId.DbType = DbType.String;
                cmd.Parameters.Add(paramShipmentSrc);
                SqlParameter paramUser = new SqlParameter("@CREATING_USER_CODE", LoggedInUser);
                paramUser.DbType = DbType.String;
                cmd.Parameters.Add(paramUser);
                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                if (_dbConn.State != ConnectionState.Open)
                    _dbConn.Open();
                int nRowsEffected = cmd.ExecuteNonQuery();

                System.Console.WriteLine("return value = " + nRowsEffected);

                bRetVal = true;
                dError = paramErrors.Value.ToString();
            }
            catch (SqlException sqlExp)
            {
                System.Console.WriteLine("Exception : " + sqlExp.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }

            finally
            {
                _dbConn.Close();
            }

            errors = dError;
            return bRetVal;
        }

        public bool assignShipments(List<long> shipmentVersionIds, long employeeId, SqlTransaction tranAssign, string errors)
        {
            bool bRetVal = false;
            try
            {
                SqlCommand cmd = _dbConn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Transaction = tranAssign;

                cmd.CommandText = "spAssignShipments";
                SqlParameter paramShipmentVersionIds = new SqlParameter("@ShipmentVersionIds", Utils.convertIdsToString(shipmentVersionIds));
                paramShipmentVersionIds.DbType = DbType.String;
                cmd.Parameters.Add(paramShipmentVersionIds);
                SqlParameter paramEmployeeId = new SqlParameter("@EmployeeId", employeeId);
                paramEmployeeId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramEmployeeId);
                SqlParameter paramUserCode = new SqlParameter("@UserCode", _userCode);
                paramUserCode.DbType = DbType.String;
                cmd.Parameters.Add(paramUserCode);
                SqlParameter paramDelimiterString = new SqlParameter("@DelimiterString", Utils.DELIMITER_STRING);
                paramDelimiterString.DbType = DbType.String;
                cmd.Parameters.Add(paramDelimiterString);
                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                paramRetVal.DbType = DbType.Int16;
                paramRetVal.Direction = System.Data.ParameterDirection.ReturnValue;
                cmd.Parameters.Add(paramRetVal);

                int nRowsEffected = cmd.ExecuteNonQuery();

                System.Console.WriteLine("@Errors value = " + paramErrors.Value);
                System.Console.WriteLine("@RetVal value = " + paramRetVal.Value);
                System.Console.WriteLine("Rows Effected = " + nRowsEffected);

                bRetVal = true;
            }
            catch (SqlException sqlExp)
            {
                System.Console.WriteLine("SQL Exception in assignShipments(): " + sqlExp.ToString());
                throw sqlExp;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Generic Exception : in assignShipments(): " + ex.ToString());
                throw ex;
            }

            return bRetVal;
        }

        public bool unassignShipments(SqlConnection conn, List<long> shipmentIds)
        {

            //TO DO:: check this logic (24NOV2012)
            bool bRetVal = false;

            if (_dbConn.State != ConnectionState.Open)
                _dbConn.Open();

            SqlTransaction tranUnAssign = _dbConn.BeginTransaction("UnAssign Transaction");

            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = "spUnassignShipments";
                SqlParameter paramShipmentIds = new SqlParameter("@ShipmentIds", Utils.convertIdsToString(shipmentIds));
                paramShipmentIds.DbType = DbType.String;
                cmd.Parameters.Add(paramShipmentIds);
                SqlParameter paramDelimiterString = new SqlParameter("@DelimiterString", Utils.DELIMITER_STRING);
                paramDelimiterString.DbType = DbType.String;
                cmd.Parameters.Add(paramDelimiterString);
                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                conn.Open();
                int nRowsEffected = cmd.ExecuteNonQuery();
                System.Console.WriteLine("return value = " + nRowsEffected);

                bRetVal = true;
            }
            catch (SqlException sqlExp)
            {
                System.Console.WriteLine("Exception : " + sqlExp.ToString());
                tranUnAssign.Rollback();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
                tranUnAssign.Rollback();
            }

            finally
            {
                tranUnAssign.Commit();
                conn.Close();
            }

            return bRetVal;
        }

        public bool saveShipments(string mapperName, StreamReader strShipmentFileAsStream, string userCode)
        {
            bool bRetValue = false;

            List<Shipment> shipments = new List<Shipment>();

            if (mapperName == "LinexCustomMapper")  // need to use enumurator types
            {

                Parsers.LinexDeliveryRunsheetParser lnxParser = new Parsers.LinexDeliveryRunsheetParser(",", "#", getClientId(), userCode);
                shipments = lnxParser.ParseShipments(strShipmentFileAsStream);
                bRetValue = true;
            }
            else if(mapperName == "RavinderMapper")
            {
                // TBD
                Parsers.ShipmentFileParser shpParser = new Parsers.ShipmentFileParser(",", "#", getClientId(), userCode);
                shipments = shpParser.ParseShipments(strShipmentFileAsStream, mapperName);
                bRetValue = true;
            }

            if (bRetValue)
            {

                // begin a DB transaction here 
                if (_dbConn.State != ConnectionState.Open)
                    _dbConn.Open();

                SqlTransaction tranSaveShipments = _dbConn.BeginTransaction("Save Shipments");
                try
                {
                    // this will save the Shipment, ShipmentVersion and Delivery Address
                    foreach (Shipment shp in shipments)
                    {
                        PODManagerDBHelper.SaveShipment(_dbConn, tranSaveShipments, shp);
                    }
                }
                catch (SqlException sqlExp)
                {
                    System.Console.WriteLine("Exception : " + sqlExp.ToString());
                    bRetValue = false;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception : " + ex.ToString());
                    bRetValue = false;
                }

                finally
                {
                    if (bRetValue == true)
                        tranSaveShipments.Commit();
                    else
                        tranSaveShipments.Rollback();

                    if (_dbConn.State == ConnectionState.Open)
                        _dbConn.Close();
                }
            }

            return bRetValue;

        }
        public bool sendShipmentAssignments()
        {
            bool bRetVal = false;


            try
            {

                #region Get Pending Shipment assignment notifications from DB (spPendingShipmentAssignmentNotifications)
                /// <summary>
                /// 
                /// </summary>

                if (_dbConn.State != ConnectionState.Open)
                    _dbConn.Open();


                SqlCommand cmdPndngNotfns = _dbConn.CreateCommand();
                cmdPndngNotfns.CommandType = System.Data.CommandType.StoredProcedure;

                cmdPndngNotfns.CommandText = "spPendingShipmentAssignmentNotifications";
                SqlParameter paramClientId = new SqlParameter("@ClientID", getClientId());
                paramClientId.DbType = DbType.Int64;
                cmdPndngNotfns.Parameters.Add(paramClientId);
                SqlParameter paramBatchSize = new SqlParameter("@BatchSize", BATCH_SIZE);
                paramBatchSize.DbType = DbType.Int16;
                cmdPndngNotfns.Parameters.Add(paramBatchSize);
                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmdPndngNotfns.Parameters.Add(paramErrors);

                SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                paramRetVal.Direction = System.Data.ParameterDirection.ReturnValue;
                cmdPndngNotfns.Parameters.Add(paramRetVal);


                //this returns three record sets, in order - CONTACT, SHIPMENT_ASSIGNMENT and NOTIFICATION
                SqlDataReader drPndngNotfns = cmdPndngNotfns.ExecuteReader();

                // list of assignments 
                Dictionary<long, ShipmentAssignment> pendingShipmntAssign = new Dictionary<long, ShipmentAssignment>();
                // list of notification 
                //Dictionary<long, Notification> pendingNotiofications = new Dictionary<long, Notification>();

                //// Contacts 
                //Dictionary<long, Contact> assignedToList = new Dictionary<long, Contact>();
                //while (drPndngNotfns.Read())
                //{
                //    Contact currContact = PODManagerDBHelper.getContactFromDR(drPndngNotfns);
                //    if (!assignedToList.ContainsKey(currContact.ContactId))
                //    {
                //        assignedToList.Add(currContact.ContactId, currContact);
                //    }
                //    else
                //    {
                //        System.Console.WriteLine("WARNING::Duplicate Contact row returned with ContactID= " + currContact.ContactId);
                //    }
                //}

                // Shipment Assignments
                //drPndngNotfns.NextResult();
                //while (drPndngNotfns.Read())
                //{
                //    ShipmentAssignment currShpAssgnMnt = PODManagerDBHelper.getShipmentAssignmentFromDR(drPndngNotfns, assignedToList);
                //    if (!pendingShipmntAssign.ContainsKey(currShpAssgnMnt.ShipmentAssignmentId))
                //    {
                //        pendingShipmntAssign.Add(currShpAssgnMnt.ShipmentAssignmentId, currShpAssgnMnt);
                //    }
                //    else
                //    {
                //        System.Console.WriteLine("WARNING::Duplicate ShipmentAssignment row returned with ShipmentAssignmentId = " + currShpAssgnMnt.ShipmentAssignmentId);
                //    }
                //}

                // Notificatrions
//                drPndngNotfns.NextResult();
//                while (drPndngNotfns.Read())
//                {
//                    Notification currNotfn = PODMessaging.MessagingDBHelper.getNotificationFromDR(drPndngNotfns);
//                    if (pendingShipmntAssign.ContainsKey(currNotfn.NotificationForID))
//                    {
//                        pendingNotiofications.Add(currNotfn.NotificationID, currNotfn);
//                    }
//                    else
//                    {
//                        System.Console.WriteLine("WARNING::Duplicate Notiofication row NotificationId = " + currNotfn.NotificationID + ", ShipmentAssignmentID = " + currNotfn.NotificationForID);
//                    }
//                }

//                _dbConn.Close();

//                System.Console.WriteLine("@Errors value = " + paramErrors.Value);
////                System.Console.WriteLine("@RetVal value = " + paramRetVal.Value);

//  //              bRetVal = !Convert.ToBoolean( paramRetVal.Value); // 0 means success

////                PODUtils.Utils.LogCollection(pendingShipmntAssign.Values);

                #endregion 

//                //Collection to save Notifications that need to be updated
//                //saves notification id and the notification status
//                SortedList<long, Notification.NOTIFICATION_STATUS> notfnForStatusUpdate = new SortedList<long, Notification.NOTIFICATION_STATUS>(pendingShipmntAssign.Count / 2);

//                //Collection to save notifications that need to send messages 
//                //saves notification id and Notification.MESSAGE_TYPE
//                SortedList<long, Message.MESSAGE_TYPE> notfnForMessage = new SortedList<long, Message.MESSAGE_TYPE>();

                #region loop through the notificatons to see what needs to be sent

/*
                for (int i = 0; i < pendingShipmntAssign.Count; i++)
                {
                    long shpAsgnId = (long)pendingShipmntAssign. ;
                    ShipmentAssignment currShpAsgnmtInit = (ShipmentAssignment)pendingShipmntAssign.GetByIndex(i);

                    System.Console.WriteLine("*INFO*:: Processing Notification = " + shpAsgnNotId);
                    
                    //skip Pending shipment assignment notifications, these will be used in the inside loop to decide 
                    //to skip init message - do not try sending Add/Delete message if there is another notification in pending state

                    if (currShpAsgnmtInit.NotificationStatus == (int)ShipmentAssignment.NOTIFICATION_STATUS.INIT)
                    {
                        for (int j = 0; j < pendingAssignments.Count; j++)
                        {
                            if(j == i)
                            {   
                                //skip to the next one if it is the same record
                                continue;
                            }

                            ShipmentAssignment currShpAsgnmtPending = (ShipmentAssignment)pendingAssignments.GetByIndex(j);
                            if (   (currShpAsgnmtInit.ShipmentAssignmentId == currShpAsgnmtPending.ShipmentAssignmentId)
                                && (currShpAsgnmtPending.NotificationStatus == (int)ShipmentAssignment.NOTIFICATION_STATUS.PENDING)
                                )
                            {
                                //we do not want to send another notification for a Assignemnt with a pending notification
                                String Info = "*INFO*:: Marking ShipmentAssignmentNotificationID = " + shpAsgnNotId + " as Waiting";
                                Info += ", as it has a pending Notification ShipmentAssignmentNotificationID = " + pendingAssignments.GetKey(j);

                                currShpAsgnmtInit.NotificationStatus = (int)ShipmentAssignment.NOTIFICATION_STATUS.WAITING;
                                notfnForStatusUpdate.Add(shpAsgnNotId, currShpAsgnmtInit);
                                System.Console.WriteLine(Info);
                                break;
                            }
                            else if (   (currShpAsgnmtInit.ShipmentAssignmentId == currShpAsgnmtPending.ShipmentAssignmentId)
                                     && (currShpAsgnmtPending.NotificationStatus == (int)ShipmentAssignment.NOTIFICATION_STATUS.INIT)
                                     )
                            {
                                // if we found another notification in the init state for the same assignment, it
                                // should be for the opposite action Add vs. delete. Skip both 

                                //cross check that they are of the opposite kind and Assignment is not active 
                                if( (currShpAsgnmtInit.MessageType == (int) ShipmentAssignment.MESSAGE_TYPE.ADD && currShpAsgnmtPending.MessageType == (int)ShipmentAssignment.MESSAGE_TYPE.DELETE)
                                    || (currShpAsgnmtInit.MessageType == (int) ShipmentAssignment.MESSAGE_TYPE.DELETE && currShpAsgnmtPending.MessageType == (int)ShipmentAssignment.MESSAGE_TYPE.ADD) 
                                    )
                                {
                                    if( ! currShpAsgnmtInit.isActive )
                                    {
                                        //two init notifications found for the same assignment id
                                        //skip sending assignment message as the assignment is not active
                                        System.Console.WriteLine("Skipping ShipmentAssignmentNotificationID = " + shpAsgnNotId );
                                        System.Console.WriteLine("Skipping ShipmentAssignmentNotificationID = " + (long)pendingAssignments.GetKey(j));

                                        currShpAsgnmtInit.NotificationStatus = (int)ShipmentAssignment.NOTIFICATION_STATUS.SKIPPED;
                                        notfnForStatusUpdate.Add(shpAsgnNotId, currShpAsgnmtInit);

                                        currShpAsgnmtPending.NotificationStatus = (int)ShipmentAssignment.NOTIFICATION_STATUS.SKIPPED;
                                        notfnForStatusUpdate.Add((long)pendingAssignments.GetKey(j), currShpAsgnmtPending);

                                        //move to the next notification 
                                        break;
                                    }
                                    else //we should never be here 
                                    {
                                        //TODO:: How to recover from this condition
                                        String Err = "*ERROR*:: Invalid state of ShipmentAssignmentID = " + currShpAsgnmtInit.ShipmentAssignmentId;
                                        Err += " with Notification ID's = " +  shpAsgnNotId + ", " + (long)pendingAssignments.GetKey(j);
                                        System.Console.WriteLine(Err);
                                    }
                                }
                            }
                        }
                    }

                }

                //Now, whatever is in INIT state, needs to be sent and marked as PENDING 
                for (int i = 0; i < pendingAssignments.Count; i++)
                {
                    long shpAsgnNotId = (long)pendingAssignments.GetKey(i);
                    ShipmentAssignment currShpAsgnmtInit = (ShipmentAssignment)pendingAssignments.GetByIndex(i);

                    if (currShpAsgnmtInit.NotificationStatus == (int)ShipmentAssignment.NOTIFICATION_STATUS.INIT)
                    {

                        System.Console.WriteLine("*INFO*:: Second pass - Processing Notification = " + shpAsgnNotId);
                        if (currShpAsgnmtInit.MessageType == (int)ShipmentAssignment.MESSAGE_TYPE.ADD)
                        {
                            //make sure that the ADD message has an active Assignment 
                            if (currShpAsgnmtInit.isActive)
                            {
                                System.Console.WriteLine("Scheduling Add message for ShipmentAssignmentNotificationID = " + shpAsgnNotId);

                                currShpAsgnmtInit.NotificationStatus = (int)ShipmentAssignment.NOTIFICATION_STATUS.PENDING;
                                notfnForMessage.Add((long)pendingAssignments.GetKey(i), currShpAsgnmtInit);
                            }
                            else
                            {
                                //TODO:: How to recover from this condition
                                String Err = "*ERROR*:: Invalid Message_type of ShipmentAssignmentNotificationID = " + shpAsgnNotId;
                                Err += " with Assignment ID = " + (long)currShpAsgnmtInit.ShipmentAssignmentId;
                                Err += "\nADD notification Message_Type should have an active Assignment";
                                System.Console.WriteLine(Err);
                            }
                        }
                        else if (currShpAsgnmtInit.MessageType == (int)ShipmentAssignment.MESSAGE_TYPE.DELETE)
                        {
                            //make sure that the DELETE message does not have an active Assignment 
                            if (!currShpAsgnmtInit.isActive)
                            {
                                System.Console.WriteLine("Scheduling Delete message for ShipmentAssignmentNotificationID = " + shpAsgnNotId);

                                currShpAsgnmtInit.NotificationStatus = (int)ShipmentAssignment.NOTIFICATION_STATUS.PENDING;
                                notfnForMessage.Add((long)pendingAssignments.GetKey(i), currShpAsgnmtInit);
                            }
                            else
                            {
                                //TODO:: How to recover from this condition
                                String Err = "*ERROR*:: Invalid Message_type of ShipmentAssignmentNotificationID = " + shpAsgnNotId;
                                Err += " with Assignment ID = " + (long)currShpAsgnmtInit.ShipmentAssignmentId;
                                Err += "\nDELETE notification Message_Type should not have an active Assignment";
                                System.Console.WriteLine(Err);
                            }
                        }
                        else if (currShpAsgnmtInit.MessageType == (int)ShipmentAssignment.MESSAGE_TYPE.UPDATE)
                        {
                            //TODO:: Need to handle code fo update message
                            System.Console.WriteLine ("*ERROR*:: Unhandled Message_type of ShipmentAssignmentNotificationID = " + shpAsgnNotId);

                            currShpAsgnmtInit.NotificationStatus = (int)ShipmentAssignment.NOTIFICATION_STATUS.PENDING;
                            notfnForMessage.Add((long)pendingAssignments.GetKey(i), currShpAsgnmtInit);
                        }
                    }
                }

 */
                #endregion

//                UpdateNotifications(notfnForStatusUpdate);

                // for now, send all notifications
                Dictionary<long, ShipmentAssignment>.Enumerator pendingShipmntAssgnEnum = pendingShipmntAssign.GetEnumerator();

                while (pendingShipmntAssgnEnum.MoveNext())
                {
                    ShipmentAssignment currShpAssng = pendingShipmntAssgnEnum.Current.Value;
//                    notfnForMessage.Add(currShpAssng.AssignmentNotification.NotificationID, currShpAssng.AssignmentNotification.MessageType);
                }


                // Call Messaging module for sending actual messages 

                bRetVal = true;
            }
            catch (SqlException sqlExp)
            {
                System.Console.WriteLine("Exception : " + sqlExp.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }
            finally
            {
                if (_dbConn.State == ConnectionState.Open)
                    _dbConn.Close();
            }

            return bRetVal;

        }

        private bool UpdateNotifications(SortedList notfnForStatusUpdate)
        {
            bool bRetValue = true;

            #region AllFunction
            /*
            //check that we have some work to do
            if (notfnForStatusUpdate.Count == 0 && notfnForStatusUpdate.Count == 0)
            {
                Console.WriteLine("ShipmentManager::ProcessNotifications()\n\tNo notiofications to update or schedule for messaging.");
                return bRetValue;
            }
            PODUtils.Utils.LogCollection(notfnForStatusUpdate);
            PODUtils.Utils.LogCollection(notfnForMessage);


            //SQlCommand object for updating the status of ShipmentAssignmentNotification(s)
            SqlCommand cmdStatusUpd = conn.CreateCommand();
            cmdStatusUpd.CommandType = System.Data.CommandType.StoredProcedure;
            cmdStatusUpd.CommandText = "spUpdNotificationStatus";
            SqlParameter paramStatusNtfnID = new SqlParameter("@NotificationID", DbType.Int64);
            cmdStatusUpd.Parameters.Add(paramStatusNtfnID);
            SqlParameter paramNtfnStatusID = new SqlParameter("@NotificationStatusID", DbType.Int64);
            cmdStatusUpd.Parameters.Add(paramNtfnStatusID);
            SqlParameter paramMessageID = new SqlParameter("@MessageID", DbType.Int64);
            cmdStatusUpd.Parameters.Add(paramMessageID);
            SqlParameter paramUserCode = new SqlParameter("@UserCode", DbType.String);
            cmdStatusUpd.Parameters.Add(paramUserCode);
            SqlParameter paramTimeStamp = new SqlParameter("@TimeStamp", DbType.Binary);
            cmdStatusUpd.Parameters.Add(paramTimeStamp);
            SqlParameter paramErrors = new SqlParameter("@Errors", DbType.String);
            paramErrors.Direction = System.Data.ParameterDirection.Output;
            cmdStatusUpd.Parameters.Add(paramErrors);

            //SQlCommand object for scheduling messages ShipmentAssignmentNotification(s)
            SqlCommand cmdScheduleMsg = conn.CreateCommand();
            cmdScheduleMsg.CommandType = System.Data.CommandType.StoredProcedure;
            cmdScheduleMsg.CommandText = "spScheduleMessgeForNotification";
            SqlParameter paramMsgNtfnID = new SqlParameter("@NotificationID", DbType.Int64);
            cmdScheduleMsg.Parameters.Add(paramMsgNtfnID);
            SqlParameter paramMsgTypeID = new SqlParameter("@MessageTypeID", DbType.Int64);
            cmdScheduleMsg.Parameters.Add(paramMsgTypeID);
            SqlParameter paramMsgUserCode = new SqlParameter("@UserCode", DbType.String);
            cmdScheduleMsg.Parameters.Add(paramMsgUserCode);
            SqlParameter paramNtfnTimestamp = new SqlParameter("@NotificationTimestamp", DbType.Binary);
            cmdScheduleMsg.Parameters.Add(paramNtfnTimestamp);
            SqlParameter paramErrors2 = new SqlParameter("@Errors", DbType.String);
            paramErrors2.Direction = System.Data.ParameterDirection.Output;
            cmdScheduleMsg.Parameters.Add(paramErrors2);

            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            cmdStatusUpd.Connection = conn;
            cmdStatusUpd.Transaction = tran;
            cmdScheduleMsg.Connection = conn;
            cmdScheduleMsg.Transaction = tran;

            try
            {
                int nRetVal;
                for (int i = 0; i < notfnForStatusUpdate.Count; i++)
                {
                    ShipmentAssignment currShpAsgnmt =  (ShipmentAssignment)notfnForStatusUpdate.GetByIndex(i);
                    Console.WriteLine("Current Notification ID = " + (long)notfnForStatusUpdate.GetKey(i) + ", ShipmentAssignment = " + currShpAsgnmt.ToString());

                    paramStatusNtfnID.Value = (long)notfnForStatusUpdate.GetKey(i);
                    paramNtfnStatusID.Value = currShpAsgnmt.NotificationStatus;
                    paramMessageID.Value = DBNull.Value;
                    paramUserCode.Value = _userCode;
                    paramTimeStamp.Value = currShpAsgnmt.TimeStamp;

                    nRetVal = cmdStatusUpd.ExecuteNonQuery();
                }

                for (int i = 0; i < notfnForMessage.Count; i++)
                {
                    ShipmentAssignment currShpAsgnmt = (ShipmentAssignment)notfnForMessage.GetByIndex(i);
                    Console.WriteLine("Current Notification ID = " + (long)notfnForMessage.GetKey(i) + ", ShipmentAssignment = " + currShpAsgnmt.ToString());

                    paramMsgNtfnID.Value = (long)notfnForMessage.GetKey(i);
                    paramMsgTypeID.Value = currShpAsgnmt.MessageType;
                    paramMsgUserCode.Value = _userCode;
                    paramNtfnTimestamp.Value = currShpAsgnmt.TimeStamp;
                    nRetVal = cmdScheduleMsg.ExecuteNonQuery();
                }

                tran.Commit();

            }
            catch (SqlException sqlExp)
            {
                System.Console.WriteLine("Exception : " + sqlExp.ToString());
                tran.Rollback();
                bRetValue = false;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
                tran.Rollback();
                bRetValue = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
*/
            #endregion AllFunction

            return bRetValue;
        }


        /*for testing only */
        public Dictionary<long, Shipment> getShipmentsByStatusTest()
        {
            long nClientId = 5;
            Dictionary<long, Shipment> retShipments = new Dictionary<long, Shipment>(2);

            Shipment shp1 = new PODBusinessObjects.Shipment(nClientId, 1001);
            shp1.ShipmentSrc = "FEDEX";
            shp1.ShipmentRef = "FEDEX-12345600";
            shp1.NumOfPieces = 2;

            ShipmentVersion shp1ver1 = new ShipmentVersion(5001, shp1.ShipmentId);
            shp1ver1.ConsigneeName = "Kim Lee";
            shp1ver1.ReceiverName = "Ashok Kumar";
            shp1ver1.ReceiverPhone = "981-110-9898";
            shp1ver1.Weight = "2.245 Kg";
            shp1ver1.VersionNum = 1;

            Address shp1ver1DeliverToAddr = new Address();
            shp1ver1DeliverToAddr.FullAddress = "C-212, Ram Enclave, Ghaziabad, Uttar Pradesh, 201011";
            shp1ver1.DeliverTo = shp1ver1DeliverToAddr;

            shp1.AddVersion(shp1ver1);

            ShipmentVersion shp1ver2 = new ShipmentVersion(5001, shp1.ShipmentId);
            shp1ver2.ConsigneeName = "Kim Lee";
            shp1ver2.ReceiverName = "Kishore Kumar";
            shp1ver2.ReceiverPhone = "981-110-9898";
            shp1ver2.Weight = "2.245 Kg";
            shp1ver2.VersionNum = 2;

            Address shp1ver2DeliverToAddr = new Address();
            shp1ver2DeliverToAddr.FullAddress = "C-212, Ram Enclave, Ghaziabad, Uttar Pradesh, 201011";
            shp1ver2.DeliverTo = shp1ver2DeliverToAddr;

            Contact shp1ver2AssignedTo = new Contact(nClientId, 9999); // dummy contact ID
            shp1ver2AssignedTo.FullName = "John Smith";
            shp1ver2AssignedTo.PhoneNumber = 5559012567;
            ShipmentAssignment shp1ver2Assgnmnt = new ShipmentAssignment(-1, shp1ver2.ShipmentVersionID, true);
            shp1ver2Assgnmnt.AssignedTo = shp1ver2AssignedTo;
            shp1ver2.AddAssignment(shp1ver2Assgnmnt);

            shp1.AddVersion(shp1ver2);

            ShipmentPermission shp1Permission = new ShipmentPermission(nClientId, shp1.ShipmentId);
            shp1Permission.CanAssign = true;
            shp1Permission.CanUnassign = true;
            shp1Permission.CanEdit = true;
            shp1Permission.CanCancel = false;

            shp1.Permissions = shp1Permission;

            retShipments.Add(shp1.ShipmentId, shp1);

            return retShipments;

        }

        public List<RenderingInfo> getRenderingInfoForEmployees()
        {
            List<RenderingInfo> objRenderingInfoList = new List<RenderingInfo>(4);

            RenderingInfo objRenderingInfo11 = new RenderingInfo();
            objRenderingInfo11.ColumnOrder = 1;
            objRenderingInfo11.ColumnName = "FullName";
            objRenderingInfo11.Header = "Assign To";
            objRenderingInfo11.Type = "String";
            objRenderingInfo11.Visible = 'Y';

            FormatInfo objFormatInfo11 = new FormatInfo();
            objFormatInfo11.Width = 200;
            objFormatInfo11.Justify = "Left";

            objRenderingInfo11.Format = objFormatInfo11;
            objRenderingInfoList.Add(objRenderingInfo11);
            /********************************************************************************/

            RenderingInfo objRenderingInfo2 = new RenderingInfo();
            objRenderingInfo2.ColumnOrder = 2;
            objRenderingInfo2.ColumnName = "PhoneNumber";
            objRenderingInfo2.Header = "Phone Number";
            objRenderingInfo2.Type = "String";
            objRenderingInfo2.Visible = 'Y';

            FormatInfo objFormatInfo2 = new FormatInfo();
            objFormatInfo2.Width = 150;
            objFormatInfo2.Justify = "Left";

            objRenderingInfo2.Format = objFormatInfo2;
            objRenderingInfoList.Add(objRenderingInfo2);
            /********************************************************************************/

            RenderingInfo objRenderingInfo3 = new RenderingInfo();
            objRenderingInfo3.ColumnOrder = 3;
            objRenderingInfo3.ColumnName = "NewPhone";
            objRenderingInfo3.Header = "New Phone Number";
            objRenderingInfo3.Type = "String";
            objRenderingInfo3.Visible = 'Y';

            FormatInfo objFormatInfo3 = new FormatInfo();
            objFormatInfo3.Width = 200;
            objFormatInfo3.Justify = "Left";

            objRenderingInfo3.Format = objFormatInfo3;
            objRenderingInfoList.Add(objRenderingInfo3);
            /********************************************************************************/

            RenderingInfo objRenderingInfo4 = new RenderingInfo();
            objRenderingInfo4.ColumnOrder = 4;
            objRenderingInfo4.ColumnName = "ContactId";
            objRenderingInfo4.Header = "";
            objRenderingInfo4.Type = "Number";
            objRenderingInfo4.Visible = 'N';

            objRenderingInfoList.Add(objRenderingInfo4);
            /********************************************************************************/
            return objRenderingInfoList;
        }
        public List<RenderingInfo> getRenderingInfo(int TypeId)
        {
            List<RenderingInfo> RenderingInfoList = new List<RenderingInfo>();

            try
            {

                SqlCommand cmd = _dbConn.CreateCommand();
                if (_dbConn.State != ConnectionState.Open)
                    _dbConn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetAppUserPreference";
                SqlParameter paramClientId = new SqlParameter("@ClientID", getClientId());
                paramClientId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramClientId);
                SqlParameter paramTypeCode = new SqlParameter("@TypeId", TypeId); //1101-Shipment; 1102-Contact
                paramTypeCode.DbType = DbType.Int32;
                cmd.Parameters.Add(paramTypeCode);

                RenderingInfoList = PODManagerDBHelper.getUserPreferenceForShipmentsFromCmd(cmd);

            }
            catch (SqlException sqlEx)
            {
                System.Console.WriteLine("Exception : " + sqlEx.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }

            return RenderingInfoList;
        }
    }


    public class FormatInfo
    {
        private int _width;
        private string _justify;

        public FormatInfo()
        { }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public string Justify
        {
            get { return _justify; }
            set { _justify = value; }
        }
    }

    public class RenderingInfo
    {
        private int _userId;
        private int _columnOrder;
        private string _columnName;
        private string _header;
        private string _type;
        private char _visible;
        private FormatInfo _format;

        public RenderingInfo()
        { }

        public int ColumnOrder
        {
            get { return _columnOrder; }
            set { _columnOrder = value; }
        }
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public char Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
        public FormatInfo Format
        {
            get { return _format; }
            set { _format = value; }
        }

    }

}
