using System.Runtime.Serialization;
using Bond;

namespace BondReader;

public class BElement
{
    public int Id = -1;
    public BondDataType DataType;
    public object Data;
    // public static int currentStructName = 0;

    public BElement(BondDataType type, object data, int id = -1)
    {
        DataType = type;
        Data = data;
        Id = id;
    }

    public BContainer ConvertToStruct()
    {
        return new BContainer(DataType, Id);
    }

    public BContainer ConvertToList(string listType = "")
    {
        return new BContainer(DataType, Id, listType);
    }

    // public virtual string ConvertToString(int index)
    // {
    //     return (Id == -1 ? "0: " : Id.ToString() + ": ") + ConvertTypeToString(DataType) + " " + ConvertTypeToString(DataType) + "_" + index.ToString() + ";";
    // }
    //
    // public static string ConvertTypeToString(BondDataType dataType)
    // {
    //     switch (dataType)
    //     {
    //         case BondDataType.BT_STRUCT:
    //         {
    //             return "struct";
    //         }
    //
    //         case BondDataType.BT_LIST:
    //         {
    //             return "list";
    //         }
    //
    //         case BondDataType.BT_SET:
    //         {
    //             return "set";
    //         }
    //
    //         case BondDataType.BT_MAP:
    //         {
    //             return "map";
    //         }
    //         case BondDataType.BT_STRING:
    //         {
    //             return "string"; 
    //         }
    //
    //         case BondDataType.BT_WSTRING:
    //         {
    //             return "wstring"; 
    //         }
    //
    //         case BondDataType.BT_BOOL:
    //         {
    //             return "bool"; 
    //         }
    //
    //         case BondDataType.BT_DOUBLE:
    //         {
    //             return "double"; 
    //         }
    //
    //         case BondDataType.BT_FLOAT:
    //         {
    //             return "float"; 
    //         }
    //
    //         case BondDataType.BT_INT8:
    //         {
    //             return "int8"; 
    //         }
    //
    //         case BondDataType.BT_INT16:
    //         {
    //             return "int16"; 
    //         }
    //
    //         case BondDataType.BT_INT32:
    //         {
    //             return "int32"; 
    //         }
    //
    //         case BondDataType.BT_INT64:
    //         {
    //             return "int64";
    //         }
    //
    //         case BondDataType.BT_UINT8:
    //         {
    //             return "uint8";
    //         }
    //
    //         case BondDataType.BT_UINT16:
    //         {
    //             return "uint16";
    //         }
    //
    //         case BondDataType.BT_UINT32:
    //         {
    //             return "uint32";
    //         }
    //
    //         case BondDataType.BT_UINT64:
    //         {
    //             return "uint64";
    //         }
    //
    //         default:
    //             throw new NotImplementedException();
    //     }
    // }
    
    
}