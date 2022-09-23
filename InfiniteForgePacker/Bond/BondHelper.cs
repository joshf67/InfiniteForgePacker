using Bond;
using Bond.IO.Unsafe;
using Bond.Protocols;
using BondReader.Schemas;
using InputBuffer = Bond.IO.Safe.InputBuffer;
using OutputBuffer = Bond.IO.Safe.OutputBuffer;

namespace BondReader;

public class BondHelper
{
    public static T ProcessFile<T>(string inputFilePath, ushort version = 2)
    {
        var inputBuffer = new Bond.IO.Unsafe.InputBuffer(File.ReadAllBytes(inputFilePath));
        var reader = new CompactBinaryReader<Bond.IO.Unsafe.InputBuffer>(inputBuffer, version);


        return Deserialize<T>.From(reader);
    }

    public static void WriteBond<T>(T src, string filePath)
    {
        //var output = new OutputBuffer();
        ;

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            var output = new OutputStream(stream);
            var writer = new CompactBinaryWriter<OutputStream>(output, 2);

            // The first calls to Serialize.To and Deserialize<T>.From can take
            // a relatively long time because they generate the de/serializer
            // for a given type and protocol.
            Serialize.To(writer, src);
            output.Flush();
            stream.Flush();
        }
    }

    public static T ReadBond<T>(ArraySegment<byte> obj)
    {
        var input = new InputBuffer(obj);
        var reader = new CompactBinaryReader<InputBuffer>(input);

        return Deserialize<T>.From(reader);
    }
}