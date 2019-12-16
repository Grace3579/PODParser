using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using PODBusinessObjects;
using Permissions;
using System.Data;

//using PODMessaging;

namespace PODManager
{
    public class PODManagerDBHelper
    {
        public static DataTable getdropdownList(SqlCommand cmd)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            SqlDataReader dr = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.Add("TYPE_ID", Type.GetType("System.Int32"));
            dt.Columns.Add("SHORT_NAME", Type.GetType("System.String"));

            try
            {
                while (dr.Read())
                {
                    DataRow drow = dt.NewRow();
                    drow["TYPE_ID"] = Convert.ToInt32(dr["TYPE_ID"]);
                    drow["SHORT_NAME"] = dr["SHORT_NAME"].ToString();
                    dt.Rows.Add(drow);
                    //result.Add(Convert.ToInt32(dr["TYPE_ID"]), dr["SHORT_NAME"].ToString());
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
            finally
            {
                dr.Close();
            }
            return dt;
        }

        public static DataTable getdropdownListByListType(SqlCommand cmd)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            SqlDataReader dr = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.Add("DATA_SOURCE_ID", Type.GetType("System.Int32"));
            dt.Columns.Add("NAME", Type.GetType("System.String"));

            try
            {
                while (dr.Read())
                {
                    DataRow drow = dt.NewRow();
                    drow["DATA_SOURCE_ID"] = Convert.ToInt32(dr["DATA_SOURCE_ID"]);
                    drow["NAME"] = dr["NAME"].ToString();
                    dt.Rows.Add(drow);
                    //result.Add(Convert.ToInt32(dr["TYPE_ID"]), dr["SHORT_NAME"].ToString());
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
            finally
            {
                dr.Close();
            }
            return dt;
        }

        public static DataTable getDropdownListForShipment(SqlCommand cmd)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            SqlDataReader dr = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.Add("USER_PREFERENCE_ID", Type.GetType("System.String"));
            dt.Columns.Add("HEADER", Type.GetType("System.String"));

            try
            {
                while (dr.Read())
                {
                    DataRow drow = dt.NewRow();
                    drow["USER_PREFERENCE_ID"] = dr["USER_PREFERENCE_ID"].ToString();
                    drow["HEADER"] = dr["HEADER"].ToString();
                    dt.Rows.Add(drow);
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
            finally
            {
                dr.Close();
            }
            return dt;
        }

        public static List<RenderingInfo> getUserPreferenceForShipmentsFromCmd(SqlCommand cmd)
        {
            List<RenderingInfo> RenderingInfoList = new List<RenderingInfo>();
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    RenderingInfo currPreference = getuserPreferenceForShipmentFromDR(dr);
                    RenderingInfoList.Add(currPreference);
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
            finally
            {
                dr.Close();
            }
            return RenderingInfoList;
        }

        public static RenderingInfo getuserPreferenceForShipmentFromDR(SqlDataReader dr)
        {

            RenderingInfo currPreference = new RenderingInfo();
            currPreference.ColumnOrder = Convert.ToInt32(dr["COLUMN_ORDER"]);
            currPreference.ColumnName = Convert.ToString(dr["COLUMN_NAME"]);
            currPreference.Header = Convert.ToString(dr["HEADER"]);
            currPreference.Visible = Convert.ToChar(dr["VISIBLE"]);
            currPreference.Type = Convert.ToString(dr["TYPE"]);
            FormatInfo _objFormatInfo = new FormatInfo();
            _objFormatInfo.Width = Convert.ToInt32(dr["WIDTH"]);
            _objFormatInfo.Justify = Convert.ToString(dr["JUSTIFY"]);
            currPreference.Format = _objFormatInfo;

            return currPreference;
        }

        public static Dictionary<long, Shipment> getShipmentsFromCmd(SqlCommand cmd)
        {
            Dictionary<long, Shipment> shipments = new Dictionary<long, Shipment>();
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    Shipment currShipment = getShipmentFromDR(dr);
                    if (!shipments.ContainsKey(currShipment.ShipmentId))
                    {
                        shipments.Add(currShipment.ShipmentId, currShipment);
                    }
                    else
                    {
                        System.Console.WriteLine("Duplicate row with ShipmentID = " + currShipment.ShipmentId);
                    }
                }

                #region DELIVER_TO_ADDRESS
                dr.NextResult(); // DELIVER_TO_ADDRESS
                Dictionary<long, Address> deliverToAddresses = new Dictionary<long, Address>();
                while (dr.Read())
                {
                    Address currAddress = getAddressFromDR(dr);
                    if (!deliverToAddresses.ContainsKey(currAddress.AddressID))
                    {
                        deliverToAddresses.Add(currAddress.AddressID, currAddress);
                    }
                    else
                    {
                        System.Console.WriteLine("Duplicate row with AddressID = " + currAddress.AddressID);
                    }
                }
                #endregion DELIVER_TO_ADDRESS

                #region Shipment versions
                dr.NextResult(); // Shipment versions
                Dictionary<long, ShipmentVersion> shipmentVersions = new Dictionary<long, ShipmentVersion>();
                while (dr.Read())
                {
                    ShipmentVersion currShpVer = getShipmentVersionFromDR(dr);

                    // add the address 
                    if (deliverToAddresses.ContainsKey(Convert.ToInt64(dr["DELIVERY_ADDRESS_ID"])))
                    {
                        currShpVer.DeliverTo = deliverToAddresses[Convert.ToInt64(dr["DELIVERY_ADDRESS_ID"])];
                    }
                    else
                    {
                        Console.WriteLine("Shipment_Version_ID = " + currShpVer.ShipmentVersionID
                            + " has an invalid DELIVERY_ADDRESS_ID = " + Convert.ToInt64(dr["DELIVERY_ADDRESS_ID"]));
                    }

                    // add the ShipmentVersion to the collection
                    shipmentVersions.Add(currShpVer.ShipmentVersionID, currShpVer);
                }
                #endregion Shipment versions

                #region ShipmentAssignment.Contact
                dr.NextResult(); //SHIPMENT_ASSIGNMENT.Contact_ID's contacts
                Dictionary<long, Contact> shipmentAssigmentContacts = new Dictionary<long, Contact>();
                while (dr.Read())
                {
                    Contact currContact = getContactFromDR(dr);
                    if (!shipmentAssigmentContacts.ContainsKey(currContact.ContactId))
                    {
                        shipmentAssigmentContacts.Add(currContact.ContactId, currContact);
                    }
                    else
                    {
                        System.Console.WriteLine("Duplicate row with ContactID = " + currContact.ContactId);
                    }
                }

                #endregion ShipmentAssignment.Contact

                #region ShipmentAssignments

                dr.NextResult(); //SHIPMENT_ASSIGNMENT
                Dictionary<long, ShipmentAssignment> shipmentAssigments = new Dictionary<long, ShipmentAssignment>();
                while (dr.Read())
                {
                    ShipmentAssignment currShpAssgnmt = getShipmentAssignmentFromDR(dr);
                    if (!shipmentAssigments.ContainsKey(currShpAssgnmt.ShipmentAssignmentId))
                    {
                        shipmentAssigments.Add(currShpAssgnmt.ShipmentAssignmentId, currShpAssgnmt);
                    }
                    else
                    {
                        System.Console.WriteLine("Duplicate row with ShipmentAssignmentID = " + currShpAssgnmt.ShipmentAssignmentId);
                    }

                    // update the Assignment Contact
                    if (shipmentAssigmentContacts.ContainsKey(Convert.ToInt64(dr["CONTACT_ID"])))
                    {
                        currShpAssgnmt.AssignedTo = shipmentAssigmentContacts[Convert.ToInt64(dr["CONTACT_ID"])];
                    }
                    else
                    {
                        Console.WriteLine("SHIPMENT_ASSIGNMENT_ID = " + currShpAssgnmt.ShipmentAssignmentId
                            + " has an invalid CONTACT_ID = " + Convert.ToInt64(dr["CONTACT_ID"]));
                    }

                    // add this ShipmentAssignment to the related ShipmentVersion
                    if (shipmentVersions.ContainsKey(currShpAssgnmt.ShipmentVersionId))
                    {
                        shipmentVersions[currShpAssgnmt.ShipmentVersionId].AddAssignment(currShpAssgnmt);
                    }
                    else
                    {
                        Console.WriteLine("Shipment_Assignment_ID = " + currShpAssgnmt.ShipmentAssignmentId
                            + " has an invalid SHIPMENT_VERSION_ID = " + currShpAssgnmt.ShipmentVersionId);
                    }
                }
                #endregion ShipmentAssignments

                // add all ShipmentVersions to the corresponding Shipments
                Dictionary<long, ShipmentVersion>.Enumerator ShpVrnsEnum = shipmentVersions.GetEnumerator();
                while (ShpVrnsEnum.MoveNext())
                {
                    if (shipments.ContainsKey(ShpVrnsEnum.Current.Value.ShipmentID))
                    {
                        shipments[ShpVrnsEnum.Current.Value.ShipmentID].AddVersion(ShpVrnsEnum.Current.Value);
                    }
                    else
                    {
                        Console.WriteLine("Shipment_Version_ID = " + ShpVrnsEnum.Current.Value.ShipmentVersionID
                            + " has an invalid SHIPMENT_ID = " + ShpVrnsEnum.Current.Value.ShipmentID);
                    }
                }

                dr.NextResult(); // Shipment Permissions
                while (dr.Read())
                {
                    ShipmentPermission currShpPermission = getShipmentPermissiondFromDR(dr);
                    // add this ShipmentPermission to the Shipment
                    if (shipments.ContainsKey(currShpPermission.ShipmentID))
                    {
                        shipments[currShpPermission.ShipmentID].Permissions = currShpPermission;
                    }
                    else
                    {
                        Console.WriteLine("Permission with Shipment_ID = " + currShpPermission.ShipmentID
                            + ", Client_ID = " + currShpPermission.ClientID + " has an invalid SHIPMENT_ID.");
                    }
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
            finally
            {
                dr.Close();
            }

            return shipments;
        }
        
        public static Dictionary<long, Contact> getContactsFromCmd(SqlCommand cmd)
        {
            Dictionary<long, Contact> contacts = new Dictionary<long, Contact>();
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                while (dr.Read()) /*first result set is contacts*/
                {
                    Contact currContact = getContactFromDR(dr);
                    if (!contacts.ContainsKey(currContact.ContactId))
                    {
                        contacts.Add(currContact.ContactId, currContact);
                    }
                    else
                    {
                        System.Console.WriteLine("Duplicate row with ShipmentID = " + currContact.ContactId);
                    }
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
            finally
            {
                dr.Close();
            }

            return contacts;
        }

        public static List<string> validateUserFromCmd(SqlCommand cmd)
        {
            List<string> user = new List<string>();
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                if (dr.Read())
                {
                    user.Add(dr["CLIENT_ID"].ToString());
                    user.Add(dr["APP_USER_ID"].ToString());
                    user.Add(dr["USER_CLIENT_NAME"].ToString());
                    user.Add(dr["APP_USER_DESCRIPTION"].ToString());
                    user.Add(dr["CLIENT_DESCRIPTION"].ToString());
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
            finally
            {
                dr.Close();
            }

            return user;
        }


        public static Shipment getShipmentFromDR(SqlDataReader dr)
        {
            Console.WriteLine("SHIPMENT_ID = " + dr["SHIPMENT_ID"]
                            + ", SHIPMENT_SRC_REF = " + dr["SHIPMENT_SRC_REF"]
                            + ", SHIPMENT_REF = " + dr["SHIPMENT_REF"]);

            Shipment currShipment = new Shipment(Convert.ToInt64(dr["CLIENT_ID"]), Convert.ToInt64(dr["SHIPMENT_ID"]));
            currShipment.ShipmentSrc = Convert.ToString(dr["SHIPMENT_SRC_REF"]);
            currShipment.ShipmentRef = Convert.ToString(dr["SHIPMENT_REF"]);
            currShipment.NumOfPieces = dr.GetInt16(dr.GetOrdinal("NUM_OF_PIECES"));
            if (!dr.IsDBNull(dr.GetOrdinal("ADDITIONAL_REF")))
                currShipment.AddtionalRef = dr.GetString(dr.GetOrdinal("ADDITIONAL_REF"));
            if (!dr.IsDBNull(dr.GetOrdinal("PARENT_REF")))
                currShipment.ParentRef = dr.GetString(dr.GetOrdinal("PARENT_REF"));
            if (!dr.IsDBNull(dr.GetOrdinal("ORIGIN_CODE")))
                currShipment.OriginCode = dr.GetString(dr.GetOrdinal("ORIGIN_CODE"));
            if (!dr.IsDBNull(dr.GetOrdinal("DESTINATION_CODE")))
                currShipment.DestinationCode = dr.GetString(dr.GetOrdinal("DESTINATION_CODE"));

            return currShipment;
        }

        public static Address getAddressFromDR(SqlDataReader dr)
        {
            Console.WriteLine("ADDRESS_ID = " + dr["ADDRESS_ID"]
                            + ", ADDRESS_TYPE_ID = " + dr["ADDRESS_TYPE_ID"]
                            + ", CLIENT_ID = " + dr["CLIENT_ID"]
                            + ", ADDRESS_LINE_1 = " + dr["ADDRESS_LINE_1"]
                            + ", ADDRESS_LINE_2 = " + dr["ADDRESS_LINE_2"]
                            + ", ADDRESS_LINE_3 = " + dr["ADDRESS_LINE_3"]
                            + ", CITY = " + dr["CITY"]
                            + ", STATE = " + dr["STATE"]
                            + ", POSTAL_CODE = " + dr["POSTAL_CODE"]
                            + ", TIMESTAMP = " + BitConverter.ToString((byte[])dr["TIMESTAMP"], 0)
                            );


            Address currAddress = new Address(Convert.ToInt64(dr["ADDRESS_ID"]), Convert.ToInt64(dr["CLIENT_ID"]), Convert.ToInt64(dr["ADDRESS_TYPE_ID"]), (byte[])dr["TIMESTAMP"]);
            currAddress.AddressLine1 = Convert.ToString(dr["ADDRESS_LINE_1"]);
            currAddress.AddressLine2 = Convert.ToString(dr["ADDRESS_LINE_2"]);
            currAddress.AddressLine3 = Convert.ToString(dr["ADDRESS_LINE_3"]);
            currAddress.AddressLine4 = Convert.ToString(dr["ADDRESS_LINE_4"]);
            currAddress.City = Convert.ToString(dr["CITY"]);
            currAddress.State = Convert.ToString(dr["STATE"]);
            currAddress.PostalCode = Convert.ToString(dr["POSTAL_CODE"]);
            currAddress.Country = Convert.ToString(dr["COUNTRY"]);
            currAddress.EffectiveFrom = Convert.ToDateTime(dr["EFFECTIVE_FROM"]);
            currAddress.EffectiveTo = Convert.ToDateTime(dr["EFFECTIVE_TO"]);
            currAddress.IsPrimary = Convert.ToBoolean(dr["IS_PRIMARY"]);
            currAddress.CreatedTime = Convert.ToDateTime(dr["CREATED_TIME"]);
            currAddress.CreatingUser = Convert.ToString(dr["CREATING_USER_CODE"]);
            if (!dr.IsDBNull(dr.GetOrdinal("UPDATED_TIME")))
                currAddress.UpdatedTime = Convert.ToDateTime(dr["UPDATED_TIME"]);
            if (!dr.IsDBNull(dr.GetOrdinal("UPDATING_USER_CODE")))
                currAddress.UpdatingUser = Convert.ToString(dr["UPDATING_USER_CODE"]);

            return currAddress;
        }

        public static ShipmentVersion getShipmentVersionFromDR(SqlDataReader dr)
        {
            Console.WriteLine("SHIPMENT_VERSION_ID = " + dr["SHIPMENT_VERSION_ID"]
                            + ", SHIPMENT_ID = " + dr["SHIPMENT_ID"]
                            + ", VERSION_NUM = " + dr["VERSION_NUM"]
                            + ", STATUS_ID = " + dr["STATUS_ID"]
                            + ", CONSIGNEE_NAME = " + dr["CONSIGNEE_NAME"]
                            + ", RECEIVER_NAME = " + dr["RECEIVER_NAME"]
                            + ", RECEIVER_PHONE = " + dr["RECEIVER_PHONE"]
                            + ", DELIVERY_ADDRESS_ID = " + dr["DELIVERY_ADDRESS_ID"]
                            + ", WEIGHT = " + dr["WEIGHT"]
                            + ", CREATED_TIME = " + dr["CREATED_TIME"]
                            + ", CREATING_USER_CODE = " + dr["CREATING_USER_CODE"]
                            + ", UPDATED_TIME = " + dr["UPDATED_TIME"]
                            + ", UPDATING_USER_CODE = " + dr["UPDATING_USER_CODE"]
                            + ", TIMESTAMP = " + BitConverter.ToString((byte[])dr["TIMESTAMP"], 0)
                            );

            ShipmentVersion currShpVer = new ShipmentVersion(Convert.ToInt64(dr["SHIPMENT_VERSION_ID"]), Convert.ToInt64(dr["SHIPMENT_ID"]));
            currShpVer.VersionNum = dr.GetInt16(dr.GetOrdinal("VERSION_NUM"));
            currShpVer.Status = (ShipmentVersion.SHIPMENT_STATUS) Convert.ToInt32( dr["STATUS_ID"]);
            if (!dr.IsDBNull(dr.GetOrdinal("CONSIGNEE_NAME")))
                currShpVer.ConsigneeName = dr.GetString(dr.GetOrdinal("CONSIGNEE_NAME"));
            if (!dr.IsDBNull(dr.GetOrdinal("RECEIVER_NAME")))
                currShpVer.ReceiverName = dr.GetString(dr.GetOrdinal("RECEIVER_NAME"));
            if (!dr.IsDBNull(dr.GetOrdinal("RECEIVER_PHONE")))
                currShpVer.ReceiverPhone = dr.GetString(dr.GetOrdinal("RECEIVER_PHONE"));
            if (!dr.IsDBNull(dr.GetOrdinal("WEIGHT")))
                currShpVer.Weight = dr.GetString(dr.GetOrdinal("WEIGHT"));
            currShpVer.CreatedTime = Convert.ToDateTime(dr["CREATED_TIME"]);
            currShpVer.CreatingUser = Convert.ToString(dr["CREATING_USER_CODE"]);
            if (!dr.IsDBNull(dr.GetOrdinal("UPDATED_TIME")))
                currShpVer.UpdatedTime = Convert.ToDateTime(dr["UPDATED_TIME"]);
            if (!dr.IsDBNull(dr.GetOrdinal("UPDATING_USER_CODE")))
                currShpVer.UpdatingUser = Convert.ToString(dr["UPDATING_USER_CODE"]);

            currShpVer.TimeStamp = (byte[])dr["TIMESTAMP"];

            return currShpVer;
        }

        public static ShipmentPermission getShipmentPermissiondFromDR(SqlDataReader dr)
        {
            Console.WriteLine("CLIENT_ID = " + dr["CLIENT_ID"]
                            + ", SHIPMENT_ID = " + dr["SHIPMENT_ID"]
                            + ", CAN_ASSIGN = " + dr["CAN_ASSIGN"]
                            + ", CAN_UNASSIGN = " + dr["CAN_UNASSIGN"]
                            + ", CAN_EDIT = " + dr["CAN_EDIT"]
                            + ", CAN_CANCEL = " + dr["CAN_CANCEL"]
                            );

            ShipmentPermission currShpPermission = new ShipmentPermission(Convert.ToInt64(dr["CLIENT_ID"]), Convert.ToInt64(dr["SHIPMENT_ID"]));
            currShpPermission.CanAssign = Convert.ToBoolean(dr["CAN_ASSIGN"]);
            currShpPermission.CanUnassign = Convert.ToBoolean(dr["CAN_UNASSIGN"]);
            currShpPermission.CanEdit = Convert.ToBoolean(dr["CAN_EDIT"]);
            currShpPermission.CanCancel = Convert.ToBoolean(dr["CAN_CANCEL"]);

            return currShpPermission;
        }

        public static ShipmentAssignment getShipmentAssignmentFromDR(SqlDataReader dr)
        {
            // this one gets the Shipment Assignments only - no Contact ID is saved! (is this really useful?!)
            // there should be no row where Contact_ID is null
            // see the overloaded funtion which takes in an addtional collection of Contacts, to correctly add the contact information 

            Console.WriteLine("SHIPMENT_ASSIGNMENT_ID = " + dr["SHIPMENT_ASSIGNMENT_ID"]
                            + ", SHIPMENT_VERSION_ID = " + dr["SHIPMENT_VERSION_ID"]
                            + ", CONTACT_ID = " + dr["CONTACT_ID"]
                            + ", NOTIFICATION_STATUS_ID = " + dr["NOTIFICATION_STATUS_ID"]
                            + ", MESSAGE_TYPE_ID = " + dr["MESSAGE_TYPE_ID"]
                            + ", CREATED_TIME = " + dr["CREATED_TIME"]
                            + ", CREATING_USER_CODE = " + dr["CREATING_USER_CODE"]
                            + ", UPDATED_TIME = " + dr["UPDATED_TIME"]
                            + ", UPDATING_USER_CODE = " + dr["UPDATING_USER_CODE"]
                            + ", IS_ACTIVE = " + dr["IS_ACTIVE"]
                            + ", TIMESTAMP = " + BitConverter.ToString((byte[])dr["TIMESTAMP"], 0)
                            );

            ShipmentAssignment currShpAsgnmnt = new ShipmentAssignment(Convert.ToInt64(dr["SHIPMENT_ASSIGNMENT_ID"]), Convert.ToInt64(dr["SHIPMENT_VERSION_ID"]), Convert.ToBoolean(dr["IS_ACTIVE"]));

            // Assignment Status
            //Notification.NOTIFICATION_STATUS notfnStatus = Notification.NOTIFICATION_STATUS.UNKN ;
            //if (!dr.IsDBNull(dr.GetOrdinal("NOTIFICATION_STATUS_ID")))
            //    notfnStatus = (Notification.NOTIFICATION_STATUS)Convert.ToInt64(dr["NOTIFICATION_STATUS_ID"]);
            //Message.MESSAGE_TYPE messageType = Message.MESSAGE_TYPE.UNKNOWN;
            //if (!dr.IsDBNull(dr.GetOrdinal("MESSAGE_TYPE_ID")))
            //    messageType = (Message.MESSAGE_TYPE)Convert.ToInt64(dr["MESSAGE_TYPE_ID"]);
            //if (notfnStatus != Notification.NOTIFICATION_STATUS.UNKN)
            //    setAssignmentStatus(currShpAsgnmnt, notfnStatus, messageType);

            currShpAsgnmnt.CreatedTime = Convert.ToDateTime(dr["CREATED_TIME"]);
            currShpAsgnmnt.CreatingUser = Convert.ToString(dr["CREATING_USER_CODE"]);
            if (!dr.IsDBNull(dr.GetOrdinal("UPDATED_TIME")))
                currShpAsgnmnt.UpdatedTime = Convert.ToDateTime(dr["UPDATED_TIME"]);
            if (!dr.IsDBNull(dr.GetOrdinal("UPDATING_USER_CODE")))
                currShpAsgnmnt.UpdatingUser = Convert.ToString(dr["UPDATING_USER_CODE"]);

            currShpAsgnmnt.TimeStamp = (byte[])dr["TIMESTAMP"];

            return currShpAsgnmnt;
        }

        public static ShipmentAssignment getShipmentAssignmentFromDR(SqlDataReader dr, Dictionary<long, Contact> assignedToList)
        {
            ShipmentAssignment currShpAssgnMnt = new ShipmentAssignment(Convert.ToInt64(dr["SHIPMENT_ASSIGNMENT_ID"]), Convert.ToInt64(dr["SHIPMENT_VERSION_ID"]), Convert.ToBoolean(dr["IS_ACTIVE"]));
            currShpAssgnMnt.AssignedTo = assignedToList[Convert.ToInt64(dr["CONTACT_ID"])];
            currShpAssgnMnt.CreatedTime = Convert.ToDateTime(dr["CREATED_TIME"]);
            currShpAssgnMnt.CreatingUser = Convert.ToString(dr["CREATING_USER_CODE"]);
            if (!dr.IsDBNull(dr.GetOrdinal("UPDATED_TIME")))
                currShpAssgnMnt.UpdatedTime = Convert.ToDateTime(dr["UPDATED_TIME"]);
            if (!dr.IsDBNull(dr.GetOrdinal("UPDATING_USER_CODE")))
                currShpAssgnMnt.UpdatingUser = Convert.ToString(dr["UPDATING_USER_CODE"]);
            currShpAssgnMnt.TimeStamp = (byte[])dr["TIMESTAMP"];

            return currShpAssgnMnt;
        }

        //Commented on Feb 24 2013 to run build project at my end as PODMessaging doesnt exists with me
       // private static void setAssignmentStatus(ShipmentAssignment shpAsgnmnt, Notification.NOTIFICATION_STATUS notfnStatus, Message.MESSAGE_TYPE msgType)
        private static void setAssignmentStatus(ShipmentAssignment shpAsgnmnt, int a, int b)
        {
            //if (msgType == Message.MESSAGE_TYPE.ADD || msgType == Message.MESSAGE_TYPE.UPDATE)
            //{

            //}
            //else if (msgType == Message.MESSAGE_TYPE.DELETE)
            //{

            //}

        }

        public static Contact getContactFromDR(SqlDataReader dr)
        {
            Console.WriteLine("CLIENT_ID = " + dr["CLIENT_ID"]
                            + ", CONTACT_ID = " + dr["CONTACT_ID"]
                            + ", CONTACT_TYPE_ID = " + dr["CONTACT_TYPE_ID"]
                            + ", PREFIX = " + dr["PREFIX"]
                            + ", FIRST_NAME = " + dr["FIRST_NAME"]
                            + ", MIDDLE_INITIAL = " + dr["MIDDLE_INITIAL"]
                            + ", LAST_NAME = " + dr["LAST_NAME"]
                            + ", FULL_NAME = " + dr["FULL_NAME"]
                            + ", PHONE_NUMBER = " + dr["PHONE_NUMBER"]
                            + ", CONTACT_CODE = " + dr["CONTACT_CODE"]
                            + ", CREATED_TIME = " + dr["CREATED_TIME"]
                            + ", CREATING_USER_CODE = " + dr["CREATING_USER_CODE"]
                            + ", UPDATED_TIME = " + dr["UPDATED_TIME"]
                            + ", UPDATING_USER_CODE = " + dr["UPDATING_USER_CODE"]
                            + ", IS_ACTIVE = " + dr["IS_ACTIVE"]
                            + ", TIMESTAMP = " + BitConverter.ToString((byte[])dr["TIMESTAMP"], 0)
                            );

            Contact currContact = new Contact(Convert.ToInt64(dr["CLIENT_ID"]), Convert.ToInt64(dr["CONTACT_ID"])
                                             , (Contact.CONTACT_TYPE)Convert.ToInt64((dr["CONTACT_TYPE_ID"])));
            // TPD CONTACT_TYPE_ID, EMAIL_ADDRESS
            if (!dr.IsDBNull(dr.GetOrdinal("PREFIX")))
                currContact.Prefix = dr.GetString(dr.GetOrdinal("PREFIX"));
            if (!dr.IsDBNull(dr.GetOrdinal("FIRST_NAME")))
                currContact.FirstName = dr.GetString(dr.GetOrdinal("FIRST_NAME"));
            if (!dr.IsDBNull(dr.GetOrdinal("MIDDLE_INITIAL")))
                currContact.MiddleInitial = dr.GetString(dr.GetOrdinal("MIDDLE_INITIAL"));
            if (!dr.IsDBNull(dr.GetOrdinal("LAST_NAME")))
                currContact.LastName = dr.GetString(dr.GetOrdinal("LAST_NAME"));
            if (!dr.IsDBNull(dr.GetOrdinal("FULL_NAME")))
                currContact.FullName = dr.GetString(dr.GetOrdinal("FULL_NAME"));
            if (!dr.IsDBNull(dr.GetOrdinal("PHONE_NUMBER")))
                currContact.PhoneNumber = Convert.ToInt64(dr["PHONE_NUMBER"]);
            if (!dr.IsDBNull(dr.GetOrdinal("CONTACT_CODE")))
                currContact.ContactCode = dr.GetString(dr.GetOrdinal("CONTACT_CODE"));
            currContact.CreatedTime = Convert.ToDateTime(dr["CREATED_TIME"]);
            currContact.CreatingUser = Convert.ToString(dr["CREATING_USER_CODE"]);
            if (!dr.IsDBNull(dr.GetOrdinal("UPDATED_TIME")))
                currContact.UpdatedTime = Convert.ToDateTime(dr["UPDATED_TIME"]);
            if (!dr.IsDBNull(dr.GetOrdinal("UPDATING_USER_CODE")))
                currContact.UpdatingUser = Convert.ToString(dr["UPDATING_USER_CODE"]);
            currContact.Active = Convert.ToBoolean(dr["IS_ACTIVE"]);

            currContact.TimeStamp = (byte[])dr["TIMESTAMP"];

            return currContact;
        }

        public static bool SaveShipment(SqlConnection sqlConn, SqlTransaction tran, Shipment newShipment)
        {
            bool bRetVal = false;
            // add a new Shipment and it's associated object tree, to the DB
            StringBuilder strErrorBuilder = new StringBuilder();
            strErrorBuilder.AppendLine("Adding new Shipment.");
            strErrorBuilder.AppendLine("Adding a New Shipment with ShipmentSource = " + newShipment.ShipmentSrc + ", ShipmentRef = " + newShipment.ShipmentRef);

            try
            {
                SqlCommand cmd = sqlConn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Transaction = tran;
                cmd.CommandText = "spShipment_I";

                SqlParameter paramShipmentId = new SqlParameter("@SHIPMENT_ID", newShipment.ShipmentId);
                paramShipmentId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramShipmentId);
                SqlParameter paramClientId = new SqlParameter("@CLIENT_ID", newShipment.ClientId);
                paramClientId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramClientId);
                SqlParameter paramShipmentSrcRef = new SqlParameter("@SHIPMENT_SRC_REF", newShipment.ShipmentSrc);
                paramShipmentSrcRef.DbType = DbType.String;
                cmd.Parameters.Add(paramShipmentSrcRef);
                SqlParameter paramShipmentRef = new SqlParameter("@SHIPMENT_REF", newShipment.ShipmentRef);
                paramShipmentRef.DbType = DbType.String;
                cmd.Parameters.Add(paramShipmentRef);
                SqlParameter paramNumOfPieces = new SqlParameter("@NUM_OF_PIECES", newShipment.NumOfPieces);
                paramNumOfPieces.DbType = DbType.Int16;
                cmd.Parameters.Add(paramNumOfPieces);
                SqlParameter paramAddnlRef = new SqlParameter("@ADDITIONAL_REF", newShipment.AddtionalRef);
                paramAddnlRef.DbType = DbType.String;
                cmd.Parameters.Add(paramAddnlRef);
                SqlParameter paramParentRef = new SqlParameter("@PARENT_REF", newShipment.ParentRef);
                paramParentRef.DbType = DbType.String;
                cmd.Parameters.Add(paramParentRef);
                SqlParameter paramOriginCode = new SqlParameter("@ORIGIN_CODE", newShipment.OriginCode);
                paramOriginCode.DbType = DbType.String;
                cmd.Parameters.Add(paramOriginCode);
                SqlParameter paramDestinationCode = new SqlParameter("@DESTINATION_CODE", newShipment.DestinationCode);
                paramDestinationCode.DbType = DbType.String;
                cmd.Parameters.Add(paramDestinationCode);

                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.DbType = DbType.String;
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                paramRetVal.DbType = DbType.Int16;
                paramRetVal.Direction = System.Data.ParameterDirection.ReturnValue;
                cmd.Parameters.Add(paramRetVal);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read()) // only one result set with the new Shipment ID
                {
                    newShipment.ShipmentId = Convert.ToInt64(dr["SHIPMENT_ID"]);
                }
                dr.Close();

                strErrorBuilder.AppendLine("Errors = " + Convert.ToString(paramErrors.Value));

                int nRetValFromProc = Convert.ToInt16(paramRetVal.Value);

                if (nRetValFromProc == 0) // update succeeded
                    bRetVal = true;

                strErrorBuilder.AppendLine("Added new Shipment with ShipmentID = " + newShipment.ShipmentId);

                // Save the Address
                bRetVal = SaveAddress(sqlConn, tran, newShipment.getLatestShipmentVersion().DeliverTo);

                // set the ShipmentID on the ShipmentVersion and then save it to the DB (there should be only one)
               // newShipment.getLatestShipmentVersion().ShipmentID = newShipment.ShipmentId;
                bRetVal = SaveShipmentVersion(sqlConn, tran, newShipment.getLatestShipmentVersion());

                System.Console.WriteLine(strErrorBuilder.ToString());

            }
            catch (SqlException sqlExp)
            {
                System.Console.WriteLine("SQL Exception in SaveShipment(): " + sqlExp.ToString());
                throw sqlExp;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Generic Exception : in SaveShipment(): " + ex.ToString());
                throw ex;
            }

           return bRetVal;

        }

        public static bool SaveShipmentVersion(SqlConnection sqlConn, SqlTransaction tran, ShipmentVersion newShipmentVersion)
        {
            bool bRetVal = false;

            // add a new Shipment VErsion to the DB
            StringBuilder strErrorBuilder = new StringBuilder();
            strErrorBuilder.AppendLine("Adding new ShipmentVersion.");
            strErrorBuilder.AppendLine("Adding a New ShipmentVersion with ShipmentID = " + newShipmentVersion.ShipmentID);

            try
            {
                SqlCommand cmd = sqlConn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Transaction = tran;
                cmd.CommandText = "spShipmentVersion_I";

                SqlParameter paramShipmentVersionId = new SqlParameter("@SHIPMENT_VERSION_ID", newShipmentVersion.ShipmentVersionID);
                paramShipmentVersionId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramShipmentVersionId);
                SqlParameter paramShipmentId = new SqlParameter("@SHIPMENT_ID", newShipmentVersion.ShipmentID);
                paramShipmentId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramShipmentId);
                SqlParameter paramShipmentVrsnNum = new SqlParameter("@VERSION_NUM", newShipmentVersion.VersionNum);
                paramShipmentVrsnNum.DbType = DbType.Int16;
                cmd.Parameters.Add(paramShipmentVrsnNum);
                SqlParameter paramStatusId = new SqlParameter("@STATUS_ID", newShipmentVersion.Status);
                paramStatusId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramStatusId);
                SqlParameter paramConsigneeName = new SqlParameter("@CONSIGNEE_NAME", newShipmentVersion.ConsigneeName);
                paramConsigneeName.DbType = DbType.String;
                cmd.Parameters.Add(paramConsigneeName);
                SqlParameter paramReceiverName = new SqlParameter("@RECEIVER_NAME", newShipmentVersion.ReceiverName);
                paramReceiverName.DbType = DbType.String;
                cmd.Parameters.Add(paramReceiverName);
                SqlParameter paramReceiverPhone = new SqlParameter("@RECEIVER_PHONE", newShipmentVersion.ReceiverPhone);
                paramReceiverPhone.DbType = DbType.String;
                cmd.Parameters.Add(paramReceiverPhone);
                SqlParameter paramDeliverToAddressId = new SqlParameter("@DELIVERY_ADDRESS_ID", newShipmentVersion.DeliverTo.AddressID);
                paramDeliverToAddressId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramDeliverToAddressId);
                SqlParameter paramWeight = new SqlParameter("@WEIGHT", newShipmentVersion.Weight);
                paramWeight.DbType = DbType.String;
                cmd.Parameters.Add(paramWeight);
                SqlParameter paramCreatingUserCode = new SqlParameter("@CREATING_USER_CODE", newShipmentVersion.CreatingUser);
                paramCreatingUserCode.DbType = DbType.String;
                cmd.Parameters.Add(paramCreatingUserCode);

                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.DbType = DbType.String;
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                paramRetVal.DbType = DbType.Int16;
                paramRetVal.Direction = System.Data.ParameterDirection.ReturnValue;
                cmd.Parameters.Add(paramRetVal);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read()) // only one result set with the new Shipment Version ID and TimeStamp
                {
                    //newShipmentVersion.ShipmentVersionID = Convert.ToInt64(dr["SHIPMENT_VERSION_ID"]);
                    newShipmentVersion.TimeStamp = (byte[])dr["TIMESTAMP"];
                }
                dr.Close();

