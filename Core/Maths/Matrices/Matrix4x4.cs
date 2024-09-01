using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core.Maths.Matrices
{
    public class Matrix4x4<T> : Matrix<T> where T : INumber<T>
    {
        public Matrix4x4() : base(4, 4)
        {
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
    }
}
