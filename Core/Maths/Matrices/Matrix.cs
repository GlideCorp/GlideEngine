using Core.Maths.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core.Maths.Matrices
{
    public class Matrix<T>:
        IAdditionOperators<Matrix<T>, Matrix<T>, Matrix<T>>,
        ISubtractionOperators<Matrix<T>, Matrix<T>, Matrix<T>>,
        IMultiplyOperators<Matrix<T>, T, Matrix<T>>,
        IMultiplyOperators<Matrix<T>, Matrix<T>, Matrix<T>>,
        IMultiplyOperators<Matrix<T>, Vectors.Vector<T>, Vectors.Vector<T>>,
        IComparisonOperators<Matrix<T>, Matrix<T>, bool>,
        IUnaryNegationOperators<Matrix<T>, Matrix<T>>
        where T : INumber<T>
    {
        public T[,] Values { get; set; }
        public int RowCount { get; init; }
        public int ColumnCount { get; init; }

        public Matrix(int rowCount, int columnCount)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;

            Values = new T[RowCount, ColumnCount];
        }

        public Vectors.Vector<T> GetColumn(int index)
        {
            Vectors.Vector<T> result = new Vectors.Vector<T>(RowCount);

            for (int j = 0; j < RowCount; j++)
            {
                result.Values[j] = Values[j, index];
            }

            return result;
        }

        public Vectors.Vector<T> GetRow(int index)
        {
            Vectors.Vector<T> result = new Vectors.Vector<T>(ColumnCount);

            for (int j = 0; j < ColumnCount; j++)
            {
                result.Values[j] = Values[index, j];
            }

            return result;
        }

        public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right)
        {
            if (left.RowCount != right.RowCount) { throw new InvalidOperationException(); }
            if (left.ColumnCount != right.ColumnCount) { throw new InvalidOperationException(); }

            Matrix<T> result = new(left.RowCount, left.ColumnCount);

            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < left.ColumnCount; j++)
                {
                    result.Values[i, j] = left.Values[i, j] + right.Values[i, j];
                }
            }
            return result;
        }

        public static Matrix<T> operator -(Matrix<T> left, Matrix<T> right)
        {
            if (left.RowCount != right.RowCount) { throw new InvalidOperationException(); }
            if (left.ColumnCount != right.ColumnCount) { throw new InvalidOperationException(); }

            Matrix<T> result = new(left.RowCount, left.ColumnCount);
            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < left.ColumnCount; j++)
                {
                    result.Values[i, j] = left.Values[i, j] - right.Values[i, j];
                }
            }

            return result;
        }

        public static Matrix<T> operator *(Matrix<T> left, T right)
        {
            Matrix<T> result = new(left.RowCount, left.ColumnCount);
            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < left.ColumnCount; j++)
                {
                    result.Values[i, j] = right * left.Values[i, j];
                }
            }

            return result;
        }
        public static Matrix<T> operator *(T left, Matrix<T> rigth) => rigth * left;

        public static Matrix<T> operator -(Matrix<T> matrix)
        {
            Matrix<T> result = new(matrix.RowCount, matrix.ColumnCount);
            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    result.Values[i, j] = -matrix.Values[i, j];
                }
            }
            return result;
        }

        public static bool operator ==(Matrix<T>? left, Matrix<T>? right)
        {
            if (left is null || right is null ||
                left.RowCount != right.RowCount||
                left.ColumnCount != right.ColumnCount) { throw new InvalidOperationException(); }

            bool result = false;
            for (int i = 0; !result && i < left.RowCount; i++)
            {
                for (int j = 0; !result && j < right.ColumnCount; j++)
                {
                    result = left.Values[i, j] == right.Values[i, j];
                }
            }

            return result;
        }

        public static bool operator !=(Matrix<T>? left, Matrix<T>? right)
        {
            if (left is null || right is null ||
                left.RowCount != right.RowCount ||
                left.ColumnCount != right.ColumnCount) { throw new InvalidOperationException(); }

            bool result = false;
            for (int i = 0; result && i < left.RowCount; i++)
            {
                for (int j = 0; result && j < right.ColumnCount; j++)
                {
                    result = left.Values[i, j] != right.Values[i, j];
                }
            }

            return result;
        }

        public Matrix<T> Transpose()
        {
            Matrix<T> result = new Matrix<T>(RowCount, ColumnCount);

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    result.Values[i, j] = Values[j, i] ;
                }
            }

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

        public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right)
        {
            if (left.ColumnCount != right.RowCount) { throw new InvalidOperationException(); }

            Matrix<T> result = new Matrix<T>(left.RowCount, left.ColumnCount);
            for (int i = 0; i < left.RowCount; i++)
            {
                for (int k = 0; k < left.RowCount; k++)
                {
                    for (int j = 0; j < left.RowCount; j++)
                    {
                        result.Values[i,j] += left.Values[i,k] * right.Values[k,j];
                    }
                }
            }

            return result;
        }

        public static Vectors.Vector<T> operator *(Matrix<T> left, Vectors.Vector<T> right)
        {
            if (right.Values.Length != left.RowCount) { throw new InvalidOperationException(); }

            Vectors.Vector<T> result = new Vectors.Vector<T>(left.RowCount);

            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < left.ColumnCount; j++)
                {
                    result.Values[i] += left.Values[i, j] * right.Values[i];
                }
            }


            return result;
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
