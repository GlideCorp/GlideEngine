
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace Core.Locations
{
    /*
     * CompressedChar
     *      'a': 1,
     *      ...,
     *      'z': 26,
     *      '': 27,
     *      '': 28,
     *      '/': 29,
     *      '.': 30,
     *      '_': 31,
     *      ':': 32
     *
     *  x = 1 bit
     * |xxxxx|xxxxx|xxxxx|x| = 1 short = 3 compressed chars with only 1 wasted bit
     * |xxxxx|xxxxx|xxxxx|x| |xxxxx|xxxxx|xxxxx|x| = 1 int = 2 short = 6 compressed chars with 2 wasted bits
     * |xxxxx|xxxxx|xxxxx|x| |xxxxx|xxxxx|xxxxx|x| |xxxxx|xxxxx|xxxxx|x| |xxxxx|xxxxx|xxxxx|x| = 1 long = 4 short = 12 compressed chars with 4 wasted bits
     */

    public class Location(ReadOnlySpan<char> alphabeticalLocation) : IEquatable<Location>
    {
        private const int BitsForCompressedChars = 5;
        private const int UnusedBits = 1;
        private const int CharsPerNumber = sizeof(ushort) * 8 / BitsForCompressedChars;
        private const int CharsPerNumberMinus1 = sizeof(ushort) * 8 / BitsForCompressedChars - 1;

        private ushort[] NumericalLocation { get; init; } = ConvertToNumerical(alphabeticalLocation);

        private static ushort CompressChar(char decompressedChar)
        {
            return decompressedChar switch
            {
                >= 'a' and <= 'z' => (ushort)(decompressedChar - 'a'),
                ':' => 31,
                '_' => 30,
                '.' => 29,
                '/' => 28,
                _ => 27,
            };
        }

        private static char DecompressChar(ushort compressedChar)
        {
            return compressedChar switch
            {
                <= 'z' - 'a' => (char)(compressedChar + 'a'),
                31 => ':',
                30 => '_',
                29 => '.',
                28 => '/',
                _ => '\0',
            };
        }

        private static ushort[] ConvertToNumerical(ReadOnlySpan<char> alphabeticalLocation)
        {
            int numericalParts = (alphabeticalLocation.Length + CharsPerNumberMinus1) / CharsPerNumber;
            ushort[] numericalLocations = numericalParts == 0 ? [] : new ushort[numericalParts];

            for (int i = 0; i < alphabeticalLocation.Length; i++)
            {
                int numericalIndex = i / CharsPerNumber;
                int shiftAmount = (CharsPerNumberMinus1 - i % CharsPerNumber) * BitsForCompressedChars + UnusedBits;

                numericalLocations[numericalIndex] |= (ushort)(CompressChar(alphabeticalLocation[i]) << shiftAmount);
            }

            for (int i = alphabeticalLocation.Length % CharsPerNumber; i < 3; i++)
            {
                int shiftAmount = (CharsPerNumberMinus1 - i % CharsPerNumber) * BitsForCompressedChars + UnusedBits;

                numericalLocations[^1] |= (ushort)(CompressChar('\0') << shiftAmount);
            }

            return numericalLocations;
        }

        private static ushort GetCompressedChar(ushort number, int position)
        {
            int shiftAmount = BitsForCompressedChars * position + UnusedBits;
            return (ushort)((number >> shiftAmount) & 0b11111);
        }

        private static string ConvertToAlphabetical(ushort[] numericalLocation)
        {
            int characterLength = numericalLocation.Length * 8;
            StringBuilder builder = new(characterLength);

            for (int i = 0; i < numericalLocation.Length; i++)
            {
                builder.Append(DecompressChar(GetCompressedChar(numericalLocation[i], position: 2)));
                builder.Append(DecompressChar(GetCompressedChar(numericalLocation[i], position: 1)));
                builder.Append(DecompressChar(GetCompressedChar(numericalLocation[i], position: 0)));
            }

            return builder.ToString();
        }

        public bool Equals(Location? other)
        {
            if (other is null || NumericalLocation.Length != other.NumericalLocation.Length) { return false; }

            int arrayLength = NumericalLocation.Length;
            int remaining = arrayLength - arrayLength % Vector<ulong>.Count;
            ReadOnlySpan<ushort> leftSpan = NumericalLocation.AsSpan();
            ReadOnlySpan<ushort> rightSpan = other.NumericalLocation.AsSpan();

            ReadOnlySpan<Vector<ushort>> left = MemoryMarshal.Cast<ushort, Vector<ushort>>(leftSpan);
            ReadOnlySpan<Vector<ushort>> right = MemoryMarshal.Cast<ushort, Vector<ushort>>(rightSpan);
            Vector<ushort> allBits = Vector<ushort>.AllBitsSet;
            int vectorSpanLength = left.Length;

            for (int i = 0; i < vectorSpanLength; i++)
            {
                var result = Vector.Equals(left[i], right[i]);
                if (result != allBits) { return false; }
            }

            for (int i = remaining; i < arrayLength; i++) { if (leftSpan[i] != rightSpan[i]) { return false; } }

            return true;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            return obj.GetType() == GetType() && Equals((Location)obj);
        }

        public override int GetHashCode()
        {
            return NumericalLocation.GetHashCode();
        }

        public static bool operator ==(Location left, Location right) { return left.Equals(right); }
        public static bool operator !=(Location left, Location right) { return !left.Equals(right); }

        public override string ToString()
        {
            return $"<{ConvertToAlphabetical(NumericalLocation)}>";
        }
    }
}
