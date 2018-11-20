using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static string CalculateChecksum(IEnumerable<char> content)
        {
            var checksum = string.Concat(content.Pairwise().Select(ChecksumBitFromPair));
            if (checksum.Length % 2 == 0)
                return CalculateChecksum(checksum);
            
            return checksum;
		}
		public static string CalculateChecksum(IEnumerable<char> content, int diskSize)
		{
			// One block of bits will become one bit of the checksum
			var blockSize = diskSize & ~(diskSize - 1);
			return string.Concat(content.Chunks(blockSize).Select(ChecksumBitFromBlock));
		}

		private static char ChecksumBitFromBlock(IEnumerable<char> block)
		{
			var parity = block.Sum(bit => bit - '0') % 2;
			return (char) ((1 - parity) + '0');
		}

		private static char ChecksumBitFromPair(Tuple<char, char> pair)
        {
            return pair.Item1 == pair.Item2 ? '1' : '0';
        }
    }

    public class CharProvider
    {
        private static string[] _ab;
        public int ContentLength { get; }
        public string Seed { get; }

        public CharProvider(string seed, int contentLength)
        {
            Seed = seed;
            ContentLength = contentLength;
			_ab = new string[2];
			_ab[0] = seed;
			_ab[1] = string.Concat(seed.Reverse().Select(c => (char) ('a' - c)));
        }

        public char CharAt(int position)
        {
			Debug.Assert(position < ContentLength, "Position too large");
            
            if (ContentLength <= Seed.Length)
            {
                // No expansion has taken place
                return Seed[position];
            }

            var blockNumber = Math.DivRem(position, Seed.Length + 1, out var posInBlock);

            if (posInBlock == Seed.Length)
            {
                if (blockNumber % 2 == 0)
                {
                    return (blockNumber % 4 == 0) ? '0' : '1';
                }
                // Dragon bit
                var levelPow = ~blockNumber & (blockNumber + 1);
				var offset = levelPow - 1;
				var step = levelPow * 4;
				return (blockNumber - offset) % step == 0 ? '0' : '1';
            }

			var idx = blockNumber % 2;
			return _ab[idx][posInBlock];
		}
	}
}