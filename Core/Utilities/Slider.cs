
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Core.Utilities
{
    public static class Slider
    {
        public static IEnumerable<T> Slide<T>(ReadOnlyCollection<T> colection) { return Slide(colection, from: 0, to: colection.Count); }
        public static IEnumerable<T> Slide<T>(ReadOnlyCollection<T> colection, int from) { return Slide(colection, from, to: colection.Count); }
        public static IEnumerable<T> Slide<T>(ReadOnlyCollection<T> colection, int from, int to)
        {
            for (int i = from; i < to; i++) { yield return colection.ElementAt(i); }
        }

        public static bool StartsWith<T>(ReadOnlyCollection<T> collection, ReadOnlyCollection<T> with)
            where T : IEquatable<T>
        {
            bool result = with.Count <= collection.Count;
            for (int i = 0; result && i < with.Count; i++) { result = with.ElementAt(i).Equals(collection.ElementAt(i)); }
            return result;
        }

        public static bool StartsWith<T>(ReadOnlyCollection<T> collection, ReadOnlyCollection<T> with, Func<T, T, bool> equal)
            where T : IEquatable<T>
        {
            bool result = with.Count <= collection.Count;
            for (int i = 0; result && i < with.Count; i++) { result = equal(with.ElementAt(i), collection.ElementAt(i)); }
            return result;
        }
    }
}
