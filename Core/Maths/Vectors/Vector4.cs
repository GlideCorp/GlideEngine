
using System.Numerics;

namespace Core.Maths.Vectors
{
    public class Vector4 : RootVector4<float> { }
    public class Vector4Double : RootVector4<double> { }
    public class Vector4Int : Vector4<int> { }
    public class Vector4Byte : Vector4<byte> { }

    public class RootVector4<T> : Vector4<T>
        where T : INumber<T>, IRootFunctions<T>
    {
        public T Magnitude()
        {
            T magnitudeSquared = Dot(this);
            return T.Sqrt(magnitudeSquared);
        }

        public RootVector4<T> Normalize()
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
    }

    public class Vector4<T>() : Vector<T>(size: 4)
        where T : INumber<T>
    {
        public static Vector4<T> Zero => new(value: T.Zero);
        public static Vector4<T> One => new(value: T.One);

        public static Vector4<T> UnitX => new(x: T.One, y: T.Zero, z: T.Zero, w: T.Zero);
        public static Vector4<T> UnitY => new(x: T.Zero, y: T.One, z: T.Zero, w: T.Zero);
        public static Vector4<T> UnitZ => new(x: T.Zero, y: T.Zero, z: T.One, w: T.Zero);
        public static Vector4<T> UnitW => new(x: T.Zero, y: T.Zero, z: T.Zero, w: T.One);

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

        public T W
        {
            get => Values[3];
            set => Values[3] = value;
        }

        public Vector4(T value) : this()
        {
            X = value;
            Y = value;
            Z = value;
            W = value;
        }

        public Vector4(T x, T y, T z, T w) : this()
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static Vector4<T> operator +(Vector4<T> left, Vector4<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            Vector4<T> result = new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z,
                W = left.W + right.W
            };

            return result;
        }

        public static Vector4<T> operator -(Vector4<T> left, Vector4<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            Vector4<T> result = new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z,
                W = left.W - right.W
            };

            return result;
        }

        public static Vector4<T> operator *(Vector4<T> vector, T scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y,
                Z = scalar * vector.Z,
                W = scalar * vector.W
            };
        }

        public static Vector4<T> operator *(T scalar, Vector4<T> vector) { return vector * scalar; }

        public static Vector4<T> operator /(Vector4<T> vector, T scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar,
                Z = vector.Z / scalar,
                W = vector.W / scalar
            };
        }

        public static Vector4<T> operator /(T scalar, Vector4<T> vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y,
                Z = scalar / vector.Z,
                W = scalar / vector.W
            };
        }

        public static bool operator ==(Vector4<T>? left, Vector4<T>? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z &&
                   left.W == right.W;
        }

        public static bool operator !=(Vector4<T>? left, Vector4<T>? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z &&
                   left.W != right.W;
        }

        public static bool operator >(Vector4<T> left, Vector4<T> right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector4<T> left, Vector4<T> right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector4<T> left, Vector4<T> right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector4<T> left, Vector4<T> right) { throw new InvalidOperationException(); }

        public static Vector4<T> operator -(Vector4<T> value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y,
                Z = -value.Z,
                W = -value.W
            };
        }

        public T Dot(Vector4<T> other)
        {
            return X * other.X +
                   Y * other.Y +
                   Z * other.Z +
                   W * other.W;
        }

        protected bool Equals(Vector4<T> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Vector4<T>)obj);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
