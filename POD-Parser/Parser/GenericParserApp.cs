using System;
using System.IO;

public class GenericParserApp
{
    private GenericParserApp()
	{
    }

    public GenericParserApp(string delimiter, string commentLinePrefix)
    {
        m_delimiter = delimiter;
        m_commentLinePrefix = commentLinePrefix;
        m_currentColumnValue = "";
        m_currentLineData = "";
    }

    # region Private Data
    private string m_delimiter;
    private string m_commentLinePrefix;
    private string m_currentColumnValue;
    private string m_currentLineData;


    # endregion Private Data

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

    
    static void Main(string[] args)
    {
        int i = 55;

        System.Console.WriteLine("This is a test app {0}", i );

        bool bTestMode = false;
        # region TestMode
        string strTestFilePath = "C:\\Projects\\POD\\InputFiles\\LineHaul";
        string strTestFileName = "SampleMockup-01.txt";
        string strTestFileNameWithFullPath = strTestFilePath + "\\" + strTestFileName;
        
        if (bTestMode)
        {
            GenericParserApp theApp = new GenericParserApp(",", "#");


            if (File.Exists(strTestFileNameWithFullPath))
            {
                System.Console.WriteLine("File name, {0}, exists.", strTestFileNameWithFullPath);

                TextReader currTxtReader = new StreamReader(strTestFileNameWithFullPath);
                theApp.CurrentLineData = currTxtReader.ReadLine();
                int currLineNum = 1;
                while (theApp.CurrentLineData != null && theApp.CurrentLineData.Length > 0)
                {
                    System.Console.WriteLine("\tLine #{0} = {1}", currLineNum, theApp.CurrentLineData);
                    theApp.ProcessLine();

                    theApp.CurrentLineData = currTxtReader.ReadLine();
                    ++currLineNum;
                }
            }
            else
            {
                System.Console.WriteLine("File does not Exist");

            }
        }
        # endregion TestMode

       
        string strPODFilePath = "C:\\Projects\\POD\\InputFiles\\LineHaul";
        string strPODFileName = "LinexSamplePODFile-20121214-CSV.csv";
        string strPODFileNameWithFullPath = strPODFilePath + "\\" + strPODFileName;

        Parsers.LinexDeliveryRunsheetParser lnxParser = new Parsers.LinexDeliveryRunsheetParser(",", "#",1,"sohalr");

        System.Console.WriteLine("Parsing File = {0}", strTestFileNameWithFullPath); 
        lnxParser.ParseShipments(new StreamReader(strTestFileNameWithFullPath));

        System.Console.WriteLine("Parsing File = {0}", strTestFileNameWithFullPath);
        lnxParser.ParseShipments(new StreamReader(strPODFileNameWithFullPath));



        return;
    }


    public bool ProcessLine()
    {
        this.CurrentLineData= this.CurrentLineData.Trim();


        int nStartIndex = 0;
        int nEndIndex = nStartIndex;

        if (LineCanBeProcessed())
        {
            // check if the first char is a double quote '"'
            // if yes, find the next one, which would indicate the end of that colum

            //Shipment Ref
            if (this.CurrentLineData.Substring(nStartIndex, 1) == "\"")
            {
                nEndIndex = this.CurrentLineData.IndexOf('"', nStartIndex + 1);

            }
            else
            {
                nEndIndex = this.CurrentLineData.IndexOf(m_delimiter, nStartIndex + 1);
                if (nEndIndex == -1)
                    nEndIndex = this.CurrentLineData.Length;
            }

            System.Console.WriteLine("\t\tShipment Ref = {0}", this.CurrentLineData.Substring(nStartIndex, nEndIndex - nStartIndex));
            
        }

        return true;
    }

    public bool ReadNextColumn(string strLineData, int nCurrIndex)
    {
        // saves the character data upto the next delimiter, starting from nCurrIndex
        // 1) Find the first non space char


        return true;
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
}
