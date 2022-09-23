using System.Numerics;
using System.Security.AccessControl;
using Bond;
using Schemas;

namespace BondReader.Schemas;

[Bond.Schema]
public class MapSchema
{
    [Id(1)] public MapIdContainer MapIdContainer { get; set; }
    [Id(3)] public LinkedList<Item> Items { get; set; } = new LinkedList<Item>();
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
    [Id(1)] public UnknownRandomListContainer Type { get; set; }
}

[Bond.Schema]
public class UnknownRandomListContainer
{
    [Id(0)] public UnknownRandomNumberList NumberListContainer { get; set; }
}

[Bond.Schema]
public class UnknownRandomNumberList
{
    [Id(0)] public LinkedList<byte> Numbers { get; set; }
}

[Bond.Schema]
public class Item
{
    // [Id(2)] public int ItemId { get; set; } = default;
    [Id(3)] public Vector3 Position { get; set; } = new Vector3();
    [Id(4)] public Vector3 Up { get; set; } = new Vector3();
    [Id(5)] public Vector3 Forward { get; set; } = new Vector3();

    /// <summary>
    /// Unknown (changes from 21 when static to 1 when dynamic)
    /// </summary>
    [Id(7)]
    public uint Unknown1 { get; set; } = default;

    [Id(8)] public ItemSettingsContainer SettingsContainer { get; set; }
    [Id(9)] public Unknown_9 Unknown9 { get; set; } = new Unknown_9();
    [Id(10)] public Unknown_10 Unknown10 { get; set; } = new Unknown_10();

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

    [Id(23)] public LinkedList<ScaleList> Type { get; set; } = new LinkedList<ScaleList>();

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