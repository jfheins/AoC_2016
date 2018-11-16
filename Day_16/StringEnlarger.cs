using System;
using System.Collections.Generic;
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

        private static string b;

        public static char CharAtPosition(string seed, int contentLength, int position)
        {
            if (position >= contentLength)
            {
                throw new ArgumentException("Position too large");
            }
            if (contentLength <= seed.Length)
            {
                // No expansion has taken place
                return seed[position];
            }

            var blockNumber = Math.DivRem(position, seed.Length + 1, out var posInBlock);

            if (posInBlock == seed.Length)
            {
                // Dragon bit
                return '_';
            }
            else
            {
                if (blockNumber % 2 == 0)
                {
                    // 'a' block
                    return seed[posInBlock];
                }
                else
                {
                    b = b ?? string.Concat(seed.Reverse().Select(c => (char) ('a' - c)));
                    return b[posInBlock];
                }
            }
        }

        public static string CalculateChecksum(IEnumerable<char> content)
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