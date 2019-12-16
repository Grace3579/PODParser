using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PODUtils;

namespace PODBusinessObjects
{
    public class ShipmentVersion
    {
        #region private members

        private long _shipmentVersionId;
        private long _shipmentId;
        private int _versionNum;
        private SHIPMENT_STATUS _status;
        private string _consigneeName;
        private string _receiverName;
        private string _receiverPhone;
        private string _weight;
        private string _creatingUser;
        private DateTime _createdTime;
        private string _updatingUser;
        private DateTime _updatedTime;
        private byte[] _timeStamp;

        private Address _deliverTo;

        /*Assignment(s) */
        private Dictionary<long, ShipmentAssignment> _shipmentAssignments;
        private ShipmentAssignment _activeShipmentAssignment;

        #endregion private members

        public enum SHIPMENT_STATUS { NEW = 701, AMEND, CANCEL };

        #region ShipmentVersion Constructors

        private ShipmentVersion()
        {
        }
        //used when a new Shipment is being created
        public ShipmentVersion(long shipmentId)
        {
            _shipmentId = shipmentId;
            _shipmentAssignments = new Dictionary<long, ShipmentAssignment>();
        }

        public ShipmentVersion(long shipmentVersionId, long shipmentId)
        {
            _shipmentVersionId = shipmentVersionId;
            _shipmentId = shipmentId;
            _shipmentAssignments = new Dictionary<long, ShipmentAssignment>();
        }

        #endregion ShipmentVersion Constructors

        #region ShipmentVersion Properties

        public long ShipmentVersionID
        {
            get { return _shipmentVersionId; }
        }
        public long ShipmentID
        {
            get { return _shipmentId; }
        }
        public int VersionNum
        {
            get{return _versionNum;}
            set{_versionNum = value;}
        }
        public SHIPMENT_STATUS Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public string ConsigneeName
        {
            get { return _consigneeName; }
            set { _consigneeName = value; }
        }
        public string ReceiverName
        {
            get { return _receiverName; }
            set { _receiverName = value; }
        }
        public string ReceiverPhone
        {
            get { return _receiverPhone; }
            set { _receiverPhone = value; }
        }
        public string Weight
        {
            get { return _weight; }
            set { _weight = value; }
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
        public string AssignedTo
        {
            get 
            {
                string sRetVal = "";
                if (getActiveAssignment() != null && getActiveAssignment().AssignedTo != null)
                    sRetVal = getActiveAssignment().AssignedTo.FullName;

                return sRetVal;
            }
        }
        public Address DeliverTo
        {
            get { return _deliverTo; }
            set { _deliverTo = value; }
        }

        #endregion ShipmentVersion Properties

        public bool AddAssignment(ShipmentAssignment shipmentAssignment)
        {
            bool bRetVal = false;
            if (!_shipmentAssignments.ContainsKey(shipmentAssignment.ShipmentAssignmentId))
            {
                _shipmentAssignments.Add(shipmentAssignment.ShipmentAssignmentId, shipmentAssignment);
                bRetVal = true;
            }
            return bRetVal;
        }

        public ShipmentAssignment getActiveAssignment()
        {
            if (_activeShipmentAssignment == null && _shipmentAssignments.Count > 0)
            {
                Dictionary<long, ShipmentAssignment>.Enumerator assnEnum = _shipmentAssignments.GetEnumerator();
                while(assnEnum.MoveNext())
                {
                    if(assnEnum.Current.Value.isActive)
                    {
                        _activeShipmentAssignment = assnEnum.Current.Value;
                        //there can be only one active assignment. 
                        break;
                    }
                }
            }
            return _activeShipmentAssignment;
        }

        public override String ToString()
        {
            String sRetStr = "ShipmentVersioID = " + Convert.ToString(_shipmentVersionId);
            sRetStr += ", ShipmentID = " + Convert.ToString(_shipmentId);
            sRetStr += ", VersionNum = " + Convert.ToString(_versionNum);
            sRetStr += ", ShipmentStatus = " + Convert.ToString(_status);
            sRetStr += ", ConsigneeName = " + _consigneeName;
            sRetStr += ", ReceiverName = " + _receiverName;
            sRetStr += ", ReceiverPhone = " + _receiverPhone;
            sRetStr += ", ReceiverPhone = " + _receiverPhone;
            sRetStr += ", Weight = " + _weight;
            sRetStr += ", DeliverTo = " + _deliverTo.ToString();

            sRetStr += ", CreatingUser = " + _creatingUser;
            sRetStr += ", CreatedTime = " + Convert.ToString(_createdTime);
            sRetStr += ", UpdatingUser = " + _updatingUser;
            sRetStr += ", UpdatedTime = " + Convert.ToString(_updatedTime);
            sRetStr += ", TimeStamp = " + Convert.ToString(BitConverter.ToUInt64(_timeStamp, 0));

            return sRetStr;
        }


    }


}
