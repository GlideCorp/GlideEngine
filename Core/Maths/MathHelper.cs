using Core.Maths.Vectors;
using System;
using System.Numerics;

namespace Core.Maths
{
    public static class MathHelper
    {
        public static readonly float Deg2Rad = (MathF.PI / 180);
        public static readonly float Rad2Deg = (180 / MathF.PI);

        /// <summary>
        /// Normalizes radians angle in Vector3Float between -PI to PI
        /// </summary>
        /// <param name="anglesVector">Vector3Float with angles in radians</param>
        /// <returns></returns>
        public static Vector3Float NormalizeAngles(Vector3Float anglesVector)
        {
            return new()
            {
                X = NormalizeAngle(anglesVector.X),
                Y = NormalizeAngle(anglesVector.Y),
                Z = NormalizeAngle(anglesVector.Z)
            };
        }

        /// <summary>
        /// Normalizes radians angle between -PI to PI
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        /// <returns></returns>
        public static float NormalizeAngle(float angle)
        {
            return angle - 2*MathF.PI * MathF.Floor((angle + MathF.PI) / 2*MathF.PI);
        }
    }
}
