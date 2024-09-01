
using System.Numerics;

namespace Core.Maths.Vectors
{
    public class Vector2() : RootedVector2<float>(),
        IAdditionOperators<Vector2, Vector2, Vector2>,
        ISubtractionOperators<Vector2, Vector2, Vector2>,
        IMultiplyOperators<Vector2, float, Vector2>,
        IDivisionOperators<Vector2, float, Vector2>,
        IComparisonOperators<Vector2, Vector2, bool>,
        IUnaryNegationOperators<Vector2, Vector2>
    {
        public static Vector2 Zero => new(value: 0);
        public static Vector2 One => new(value: 0);

        public static Vector2 UnitX => new(x: 1, y: 0);
        public static Vector2 UnitY => new(x: 0, y: 1);

        public Vector2(float value) : this()
        {
            X = value;
            Y = value;
        }

        public Vector2(float x, float y) : this()
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y
            };
        }

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y
            };
        }

        public static Vector2 operator *(Vector2 vector, float scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y
            };
        }

        public static Vector2 operator *(float scalar, Vector2 vector) { return vector * scalar; }

        public static Vector2 operator /(Vector2 vector, float scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar
            };
        }

        public static Vector2 operator /(float scalar, Vector2 vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y
            };
        }

        public static bool operator ==(Vector2? left, Vector2? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y;
        }

        public static bool operator !=(Vector2? left, Vector2? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y;
        }

        public static bool operator >(Vector2 left, Vector2 right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector2 left, Vector2 right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector2 left, Vector2 right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector2 left, Vector2 right) { throw new InvalidOperationException(); }

        public static Vector2 operator -(Vector2 value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y
            };
        }

        public override Vector2 Normalize()
        {
            float magnitude = Magnitude();
            return new()
            {
                Values =
                {
                    [0] = Values[0] / magnitude,
                    [1] = Values[1] / magnitude
                }
            };
        }

        protected bool Equals(Vector2 other)
        {
            return Values.Equals(other.Values);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector2)obj);
        }

        public override int GetHashCode()
        {
            return Values.GetHashCode();
        }
    }

    public class Vector2Double() : RootedVector2<double>(),
        IAdditionOperators<Vector2Double, Vector2Double, Vector2Double>,
        ISubtractionOperators<Vector2Double, Vector2Double, Vector2Double>,
        IMultiplyOperators<Vector2Double, double, Vector2Double>,
        IDivisionOperators<Vector2Double, double, Vector2Double>,
        IComparisonOperators<Vector2Double, Vector2Double, bool>,
        IUnaryNegationOperators<Vector2Double, Vector2Double>
    {
        public static Vector2Double Zero => new(value: 0);
        public static Vector2Double One => new(value: 0);

        public static Vector2Double UnitX => new(x: 1, y: 0);
        public static Vector2Double UnitY => new(x: 0, y: 1);

        public Vector2Double(double value) : this()
        {
            X = value;
            Y = value;
        }

        public Vector2Double(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        public static Vector2Double operator +(Vector2Double left, Vector2Double right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y
            };
        }

        public static Vector2Double operator -(Vector2Double left, Vector2Double right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y
            };
        }

        public static Vector2Double operator *(Vector2Double vector, double scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y
            };
        }

        public static Vector2Double operator *(double scalar, Vector2Double vector) { return vector * scalar; }

        public static Vector2Double operator /(Vector2Double vector, double scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar
            };
        }

        public static Vector2Double operator /(double scalar, Vector2Double vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y
            };
        }

        public static bool operator ==(Vector2Double? left, Vector2Double? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y;
        }

        public static bool operator !=(Vector2Double? left, Vector2Double? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y;
        }

        public static bool operator >(Vector2Double left, Vector2Double right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector2Double left, Vector2Double right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector2Double left, Vector2Double right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector2Double left, Vector2Double right) { throw new InvalidOperationException(); }

        public static Vector2Double operator -(Vector2Double value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y
            };
        }

        public override Vector2Double Normalize()
        {
            double magnitude = Magnitude();
            return new()
            {
                Values =
                {
                    [0] = Values[0] / magnitude,
                    [1] = Values[1] / magnitude
                }
            };
        }

        protected bool Equals(Vector2Double other)
        {
            return Values.Equals(other.Values);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector2Double)obj);
        }

        public override int GetHashCode()
        {
            return Values.GetHashCode();
        }
    }

    public class Vector2Int() : Vector2<int>(),
        IAdditionOperators<Vector2Int, Vector2Int, Vector2Int>,
        ISubtractionOperators<Vector2Int, Vector2Int, Vector2Int>,
        IMultiplyOperators<Vector2Int, int, Vector2Int>,
        IDivisionOperators<Vector2Int, int, Vector2Int>,
        IComparisonOperators<Vector2Int, Vector2Int, bool>,
        IUnaryNegationOperators<Vector2Int, Vector2Int>
    {
        public static Vector2Int Zero => new(value: 0);
        public static Vector2Int One => new(value: 0);

        public static Vector2Int UnitX => new(x: 1, y: 0);
        public static Vector2Int UnitY => new(x: 0, y: 1);

        public Vector2Int(int value) : this()
        {
            X = value;
            Y = value;
        }

        public Vector2Int(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public static Vector2Int operator +(Vector2Int left, Vector2Int right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y
            };
        }

        public static Vector2Int operator -(Vector2Int left, Vector2Int right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y
            };
        }

        public static Vector2Int operator *(Vector2Int vector, int scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y
            };
        }

        public static Vector2Int operator *(int scalar, Vector2Int vector) { return vector * scalar; }

        public static Vector2Int operator /(Vector2Int vector, int scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar
            };
        }

        public static Vector2Int operator /(int scalar, Vector2Int vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y
            };
        }

        public static bool operator ==(Vector2Int? left, Vector2Int? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y;
        }

        public static bool operator !=(Vector2Int? left, Vector2Int? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y;
        }

        public static bool operator >(Vector2Int left, Vector2Int right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector2Int left, Vector2Int right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector2Int left, Vector2Int right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector2Int left, Vector2Int right) { throw new InvalidOperationException(); }

        public static Vector2Int operator -(Vector2Int value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y
            };
        }

        protected bool Equals(Vector2Int other)
        {
            return Values.Equals(other.Values);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector2Int)obj);
        }

        public override int GetHashCode()
        {
            return Values.GetHashCode();
        }
    }

    public class Vector2Byte() : Vector2<byte>(),
        IAdditionOperators<Vector2Byte, Vector2Byte, Vector2Byte>,
        ISubtractionOperators<Vector2Byte, Vector2Byte, Vector2Byte>,
        IMultiplyOperators<Vector2Byte, byte, Vector2Byte>,
        IDivisionOperators<Vector2Byte, byte, Vector2Byte>,
        IComparisonOperators<Vector2Byte, Vector2Byte, bool>,
        IUnaryNegationOperators<Vector2Byte, Vector2Byte>
    {
        public static Vector2Byte Zero => new(value: 0);
        public static Vector2Byte One => new(value: 0);

        public static Vector2Byte UnitX => new(x: 1, y: 0);
        public static Vector2Byte UnitY => new (x: 0, y: 1);

        public Vector2Byte(byte value) : this()
        {
            X = value;
            Y = value;
        }

        public Vector2Byte(byte x, byte y) : this()
        {
            X = x;
            Y = y;
        }
        public static Vector2Byte operator +(Vector2Byte left, Vector2Byte right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = (byte)(left.X + right.X),
                Y = (byte)(left.Y + right.Y)
            };
        }

        public static Vector2Byte operator -(Vector2Byte left, Vector2Byte right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = (byte)(left.X - right.X),
                Y = (byte)(left.Y - right.Y)
            };
        }

        public static Vector2Byte operator *(Vector2Byte vector, byte scalar)
        {
            return new()
            {
                X = (byte)(scalar * vector.X),
                Y = (byte)(scalar * vector.Y)
            };
        }

        public static Vector2Byte operator *(byte scalar, Vector2Byte vector) { return vector * scalar; }

        public static Vector2Byte operator /(Vector2Byte vector, byte scalar)
        {
            return new()
            {
                X = (byte)(vector.X / scalar),
                Y = (byte)(vector.Y / scalar)
            };
        }

        public static Vector2Byte operator /(byte scalar, Vector2Byte vector)
        {
            return new()
            {
                X = (byte)(scalar / vector.X),
                Y = (byte)(scalar / vector.Y)
            };
        }

        public static bool operator ==(Vector2Byte? left, Vector2Byte? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y;
        }

        public static bool operator !=(Vector2Byte? left, Vector2Byte? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y;
        }

        public static bool operator >(Vector2Byte left, Vector2Byte right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector2Byte left, Vector2Byte right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector2Byte left, Vector2Byte right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector2Byte left, Vector2Byte right) { throw new InvalidOperationException(); }

        public static Vector2Byte operator -(Vector2Byte value)
        {
            return new()
            {
                X = (byte)-value.X,
                Y = (byte)-value.Y
            };
        }

        protected bool Equals(Vector2Byte other)
        {
            return Values.Equals(other.Values);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector2Byte)obj);
        }

        public override int GetHashCode()
        {
            return Values.GetHashCode();
        }
    }

    public class RootedVector2<T>() : RootedVector<T>(size: 2),
        IAdditionOperators<RootedVector2<T>, RootedVector2<T>, RootedVector2<T>>,
        ISubtractionOperators<RootedVector2<T>, RootedVector2<T>, RootedVector2<T>>,
        IMultiplyOperators<RootedVector2<T>, T, RootedVector2<T>>,
        IDivisionOperators<RootedVector2<T>, T, RootedVector2<T>>,
        IComparisonOperators<RootedVector2<T>, RootedVector2<T>, bool>,
        IUnaryNegationOperators<RootedVector2<T>, RootedVector2<T>>
        where T : INumber<T>, IRootFunctions<T>
    {
        public static RootedVector2<T> Zero => new(value: T.Zero);
        public static RootedVector2<T> One => new(value: T.One);

        public static RootedVector2<T> UnitX => new(x: T.One, y: T.Zero);
        public static RootedVector2<T> UnitY => new(x: T.Zero, y: T.One);

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

        public RootedVector2(T value) : this()
        {
            X = value;
            Y = value;
        }

        public RootedVector2(T x, T y) : this()
        {
            X = x;
            Y = y;
        }

        public static RootedVector2<T> operator +(RootedVector2<T> left, RootedVector2<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y
            };
        }

        public static RootedVector2<T> operator -(RootedVector2<T> left, RootedVector2<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y
            };
        }

        public static RootedVector2<T> operator *(RootedVector2<T> vector, T scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y
            };
        }

        public static RootedVector2<T> operator *(T scalar, RootedVector2<T> vector) { return vector * scalar; }

        public static RootedVector2<T> operator /(RootedVector2<T> vector, T scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar
            };
        }

        public static RootedVector2<T> operator /(T scalar, RootedVector2<T> vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y
            };
        }

        public static bool operator ==(RootedVector2<T>? left, RootedVector2<T>? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y;
        }

        public static bool operator !=(RootedVector2<T>? left, RootedVector2<T>? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y;
        }

        public static bool operator >(RootedVector2<T> left, RootedVector2<T> right) { throw new InvalidOperationException(); }
        public static bool operator >=(RootedVector2<T> left, RootedVector2<T> right) { throw new InvalidOperationException(); }
        public static bool operator <(RootedVector2<T> left, RootedVector2<T> right) { throw new InvalidOperationException(); }
        public static bool operator <=(RootedVector2<T> left, RootedVector2<T> right) { throw new InvalidOperationException(); }

        public static RootedVector2<T> operator -(RootedVector2<T> value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y
            };
        }

        public T Dot(RootedVector2<T> other)
        {
            return X * other.X +
                   Y * other.Y;
        }

        public override T Magnitude()
        {
            T magnitudeSquared = Dot(this);
            return T.Sqrt(magnitudeSquared);
        }

        public override RootedVector2<T> Normalize()
        {
            T magnitude = Magnitude();
            return new()
            {
                Values =
                {
                    [0] = Values[0] / magnitude,
                    [1] = Values[1] / magnitude
                }
            };
        }

        protected bool Equals(RootedVector2<T> other)
        {
            return Values.Equals(other.Values);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((RootedVector2<T>)obj);
        }

        public override int GetHashCode()
        {
            return Values.GetHashCode();
        }
    }

    public class Vector2<T>() : Vector<T>(size: 2),
        IAdditionOperators<Vector2<T>, Vector2<T>, Vector2<T>>,
        ISubtractionOperators<Vector2<T>, Vector2<T>, Vector2<T>>,
        IMultiplyOperators<Vector2<T>, T, Vector2<T>>,
        IDivisionOperators<Vector2<T>, T, Vector2<T>>,
        IComparisonOperators<Vector2<T>, Vector2<T>, bool>,
        IUnaryNegationOperators<Vector2<T>, Vector2<T>>
        where T : INumber<T>
    {
        public static Vector2<T> Zero => new(value: T.Zero);
        public static Vector2<T> One => new(value: T.One);

        public static Vector2<T> UnitX => new(x: T.One, y: T.Zero);
        public static Vector2<T> UnitY => new(x: T.Zero, y: T.One);

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

        public Vector2(T value) : this()
        {
            X = value;
            Y = value;
        }

        public Vector2(T x, T y) : this()
        {
            X = x;
            Y = y;
        }

        public static Vector2<T> operator +(Vector2<T> left, Vector2<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y
            };
        }

        public static Vector2<T> operator -(Vector2<T> left, Vector2<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y
            };
        }

        public static Vector2<T> operator *(Vector2<T> vector, T scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y
            };
        }

        public static Vector2<T> operator *(T scalar, Vector2<T> vector) { return vector * scalar; }

        public static Vector2<T> operator /(Vector2<T> vector, T scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar
            };
        }

        public static Vector2<T> operator /(T scalar, Vector2<T> vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y
            };
        }

        public static bool operator ==(Vector2<T>? left, Vector2<T>? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y;
        }

        public static bool operator !=(Vector2<T>? left, Vector2<T>? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y;
        }

        public static bool operator >(Vector2<T> left, Vector2<T> right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector2<T> left, Vector2<T> right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector2<T> left, Vector2<T> right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector2<T> left, Vector2<T> right) { throw new InvalidOperationException(); }

        public static Vector2<T> operator -(Vector2<T> value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y
            };
        }

        public T Dot(Vector2<T> other)
        {
            return X * other.X +
                   Y * other.Y;
        }

        protected bool Equals(Vector2<T> other)
        {
            return Values.Equals(other.Values);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector2<T>)obj);
        }

        public override int GetHashCode()
        {
            return Values.GetHashCode();
        }
    }
}
