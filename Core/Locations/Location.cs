
using System;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace Core.Locations
{
    /*
     * ZippedChar:
     *     'a': 0,
     *      .
     *      .
     *      .
     *     'z': 25,
     *     '/': 26
     *     '_': 27,
     *     '<': 28,
     *     '>': 29,
     *     '.': 30,
     *     '\0': 31, // no char
     *
     *  x = 1 bit
     * |xxxxx|xxxxx|xxxxx|x| = 1 short = 3 compressed chars with only 1 wasted bit
     * |xxxxx|xxxxx|xxxxx|x| |xxxxx|xxxxx|xxxxx|x| = 1 int = 2 short = 6 compressed chars with 2 wasted bits
     * |xxxxx|xxxxx|xxxxx|x| |xxxxx|xxxxx|xxxxx|x| |xxxxx|xxxxx|xxxxx|x| |xxxxx|xxxxx|xxxxx|x| = 1 long = 4 short = 12 compressed chars with 4 wasted bits
     */

    public class Location : IEquatable<Location>
    {
        private const char Separator = ':';
        private const int ZippedCharsSize = 5;
        private const int UnusedBits = 1;
        private const int CharsPerValue = sizeof(ushort) * 8 / ZippedCharsSize;

        private ushort[] NumericalLocation { get; init; }
        private int[] SeparationIndices { get; init; }

        public Location(ReadOnlySpan<char> alphabeticalLocation)
        {
            ConvertToNumerical(alphabeticalLocation, out ushort[] numericalLocation, out int[] separationIndices);
            NumericalLocation = numericalLocation;
            SeparationIndices = separationIndices;
        }

        private static void ToBinary(ushort number)
        {
            StringBuilder builder = new();
            int counter = 0;

            for (int i = sizeof(ushort) * 8 - 1; i >= 0; i--)
            {
                counter++;
                builder.Append((number & (0b1 << i)) > 0 ? '1' : '0');
                if ((sizeof(ushort) * 8 - i) % 16 == 0)
                {
                    builder.Append('|');
                    counter = 0;
                }
                else if (counter % 5 == 0) { builder.Append('|'); }
            }

            Console.WriteLine(builder.ToString());
        }

        private static ushort Zip(char c)
        {
            return c switch
            {
                >= 'a' and <= 'z' => (ushort)(c - 'a'),
                '/' => 26,
                '_' => 27,
                '<' => 28,
                '>' => 29,
                '.' => 30,
                _ => 31
            };
        }

        private static void CountPartsAndSeparations(ReadOnlySpan<char> alphabeticalLocation, out int parts, out int separations)
        {
            parts = separations = 0;
            int counter = 0;

            for (int i = 0; i < alphabeticalLocation.Length; i++)
            {
                if (alphabeticalLocation[i] == Separator)
                {
                    parts++;
                    separations++;
                    counter = 0;
                }
                else if (counter + 1 == CharsPerValue)
                {
                    parts++;
                    counter = 0;
                }
                else { counter++; }
            }

            if (counter > 0) { parts++; }
        }

        private static void ConvertToNumerical(ReadOnlySpan<char> alphabeticalLocation, out ushort[] numericalLocation, out int[] separationIndices)
        {
            CountPartsAndSeparations(alphabeticalLocation, out int parts, out int separations);
            numericalLocation = new ushort[parts];
            separationIndices = new int[separations];

            int numericalLocationIndex = 0;
            int numericalLocationCounter = 0;
            int separationIndex = 0;
            int shift;

            for (int i = 0; i < alphabeticalLocation.Length; i++)
            {
                if (alphabeticalLocation[i] == Separator)
                {
                    while (numericalLocationCounter < CharsPerValue)
                    {
                        shift = (CharsPerValue - numericalLocationCounter++ - 1) * ZippedCharsSize + UnusedBits;
                        numericalLocation[numericalLocationIndex] |= (ushort)(Zip(Separator) << shift);
                        //ToBinary(numericalLocation[numericalLocationIndex]);
                    }

                    separationIndices[separationIndex++] = ++numericalLocationIndex;
                    numericalLocationCounter = 0;
                    continue;
                }

                shift = (CharsPerValue - numericalLocationCounter++ - 1) * ZippedCharsSize + UnusedBits;
                numericalLocation[numericalLocationIndex] |= (ushort)(Zip(alphabeticalLocation[i]) << shift);
                //ToBinary(numericalLocation[numericalLocationIndex]);

                if (numericalLocationCounter != CharsPerValue) { continue; }
                numericalLocationIndex++;
                numericalLocationCounter = 0;
            }

            while (numericalLocationCounter is > 0 and < CharsPerValue)
            {
                shift = (CharsPerValue - numericalLocationCounter++ - 1) * ZippedCharsSize + UnusedBits;
                numericalLocation[numericalLocationIndex] |= (ushort)(Zip(Separator) << shift);
            }
        }

        private static void Unzip(ushort number, out char c1, out char c2, out char c3)
        {
            byte a = (byte)(number >> 11 & 0b11111);
            byte b = (byte)(number >> 6 & 0b11111);
            byte c = (byte)(number >> 1 & 0b11111);

            c1 = a switch
            {
                <= 'z' - 'a' => (char)(a + 'a'),
                26 => '/',
                27 => '_',
                28 => '<',
                29 => '>',
                30 => '.',
                _ => Separator
            };

            c2 = b switch
            {
                <= 'z' - 'a' => (char)(b + 'a'),
                26 => '/',
                27 => '_',
                28 => '<',
                29 => '>',
                30 => '.',
                _ => Separator
            };

            c3 = c switch
            {
                <= 'z' - 'a' => (char)(c + 'a'),
                26 => '/',
                27 => '_',
                28 => '<',
                29 => '>',
                30 => '.',
                _ => Separator
            };
        }

        private static string ConvertToAlphabetical(ushort[] numericalLocation)
        {
            int characterLength = numericalLocation.Length * CharsPerValue;
            StringBuilder builder = new(characterLength);

            for (int i = 0; i < numericalLocation.Length; i++)
            {
                Unzip(numericalLocation[i], out char c1, out char c2, out char c3);

                if (c1 == Separator) { builder.Append($"{c1}"); continue; }
                if (c2 == Separator) { builder.Append($"{c1}{c2}"); continue; }
                builder.Append($"{c1}{c2}{c3}");
            }

            return builder.ToString();
        }


        private static bool ArrayEqual<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
            where T : struct, IEqualityOperators<T, T, bool>
        {
            // most common case: first element is different.

            if (a[0] != b[0]) { return false; }

            int length = a.Length;
            int remaining = length % Vector<T>.Count;

            for (int i = 1; i < length - remaining; i += Vector<T>.Count)
            {
                var v1 = new Vector<T>(a[i..]);
                var v2 = new Vector<T>(b[i..]);
                if (v1 != v2) { return false; }
            }

            for (int i = a.Length - remaining; i < length; i++) { if (a[i] != b[i]) { return false; } }

            return true;
        }

        public bool Equals(Location? other)
        {
            if (other is null ||
                SeparationIndices.Length != other.SeparationIndices.Length ||
                NumericalLocation.Length != other.NumericalLocation.Length) { return false; }

            return ArrayEqual<int>(SeparationIndices.AsSpan(), other.SeparationIndices.AsSpan()) &&
                   ArrayEqual<ushort>(NumericalLocation.AsSpan(), other.NumericalLocation.AsSpan());
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
