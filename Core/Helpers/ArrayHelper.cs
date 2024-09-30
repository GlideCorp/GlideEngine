
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Core.Helpers
{
    #region Fill
    public static partial class ArrayHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FillPrivate<T>(T[] array, T value, int offset, int length)
        {
            Span<T> arraySpan = array.AsSpan(offset, length);
            arraySpan.Fill(value);
        }

        public static void Fill<T>(T[] array, T value)
        {
            FillPrivate(array, value, offset: 0, length: array.Length);
        }

        public static void FillAfter<T>(T[] array, T value, int afterInclusive)
        {
            /* constraints:
             *  - after inclusive >= 0
             *  - after inclusive < array length
             */
            Debug.Assert(afterInclusive >= 0 && afterInclusive < array.Length);

            int length = afterInclusive - array.Length;
            FillPrivate(array, value, offset: afterInclusive, length);
        }

        public static void FillUntil<T>(T[] array, T value, int untilExclusive)
        {
            /* constraints:
             *  - until exclusive >= 0
             *  - until exclusive <= array length
             */
            Debug.Assert(untilExclusive >= 0 && untilExclusive <= array.Length);
            FillPrivate(array, value, offset: 0, length: untilExclusive);
        }

        public static void FillBlock<T>(T[] array, T value, int startInclusive, int endExclusive)
        {
            /* constraints:
             *  - start inclusive >= 0
             *  - start inclusive < array length
             *
             *  - end exclusive >= start inclusive
             *  - end exclusive <= array length
             */
            Debug.Assert(startInclusive >= 0 && startInclusive < array.Length);
            Debug.Assert(endExclusive >= startInclusive && endExclusive <= array.Length);

            int length = endExclusive - startInclusive;
            FillPrivate(array, value, offset: startInclusive, length);
        }

        public static void FillOffset<T>(T[] array, T value, int offset, int length)
        {
            /* constraints:
             *  - offset >= 0
             *  - offset < array length
             *
             *  - offset + length <= array length
             */
            Debug.Assert(offset >= 0 && offset < array.Length);
            Debug.Assert(offset + length < array.Length);

            FillPrivate(array, value, offset, length);
        }
    }
    #endregion

    #region Copy
    public static partial class ArrayHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CopyPrivate<T>(T[] source, T[] destination, int sourceOffset, int destinationOffset, int length)
        {
            Span<T> sourceSpan = source.AsSpan(sourceOffset, length);
            Span<T> destinationSpan = destination.AsSpan(destinationOffset, length);
            sourceSpan.CopyTo(destinationSpan);
        }

        public static void Copy<T>(T[] source, T[] destination)
        {
            /* constraints:
             *  - length < min(source length, destination length)
             */
            int length = Math.Min(source.Length, destination.Length);
            CopyPrivate(source, destination, sourceOffset: 0, destinationOffset: 0, length);
        }

        public static void CopyAfter<T>(T[] source, T[] destination, int afterInclusive)
        {
            /* constraints:
             *  - after inclusive >= 0
             *  - after inclusive < min(source length, destination length)
             */
            Debug.Assert(afterInclusive >= 0 && afterInclusive < Math.Min(source.Length, destination.Length));

            int length = Math.Min(source.Length, destination.Length) - afterInclusive;
            CopyPrivate(source, destination, sourceOffset: afterInclusive, destinationOffset: afterInclusive, length);
        }

        public static void CopyUntil<T>(T[] source, T[] destination, int untilExclusive)
        {
            /* constraints:
             *  - until exclusive >= 0
             *  - until exclusive <= min(source length, destination length)
             */
            Debug.Assert(untilExclusive >= 0 && untilExclusive <= Math.Min(source.Length, destination.Length));

            CopyPrivate(source, destination, sourceOffset: 0, destinationOffset: 0, length: untilExclusive);
        }

        public static void CopyBlock<T>(T[] source, T[] destination, int startInclusive, int endExclusive)
        {
            /* constraints:
             *  - start inclusive >= 0
             *  - start inclusive < min(source length, destination length)
             *
             *  - end exclusive >= start
             *  - end exclusive <= min(source length, destination length)
             */
            Debug.Assert(startInclusive >= 0 && startInclusive < Math.Min(source.Length, destination.Length));
            Debug.Assert(endExclusive >= startInclusive && endExclusive <= Math.Min(source.Length, destination.Length));

            int length = endExclusive - startInclusive;
            CopyPrivate(source, destination, sourceOffset: startInclusive, destinationOffset: startInclusive, length);
        }

        public static void CopyOffset<T>(T[] source, T[] destination, int sourceOffset, int destinationOffset, int length)
        {
            /* constraints:
             *  - sourceOffset >= 0
             *  - sourceOffset + length < source length
             *
             *  - destinationOffset >= 0
             *  - destinationOffset + length < destination length
             */
            Debug.Assert(sourceOffset >= 0 && sourceOffset + length < source.Length);
            Debug.Assert(destinationOffset >= 0 && destinationOffset + length < destination.Length);
            CopyPrivate(source, destination, sourceOffset, destinationOffset, length);
        }
    }
    #endregion
}
