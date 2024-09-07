
using System;
using System.Numerics;

namespace Core.Maths.Vectors
{
    public class Vector2Float(float x, float y) : RootedVector2<float>(x, y),
        IAdditionOperators<Vector2Float, Vector2Float, Vector2Float>,
        ISubtractionOperators<Vector2Float, Vector2Float, Vector2Float>,
        IMultiplyOperators<Vector2Float, float, Vector2Float>,
        IDivisionOperators<Vector2Float, float, Vector2Float>,
        IComparisonOperators<Vector2Float, Vector2Float, bool>,
        IUnaryNegationOperators<Vector2Float, Vector2Float>
    {
        public new static Vector2Float Zero => new(value: 0);
        public new static Vector2Float One => new(value: 0);

        public new static Vector2Float UnitX => new(x: 1, y: 0);
        public new static Vector2Float UnitY => new(x: 0, y: 1);

        public Vector2Float() : this(x: 0, y: 0) { }
        public Vector2Float(float value) : this(x: value, y: value) { }

        public static Vector2Float operator +(Vector2Float left, Vector2Float right)
        {
            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y
            };
        }

        public static Vector2Float operator -(Vector2Float left, Vector2Float right)
        {
            return new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y
            };
        }

        public static Vector2Float operator *(Vector2Float vector, float scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y
            };
        }

        public static Vector2Float operator *(float scalar, Vector2Float vector) { return vector * scalar; }

        public static Vector2Float operator /(Vector2Float vector, float scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar
            };
        }

        public static Vector2Float operator /(float scalar, Vector2Float vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y
            };
        }

        public static bool operator ==(Vector2Float? left, Vector2Float? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y;
        }

        public static bool operator !=(Vector2Float? left, Vector2Float? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y;
        }

        public static bool operator >(Vector2Float left, Vector2Float right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector2Float left, Vector2Float right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector2Float left, Vector2Float right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector2Float left, Vector2Float right) { throw new InvalidOperationException(); }

        public static Vector2Float operator -(Vector2Float value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y
            };
        }

        public override Vector2Float Normalize()
        {
            float magnitude = Magnitude();
            return new()
            {
                X = X / magnitude,
                Y = Y / magnitude
            };
        }

        protected bool Equals(Vector2Float other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector2Float)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    public class Vector2Double(double x, double y) : RootedVector2<double>(x, y),
        IAdditionOperators<Vector2Double, Vector2Double, Vector2Double>,
        ISubtractionOperators<Vector2Double, Vector2Double, Vector2Double>,
        IMultiplyOperators<Vector2Double, double, Vector2Double>,
        IDivisionOperators<Vector2Double, double, Vector2Double>,
        IComparisonOperators<Vector2Double, Vector2Double, bool>,
        IUnaryNegationOperators<Vector2Double, Vector2Double>
    {
        public new static Vector2Double Zero => new(value: 0);
        public new static Vector2Double One => new(value: 0);

        public new static Vector2Double UnitX => new(x: 1, y: 0);
        public new static Vector2Double UnitY => new(x: 0, y: 1);

        public Vector2Double() : this(x: 0, y: 0) { }
        public Vector2Double(double value) : this(x: value, y: value) { }

        public static Vector2Double operator +(Vector2Double left, Vector2Double right)
        {
            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y
            };
        }

        public static Vector2Double operator -(Vector2Double left, Vector2Double right)
        {
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
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y;
        }

        public static bool operator !=(Vector2Double? left, Vector2Double? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

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
                X = X / magnitude,
                Y = Y / magnitude
            };
        }

        protected bool Equals(Vector2Double other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
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
            return HashCode.Combine(X, Y);
        }
    }

    public class Vector2Int(int x, int y) : Vector2<int>(x, y),
        IAdditionOperators<Vector2Int, Vector2Int, Vector2Int>,
        ISubtractionOperators<Vector2Int, Vector2Int, Vector2Int>,
        IMultiplyOperators<Vector2Int, int, Vector2Int>,
        IDivisionOperators<Vector2Int, int, Vector2Int>,
        IComparisonOperators<Vector2Int, Vector2Int, bool>,
        IUnaryNegationOperators<Vector2Int, Vector2Int>
    {
        public new static Vector2Int Zero => new(value: 0);
        public new static Vector2Int One => new(value: 0);

        public new static Vector2Int UnitX => new(x: 1, y: 0);
        public new static Vector2Int UnitY => new(x: 0, y: 1);

        public Vector2Int() : this(x: 0, y: 0) { }
        public Vector2Int(int value) : this(x: value, y: value) { }

        public static Vector2Int operator +(Vector2Int left, Vector2Int right)
        {
            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y
            };
        }

        public static Vector2Int operator -(Vector2Int left, Vector2Int right)
        {
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
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y;
        }

        public static bool operator !=(Vector2Int? left, Vector2Int? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

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
            return X.Equals(other.X) && Y.Equals(other.Y);
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
            return HashCode.Combine(X, Y);
        }
    }

    public class Vector2Byte(byte x, byte y) : Vector2<byte>(x, y),
        IAdditionOperators<Vector2Byte, Vector2Byte, Vector2Byte>,
        ISubtractionOperators<Vector2Byte, Vector2Byte, Vector2Byte>,
        IMultiplyOperators<Vector2Byte, byte, Vector2Byte>,
        IDivisionOperators<Vector2Byte, byte, Vector2Byte>,
        IComparisonOperators<Vector2Byte, Vector2Byte, bool>,
        IUnaryNegationOperators<Vector2Byte, Vector2Byte>
    {
        public new static Vector2Byte Zero => new(value: 0);
        public new static Vector2Byte One => new(value: 0);

        public new static Vector2Byte UnitX => new(x: 1, y: 0);
        public new static Vector2Byte UnitY => new(x: 0, y: 1);

        public Vector2Byte() : this(x: 0, y: 0) { }
        public Vector2Byte(byte value) : this(x: value, y: value) { }

        public static Vector2Byte operator +(Vector2Byte left, Vector2Byte right)
        {
            return new()
            {
                X = (byte)(left.X + right.X),
                Y = (byte)(left.Y + right.Y)
            };
        }

        public static Vector2Byte operator -(Vector2Byte left, Vector2Byte right)
        {
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
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y;
        }

        public static bool operator !=(Vector2Byte? left, Vector2Byte? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

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
            return X.Equals(other.X) && Y.Equals(other.Y);
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
            return HashCode.Combine(X, Y);
        }
    }

    public class RootedVector2<T>(T x, T y) :
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

        public T X { get; set; } = x;
        public T Y { get; set; } = y;

        public RootedVector2() : this(x: T.Zero, y: T.Zero) { }
        public RootedVector2(T value) : this(x: value, y: value) { }

        public static RootedVector2<T> operator +(RootedVector2<T> left, RootedVector2<T> right)
        {
            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y
            };
        }

        public static RootedVector2<T> operator -(RootedVector2<T> left, RootedVector2<T> right)
        {
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
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y;
        }

        public static bool operator !=(RootedVector2<T>? left, RootedVector2<T>? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

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

        public T Magnitude()
        {
            T magnitudeSquared = Dot(this);
            return T.Sqrt(magnitudeSquared);
        }

        public virtual RootedVector2<T> Normalize()
        {
            T magnitude = Magnitude();
            return new()
            {
                X = X / magnitude,
                Y = Y / magnitude
            };
        }

        protected bool Equals(RootedVector2<T> other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
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
            return HashCode.Combine(X, Y);
        }
    }

    public class Vector2<T>(T x, T y) :
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

        public T X { get; set; } = x;
        public T Y { get; set; } = y;

        public Vector2() : this(x: T.Zero, y: T.Zero) { }
        public Vector2(T value) : this(x: value, y: value) { }

        public static Vector2<T> operator +(Vector2<T> left, Vector2<T> right)
        {
            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y
            };
        }

        public static Vector2<T> operator -(Vector2<T> left, Vector2<T> right)
        {
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
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y;
        }

        public static bool operator !=(Vector2<T>? left, Vector2<T>? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

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
            return X.Equals(other.X) && Y.Equals(other.Y);
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
            return HashCode.Combine(X, Y);
        }
    }
}
