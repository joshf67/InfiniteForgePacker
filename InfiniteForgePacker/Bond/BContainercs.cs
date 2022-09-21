using System.Runtime.Serialization;
using System.Collections.Generic;
using Bond;

namespace BondReader;

public class BContainer : BElement
{
    public List<BElement> Children = new List<BElement>();
    public string ListType = "";

    public BContainer(BondDataType type, int id = -1, string listType = "") : base (type, null, id)
    {
        ListType = listType;
    }

    public BElement AddChild(BElement child)
    {
        Children.Add(child);
        return child;
    }

    public BContainer AddChildContainer(BContainer child)
    {
        Children.Add(child);
        return child;
    }

    // public override string ConvertToString(int index)
    // {
    //     var ret = "";
    //     if (ListType != "")
    //     {
    //         if (ListType == "struct")
    //         {
    //             var newStructName = "Struct" + (currentStructName++).ToString();
    //             ret += (Id == -1 ? "0: " : Id.ToString() + ": ") + "list<" + newStructName + "> listVal" + index + "; \n } \n";
    //             for (var i = 0; i < Children.Count; i++)
    //             {
    //                 ret += Children[i].ConvertToString(i);
    //             }
    //         }
    //         else
    //         {
    //             if (Children.Count != 0)
    //             {
    //                 ret += (Id == -1 ? "0: " : Id.ToString() + ": ") + "list<" + ConvertTypeToString(Children[0].DataType) + "> listVal" + index + "; \n";
    //             }
    //         }
    //     }
    //     else if (DataType == BondDataType.BT_STRUCT)
    //     {
    //         
    //     }
    //     else
    //     {
    //         throw new NotImplementedException();
    //     }
    //
    //     return ret;
    // }
    
    
}