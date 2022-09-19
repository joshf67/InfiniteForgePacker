#pragma warning disable CS8600

using System.Numerics;
using System.Xml.Linq;

namespace InfiniteForgePacker.XML;

public static class XMLReader
{

    //Get an element using typeof() or exposes option to create them if they don't exist
    public static XElement? GetXElement(XContainer parent, Type type, int id = -1, bool createIfNull = false)
    {
        return GetXElement(parent, type.Name, id, createIfNull);
    }
    
    //Get an element using value or exposes option to create them if they don't exist
    public static XElement? GetXElement(XContainer parent, object type, int id = -1, bool createIfNull = false)
    {
        return GetXElement(parent, type.GetType().Name, id, createIfNull);
    }

    //Get an element using string type or exposes option to create them if they don't exist
    public static XElement? GetXElement(XContainer parent, string type, int id = -1, bool createIfNull = false)
    {
        if (parent is null)
            throw new Exception("Cannot get element when parent is null");
        
        if (type == "")
            throw new Exception($"Type is invalid {type}");

        var ret = id != -1 ? 
            (from el in parent.Elements(type)
                where (string) el.Attribute("id") == id.ToString()
                select el).FirstOrDefault() 
            : parent.Elements(type).FirstOrDefault();

        if (ret == null && !createIfNull) return ret;
        return XMLWriter.WriteObjectToContainer(parent, type, id);
    }
    
    //Get a struct/list or exposes option to create them if they don't exist
    public static XElement? GetXContainer(XContainer parent, string type, int id = -1, string listType = "", bool createIfNull = false, bool clearOnFind = false)
    {
        if (parent is null)
            throw new Exception("Cannot get container when parent is null");
        
        if (type == "")
            throw new Exception($"Type is invalid {type}");
        
        var ret = id != -1 ? 
            (from el in parent.Elements(type)
                where (string) el.Attribute("id") == id.ToString()
                select el).FirstOrDefault() 
            : parent.Elements(type).FirstOrDefault();

        switch (ret)
        {
            case null when !createIfNull:
                return null;
            case null when createIfNull:
                ret = type switch
                {
                    "struct" => XMLWriter.WriteStruct(id),
                    "list" => XMLWriter.WriteList(id, listType),
                    _ => throw new NotImplementedException()
                };
            
                parent.Add(ret);
                break;
            case not null when clearOnFind:
                ret.Nodes().Remove();
                break;
        } 

        return ret;
    }
    
    public static (XElement? parent, XElement? x, XElement? y, XElement? z)? GetVector3(XContainer parent, int id = -1,
        bool createIfNull = false)
    {
        if (parent is null)
            throw new Exception("Cannot get vector3 when parent is null");
        
        XElement? vectorContainer = GetXContainer(parent, "struct", id);

        if (vectorContainer is null)
        {
            if (createIfNull is false)
                return null;

            vectorContainer = XMLWriter.WriteVector3ToContainer(parent, Vector3.Zero);
        }

        XElement? x = GetXElement(vectorContainer, typeof(float), 0);
        XElement? y = GetXElement(vectorContainer, typeof(float), 1);
        XElement? z = GetXElement(vectorContainer, typeof(float), 2);
        
        return (vectorContainer, x, y, z);
    }
    
    public static (XElement? parent, XElement? x, XElement? y)? GetVector2(XContainer parent, int id = -1,
        bool createIfNull = false)
    {
        if (parent is null)
            throw new Exception("Cannot get vector2 when parent is null");
        
        XElement? vectorContainer = GetXContainer(parent, "struct", id);

        if (vectorContainer is null)
        {
            if (createIfNull is false)
                return null;

            vectorContainer = XMLWriter.WriteVector2ToContainer(parent, Vector2.Zero);
        }

        XElement? x = GetXElement(vectorContainer, typeof(float), 0);
        XElement? y = GetXElement(vectorContainer, typeof(float), 1);
        
        return (vectorContainer, x, y);
    }
}