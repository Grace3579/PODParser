using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;

using Permissions;

namespace PODBusinessObjects
{
    public class Shipment
    {
        private long _shipmentId;
        private long _clientId;
        private string _shipmentSrcRef;
        private string _shipmentRef;
        private int _numOfPieces;
        private string _addtionalRef;
        private string _parentRef;
        private string _originCode;
        private string _destinationCode;

        /**Permissions */
        private ShipmentPermission _permission;

        /*Versions */
        private int _nMaxVersionNum;
        private Dictionary<int, ShipmentVersion> _shipmentVersions;
        private ShipmentVersion _latestShipmentVersion;

        #region Shipment Constructors

        private Shipment()
        {
        }

        public Shipment(long clientId, long shipmentId)
        {
            _clientId = clientId;
            _shipmentId = shipmentId;
            _shipmentVersions = new Dictionary<int, ShipmentVersion>();
            _nMaxVersionNum = -1;
        }

        public Shipment(long clientId, string shiplmentSrc, string shipmentRef)
        {
            _clientId = clientId;
            _shipmentSrcRef = shiplmentSrc;
            _shipmentRef = shipmentRef;
            _shipmentVersions = new Dictionary<int, ShipmentVersion>();
            _nMaxVersionNum = -1;
        }

        #endregion Shipment Constructors

        #region Shipment Properties

        public long ShipmentId
        {
            get { return _shipmentId; }
            set { _shipmentId = value; }
        }
        public long ClientId
        {
            get{return _clientId;}
            set{_clientId = value;}
        }
        public string ShipmentSrc
        {
            get { return _shipmentSrcRef; }
            set { _shipmentSrcRef = value; }
        }
        public string ShipmentRef
        {
            get { return _shipmentRef; }
            set { _shipmentRef = value; }
        }
        public int NumOfPieces
        {
            get { return _numOfPieces; }
            set { _numOfPieces = value; }
        }
        public string AddtionalRef
        {
            get { return _addtionalRef; }
            set { _addtionalRef = value; }
        }
        public string ParentRef
        {
            get { return _parentRef; }
            set { _parentRef = value; }
        }
        public string OriginCode
        {
            get { return _originCode; }
            set { _originCode = value; }
        }
        public string DestinationCode
        {
            get { return _destinationCode; }
            set { _destinationCode = value; }
        }
        public ShipmentPermission Permissions
        {
            get{return _permission;}
            set{_permission = value;}
        }
        public bool HasVersions
        {
            get { return _shipmentVersions.Count > 1 ? true : false; }
        }

        /**
         * Returns the Address of the latest shipment version as a single string
         **/
        public string DeliverToAddress
        {
            get { return getLatestShipmentVersion().DeliverTo.FullAddress; }
        }

        public string AssignedTo
        {
            get {return getAssignedTo();}
        }

        ShipmentVersion LatestShipmentVersion
        {
            get { return getLatestShipmentVersion(); }
        }

        long LatestShipmentVersionId
        {
            get { return getLatestShipmentVersion().ShipmentVersionID; }
        }

        #endregion Shipment Properties


        public bool AddVersion(ShipmentVersion shipmentVersion)
        {
            bool bRetVal = false;
            if (! _shipmentVersions.ContainsKey(shipmentVersion.VersionNum) )
            {
                _shipmentVersions.Add(shipmentVersion.VersionNum, shipmentVersion);

                // set the max ShipmentVersion Number
                if (shipmentVersion.VersionNum > _nMaxVersionNum)
                {
                    _nMaxVersionNum = shipmentVersion.VersionNum;
                    _latestShipmentVersion = _shipmentVersions[_nMaxVersionNum];
                }
                bRetVal = true;
            }
            return bRetVal;
        }

        public ShipmentVersion getLatestShipmentVersion()
        {
            return _latestShipmentVersion;
        }

        public Dictionary<int, ShipmentVersion> getAllVersions()
        {
            return _shipmentVersions;
        }

        public string getAssignedTo()
        {
            string sFullName = "";
            if (getLatestShipmentVersion() != null && getLatestShipmentVersion().AssignedTo != null)
            {
                sFullName = getLatestShipmentVersion().AssignedTo;
            }
            return sFullName;
        }

