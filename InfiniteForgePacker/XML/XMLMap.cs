using System.Xml.Linq;
using InfiniteForgeConstants;
using InfiniteForgeConstants.MapSettings;
using InfiniteForgeConstants.MapSettings.AmbientSound;
using InfiniteForgePacker.XML.Object;

namespace InfiniteForgePacker.XML;

public class XMLMap
{
    public Map Map;

    public XMLMap(Map map)
    {
        Map = map;
    }

    public static XMLMap? GenerateMapFromXML(XContainer mapContainer)
    {
        if (mapContainer is null)
            throw new Exception("Invalid object container when trying to create XMLMap from XML");

        var mapId = ReadMapId(mapContainer);
        var mapDecorators = ReadMapDecorators(mapContainer);
        var mapScreenEffectId = ReadMapScreenEffectId(mapContainer);
        var mapOptions = ReadMapOptions(mapContainer);
        var mapAmbientSound = ReadMapAmbientSound(mapContainer);
        
        return new XMLMap(new Map((MapId)mapId, mapDecorators, (ScreenEffectId)mapScreenEffectId, mapOptions, mapAmbientSound, null));
    }

    public static void WriteMap(XDocument document, Map map)
    {
        var mapContainer = XMLHelper.GetMapDataStruct(document);
        WriteMapId(mapContainer, map);
        WriteMapScreenEffectId(mapContainer, map);

        if (map.Options is not null)
        {
            var mapOptionsContainer = XMLReader.GetXContainer(mapContainer, "struct", 39);
            XMLMapOptions.WriteMapOptions(mapOptionsContainer, map.Options);
        }

        WriteMapAmbientSound(mapContainer, map);
    }

    public static int? ReadMapId(XContainer mapContainer)
    {
        if (mapContainer is null)
            throw new Exception("Invalid map container when trying to read map id");

        var mapIdContainer = XMLReader.GetXContainer(mapContainer, "struct", 0);

        if (mapIdContainer is null)
            return null;

        var mapIdFloat = XMLReader.GetXElement(mapIdContainer, typeof(float), 0);

        return int.Parse(mapIdFloat.Value);
    }

    public static bool? ReadMapDecorators(XContainer mapContainer)
    {
        if (mapContainer is null)
            throw new Exception("Invalid map container when trying to read map decorators");
        
        var mapDecoratorBool = XMLReader.GetXElement(mapContainer, typeof(bool), 25);

        if (mapDecoratorBool is null)
            return null;
        
        return bool.Parse(mapDecoratorBool.Value);
    }

    public static int? ReadMapScreenEffectId(XContainer mapContainer)
    {
        if (mapContainer is null)
            throw new Exception("Invalid map container when trying to read map screen effect");
        
        var mapScreenEffectContainer = XMLReader.GetXContainer(mapContainer, "struct", 34);

        if (mapScreenEffectContainer is null)
            return null;
        
        var mapScreenEffectId = XMLReader.GetXElement(mapScreenEffectContainer, typeof(int), 0);

        if (mapScreenEffectId is null)
            return null;

        return int.Parse(mapScreenEffectId.Value);
    }

    public static MapOptions? ReadMapOptions(XContainer mapContainer)
    {
        if (mapContainer is null)
            throw new Exception("Invalid map container when trying to read map ambient sound");
        
        var mapOptionsContainer = XMLReader.GetXContainer(mapContainer, "struct", 39);

        if (mapOptionsContainer is null)
            return null;
        
        var ret = new MapOptions();

        var sunlightOptions = XMLMapOptions.ReadSunlightOptions(mapOptionsContainer);
        if (sunlightOptions is not null)
            ret.Sunlight = sunlightOptions;
        
        var lightBounceOptions = XMLMapOptions.ReadLightBounceOptions(mapOptionsContainer);
        if (lightBounceOptions is not null)
            ret.LightBounce = lightBounceOptions;
        
        var skylightOptions = XMLMapOptions.ReadSkylightOptions(mapOptionsContainer);
        if (skylightOptions is not null)
            ret.SkyLight = skylightOptions;

        var directionSkylightOptions = XMLMapOptions.ReadDirectionSkylightOptions(mapOptionsContainer);
        if (directionSkylightOptions is not null)
            ret.DirectionalSkyLight = directionSkylightOptions;
        
        var windDirectionOptions = XMLMapOptions.ReadWindDirectionOptions(mapOptionsContainer);
        if (windDirectionOptions is not null)
            ret.WindDirection = windDirectionOptions;
        
        var volumetricFogOptions = XMLMapOptions.ReadVolumetricFogOptions(mapOptionsContainer);
        if (volumetricFogOptions is not null)
            ret.VolumetricFog = volumetricFogOptions;
        
        var skyRenderingOptions = XMLMapOptions.ReadSkyRenderingOptions(mapOptionsContainer);
        if (skyRenderingOptions is not null)
            ret.SkyRendering = skyRenderingOptions;
        
        var atmosphericFogOptions = XMLMapOptions.ReadAtmosphericFogOptions(mapOptionsContainer);
        if (atmosphericFogOptions is not null)
            ret.AtmosphericFog = atmosphericFogOptions;

        return ret;
    }

