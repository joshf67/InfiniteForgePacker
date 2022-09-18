#pragma warning disable CS8600, CS8602, CS8604

using System.Numerics;
using System.Xml.Linq;
using InfiniteForgeConstants.ObjectSettings;
using InfiniteForgePacker.XML.Object;

namespace InfiniteForgePacker.XML.Application;

public static class ReplaceType
{
    //Glorified find and replace all XML script
    public static void Convert(string xmlPath, (ObjectId typeToReplace, ObjectId replacement)[] replacementArray)
    {
        XDocument document = XDocument.Load(xmlPath);

        //Return all objects within an array and add them to a dictionary to have O(1) lookup for replacement arrays
        List<(XElement Element, int ObjectId)> allElements = XMLObject.ReturnObjectsOfIds(document);
        Dictionary<int, List<XElement>> objectIdsToElements = new Dictionary<int, List<XElement>>();

        foreach (var element in allElements)
        {
            if (objectIdsToElements.ContainsKey(element.ObjectId))
            {
                objectIdsToElements[element.ObjectId].Add(element.Element);
            }
            else
            {
                var elements = new List<XElement>();
                elements.Add(element.Element);
                objectIdsToElements.Add(element.ObjectId, elements);
            }
        }

        //Loop through all replacements to be made and rewrite ID struct
        foreach (var replacement in replacementArray)
        {
            if (objectIdsToElements.ContainsKey((int)replacement.typeToReplace))
            {
                foreach (var element in objectIdsToElements[(int)replacement.typeToReplace])
                {
                    var objectIdContainer = XMLReader.GetXContainer(element, "struct", 2, createIfNull: true, clearOnFind: true);
                    XMLWriter.WriteObjectToContainer(objectIdContainer, (int)replacement.replacement, 0);
                }
            }
        }

        document.Save(xmlPath);
    }
}