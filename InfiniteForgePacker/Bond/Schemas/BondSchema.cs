using System.Drawing;
using Bond;
using BondReader.Schemas.Generic;
using BondReader.Schemas.Items;
using InfiniteForgeConstants;
using InfiniteForgeConstants.MapSettings;

namespace BondReader.Schemas;

[Bond.Schema]
public class BondSchema
{
    public BondSchema(Map map)
    {
        MapIdContainer = new MapSchema(map);

        if (map.GameObjects == null) return;

        foreach (var gameObject in map.GameObjects)
        {
            Items.AddFirst(new ItemSchema(gameObject));
        }
    }

    public BondSchema(MapId mapid)
    {
        MapIdContainer = new MapSchema(mapid);
        MapIdContainer.MapId.Int = (int)mapid;
       
    }

    public BondSchema()
    {
    }

    [Id(1)] public MapSchema MapIdContainer { get; set; } = new MapSchema();

    [Id(3)] public LinkedList<ItemSchema> Items { get; set; } = new LinkedList<ItemSchema>();
    [Id(6)] public DebugStruct FolderContainer { get; set; } = new DebugStruct();
    [Id(7)] public DebugStruct id7Struct { get; set; } = new DebugStruct();

    [Id(10)] public DebugStruct id10Struct { get; set; } = new DebugStruct();
}