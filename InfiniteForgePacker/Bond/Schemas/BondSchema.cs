using Bond;
using BondReader.Schemas.Generic;
using BondReader.Schemas.Items;
using InfiniteForgeConstants;

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
    
    [Id(1)] public MapSchema MapIdContainer { get; set; }

    [Id(3)] public LinkedList<ItemSchema> Items { get; set; } = new LinkedList<ItemSchema>();
    [Id(6)] public DebugStruct FolderContainer { get; set; }
    [Id(7)] public DebugStruct id7Struct { get; set; }

    [Id(10)] public DebugStruct id10Struct { get; set; }
}