using System.Numerics;
using System.Xml;
using System.Xml.Linq;
using InfiniteForgePacker.XML.Object;

namespace InfiniteForgePacker.XML;

public static class XMLHelper
{
    //Make sure you run this unless you can guarantee that everything is ordered correctly
    public static XDocument EnsureSchemaOrdering(XDocument document)
    {
        if (document?.Root == null)
            throw new Exception("Cannot add object to invalid document.");
        
        return new XDocument(
            document.Declaration,
            from child in document.Nodes()
            where child.NodeType != XmlNodeType.Element
            select child,
            SortByID(document.Root));
    }

    private static XElement SortByID(XElement element)
    {

        if (!element.HasElements) return element;
        
        List<XElement> children = new List<XElement>();

        foreach (var child in element.Elements())
        {
            children.Add(SortByID(child));
        }
        
        children.Sort((a, b) =>
        {
            var aId = a.Attribute("id");
            var bId = b.Attribute("id");
            if (aId is null || bId is null) return 0;
            return int.Parse((string)aId) - int.Parse((string)bId);
        });

        return new XElement(element.Name, element.Attributes(), children);
    }
    
    public static XContainer GetMapDataStruct(XDocument document)
    {
        if (document?.Root == null)
            throw new Exception("Cannot Get Map Data from invalid document.");
        
        XContainer? mapData = XMLReader.GetXContainer(document.Root, "struct", 1, createIfNull: true);
        if (mapData is null)
            throw new Exception($"Unable to find/create map data struct inside document {document}");

        return mapData;
    }

    public static XContainer GetObjectList(XDocument document)
    {
        if (document?.Root == null)
            throw new Exception("Cannot Get Object List from invalid document.");
        
        XContainer? objectList = XMLReader.GetXContainer(document.Root, "list", 3, "struct", true);
        if (objectList is null)
            throw new Exception($"Unable to find/create object list inside document {document}");

        return objectList;
    }
    
    public static XContainer GetFolderStruct(XDocument document)
    {
        if (document?.Root == null)
            throw new Exception("Cannot Get Folder Structure from invalid document.");
        
        XContainer? folderStruct = XMLReader.GetXContainer(document.Root, "struct", 6, createIfNull: true);
        if (folderStruct is null)
            throw new Exception($"Unable to find/create folder struct inside document {document}");

        return folderStruct;
    }
    
    public static void AddObject(XDocument document, XMLObject obj)
    {
        XElement xmlObject = XMLWriter.WriteStructToContainer(GetObjectList(document));
        
        obj.WriteObjectId(xmlObject);
        obj.WriteObjectPosition(xmlObject);
        obj.WriteObjectScale(xmlObject);
        obj.WriteObjectSpecifics(document, xmlObject);
    }
}