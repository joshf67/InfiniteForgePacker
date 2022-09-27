using Bond;

namespace BondReader.Schemas.Generic;

[Bond.Schema]
public class GenericIntStruct
{
    public GenericIntStruct(int value)
    {
        Int = value;
    }

    public GenericIntStruct()
    {
    }

    [Id(0)] public int Int { get; set; }
}

[Bond.Schema]
public class Vector3
{
    public Vector3(float x = 0, float y = 0, float z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3()
    {
        
    }

    public Vector3(System.Numerics.Vector3 vector3)
    {
        X = vector3.X;
        Y = vector3.Y;
        Z = vector3.Z;
    }

    [Id(0)] public float X { get; set; } = default;
    [Id(1)] public float Y { get; set; } = default;
    [Id(2)] public float Z { get; set; } = default;
}

[Bond.Schema]
public class Vector2
{
    public Vector2(float x = 0, float y = 0)
    {
        X = x;
        Y = y;
    }

    public Vector2(System.Numerics.Vector2 vector2)
    {
        X = vector2.X;
        Y = vector2.Y;
    }

    [Id(0)] public float X { get; set; } = default;
    [Id(1)] public float Y { get; set; } = default;
}

[Bond.Schema]
public class DebugStruct
{
    public DebugStruct()
    {
        
    }
}

[Bond.Schema]
public class UnknownStruct
{
    public UnknownStruct()
    {
        
    }
}