                strErrorBuilder.AppendLine("Errors = " + Convert.ToString(paramErrors.Value));

                int nRetValFromProc = Convert.ToInt16(paramRetVal.Value);

                if (nRetValFromProc == 0) // update succeeded
                    bRetVal = true;

                strErrorBuilder.AppendLine("Added new Shipment_Version with ShipmentVersionID = " + newShipmentVersion.ShipmentVersionID);

                System.Console.WriteLine(strErrorBuilder.ToString());
            }
            catch (SqlException sqlExp)
            {
                System.Console.WriteLine("SQL Exception in SaveShipmentVersion(): " + sqlExp.ToString());
                throw sqlExp;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Generic Exception : in SaveShipmentVersion(): " + ex.ToString());
                throw ex;
            }

           return bRetVal;

        }

        public static bool SaveAddress(SqlConnection sqlConn, SqlTransaction tran, Address newAddress)
        {
            bool bRetVal = false;

            // add a new Shipment VErsion to the DB
            StringBuilder strErrorBuilder = new StringBuilder();
            strErrorBuilder.AppendLine("Adding new Address.");
            strErrorBuilder.AppendLine("Adding a New Address with Address = " + newAddress.FullAddress);

            try
            {
                SqlCommand cmd = sqlConn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Transaction = tran;
                cmd.CommandText = "spAddress_I";

                SqlParameter paramAddressId = new SqlParameter("@ADDRESS_ID", newAddress.AddressID);
                paramAddressId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramAddressId);
                SqlParameter paramAddressTypeId = new SqlParameter("@ADDRESS_TYPE_ID", newAddress.AddressType);
                paramAddressTypeId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramAddressTypeId);
                SqlParameter paramClientId = new SqlParameter("@CLIENT_ID", newAddress.ClientID);
                paramClientId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramClientId);
                SqlParameter paramContactId = new SqlParameter("@CONTACT_ID", newAddress.ContactID);
                paramContactId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramContactId);
                SqlParameter paramAddressLine1 = new SqlParameter("@ADDRESS_LINE_1", newAddress.AddressLine1);
                paramAddressLine1.DbType = DbType.String;
                cmd.Parameters.Add(paramAddressLine1);
                SqlParameter paramAddressLine2 = new SqlParameter("@ADDRESS_LINE_2", newAddress.AddressLine2);
                paramAddressLine2.DbType = DbType.String;
                cmd.Parameters.Add(paramAddressLine2);
                SqlParameter paramAddressLine3 = new SqlParameter("@ADDRESS_LINE_3", newAddress.AddressLine3);
                paramAddressLine3.DbType = DbType.String;
                cmd.Parameters.Add(paramAddressLine3);
                SqlParameter paramAddressLine4 = new SqlParameter("@ADDRESS_LINE_4", newAddress.AddressLine4);
                paramAddressLine4.DbType = DbType.String;
                cmd.Parameters.Add(paramAddressLine4);
                SqlParameter paramCity = new SqlParameter("@CITY", newAddress.City);
                paramCity.DbType = DbType.String;
                cmd.Parameters.Add(paramCity);
                SqlParameter paramState = new SqlParameter("@STATE", newAddress.State);
                paramState.DbType = DbType.String;
                cmd.Parameters.Add(paramState);
                SqlParameter paramPostalCode = new SqlParameter("@POSTAL_CODE", newAddress.PostalCode);
                paramPostalCode.DbType = DbType.String;
                cmd.Parameters.Add(paramPostalCode);
                SqlParameter paramCountry = new SqlParameter("@COUNTRY", newAddress.Country);
                paramCountry.DbType = DbType.String;
                cmd.Parameters.Add(paramCountry);
                SqlParameter paramEffectiveFrom = new SqlParameter("@EFFECTIVE_FROM", newAddress.EffectiveFrom);
                paramEffectiveFrom.DbType = DbType.DateTime;
                cmd.Parameters.Add(paramEffectiveFrom);
                SqlParameter paramEffectiveTo = new SqlParameter("@EFFECTIVE_TO", newAddress.EffectiveTo);
                paramEffectiveTo.DbType = DbType.DateTime;
                cmd.Parameters.Add(paramEffectiveTo);
                SqlParameter paramCreatingUserCode = new SqlParameter("@CREATING_USER_CODE", newAddress.CreatingUser);
                paramCreatingUserCode.DbType = DbType.String;
                cmd.Parameters.Add(paramCreatingUserCode);
                SqlParameter paramIsPrimary = new SqlParameter("@IS_PRIMARY", newAddress.IsPrimary);
                paramIsPrimary.DbType = DbType.Byte;
                cmd.Parameters.Add(paramIsPrimary);

                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.DbType = DbType.String;
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                paramRetVal.DbType = DbType.Int16;
                paramRetVal.Direction = System.Data.ParameterDirection.ReturnValue;
                cmd.Parameters.Add(paramRetVal);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read()) // only one result set with the new Address ID and TimeStamp
                {
                    newAddress.AddressID = Convert.ToInt64(dr["ADDRESS_ID"]);
                    newAddress.TimeStamp = (byte[])dr["TIMESTAMP"];
                }
                dr.Close();

                strErrorBuilder.AppendLine("Errors = " + Convert.ToString(paramErrors.Value));

                int nRetValFromProc = Convert.ToInt16(paramRetVal.Value);

                if (nRetValFromProc == 0) // update succeeded
                    bRetVal = true;

                strErrorBuilder.AppendLine("Added new Address with AddressID = " + newAddress.AddressID);

                System.Console.WriteLine(strErrorBuilder.ToString());


            }
            catch (SqlException sqlExp)
            {
                System.Console.WriteLine("SQL Exception in SaveAddress(): " + sqlExp.ToString());
                throw sqlExp;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Generic Exception : in SaveAddress(): " + ex.ToString());
                throw ex;
            }

            return bRetVal;
        }

        public static bool addContact(SqlConnection sqlConn, SqlTransaction tran, Contact newContact)
        {
            // add a contact to the DB and get the newly added contact's contact ID
            // update the input Contact with the new Contact ID
            StringBuilder strErrorBuilder = new StringBuilder();
            strErrorBuilder.AppendLine("Adding new contact.");

            strErrorBuilder.AppendLine("Adding a New Contact with FullName = " + newContact.FullName +
                        ", PhoneNumber = " + newContact.PhoneNumber);
            // add a new contact to DB and get the contact ID
            // update the contact ID of the input employee
            bool bRetVal = false;
            try
            {
                SqlCommand cmd = sqlConn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Transaction = tran;

                cmd.CommandText = "spContact_I";
                SqlParameter paramContactId = new SqlParameter("@CONTACT_ID", -1 /*Dummy Value*/);
                paramContactId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramContactId);
                SqlParameter paramClientId = new SqlParameter("@CLIENT_ID", newContact.ClientId);
                paramClientId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramClientId);
                SqlParameter paramContactTypeId = new SqlParameter("@CONTACT_TYPE_ID", newContact.ContactTypeId);
                paramContactTypeId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramContactTypeId);
                SqlParameter paramFullName = new SqlParameter("@FULL_NAME", newContact.FullName);
                paramFullName.DbType = DbType.String;
                cmd.Parameters.Add(paramFullName);
                SqlParameter paramPhoneNum = new SqlParameter("@PHONE_NUMBER", newContact.PhoneNumber);
                paramPhoneNum.DbType = DbType.Int64;
                cmd.Parameters.Add(paramPhoneNum);
                SqlParameter paramCreatingUser = new SqlParameter("@CREATING_USER_CODE", newContact.CreatingUser);
                paramCreatingUser.DbType = DbType.String;
                cmd.Parameters.Add(paramCreatingUser);
                SqlParameter paramIsActive = new SqlParameter("@IS_ACTIVE", newContact.Active);
                paramIsActive.DbType = DbType.Boolean;
                cmd.Parameters.Add(paramIsActive);

                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.DbType = DbType.String;
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                paramRetVal.DbType = DbType.Int16;
                paramRetVal.Direction = System.Data.ParameterDirection.ReturnValue;
                cmd.Parameters.Add(paramRetVal);

                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.Read()) // only one result set with the new TimeStamp
                {
                    newContact.ContactId = Convert.ToInt64(dr["CONTACT_ID"]);
                    newContact.TimeStamp = (byte[])dr["TIMESTAMP"];
                }
                dr.Close();

                strErrorBuilder.AppendLine("Errors = " + Convert.ToString(paramErrors.Value));

                int nRetValFromProc = Convert.ToInt16(paramRetVal.Value);

                if (nRetValFromProc == 0) // update succeeded
                    bRetVal = true;


                strErrorBuilder.AppendLine("Added new Contact with ContactID = " + newContact.ContactId);

                System.Console.WriteLine(strErrorBuilder.ToString());

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

        public static bool updateContactPhoneNum(SqlConnection _dbConn, SqlTransaction tran, Contact contact)
        {
            // update a contact's Phone number
            StringBuilder strErrorBuilder = new StringBuilder();
            strErrorBuilder.AppendLine("Updating contact with Contact_ID = " + contact.ContactId + ", Phone Number =" + contact.PhoneNumber);
            bool bRetVal = false;
            try
            {
                SqlCommand cmd = _dbConn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Transaction = tran;

                cmd.CommandText = "spContactUpdPhone";
                SqlParameter paramContactId = new SqlParameter("@Contact_ID", contact.ContactId);
                paramContactId.DbType = DbType.Int64;
                cmd.Parameters.Add(paramContactId);
                SqlParameter paramPhoneNum = new SqlParameter("@NewPhoneNumber", contact.PhoneNumber);
                paramPhoneNum.DbType = DbType.Int64;
                cmd.Parameters.Add(paramPhoneNum);
                SqlParameter paramUserCode = new SqlParameter("@TimeStamp", SqlDbType.Timestamp);
                paramUserCode.Value = contact.TimeStamp;
                cmd.Parameters.Add(paramUserCode);
                SqlParameter paramErrors = new SqlParameter("@Errors", "");
                paramErrors.DbType = DbType.String;
                paramErrors.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(paramErrors);

                SqlParameter paramRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                paramRetVal.DbType = DbType.Int16;
                paramRetVal.Direction = System.Data.ParameterDirection.ReturnValue;
                cmd.Parameters.Add(paramRetVal);

                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.Read()) // only one result set with the new TimeStamp
                {
                    if (Convert.ToInt64(dr["CONTACT_ID"]) ==  contact.ContactId)
                    {
                         contact.TimeStamp = (byte[])dr["TIMESTAMP"];
                    }
                    else
                    {
                        // we should never be here
                        System.Console.WriteLine("Incorrect Contact updated = " + Convert.ToInt64(dr["CONTACT_ID"]) + ", instead of " + contact.ContactId);
                    }
                }
                dr.Close();

                strErrorBuilder.AppendLine("Errors = " + Convert.ToString(paramErrors.Value));

                int nRetValFromProc = Convert.ToInt16(paramRetVal.Value);

                if (nRetValFromProc == 0) // update failed
                    bRetVal = true;

                System.Console.WriteLine(strErrorBuilder.ToString());

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

    }
}
