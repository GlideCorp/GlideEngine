using System;
using Core.Maths.Vectors;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Core.Maths.Matrices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4x4(Vector4Float column0, Vector4Float column1, Vector4Float column2, Vector4Float column3) :
        IAdditionOperators<Matrix4x4, Matrix4x4, Matrix4x4>,
        ISubtractionOperators<Matrix4x4, Matrix4x4, Matrix4x4>,
        IMultiplyOperators<Matrix4x4, float, Matrix4x4>,
        IMultiplyOperators<Matrix4x4, Vector4Float, Vector4Float>,
        IMultiplyOperators<Matrix4x4, Matrix4x4, Matrix4x4>,
        IUnaryNegationOperators<Matrix4x4, Matrix4x4>,
        IComparisonOperators<Matrix4x4, Matrix4x4, bool>
    {
        public static Matrix4x4 Identity => new(
            new(1, 0, 0, 0),
            new(0, 1, 0, 0),
            new(0, 0, 1, 0),
            new(0, 0, 0, 1)
        );

        public float M00 { get; set; } = column0.X;
        public float M10 { get; set; } = column0.Y;
        public float M20 { get; set; } = column0.Z;
        public float M30 { get; set; } = column0.W;

        public float M01 { get; set; } = column1.X;
        public float M11 { get; set; } = column1.Y;
        public float M21 { get; set; } = column1.Z;
        public float M31 { get; set; } = column1.W;

        public float M02 { get; set; } = column2.X;
        public float M12 { get; set; } = column2.Y;
        public float M22 { get; set; } = column2.Z;
        public float M32 { get; set; } = column2.W;

        public float M03 { get; set; } = column3.X;
        public float M13 { get; set; } = column3.Y;
        public float M23 { get; set; } = column3.Z;
        public float M33 { get; set; } = column3.W;


        public Matrix4x4() : this(Vector4Float.Zero, Vector4Float.Zero, Vector4Float.Zero, Vector4Float.Zero)
        {
            /*
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
            */
        }

        public static Matrix4x4 Translate(Vector3Float vector)
        {
            Matrix4x4 result = Identity;
            result.M03 = vector.X;
            result.M13 = vector.Y;
            result.M23 = vector.Z;
            result.M33 = 1;
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

            result.M00 = 1.0f - (yy + zz);
            result.M10 = (xy + wz);
            result.M20 = (xz - wy);
            result.M30 = 0.0f;

            result.M01 = (xy - wz);
            result.M11 = 1.0f - (xx + zz);
            result.M21 = (yz + wx);
            result.M31 = 0.0f;

            result.M02 = (xz + wy);
            result.M12 = (yz - wx);
            result.M22 = 1.0f - (xx + yy);
            result.M32 = 0.0F;

            result.M03 = 0.0f;
            result.M13 = 0.0f;
            result.M23 = 0.0f;
            result.M33 = 1.0f;

            return result;
        }

        public static Matrix4x4 Scale(Vector3Float vector)
        {
            Matrix4x4 result = new()
            {
                M00 = vector.X,
                M11 = vector.Y,
                M22 = vector.Z,
                M33 = 1
            };
            return result;
        }

        public static Matrix4x4 Perspective(float fov, float ratio, float near, float far)
        {
            if (fov <= 0.0f || fov >= Math.PI)
                throw new ArgumentOutOfRangeException();

            if (near <= 0.0f)
                throw new ArgumentOutOfRangeException();

            if (far <= 0.0f)
                throw new ArgumentOutOfRangeException();

            if (near >= far)
                throw new ArgumentOutOfRangeException();

            float yScale = 1.0f / (float)Math.Tan(fov * 0.5f);
            float xScale = yScale / ratio;

            return new()
            {
                M00 = xScale,

                M11 = yScale,

                M22 = far / (near - far),
                M32 = -1.0f,

                M23 = near * far / (near - far)
            };
        }

        public static Matrix4x4 LookAt(Vector3Float position, Vector3Float target, Vector3Float up)
        {
            Vector3Float zAxis = (position - target).Normalize();
            Vector3Float xAxis = (up.CrossProduct(zAxis)).Normalize();
            Vector3Float yAxis = zAxis.CrossProduct(xAxis).Normalize();

            return new()
            {
                M00 = xAxis.X,
                M10 = yAxis.X,
                M20 = zAxis.X,

                M01 = xAxis.Y,
                M11 = yAxis.Y,
                M21 = zAxis.Y,

                M02 = xAxis.Z,
                M12 = yAxis.Z,
                M22 = zAxis.Z,

                M03 = -xAxis.Dot(position),
                M13 = -yAxis.Dot(position),
                M23 = -zAxis.Dot(position),
                M33 = 1.0f
            };
        }

        public Matrix4x4 Transpose()
        {
            return new()
            {
                M00 = M00,
                M01 = M10,
                M02 = M20,
                M03 = M30,
                M10 = M01,
                M11 = M11,
                M12 = M21,
                M13 = M31,
                M20 = M02,
                M21 = M12,
                M22 = M22,
                M23 = M32,
                M30 = M03,
                M31 = M13,
                M32 = M23,
                M33 = M33
            };
        }

        public Vector4Float GetColumn(int index)
        {
            switch (index)
            {
                case 0:
                    return new(M00, M10, M20, M30);

                case 1:
                    return new(M01, M11, M21, M31);

                case 2:
                    return new(M02, M12, M22, M32);

                case 3:
                    return new(M03, M13, M23, M33);

                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public Vector4Float GetRow(int index)
        {
            switch (index)
            {
                case 0:
                    return new(M00, M01, M02, M03);

                case 1:
                    return new(M10, M11, M12, M13);

                case 2:
                    return new(M20, M21, M22, M23);

                case 3:
                    return new(M30, M31, M32, M33);

                default:
                    throw new IndexOutOfRangeException();
            }
        }

        #region Arithmetic Operations
        public static Matrix4x4 operator +(Matrix4x4 left, Matrix4x4 right)
        {
            return new()
            {
                M00 = left.M00 + right.M00,
                M01 = left.M01 + right.M01,
                M02 = left.M02 + right.M02,
                M03 = left.M03 + right.M03,
                M10 = left.M10 + right.M10,
                M11 = left.M11 + right.M11,
                M12 = left.M12 + right.M12,
                M13 = left.M13 + right.M13,
                M20 = left.M20 + right.M20,
                M21 = left.M21 + right.M21,
                M22 = left.M22 + right.M22,
                M23 = left.M23 + right.M23,
                M30 = left.M30 + right.M30,
                M31 = left.M31 + right.M31,
                M32 = left.M32 + right.M32,
                M33 = left.M33 + right.M33
            };
        }

        public static Matrix4x4 operator -(Matrix4x4 left, Matrix4x4 right)
        {
            return new()
            {
                M00 = left.M00 - right.M00,
                M01 = left.M01 - right.M01,
                M02 = left.M02 - right.M02,
                M03 = left.M03 - right.M03,
                M10 = left.M10 - right.M10,
                M11 = left.M11 - right.M11,
                M12 = left.M12 - right.M12,
                M13 = left.M13 - right.M13,
                M20 = left.M20 - right.M20,
                M21 = left.M21 - right.M21,
                M22 = left.M22 - right.M22,
                M23 = left.M23 - right.M23,
                M30 = left.M30 - right.M30,
                M31 = left.M31 - right.M31,
                M32 = left.M32 - right.M32,
                M33 = left.M33 - right.M33
            };
        }

        public static Matrix4x4 operator -(Matrix4x4 value)
        {
            return new()
            {
                M00 = -value.M00,
                M01 = -value.M01,
                M02 = -value.M02,
                M03 = -value.M03,
                M10 = -value.M10,
                M11 = -value.M11,
                M12 = -value.M12,
                M13 = -value.M13,
                M20 = -value.M20,
                M21 = -value.M21,
                M22 = -value.M22,
                M23 = -value.M23,
                M30 = -value.M30,
                M31 = -value.M31,
                M32 = -value.M32,
                M33 = -value.M33
            };
        }

        public static Matrix4x4 operator *(Matrix4x4 left, float right)
        {
            return new()
            {
                M00 = left.M00 * right,
                M01 = left.M01 * right,
                M02 = left.M02 * right,
                M03 = left.M03 * right,
                M10 = left.M10 * right,
                M11 = left.M11 * right,
                M12 = left.M12 * right,
                M13 = left.M13 * right,
                M20 = left.M20 * right,
                M21 = left.M21 * right,
                M22 = left.M22 * right,
                M23 = left.M23 * right,
                M30 = left.M30 * right,
                M31 = left.M31 * right,
                M32 = left.M32 * right,
                M33 = left.M33 * right
            };
        }

        public static Vector4Float operator *(Matrix4x4 left, Vector4Float right)
        {
            return new()
            {
                X = right.X * left.M00 + right.Y * left.M01 + right.Z* left.M02 + right.W * left.M03,
                Y = right.X * left.M10 + right.Y * left.M11 + right.Z* left.M12 + right.W * left.M13,
                Z = right.X * left.M20 + right.Y * left.M21 + right.Z* left.M22 + right.W * left.M23,
                W = right.X * left.M30 + right.Y * left.M31 + right.Z* left.M32 + right.W * left.M33
            };
        }

        public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
        {
            return new()
            {
                M00 = left.M00 * right.M00 + left.M01 * right.M10 + left.M02 * right.M20 + left.M03 * right.M30,
                M01 = left.M00 * right.M01 + left.M01 * right.M11 + left.M02 * right.M21 + left.M03 * right.M31,
                M02 = left.M00 * right.M02 + left.M01 * right.M12 + left.M02 * right.M22 + left.M03 * right.M32,
                M03 = left.M00 * right.M03 + left.M01 * right.M13 + left.M02 * right.M23 + left.M03 * right.M33,
                M10 = left.M10 * right.M00 + left.M11 * right.M10 + left.M12 * right.M20 + left.M13 * right.M30,
                M11 = left.M10 * right.M01 + left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31,
                M12 = left.M10 * right.M02 + left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32,
                M13 = left.M10 * right.M03 + left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33,
                M20 = left.M20 * right.M00 + left.M21 * right.M10 + left.M22 * right.M20 + left.M23 * right.M30,
                M21 = left.M20 * right.M01 + left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31,
                M22 = left.M20 * right.M02 + left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32,
                M23 = left.M20 * right.M03 + left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33,
                M30 = left.M30 * right.M00 + left.M31 * right.M10 + left.M32 * right.M20 + left.M33 * right.M30,
                M31 = left.M30 * right.M01 + left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31,
                M32 = left.M30 * right.M02 + left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32,
                M33 = left.M30 * right.M03 + left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33
            };
        }

        public static bool operator >(Matrix4x4 left, Matrix4x4 right) { throw new NotImplementedException(); }
        public static bool operator >=(Matrix4x4 left, Matrix4x4 right) { throw new NotImplementedException(); }
        public static bool operator <(Matrix4x4 left, Matrix4x4 right) { throw new NotImplementedException(); }
        public static bool operator <=(Matrix4x4 left, Matrix4x4 right) { throw new NotImplementedException(); }

        public static bool operator ==(Matrix4x4 left, Matrix4x4 right)
        {
            return left.GetColumn(0) == right.GetColumn(0) &&
                    left.GetColumn(1) == right.GetColumn(1) &&
                    left.GetColumn(2) == right.GetColumn(2) &&
                    left.GetColumn(3) == right.GetColumn(3);
        }

        public static bool operator !=(Matrix4x4 left, Matrix4x4 right)
        {
            return left.GetColumn(0) != right.GetColumn(0) &&
                    left.GetColumn(1) != right.GetColumn(1) &&
                    left.GetColumn(2) != right.GetColumn(2) &&
                    left.GetColumn(3) != right.GetColumn(3);
        }
        #endregion

        private bool Equals(Matrix4x4 other)
        {
            return GetColumn(0).Equals(other.GetColumn(0)) &&
                    GetColumn(1).Equals(other.GetColumn(1)) &&
                    GetColumn(2).Equals(other.GetColumn(2)) &&
                    GetColumn(2).Equals(other.GetColumn(2));
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != GetType()) return false;
            return Equals((Matrix4x4)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GetColumn(0), GetColumn(1), GetColumn(2), GetColumn(3));
        }

    }
}
