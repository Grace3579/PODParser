using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Permissions
{
    public class ShipmentPermission: Permission
    {
        private long _shipmentId;
        private bool _canAssign;
        private bool _canUnassign;
        private bool _canEdit;
        private bool _canCancel;

        private ShipmentPermission() { }

        public ShipmentPermission(long clientId, long shipmentId)
        {
            _clientId = clientId;
            _shipmentId = shipmentId;
        }

        # region Properties
        public long ShipmentID
        {
            get { return _shipmentId; }
            set { _shipmentId = value; }
        }
        public bool CanAssign
        {
            get { return _canAssign; }
            set { _canAssign = value; }
        }
        public bool CanUnassign
        {
            get { return _canUnassign; }
            set { _canUnassign = value; }
        }
        public bool CanEdit
        {
            get { return _canEdit; }
            set { _canEdit = value; }
        }
        public bool CanCancel
        {
            get { return _canCancel; }
            set { _canCancel = value; }
        }

        #endregion Properties


    }
}
