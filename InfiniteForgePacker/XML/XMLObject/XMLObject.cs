using System.Numerics;
using System.Xml.Linq;
using InfiniteForgeConstants.ObjectSettings;

namespace InfiniteForgePacker.XML.Object;

public class XMLObject
{
    public readonly GameObject GameObject;

    public XMLObject(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    public static XMLObject? GenerateObjectFromXML(XContainer objectContainer)
    {
        if (objectContainer is null)
            throw new Exception("Invalid object container when trying to create XMLObject from XML");
        
        var isStatic = false; //Temporary for now until we can determine what makes an object static in XML
        var id = ReadObjectId(objectContainer);
        var position = ReadObjectPosition(objectContainer);
        var rotation = ReadObjectRotation(objectContainer).Degrees;
        var scale = ReadObjectScale(objectContainer);
        
        return new XMLObject(new GameObject(null, (ObjectId)id, new Transform(position, rotation, isStatic, scale)));
    }

    public static int ReadObjectId(XContainer objectContainer)
    {
        if (objectContainer is null)
            throw new Exception("Invalid object container when trying to read object id");
        
        var id = XMLReader.GetXContainer(objectContainer, "struct", 2);
        if (id is null)
            return -1;

        return int.Parse(id.Value);
    }

    public static Vector3 ReadObjectPosition(XContainer objectContainer)
    {
        if (objectContainer is null)
            throw new Exception("Invalid object container when trying to read object position");
        
        var position = XMLReader.GetXContainer(objectContainer, "struct", 3);
        if (position is null)
            return Vector3.Zero;

        var x = XMLReader.GetXElement(position, 0);
        var y = XMLReader.GetXElement(position, 1);
        var z = XMLReader.GetXElement(position, 2);

        return new Vector3(x == null ? 0 : float.Parse(x.Value), y == null ? 0 : float.Parse(y.Value),
            z == null ? 0 : float.Parse(z.Value));
    }

    public static (Vector3 Forward, Vector3 Up, Vector3 Degrees) ReadObjectRotation(XContainer objectContainer)
    {
        if (objectContainer is null)
            throw new Exception("Invalid object container when trying to read object rotation");
        
        var objectUpRotationContainer = XMLReader.GetXContainer(objectContainer, "struct", 4);
        var objectForwardRotationContainer = XMLReader.GetXContainer(objectContainer, "struct", 5);

        var upVec = Vector3.Zero;
        if (objectUpRotationContainer is not null)
        {
            var x = XMLReader.GetXElement(objectUpRotationContainer, 0);
            var y = XMLReader.GetXElement(objectUpRotationContainer, 1);
            var z = XMLReader.GetXElement(objectUpRotationContainer, 2);

            upVec = new Vector3(x == null ? 0 : float.Parse(x.Value), y == null ? 0 : float.Parse(y.Value),
                z == null ? 0 : float.Parse(z.Value));
        }
        
        var forwardVec = Vector3.Zero;
        if (objectForwardRotationContainer is not null)
        {
            var x = XMLReader.GetXElement(objectForwardRotationContainer, 0);
            var y = XMLReader.GetXElement(objectForwardRotationContainer, 1);
            var z = XMLReader.GetXElement(objectForwardRotationContainer, 2);

            forwardVec = new Vector3(x == null ? 0 : float.Parse(x.Value), y == null ? 0 : float.Parse(y.Value),
                z == null ? 0 : float.Parse(z.Value));
        }

        return (forwardVec, upVec, Transform.DirectionToEuler(forwardVec, upVec).Degrees);
    }

    public static XElement? ReadObjectAdditionalData(XContainer objectContainer, bool createIfNull = false, bool clearOnFind = false)
    {
        if (objectContainer is null)
            throw new Exception("Invalid object container when trying to read object additional data");
        
        return XMLReader.GetXContainer(objectContainer, "struct", 8, createIfNull: createIfNull, clearOnFind: clearOnFind);
    }
    
    public static Vector3 ReadObjectScale(XContainer objectContainer)
    {
        if (objectContainer is null)
            throw new Exception("Invalid object container when trying to read object scale");
        
        var position = XMLReader.GetXContainer(objectContainer, "struct", 3);
        if (position is null)
            return Vector3.One;

        var x = XMLReader.GetXElement(position, 0);
        var y = XMLReader.GetXElement(position, 1);
        var z = XMLReader.GetXElement(position, 2);

        return new Vector3(x == null ? 0 : float.Parse(x.Value), y == null ? 0 : float.Parse(y.Value),
            z == null ? 0 : float.Parse(z.Value));
    }

    public static List<(XElement Element, int ObjectId)>? ReturnObjectsOfIds(XDocument document, int id = -1)
    {
        if (document is null)
            throw new Exception("Invalid document when trying to return all objects");
        
        XContainer objectList = XMLHelper.GetObjectList(document);

        if (objectList is null)
            return null;

        if (id == -1)
            //Converts all elements into tuple format
            return objectList.Elements().Select(el => (el, ReadObjectId(el))).ToList();

        var ret = new List<(XElement Element, int ObjectId)>();
        foreach (var obj in objectList.Elements())
        {
            if (ReadObjectId(obj) == id)
                ret.Add((obj, id));
        }

        return ret;
    }

    public void WriteObject(XContainer container, XDocument? document = null)
    {
        if (container is null)
            throw new Exception("Invalid object container when trying to write object");

        WriteObjectId(container);
        WriteObjectPosition(container);
        WriteObjectRotation(container);
        WriteObjectScale(container);
        
        if (document is not null)
            WriteObjectSpecifics(document, container);
    }
    
    public void WriteObjectId(XContainer container)
    {
        if (container is null)
            throw new Exception("Invalid object container when trying to write object id");
        
        var objectIdContainer = XMLReader.GetXContainer(container, "struct", 2, createIfNull: true, clearOnFind: true);
        XMLWriter.WriteObjectToContainer(objectIdContainer, GameObject.ObjectId, 0);
    }

    public void WriteObjectPosition(XContainer container)
    {
        if (container is null)
            throw new Exception("Invalid object container when trying to write object position");
        
        var objectPositionContainer =
            XMLReader.GetXContainer(container, "struct", 3, createIfNull: true, clearOnFind: true);

        XMLWriter.WriteObjectToContainer(objectPositionContainer, GameObject.Transform.Position.X, 0);
        XMLWriter.WriteObjectToContainer(objectPositionContainer, GameObject.Transform.Position.Y, 1);
        XMLWriter.WriteObjectToContainer(objectPositionContainer, GameObject.Transform.Position.Z, 2);
    }

    public void WriteObjectRotation(XContainer container)
    {
        if (container is null)
            throw new Exception("Invalid object container when trying to write object rotation");
        
        var objectUpRotationContainer =
            XMLReader.GetXContainer(container, "struct", 4, createIfNull: true, clearOnFind: true);
        
        var objectForwardRotationContainer =
            XMLReader.GetXContainer(container, "struct", 5, createIfNull: true, clearOnFind: true);

        var rotationDirections = GameObject.Transform.DirectionVectors;

        XMLWriter.WriteObjectToContainer(objectUpRotationContainer, rotationDirections.Up.X, 0);
        XMLWriter.WriteObjectToContainer(objectUpRotationContainer, rotationDirections.Up.Y, 1);
        XMLWriter.WriteObjectToContainer(objectUpRotationContainer, rotationDirections.Up.Z, 2);
        
        XMLWriter.WriteObjectToContainer(objectForwardRotationContainer, rotationDirections.Forward.X, 0);
        XMLWriter.WriteObjectToContainer(objectForwardRotationContainer, rotationDirections.Forward.Y, 1);
        XMLWriter.WriteObjectToContainer(objectForwardRotationContainer, rotationDirections.Forward.Z, 2);
    }
    
    public void WriteObjectScale(XContainer container)
    {
        if (container is null)
            throw new Exception("Invalid object container when trying to write object scale");
        
        var objectDataScaleList =
            XMLReader.GetXContainer(ReadObjectAdditionalData(container, true, true),
                "list", 23, "struct", createIfNull: true, clearOnFind: true);

        var objectScaleStruct = XMLReader.GetXContainer(objectDataScaleList, "struct", -1,
            createIfNull: true, clearOnFind: true);
        
        var objectScaleFloatStruct = XMLReader.GetXContainer(objectScaleStruct, "struct", 0,
            createIfNull: true, clearOnFind: true);

        XMLWriter.WriteObjectToContainer(objectScaleFloatStruct, GameObject.Transform.Scale.X, 0);
        XMLWriter.WriteObjectToContainer(objectScaleFloatStruct, GameObject.Transform.Scale.Y, 1);
        XMLWriter.WriteObjectToContainer(objectScaleFloatStruct, GameObject.Transform.Scale.Z, 2);
        
        // var randomContainer =
        //     XMLReader.GetXContainer(objectScaleContainer, "list", 24, "struct", createIfNull: true, clearOnFind: true);
        // var randomStruct = XMLWriter.WriteStructToContainer(randomContainer);
        //
        // XMLWriter.WriteObjectToContainer(randomStruct, 0, 2);
        //
        // var randomStructInside = XMLReader.GetXContainer(randomStruct, "struct", 1, createIfNull: true, clearOnFind: true);
        // XMLWriter.WriteObjectToContainer(randomStructInside, 0, 0);
        //
        // XMLWriter.WriteObjectToContainer(randomStruct, 2, 1);
    }

    public virtual void WriteObjectSpecifics(XDocument? document = null, XContainer? objectContainer = null)
    {
        
    }
}