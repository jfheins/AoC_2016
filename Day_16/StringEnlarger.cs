using System;
using System.Linq;

namespace Day_16
{
    public class StringEnlarger
    {
        public static string Enlarge(string a)
        {
            var b = string.Concat(a.Reverse().Select(c => (char)('a' - c)));
            return string.Concat(a, "0", b);
        }

        public static string EnlargeToLength(string seed, int length)
        {
            var result = seed;
            while (result.Length < length)
            {
                result = Enlarge(result);
            }

            return result.Substring(0, length);
        }

        public static string CalculateChecksum(string content)
        {
            var checksum = string.Concat(content.Pairwise().Select(ChecksumBitFromPair));
            if (checksum.Length % 2 == 0)
                return CalculateChecksum(checksum);
            
            return checksum;
        }

        private static char ChecksumBitFromPair(Tuple<char, char> pair)
        {
            return pair.Item1 == pair.Item2 ? '1' : '0';
        }
    }
}