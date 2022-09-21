using Bond;
using Bond.Protocols;

namespace BondReader;

public class BondHelper
{
    public static T ProcessFile<T>(string inputFilePath, ushort version = 2)
    {
        var inputBuffer = new Bond.IO.Unsafe.InputBuffer(File.ReadAllBytes(inputFilePath));
        var reader = new CompactBinaryReader<Bond.IO.Unsafe.InputBuffer>(inputBuffer, version);


        return Deserialize<T>.From(reader);
    }
}