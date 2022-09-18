using System.Numerics;
using System.Xml.Linq;
using InfiniteForgeConstants.MapSettings;
using InfiniteForgeConstants.MapSettings.Options;

namespace InfiniteForgePacker.XML;

public static class XMLMapOptions
{
    public static Sunlight? ReadSunlightOptions(XContainer mapOptionsContainer)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's sunlight options");
        
        var ret = new Sunlight();
        var foundData = false;
            
        var mapSunlightIntensity = XMLReader.GetXElement(mapOptionsContainer, 0);
        if (mapSunlightIntensity is not null)
        {
            ret.Intensity = float.Parse(mapSunlightIntensity.Value);
            foundData = true;
        }
            
        var mapSunlightColor = XMLReader.GetXElement(mapOptionsContainer, 1);
        if (mapSunlightColor is not null)
        {
            ret.ColorOverride = (ColorId)int.Parse(mapSunlightColor.Value);
            foundData = true;
        }
            
        var mapSunlightDirection = XMLReader.GetXContainer(mapOptionsContainer, "struct", 2);
        if (mapSunlightDirection is not null)
        {
            var direction = Vector2.Zero;
                
            var mapSunlightYaw = XMLReader.GetXElement(mapSunlightDirection, 0);
            if (mapSunlightYaw is not null)
            {
                direction.X = float.Parse(mapSunlightYaw.Value);
                foundData = true;
            }
                
            var mapSunlightPitch = XMLReader.GetXElement(mapSunlightDirection, 0);
            if (mapSunlightPitch is not null)
            {
                direction.Y = float.Parse(mapSunlightPitch.Value);
                foundData = true;
            }

            ret.Direction = direction;
        }

