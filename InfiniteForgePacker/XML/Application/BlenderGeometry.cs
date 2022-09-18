using System.Numerics;
using System.Xml.Linq;
using InfiniteForgeConstants.ObjectSettings;
using InfiniteForgePacker.XML.Object;

namespace InfiniteForgePacker.XML.Application;

public static class BlenderGeometry
{
    private static readonly float TransformOffset = (float) 3.28084 / 10;
    
    public static void Convert(string inPath, string outPath, bool metric = false)
    {
        
        var sr = new StreamReader(inPath);
        string[] positions = sr.ReadToEnd().Split(",");
        sr.Close();

        XDocument document = XDocument.Load(outPath);
        int i = 0;

        foreach (var position in positions)
        {
    
            if (position == "") continue;

            string[] pos = position.Split(" ");
            float posX = float.Parse(pos[0]) * (metric ? 1 : TransformOffset);
            float posY = float.Parse(pos[1]) * (metric ? 1 : TransformOffset);
            float posZ = float.Parse(pos[2]) * (metric ? 1 : TransformOffset);
    
            XMLHelper.AddObject(document, 
                new XMLObject(
                    new GameObject("Vertex " + i, 
                        ObjectId.FORERUNNER_CONE,
                        new Transform(new Vector3(posX, posY, posZ))
                        )
                    )
                );
            i++;
        }

        document.Save(outPath);
    }
}