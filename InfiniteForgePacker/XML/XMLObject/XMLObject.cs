using System.Numerics;
using System.Xml.Linq;
using InfiniteForgeConstants.ObjectSettings;

namespace InfiniteForgePacker.XML;

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
        
        var position = XMLReader.GetVector3(objectContainer, 3);

        if (position is null)
            return Vector3.Zero;
        
        return new Vector3(position.Value.x == null ? 0 : float.Parse(position.Value.x.Value),
            position.Value.y == null ? 0 : float.Parse(position.Value.y.Value),
            position.Value.z == null ? 0 : float.Parse(position.Value.z.Value));
    }

    public static (Vector3 Forward, Vector3 Up, Vector3 Degrees) ReadObjectRotation(XContainer objectContainer)
    {
        if (objectContainer is null)
            throw new Exception("Invalid object container when trying to read object rotation");
        
        
        var upRotation = XMLReader.GetVector3(objectContainer, 4);

        var upVec = Vector3.Zero;
        if (upRotation is not null)
            upVec = new Vector3(upRotation.Value.x == null ? 0 : float.Parse(upRotation.Value.x.Value),
                upRotation.Value.y == null ? 0 : float.Parse(upRotation.Value.y.Value),
                upRotation.Value.z == null ? 0 : float.Parse(upRotation.Value.z.Value));
        
        var forwardRotation = XMLReader.GetVector3(objectContainer, 5);

        var forwardVec = Vector3.Zero;
        if (forwardRotation is not null)
            upVec = new Vector3(forwardRotation.Value.x == null ? 0 : float.Parse(forwardRotation.Value.x.Value),
                forwardRotation.Value.y == null ? 0 : float.Parse(forwardRotation.Value.y.Value),
                forwardRotation.Value.z == null ? 0 : float.Parse(forwardRotation.Value.z.Value));

        return (forwardVec, upVec, Transform.DirectionToEuler(forwardVec, upVec).Degrees);
    }

    public static XElement? ReadObjectAdditionalData(XContainer objectContainer, bool createIfNull = false, bool clearOnFind = false)
    {
        if (objectContainer is null)
            throw new Exception("Invalid object container when trying to read object additional data");
        
        return XMLReader.GetXContainer(objectContainer, "struct", 8, createIfNull: createIfNull, clearOnFind: clearOnFind);
    }
    
    public static Vector3? ReadObjectScale(XContainer objectContainer)
    {
        if (objectContainer is null)
            throw new Exception("Invalid object container when trying to read object scale");

        var additionalDataStruct = ReadObjectAdditionalData(objectContainer);

        if (additionalDataStruct is null)
            return null;
        
        var scaleList = XMLReader.GetXContainer(additionalDataStruct, "list", 23);
        
        if (scaleList is null)
            return null;
        
        var scale = XMLReader.GetVector3(scaleList);

        if (scale is null)
            return null;
        
        return new Vector3(scale.Value.x == null ? 0 : float.Parse(scale.Value.x.Value),
            scale.Value.y == null ? 0 : float.Parse(scale.Value.y.Value),
            scale.Value.z == null ? 0 : float.Parse(scale.Value.z.Value));
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

    public void WriteObject( XContainer container, XDocument? document = null)
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

        XMLWriter.WriteVector3ToContainer(container, GameObject.Transform.Position, 3);
    }

    public void WriteObjectRotation(XContainer container)
    {
        if (container is null)
            throw new Exception("Invalid object container when trying to write object rotation");

        var rotationDirections = GameObject.Transform.DirectionVectors;

        XMLWriter.WriteVector3ToContainer(container, rotationDirections.Up, 4);
        XMLWriter.WriteVector3ToContainer(container, rotationDirections.Forward, 5);
    }
    
    public void WriteObjectScale(XContainer container)
    {
        if (container is null)
            throw new Exception("Invalid object container when trying to write object scale");

        if (GameObject.Transform.IsStatic)
        {
            var objectDataScaleList =
                XMLReader.GetXContainer(ReadObjectAdditionalData(container, true, true),
                    "list", 23, "struct", createIfNull: true, clearOnFind: true);

            XMLWriter.WriteVector3ToContainer(objectDataScaleList, GameObject.Transform.Scale);
        }
        else
        {
            throw new NotImplementedException("Dyanmic objects are not supported yet");
        }
    }

    public virtual void WriteObjectSpecifics(XDocument? document = null, XContainer? objectContainer = null)
    {
        
    }
}