using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PODBusinessObjects
{
    public class Contact
    {
        private long _clientId;
        private long _contactId;
        private CONTACT_TYPE _contactTypeId;
        private string _prefix;
        private string _firstName;
        private string _middleInitial;
        private string _lastName;
        private string _fullName;
        private long _phoneNumber;
        private string _contactCode;
        private string _creatingUser;
        private DateTime _createdTime;
        private string _updatingUser;
        private DateTime _updatedTime;
        private bool _isActive;
        private byte[] _timeStamp;

        //private 
        private List<Phone> _phones;

        // from  Select * from TYPE_CODES where TYPE_CODE = 'CONTACT'
        public enum CONTACT_TYPE
        {
            UNKNOWN = 100, EMPLOYEE, CUSTOMER_INDIVIDUAL,
            CUSTOMER_PVT_COMPANY
                , CUSTOMER_PVT_CORP, CUSTOMER_PUBLIC_CORP
        };

        #region Constructors
        /*Do not allow Contact object creation without any parameters*/
        private Contact()
        {
            _phones = new List<Phone>(4);
        }
        public Contact(long clientId, long contactId)
        {
            _phones = new List<Phone>();
            _clientId = clientId;
            _contactTypeId = CONTACT_TYPE.UNKNOWN;
        }

        public Contact(long clientId, long contactId, Contact.CONTACT_TYPE contactType)
        {
            _phones = new List<Phone>();
            _clientId = clientId;
            _contactId = contactId;
            _contactTypeId = contactType;
        }

        // typically used for creating a new employee on-the-fly, when doing shipment Assignments
        public Contact(long clientId, string fullName, long phoneNumber)
        {
            _phones = new List<Phone>();
            _clientId = clientId;
            _contactId = -1;    // new contact, that would need to be saved to the DB, if required
            _fullName = fullName;
            _phoneNumber = phoneNumber;
            _contactTypeId = CONTACT_TYPE.UNKNOWN;
        }

        #endregion Constructors

        #region Contact Properties

        public long ContactId
        {
            get { return _contactId; }
            set { _contactId = value; }
        }
        public long ClientId
        {
            get { return _clientId; }
            set { _clientId = value; }
        }
        public CONTACT_TYPE ContactTypeId
        {
            get { return _contactTypeId; }
            set { _contactTypeId = value; }
        }
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }
        public string MiddleInitial
        {
            get
            {
                return _middleInitial;
            }
            set
            {
                _middleInitial = value;
            }
        }
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }
        public string FullName
        {
            get
            {
                string sFullName = "";
                if (_fullName != null && _fullName.Length > 0)
                {
                    sFullName = _fullName;
                }
                else
                {
                    if (_prefix != null && _prefix.Length > 0)
                        sFullName = _prefix + " ";
                    if (_firstName != null && _firstName.Length > 0)
                        sFullName += _firstName + " ";
                    if (_middleInitial != null && _middleInitial.Length > 0)
                        sFullName += _middleInitial + " ";
                    if (_lastName != null && _lastName.Length > 0)
                        sFullName += _lastName;
                }

                return sFullName.Trim();
            }

            set { _fullName = value; }
        }
        public long PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }
        public string ContactCode
        {
            get { return _contactCode; }
            set { _contactCode = value; }
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
        public bool Active
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
            }
        }
        public byte[] TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }

        #endregion Contact Properties

        #region manipulating Phone collection

        public bool addPhone(Phone _phone)
        {
            _phones.Add(_phone);
            return true;
        }
        public bool removePhone(Phone _phone)
        {
            _phones.Remove(_phone);
            return true;
        }
        public List<Phone> getPhones()
        {
            return _phones;
        }

        public Phone getMobilePhone()
        {
            return _phones.Find(mobilePhone);
        }

        private bool mobilePhone(Phone phone)
        {
            if (phone.PhoneTypeId == 303 && phone.EffectiveFrom <= System.DateTime.Now
                    && phone.EffectiveTo >= System.DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion manipulating Phone collection

    }
}