        /*Returns the object as xml*/
        public XmlDocument toXML(bool bAllVersions)
        {
            XmlDocument retXml = new XmlDocument();

            XmlNode shpNode = retXml.CreateElement("Shipment");

            //add all atributes
            XmlAttribute attrCanAssign = retXml.CreateAttribute("CanAssign");
            attrCanAssign.Value = _permission.CanAssign ? "Y" : "N";
            shpNode.Attributes.Append(attrCanAssign);
            XmlAttribute attrCanUnassign = retXml.CreateAttribute("CanUnassign");
            attrCanUnassign.Value = _permission.CanUnassign ? "Y" : "N";
            shpNode.Attributes.Append(attrCanUnassign);
            XmlAttribute attrCanEdit = retXml.CreateAttribute("CanEdit");
            attrCanEdit.Value = _permission.CanEdit? "Y" : "N";
            shpNode.Attributes.Append(attrCanEdit);
            XmlAttribute attrCanCancel = retXml.CreateAttribute("CanCancel");
            attrCanCancel.Value = _permission.CanCancel ? "Y" : "N";
            shpNode.Attributes.Append(attrCanCancel);
            XmlAttribute attrHasVersions = retXml.CreateAttribute("HasVersions");
            attrHasVersions.Value = HasVersions ? "Y" : "N";
            shpNode.Attributes.Append(attrHasVersions);

            //add all elements
            XmlNode elementSource = retXml.CreateElement("Source");
            elementSource.InnerText = _shipmentSrcRef;
            shpNode.AppendChild(elementSource);
            XmlNode elementSourceRef = retXml.CreateElement("Reference");
            elementSourceRef.InnerText = _shipmentRef;
            shpNode.AppendChild(elementSourceRef);
            XmlNode elementNumOfPeices = retXml.CreateElement("Pieces");
            elementNumOfPeices.InnerText = _numOfPieces.ToString();
            shpNode.AppendChild(elementNumOfPeices);

                //add the ShipmentVersions node
                XmlNode shpVrsnsNode = retXml.CreateElement("ShipmentVersions");
                //add the count of versions being added
                XmlAttribute attrTotalCount = retXml.CreateAttribute("TotalCount");
                attrTotalCount.Value = bAllVersions ? _shipmentVersions.Count.ToString() : "1";
                shpVrsnsNode.Attributes.Append(attrTotalCount);
                    
                    //add versions
                    //always add the latest Vsersion
                    int nCurrVersion = _shipmentVersions.Count;
                    while (nCurrVersion == _shipmentVersions.Count || (nCurrVersion > 0 && bAllVersions))
                    {

                        ShipmentVersion currShpVersion = _shipmentVersions[nCurrVersion];
                        ShipmentVersion prevShpVersion = new ShipmentVersion(-1);
                        if (nCurrVersion < _shipmentVersions.Count)
                            prevShpVersion = _shipmentVersions[nCurrVersion + 1];

                        XmlNode shpVrsnNode = retXml.CreateElement("ShipmentVersion");

                        XmlAttribute attrVersionNum = retXml.CreateAttribute("VersionNum");
                        attrVersionNum.Value = currShpVersion.VersionNum.ToString();
                        shpVrsnNode.Attributes.Append(attrVersionNum);

                        if (currShpVersion.ConsigneeName != prevShpVersion.ConsigneeName)
                        {
                            XmlNode elementConsignee = retXml.CreateElement("Consignee");
                            elementConsignee.InnerText = currShpVersion.ConsigneeName;
                            shpVrsnNode.AppendChild(elementConsignee);
                        }

                        if (currShpVersion.ReceiverName != prevShpVersion.ReceiverName)
                        {
                            XmlNode elementReceiver = retXml.CreateElement("Receiver");
                            elementReceiver.InnerText = currShpVersion.ReceiverName;
                            shpVrsnNode.AppendChild(elementReceiver);
                        }

                        if (currShpVersion.ReceiverPhone != prevShpVersion.ReceiverPhone)
                        {
                            XmlNode elementReceiverPhone = retXml.CreateElement("ReceiverPhone");
                            elementReceiverPhone.InnerText = currShpVersion.ReceiverPhone;
                            shpVrsnNode.AppendChild(elementReceiverPhone);
                        }

                        if (
                            (currShpVersion.DeliverTo != null) &&
                                ( currShpVersion.DeliverTo.FullAddress != 
                                    ( (prevShpVersion.DeliverTo != null ? prevShpVersion.DeliverTo.FullAddress : "") )
                                )
                           )
                        {
                            XmlNode elementAddress = retXml.CreateElement("Address");
                            elementAddress.InnerText = currShpVersion.DeliverTo.FullAddress;
                            shpVrsnNode.AppendChild(elementAddress);
                        }

                        if (currShpVersion.Weight != prevShpVersion.Weight)
                        {

                            XmlNode elementWeight = retXml.CreateElement("Weight");
                            elementWeight.InnerText = currShpVersion.Weight;
                            shpVrsnNode.AppendChild(elementWeight);
                        }

                        if (
                            (currShpVersion.AssignedTo != null) &&
                                (currShpVersion.AssignedTo !=
                                    ((prevShpVersion.AssignedTo != null ? prevShpVersion.AssignedTo : ""))
                                )
                           )
                        {
                            XmlNode elementAssignedTo = retXml.CreateElement("AssignedTo");
                            elementAssignedTo.InnerText = currShpVersion.AssignedTo;
                            shpVrsnNode.AppendChild(elementAssignedTo);
                        }

                        shpVrsnsNode.AppendChild(shpVrsnNode);
                        nCurrVersion--;
                    }

                shpNode.AppendChild(shpVrsnsNode);

            retXml.AppendChild(shpNode);
            return retXml;
        }

    }
}
