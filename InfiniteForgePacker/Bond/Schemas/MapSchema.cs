using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using Bond;
using BondReader.Schemas.Generic;
using BondReader.Schemas.Items;
using InfiniteForgeConstants;
using InfiniteForgeConstants.MapSettings;
using Vector2 = BondReader.Schemas.Generic.Vector2;
using Vector3 = BondReader.Schemas.Generic.Vector3;

namespace BondReader.Schemas;

[Bond.Schema]
public class MapSchema
{
    public MapSchema(Map map)
    {
        //  MapId = new GenericIntStruct((int)map.MapId);
        //LightSettings = new MapLightingSettings(map);
    }

    public MapSchema(MapId mapId)
    {
        this.MapId = new GenericIntStruct();
        this.MapId.Int = (int)mapId;
    }

    public MapSchema()
    {
    }

    [Id(0)] public GenericIntStruct MapId { get; set; }

    /*
    [Id(1)] public UnknownRandomListContainer Type { get; set; }
    [Id(3)] public int id3Int { get; set; }
    [Id(4)] public int id4Int { get; set; }
    [Id(21)] public Id21Struct id21Struct { get; set; }
    [Id(24)] public LinkedList<Id24ListStruct> id24ListStruct { get; set; }
    [Id(28)] public GenericIntStruct id28Int { get; set; }

    [Id(29)] public GenericIntStruct int29Int { get; set; }

    //[Id(30)] public DebugStruct id30Struct { get; set; } // this is a struct
    //[Id(31)] public DebugStruct id31Struct { get; set; }
    //[Id(32)] public DebugStruct id32Struct { get; set; }
    //[Id(33)] public DebugStruct id33Struct { get; set; }
    //[Id(34)] public DebugStruct id34Struct { get; set; }
    [Id(39)] public MapLightingSettings LightSettings { get; set; }
    [Id(41)] public Id41StructContainer id41StructContainer { get; set; }
    [Id(47)] public LinkedList<int> id47List { get; set; }
*/
    [Bond.Schema]
    public class Id41StructContainer
    {
        [Id(0)] public DebugStruct id0Struct { get; set; }
        [Id(1)] public DebugStruct id1Struct { get; set; }
        [Id(2)] public DebugStruct id2Struct { get; set; }
        [Id(3)] public DebugStruct id3Struct { get; set; }
        [Id(5)] public bool id5Bool { get; set; }
    }

    [Bond.Schema]
    public class MapLightingSettings
    {
        public MapLightingSettings(Map map)
        {
            if (map.Options?.Sunlight?.Intensity != null) SunLightIntensity = (float)map.Options.Sunlight.Intensity;
            if (map.Options?.Sunlight?.ColorOverride != null) SunLightColor = (int)map.Options.Sunlight.ColorOverride;
            if (map.Options?.Sunlight?.Direction != null)
                SunDirection = new Vector2(map.Options.Sunlight.Direction.Value);

            if (map.Options?.LightBounce?.Intensity != null) BounceIntensity = (float)map.Options.LightBounce.Intensity;
            if (map.Options?.LightBounce?.TintOverride != null) BounceTint = (int)map.Options.LightBounce.TintOverride;

            if (map.Options?.SkyLight?.Intensity != null) SkyLightIntensity = (float)map.Options.SkyLight.Intensity;
            if (map.Options?.SkyLight?.TintOverride != null) SkyLightTint = (int)map.Options.SkyLight.TintOverride;

            //Still unknown how to do DirectionalSkyLight

            if (map.Options?.WindDirection?.Direction != null)
                WindDirection = new Vector3(map.Options.WindDirection.Direction.Value);
            if (map.Options?.WindDirection?.Speed != null) WindSpeed = map.Options.WindDirection.Speed.Value;

            if (map.Options?.VolumetricFog?.Enabled != null) EnableFog = (bool)map.Options.VolumetricFog.Enabled;
            if (map.Options?.VolumetricFog?.Density != null) FogDensity = (float)map.Options.VolumetricFog.Density;
            if (map.Options?.VolumetricFog?.Color != null) FogColor = (int)map.Options.VolumetricFog.Color;
            if (map.Options?.VolumetricFog?.NearRange != null) NearRange = (float)map.Options.VolumetricFog.NearRange;
            if (map.Options?.VolumetricFog?.FarRange != null) FarRange = (float)map.Options.VolumetricFog.FarRange;

            if (map.Options?.SkyRendering?.SkyIntensity != null)
                SkyIntensity = (float)map.Options.SkyRendering.SkyIntensity;
            if (map.Options?.SkyRendering?.SunIntensity != null)
                SkySunIntensity = (float)map.Options.SkyRendering.SunIntensity;
            if (map.Options?.SkyRendering?.SkyTint != null) SkyTint = (int)map.Options.SkyRendering.SkyTint;
            if (map.Options?.SkyRendering?.SkyTintIntensity != null)
                SkyTintIntensity = (float)map.Options.SkyRendering.SkyTintIntensity;
            if (map.Options?.SkyRendering?.SunTint != null) SkySunTint = (int)map.Options.SkyRendering.SunTint;
            if (map.Options?.SkyRendering?.SunTintIntensity != null)
                SkySunTintIntensity = (float)map.Options.SkyRendering.SunTintIntensity;

            if (map.Options?.AtmosphericFog?.FogOffset != null) FogOffset = (float)map.Options.AtmosphericFog.FogOffset;
            if (map.Options?.AtmosphericFog?.FogNearFallof != null)
                FogNearFalloff = (float)map.Options.AtmosphericFog.FogNearFallof;
            if (map.Options?.AtmosphericFog?.FogIntensity != null)
                FogIntensity = (float)map.Options.AtmosphericFog.FogIntensity;
            if (map.Options?.AtmosphericFog?.FogDepthScale != null)
                FogDepthScale = (float)map.Options.AtmosphericFog.FogDepthScale;
            if (map.Options?.AtmosphericFog?.FogFallofUp != null)
                FogFalloffUp = (float)map.Options.AtmosphericFog.FogFallofUp;
            if (map.Options?.AtmosphericFog?.FogFallofDown != null)
                FogFalloffDown = (float)map.Options.AtmosphericFog.FogFallofDown;
            if (map.Options?.AtmosphericFog?.SkyFogIntensity != null)
                SkyFogIntensity = (float)map.Options.AtmosphericFog.SkyFogIntensity;
            if (map.Options?.AtmosphericFog?.Inscattering != null)
                Inscattering = (float)map.Options.AtmosphericFog.Inscattering;
            if (map.Options?.AtmosphericFog?.FakeInscatteringTint != null)
                InscatteringTint = (int)map.Options.AtmosphericFog.FakeInscatteringTint;
        }