        return foundData ? ret : null;
    }
    
    public static LightBounce? ReadLightBounceOptions(XContainer mapOptionsContainer)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's light bounce options");
        
        var ret = new LightBounce();
        var foundData = false;
            
        var mapLightBounceIntensity = XMLReader.GetXElement(mapOptionsContainer, 3);
        if (mapLightBounceIntensity is not null)
        {
            ret.Intensity = float.Parse(mapLightBounceIntensity.Value);
            foundData = true;
        }
            
        var mapLightBounceTint = XMLReader.GetXElement(mapOptionsContainer, 4);
        if (mapLightBounceTint is not null)
        {
            ret.TintOverride = (ColorId)int.Parse(mapLightBounceTint.Value);
            foundData = true;
        }

        return foundData ? ret : null;
    }
    
    public static SkyLight? ReadSkylightOptions(XContainer mapOptionsContainer)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's skylight options");
        
        var ret = new SkyLight();
        var foundData = false;
            
        var mapSkylightIntensity = XMLReader.GetXElement(mapOptionsContainer, 5);
        if (mapSkylightIntensity is not null)
        {
            ret.Intensity = float.Parse(mapSkylightIntensity.Value);
            foundData = true;
        }
            
        var mapSkylightBounceTint = XMLReader.GetXElement(mapOptionsContainer, 6);
        if (mapSkylightBounceTint is not null)
        {
            ret.TintOverride = (ColorId)int.Parse(mapSkylightBounceTint.Value);
            foundData = true;
        }

        return foundData ? ret : null;
    }
    
    //Still need a way to implement this because its a list and I'm not certain on the layout
    public static DirectionalSkyLight? ReadDirectionSkylightOptions(XContainer mapOptionsContainer)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's directional skylight options");
        
        var ret = new DirectionalSkyLight();
            
        var mapDirectionalSkylight = XMLReader.GetXContainer(mapOptionsContainer, "list", 7);
        if (mapDirectionalSkylight is not null)
        {
            
        }
        
        return null;
    }
    
    public static WindDirection? ReadWindDirectionOptions(XContainer mapOptionsContainer)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's wind direction options");
        
        var ret = new WindDirection();
        var foundData = false;
            
        var mapWindDirectionContainer = XMLReader.GetXContainer(mapOptionsContainer, "struct", 9);
        if (mapWindDirectionContainer is not null)
        {
            var direction = Vector3.Zero;
            
            var mapWindDirectionYaw = XMLReader.GetXElement(mapWindDirectionContainer, 0);
            if (mapWindDirectionYaw is not null)
            {
                direction.X =  float.Parse(mapWindDirectionYaw.Value);
                foundData = true;
            }
            
            var mapWindDirectionPitch = XMLReader.GetXElement(mapWindDirectionContainer, 1);
            if (mapWindDirectionPitch is not null)
            {
                direction.Y =  float.Parse(mapWindDirectionPitch.Value);
                foundData = true;
            }
            
            var mapWindDirectionRoll = XMLReader.GetXElement(mapWindDirectionContainer, 2);
            if (mapWindDirectionRoll is not null)
            {
                direction.Z =  float.Parse(mapWindDirectionRoll.Value);
                foundData = true;
            }

            ret.Direction = direction;
        }
        
        var mapWindDirectionSpeed = XMLReader.GetXElement(mapOptionsContainer, 10);
        if (mapWindDirectionSpeed is not null)
        {
            ret.Speed =  float.Parse(mapWindDirectionSpeed.Value);
            foundData = true;
        }
        
        return foundData ? ret : null;
    }
    
    public static VolumetricFog? ReadVolumetricFogOptions(XContainer mapOptionsContainer)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's volumetric fog options");
        
        var ret = new VolumetricFog();
        var foundData = false;
            
        var mapVolumetricFogEnabled = XMLReader.GetXElement(mapOptionsContainer, 13);
        if (mapVolumetricFogEnabled is not null)
        {
            ret.Enabled =  bool.Parse(mapVolumetricFogEnabled.Value);
            foundData = true;
        }
        
        var mapVolumetricFogDensity = XMLReader.GetXElement(mapOptionsContainer, 14);
        if (mapVolumetricFogDensity is not null)
        {
            ret.Density =  float.Parse(mapVolumetricFogDensity.Value);
            foundData = true;
        }
        
        var mapVolumetricFogColor = XMLReader.GetXElement(mapOptionsContainer, 15);
        if (mapVolumetricFogColor is not null)
        {
            ret.Color =  (ColorId)int.Parse(mapVolumetricFogColor.Value);
            foundData = true;
        }
        
        var mapVolumetricFogNearRange = XMLReader.GetXElement(mapOptionsContainer, 16);
        if (mapVolumetricFogNearRange is not null)
        {
            ret.NearRange =  float.Parse(mapVolumetricFogNearRange.Value);
            foundData = true;
        }
        
        var mapVolumetricFogFarRange = XMLReader.GetXElement(mapOptionsContainer, 17);
        if (mapVolumetricFogFarRange is not null)
        {
            ret.FarRange =  float.Parse(mapVolumetricFogFarRange.Value);
            foundData = true;
        }

        return foundData ? ret : null;
    }
    
    public static SkyRendering? ReadSkyRenderingOptions(XContainer mapOptionsContainer)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's sky rendering options");
        
        var ret = new SkyRendering();
        var foundData = false;
            
        var mapSkyRenderingSkyIntensity = XMLReader.GetXElement(mapOptionsContainer, 23);
        if (mapSkyRenderingSkyIntensity is not null)
        {
            ret.SkyIntensity =  float.Parse(mapSkyRenderingSkyIntensity.Value);
            foundData = true;
        }
        
        var mapSkyRenderingSunIntensity = XMLReader.GetXElement(mapOptionsContainer, 24);
        if (mapSkyRenderingSunIntensity is not null)
        {
            ret.SunIntensity =  float.Parse(mapSkyRenderingSunIntensity.Value);
            foundData = true;
        }
        
        var mapSkyRenderingSkyTint = XMLReader.GetXElement(mapOptionsContainer, 25);
        if (mapSkyRenderingSkyTint is not null)
        {
            ret.SkyTint =  (ColorId)int.Parse(mapSkyRenderingSkyTint.Value);
            foundData = true;
        }
        
        var mapSkyRenderingSkyTintIntensity = XMLReader.GetXElement(mapOptionsContainer, 27);
        if (mapSkyRenderingSkyTintIntensity is not null)
        {
            ret.SkyTintIntensity =  float.Parse(mapSkyRenderingSkyTintIntensity.Value);
            foundData = true;
        }
        
        var mapSkyRenderingSunTint = XMLReader.GetXElement(mapOptionsContainer, 28);
        if (mapSkyRenderingSunTint is not null)
        {
            ret.SkyTint =  (ColorId)int.Parse(mapSkyRenderingSunTint.Value);
            foundData = true;
        }
        
        var mapSkyRenderingSunTintIntensity = XMLReader.GetXElement(mapOptionsContainer, 30);
        if (mapSkyRenderingSunTintIntensity is not null)
        {
            ret.SkyTintIntensity =  float.Parse(mapSkyRenderingSunTintIntensity.Value);
            foundData = true;
        }

        return foundData ? ret : null;
    }

    public static AtmosphericFog? ReadAtmosphericFogOptions(XContainer mapOptionsContainer)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's atmospheric fog options");
        
        var ret = new AtmosphericFog();
        var foundData = false;
            
        var mapAtmosphericFogFogOffset = XMLReader.GetXElement(mapOptionsContainer, 31);
        if (mapAtmosphericFogFogOffset is not null)
        {
            ret.FogOffset =  float.Parse(mapAtmosphericFogFogOffset.Value);
            foundData = true;
        }
        
        var mapAtmosphericFogFogNearFallof = XMLReader.GetXElement(mapOptionsContainer, 32);
        if (mapAtmosphericFogFogNearFallof is not null)
        {
            ret.FogNearFallof =  float.Parse(mapAtmosphericFogFogNearFallof.Value);
            foundData = true;
        }
        
        var mapAtmosphericFogFogIntensity = XMLReader.GetXElement(mapOptionsContainer, 33);
        if (mapAtmosphericFogFogIntensity is not null)
        {
            ret.FogIntensity =  float.Parse(mapAtmosphericFogFogIntensity.Value);
            foundData = true;
        }
        
        var mapAtmosphericFogFogDepthScale = XMLReader.GetXElement(mapOptionsContainer, 34);
        if (mapAtmosphericFogFogDepthScale is not null)
        {
            ret.FogDepthScale =  float.Parse(mapAtmosphericFogFogDepthScale.Value);
            foundData = true;
        }
        
        var mapAtmosphericFogFogFallofUp = XMLReader.GetXElement(mapOptionsContainer, 35);
        if (mapAtmosphericFogFogFallofUp is not null)
        {
            ret.FogFallofUp =  float.Parse(mapAtmosphericFogFogFallofUp.Value);
            foundData = true;
        }
        
        var mapAtmosphericFogFogFallofDown = XMLReader.GetXElement(mapOptionsContainer, 36);
        if (mapAtmosphericFogFogFallofDown is not null)
        {
            ret.FogFallofDown =  float.Parse(mapAtmosphericFogFogFallofDown.Value);
            foundData = true;
        }
        
        var mapAtmosphericFogSkyFogIntensity = XMLReader.GetXElement(mapOptionsContainer, 37);
        if (mapAtmosphericFogSkyFogIntensity is not null)
        {
            ret.SkyFogIntensity =  float.Parse(mapAtmosphericFogSkyFogIntensity.Value);
            foundData = true;
        }
        
        var mapAtmosphericFogFakeInscatteringTint = XMLReader.GetXElement(mapOptionsContainer, 38);
        if (mapAtmosphericFogFakeInscatteringTint is not null)
        {
            ret.FakeInscatteringTint =  (ColorId)int.Parse(mapAtmosphericFogFakeInscatteringTint.Value);
            foundData = true;
        }

        return foundData ? ret : null;
    }

    public static void WriteMapOptions(XContainer mapOptionsContainer, MapOptions map)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to write options options");
        
        if (map.Sunlight is not null)
            WriteSunlightOptions(mapOptionsContainer, map.Sunlight);
        
        if (map.LightBounce is not null)
            WriteLightBounceOptions(mapOptionsContainer, map.LightBounce);
        
        if (map.SkyLight is not null)
            WriteSkylightOptions(mapOptionsContainer, map.SkyLight);
        
        if (map.DirectionalSkyLight is not null)
            WriteDirectionSkylightOptions(mapOptionsContainer, map.DirectionalSkyLight);
        
        if (map.WindDirection is not null)
            WriteWindDirectionOptions(mapOptionsContainer, map.WindDirection);
        
        if (map.VolumetricFog is not null)
            WriteVolumetricFogOptions(mapOptionsContainer, map.VolumetricFog);
        
        if (map.SkyRendering is not null)
            WriteSkyRenderingOptions(mapOptionsContainer, map.SkyRendering);
        
        if (map.AtmosphericFog is not null)
            WriteAtmosphericFogOptions(mapOptionsContainer, map.AtmosphericFog);
    }
    
    public static void WriteSunlightOptions(XContainer mapOptionsContainer, Sunlight sunlight)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's sunlight options");

        if (sunlight.Intensity is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, sunlight.Intensity, 0);
        
        if (sunlight.ColorOverride is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, sunlight.ColorOverride, 1);
        
        if (sunlight.Direction is not null)
            XMLWriter.WriteVector2ToContainer(mapOptionsContainer, (Vector2)sunlight.Direction, 2);
    }
    
    public static void WriteLightBounceOptions(XContainer mapOptionsContainer, LightBounce lightBounce)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's light bounce options");
        
        if (lightBounce.Intensity is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, lightBounce.Intensity, 3);
        
        if (lightBounce.TintOverride is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, lightBounce.TintOverride, 4);
    }
    
    public static void WriteSkylightOptions(XContainer mapOptionsContainer, SkyLight skyLight)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's skylight options");
        
        if (skyLight.Intensity is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, skyLight.Intensity, 5);
        
        if (skyLight.TintOverride is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, skyLight.TintOverride, 6);
    }
    
    //Still need a way to implement this because its a list and I'm not certain on the layout
    public static void WriteDirectionSkylightOptions(XContainer mapOptionsContainer, DirectionalSkyLight directionalSkyLight)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's directional skylight options");
        
        return;
    }
    
    public static void WriteWindDirectionOptions(XContainer mapOptionsContainer, WindDirection windDirection)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's wind direction options");
        
        if (windDirection.Direction is not null)
            XMLWriter.WriteVector3ToContainer(mapOptionsContainer, (Vector3)windDirection.Direction, 9);
        
        if (windDirection.Speed is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, windDirection.Speed, 10);
    }
    
    public static void WriteVolumetricFogOptions(XContainer mapOptionsContainer, VolumetricFog volumetricFog)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's volumetric fog options");
        
        if (volumetricFog.Enabled is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, volumetricFog.Enabled, 13);
        
        if (volumetricFog.Density is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, volumetricFog.Density, 14);
        
        if (volumetricFog.Color is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, volumetricFog.Color, 15);
        
        if (volumetricFog.NearRange is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, volumetricFog.NearRange, 16);
        
        if (volumetricFog.FarRange is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, volumetricFog.FarRange, 17);
    }
    
    public static void WriteSkyRenderingOptions(XContainer mapOptionsContainer, SkyRendering skyRendering)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's sky rendering options");
        
        if (skyRendering.SkyIntensity is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, skyRendering.SkyIntensity, 23);
        
        if (skyRendering.SunIntensity is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, skyRendering.SunIntensity, 24);

        if (skyRendering.SkyTint is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, skyRendering.SkyTint, 25);

        if (skyRendering.SkyTintIntensity is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, skyRendering.SkyTintIntensity, 27);

        if (skyRendering.SunTint is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, skyRendering.SunTint, 28);

        if (skyRendering.SunTintIntensity is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, skyRendering.SunTintIntensity, 30);
    }

    public static void WriteAtmosphericFogOptions(XContainer mapOptionsContainer, AtmosphericFog atmosphericFog)
    {
        if (mapOptionsContainer is null)
            throw new Exception("Invalid map options container when trying to read map's atmospheric fog options");
        
        if (atmosphericFog.FogOffset is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, atmosphericFog.FogOffset, 31);
        
        if (atmosphericFog.FogNearFallof is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, atmosphericFog.FogNearFallof, 32);
        
        if (atmosphericFog.FogIntensity is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, atmosphericFog.FogIntensity, 33);
        
        if (atmosphericFog.FogDepthScale is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, atmosphericFog.FogDepthScale, 34);
        
        if (atmosphericFog.FogFallofUp is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, atmosphericFog.FogFallofUp, 35);
        
        if (atmosphericFog.FogFallofDown is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, atmosphericFog.FogFallofDown, 36);
        
        if (atmosphericFog.SkyFogIntensity is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, atmosphericFog.SkyFogIntensity, 37);
        
        if (atmosphericFog.FakeInscatteringTint is not null)
            XMLWriter.WriteObjectToContainer(mapOptionsContainer, atmosphericFog.FakeInscatteringTint, 38);
    }
    
}