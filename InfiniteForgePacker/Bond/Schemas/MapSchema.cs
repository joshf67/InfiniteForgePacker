using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using Bond;
using Schemas;

namespace BondReader.Schemas;

[Bond.Schema]
public class MapSchema
{
    [Id(1)] public MapIdContainer MapIdContainer { get; set; }


    [Id(3)] public LinkedList<Item> Items { get; set; }
    [Id(6)] public MapIdContainer.DebugStruct FolderContainer { get; set; }
    [Id(7)] public MapIdContainer.DebugStruct id7Struct { get; set; }

    [Id(10)] public MapIdContainer.DebugStruct id10Struct { get; set; }
}

[Bond.Schema]
public class MapId
{
    [Id(0)] public int Id { get; set; }
}

[Bond.Schema]
public class MapIdContainer
{
    [Id(0)] public MapId MapId { get; set; }

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
    public class DebugStruct
    {
    }

    [Bond.Schema]
    public class MapLightingSettings
    {
        //Sunlight
        [Id(0)] public float SunLightIntensity { get; set; }
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

    [Bond.Schema]
    public class Id24ListStruct
    {
    }

    [Bond.Schema]
    public class Id21Struct
    {
    }
}

[Bond.Schema]
public class GenericIntStruct
{
    [Id(0)] public int Int { get; set; }
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

[Bond.Schema]
public class Item
{
    [Id(2)] public GenericIntStruct ItemId { get; set; } = default;
    [Id(3)] public Vector3 Position { get; set; } = new Vector3();
    [Id(4)] public Vector3 Up { get; set; } = new Vector3();
    [Id(5)] public Vector3 Forward { get; set; } = new Vector3();

    /// <summary>
    /// Unknown (changes from 21 when static to 1 when dynamic)
    [Id(7)]
    public byte StaticDynamicFlagUnknown { get; set; } = default;

    [Id(24)]
    public LinkedList<UnknownVariantSettings> VariantSettingsList { get; set; } =
        new LinkedList<UnknownVariantSettings>();

    /// <summary>
    /// Unknown (changes from 21 when static to 1 when dynamic)
    /// </summary>

    [Id(8)]
    public ItemSettingsContainer SettingsContainer { get; set; } = new ItemSettingsContainer();

    [Id(9)] public Unknown_9 Unknown9 { get; set; } = new Unknown_9();
    [Id(10)] public Unknown_10 Unknown10 { get; set; } = new Unknown_10();


    [Bond.Schema]
    public class UnknownVariantSettings
    {
        /// <summary>
        /// 2 when static 1 when dynamic 
        /// </summary>
        [Id(0)]
        public int StaticDynamicFlag { get; set; }

        [Id(2)] public int ScriptBrainFlag { get; set; }
    }

    [Bond.Schema]
    public class Unknown_9
    {
    }

    [Bond.Schema]
    public class Unknown_10
    {
    }
}

[Bond.Schema]
public class ItemSettingsContainer
{
    [Id(0)] public LinkedList<ItemSettings0> ItemSettings { get; set; } = new LinkedList<ItemSettings0>();

    [Id(22)]
    public LinkedList<ScriptIndexSettings22> ScriptIndex22 { get; set; } = new LinkedList<ScriptIndexSettings22>();

    [Id(23)] public LinkedList<ScaleList> Scalelist { get; set; } = new LinkedList<ScaleList>();

    [Id(24)] public LinkedList<VariantOptions> VariantSettings { get; set; }


    [Bond.Schema]
    public class VariantOptions
    {
    }

    [Bond.Schema]
    public class ScaleList
    {
        [Id(0)] public Vector3 ScaleContainer { get; set; }
    }

    [Bond.Schema]
    public class ScriptIndexSettings22
    {
        /// <summary>
        /// The index of what scripts are attached to this (possibly only on brains)
        /// </summary>
        [Id(0)]
        public uint AttachedScriptIndex { get; set; } = default;
    }

    [Bond.Schema]
    public class ItemSettings0
    {
        /// <summary>
        /// Unknown but its seems to be related to dynamic / static items
        /// 2 = static 1 = dynamic
        /// </summary>
        [Id(1)]
        public int UnknownStaticDynamicFlag { get; set; } = default;

        [Id(10)] public ushort UnknownuUnSignedshort { get; set; } = default;
    }
}

[Bond.Schema]
public class Vector3
{
    [Id(0)] public float X { get; set; } = default;
    [Id(1)] public float Y { get; set; } = default;
    [Id(2)] public float Z { get; set; } = default;
}

[Bond.Schema]
public class Vector2
{
    [Id(0)] public float X { get; set; } = default;
    [Id(1)] public float Y { get; set; } = default;
}