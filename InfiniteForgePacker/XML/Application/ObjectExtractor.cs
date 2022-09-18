#pragma warning disable CS8600, CS8602, CS8604
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace InfiniteForgePacker.XML.Application;

public static class ObjectExtractor
{

    private static HashSet<string> UniqueObjects = new HashSet<string>();
    private const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    
    //Extract all objects from an XML and output it to a text file
    public static void Extract(string inPath, string outPath)
    {
        Console.WriteLine("Extraction starting");
        StreamWriter enumWriter = new(outPath);
        
        XDocument xml = XDocument.Load(inPath);

        if (xml?.Root is null)
            throw new Exception($"Unable to load XML document {inPath}");
        
        XContainer folder = XMLReader.GetXContainer(xml.Root, "struct", 6);

        if (folder is null)
            throw new Exception($"Document does not have a valid folder structure");
        
        XContainer mainFolderList = XMLReader.GetXContainer(folder, "list", 0);
        
        if (mainFolderList is null)
            throw new Exception($"Document probably has sub folders, this script runs on one main folder");

        IEnumerable<XElement> allObjects = XMLReader.GetXContainer(xml.Root, "list", 3)?.Elements();

        if (allObjects is null)
            throw new Exception($"Document does not have a valid object list");
        
        int extracting = 0;
        List<String> allStrings = new List<string>(5000);

        //Loop through all objects in folder and lookup related object index
        foreach (XElement mainFolder in mainFolderList.Elements())
        {
            XContainer subFolderList = XMLReader.GetXContainer(mainFolder, "list", 1);
            
            if (subFolderList is null)
                throw new Exception($"Document does not have a valid folder structure");

            foreach (XElement subFolder in subFolderList.Elements())
            {
                XContainer objectsInSubFolderList = XMLReader.GetXContainer(subFolder, "list", 7);
                
                if (objectsInSubFolderList is null)
                    throw new Exception($"Document does not have a valid folder structure");
        
                if (objectsInSubFolderList != null)
                {
                    foreach (XElement obj in objectsInSubFolderList.Elements())
                    {
                        XElement nodeId = XMLReader.GetXElement(obj, "uint16", 8);
                        if (nodeId != null)
                        {
                            XElement nodeName = XMLReader.GetXElement(obj, "wstring", 2);
                            XElement nodeObjectParent = allObjects.ElementAt(int.Parse(nodeId.Value));
                            XElement nodeObjectId = XMLReader.GetXContainer(nodeObjectParent, "struct", 2);
                            XElement nodeObjectIdId = XMLReader.GetXElement(nodeObjectId, "int32");

                            extracting++;
                            allStrings.Add($"{ConvertString(nodeName.Value)} = {nodeObjectIdId.Value},");
                        }
                    }
                }

                enumWriter.Flush();
            }
        }

        allStrings.Sort((a, b) => string.CompareOrdinal(a[..(a.IndexOf("=", StringComparison.Ordinal))], b[..(b.IndexOf("=", StringComparison.Ordinal))]));

        foreach (var str in allStrings)
        {
            enumWriter.WriteLine(str);
        }
        
        enumWriter.Close();
        Console.WriteLine($"Extraction complete, {extracting} objects have been extracted");
    }

    //Handles converting strings into enumable strings
    private static string ConvertString(string str)
    {
        string ret = Regex.Replace(Regex.Replace(str.ToUpper(), @"[^a-zA-Z0-9]", " "), @"\s+", "_");

        if (ret[0].ToString() == "_")
            ret = ret[1..];
            
        if (int.TryParse(ret[0].ToString(), out int discard))
            ret = "_" + ret;
        
        ret = ret.Replace("_X", "_X_");

        int uniqueStirng = 0;
        if (UniqueObjects.Contains(ret))
        {
            ret += "_A";
        }
        
        while (UniqueObjects.Contains(ret))
        {
            uniqueStirng++;
            ret = ret.Substring(0, ret.Length - 2) + "_" + Letters[uniqueStirng];
        }
        
        UniqueObjects.Add(ret);

        ret = Regex.Replace(ret, @"_+", "_");

        return ret;
    }
    
}