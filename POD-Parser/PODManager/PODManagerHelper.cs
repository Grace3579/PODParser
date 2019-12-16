using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Xml;
using System.Reflection;

using PODBusinessObjects;
using PODUtils;
using Permissions;

namespace PODManagerGUIHelper
{
    public class ShipmentManagerHelper
    {
        private String _userCode;
        private String _compayCode;
        private String _configFilesRoot;
        private String[] _configFiles;


        public List<RenderingInfo> getRenderingInfoForEmployees()
        {
            List<RenderingInfo> objRenderingInfoList = new List<RenderingInfo>(4);

            RenderingInfo objRenderingInfo11 = new RenderingInfo();
            objRenderingInfo11.ColumnOrder = 1;
            objRenderingInfo11.ColumnName = "FullName";
            objRenderingInfo11.Header = "Assign To";
            objRenderingInfo11.Type = "String";
            objRenderingInfo11.Visible = 'Y';

            FormatInfo objFormatInfo11 = new FormatInfo();
            objFormatInfo11.Width = 200;
            objFormatInfo11.Justify = "Left";

            objRenderingInfo11.Format = objFormatInfo11;
            objRenderingInfoList.Add(objRenderingInfo11);
            /********************************************************************************/

            RenderingInfo objRenderingInfo2 = new RenderingInfo();
            objRenderingInfo2.ColumnOrder = 2;
            objRenderingInfo2.ColumnName = "PhoneNumber";
            objRenderingInfo2.Header = "Phone Number";
            objRenderingInfo2.Type = "String";
            objRenderingInfo2.Visible = 'Y';

            FormatInfo objFormatInfo2 = new FormatInfo();
            objFormatInfo2.Width = 150;
            objFormatInfo2.Justify = "Left";

            objRenderingInfo2.Format = objFormatInfo2;
            objRenderingInfoList.Add(objRenderingInfo2);
            /********************************************************************************/

            RenderingInfo objRenderingInfo3 = new RenderingInfo();
            objRenderingInfo3.ColumnOrder = 3;
            objRenderingInfo3.ColumnName = "NewPhone";
            objRenderingInfo3.Header = "New Phone Number";
            objRenderingInfo3.Type = "String";
            objRenderingInfo3.Visible = 'Y';

            FormatInfo objFormatInfo3 = new FormatInfo();
            objFormatInfo3.Width = 200;
            objFormatInfo3.Justify = "Left";

            objRenderingInfo3.Format = objFormatInfo3;
            objRenderingInfoList.Add(objRenderingInfo3);
            /********************************************************************************/

            RenderingInfo objRenderingInfo4 = new RenderingInfo();
            objRenderingInfo4.ColumnOrder = 4;
            objRenderingInfo4.ColumnName = "ContactId";
            objRenderingInfo4.Header = "";
            objRenderingInfo4.Type = "Number";
            objRenderingInfo4.Visible = 'N';

            objRenderingInfoList.Add(objRenderingInfo4);
            /********************************************************************************/
            return objRenderingInfoList;
        }
        public List<RenderingInfo> getRenderingInfo()
        {
            List<RenderingInfo> RenderingInfoList = new List<RenderingInfo>(10);//10=>TotalCount


            RenderingInfo objRenderingInfo11 = new RenderingInfo();
            objRenderingInfo11.ColumnOrder = 11;
            objRenderingInfo11.ColumnName = "ShipmentVersionID";
            objRenderingInfo11.Header = "ShipmentVersion ID";
            objRenderingInfo11.Type = "Number";
            objRenderingInfo11.Visible = 'N';

            FormatInfo objFormatInfo11 = new FormatInfo();
            objFormatInfo11.Width = 100;
            objFormatInfo11.Justify = "Left";

            objRenderingInfo11.Format = objFormatInfo11;
            RenderingInfoList.Add(objRenderingInfo11);
            /********************************************************************************/


            RenderingInfo objRenderingInfo1 = new RenderingInfo();
            objRenderingInfo1.ColumnOrder = 2;
            objRenderingInfo1.ColumnName = "ShipmentRef";
            objRenderingInfo1.Header = "Reference";
            objRenderingInfo1.Type = "String";
            objRenderingInfo1.Visible = 'Y';

            FormatInfo objFormatInfo1 = new FormatInfo();
            objFormatInfo1.Width = 100;
            objFormatInfo1.Justify = "Left";

            objRenderingInfo1.Format = objFormatInfo1;
            RenderingInfoList.Add(objRenderingInfo1);
            /********************************************************************************/

            RenderingInfo objRenderingInfo = new RenderingInfo();
            objRenderingInfo.ColumnOrder = 1;
            objRenderingInfo.ColumnName = "ShipmentSrc";
            objRenderingInfo.Header = "Source";
            objRenderingInfo.Type = "String";
            objRenderingInfo.Visible = 'Y';

            FormatInfo objFormatInfo = new FormatInfo();
            objFormatInfo.Width = 100;
            objFormatInfo.Justify = "Left";

            objRenderingInfo.Format = objFormatInfo;
            RenderingInfoList.Add(objRenderingInfo);
            /********************************************************************************/


            RenderingInfo objRenderingInfo2 = new RenderingInfo();
            objRenderingInfo2.ColumnOrder = 3;
            objRenderingInfo2.ColumnName = "ParentRef";
            objRenderingInfo2.Header = "Receiver";
            objRenderingInfo2.Type = "String";
            objRenderingInfo2.Visible = 'Y';

            FormatInfo objFormatInfo2 = new FormatInfo();
            objFormatInfo2.Width = 100;
            objFormatInfo2.Justify = "Left";

            objRenderingInfo2.Format = objFormatInfo2;
            RenderingInfoList.Add(objRenderingInfo2);
            /********************************************************************************/

            RenderingInfo objRenderingInfo3 = new RenderingInfo();
            objRenderingInfo3.ColumnOrder = 4;
            objRenderingInfo3.ColumnName = "AddtionalRef";
            objRenderingInfo3.Header = "Delivery Address";
            objRenderingInfo3.Type = "String";
            objRenderingInfo3.Visible = 'Y';

            FormatInfo objFormatInfo3 = new FormatInfo();
            objFormatInfo3.Width = 200;
            objFormatInfo3.Justify = "Left";

            objRenderingInfo3.Format = objFormatInfo3;
            RenderingInfoList.Add(objRenderingInfo3);
            /********************************************************************************/

            RenderingInfo objRenderingInfo4 = new RenderingInfo();
            objRenderingInfo4.ColumnOrder = 5;
            objRenderingInfo4.ColumnName = "AssignedTo";
            objRenderingInfo4.Header = "Assigned To";
            objRenderingInfo4.Type = "String";
            objRenderingInfo4.Visible = 'Y';

            FormatInfo objFormatInfo4 = new FormatInfo();
            objFormatInfo4.Width = 100;
            objFormatInfo4.Justify = "Left";

            objRenderingInfo4.Format = objFormatInfo4;
            RenderingInfoList.Add(objRenderingInfo4);
            /********************************************************************************/

            RenderingInfo objRenderingInfo5 = new RenderingInfo();
            objRenderingInfo5.ColumnOrder = 6;
            objRenderingInfo5.ColumnName = "Status";
            objRenderingInfo5.Header = "Status";
            objRenderingInfo5.Type = "String";
            objRenderingInfo5.Visible = 'Y';

            FormatInfo objFormatInfo5 = new FormatInfo();
            objFormatInfo5.Width = 100;
            objFormatInfo5.Justify = "Left";

            objRenderingInfo5.Format = objFormatInfo5;
            RenderingInfoList.Add(objRenderingInfo5);
            /********************************************************************************/

            RenderingInfo objRenderingInfo6 = new RenderingInfo();
            objRenderingInfo6.ColumnOrder = 7;
            objRenderingInfo6.ColumnName = "Weight";
            objRenderingInfo6.Header = "Weight";
            objRenderingInfo6.Type = "Decimal";
            objRenderingInfo6.Visible = 'Y';

            FormatInfo objFormatInfo6 = new FormatInfo();
            objFormatInfo6.Width = 100;
            objFormatInfo6.Justify = "Left";

            objRenderingInfo6.Format = objFormatInfo6;
            RenderingInfoList.Add(objRenderingInfo6);
            /********************************************************************************/

            RenderingInfo objRenderingInfo7 = new RenderingInfo();
            objRenderingInfo7.ColumnOrder = 8;
            objRenderingInfo7.ColumnName = "NumOfPieces";
            objRenderingInfo7.Header = "Total Packets";
            objRenderingInfo7.Type = "String";
            objRenderingInfo7.Visible = 'Y';

            FormatInfo objFormatInfo7 = new FormatInfo();
            objFormatInfo7.Width = 150;
            objFormatInfo7.Justify = "Right";

            objRenderingInfo7.Format = objFormatInfo7;
            RenderingInfoList.Add(objRenderingInfo7);
            /********************************************************************************/

            RenderingInfo objRenderingInfo8 = new RenderingInfo();
            objRenderingInfo8.ColumnOrder = 9;
            objRenderingInfo8.ColumnName = "DeliveryTime";
            objRenderingInfo8.Header = "Delivery Time";
            objRenderingInfo8.Type = "Date";

            FormatInfo objFormatInfo8 = new FormatInfo();
            objFormatInfo8.Width = 100;
            objFormatInfo8.Justify = "Left";

            objRenderingInfo8.Format = objFormatInfo8;
            RenderingInfoList.Add(objRenderingInfo8);
            /********************************************************************************/

            RenderingInfo objRenderingInfo9 = new RenderingInfo();
            objRenderingInfo9.ColumnOrder = 10;
            objRenderingInfo9.ColumnName = "ShipmentId";
            objRenderingInfo9.Header = "ShipmentId";
            objRenderingInfo9.Type = "Number";
            objRenderingInfo9.Visible = 'N';

            FormatInfo objFormatInfo9 = new FormatInfo();
            objFormatInfo9.Width = 100;
            objFormatInfo9.Justify = "Right";

            objRenderingInfo9.Format = objFormatInfo9;
            RenderingInfoList.Add(objRenderingInfo9);
            /********************************************************************************/

            return RenderingInfoList;
        }
    }

    public class FormatInfo
    {
        private int _width;
        private string _justify;

        public FormatInfo()
        { }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public string Justify
        {
            get { return _justify; }
            set { _justify = value; }
        }
    }

    public class RenderingInfo
    {
        private int _userId;
        private int _columnOrder;
        private string _columnName;
        private string _header;
        private string _type;
        private char _visible;
        private FormatInfo _format;

        public RenderingInfo()
        { }

        public int ColumnOrder
        {
            get { return _columnOrder; }
            set { _columnOrder = value; }
        }
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public char Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
        public FormatInfo Format
        {
            get { return _format; }
            set { _format = value; }
        }

    }

}
