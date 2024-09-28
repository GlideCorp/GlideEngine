
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections
{
    /// <summary>
    /// This class allows you to implement a method to filter elements in a <see cref="ICollection{TKey,TValue}"/>>
    /// based on a key type, without the need to create a new object to use the compare method
    /// </summary>
    /// <typeparam name="TKey">The key type with which to search for the match</typeparam>
    /// <typeparam name="TValue">The value type passed by <see cref="ICollection{TKey,TValue}.Search(TKey,out TValue?)"/>></typeparam>
    public interface IFilter<TKey, in TValue>
    {
        /// <summary>
        /// The key with which to search for the match
        /// </summary>
        public TKey Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">The value passed by collection.filter()</param>
        /// <returns>true if value matches the key</returns>
        public bool Match(TValue value);
    }

    /// <summary>
    /// The basic implementation of <see cref="IFilter{TKey,TValue}"/> if the key type and value type are the same
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class DefaultFilter<TValue> : IFilter<TValue, TValue> where TValue : IComparable<TValue>, IEquatable<TValue>
    {
        public TValue Key { get; set; } = default!;

        public bool Match(TValue value) { return Key.Equals(value); }
    }

    /// <summary>
    /// The interface of a generic collection that searches for values ​​with <see cref="IFilter{TKey,TValue}"/>>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface ICollection<TKey, TValue>
    {
        /// <summary>
        /// The default filter to search with if not specified
        /// </summary>
        public IFilter<TKey, TValue> DefaultFilter { get; }

        /// <summary>
        /// The number of elements in the list
        /// </summary>
        public int Count { get; }

        public bool IsPacked { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns>The value at the index</returns>
        public TValue this[int index] { get; }

        /// <summary>
        /// Insert the value at the default collection location
        /// </summary>
        /// <param name="value"></param>
        public void Insert(TValue value);

        /// <summary>
        /// Insert the value at the specified index
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public void InsertAt(TValue value, int index);

        /// <summary>
        /// Remove the first value that matches the key requirements with <see cref="DefaultFilter"/>
        /// </summary>
        /// <param name="key"></param>
        public void Remove(TKey key);

        /// <summary>
        /// Remove the first value that matches the key requirements with the custom filter
        /// </summary>
        /// <param name="filter"></param>
        public void Remove(IFilter<TKey, TValue> filter);

        /// <summary>
        /// Remove the value at the specified index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index);

        /// <summary>
        /// Search the first value that matches the key requirements with <see cref="DefaultFilter"/>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Search(TKey key, [NotNullWhen(true)] out TValue? value);

        /// <summary>
        /// Search the first value that matches the key requirements with the custom filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Search(IFilter<TKey, TValue> filter, [NotNullWhen(true)] out TValue? value);

        /// <summary>
        /// Removes every value from the list
        /// </summary>
        public void Clear();

        /// <summary>
        /// Prevents values to be added or removed.
        /// </summary>
        public void Pack();

        /// <summary>
        /// Returns all the values that matches the key with <see cref="DefaultFilter"/> one at a time
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<TValue> Filter(TKey key);

        /// <summary>
        /// Returns all the values that matches the key with the custom filter one at a time
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<TValue> Filter(IFilter<TKey, TValue> filter);
    }
}