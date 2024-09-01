using Core.Logs;
using Core.Maths.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core.Maths.Matrices
{
    public class Matrix4x4<T> :
        IAdditionOperators<Matrix4x4<T>, Matrix4x4<T>, Matrix4x4<T>>,
        ISubtractionOperators<Matrix4x4<T>, Matrix4x4<T>, Matrix4x4<T>>,
        IMultiplyOperators<Matrix4x4<T>, T, Matrix4x4<T>>,
        IMultiplyOperators<Matrix4x4<T>, Matrix4x4<T>, Matrix4x4<T>>,
        IMultiplyOperators<Matrix4x4<T>, Vector4<T>, Vector4<T>>,
        IComparisonOperators<Matrix4x4<T>, Matrix4x4<T>, bool>,
        IUnaryNegationOperators<Matrix4x4<T>, Matrix4x4<T>>
        where T : INumber<T>
    {

        public static Matrix4x4<T> Identity => new(
            new(T.One, T.Zero, T.Zero, T.Zero),
            new(T.Zero, T.One, T.Zero, T.Zero),
            new(T.Zero, T.Zero, T.One, T.Zero),
            new(T.Zero, T.Zero, T.Zero, T.One)
        );

        public T[,] Values { get; set; } = new T[4, 4];

        public Matrix4x4()
        {
        }

        public Matrix4x4(Vector4<T> column0, Vector4<T> column1, Vector4<T> column2, Vector4<T> column3)
        {
            Values[0, 0] = column0.X;
            Values[0, 1] = column1.X;
            Values[0, 2] = column2.X;
            Values[0, 3] = column3.X;

            Values[1, 0] = column0.Y;
            Values[1, 1] = column1.Y;
            Values[1, 2] = column2.Y;
            Values[1, 3] = column3.Y;

            Values[2, 0] = column0.Z;
            Values[2, 1] = column1.Z;
            Values[2, 2] = column2.Z;
            Values[2, 3] = column3.Z;

            Values[3, 0] = column0.W;
            Values[3, 1] = column1.W;
            Values[3, 2] = column2.W;
            Values[3, 3] = column3.W;
        }

        public static Matrix4x4<T> operator +(Matrix4x4<T> left, Matrix4x4<T> right)
        {
            Matrix4x4<T> result = new();

            result.Values[0, 0] = left.Values[0, 0] + right.Values[0, 0];
            result.Values[0, 1] = left.Values[0, 1] + right.Values[0, 1];
            result.Values[0, 2] = left.Values[0, 2] + right.Values[0, 2];
            result.Values[0, 3] = left.Values[0, 3] + right.Values[0, 3];

            result.Values[1, 0] = left.Values[1, 0] + right.Values[1, 0];
            result.Values[1, 1] = left.Values[1, 1] + right.Values[1, 1];
            result.Values[1, 2] = left.Values[1, 2] + right.Values[1, 2];
            result.Values[1, 3] = left.Values[1, 3] + right.Values[1, 3];

            result.Values[2, 0] = left.Values[2, 0] + right.Values[2, 0];
            result.Values[2, 1] = left.Values[2, 1] + right.Values[2, 1];
            result.Values[2, 2] = left.Values[2, 2] + right.Values[2, 2];
            result.Values[2, 3] = left.Values[2, 3] + right.Values[2, 3];

            result.Values[3, 0] = left.Values[3, 0] + right.Values[3, 0];
            result.Values[3, 1] = left.Values[3, 1] + right.Values[3, 1];
            result.Values[3, 2] = left.Values[3, 2] + right.Values[3, 2];
            result.Values[3, 3] = left.Values[3, 3] + right.Values[3, 3];

            return result;
        }

        public static Matrix4x4<T> operator -(Matrix4x4<T> left, Matrix4x4<T> right)
        {
            Matrix4x4<T> result = new();

            result.Values[0, 0] = left.Values[0, 0] - right.Values[0, 0];
            result.Values[0, 1] = left.Values[0, 1] - right.Values[0, 1];
            result.Values[0, 2] = left.Values[0, 2] - right.Values[0, 2];
            result.Values[0, 3] = left.Values[0, 3] - right.Values[0, 3];

            result.Values[1, 0] = left.Values[1, 0] - right.Values[1, 0];
            result.Values[1, 1] = left.Values[1, 1] - right.Values[1, 1];
            result.Values[1, 2] = left.Values[1, 2] - right.Values[1, 2];
            result.Values[1, 3] = left.Values[1, 3] - right.Values[1, 3];

            result.Values[2, 0] = left.Values[2, 0] - right.Values[2, 0];
            result.Values[2, 1] = left.Values[2, 1] - right.Values[2, 1];
            result.Values[2, 2] = left.Values[2, 2] - right.Values[2, 2];
            result.Values[2, 3] = left.Values[2, 3] - right.Values[2, 3];

            result.Values[3, 0] = left.Values[3, 0] - right.Values[3, 0];
            result.Values[3, 1] = left.Values[3, 1] - right.Values[3, 1];
            result.Values[3, 2] = left.Values[3, 2] - right.Values[3, 2];
            result.Values[3, 3] = left.Values[3, 3] - right.Values[3, 3];

            return result;
        }

        public static Matrix4x4<T> operator -(Matrix4x4<T> matrix)
        {
            Matrix4x4<T> result = new();

            result.Values[0, 0] = -matrix.Values[0, 0];
            result.Values[0, 1] = -matrix.Values[0, 1];
            result.Values[0, 2] = -matrix.Values[0, 2];
            result.Values[0, 3] = -matrix.Values[0, 3];

            result.Values[1, 0] = -matrix.Values[1, 0];
            result.Values[1, 1] = -matrix.Values[1, 1];
            result.Values[1, 2] = -matrix.Values[1, 2];
            result.Values[1, 3] = -matrix.Values[1, 3];

            result.Values[2, 0] = -matrix.Values[2, 0];
            result.Values[2, 1] = -matrix.Values[2, 1];
            result.Values[2, 2] = -matrix.Values[2, 2];
            result.Values[2, 3] = -matrix.Values[2, 3];

            result.Values[3, 0] = -matrix.Values[3, 0];
            result.Values[3, 1] = -matrix.Values[3, 1];
            result.Values[3, 2] = -matrix.Values[3, 2];
            result.Values[3, 3] = -matrix.Values[3, 3];

            return result;
        }

        public static Matrix4x4<T> operator *(Matrix4x4<T> left, T right)
        {
            Matrix4x4<T> result = new();

            result.Values[0, 0] *= right;
            result.Values[0, 1] *= right;
            result.Values[0, 2] *= right;
            result.Values[0, 3] *= right;

            result.Values[1, 0] *= right;
            result.Values[1, 1] *= right;
            result.Values[1, 2] *= right;
            result.Values[1, 3] *= right;

            result.Values[2, 0] *= right;
            result.Values[2, 1] *= right;
            result.Values[2, 2] *= right;
            result.Values[2, 3] *= right;

            result.Values[3, 0] *= right;
            result.Values[3, 1] *= right;
            result.Values[3, 2] *= right;
            result.Values[3, 3] *= right;

            return result;
        }
        public static Matrix4x4<T> operator *(T left, Matrix4x4<T> right) => right * left;

        public static Vector4<T> operator *(Matrix4x4<T> left, Vector4<T> right)
        {
            Vector4<T> result = new();

            result.Values[0] = right.Values[0] * left.Values[0, 0] + right.Values[1] * left.Values[0, 1] + right.Values[2] * left.Values[0, 2] + right.Values[3] * left.Values[0, 3];
            result.Values[1] = right.Values[1] * left.Values[1, 0] + right.Values[1] * left.Values[1, 1] + right.Values[2] * left.Values[1, 2] + right.Values[3] * left.Values[1, 3];
            result.Values[2] = right.Values[2] * left.Values[2, 0] + right.Values[1] * left.Values[2, 1] + right.Values[2] * left.Values[2, 2] + right.Values[3] * left.Values[2, 3];
            result.Values[3] = right.Values[3] * left.Values[3, 0] + right.Values[1] * left.Values[3, 1] + right.Values[2] * left.Values[3, 2] + right.Values[3] * left.Values[3, 3];

            return result;
        }
        public static Matrix4x4<T> operator *(Matrix4x4<T> left, Matrix4x4<T> right)
        {
            Matrix4x4<T> result = new();

            result.Values[0, 0] = left.Values[0, 0] * right.Values[0, 0] + left.Values[0, 1] * right.Values[1, 0] + left.Values[0, 2] * right.Values[2, 0] + left.Values[0, 3] * right.Values[3, 0];
            result.Values[0, 1] = left.Values[0, 0] * right.Values[0, 1] + left.Values[0, 1] * right.Values[1, 1] + left.Values[0, 2] * right.Values[2, 1] + left.Values[0, 3] * right.Values[3, 1];
            result.Values[0, 2] = left.Values[0, 0] * right.Values[0, 2] + left.Values[0, 1] * right.Values[1, 2] + left.Values[0, 2] * right.Values[2, 2] + left.Values[0, 3] * right.Values[3, 2];
            result.Values[0, 3] = left.Values[0, 0] * right.Values[0, 3] + left.Values[0, 1] * right.Values[1, 3] + left.Values[0, 2] * right.Values[2, 3] + left.Values[0, 3] * right.Values[3, 3];

            result.Values[1, 0] = left.Values[1, 0] * right.Values[0, 0] + left.Values[1, 1] * right.Values[1, 0] + left.Values[1, 2] * right.Values[2, 0] + left.Values[1, 3] * right.Values[3, 0];
            result.Values[1, 1] = left.Values[1, 0] * right.Values[0, 1] + left.Values[1, 1] * right.Values[1, 1] + left.Values[1, 2] * right.Values[2, 1] + left.Values[1, 3] * right.Values[3, 1];
            result.Values[1, 2] = left.Values[1, 0] * right.Values[0, 2] + left.Values[1, 1] * right.Values[1, 2] + left.Values[1, 2] * right.Values[2, 2] + left.Values[1, 3] * right.Values[3, 2];
            result.Values[1, 3] = left.Values[1, 0] * right.Values[0, 3] + left.Values[1, 1] * right.Values[1, 3] + left.Values[1, 2] * right.Values[2, 3] + left.Values[1, 3] * right.Values[3, 3];

            result.Values[2, 0] = left.Values[2, 0] * right.Values[0, 0] + left.Values[2, 1] * right.Values[1, 0] + left.Values[2, 2] * right.Values[2, 0] + left.Values[2, 3] * right.Values[3, 0];
            result.Values[2, 1] = left.Values[2, 0] * right.Values[0, 1] + left.Values[2, 1] * right.Values[1, 1] + left.Values[2, 2] * right.Values[2, 1] + left.Values[2, 3] * right.Values[3, 1];
            result.Values[2, 2] = left.Values[2, 0] * right.Values[0, 2] + left.Values[2, 1] * right.Values[1, 2] + left.Values[2, 2] * right.Values[2, 2] + left.Values[2, 3] * right.Values[3, 2];
            result.Values[2, 3] = left.Values[2, 0] * right.Values[0, 3] + left.Values[2, 1] * right.Values[1, 3] + left.Values[2, 2] * right.Values[2, 3] + left.Values[2, 3] * right.Values[3, 3];

            result.Values[3, 0] = left.Values[3, 0] * right.Values[0, 0] + left.Values[3, 1] * right.Values[1, 0] + left.Values[3, 2] * right.Values[2, 0] + left.Values[3, 3] * right.Values[3, 0];
            result.Values[3, 1] = left.Values[3, 0] * right.Values[0, 1] + left.Values[3, 1] * right.Values[1, 1] + left.Values[3, 2] * right.Values[2, 1] + left.Values[3, 3] * right.Values[3, 1];
            result.Values[3, 2] = left.Values[3, 0] * right.Values[0, 2] + left.Values[3, 1] * right.Values[1, 2] + left.Values[3, 2] * right.Values[2, 2] + left.Values[3, 3] * right.Values[3, 2];
            result.Values[3, 3] = left.Values[3, 0] * right.Values[0, 3] + left.Values[3, 1] * right.Values[1, 3] + left.Values[3, 2] * right.Values[2, 3] + left.Values[3, 3] * right.Values[3, 3];

            return result;
        }

        public static bool operator ==(Matrix4x4<T> left, Matrix4x4<T> right)
        {
            return left.GetColumn(0) == right.GetColumn(0) &&
                   left.GetColumn(1) == right.GetColumn(1) &&
                   left.GetColumn(2) == right.GetColumn(2) &&
                   left.GetColumn(3) == right.GetColumn(3);
        }

        public static bool operator !=(Matrix4x4<T> left, Matrix4x4<T> right)
        {
            return left.GetColumn(0) != right.GetColumn(0) ||
                   left.GetColumn(1) != right.GetColumn(1) ||
                   left.GetColumn(2) != right.GetColumn(2) ||
                   left.GetColumn(3) != right.GetColumn(3);
        }

        public static bool operator >(Matrix4x4<T> left, Matrix4x4<T> right)
        {
            throw new NotImplementedException();
        }

        public static bool operator >=(Matrix4x4<T> left, Matrix4x4<T> right)
        {
            throw new NotImplementedException();
        }

        public static bool operator <(Matrix4x4<T> left, Matrix4x4<T> right)
        {
            throw new NotImplementedException();
        }

        public static bool operator <=(Matrix4x4<T> left, Matrix4x4<T> right)
        {
            throw new NotImplementedException();
        }

        public static Matrix4x4<T> Translate(Vector3<T> vector)
        {
            Matrix4x4<T> result = Matrix4x4<T>.Identity;
            result.Values[0, 3] = vector.X;
            result.Values[1, 3] = vector.Y;
            result.Values[2, 3] = vector.Z;
            result.Values[3, 3] = T.One;
            return result;
        }

        public static Matrix4x4<T> Rotate(Vector3<T> vector)
        {
            Matrix4x4<T> result = Matrix4x4<T>.Identity;
            result.Values[0, 3] = vector.X;
            result.Values[1, 3] = vector.Y;
            result.Values[2, 3] = vector.Z;
            result.Values[3, 3] = T.One;
            return result;
        }

        public static Matrix4x4<T> Scale()
        {
            Matrix4x4<T> result = Matrix4x4<T>.Identity;
            Logger.Info("Imbecille implementalo quando hai quaternion :)");
            return result;
        }

        public Matrix4x4<T> Transpose()
        {
            Matrix4x4<T> result = new(); 
            
            result.Values[0, 0] = Values[0, 0];
            result.Values[0, 1] = Values[1, 0];
            result.Values[0, 2] = Values[2, 0];
            result.Values[0, 3] = Values[3, 0];

            result.Values[1, 0] = Values[0, 1];
            result.Values[1, 1] = Values[1, 1];
            result.Values[1, 2] = Values[2, 1];
            result.Values[1, 3] = Values[3, 1];

            result.Values[2, 0] = Values[0, 2];
            result.Values[2, 1] = Values[1, 2];
            result.Values[2, 2] = Values[2, 2];
            result.Values[2, 3] = Values[3, 2];

            result.Values[3, 0] = Values[0, 3];
            result.Values[3, 1] = Values[1, 3];
            result.Values[3, 2] = Values[2, 3];
            result.Values[3, 3] = Values[3, 3];

            return result;
        }

        public Vector4<T> GetColumn(int index)
        {
            return new Vector4<T>()
            {
                X = Values[0, index],
                Y = Values[1, index],
                Z = Values[2, index],
                W = Values[3, index]
            };
        }
        public Vector4<T> GetRow(int index)
        {
            return new Vector4<T>()
            {
                X = Values[index, 0],
                Y = Values[index, 1],
                Z = Values[index, 2],
                W = Values[index, 3]
            };
        }
    }
}
