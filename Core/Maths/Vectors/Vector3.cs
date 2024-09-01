
using System.Numerics;

namespace Core.Maths.Vectors
{
    public class Vector3() : RootVector3<float>()
    {
        public Vector3(float value) : this()
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3(float x, float y, float z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class Vector3Double() : RootVector3<double>()
    {
        public Vector3Double(double value) : this()
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3Double(double x, double y, double z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class Vector3Int() : Vector3<int>()
    {
        public Vector3Int(int value) : this()
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3Int(int x, int y, int z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class Vector3Byte() : Vector3<byte>()
    {
        public Vector3Byte(byte value) : this()
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3Byte(byte x, byte y, byte z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class RootVector3<T>() : Vector3<T>()
        where T : INumber<T>, IRootFunctions<T>
    {
        public static RootVector3<T> Zero => new(value: T.Zero);
        public static RootVector3<T> One => new(value: T.One);

        public static RootVector3<T> UnitX => new(x: T.One, y: T.Zero, z: T.Zero);
        public static RootVector3<T> UnitY => new(x: T.Zero, y: T.One, z: T.Zero);
        public static RootVector3<T> UnitZ => new(x: T.Zero, y: T.Zero, z: T.One);

        public T X
        {
            get => Values[0];
            set => Values[0] = value;
        }

        public T Y
        {
            get => Values[1];
            set => Values[1] = value;
        }

        public T Z
        {
            get => Values[2];
            set => Values[2] = value;
        }

        public RootVector3(T value) : this()
        {
            X = value;
            Y = value;
            Z = value;
        }

        public RootVector3(T x, T y, T z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static RootVector3<T> operator +(RootVector3<T> left, RootVector3<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            RootVector3<T> result = new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z
            };

            return result;
        }

        public static RootVector3<T> operator -(RootVector3<T> left, RootVector3<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            RootVector3<T> result = new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z
            };

            return result;
        }

        public static RootVector3<T> operator *(RootVector3<T> vector, T scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y,
                Z = scalar * vector.Z
            };
        }

        public static RootVector3<T> operator *(T scalar, RootVector3<T> vector) { return vector * scalar; }

        public static RootVector3<T> operator /(RootVector3<T> vector, T scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar,
                Z = vector.Z / scalar
            };
        }

        public static RootVector3<T> operator /(T scalar, RootVector3<T> vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y,
                Z = scalar / vector.Z
            };
        }

        public static bool operator ==(RootVector3<T>? left, RootVector3<T>? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(RootVector3<T>? left, RootVector3<T>? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z;
        }

        public static bool operator >(RootVector3<T> left, RootVector3<T> right) { throw new InvalidOperationException(); }
        public static bool operator >=(RootVector3<T> left, RootVector3<T> right) { throw new InvalidOperationException(); }
        public static bool operator <(RootVector3<T> left, RootVector3<T> right) { throw new InvalidOperationException(); }
        public static bool operator <=(RootVector3<T> left, RootVector3<T> right) { throw new InvalidOperationException(); }

        public static RootVector3<T> operator -(RootVector3<T> value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y,
                Z = -value.Z
            };
        }

        public T Magnitude()
        {
            T magnitudeSquared = Dot(this);
            return T.Sqrt(magnitudeSquared);
        }

        public RootVector3<T> Normalize()
        {
            T magnitude = Magnitude();
            return new()
            {
                Values =
                {
                    [0] = Values[0] / magnitude,
                    [1] = Values[1] / magnitude,
                    [2] = Values[2] / magnitude
                }
            };
        }

        protected bool Equals(RootVector3<T> other)
        {
            return Values.Equals(other.Values);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((RootVector3<T>)obj);
        }

        public override int GetHashCode()
        {
            return Values.GetHashCode();
        }
    }


    public class Vector3<T>() : Vector<T>(size: 3)
        where T : INumber<T>
    {
        public static Vector3<T> Zero => new(value: T.Zero);
        public static Vector3<T> One => new(value: T.One);

        public static Vector3<T> UnitX => new(x: T.One, y: T.Zero, z: T.Zero);
        public static Vector3<T> UnitY => new(x: T.Zero, y: T.One, z: T.Zero);
        public static Vector3<T> UnitZ => new(x: T.Zero, y: T.Zero, z: T.One);

        public T X
        {
            get => Values[0];
            set => Values[0] = value;
        }

        public T Y
        {
            get => Values[1];
            set => Values[1] = value;
        }

        public T Z
        {
            get => Values[2];
            set => Values[2] = value;
        }

        public Vector3(T value) : this()
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3(T x, T y, T z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3<T> operator +(Vector3<T> left, Vector3<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            Vector3<T> result = new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z
            };

            return result;
        }

        public static Vector3<T> operator -(Vector3<T> left, Vector3<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            Vector3<T> result = new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z
            };

            return result;
        }

        public static Vector3<T> operator *(Vector3<T> vector, T scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y,
                Z = scalar * vector.Z
            };
        }

        public static Vector3<T> operator *(T scalar, Vector3<T> vector) { return vector * scalar; }

        public static Vector3<T> operator /(Vector3<T> vector, T scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar,
                Z = vector.Z / scalar
            };
        }

        public static Vector3<T> operator /(T scalar, Vector3<T> vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y,
                Z = scalar / vector.Z
            };
        }

        public static bool operator ==(Vector3<T>? left, Vector3<T>? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(Vector3<T>? left, Vector3<T>? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z;
        }

        public static bool operator >(Vector3<T> left, Vector3<T> right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector3<T> left, Vector3<T> right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector3<T> left, Vector3<T> right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector3<T> left, Vector3<T> right) { throw new InvalidOperationException(); }

        public static Vector3<T> operator -(Vector3<T> value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y,
                Z = -value.Z
            };
        }

        public T Dot(Vector3<T> other)
        {
            return X * other.X +
                   Y * other.Y +
                   Z * other.Z;
        }

        public Vector3<T> CrossProduct(Vector3<T> other)
        {
            return new()
            {
                X = Y * other.Z - Z * other.Y,
                Y = Z * other.X - X * other.Z,
                Z = X * other.Y - Y * other.X
            };
        }

        protected bool Equals(Vector3<T> other)
        {
            return Values.Equals(other.Values);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector3<T>)obj);
        }

        public override int GetHashCode()
        {
            return Values.GetHashCode();
        }
    }
}
