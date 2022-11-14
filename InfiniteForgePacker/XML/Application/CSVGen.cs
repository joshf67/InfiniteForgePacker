#pragma warning disable CS8600, CS8602, CS8604

using System.Xml.Linq;
using InfiniteForgeConstants.ObjectSettings;

namespace InfiniteForgePacker.XML.Application;

public static class CSVGen
{
    public static void SaveCSV(string xmlPath)
    {
        XDocument document = XDocument.Load(xmlPath);
        StreamWriter writer = new StreamWriter("CSVOutput");

        //Return all objects within an array and add them to a dictionary to have O(1) lookup for replacement arrays
        List<(XElement Element, int ObjectId)> allElements = XMLObject.ReturnObjectsOfIds(document);
        Dictionary<int, List<XElement>> objectIdsToElements = new Dictionary<int, List<XElement>>();

        allElements.Sort((a, b) => a.ObjectId.CompareTo(b.ObjectId));
        var lastObjecID = allElements[0].ObjectId;
        string currentCSV = "";
        foreach (var element in allElements)
        {
            if (element.ObjectId != lastObjecID)
            {
                writer.WriteLine(Enum.GetName(typeof(ObjectId), lastObjecID) + "," + currentCSV);
                currentCSV = "";
                lastObjecID = element.ObjectId;
            }
            else
            {
                if (currentCSV != "")
                    currentCSV += ",";
            }

            var gameObject = XMLObject.GenerateObjectFromXML(element.Element);
            
            //Size, positon, rotation
            // currentCSV += gameObject.GameObject.Transform.Scale.X == 0 ? 1 : gameObject.GameObject.Transform.Position.X;
            // currentCSV += ",";
            // currentCSV += gameObject.GameObject.Transform.Scale.Y == 0 ? 1 : gameObject.GameObject.Transform.Position.Y;;
            // currentCSV += ",";
            // currentCSV += gameObject.GameObject.Transform.Scale.Z == 0 ? 1 : gameObject.GameObject.Transform.Position.Z;;
            // currentCSV += ",";
            //
            // currentCSV += gameObject.GameObject.Transform.Position.X;
            // currentCSV += ",";
            // currentCSV += gameObject.GameObject.Transform.Position.Y;
            // currentCSV += ",";
            // currentCSV += gameObject.GameObject.Transform.Position.Z;
            // currentCSV += ",";
            
            currentCSV += gameObject.GameObject.Transform.EulerRotation.X;
            currentCSV += ",";
            currentCSV += gameObject.GameObject.Transform.EulerRotation.Y;
            currentCSV += ",";
            currentCSV += gameObject.GameObject.Transform.EulerRotation.Z;
        }
        writer.Close();
    }
}