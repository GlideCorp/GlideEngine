using Core.Maths.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Maths
{
    public class Quaternion : Vector4
    {
        public static Quaternion Identity => new(0, 0, 0, 1);

        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Quaternion(Vector3 xyz, float w) : base(xyz.X, xyz.Y, xyz.Z, w) { }

        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            return new (
                left.W * right.X + left.X * right.W + left.Y * right.Z - left.Z * right.Y,
                left.W * right.Y + left.Y * right.W + left.Z * right.X - left.X * right.Z,
                left.W * right.Z + left.Z * right.W + left.X * right.Y - left.Y * right.X,
                left.W * right.W - left.X * right.X - left.Y * right.Y - left.Z * right.Z);
        }
        public static Vector3 operator *(Quaternion left, Vector3 right)
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

            Vector3 result = new Vector3();
            result.X = (1F - (yy + zz)) * right.X + (xy - wz) * right.Y + (xz + wy) * right.Z;
            result.Y = (xy + wz) * right.X + (1F - (xx + zz)) * right.Y + (yz - wx) * right.Z;
            result.Z = (xz - wy) * right.X + (yz + wx) * right.Y + (1F - (xx + yy)) * right.Z;
            return result;
        }

        public static Quaternion LookRotation(Vector3 forward, Vector3 upwards)
        {
            //Controlla correttezza, in teoria funziona ma se stai leggendo questo e stai avendo problemi controlla
            //https://math.stackexchange.com/questions/60511/quaternion-for-an-object-that-to-point-in-a-direction
            Vector3 v = Vector3.UnitX;
            Vector3 w = forward;

            v = (v - (v.Dot(upwards) * upwards)).Normalize();
            w = (w - (w.Dot(upwards) * upwards)).Normalize();

            Vector3 a = v.CrossProduct(w);
            float theta = MathF.Acos(v.Dot(w));
            return AxisAngle(a, theta);
        }

        public static Quaternion AxisAngle(Vector3 axis, float angle)
        {
            float sina, cosa;
            (sina, cosa) = MathF.SinCos(0.5f * angle);
            
            Vector3 result = axis * sina;

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
    }
}
