using Bond;
using BondReader.Schemas.Generic;
using InfiniteForgeConstants.ObjectSettings;

namespace BondReader.Schemas.Items;

[Bond.Schema]
public class ItemSchema
{
    public ItemSchema(GameObject gameObject)
    {
        ItemId = new GenericIntStruct((int)gameObject.ObjectId);
        Position = new Vector3(gameObject.Transform.Position);
        Up = new Vector3(gameObject.Transform.DirectionVectors.Up);
        Forward = new Vector3(gameObject.Transform.DirectionVectors.Forward);
        StaticDynamicFlagUnknown = (byte)(gameObject.Transform.IsStatic ? 21 : 1);
        SettingsContainer = new ItemSettingsContainer(gameObject);
        VariantSettingsList.AddFirst(new UnknownVariantSettings(gameObject.Transform.IsStatic ? 2 : 1, 0));
    }
    
    [Id(2)] public GenericIntStruct ItemId { get; set; } = default;
    [Id(3)] public Vector3 Position { get; set; } = new Vector3();
    [Id(4)] public Vector3 Up { get; set; } = new Vector3();
    [Id(5)] public Vector3 Forward { get; set; } = new Vector3();
    
    /// <summary>
    /// Unknown (changes from 21 when static to 1 when dynamic)
    [Id(7)]
    public byte StaticDynamicFlagUnknown { get; set; } = default;

    /// <summary>
    /// Unknown (changes from 21 when static to 1 when dynamic)
    /// </summary>

    [Id(8)] public ItemSettingsContainer SettingsContainer { get; set; } = new ItemSettingsContainer();
    
    [Id(9)] public UnknownStruct Unknown9 { get; set; } = new UnknownStruct();
    [Id(10)] public UnknownStruct Unknown10 { get; set; } = new UnknownStruct();
    
    [Id(24)]
    public LinkedList<UnknownVariantSettings> VariantSettingsList { get; set; } =
        new LinkedList<UnknownVariantSettings>();

    [Bond.Schema]
    public class UnknownVariantSettings
    {
        public UnknownVariantSettings(int staticDynamicFlag, int scriptBrainFlag)
        {
            StaticDynamicFlag = staticDynamicFlag;
            ScriptBrainFlag = scriptBrainFlag;
        }
        
        /// <summary>
        /// 2 when static 1 when dynamic 
        /// </summary>
        [Id(0)]
        public int StaticDynamicFlag { get; set; }

        [Id(2)] public int ScriptBrainFlag { get; set; }
    }
}

[Bond.Schema]
public class ItemSettingsContainer
{
    public ItemSettingsContainer() {}
    
    /// <summary>
    /// Potentially change this constructor to take settings instead of gameObject
    /// </summary>
    public ItemSettingsContainer(GameObject gameObject)
    {
        ItemSettings.AddFirst(new ItemSettings0(gameObject.Transform.IsStatic ? 2 : 1, default));
        Scale.AddFirst(new ScaleList(gameObject));
    }
    
    [Id(0)] public LinkedList<ItemSettings0> ItemSettings { get; set; } = new LinkedList<ItemSettings0>();

    [Id(22)]
    public LinkedList<ScriptIndexSettings22> ScriptIndex22 { get; set; } = new LinkedList<ScriptIndexSettings22>();

    [Id(23)] public LinkedList<ScaleList> Scale { get; set; } = new LinkedList<ScaleList>();

    [Id(24)] public LinkedList<VariantOptions> VariantSettings { get; set; }


    [Bond.Schema]
    public class VariantOptions
    {
    }

    [Bond.Schema]
    public class ScaleList
    {
        public ScaleList(GameObject gameObject)
        {
            ScaleContainer = new Vector3(gameObject.Transform.Scale);
        }
        
        [Id(0)] public Vector3 ScaleContainer { get; set; }
    }

    [Bond.Schema]
    public class ScriptIndexSettings22
    {
        public ScriptIndexSettings22(uint attachedScriptIndex)
        {
            AttachedScriptIndex = attachedScriptIndex;
        }
        
        /// <summary>
        /// The index of what scripts are attached to this (possibly only on brains)
        /// </summary>
        [Id(0)]
        public uint AttachedScriptIndex { get; set; } = default;
    }

    [Bond.Schema]
    public class ItemSettings0
    {
        public ItemSettings0(int unknownStaticDynamicFlag, ushort unknown)
        {
            UnknownStaticDynamicFlag = unknownStaticDynamicFlag;
            UnknownuUnSignedshort = unknown;
        }
        
        /// <summary>
        /// Unknown but its seems to be related to dynamic / static items
        /// 2 = static 1 = dynamic
        /// </summary>
        [Id(1)]
        public int UnknownStaticDynamicFlag { get; set; } = default;

        [Id(10)] public ushort UnknownuUnSignedshort { get; set; } = default;
    }
}