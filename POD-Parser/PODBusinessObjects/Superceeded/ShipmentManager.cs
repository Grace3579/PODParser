using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using PODMessaging;
using PODUtils;


namespace PODBusinessObjects
{
    public class ShipmentManager
    {
        //TODO:: move to a config file
        public static int BATCH_SIZE = 10;
        
        //TODO:: get user code from intitializing code
        private String _userCode;

        public String UserCode
        {
            get { return _userCode; }
            set { _userCode = value; }
        }

        public ShipmentManager(String userCode)
        {
            _userCode = userCode;
        }
        public enum SHIPMENT_STATUS { ALL, UN_ASSIGNED, ASSIGNED, DELIVERED}

        public SortedList getShipmentsByType(SqlConnection conn, SHIPMENT_STATUS shipmentStatus)
        {

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "spGetShipmentsByStatus";
            SqlParameter paramShipmentStatus = new SqlParameter("@ShipmentStatus", shipmentStatus);
            paramShipmentStatus.DbType = DbType.Int16;
            cmd.Parameters.Add(paramShipmentStatus);
            SqlParameter paramErrors = new SqlParameter("@Errors", "");
            paramErrors.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(paramErrors);

            SqlDataReader dr = cmd.ExecuteReader();
            int currRecord = 0;
            SortedList shipmentsList = new SortedList();

            while (dr.Read())
            {

                Console.WriteLine( "Record # = " + ++currRecord
                                + ", SHIPMENT_ID = " + dr["SHIPMENT_ID"]
                                + ", SHIPMENT_SRC_REF = " + dr["SHIPMENT_SRC_REF"] 
                                + ", SHIPMENT_REF = " + dr["SHIPMENT_REF"]);

                Shipment currShipment = new Shipment();
                currShipment.setShipmentId(Convert.ToInt64(dr["SHIPMENT_ID"]));
                currShipment.setShipmentSrcRef(Convert.ToString(dr["SHIPMENT_SRC_REF"]));
                currShipment.setShipmentRef(Convert.ToString(dr["SHIPMENT_REF"]));
                currShipment.setDeliverTo(Convert.ToString(dr["DELIVER_TO"]));
                if (shipmentStatus == SHIPMENT_STATUS.ASSIGNED)
                {
                    currShipment.setAllocatedTo(Convert.ToString(dr["ALLOCATED_TO"]));
                }

                shipmentsList.Add(currShipment.getShipmentId(), currShipment);
            }

            return shipmentsList;
        }

        public bool assignShipments(SqlConnection conn, List<long> shipmentIds, long employeeId)
        {
            bool bRetVal = false;
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = "spAssignShipments";
                SqlParameter paramShipmentIds = new SqlParameter("@ShipmentIds", Utils.convertIdsToString(shipmentIds));
                paramShipmentIds.DbType = DbType.String;
                cmd.Parameters.Add(paramShipmentIds);
                SqlParameter paramEmployeeId = new SqlParameter("@EmployeeId", employeeId);
                paramEmployeeId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramEmployeeId);
                SqlParameter paramDelimiterString = new SqlParameter("@DelimiterString", Utils.DELIMITER_STRING);
                paramDelimiterString.DbType = DbType.String;
                cmd.Parameters.Add(paramDelimiterString);
                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);
                /*            SqlParameter paramRetVal = new SqlParameter("@Errors", employeeId);
                            paramErrors.Direction = System.Data.ParameterDirection.Output;
                            cmd.Parameters.Add(paramErrors);
                */
                conn.Open();
                int nRowsEffected = cmd.ExecuteNonQuery();

