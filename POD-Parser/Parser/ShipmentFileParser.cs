using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using PODBusinessObjects;

namespace Parsers
{
    public class ShipmentFileParser : FileParser
    {
        protected long _clientId;
        protected string _userCode;

        protected ShipmentFileParser() { }

        public ShipmentFileParser(string delimiter, string commentLinePrefix, long nClientId, string userCode) 
            : base(delimiter, commentLinePrefix)
        {
            _clientId = nClientId;
            _userCode = userCode;
        }

        public virtual List<Shipment> ParseShipments(StreamReader strFileAsStream, string mapperName)
        {
            List<Shipment> shipments = new List<Shipment>();


            return shipments;
        }
    }
}
