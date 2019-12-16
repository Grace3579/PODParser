using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlTypes;

using PODUtils;

namespace PODBusinessObjects
{
    public class ShipmentAssignment
    {
        private long _shipmentAssignmentId;
        private long _shipmentVersionId;
        private bool _isActive;

        private Contact _assignedTo;

        private string _creatingUser;
        private DateTime _createdTime;
        private string _updatingUser;
        private DateTime _updatedTime;
        private byte[] _timeStamp;

        //no default constructor
        private ShipmentAssignment(){}


        public ShipmentAssignment(long shipmentAssignmentId, long shipmentVersionId, bool isActive)
        {
            _shipmentAssignmentId = shipmentAssignmentId;
            _shipmentVersionId = shipmentVersionId;
            _isActive = isActive;
        }

        #region ShipmentAssignment Properties
        
        public long ShipmentAssignmentId
        {
            get { return _shipmentAssignmentId;}
        }
        public long ShipmentVersionId
        {
            get { return _shipmentVersionId; }
        }
        public bool isActive
        {
            get {return _isActive; }
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
        public byte[] TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }
        public Contact AssignedTo
        {
            get { return _assignedTo; }
            set { _assignedTo = value; }
        }
        
        #endregion ShipmentAssignment Properties

        public override String ToString()
        {
            String sRetStr = "ShipmentAssignmentID = " + Convert.ToString(_shipmentAssignmentId);
            sRetStr += ", ShipmentVersionID = " + Convert.ToString(_shipmentVersionId);
            sRetStr += ", IsActive = " + Convert.ToString(_isActive);
            sRetStr += ", AssignedTo = " + _assignedTo.ToString();
            sRetStr += ", CreatingUser = " + _creatingUser;
            sRetStr += ", CreatedTime = " + Convert.ToString(_createdTime);
            sRetStr += ", UpdatingUser = " + _updatingUser;
            sRetStr += ", UpdatedTime = " + Convert.ToString(_updatedTime);
            sRetStr += ", TimeStamp = " + Convert.ToString(BitConverter.ToUInt64(_timeStamp, 0));

            return sRetStr;
        }
    }
}
