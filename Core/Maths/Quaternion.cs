using Core.Maths.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core.Maths
{
    public struct Quaternion(float x, float y, float z, float w) :
        IMultiplyOperators<Quaternion, Quaternion, Quaternion>,
        IMultiplyOperators<Quaternion, Vector3Float, Vector3Float>,
        IComparisonOperators<Quaternion, Quaternion, bool>,
        IUnaryNegationOperators<Quaternion, Quaternion>
    {
        public static Quaternion Identity => new(x: 0, y: 0, z: 0, w: 1);

        public float X { get; set; } = x;
        public float Y { get; set; } = y;
        public float Z { get; set; } = z;
        public float W { get; set; } = w;

        public Quaternion() : this(0, 0, 0, 1) { }
        public Quaternion(Vector3Float xyz, float w) : this(xyz.X, xyz.Y, xyz.Z, w) { }

        #region Arithmetic Operations
        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            return new (
                left.W * right.X + left.X * right.W + left.Y * right.Z - left.Z * right.Y,
                left.W * right.Y + left.Y * right.W + left.Z * right.X - left.X * right.Z,
                left.W * right.Z + left.Z * right.W + left.X * right.Y - left.Y * right.X,
                left.W * right.W - left.X * right.X - left.Y * right.Y - left.Z * right.Z);
        }
        public static Vector3Float operator *(Quaternion left, Vector3Float right)
        {
            float x = left.X * 2F;
            float y = left.Y * 2F;
            float z = left.Z * 2F;
            float xx = left.X * x;
            float yy = left.Y * y;
            float zz = left.Z * z;
            float xy = left.X * y;
            float xz = left.X * z;
            float yz = left.Y * z;
            float wx = left.W * x;
            float wy = left.W * y;
            float wz = left.W * z;

            Vector3Float result = new Vector3Float();
            result.X = (1F - (yy + zz)) * right.X + (xy - wz) * right.Y + (xz + wy) * right.Z;
            result.Y = (xy + wz) * right.X + (1F - (xx + zz)) * right.Y + (yz - wx) * right.Z;
            result.Z = (xz - wy) * right.X + (yz + wx) * right.Y + (1F - (xx + yy)) * right.Z;
            return result;
        }

        public static Quaternion operator -(Quaternion value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y,
                Z = -value.Z,
                W = -value.W
            };
        }

        public static bool operator >(Quaternion left, Quaternion right) { throw new NotImplementedException(); }

        public static bool operator >=(Quaternion left, Quaternion right) { throw new NotImplementedException(); }

        public static bool operator <(Quaternion left, Quaternion right) { throw new NotImplementedException(); }

        public static bool operator <=(Quaternion left, Quaternion right) { throw new NotImplementedException(); }

        public static bool operator ==(Quaternion left, Quaternion right)
        {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z &&
                   left.W == right.W;
        }

        public static bool operator !=(Quaternion left, Quaternion right)
        {
            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z &&
                   left.W != right.W;
        }
        #endregion

        #region Quaternion Operations
        public static Quaternion LookRotation(Vector3Float forward, Vector3Float upwards)
        {
            //Controlla correttezza, in teoria funziona ma se stai leggendo questo e stai avendo problemi controlla
            //https://math.stackexchange.com/questions/60511/quaternion-for-an-object-that-to-point-in-a-direction
            Vector3Float v = Vector3Float.UnitX;
            Vector3Float w = forward;

            v = (v - (v.Dot(upwards) * upwards)).Normalize();
            w = (w - (w.Dot(upwards) * upwards)).Normalize();

            Vector3Float a = v.CrossProduct(w);
            float theta = MathF.Acos(v.Dot(w));
            return AxisAngle(a, theta);
        }

        public static Quaternion AxisAngle(Vector3Float axis, float angle)
        {
            float sina, cosa;
            (sina, cosa) = MathF.SinCos(0.5f * angle);

            Vector3Float result = axis * sina;

            return new Quaternion(result, cosa);
        }

        public static Quaternion Euler(Vector3 eulerAngles)
        {
            float cy = MathF.Cos(eulerAngles.Z * 0.5f);
            float sy = MathF.Sin(eulerAngles.Z * 0.5f);
            float cp = MathF.Cos(eulerAngles.Y * 0.5f);
            float sp = MathF.Sin(eulerAngles.Y * 0.5f);
            float cr = MathF.Cos(eulerAngles.X * 0.5f);
            float sr = MathF.Sin(eulerAngles.X * 0.5f);

            return new Quaternion
            (
                x: (sr * cp * cy - cr * sp * sy),
                y: (cr * sp * cy + sr * cp * sy),
                z: (cr * cp * sy - sr * sp * cy),
                w: (cr * cp * cy + sr * sp * sy)
            );
        }
        #endregion

        private bool Equals(Quaternion other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Quaternion)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }
    }
}
