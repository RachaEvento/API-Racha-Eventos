using System.Security.Cryptography;

namespace OrganizadorEventos.Util;

public static class ConviteUtil
{
    public static string GuidTo12DigitId(Guid guid)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(guid.ToByteArray());

            // Take first 6 bytes = 48 bits (~13 digits, we'll trim to 12)
            ulong value = BitConverter.ToUInt64(hashBytes, 0) & 0x0000FFFFFFFFFFFF; // mask to 48 bits

            // Ensure it's exactly 12 digits (pad if necessary)
            return (value % 1_000_000_000_000).ToString("D12");
        }
    }
}