using Bond;
using Bond.IO.Safe;
using Bond.Protocols;
using BondReader.Schemas;

namespace BondReader;

public class BondHelper
{
    public static T ProcessFile<T>(string inputFilePath, ushort version = 2)
    {
        var inputBuffer = new Bond.IO.Unsafe.InputBuffer(File.ReadAllBytes(inputFilePath));
        var reader = new CompactBinaryReader<Bond.IO.Unsafe.InputBuffer>(inputBuffer, version);


        return Deserialize<T>.From(reader);
    }

    public static ArraySegment<byte> WriteBond<T>(T src)
    {
        var output = new OutputBuffer();
        var writer = new CompactBinaryWriter<OutputBuffer>(output);

        // The first calls to Serialize.To and Deserialize<T>.From can take
        // a relatively long time because they generate the de/serializer
        // for a given type and protocol.
        Serialize.To(writer, src);


        return output.Data;
    }

    public static T ReadBond<T>(ArraySegment<byte> obj)
    {
        var input = new InputBuffer(obj);
        var reader = new CompactBinaryReader<InputBuffer>(input);

        return Deserialize<T>.From(reader);
    }
}