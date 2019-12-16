using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PODBusinessObjects
{
    public class Phone
    {
        private long _phoneId;
        private long _phoneTypeId;
        private long _contactId;
        int _countryCode;
        int _areaCode;
        int _phoneNumber;
        private DateTime _createdDtime;
        private DateTime _upadatedDtime;
        private DateTime _effFrom;
        private DateTime _effTo;
        private string _userCode;

        public long PhoneId
        {
            get
            {
                return _phoneId;
            }
            set
            {
                _phoneId = value;
            }
        }
        public long PhoneTypeId
        {
            get
            {
                return _phoneTypeId;
            }
            set
            {
                _phoneTypeId = value;
            }
        }
        public long ContactId
        {
            get
            {
                return _contactId;
            }
            set
            {
                _contactId = value;
            }
        }
        public int CountryCode
        {
            get
            {
                return _countryCode;
            }
            set
            {
                _countryCode = value;
            }
        }
        public int AreaCode
        {
            get
            {
                return _areaCode;
            }
            set
            {
                _areaCode = value;
            }
        }
        public int PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value;
            }
        }
        public DateTime CreatedDtime
        {
            get
            {
                return _createdDtime;
            }
            set
            {
                _createdDtime = value;
            }
        }
        public DateTime UpadtedDtime
        {
            get
            {
                return _upadatedDtime;
            }
            set
            {
                _upadatedDtime = value;
            }
        }
        public DateTime EffectiveFrom
        {
            get
            {
                return _effFrom;
            }
            set
            {
                _effFrom = value;
            }
        }
        public DateTime EffectiveTo
        {
            get
            {
                return _effTo;
            }
            set
            {
                _effTo = value;
            }
        }
        public string UserCode
        {
            get
            {
                return _userCode;
            }
            set
            {
                _userCode = value;
            }
        }

        public override String ToString()
        {
            return "+" + _countryCode + "-" + _areaCode + "-" + _phoneNumber;
        }



    }
}
