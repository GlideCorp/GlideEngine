using Core.Maths.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core.Maths.Matrices
{
    public class Matrix<T>(params T[] values) :
        IAdditionOperators<Matrix<T>, Matrix<T>, Matrix<T>>,
        ISubtractionOperators<Matrix<T>, Matrix<T>, Matrix<T>>,
        IMultiplyOperators<Matrix<T>, T, Matrix<T>>,
        IComparisonOperators<Matrix<T>, Matrix<T>, bool>,
        IUnaryNegationOperators<Matrix<T>, Matrix<T>>
        where T : INumber<T>
    {
        public T[] Values { get; set; } = values;
        public int RowLength { get; init; }
        public int ColumnLength { get; init; }

        public Matrix(int rowLength, int columnLength) : this(rowLength * columnLength == 0 ? [] : new T[rowLength * columnLength]) 
        {
            RowLength = rowLength;
            ColumnLength = columnLength;
        }

        public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right)
        {
            if (left.RowLength != right.RowLength) { throw new InvalidOperationException(); }
            if (left.ColumnLength != right.ColumnLength) { throw new InvalidOperationException(); }

            int size = left.Values.Length; //equals to left.RowLength* left.ColumnLength
            int numberOfOperations = System.Numerics.Vector<T>.Count;
            int remaining = size % numberOfOperations;
            Matrix<T> result = new(left.RowLength, left.ColumnLength);

            Span<T> resultSpan = result.Values.AsSpan();
            ReadOnlySpan<T> leftSpan = left.Values.AsSpan();
            ReadOnlySpan<T> rightSpan = right.Values.AsSpan();

            for (int i = 0; i < size - remaining; i += numberOfOperations)
            {
                var v1 = new System.Numerics.Vector<T>(leftSpan.Slice(i, numberOfOperations));
                var v2 = new System.Numerics.Vector<T>(rightSpan.Slice(i, numberOfOperations));
                (v1 + v2).CopyTo(resultSpan.Slice(i, numberOfOperations));
            }

            for (int i = size - remaining; i < size; i++) { result.Values[i] = left.Values[i] + right.Values[i]; }

            return result;
        }

        public static Matrix<T> operator -(Matrix<T> left, Matrix<T> right)
        {
            if (left.RowLength != right.RowLength) { throw new InvalidOperationException(); }
            if (left.ColumnLength != right.ColumnLength) { throw new InvalidOperationException(); }

            int size = left.Values.Length; //equals to left.RowLength* left.ColumnLength
            int numberOfOperations = System.Numerics.Vector<T>.Count;
            int remaining = size % numberOfOperations;
            Matrix<T> result = new(left.RowLength, left.ColumnLength);

            Span<T> resultSpan = result.Values.AsSpan();
            ReadOnlySpan<T> leftSpan = left.Values.AsSpan();
            ReadOnlySpan<T> rightSpan = right.Values.AsSpan();

            for (int i = 0; i < size - remaining; i += numberOfOperations)
            {
                var v1 = new System.Numerics.Vector<T>(leftSpan.Slice(i, numberOfOperations));
                var v2 = new System.Numerics.Vector<T>(rightSpan.Slice(i, numberOfOperations));
                (v1 - v2).CopyTo(resultSpan.Slice(i, numberOfOperations));
            }

            for (int i = size - remaining; i < size; i++) { result.Values[i] = left.Values[i] - right.Values[i]; }

            return result;
        }

        public static Matrix<T> operator *(Matrix<T> left, T right)
        {
            int size = left.Values.Length;
            int numberOfOperations = System.Numerics.Vector<T>.Count;
            int remaining = size % numberOfOperations;
            Matrix<T> result = new(left.RowLength, left.ColumnLength);

            Span<T> resultSpan = result.Values.AsSpan();
            ReadOnlySpan<T> leftSpan = left.Values.AsSpan();

            for (int i = 0; i < size - remaining; i += numberOfOperations)
            {
                var v1 = new System.Numerics.Vector<T>(leftSpan.Slice(i, numberOfOperations));
                (v1 * right).CopyTo(resultSpan.Slice(i, numberOfOperations));
            }

            for (int i = size - remaining; i < size; i++) { result.Values[i] = left.Values[i] * right; }

            return result;
        }
        public static Matrix<T> operator *(T left, Matrix<T> rigth) => rigth * left;

        public static Matrix<T> operator -(Matrix<T> value)
        {
            Matrix<T> result = new(value.RowLength, value.ColumnLength);
            ReadOnlySpan<T> valueSpan = value.Values.AsSpan();

            for (int i = 0; i < valueSpan.Length; i++) { result.Values[i] = -valueSpan[i]; }
            return result;
        }

        public static bool operator ==(Matrix<T>? left, Matrix<T>? right)
        {
            if (left is null || right is null ||
                left.RowLength != right.RowLength||
                left.ColumnLength != right.ColumnLength) { throw new InvalidOperationException(); }

            bool result = false;
            ReadOnlySpan<T> leftSpan = left.Values.AsSpan();
            ReadOnlySpan<T> rightSpan = right.Values.AsSpan();

            for (int i = 0; !result && i < left.Values.Length; i++) { result = leftSpan[i] == rightSpan[i]; }

            return result;
        }

        public static bool operator !=(Matrix<T>? left, Matrix<T>? right)
        {
            if (left is null || right is null ||
                left.RowLength != right.RowLength ||
                left.ColumnLength != right.ColumnLength) { throw new InvalidOperationException(); }

            bool result = false;
            ReadOnlySpan<T> leftSpan = left.Values.AsSpan();
            ReadOnlySpan<T> rightSpan = right.Values.AsSpan();

            for (int i = 0; !result && i < left.Values.Length; i++) { result = leftSpan[i] != rightSpan[i]; }

            return result;
        }

        public static bool operator >(Matrix<T> left, Matrix<T> right)
        {
            throw new NotImplementedException();
        }

        public static bool operator >=(Matrix<T> left, Matrix<T> right)
        {
            throw new NotImplementedException();
        }

        public static bool operator <(Matrix<T> left, Matrix<T> right)
        {
            throw new NotImplementedException();
        }

        public static bool operator <=(Matrix<T> left, Matrix<T> right)
        {
            throw new NotImplementedException();
        }
        protected bool Equals(Matrix<T> other)
        {
            return Values.Equals(other.Values);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Matrix<T>)obj);
        }

        public override int GetHashCode()
        {
            return Values.GetHashCode();
        }
    }
}
