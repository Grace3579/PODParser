using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PODBusinessObjects
{
    public class Address
    {
        private long _addressId;
        private long _clientId;
        private ADDRESS_TYPE _addressType;

        private long _contactId;
        private string _addressLine1;
        private string _addressLine2;
        private string _addressLine3;
        private string _addressLine4;
        private string _city;
        private string _state;
        private string _postalcode;
        private string _country;
        private DateTime _effFrom;
        private DateTime _effTo;
        private bool _isPrimary;
        private string _creatingUser;
        private DateTime _createdTime;
        private string _updatingUser;
        private DateTime _updatedTime;
        private Byte[] _timeStamp;

        public enum ADDRESS_TYPE { HOME = 201, OFFICE, MAIL, SHOWROOM, WAREHOUSE, CLIENT_REGD_OFFICE, CLIENT_OFFICE, CLIENT_MAIL, DELIVERY };

        /*Do not allow Address object creation without any parameters*/
        /*Temp allowed for testing only*/
        public Address() { }
        public Address(long addressId, long clientId, ADDRESS_TYPE addressType)
        {
            _addressId = addressId;
            _clientId = clientId;
            _addressType = addressType;
        }
        public Address(long addressId, long clientId, long addressTypeId, Byte[] timeStamp)
        {
            _addressId = addressId;
            _clientId = clientId;
            _addressType = (ADDRESS_TYPE)addressTypeId;
            _timeStamp = timeStamp;
        }

        #region Address Properties
        public long AddressID 
        { 
            get { return _addressId; } 
            set { _addressId = value; } 
        }
        public long ClientID { get { return _clientId; } }
        public ADDRESS_TYPE AddressType { get { return _addressType; } }
        public long ContactID 
        { 
            get { return _contactId; }
            set { _contactId = value; }
        }
        public string AddressLine1
        {
            get { return _addressLine1; }
            set { _addressLine1 = value; }
        }
        public string AddressLine2
        {
            get { return _addressLine2; }
            set { _addressLine2 = value; }
        }
        public string AddressLine3
        {
            get { return _addressLine3; }
            set { _addressLine3 = value; }
        }
        public string AddressLine4
        {
            get { return _addressLine4; }
            set { _addressLine4 = value; }
        }
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }
        public string PostalCode
        {
            get { return _postalcode; }
            set { _postalcode = value; }
        }
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }
        public DateTime EffectiveFrom
        {
            get { return _effFrom; }
            set { _effFrom = value; }
        }
        public DateTime EffectiveTo
        {
            get { return _effTo; }
            set { _effTo = value; }
        }
        public bool IsPrimary
        {
            get { return _isPrimary; }
            set { _isPrimary = value; }
        }
        public string CreatingUser
        {
            get { return _creatingUser; }
            set { _creatingUser = value; }
        }
        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }
        public string UpdatingUser
        {
            get { return _updatingUser; }
            set { _updatingUser = value; }
        }
        public DateTime UpdatedTime
        {
            get { return _updatedTime; }
            set { _updatedTime = value; }
        }
        public string FullAddress
        {
            set
            {
                /*full address is stored in one field - addressLine1*/
                _addressLine1 = value;
            }
            get
            {
                StringBuilder sFullAdd = new StringBuilder();
                sFullAdd.Append(_addressLine1);
                if (_addressLine2 != null && _addressLine2.Trim().Length > 0)
                    sFullAdd.Append(", " + _addressLine2.Trim());
                if (_addressLine3 != null && _addressLine3.Trim().Length > 0)
                    sFullAdd.Append(", " + _addressLine3.Trim());
                if (_addressLine4 != null && _addressLine4.Trim().Length > 0)
                    sFullAdd.Append(", " + _addressLine4.Trim());
                if (_city != null && _city.Trim().Length > 0)
                    sFullAdd.Append(", " + _city.Trim());
                if (_state != null && _state.Trim().Length > 0)
                    sFullAdd.Append(", " + _state.Trim());
                if (_postalcode != null && _postalcode.Trim().Length > 0)
                    sFullAdd.Append(" - " + _postalcode.Trim());
                if (_country != null && _country.Trim().Length > 0)
                    sFullAdd.Append(", " + _country.Trim().ToUpper());

                return sFullAdd.ToString();
            }
        }

        //read only - can only be set at object creation time, when read from DB
        public byte[] TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }

        #endregion

        
       
    }
}
