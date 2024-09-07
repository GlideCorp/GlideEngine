
using System;
using Core.Logs;
using Core.Maths.Vectors;
using System.Numerics;
using System.Runtime.Intrinsics;
using Vector3 = Core.Maths.Vectors.Vector3;
using Vector4 = Core.Maths.Vectors.Vector4;

namespace Core.Maths.Matrices
{
    public class Matrix4x4() : RootMatrix4x4<float>()
    {
        public new static Matrix4x4 Identity => new(
            new(1, 0, 0, 0),
            new(0, 1, 0, 0),
            new(0, 0, 1, 0),
            new(0, 0, 0, 1)
        );

        public Matrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3) : this()
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

        public static Matrix4x4 Translate(Vector3 vector)
        {
            Matrix4x4 result = Identity;
            result.Values[0, 3] = vector.X;
            result.Values[1, 3] = vector.Y;
            result.Values[2, 3] = vector.Z;
            result.Values[3, 3] = 1;
            return result;
        }

        public static Matrix4x4 Rotate(Quaternion quaternion)
        {
            Matrix4x4 result = Identity;

            // Precalculate coordinate products
            float x = quaternion.X * 2.0F;
            float y = quaternion.Y * 2.0F;
            float z = quaternion.Z * 2.0F;
            float xx = quaternion.X * x;
            float yy = quaternion.Y * y;
            float zz = quaternion.Z * z;
            float xy = quaternion.X * y;
            float xz = quaternion.X * z;
            float yz = quaternion.Y * z;
            float wx = quaternion.W * x;
            float wy = quaternion.W * y;
            float wz = quaternion.W * z;

            result.Values[0, 0] = 1.0f - (yy + zz);
            result.Values[1, 0] = (xy + wz);
            result.Values[2, 0] = (xz + wy);
            result.Values[3, 0] = 0.0f;

            result.Values[0, 1] = (xy - wz);
            result.Values[1, 1] = 1.0f - (xx + zz);
            result.Values[2, 1] = (yz + wx);
            result.Values[3, 1] = 0.0f;

            result.Values[0, 2] = (xz + wy);
            result.Values[1, 2] = (yz - wx);
            result.Values[2, 2] = 1.0f - (xx + yy);
            result.Values[3, 2] = 0.0F;

            result.Values[0, 3] = 0.0f;
            result.Values[1, 3] = 0.0f;
            result.Values[2, 3] = 0.0f;
            result.Values[3, 3] = 1.0f;

            return result;
        }

