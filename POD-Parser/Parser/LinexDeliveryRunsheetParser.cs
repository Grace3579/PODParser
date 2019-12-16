using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using PODBusinessObjects;

namespace Parsers
{
    public class LinexDeliveryRunsheetParser : ShipmentFileParser
    {
        protected LinexDeliveryRunsheetParser()
        {
        }

        public LinexDeliveryRunsheetParser(string delimiter, string commentLinePrefix, long nClientId, String userCode) 
            : base(delimiter, commentLinePrefix, nClientId, userCode)
        {
        }

        public List<Shipment> ParseShipments(StreamReader strFileAsStream)
        {
            
            SortedList<Int32, SortedList<short, string>> fileAsStrings = base.ParseFile(strFileAsStream);

            return CreateShipmentsFromParsedFile(fileAsStrings);

        }

        private List<Shipment> CreateShipmentsFromParsedFile(SortedList<Int32, SortedList<short, string>> fileAsStrings)
        {
            List<Shipment> shipments = new List<Shipment>();

            # region File Layout description
            // Line#0 is empty, Line#1 has the heading, Line#2 also empty, Line#3has the staff and DRS number, Line#4 is again empty
            // Line#5 has area and date information, Line#6 and Line#7 are empty, Line#8 and Line#9 are Column Headers, Line#10 and Line#11 are empty
            // Line#12 onwards is shipment data
            // Page information is provided when two empty lines exist after a shipment record, which is in Column 20 as "Page n of m"
            //  Each shipment data set is layed out in seven lines as follows
            //      Line offset 0, Column 0 is Sr No.
            //      Line offset 0, Column 2 is Shipment Ref
            //      Line offset 0, Column 7 is Receiver Name
            //      Line offset 1, Column 7 is Address Line# 1
            //      Line offset 2, Column 7 is Address Line# 2
            //      Line offset 3, Column 7 is Address Line# 3
            //      Line offset 4, Column 7 is Number of Pieces
            //      Line offset 4, Column 10 is the text "pcs", which will be ignored
            //      Line offset 5, Column 7 is weight
            //      Line offset 5, Column 10 is the unit of weight, which will be added to the weight (weigth is stored as string)
            //      Line offset 6 is empty

            // TBD :: How does Page number > 1 look like

            # endregion File Layout description

            int nCurrShipmentStartingLineNum = 12;   // first shipment starts from this line
            int nTotalLinesPerShipment = 7;         // as explained above in comments
            int nCurrOffset = 0;

            Dictionary<Int32, SortedList<short, string> > linesDic = new Dictionary<int,SortedList<short,string>> (fileAsStrings);
            Dictionary<Int32, SortedList<short, string> >.Enumerator linesEnum = linesDic.GetEnumerator();

            while (linesEnum.MoveNext())
            {
                if (linesEnum.Current.Key == nCurrShipmentStartingLineNum + nCurrOffset)
                {
                    // if this is an empty line, we may be inbetween two pages 
                    if (linesEnum.Current.Value.Count == 0)
                    {
                        linesEnum.MoveNext();
                        nCurrOffset++;
                        // confirm that this is a page break
                        nCurrShipmentStartingLineNum = GetNextPageShipmentOffset(linesEnum, nCurrShipmentStartingLineNum + nCurrOffset);
                        nCurrOffset = 0;
                        break;

                    }

                    SortedList<short, string> lineStrings0 = linesEnum.Current.Value;
                    String sSrNo = lineStrings0[0];
                    Shipment newShipment = new Shipment(_clientId, "Linex", lineStrings0[2]);
                    ShipmentVersion newShipmentVersion = new ShipmentVersion(-1);                   // we do not have the ShipmentVersionID for now
                    newShipmentVersion.VersionNum = 1;                                              // set the VersionNum = 1, this will be updated in the db correctly
                    newShipmentVersion.CreatingUser = _userCode;
                    newShipmentVersion.Status = ShipmentVersion.SHIPMENT_STATUS.NEW;
                    newShipmentVersion.ReceiverName = lineStrings0[7];

                    Address currAddress = new Address(-1, _clientId, Address.ADDRESS_TYPE.DELIVERY);    // we do not have the AddresssID for now
                    currAddress.CreatingUser = _userCode;
                    currAddress.IsPrimary = true;
                    currAddress.EffectiveFrom = DateTime.Now;
                    currAddress.EffectiveTo = new DateTime(9999, 12, 1);    //end of time

                    // move to the next line (offset 1)
                    if (linesEnum.MoveNext())
                    {
                        nCurrOffset++;
                        SortedList<short, string> lineStrings1 = linesEnum.Current.Value;
                        if ((lineStrings1.Count > 0) && lineStrings1.ContainsKey(7) )   // Address is at offset 7
                            currAddress.AddressLine1 = lineStrings1[7];
                    }
                    else
                    {
                        System.Console.WriteLine("Skipping Incomplete/Incorrect Shipment data for Sr No. {0}, Shipment Number = {1}", sSrNo, newShipment.ShipmentRef);
                        continue;
                    }

                    // move to the next line (offset 2)
                    if (linesEnum.MoveNext())
                    {
                        nCurrOffset++;
                        SortedList<short, string> lineStrings2 = linesEnum.Current.Value;
                        if ((lineStrings2.Count > 0) && lineStrings2.ContainsKey(7))   // Address is at offset 7
                            currAddress.AddressLine2 = lineStrings2[7];
                    }
                    else
                    {
                        System.Console.WriteLine("Skipping Incomplete/Incorrect Shipment data for Sr No. {0}, Shipment Number = {1}", sSrNo, newShipment.ShipmentRef);
                        continue;
                    }

                    // move to the next line (offset 3)
                    if (linesEnum.MoveNext())
                    {
                        nCurrOffset++;
                        SortedList<short, string> lineStrings3 = linesEnum.Current.Value;
                        if ((lineStrings3.Count > 0) && lineStrings3.ContainsKey(7))   // Address is at offset 7
                            currAddress.AddressLine3 = lineStrings3[7];
                    }
                    else
                    {
                        System.Console.WriteLine("Skipping Incomplete/Incorrect Shipment data for Sr No. {0}, Shipment Number = {1}", sSrNo, newShipment.ShipmentRef);
                        continue;
                    }

                    // add the address to the ShipmentVersion
                    newShipmentVersion.DeliverTo = currAddress;

                    // move to the next line (offset 4)
                    if (linesEnum.MoveNext())
                    {
                        nCurrOffset++;
                        SortedList<short, string> lineStrings4 = linesEnum.Current.Value;
                        newShipment.NumOfPieces = Convert.ToInt16(lineStrings4[7]);
                    }
                    else
                    {
                        System.Console.WriteLine("Skipping Incomplete/Incorrect Shipment data for Sr No. {0}, Shipment Number = {1}", sSrNo, newShipment.ShipmentRef);
                        continue;
                    }

                    // move to the next line (offset 5)
                    if (linesEnum.MoveNext())
                    {
                        nCurrOffset++;
                        SortedList<short, string> lineStrings5 = linesEnum.Current.Value;
                        newShipmentVersion.Weight = lineStrings5[7] + " " + lineStrings5[10];
                    }
                    else
                    {
                        System.Console.WriteLine("Skipping Incomplete/Incorrect Shipment data for Sr No. {0}, Shipment Number = {1}", sSrNo, newShipment.ShipmentRef);
                        continue;
                    }

                    // add the ShipmentVersion to the Shipment
                    newShipment.AddVersion(newShipmentVersion);

                    // move to the next line (offset 6)
                    if (linesEnum.MoveNext())
                    {
                        nCurrOffset++;
                        // This is an empty line
                    }
                    else
                    {
                        System.Console.WriteLine("Skipping Incomplete/Incorrect Shipment data for Sr No. {0}, Shipment Number = {1}", sSrNo, newShipment.ShipmentRef);
                        continue;
                    }

                    shipments.Add(newShipment);

                    // move the Shipment Starting to be the next line
                    nCurrShipmentStartingLineNum = nCurrShipmentStartingLineNum + nCurrOffset + 1; 
                    // reset offset
                    nCurrOffset = 0;

                }
                else
                    continue;
                    
            }

            return shipments;
        }

        private int GetNextPageShipmentOffset(Dictionary<Int32, SortedList<short, string>>.Enumerator linesEnum, int nCurrPostion)
        {
            int nextShipmentOffset = nCurrPostion;
            // confirm that the key matches withthe current postion
            // linesEnum.Current.Key == nCurrPostion;

            SortedList<short, string> lineStrings0 = linesEnum.Current.Value;
            // confirm that the corrent collection as "Page n of m" 
            if (lineStrings0[20].IndexOf("Page") < 0)
            {
                System.Console.WriteLine("Page break data not found!");

            }
            linesEnum.MoveNext();

            // for now add an arbitrary value
            return nextShipmentOffset + 5;
        }
   }
}
