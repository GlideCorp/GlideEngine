using Core.Maths;
using Core.Maths.Vectors;
using Silk.NET.Maths;
using System.Numerics;
using Quaternion = Core.Maths.Quaternion;

namespace Core.Extensions
{
    //TODO: Move from here
    public static class Vector3FloatExtensions
    {
        //This can go inside vector3Float, vector3double
        public static Vector3Float RotateAround (this Vector3Float v, Vector3Float point, float alpha, float beta)
        {
            Vector3Float direction = (point - v).Normalize();
            Vector3Float right = direction.CrossProduct(Vector3Float.UnitY);
            Vector3Float up = direction.CrossProduct(right).Normalize();

            Quaternion rot = Quaternion.AxisAngle(up, alpha) * Quaternion.AxisAngle(right, beta);

            Vector3Float pToC = v - point;
            return rot*pToC + point;
        }
        public static Vector3 ToSystem(this Vector3Float v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }
        /*
        public static Vector3D<float> ToSilk(this Vector3 v)
        {
            return new Vector3D<float>(v.X, v.Y, v.Z);
        }

        public static Quaternion<float> ToQuat(this Vector3D<float> v)
        {
            float cy = MathF.Cos(v.Z * 0.5f);
            float sy = MathF.Sin(v.Z * 0.5f);
            float cp = MathF.Cos(v.Y * 0.5f);
            float sp = MathF.Sin(v.Y * 0.5f);
            float cr = MathF.Cos(v.X * 0.5f);
            float sr = MathF.Sin(v.X * 0.5f);

            return new Quaternion<float>
            {
                W = (cr * cp * cy + sr * sp * sy),
                X = (sr * cp * cy - cr * sp * sy),
                Y = (cr * sp * cy + sr * cp * sy),
                Z = (cr * cp * sy - sr * sp * cy)
            };
        }
        */
    }
}