    public static AmbientSound? ReadMapAmbientSound(XContainer mapContainer)
    {
        if (mapContainer is null)
            throw new Exception("Invalid map container when trying to read map ambient sound");
        
        var mapAmbientSoundContainer = XMLReader.GetXContainer(mapContainer, "struct", 41);

        if (mapAmbientSoundContainer is null)
            return null;

        var ambientSound = new AmbientSound();
        
        var mapPrimarySoundContainer = XMLReader.GetXContainer(mapAmbientSoundContainer, "struct", 0);
        if (mapPrimarySoundContainer is not null)
        {
            var mapPrimarySoundId = XMLReader.GetXElement(mapPrimarySoundContainer, 0);
            if (mapPrimarySoundId is not null)
                ambientSound.PrimarySound = (PrimarySoundId)int.Parse(mapPrimarySoundId.Value);
        }

        var mapSecondarySoundContainer = XMLReader.GetXContainer(mapAmbientSoundContainer, "struct", 1);
        if (mapSecondarySoundContainer is not null)
        {
            var mapSecondarySoundId = XMLReader.GetXElement(mapSecondarySoundContainer, 0);
            if (mapSecondarySoundId is not null)
                ambientSound.SecondarySound = (SecondarySoundId)int.Parse(mapSecondarySoundId.Value);
        }
        
        var mapReverbContainer = XMLReader.GetXContainer(mapAmbientSoundContainer, "struct", 2);
        if (mapReverbContainer is not null)
        {
            var mapReverbSoundId = XMLReader.GetXElement(mapReverbContainer, 0);
            if (mapReverbSoundId is not null)
                ambientSound.Reverb = (ReverbId)int.Parse(mapReverbSoundId.Value);
        }
        
        var mapAudioEffectContainer = XMLReader.GetXContainer(mapAmbientSoundContainer, "struct", 3);
        if (mapAudioEffectContainer is not null)
        {
            var mapAudioEffectSoundId = XMLReader.GetXElement(mapAudioEffectContainer, 0);
            if (mapAudioEffectSoundId is not null)
                ambientSound.AudioEffect = (AudioEffectId)int.Parse(mapAudioEffectSoundId.Value);
        }
        
        var mapAmbientPreview = XMLReader.GetXElement(mapAmbientSoundContainer, 4);
        if (mapAmbientPreview is not null)
            ambientSound.EnablePreview = bool.Parse(mapAmbientPreview.Value);

        return ambientSound;
    }

    public static List<XMLObject>? ReadMapObjects(XDocument document)
    {
        if (document is null)
            throw new Exception("Invalid document when trying to return all objects");
        
        XContainer objectList = XMLHelper.GetObjectList(document);
        var objects = objectList.Elements().Select(XMLObject.GenerateObjectFromXML).ToList();

        if (objects.Count == 0)
            return null;

        return objects;
    }

    public static void WriteMapId(XContainer mapContainer, Map map)
    {
        if (mapContainer is null)
            throw new Exception("Invalid map container when trying to write map id");

        if (map.MapId is null)
            return;

        var mapIdContainer = XMLReader.GetXContainer(mapContainer, "struct", 0, createIfNull: true, clearOnFind: true);
        XMLWriter.WriteObjectToContainer(mapIdContainer, map.MapId, 0);
    }

    public static void WriteDecorators(XContainer mapContainer, Map map)
    {
        if (mapContainer is null)
            throw new Exception("Invalid map container when trying to write map id");
        
        if (map.Decorators is null)
            return;

        XMLWriter.WriteObjectToContainer(mapContainer, map.Decorators, 25);
    }
    
    public static void WriteMapScreenEffectId(XContainer mapContainer, Map map)
    {
        if (mapContainer is null)
            throw new Exception("Invalid map container when trying to write map screen effect");
        
        if (map.ScreenEffectId is null)
            return;

        var mapScreenEffectContainer = XMLReader.GetXContainer(mapContainer, "struct", 34, createIfNull: true, clearOnFind: true);
        XMLWriter.WriteObjectToContainer(mapScreenEffectContainer, map.ScreenEffectId, 0);
    }

    public static void WriteMapAmbientSound(XContainer mapContainer, Map map)
    {
        if (mapContainer is null)
            throw new Exception("Invalid map container when trying to write map screen effect");
        
        if (map.AmbientSound is null)
            return;

        var mapAmbientSoundContainer = XMLReader.GetXContainer(mapContainer, "struct", 41, createIfNull: true, clearOnFind: true);

        if (map.AmbientSound.PrimarySound is not null)
        {
            var mapPrimarySoundContainer = XMLReader.GetXContainer(mapAmbientSoundContainer, "struct", 0,
                createIfNull: true, clearOnFind: true);
            XMLWriter.WriteObjectToContainer(mapPrimarySoundContainer, map.AmbientSound.PrimarySound, 0);
        }
        
        if (map.AmbientSound.SecondarySound is not null)
        {
            var mapSecondarySoundContainer = XMLReader.GetXContainer(mapAmbientSoundContainer, "struct", 1,
                createIfNull: true, clearOnFind: true);
            XMLWriter.WriteObjectToContainer(mapSecondarySoundContainer, map.AmbientSound.SecondarySound, 0);
        }
        
        if (map.AmbientSound.Reverb is not null)
        {
            var mapReverbSoundContainer = XMLReader.GetXContainer(mapAmbientSoundContainer, "struct", 2,
                createIfNull: true, clearOnFind: true);
            XMLWriter.WriteObjectToContainer(mapReverbSoundContainer, map.AmbientSound.Reverb, 0);
        }
        
        if (map.AmbientSound.AudioEffect is not null)
        {
            var mapAudioEffectSoundContainer = XMLReader.GetXContainer(mapAmbientSoundContainer, "struct", 3,
                createIfNull: true, clearOnFind: true);
            XMLWriter.WriteObjectToContainer(mapAudioEffectSoundContainer, map.AmbientSound.AudioEffect, 0);
        }
        
        if (map.AmbientSound.EnablePreview is not null)
            XMLWriter.WriteObjectToContainer(mapAmbientSoundContainer, map.AmbientSound.EnablePreview, 4);
    }
}