        public static Matrix4x4 Scale(Vector3 vector)
        {
            Matrix4x4 result = new()
            {
                Values =
                {
                    [0, 0] = vector.X,
                    [1, 1] = vector.Y,
                    [2, 2] = vector.Z,
                    [3, 3] = 1
                }
            };
            return result;
        }
    }

    public class RootMatrix4x4<T>() : Matrix<T>(4, 4)
        where T : INumber<T>
    {
        public static RootMatrix4x4<T> Identity => new(
            new(T.One, T.Zero, T.Zero, T.Zero),
            new(T.Zero, T.One, T.Zero, T.Zero),
            new(T.Zero, T.Zero, T.One, T.Zero),
            new(T.Zero, T.Zero, T.Zero, T.One)
        );

        public RootMatrix4x4(Vector4<T> column0, Vector4<T> column1, Vector4<T> column2, Vector4<T> column3) : this()
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

        public static RootMatrix4x4<T> operator +(RootMatrix4x4<T> left, RootMatrix4x4<T> right)
        {
            RootMatrix4x4<T> result = new()
            {
                Values =
                {
                    [0, 0] = left.Values[0, 0] + right.Values[0, 0],
                    [0, 1] = left.Values[0, 1] + right.Values[0, 1],
                    [0, 2] = left.Values[0, 2] + right.Values[0, 2],
                    [0, 3] = left.Values[0, 3] + right.Values[0, 3],
                    [1, 0] = left.Values[1, 0] + right.Values[1, 0],
                    [1, 1] = left.Values[1, 1] + right.Values[1, 1],
                    [1, 2] = left.Values[1, 2] + right.Values[1, 2],
                    [1, 3] = left.Values[1, 3] + right.Values[1, 3],
                    [2, 0] = left.Values[2, 0] + right.Values[2, 0],
                    [2, 1] = left.Values[2, 1] + right.Values[2, 1],
                    [2, 2] = left.Values[2, 2] + right.Values[2, 2],
                    [2, 3] = left.Values[2, 3] + right.Values[2, 3],
                    [3, 0] = left.Values[3, 0] + right.Values[3, 0],
                    [3, 1] = left.Values[3, 1] + right.Values[3, 1],
                    [3, 2] = left.Values[3, 2] + right.Values[3, 2],
                    [3, 3] = left.Values[3, 3] + right.Values[3, 3]
                }
            };

            return result;
        }

        public static RootMatrix4x4<T> operator -(RootMatrix4x4<T> left, RootMatrix4x4<T> right)
        {
            RootMatrix4x4<T> result = new()
            {
                Values =
                {
                    [0, 0] = left.Values[0, 0] - right.Values[0, 0],
                    [0, 1] = left.Values[0, 1] - right.Values[0, 1],
                    [0, 2] = left.Values[0, 2] - right.Values[0, 2],
                    [0, 3] = left.Values[0, 3] - right.Values[0, 3],
                    [1, 0] = left.Values[1, 0] - right.Values[1, 0],
                    [1, 1] = left.Values[1, 1] - right.Values[1, 1],
                    [1, 2] = left.Values[1, 2] - right.Values[1, 2],
                    [1, 3] = left.Values[1, 3] - right.Values[1, 3],
                    [2, 0] = left.Values[2, 0] - right.Values[2, 0],
                    [2, 1] = left.Values[2, 1] - right.Values[2, 1],
                    [2, 2] = left.Values[2, 2] - right.Values[2, 2],
                    [2, 3] = left.Values[2, 3] - right.Values[2, 3],
                    [3, 0] = left.Values[3, 0] - right.Values[3, 0],
                    [3, 1] = left.Values[3, 1] - right.Values[3, 1],
                    [3, 2] = left.Values[3, 2] - right.Values[3, 2],
                    [3, 3] = left.Values[3, 3] - right.Values[3, 3]
                }
            };

            return result;
        }

        public static RootMatrix4x4<T> operator -(RootMatrix4x4<T> matrix)
        {
            RootMatrix4x4<T> result = new()
            {
                Values =
                {
                    [0, 0] = -matrix.Values[0, 0],
                    [0, 1] = -matrix.Values[0, 1],
                    [0, 2] = -matrix.Values[0, 2],
                    [0, 3] = -matrix.Values[0, 3],
                    [1, 0] = -matrix.Values[1, 0],
                    [1, 1] = -matrix.Values[1, 1],
                    [1, 2] = -matrix.Values[1, 2],
                    [1, 3] = -matrix.Values[1, 3],
                    [2, 0] = -matrix.Values[2, 0],
                    [2, 1] = -matrix.Values[2, 1],
                    [2, 2] = -matrix.Values[2, 2],
                    [2, 3] = -matrix.Values[2, 3],
                    [3, 0] = -matrix.Values[3, 0],
                    [3, 1] = -matrix.Values[3, 1],
                    [3, 2] = -matrix.Values[3, 2],
                    [3, 3] = -matrix.Values[3, 3]
                }
            };

            return result;
        }

        public static RootMatrix4x4<T> operator *(RootMatrix4x4<T> left, T right)
        {
            RootMatrix4x4<T> result = new()
            {
                Values =
                {
                    [0, 0] = left.Values[0, 0] * right,
                    [0, 1] = left.Values[0, 1] * right,
                    [0, 2] = left.Values[0, 2] * right,
                    [0, 3] = left.Values[0, 3] * right,
                    [1, 0] = left.Values[1, 0] * right,
                    [1, 1] = left.Values[1, 1] * right,
                    [1, 2] = left.Values[1, 2] * right,
                    [1, 3] = left.Values[1, 3] * right,
                    [2, 0] = left.Values[2, 0] * right,
                    [2, 1] = left.Values[2, 1] * right,
                    [2, 2] = left.Values[2, 2] * right,
                    [2, 3] = left.Values[2, 3] * right,
                    [3, 0] = left.Values[3, 0] * right,
                    [3, 1] = left.Values[3, 1] * right,
                    [3, 2] = left.Values[3, 2] * right,
                    [3, 3] = left.Values[3, 3] * right
                }
            };

            return result;
        }
        public static RootMatrix4x4<T> operator *(T left, RootMatrix4x4<T> right) => right * left;

        public static Vector4<T> operator *(RootMatrix4x4<T> left, Vector4<T> right)
        {
            Vector4<T> result = new()
            {
                Values =
                {
                    [0] = right.Values[0] * left.Values[0, 0] + right.Values[1] * left.Values[0, 1] + right.Values[2] * left.Values[0, 2] + right.Values[3] * left.Values[0, 3],
                    [1] = right.Values[1] * left.Values[1, 0] + right.Values[1] * left.Values[1, 1] + right.Values[2] * left.Values[1, 2] + right.Values[3] * left.Values[1, 3],
                    [2] = right.Values[2] * left.Values[2, 0] + right.Values[1] * left.Values[2, 1] + right.Values[2] * left.Values[2, 2] + right.Values[3] * left.Values[2, 3],
                    [3] = right.Values[3] * left.Values[3, 0] + right.Values[1] * left.Values[3, 1] + right.Values[2] * left.Values[3, 2] + right.Values[3] * left.Values[3, 3]
                }
            };

            return result;
        }
        public static RootMatrix4x4<T> operator *(RootMatrix4x4<T> left, RootMatrix4x4<T> right)
        {
            RootMatrix4x4<T> result = new()
            {
                Values =
                {
                    [0, 0] = left.Values[0, 0] * right.Values[0, 0] + left.Values[0, 1] * right.Values[1, 0] + left.Values[0, 2] * right.Values[2, 0] + left.Values[0, 3] * right.Values[3, 0],
                    [0, 1] = left.Values[0, 0] * right.Values[0, 1] + left.Values[0, 1] * right.Values[1, 1] + left.Values[0, 2] * right.Values[2, 1] + left.Values[0, 3] * right.Values[3, 1],
                    [0, 2] = left.Values[0, 0] * right.Values[0, 2] + left.Values[0, 1] * right.Values[1, 2] + left.Values[0, 2] * right.Values[2, 2] + left.Values[0, 3] * right.Values[3, 2],
                    [0, 3] = left.Values[0, 0] * right.Values[0, 3] + left.Values[0, 1] * right.Values[1, 3] + left.Values[0, 2] * right.Values[2, 3] + left.Values[0, 3] * right.Values[3, 3],
                    [1, 0] = left.Values[1, 0] * right.Values[0, 0] + left.Values[1, 1] * right.Values[1, 0] + left.Values[1, 2] * right.Values[2, 0] + left.Values[1, 3] * right.Values[3, 0],
                    [1, 1] = left.Values[1, 0] * right.Values[0, 1] + left.Values[1, 1] * right.Values[1, 1] + left.Values[1, 2] * right.Values[2, 1] + left.Values[1, 3] * right.Values[3, 1],
                    [1, 2] = left.Values[1, 0] * right.Values[0, 2] + left.Values[1, 1] * right.Values[1, 2] + left.Values[1, 2] * right.Values[2, 2] + left.Values[1, 3] * right.Values[3, 2],
                    [1, 3] = left.Values[1, 0] * right.Values[0, 3] + left.Values[1, 1] * right.Values[1, 3] + left.Values[1, 2] * right.Values[2, 3] + left.Values[1, 3] * right.Values[3, 3],
                    [2, 0] = left.Values[2, 0] * right.Values[0, 0] + left.Values[2, 1] * right.Values[1, 0] + left.Values[2, 2] * right.Values[2, 0] + left.Values[2, 3] * right.Values[3, 0],
                    [2, 1] = left.Values[2, 0] * right.Values[0, 1] + left.Values[2, 1] * right.Values[1, 1] + left.Values[2, 2] * right.Values[2, 1] + left.Values[2, 3] * right.Values[3, 1],
                    [2, 2] = left.Values[2, 0] * right.Values[0, 2] + left.Values[2, 1] * right.Values[1, 2] + left.Values[2, 2] * right.Values[2, 2] + left.Values[2, 3] * right.Values[3, 2],
                    [2, 3] = left.Values[2, 0] * right.Values[0, 3] + left.Values[2, 1] * right.Values[1, 3] + left.Values[2, 2] * right.Values[2, 3] + left.Values[2, 3] * right.Values[3, 3],
                    [3, 0] = left.Values[3, 0] * right.Values[0, 0] + left.Values[3, 1] * right.Values[1, 0] + left.Values[3, 2] * right.Values[2, 0] + left.Values[3, 3] * right.Values[3, 0],
                    [3, 1] = left.Values[3, 0] * right.Values[0, 1] + left.Values[3, 1] * right.Values[1, 1] + left.Values[3, 2] * right.Values[2, 1] + left.Values[3, 3] * right.Values[3, 1],
                    [3, 2] = left.Values[3, 0] * right.Values[0, 2] + left.Values[3, 1] * right.Values[1, 2] + left.Values[3, 2] * right.Values[2, 2] + left.Values[3, 3] * right.Values[3, 2],
                    [3, 3] = left.Values[3, 0] * right.Values[0, 3] + left.Values[3, 1] * right.Values[1, 3] + left.Values[3, 2] * right.Values[2, 3] + left.Values[3, 3] * right.Values[3, 3]
                }
            };

            return result;
        }

        public static bool operator ==(RootMatrix4x4<T>? left, RootMatrix4x4<T>? right)
        {
            if (left is null || right is null) { return false; }

            return left.GetColumn(0) == right.GetColumn(0) &&
                   left.GetColumn(1) == right.GetColumn(1) &&
                   left.GetColumn(2) == right.GetColumn(2) &&
                   left.GetColumn(3) == right.GetColumn(3);
        }

        public static bool operator !=(RootMatrix4x4<T>? left, RootMatrix4x4<T>? right)
        {
            if (left is null || right is null) { return false; }

            return left.GetColumn(0) != right.GetColumn(0) ||
                   left.GetColumn(1) != right.GetColumn(1) ||
                   left.GetColumn(2) != right.GetColumn(2) ||
                   left.GetColumn(3) != right.GetColumn(3);
        }

        public static bool operator >(RootMatrix4x4<T> left, RootMatrix4x4<T> right) { throw new NotImplementedException(); }
        public static bool operator >=(RootMatrix4x4<T> left, RootMatrix4x4<T> right) { throw new NotImplementedException(); }
        public static bool operator <(RootMatrix4x4<T> left, RootMatrix4x4<T> right) { throw new NotImplementedException(); }
        public static bool operator <=(RootMatrix4x4<T> left, RootMatrix4x4<T> right) { throw new NotImplementedException(); }

        public RootMatrix4x4<T> Transpose()
        {
            RootMatrix4x4<T> result = new()
            {
                Values =
                {
                    [0, 0] = Values[0, 0],
                    [0, 1] = Values[1, 0],
                    [0, 2] = Values[2, 0],
                    [0, 3] = Values[3, 0],
                    [1, 0] = Values[0, 1],
                    [1, 1] = Values[1, 1],
                    [1, 2] = Values[2, 1],
                    [1, 3] = Values[3, 1],
                    [2, 0] = Values[0, 2],
                    [2, 1] = Values[1, 2],
                    [2, 2] = Values[2, 2],
                    [2, 3] = Values[3, 2],
                    [3, 0] = Values[0, 3],
                    [3, 1] = Values[1, 3],
                    [3, 2] = Values[2, 3],
                    [3, 3] = Values[3, 3]
                }
            };

            return result;
        }

        public Vector4<T> GetColumn(int index)
        {
            return new Vector4<T>
            {
                X = Values[0, index],
                Y = Values[1, index],
                Z = Values[2, index],
                W = Values[3, index]
            };
        }
        public Vector4<T> GetRow(int index)
        {
            return new Vector4<T>
            {
                X = Values[index, 0],
                Y = Values[index, 1],
                Z = Values[index, 2],
                W = Values[index, 3]
            };
        }

        protected bool Equals(RootMatrix4x4<T> other)
        {
            return Values.Equals(other.Values);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RootMatrix4x4<T>)obj);
        }

        public override int GetHashCode()
        {
            return Values.GetHashCode();
        }
    }
}
