using System.Security.Cryptography;

namespace Fleet.Core;

public class SequentialGuid
{
    public static Guid NewGuid()
    {
        return NewGuid(DateTimeOffset.UtcNow);
    }

    public static Guid NewGuid(DateTimeOffset dateTimeOffset)
    {
        // bytes [0-5]: datetimeoffset yyyy-MM-dd hh:mm:ss fff
        // bytes [6]: 4 bits dedicated to guid version (version: 7)
        // bytes [6]: 4 bits dedicated to random part
        // bytes [7-15]: random part
        byte[] uuidAsBytes = new byte[16];
        FillTimePart(ref uuidAsBytes, dateTimeOffset);
        Span<byte> randomPart = uuidAsBytes.AsSpan()[6..];
        RandomNumberGenerator.Fill(randomPart);
        // add mask to set guid version
        uuidAsBytes[6] &= 0x0F;
        uuidAsBytes[6] += 0x70;
        return new Guid(uuidAsBytes, true);
    }

    private static void FillTimePart(ref byte[] uuidAsBytes, DateTimeOffset dateTimeOffset)
    {
        long currentTimestamp = dateTimeOffset.ToUnixTimeMilliseconds();
        byte[] current = BitConverter.GetBytes(currentTimestamp);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(current);
        }

        current[2..8].CopyTo(uuidAsBytes, 0);
    }
}