using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PODUtils
{
    public class Utils
    {

        //TODO:: move to a config file
        public static String DELIMITER_STRING = "|";

        public static void LogCollection(SortedList list)
        {
            /*TODO:: This should be written to a log file*/
            for (int i = 0; i < list.Count; i++)
            {
                String currLine = "Element# = " + i;
                currLine += "\n\tKey = " + list.GetKey(i).ToString();
                currLine += "\n\t\tObject = " + list.GetByIndex(i).ToString();

                System.Console.WriteLine (currLine);

            }
        }

        public static string ByteArrayToString(Byte[] byteArray)
        {
            StringBuilder strBuilder = new StringBuilder(byteArray.Length);
            for (int i =0; i< byteArray.Length; i++)
            {
                strBuilder.Append(String.Format("{0:X}", Convert.ToInt16(byteArray[i])));
            }

            return strBuilder.ToString();
        }

        public static String convertIdsToString(List<long> ids)
        {
            String strIds = "";
            for (int i = 0; i < ids.Count; ++i)
            {
                strIds += ids[i] + DELIMITER_STRING;
            }

            return strIds;
        }
    }
}
