using System;
using System.Text;

public static class RandomIdGenerator
{
    private static char[] _base62chars =
        "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"
        .ToCharArray();

    private static System.Random _random = new System.Random();

    public static int GetBase62(int length)
    {
        var sb = new StringBuilder(length);

        for (int i = 0; i < length; i++)
            sb.Append(_base62chars[_random.Next(62)]);

        int retValue = Convert.ToInt32(sb.ToString());

        return retValue;
    }

    public static string GetBase36(int length)
    {
        var sb = new StringBuilder(length);

        for (int i = 0; i < length; i++)
            sb.Append(_base62chars[_random.Next(36)]);

        return sb.ToString();
    }
}