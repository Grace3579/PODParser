using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using PODBusinessObjects;

namespace PODManager
{
    class ContactType
    {
        private long _contactTypeId;
        private String _shortName;
        private String _description;

        public long ContactTypeID
        {
            get
            {
                return _contactTypeId;
            }
        }
        public String ShortName
        {
            get
            {
                return _shortName;
            }
        }
        public String Description
        {
            get
            {
                return _description;
            }
        }

        ContactType(long ContactTypeId, String ShortName, String Description)
        {
            _contactTypeId = ContactTypeId;
            _shortName = ShortName;
            _description = Description;
        }
        
        
    }

    public class ContactManager
    {
        /*as per entries in CONTACT_TYPE table*/
        public enum CONTACT_TYPE { EMP = 101, CUST_IND, CUST_PVTCO, CUST_PVTCORP, CUST_PUBCORP };

        private static List<ContactType> _contactTypes = new List<ContactType>();


        public ContactManager()
        {

/*            if (_contactTypes.Count == 0)
            {


            }
*/
        }

        public SortedList<long, Contact> getEmployees(SqlConnection conn)
        {
            return getContactsByType(ContactManager.CONTACT_TYPE.EMP, conn);
        }

        public SortedList<long, Contact> getContactsByType(CONTACT_TYPE contactType, SqlConnection conn)
        {
            SortedList<long, Contact> contacts = new SortedList<long,Contact>();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
            //get the contacts
            cmd.CommandText = "spGetContactsByTypeId";
            SqlParameter paramContactType = new SqlParameter("@ContactTypeId", contactType);
 
            cmd.Parameters.Add(paramContactType);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
/*                Console.WriteLine(dr[0] + "-" + dr[1] + "-" + dr[2] + "-" + dr[3] 
                                + "-" + dr[4]);
                Contact currContact = new Contact(Convert.ToInt64(dr["CONTACT_ID"]), 1 );
                if(!DBNull.Value.Equals( dr["PREFIX"]))
                    currContact.Prefix = Convert.ToString(dr["PREFIX"]);

                currContact.FirstName = Convert.ToString(dr["FIRST_NAME"]);
                if (!DBNull.Value.Equals( dr["MIDDLE_INITIAL"]))
                    currContact.MiddleInitial = Convert.ToString(dr["MIDDLE_INITIAL"]);
                currContact.LastName = Convert.ToString(dr["LAST_NAME"]);
                currContact.CreatedDtime = Convert.ToDateTime(dr["CREATED"]);
                if (!DBNull.Value.Equals( dr["UPDATED"]))
                    currContact.UpadtedDtime = Convert.ToDateTime(dr["UPDATED"]);
                currContact.Active = Convert.ToBoolean(dr["ACTIVE"]);

//                object timpestamp  = dr["TIME_STAMP"];

                contacts.Add(currContact.ContactId, currContact);
*/            }

            dr.Close();
            dr.Dispose();

            // get contact phone numbers
            cmd.Parameters.Clear();
            cmd.CommandText = "spGetContactPhonesByTypeId";
//            SqlParameter paramContactType = new SqlParameter("@ContactTypeId", contactType);
 
            cmd.Parameters.Add(paramContactType);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Console.WriteLine(dr[0] + "-" + dr[1] + "-" + dr[2] + "-" + dr[3]
                 + "-" + dr[4] + "-" + dr[5]);
 
                Phone currPhone = new Phone();
                currPhone.PhoneId = Convert.ToInt64(dr["PHONE_ID"]);
                currPhone.PhoneTypeId = Convert.ToInt64(dr["PHONE_TYPE_ID"]);
                currPhone.ContactId = Convert.ToInt64(dr["CONTACT_ID"]);
                currPhone.CountryCode = Convert.ToInt16(dr["COUNTRY_CODE"]);
                currPhone.AreaCode = Convert.ToInt16(dr["AREA_CODE"]);
                currPhone.PhoneNumber = Convert.ToInt32(dr["PHONE_NUMBER"]);
                currPhone.CreatedDtime = Convert.ToDateTime(dr["CREATED"]);
                if (!DBNull.Value.Equals( dr["UPDATED"]))
                    currPhone.UpadtedDtime = Convert.ToDateTime(dr["UPDATED"]);
                currPhone.EffectiveFrom =  Convert.ToDateTime(dr["EFF_FROM"]);
                currPhone.EffectiveTo = Convert.ToDateTime(dr["EFF_TO"]);
                currPhone.UserCode = Convert.ToString(dr["USER_CODE"]);

                contacts[currPhone.ContactId].addPhone(currPhone);

            }

            return contacts;

        }
    }
}