        //Sunlight
        [Id(0)] public float SunLightIntensity { get; set; }
        [Id(1)] public int SunLightColor { get; set; }
        [Id(2)] public Vector2 SunDirection { get; set; }
        [Id(3)] public float BounceIntensity { get; set; }
        [Id(4)] public int BounceTint { get; set; }
        [Id(5)] public float SkyLightIntensity { get; set; }
        [Id(6)] public int SkyLightTint { get; set; }

        /// <summary>
        /// overrides in order
        /// Top , Bottom , North, South,
        /// Ease , West
        /// </summary>
        [Id(7)]
        public LinkedList<int> DirectionalSkyLight { get; set; }

        //Wind
        ///<summary> x = yaw, y = pitch, z = unknown (maybe roll?) </summary>
        [Id(9)]
        public Vector3 WindDirection { get; set; }

        [Id(10)] public float WindSpeed { get; set; }
        [Id(11)] public SortedSet<int> id11IntSet { get; set; }

        [Id(12)] public SortedSet<int> id12IntSet { get; set; }

        //Volumetric Fog
        [Id(13)] public bool EnableFog { get; set; }
        [Id(14)] public float FogDensity { get; set; }
        [Id(15)] public int FogColor { get; set; }
        [Id(16)] public float NearRange { get; set; }
        [Id(17)] public float FarRange { get; set; }

        /// <summary>Might Be fog related </summary>
        [Id(18)]
        public int id18int { get; set; }

        /// <summary>Might Be fog related </summary>
        [Id(19)]
        public int id19int { get; set; }

        /// <summary>Might Be fog related </summary>
        [Id(20)]
        public int id20int { get; set; }

        /// <summary>Might Be fog related </summary>
        [Id(21)]
        public LinkedList<int> id21IntList { get; set; }

        /// <summary>Might Be fog related </summary>
        [Id(22)]
        public int id22Int { get; set; }

        //SKY RENDERING
        [Id(23)] public float SkyIntensity { get; set; }
        [Id(24)] public float SkySunIntensity { get; set; }
        [Id(25)] public int SkyTint { get; set; }

        /// <summary> Unknown (Seems to come after every color)</summary>

        [Id(26)]
        public int id26Int { get; set; }

        [Id(27)] public float SkyTintIntensity { get; set; }
        [Id(28)] public int SkySunTint { get; set; }

        /// <summary> Unknown (Seems to come after every color)</summary>
        [Id(29)]
        public int id29Int { get; set; }

        [Id(30)] public float SkySunTintIntensity { get; set; }


        //ATMOSPHERIC FOG
        [Id(31)] public float FogOffset { get; set; }
        [Id(32)] public float FogNearFalloff { get; set; }
        [Id(33)] public float FogIntensity { get; set; }
        [Id(34)] public float FogDepthScale { get; set; }
        [Id(35)] public float FogFalloffUp { get; set; }
        [Id(36)] public float FogFalloffDown { get; set; }
        [Id(37)] public float SkyFogIntensity { get; set; }
        [Id(38)] public float Inscattering { get; set; }
        [Id(39)] public int InscatteringTint { get; set; }

        /// <summary> Unknown (Seems to come after every color)</summary>
        [Id(40)]
        public int id40Int { get; set; }
    }
}

[Bond.Schema]
public class UnknownRandomListContainer
{
    [Id(0)] public UnknownRandomNumberList NumberListContainer { get; set; }
    [Id(1)] public UnknownRandonIntContainer RandonIntContainer { get; set; }
}

[Bond.Schema]
public class UnknownRandonIntContainer
{
    [Id(0)] public LinkedList<UnknownListData> Type { get; set; }

    [Bond.Schema]
    public class UnknownListData
    {
        [Id(0)] public bool UnknownBool { get; set; }
        [Id(1)] public LinkedList<int> UnknownInts { get; set; }
    }
}

[Bond.Schema]
public class UnknownRandomNumberList
{
    [Id(0)] public LinkedList<byte> Numbers { get; set; }
}