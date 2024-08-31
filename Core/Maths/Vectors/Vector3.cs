

namespace Core.Maths.Vectors
{
    public class Vector3 : Vector<float>
    {
        public static Vector3 Zero => new(x: 0, y: 0, z: 0);
        public static Vector3 One => new(x: 1, y: 1, z: 1);

        public static Vector3 UnitX => new(x: 1, y: 0, z: 0);
        public static Vector3 UnitY => new(x: 0, y: 1, z: 0);
        public static Vector3 UnitZ => new(x: 0, y: 0, z: 1);

        public float X
        {
            get => Values[0];
            set => Values[0] = value;
        }

        public float Y
        {
            get => Values[1];
            set => Values[1] = value;
        }

        public float Z
        {
            get => Values[2];
            set => Values[2] = value;
        }

        public Vector3() : base(size: 3) { }

        public Vector3(float value) : base(size: 3)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3(float x, float y, float z) : base(size: 3)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            Vector3 result = new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z
            };

            return result;
        }

        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            Vector3 result = new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z
            };

            return result;
        }

        public static Vector3 operator *(Vector3 vector, float scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y,
                Z = scalar * vector.Z,
            };
        }

        public static Vector3 operator *(float scalar, Vector3 vector) { return vector * scalar; }

        public static Vector3 operator /(Vector3 vector, float scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y,
                Z = scalar * vector.Z,
            };
        }

        public static Vector3 operator /(float scalar, Vector3 vector) { return vector / scalar; }

        public static bool operator ==(Vector3? left, Vector3? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(Vector3? left, Vector3? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z;
        }

        public static bool operator >(Vector3 left, Vector3 right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X > right.X &&
                   left.Y > right.Y &&
                   left.Z > right.Z;
        }

        public static bool operator >=(Vector3 left, Vector3 right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X >= right.X &&
                   left.Y >= right.Y &&
                   left.Z >= right.Z;
        }

        public static bool operator <(Vector3 left, Vector3 right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X < right.X &&
                   left.Y < right.Y &&
                   left.Z < right.Z;
        }

        public static bool operator <=(Vector3 left, Vector3 right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X <= right.X &&
                   left.Y <= right.Y &&
                   left.Z <= right.Z;
        }

        public static Vector3 operator -(Vector3 value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y,
                Z = -value.Z
            };
        }

        public float Dot(Vector3 other)
        {
            return X * other.X + 
                   Y * other.Y +
                   Z * other.Z;
        }
    }
}
