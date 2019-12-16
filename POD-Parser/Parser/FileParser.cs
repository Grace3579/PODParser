using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Parsers
{
    public class FileParser
    {
        # region Private Data
        private string m_delimiter;
        private string m_commentLinePrefix;
        private string m_currentColumnValue;
        private string m_currentLineData;
        private string m_fileName;
        # endregion Private Data

        # region Constructors
        // do not allow a FileParser to be created without delimiter and comment line prefix (commentLinePrefix)
        protected FileParser()
        {
        }

        public FileParser(string delimiter, string commentLinePrefix)
        {
            m_delimiter = delimiter;
            m_commentLinePrefix = commentLinePrefix;
            m_currentColumnValue = "";
            m_currentLineData = "";
        }

        # endregion Constructors

        # region Properties
        string Delimiter
        { get { return m_delimiter; } }

        string CommentLinePrefix
        { get { return m_commentLinePrefix; } }

        string CurrentColumnValue
        {
            get { return m_currentColumnValue; }
            set { m_currentColumnValue = value; }
        }

        string CurrentLineData
        {
            get { return m_currentLineData; }
            set { m_currentLineData = value; }
        }

        # endregion Properties

        # region Public Functions

        public virtual SortedList<Int32, SortedList<Int16, string>> ParseFile(string fullFileName)
        {
            m_fileName = fullFileName;
            return ParseFile(new StreamReader(m_fileName));
        }
        
        // returns a collection of strings, one for each line, with each value split by the delimiter
        // skips commented lines
        public virtual SortedList<Int32, SortedList<short, string>> ParseFile(StreamReader strFileAsStream)
        {
            SortedList<Int32, SortedList<short, string>> parsedLines = new SortedList<int, SortedList<short, string>>();

            this.CurrentLineData = strFileAsStream.ReadLine();
            short currLineNum = 0;
            while (this.CurrentLineData != null && this.CurrentLineData.Length > 0)
            {
                System.Console.WriteLine("\tLine #{0} = {1}", currLineNum, this.CurrentLineData);

                if (LineCanBeProcessed())
                {
                    parsedLines.Add(currLineNum, this.ParseLine());
                    ++currLineNum;
                }

                this.CurrentLineData = strFileAsStream.ReadLine();
            }

            return parsedLines;
        }

        # endregion Public Functions

        private SortedList<short, string> ParseLine()
        {
            SortedList<short, string> currLineStrings = new SortedList<short,string>();

            this.CurrentLineData= this.CurrentLineData.Trim();
            int nStartIndex = 0;
            int nEndIndex = nStartIndex;
            short currColumnIndex = 0;

            while (nEndIndex < this.CurrentLineData.Length)
            {
                // check if the first non-space char is a double quote '"'
                // if yes, find the next one, which would indicate the end of that colum
                //nStartIndex = getNextNonSpaceCharIndex(nStartIndex  + m_delimiter.Length);        // TBD :: to check this logic!! Is this needed?
                bool bQuoteFound = false;
                if (nStartIndex + m_delimiter.Length < this.CurrentLineData.Length)
                {
                    if (this.CurrentLineData.Substring(nStartIndex + m_delimiter.Length, 1) == "\"")
                    {
                        nEndIndex = this.CurrentLineData.IndexOf('"', nStartIndex + m_delimiter.Length + 1);
                        nStartIndex++;          // skip the leading double quote as that is not part of the data
                        bQuoteFound = true;     // note that a starting quote was found, use it to skip the ending quote
                    }
                    else
                    {
                        nEndIndex = this.CurrentLineData.IndexOf(m_delimiter, nStartIndex + m_delimiter.Length);
                        if (nEndIndex == -1)
                            nEndIndex = this.CurrentLineData.Length;
                    }

//                    System.Console.WriteLine("\t\tLevel01 currColumnIndex = {0}, nStartIndex = {1}, nEndIndex = {2}", currColumnIndex, nStartIndex, nEndIndex);

                    if (nStartIndex == 0 && nEndIndex > nStartIndex)
                    {
                        currLineStrings.Add(currColumnIndex, this.CurrentLineData.Substring(nStartIndex, nEndIndex - nStartIndex));
                        System.Console.WriteLine("\t\tColumnIndex = {0}, Column Data = {1}", currColumnIndex, this.CurrentLineData.Substring(nStartIndex, nEndIndex - nStartIndex));
                    }
                    else if (nEndIndex > nStartIndex + m_delimiter.Length)
                    {
                        //skip the delimiter if the start index is not zero
                        if(nStartIndex >0)
                            nStartIndex = nStartIndex + m_delimiter.Length;

                        currLineStrings.Add(currColumnIndex, this.CurrentLineData.Substring(nStartIndex, nEndIndex - nStartIndex));
                        System.Console.WriteLine("\t\tColumnIndex = {0}, Column Data = {1}", currColumnIndex, this.CurrentLineData.Substring(nStartIndex, nEndIndex - nStartIndex));
                    }

                    nStartIndex = nEndIndex;        //start the next check from current end point

                    if (bQuoteFound)
                    {
                        nStartIndex++;          // skip the ending quote
                    }
                    currColumnIndex++;

                }
                else
                    break;  // we have reached the end of the line
            }

            return currLineStrings;
        }

        private bool LineCanBeProcessed()
        {
            bool bRetVal = true;

            //Line needs to be skipped if this is a comment line
            // defined as per starting string sequence
            if (this.CurrentLineData.Substring(0, this.CommentLinePrefix.Length) == this.CommentLinePrefix)
            {
                System.Console.WriteLine("\t\tSkipping Current line as it is comment line.");
                bRetVal = false;
            }

            return bRetVal;

        }

        private int getNextNonSpaceCharIndex(int startIndex)
        {
            int currrStartIndex = startIndex;

            if (this.CurrentLineData.Length > currrStartIndex)
            {
                while ( this.CurrentLineData.Substring(currrStartIndex, 1) == " ")
                {
                    currrStartIndex++;

                    if (currrStartIndex >= this.CurrentLineData.Length)
                        break;  //break out of the loop if we have reached the end of the line
                }
            }
            return currrStartIndex;
        }
    }
}