                System.Console.WriteLine("return value = " + nRowsEffected);

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
                conn.Close();
            }

            return bRetVal;
        }

        public bool unassignShipments(SqlConnection conn, List<long> shipmentIds)
        {
            bool bRetVal = false;
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
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : " + ex.ToString());
            }

            finally
            {
                conn.Close();
            }

            return bRetVal;
        }

        public bool sendShipmentAssignments(SqlConnection conn)
        {
            bool bRetVal = false;
            try
            {

                #region Get Pending Shipment assignment notifications from DB (spPendingShipmentAssignmentNotifications)
                /// <summary>
                /// 
                /// </summary>
                
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = "spPendingShipmentAssignmentNotifications";
                SqlParameter paramBatchSize = new SqlParameter("@BatchSize", BATCH_SIZE);
                paramBatchSize.DbType = DbType.Int16;
                cmd.Parameters.Add(paramBatchSize);
                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                int currRecord = 0;

                SortedList pendingAssignments = new SortedList();
                
                while (dr.Read())
                {

                    Console.WriteLine( "Record # = " + ++currRecord
                        + ", SHIPMENT_ASSIGNMENT_NOTIFICATION_ID = " + dr["SHIPMENT_ASSIGNMENT_NOTIFICATION_ID"]
                        + ", SHIPMENT_ASSIGNMENT_ID = " + dr["SHIPMENT_ASSIGNMENT_ID"]
                        + ", SHIPMENT_ID = " + dr["SHIPMENT_ID"]
                        + ", ASSIGNMENT_IS_ACTIVE = " + dr["ASSIGNMENT_IS_ACTIVE"]
                        + ", NOTIFICATION_STATUS_ID = " + dr["NOTIFICATION_STATUS_ID"]
                        + ", MESSAGE_TYPE_ID = " + dr["MESSAGE_TYPE_ID"]
                        + ", UPDATED_TIME = " + dr["UPDATED_TIME"]
                        + ", TIMESTAMP = " + PODUtils.Utils.ByteArrayToString((Byte[])dr["TIMESTAMP"])
                            );

                    ShipmentAssignment currShpAsgnmt 
                            = new ShipmentAssignment(Convert.ToInt64(dr["SHIPMENT_ASSIGNMENT_ID"])
                                                        , Convert.ToInt64(dr["SHIPMENT_ID"])
                                                        , Convert.ToBoolean(dr["ASSIGNMENT_IS_ACTIVE"])
                                                    );

                    currShpAsgnmt.NotificationStatus = Convert.ToInt16(dr["NOTIFICATION_STATUS_ID"]);
                    currShpAsgnmt.MessageType = Convert.ToInt16(dr["MESSAGE_TYPE_ID"]);
                    if (! dr.IsDBNull(dr.GetOrdinal("UPDATED_TIME") ) )
                        currShpAsgnmt.UpdateTime = Convert.ToDateTime(dr["UPDATED_TIME"]);

                    currShpAsgnmt.TimeStamp  = (Byte[])dr["TIMESTAMP"];

                    pendingAssignments.Add(Convert.ToInt64(dr["SHIPMENT_ASSIGNMENT_NOTIFICATION_ID"]), currShpAsgnmt);

                }
                conn.Close();

                System.Console.WriteLine("*INFO*:: Pending Notification(s) Read = " + pendingAssignments.Count);
                PODUtils.Utils.LogCollection(pendingAssignments);

                #endregion 

                #region loop through the notificatons to see what needs to be sent

                /*Collection to save Notifications that need to be updated*/
                /*saves notification id and the notification status*/
                SortedList notfnForStatusUpdate = new SortedList(pendingAssignments.Count/2);

                for (int i = 0; i < pendingAssignments.Count; i++)
                {
                    long shpAsgnNotId = (long)pendingAssignments.GetKey(i);
                    ShipmentAssignment currShpAsgnmtInit = (ShipmentAssignment)pendingAssignments.GetByIndex(i);

                    System.Console.WriteLine("*INFO*:: Processing Notification = " + shpAsgnNotId);
                    
                    //skip Pending shipment assignment notifications, these will be used in the inside loop to decide 
                    //to skip init message - do not try sending Add/Delete message if there is another notification in pending state

                    if (currShpAsgnmtInit.NotificationStatus == (int)ShipmentAssignment.NOTIFICATION_STATUS.INIT)
                    {
                        for (int j = 0; j < pendingAssignments.Count; j++)
                        {
                            if(j == i)
                            {   
                                /*skip to the next one if it is the same record*/
                                continue;
                            }

                            ShipmentAssignment currShpAsgnmtPending = (ShipmentAssignment)pendingAssignments.GetByIndex(j);
                            if (   (currShpAsgnmtInit.ShipmentAssignmentId == currShpAsgnmtPending.ShipmentAssignmentId)
                                && (currShpAsgnmtPending.NotificationStatus == (int)ShipmentAssignment.NOTIFICATION_STATUS.PENDING)
                                )
                            {
                                /*we do not want to send another notification for a Assignemnt with a pending notification*/
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
                                /* if we found another notification in the init state for the same assignment, it
                                /* should be for the opposite action Add vs. delete. Skip both */

                                /*cross check that they are of the opposite kind and Assignment is not active */
                                if( (currShpAsgnmtInit.MessageType == (int) ShipmentAssignment.MESSAGE_TYPE.ADD && currShpAsgnmtPending.MessageType == (int)ShipmentAssignment.MESSAGE_TYPE.DELETE)
                                    || (currShpAsgnmtInit.MessageType == (int) ShipmentAssignment.MESSAGE_TYPE.DELETE && currShpAsgnmtPending.MessageType == (int)ShipmentAssignment.MESSAGE_TYPE.ADD) 
                                    )
                                {
                                    if( ! currShpAsgnmtInit.isActive )
                                    {
                                        /*two init notifications found for the same assignment id*/
                                        /*skip sending assignment message as the assignment is not active*/
                                        System.Console.WriteLine("Skipping ShipmentAssignmentNotificationID = " + shpAsgnNotId );
                                        System.Console.WriteLine("Skipping ShipmentAssignmentNotificationID = " + (long)pendingAssignments.GetKey(j));

                                        currShpAsgnmtInit.NotificationStatus = (int)ShipmentAssignment.NOTIFICATION_STATUS.SKIPPED;
                                        notfnForStatusUpdate.Add(shpAsgnNotId, currShpAsgnmtInit);

                                        currShpAsgnmtPending.NotificationStatus = (int)ShipmentAssignment.NOTIFICATION_STATUS.SKIPPED;
                                        notfnForStatusUpdate.Add((long)pendingAssignments.GetKey(j), currShpAsgnmtPending);

                                        /*move to the next notification */
                                        break;
                                    }
                                    else /*we should never be here */
                                    {
                                        /*TODO:: How to recover from this condition*/
                                        String Err = "*ERROR*:: Invalid state of ShipmentAssignmentID = " + currShpAsgnmtInit.ShipmentAssignmentId;
                                        Err += " with Notification ID's = " +  shpAsgnNotId + ", " + (long)pendingAssignments.GetKey(j);
                                        System.Console.WriteLine(Err);
                                    }
                                }
                            }
                        }
                    }

                }

                /*Collection to save notifications that need to send messages */
                /*saves notification id and the message type*/
                SortedList notfnForMessage = new SortedList();

                /*Now, whatever is in INIT state, needs to be sent and marked as PENDING */
                for (int i = 0; i < pendingAssignments.Count; i++)
                {
                    long shpAsgnNotId = (long)pendingAssignments.GetKey(i);
                    ShipmentAssignment currShpAsgnmtInit = (ShipmentAssignment)pendingAssignments.GetByIndex(i);

                    if (currShpAsgnmtInit.NotificationStatus == (int)ShipmentAssignment.NOTIFICATION_STATUS.INIT)
                    {

                        System.Console.WriteLine("*INFO*:: Second pass - Processing Notification = " + shpAsgnNotId);
                        if (currShpAsgnmtInit.MessageType == (int)ShipmentAssignment.MESSAGE_TYPE.ADD)
                        {
                            /*make sure that the ADD message has an active Assignment */
                            if (currShpAsgnmtInit.isActive)
                            {
                                System.Console.WriteLine("Scheduling Add message for ShipmentAssignmentNotificationID = " + shpAsgnNotId);

                                currShpAsgnmtInit.NotificationStatus = (int)ShipmentAssignment.NOTIFICATION_STATUS.PENDING;
                                notfnForMessage.Add((long)pendingAssignments.GetKey(i), currShpAsgnmtInit);
                            }
                            else
                            {
                                /*TODO:: How to recover from this condition*/
                                String Err = "*ERROR*:: Invalid Message_type of ShipmentAssignmentNotificationID = " + shpAsgnNotId;
                                Err += " with Assignment ID = " + (long)currShpAsgnmtInit.ShipmentAssignmentId;
                                Err += "\nADD notification Message_Type should have an active Assignment";
                                System.Console.WriteLine(Err);
                            }
                        }
                        else if (currShpAsgnmtInit.MessageType == (int)ShipmentAssignment.MESSAGE_TYPE.DELETE)
                        {
                            /*make sure that the DELETE message does not have an active Assignment */
                            if (!currShpAsgnmtInit.isActive)
                            {
                                System.Console.WriteLine("Scheduling Delete message for ShipmentAssignmentNotificationID = " + shpAsgnNotId);

                                currShpAsgnmtInit.NotificationStatus = (int)ShipmentAssignment.NOTIFICATION_STATUS.PENDING;
                                notfnForMessage.Add((long)pendingAssignments.GetKey(i), currShpAsgnmtInit);
                            }
                            else
                            {
                                /*TODO:: How to recover from this condition*/
                                String Err = "*ERROR*:: Invalid Message_type of ShipmentAssignmentNotificationID = " + shpAsgnNotId;
                                Err += " with Assignment ID = " + (long)currShpAsgnmtInit.ShipmentAssignmentId;
                                Err += "\nDELETE notification Message_Type should not have an active Assignment";
                                System.Console.WriteLine(Err);
                            }
                        }
                        else if (currShpAsgnmtInit.MessageType == (int)ShipmentAssignment.MESSAGE_TYPE.UPDATE)
                        {
                            /*TODO:: Need to handle code fo update message*/
                            System.Console.WriteLine ("*ERROR*:: Unhandled Message_type of ShipmentAssignmentNotificationID = " + shpAsgnNotId);

                            currShpAsgnmtInit.NotificationStatus = (int)ShipmentAssignment.NOTIFICATION_STATUS.PENDING;
                            notfnForMessage.Add((long)pendingAssignments.GetKey(i), currShpAsgnmtInit);
                        }
                    }
                }

                #endregion

                ProcessNotifications(conn, notfnForStatusUpdate, notfnForMessage);

                /* Call Messaging module for sending actual messages */

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
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            return bRetVal;

        }

        private bool ProcessNotifications(SqlConnection conn, SortedList notfnForStatusUpdate, SortedList notfnForMessage)
        {
            bool bRetValue = true;

            PODUtils.Utils.LogCollection(notfnForStatusUpdate);
            PODUtils.Utils.LogCollection(notfnForMessage);

            /*SQlCommand object for updating the status of ShipmentAssignmentNotification(s)*/
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

            /*SQlCommand object for scheduling messages ShipmentAssignmentNotification(s)*/
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
                /**/
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

            return bRetValue;
        }

        private bool SendMessages(SqlConnection conn)
        {

            bool bRetValue = true;

            /*SQlCommand object for getting the messages to be sent*/
            SqlCommand cmdGetMsgs = conn.CreateCommand();
            cmdStatusUpd.CommandType = System.Data.CommandType.StoredProcedure;
            cmdStatusUpd.CommandText = "spGetMessagesToBeSent";
            SqlParameter paramErrors = new SqlParameter("@Errors", DbType.String);
            paramErrors.Direction = System.Data.ParameterDirection.Output;
            cmdStatusUpd.Parameters.Add(paramErrors);


            return bRetValue;
        }
        

    }
}
