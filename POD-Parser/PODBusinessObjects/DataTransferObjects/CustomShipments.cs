using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PODBusinessObjects.DataTransferObjects
{
    /************************Custom class for Data Load file parsing****************************/
    public class CustomShipments
    {
        public long ClientId { get; set; }
        public string ShipmentSrc { get; set; }
        public string ShipmentRef { get; set; }
        public string ReceiverName { get; set; }
        public string DeliveryAddress { get; set; }
        public int NumOfPieces { get; set; }
        public string Weight { get; set; }
    }
}
