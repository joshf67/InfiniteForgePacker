using Bond;
using BondReader.Schemas.Generic;
using InfiniteForgeConstants.ObjectSettings;

namespace BondReader.Schemas.Items;

[Bond.Schema]
public class ItemSchema
{
    public ItemSchema(GameObject gameObject , bool useMetric = false)
    {
        ItemId = new GenericIntStruct((int)gameObject.ObjectId);
        Position = useMetric ? new Vector3(gameObject.Transform.MetricPosition) : new Vector3(gameObject.Transform.Position);
        Up = new Vector3(gameObject.Transform.DirectionVectors.Up);
        Forward = new Vector3(gameObject.Transform.DirectionVectors.Forward);
        StaticDynamicFlagUnknown = (byte)(gameObject.Transform.IsStatic ? 21 : 1);
        SettingsContainer = new ItemSettingsContainer(gameObject);
        SettingsContainer.VariantSettings.AddFirst(new VariantSettings(gameObject.Transform.IsStatic ? 2 : 1,
            gameObject.ObjectSettings.VariantId));


        //todo find int0 and bool meaning
        SettingsContainer.DynamicObjectDataContainer.AddFirst
            (new ItemSettingsContainer.UnnamedDynamicObjectDataContainer(0, true));
    }


    public ItemSchema()
    {
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

    [Id(8)]
    public ItemSettingsContainer SettingsContainer { get; set; } = new ItemSettingsContainer();

    [Id(9)] public UnknownStruct Unknown9 { get; set; } = new UnknownStruct();
    [Id(10)] public UnknownStruct Unknown10 { get; set; } = new UnknownStruct();


    [Bond.Schema]
    public class VariantSettings
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="staticDynamicFlag"> 2 when static 1 when dynamic </param>
        /// <param name="scriptBrainFlag"> 1 for objects 2 on script brain</param>
        public VariantSettings(int staticDynamicFlag, int variantId, int scriptBrainFlag = 1)
        {
            StaticDynamicFlag = staticDynamicFlag;
            ScriptBrainFlag = scriptBrainFlag;
            VaraintIdContainer = new ItemVaraintIdContainer(variantId);
            VaraintIdContainer.ItemVaraintId = variantId;
        }

        public VariantSettings()
        {
        }

        /// <summary>
        /// 2 when static 1 when dynamic 
        /// </summary>
        [Id(0)]
        public int StaticDynamicFlag { get; set; }

        [Id(1)] public ItemVaraintIdContainer VaraintIdContainer { get; set; } = new ItemVaraintIdContainer();


        /// <summary>
        /// 1 on objects, 2 on scripting brain and dynamic object), I think this determines if an object is static or dynamic
        /// </summary>
        [Id(2)]
        public int ScriptBrainFlag { get; set; }


        [Schema]
        public class ItemVaraintIdContainer
        {
            public ItemVaraintIdContainer(int varaintID)
            {
                this.ItemVaraintId = varaintID;
            }

            public ItemVaraintIdContainer()
            {
            }

            [Id(0)] public int ItemVaraintId { get; set; }
        }
    }
}

[Bond.Schema]
public class ItemSettingsContainer
{
    public ItemSettingsContainer()
    {
    }


    /// <summary>
    /// Potentially change this constructor to take settings instead of gameObject
    /// </summary>
    public ItemSettingsContainer(GameObject gameObject)
    {
        ItemSettings.AddFirst(new ItemSettings0ObjectPhysics
            (gameObject.Transform.PhysicsType, default));
        Scale.AddFirst(new ScaleList(gameObject));
        VariantSettings.AddFirst(new ItemSchema.VariantSettings(gameObject.Transform.IsStatic ? 2 : 1
            , gameObject.ObjectSettings.VariantId, gameObject.Transform.IsStatic ? 1 : 2));
    }

    [Id(0)]
    public LinkedList<ItemSettings0ObjectPhysics> ItemSettings { get; set; } =
        new LinkedList<ItemSettings0ObjectPhysics>();

    [Id(1)]
    public LinkedList<UnnamedDynamicObjectDataContainer> DynamicObjectDataContainer { get; set; }
        = new LinkedList<UnnamedDynamicObjectDataContainer>();

    //public LinkedList<GenericFactory> ItemLables { get; set; } = new LinkedList<ItemLable>();

    [Id(22)]
    public LinkedList<ScriptIndexSettings22> ScriptIndex22 { get; set; }
        = new LinkedList<ScriptIndexSettings22>();

    [Id(23)]
    public LinkedList<ScaleList> Scale { get; set; }
        = new LinkedList<ScaleList>();

    [Id(24)]
    public LinkedList<ItemSchema.VariantSettings> VariantSettings { get; set; } =
        new LinkedList<ItemSchema.VariantSettings>();


    [Bond.Schema]
    public class UnnamedDynamicObjectDataContainer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unknownId0Int"> this is 0 for dynamic</param>
        /// <param name="unknowDynamicBool"> this is true for dynamic</param>
        public UnnamedDynamicObjectDataContainer(int unknownId0Int, bool unknowDynamicBool)
        {
            var objectData = new DynamicObjectData(unknownId0Int);
            ObjectDataList.AddFirst(objectData);

            this.unknowDynamicBool = unknowDynamicBool;
        }

        public UnnamedDynamicObjectDataContainer()
        {
        }

        [Id(0)]
        public LinkedList<DynamicObjectData> ObjectDataList { get; set; }
            = new LinkedList<DynamicObjectData>();

        [Id(13)] public bool unknowDynamicBool { get; set; }

        [Bond.Schema]
        public class DynamicObjectData
        {
            public DynamicObjectData(int unknownId0Int)
            {
                this.UnknownId0Int = unknownId0Int;
            }

            public DynamicObjectData()
            {
            }

            [Id(0)] public int UnknownId0Int { get; set; }
        }
    }


    [Bond.Schema]
    public class ScaleList
    {
        public ScaleList(GameObject gameObject)
        {
            ScaleContainer = new Vector3(gameObject.Transform.Scale);
        }

        public ScaleList()
        {
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

        public ScriptIndexSettings22()
        {
        }

        /// <summary>
        /// The index of what scripts are attached to this (possibly only on brains)
        /// </summary>
        [Id(0)]
        public uint AttachedScriptIndex { get; set; } = default;
    }

    [Bond.Schema]
    public class ItemSettings0ObjectPhysics
    {
        public ItemSettings0ObjectPhysics(PhysicsType physicsType, ushort unknown)
        {
            ObjectPhysicsMode = (int)physicsType;
            UnknownuUnSignedshort = unknown;
        }


        public ItemSettings0ObjectPhysics()
        {
        }

        /// <summary>
        /// Unknown but its seems to be related to dynamic / static items
        /// (0 Normal, 1 Fixed, 2 Phased)
        /// </summary>
        [Id(1)]
        public int ObjectPhysicsMode { get; set; }

        [Id(10)] public ushort UnknownuUnSignedshort { get; set; } = default;
    }
}