
using System;
using System.Collections;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Intrinsics;
using Microsoft.VisualBasic;

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
    * x = 1 byte
    * |xxxx|xxxx|xxxx|xxxx| = 1 int = 4 compressed chars
    * |xxxx|xxxx|xxxx|xxxx|xxxx|xxxx|xxxx|xxxx| = 1 long = 8 compressed chars
    */

    public class Location(ReadOnlySpan<char> alphabeticalLocation) : IEquatable<Location>
    {
        private ulong[] NumericalLocation { get; set; } = ConvertToNumerical(alphabeticalLocation);

        private static ulong CompressChar(char decompressedChar)
        {
            return decompressedChar switch
            {
                >= 'a' and <= 'z' => (ulong)(decompressedChar - 'a'),
                ':' => 31,
                '_' => 30,
                '.' => 29,
                '/' => 28,
                _ => 0,
            };
        }

        private static char DecompressChar(ulong compressedChar)
        {
            return ' ';
        }

        private static ulong[] ConvertToNumerical(ReadOnlySpan<char> alphabeticalLocation)
        {
            int numericalParts = (alphabeticalLocation.Length + 7) / 8;
            ulong[] numericalLocations = numericalParts == 0 ? [] : new ulong[numericalParts];

            for (int i = 0; i < alphabeticalLocation.Length; i++)
            {
                ulong compressedChar = CompressChar(alphabeticalLocation[i]);
                numericalLocations[i / 8] |= compressedChar << ((8 - i % 8 - 1) * 8);
            }

            return numericalLocations;
        }

        private static string ConvertToAlphabetical(ulong[] numericalLocation)
        {
            return "";
        }
        /*
        public bool Equals(Location? other)
        {
            if (other is null || NumericalLocation.Length != other.NumericalLocation.Length) { return false; }
           
           int length = NumericalLocation.Length;
           int remaining = length % Vector<ulong>.Count;
           ReadOnlySpan<ulong> leftSpan = NumericalLocation.AsSpan();
           ReadOnlySpan<ulong> rightSpan = other.NumericalLocation.AsSpan();
           Vector<ulong> allBitsSet = Vector<ulong>.AllBitsSet;
           ref ulong leftReference = ref MemoryMarshal.GetReference(leftSpan);
           ref ulong rightReference = ref MemoryMarshal.GetReference(rightSpan);
           
           for (int i = 0; i < length - remaining; i += Vector<ulong>.Count)
           {
               Vector<ulong> v = Vector.Equals(
                   Unsafe.As<ulong, Vector<ulong>>(ref Unsafe.Add(ref leftReference, i)),
                   Unsafe.As<ulong, Vector<ulong>>(ref Unsafe.Add(ref rightReference, i)));
           
               if (v != allBitsSet) { return false; }
           }
           
           for (int i = length - remaining; i < length; i++) { if (leftSpan[i] != rightSpan[i]) { return false; } }
           
           return true;
        }
        */

        public bool Equals(Location? other)
        {
            if (other is null || NumericalLocation.Length != other.NumericalLocation.Length) { return false; }

            int arrayLength = NumericalLocation.Length;
            int remaining = arrayLength - arrayLength % Vector<ulong>.Count;
            ReadOnlySpan<ulong> leftSpan = NumericalLocation.AsSpan();
            ReadOnlySpan<ulong> rightSpan = other.NumericalLocation.AsSpan();

            ReadOnlySpan<Vector<ulong>> left = MemoryMarshal.Cast<ulong, Vector<ulong>>(leftSpan);
            ReadOnlySpan<Vector<ulong>> right = MemoryMarshal.Cast<ulong, Vector<ulong>>(rightSpan);
            Vector<ulong> allBits = Vector<ulong>.AllBitsSet;
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
    }
}
