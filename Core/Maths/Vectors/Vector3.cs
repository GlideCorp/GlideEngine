
using System.Numerics;

namespace Core.Maths.Vectors
{
    public class Vector3<T>() :
        IAdditionOperators<Vector3<T>, Vector3<T>, Vector3<T>>,
        ISubtractionOperators<Vector3<T>, Vector3<T>, Vector3<T>>,
        IMultiplyOperators<Vector3<T>, T, Vector3<T>>,
        IDivisionOperators<Vector3<T>, T, Vector3<T>>,
        IComparisonOperators<Vector3<T>, Vector3<T>, bool>,
        IUnaryNegationOperators<Vector3<T>, Vector3<T>>
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

        public T[] Values { get; set; } = new T[3];

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
            throw new NotImplementedException();
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Vector3<T>)obj);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
