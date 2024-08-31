﻿using Silk.NET.Maths;
using System.Numerics;

namespace Core.Extensions
{
    public static class Vector3DExtensions
    {
        public static Vector3D<float> RotateAround (this Vector3D<float> v, Vector3D<float> point, float alpha, float beta)
        {
            Vector3D<float> direction = Vector3D.Normalize(point - v);
            Vector3D<float> right = Vector3D.Cross(direction, Vector3D<float>.UnitY);
            Vector3D<float> up = Vector3D.Cross(direction, right);

            Quaternion<float> rot = Quaternion<float>.CreateFromAxisAngle(up, alpha) * Quaternion<float>.CreateFromAxisAngle(right, beta);

            Vector3D<float> pToC = v - point;
            return Vector3D.Transform(pToC, rot) + point;
        }


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
    }
}