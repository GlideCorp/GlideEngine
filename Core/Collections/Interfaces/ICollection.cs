
using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Interfaces
{
    /// <summary>
    /// The interface of a generic collection
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface ICollection<in TValue>
    {
        /// <summary>
        /// The number of elements in the list.
        /// </summary>
        public int Count { get; }

        public bool IsEmpty => Count == 0;

        /// <summary>
        /// Insert the value at the default collection location.
        /// </summary>
        /// <param name="value"></param>
        public void Insert(TValue value);

        /// <summary>
        /// Removes every value from the list
        /// </summary>
        public void Clear();
    }
}