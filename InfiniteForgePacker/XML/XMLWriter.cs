using System.Numerics;
using System.Xml.Linq;

namespace InfiniteForgePacker.XML;

public static class XMLWriter
{
    //Creates an XElement using WriteObject(typeof()) and adds it as a child to container, returns the new XElement
    public static XElement WriteObjectToContainer(XContainer container, Type type, int id = -1)
    {
        XElement ret = WriteObject(type, id);
        container.Add(ret);
        return ret;
    }
    
    //Creates an XElement using WriteObject(value) and adds it as a child to container, returns the new XElement
    public static XElement WriteObjectToContainer(XContainer container, object value, int id = -1)
    {
        XElement ret = WriteObject(value, id);
        container.Add(ret);
        return ret;
    }

    //Creates an XElement using typeof(), applies Id if given and returns it
    public static XElement WriteObject(Type type, int id = -1)
    {
        XElement? ret = null;
        if (type == typeof(bool))
        {
            ret = new XElement("bool");
        }
        else if (type == typeof(ushort))
        {
            ret = new XElement("uint16");
        }
        else if (type == typeof(uint))
        {
            ret = new XElement("uint32");
        }
        else if (type == typeof(int))
        {
            ret = new XElement("int32");
        }
        else if (type == typeof(string))
        {
            ret = new XElement("wstring");
        }
        else if (type == typeof(float))
        {
            ret = new XElement("float");
        }
        else
            throw new NotImplementedException();

        if (id is not -1)
            ret.SetAttributeValue("id", id.ToString());
        
        return ret;
    }
    
    //Creates an XElement using value, applies Id if given and returns it
    public static XElement WriteObject(object value, int id = -1)
    {
        XElement ret = value switch
        {
            bool => new XElement("bool"),
            ushort => new XElement("uint16"),
            uint => new XElement("uint32"),
            int => new XElement("int32"),
            string => new XElement("wstring"),
            float => new XElement("float"),
            _ => throw new NotImplementedException()
        };
        
        ret.SetValue(value);
        
        if (id is not -1)
            ret.SetAttributeValue("id", id.ToString());
        
        return ret;
    }

    //Creates an XElement of type struct, adds three float values, adds it as a child to container and returns it
    public static XElement WriteVector3ToContainer(XContainer container, Vector3 vector, int id = -1)
    {
        XElement? ret = XMLReader.GetXContainer(container, "struct", id, clearOnFind: true);

        //If container doesn't exist proceed as normal
        if (ret is null)
        {
            ret = WriteVector3(vector, id);
            container.Add(ret);
            return ret;
        }

        //Otherwise setup all the valid values
        WriteObjectToContainer(ret, vector.X, 0);
        WriteObjectToContainer(ret, vector.Y, 1);
        WriteObjectToContainer(ret, vector.Z, 2);
        return ret;
    }

    //Creates an XElement of type struct, adds three float values and returns it
    public static XElement WriteVector3(Vector3 vector, int id = -1)
    {
        var ret = WriteStruct(id);
        WriteObjectToContainer(ret, vector.X, 0);
        WriteObjectToContainer(ret, vector.Y, 1);
        WriteObjectToContainer(ret, vector.Z, 2);
        return ret;
    }
    
    //Creates an XElement of type struct, adds two float values, adds it as a child to container and returns it
    public static XElement WriteVector2ToContainer(XContainer container, Vector2 vector, int id = -1)
    {
        XElement? ret = XMLReader.GetXContainer(container, "struct", id, clearOnFind: true);
        
        //If container doesn't exist proceed as normal
        if (ret is null)
        {
            ret = WriteVector2(vector, id);
            container.Add(ret);
            return ret;
        }

        //Otherwise setup all the valid values
        WriteObjectToContainer(ret, vector.X, 0);
        WriteObjectToContainer(ret, vector.Y, 1);
        return ret;
    }
    
    //Creates an XElement of type struct, adds two float values and returns it
    public static XElement WriteVector2(Vector2 vector, int id = -1)
    {
        var ret = WriteStruct(id);
        WriteObjectToContainer(ret, vector.X, 0);
        WriteObjectToContainer(ret, vector.Y, 1);
        return ret;
    }

    //Creates an XElement of type struct, applies Id, adds it as a child to container and returns the new XElement
    public static XElement WriteStructToContainer(XContainer container, int id = -1)
    {
        XElement ret = WriteStruct(id);
        container.Add(ret);
        return ret;
    }

    //Creates an XElement of type struct, applies Id if given and returns it
    public static XElement WriteStruct(int id = -1)
    {
        XElement ret = new XElement("struct");
        if (id != -1)
            ret.SetAttributeValue("id", id.ToString());
        return ret;
    }
    
    //Creates an XElement of type list, applies Id and list type, adds it as a child to container and returns the new XElement
    public static XElement WriteListToContainer(XContainer container, int id = -1, string type = "")
    {
        XElement ret = WriteList(id, type);
        container.Add(ret);
        return ret;
    }
    
    //Creates an XElement of type list, applies Id and list type if given and returns it
    public static XElement WriteList(int id = -1, string type = "")
    {
        XElement ret = new XElement("list");
        
        if (id != -1)
            ret.SetAttributeValue("id", id.ToString());
        
        if (type != "")
            ret.SetAttributeValue("type", type);
        
        return ret;
    }
}