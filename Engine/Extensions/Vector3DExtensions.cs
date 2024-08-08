using Silk.NET.Maths;

namespace Core.Extensions
{
    public static class Vector3DExtensions
    {
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
