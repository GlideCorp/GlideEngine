
using System;
using System.Numerics;

namespace Core.Maths.Vectors
{
    public class Vector3Float(float x, float y, float z) : RootedVector3<float>(x, y, z),
        IAdditionOperators<Vector3Float, Vector3Float, Vector3Float>,
        ISubtractionOperators<Vector3Float, Vector3Float, Vector3Float>,
        IMultiplyOperators<Vector3Float, float, Vector3Float>,
        IDivisionOperators<Vector3Float, float, Vector3Float>,
        IComparisonOperators<Vector3Float, Vector3Float, bool>,
        IUnaryNegationOperators<Vector3Float, Vector3Float>
    {
        public new static Vector3Float Zero => new(value: 0);
        public new static Vector3Float One => new(value: 1);

        public new static Vector3Float UnitX => new(x: 1, y: 0, z: 0);
        public new static Vector3Float UnitY => new(x: 0, y: 1, z: 0);
        public new static Vector3Float UnitZ => new(x: 0, y: 0, z: 1);

        public Vector3Float() : this(x: 0, y: 0, z: 0) { }
        public Vector3Float(float value) : this(x: value, y: value, z: value) { }
        public Vector3Float(Vector3Float xy, float z) : this(x: xy.X, y: xy.Y, z: z) { }

        public static Vector3Float operator +(Vector3Float left, Vector3Float right)
        {
            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z
            };
        }

        public static Vector3Float operator -(Vector3Float left, Vector3Float right)
        {
            return new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z
            };
        }

        public static Vector3Float operator *(Vector3Float vector, float scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y,
                Z = scalar * vector.Z
            };
        }

        public static Vector3Float operator *(float scalar, Vector3Float vector) { return vector * scalar; }

        public static Vector3Float operator /(Vector3Float vector, float scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar,
                Z = vector.Z / scalar
            };
        }

        public static Vector3Float operator /(float scalar, Vector3Float vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y,
                Z = scalar / vector.Z
            };
        }

        public static bool operator ==(Vector3Float? left, Vector3Float? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(Vector3Float? left, Vector3Float? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z;
        }

        public static bool operator >(Vector3Float left, Vector3Float right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector3Float left, Vector3Float right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector3Float left, Vector3Float right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector3Float left, Vector3Float right) { throw new InvalidOperationException(); }

        public static Vector3Float operator -(Vector3Float value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y,
                Z = -value.Z
            };
        }

        public override Vector3Float Normalize()
        {
            float magnitude = Magnitude();
            return new()
            {
                X = X / magnitude,
                Y = Y / magnitude,
                Z = Z / magnitude
            };
        }

        public Vector3Float CrossProduct(Vector3Float other)
        {
            return new()
            {
                X = Y * other.Z - Z * other.Y,
                Y = Z * other.X - X * other.Z,
                Z = X * other.Y - Y * other.X
            };
        }


        protected bool Equals(Vector3Float other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector3Float)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }

    public class Vector3Double(double x, double y, double z) : RootedVector3<double>(x, y, z),
        IAdditionOperators<Vector3Double, Vector3Double, Vector3Double>,
        ISubtractionOperators<Vector3Double, Vector3Double, Vector3Double>,
        IMultiplyOperators<Vector3Double, double, Vector3Double>,
        IDivisionOperators<Vector3Double, double, Vector3Double>,
        IComparisonOperators<Vector3Double, Vector3Double, bool>,
        IUnaryNegationOperators<Vector3Double, Vector3Double>
    {
        public new static Vector3Double Zero => new(value: 0);
        public new static Vector3Double One => new(value: 1);

        public new static Vector3Double UnitX => new(x: 1, y: 0, z: 0);
        public new static Vector3Double UnitY => new(x: 0, y: 1, z: 0);
        public new static Vector3Double UnitZ => new(x: 0, y: 0, z: 1);

        public Vector3Double() : this(x: 0, y: 0, z: 0) { }
        public Vector3Double(double value) : this(x: value, y: value, z: value) { }
        public Vector3Double(Vector3Double xy, double z) : this(x: xy.X, y: xy.Y, z: z) { }

        public static Vector3Double operator +(Vector3Double left, Vector3Double right)
        {
            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z
            };
        }

        public static Vector3Double operator -(Vector3Double left, Vector3Double right)
        {
            return new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z
            };
        }

        public static Vector3Double operator *(Vector3Double vector, double scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y,
                Z = scalar * vector.Z
            };
        }

        public static Vector3Double operator *(double scalar, Vector3Double vector) { return vector * scalar; }

        public static Vector3Double operator /(Vector3Double vector, double scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar,
                Z = vector.Z / scalar
            };
        }

        public static Vector3Double operator /(double scalar, Vector3Double vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y,
                Z = scalar / vector.Z
            };
        }

        public static bool operator ==(Vector3Double? left, Vector3Double? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(Vector3Double? left, Vector3Double? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z;
        }

        public static bool operator >(Vector3Double left, Vector3Double right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector3Double left, Vector3Double right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector3Double left, Vector3Double right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector3Double left, Vector3Double right) { throw new InvalidOperationException(); }

        public static Vector3Double operator -(Vector3Double value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y,
                Z = -value.Z
            };
        }

        public override Vector3Double Normalize()
        {
            double magnitude = Magnitude();
            return new()
            {
                X = X / magnitude,
                Y = Y / magnitude,
                Z = Z / magnitude
            };
        }

        public Vector3Double CrossProduct(Vector3Double other)
        {
            return new()
            {
                X = Y * other.Z - Z * other.Y,
                Y = Z * other.X - X * other.Z,
                Z = X * other.Y - Y * other.X
            };
        }

        protected bool Equals(Vector3Double other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector3Double)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }

    public class Vector3Int(int x, int y, int z) : Vector3<int>(x, y, z),
        IAdditionOperators<Vector3Int, Vector3Int, Vector3Int>,
        ISubtractionOperators<Vector3Int, Vector3Int, Vector3Int>,
        IMultiplyOperators<Vector3Int, int, Vector3Int>,
        IDivisionOperators<Vector3Int, int, Vector3Int>,
        IComparisonOperators<Vector3Int, Vector3Int, bool>,
        IUnaryNegationOperators<Vector3Int, Vector3Int>
    {
        public new static Vector3Int Zero => new(value: 0);
        public new static Vector3Int One => new(value: 1);

        public new static Vector3Int UnitX => new(x: 1, y: 0, z: 0);
        public new static Vector3Int UnitY => new(x: 0, y: 1, z: 0);
        public new static Vector3Int UnitZ => new(x: 0, y: 0, z: 1);

        public Vector3Int() : this(x: 0, y: 0, z: 0) { }
        public Vector3Int(int value) : this(x: value, y: value, z: value) { }
        public Vector3Int(Vector3Int xy, int z) : this(x: xy.X, y: xy.Y, z: z) { }

        public static Vector3Int operator +(Vector3Int left, Vector3Int right)
        {

            return new()
            {
                X = (byte)(left.X + right.X),
                Y = (byte)(left.Y + right.Y),
                Z = (byte)(left.Z + right.Z)
            };
        }

        public static Vector3Int operator -(Vector3Int left, Vector3Int right)
        {

            return new()
            {
                X = (byte)(left.X - right.X),
                Y = (byte)(left.Y - right.Y),
                Z = (byte)(left.Z - right.Z)
            };
        }

        public static Vector3Int operator *(Vector3Int vector, int scalar)
        {
            return new()
            {
                X = (byte)(scalar * vector.X),
                Y = (byte)(scalar * vector.Y),
                Z = (byte)(scalar * vector.Z)
            };
        }

        public static Vector3Int operator *(int scalar, Vector3Int vector) { return vector * scalar; }

        public static Vector3Int operator /(Vector3Int vector, int scalar)
        {
            return new()
            {
                X = (byte)(vector.X / scalar),
                Y = (byte)(vector.Y / scalar),
                Z = (byte)(vector.Z / scalar)
            };
        }

        public static Vector3Byte operator /(int scalar, Vector3Int vector)
        {
            return new()
            {
                X = (byte)(scalar / vector.X),
                Y = (byte)(scalar / vector.Y),
                Z = (byte)(scalar / vector.Z)
            };
        }

        public static bool operator ==(Vector3Int? left, Vector3Int? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(Vector3Int? left, Vector3Int? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z;
        }

        public static bool operator >(Vector3Int left, Vector3Int right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector3Int left, Vector3Int right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector3Int left, Vector3Int right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector3Int left, Vector3Int right) { throw new InvalidOperationException(); }

        public static Vector3Int operator -(Vector3Int value)
        {
            return new()
            {
                X = (byte)-value.X,
                Y = (byte)-value.Y,
                Z = (byte)-value.Z
            };
        }

        protected bool Equals(Vector3Int other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector3Int)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }

    public class Vector3Byte(byte x, byte y, byte z) : Vector3<byte>(x, y, z),
        IAdditionOperators<Vector3Byte, Vector3Byte, Vector3Byte>,
        ISubtractionOperators<Vector3Byte, Vector3Byte, Vector3Byte>,
        IMultiplyOperators<Vector3Byte, byte, Vector3Byte>,
        IDivisionOperators<Vector3Byte, byte, Vector3Byte>,
        IComparisonOperators<Vector3Byte, Vector3Byte, bool>,
        IUnaryNegationOperators<Vector3Byte, Vector3Byte>
    {
        public new static Vector3Byte Zero => new(value: 0);
        public new static Vector3Byte One => new(value: 1);

        public new static Vector3Byte UnitX => new(x: 1, y: 0, z: 0);
        public new static Vector3Byte UnitY => new(x: 0, y: 1, z: 0);
        public new static Vector3Byte UnitZ => new(x: 0, y: 0, z: 1);

        public Vector3Byte() : this(x: 0, y: 0, z: 0) { }
        public Vector3Byte(byte value) : this(x: value, y: value, z: value) { }
        public Vector3Byte(Vector3Byte xy, byte z) : this(x: xy.X, y: xy.Y, z: z) { }

        public static Vector3Byte operator +(Vector3Byte left, Vector3Byte right)
        {
            return new()
            {
                X = (byte)(left.X + right.X),
                Y = (byte)(left.Y + right.Y),
                Z = (byte)(left.Z + right.Z)
            };
        }

        public static Vector3Byte operator -(Vector3Byte left, Vector3Byte right)
        {
            return new()
            {
                X = (byte)(left.X - right.X),
                Y = (byte)(left.Y - right.Y),
                Z = (byte)(left.Z - right.Z)
            };
        }

        public static Vector3Byte operator *(Vector3Byte vector, byte scalar)
        {
            return new()
            {
                X = (byte)(scalar * vector.X),
                Y = (byte)(scalar * vector.Y),
                Z = (byte)(scalar * vector.Z)
            };
        }

        public static Vector3Byte operator *(byte scalar, Vector3Byte vector) { return vector * scalar; }

        public static Vector3Byte operator /(Vector3Byte vector, byte scalar)
        {
            return new()
            {
                X = (byte)(vector.X / scalar),
                Y = (byte)(vector.Y / scalar),
                Z = (byte)(vector.Z / scalar)
            };
        }

        public static Vector3Byte operator /(byte scalar, Vector3Byte vector)
        {
            return new()
            {
                X = (byte)(scalar / vector.X),
                Y = (byte)(scalar / vector.Y),
                Z = (byte)(scalar / vector.Z)
            };
        }

        public static bool operator ==(Vector3Byte? left, Vector3Byte? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(Vector3Byte? left, Vector3Byte? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z;
        }

        public static bool operator >(Vector3Byte left, Vector3Byte right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector3Byte left, Vector3Byte right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector3Byte left, Vector3Byte right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector3Byte left, Vector3Byte right) { throw new InvalidOperationException(); }

        public static Vector3Byte operator -(Vector3Byte value)
        {
            return new()
            {
                X = (byte)-value.X,
                Y = (byte)-value.Y,
                Z = (byte)-value.Z
            };
        }

        protected bool Equals(Vector3Byte other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector3Byte)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }

    public class RootedVector3<T>(T x, T y, T z) :
        IAdditionOperators<RootedVector3<T>, RootedVector3<T>, RootedVector3<T>>,
        ISubtractionOperators<RootedVector3<T>, RootedVector3<T>, RootedVector3<T>>,
        IMultiplyOperators<RootedVector3<T>, T, RootedVector3<T>>,
        IDivisionOperators<RootedVector3<T>, T, RootedVector3<T>>,
        IComparisonOperators<RootedVector3<T>, RootedVector3<T>, bool>,
        IUnaryNegationOperators<RootedVector3<T>, RootedVector3<T>>
        where T : INumber<T>, IRootFunctions<T>
    {
        public static RootedVector3<T> Zero => new(value: T.Zero);
        public static RootedVector3<T> One => new(value: T.One);

        public static RootedVector3<T> UnitX => new(x: T.One, y: T.Zero, z: T.Zero);
        public static RootedVector3<T> UnitY => new(x: T.Zero, y: T.One, z: T.Zero);
        public static RootedVector3<T> UnitZ => new(x: T.Zero, y: T.Zero, z: T.One);

        public T X { get; set; } = x;
        public T Y { get; set; } = y;
        public T Z { get; set; } = z;

        public RootedVector3() : this(x: T.Zero, y: T.Zero, z: T.Zero) { }
        public RootedVector3(T value) : this(x: value, y: value, z: value) { }
        public RootedVector3(RootedVector2<T> xy, T z) : this(x: xy.X, y: xy.Y, z: z) { }

        public static RootedVector3<T> operator +(RootedVector3<T> left, RootedVector3<T> right)
        {
            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z
            };
        }

        public static RootedVector3<T> operator -(RootedVector3<T> left, RootedVector3<T> right)
        {
            return new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z
            };
        }

        public static RootedVector3<T> operator *(RootedVector3<T> vector, T scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y,
                Z = scalar * vector.Z
            };
        }

        public static RootedVector3<T> operator *(T scalar, RootedVector3<T> vector) { return vector * scalar; }

        public static RootedVector3<T> operator /(RootedVector3<T> vector, T scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar,
                Z = vector.Z / scalar
            };
        }

        public static RootedVector3<T> operator /(T scalar, RootedVector3<T> vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y,
                Z = scalar / vector.Z
            };
        }

        public static bool operator ==(RootedVector3<T>? left, RootedVector3<T>? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(RootedVector3<T>? left, RootedVector3<T>? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z;
        }

        public static bool operator >(RootedVector3<T> left, RootedVector3<T> right) { throw new InvalidOperationException(); }
        public static bool operator >=(RootedVector3<T> left, RootedVector3<T> right) { throw new InvalidOperationException(); }
        public static bool operator <(RootedVector3<T> left, RootedVector3<T> right) { throw new InvalidOperationException(); }
        public static bool operator <=(RootedVector3<T> left, RootedVector3<T> right) { throw new InvalidOperationException(); }

        public static RootedVector3<T> operator -(RootedVector3<T> value)
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

        public virtual RootedVector3<T> Normalize()
        {
            T magnitude = Magnitude();
            return new()
            {
                X = X / magnitude,
                Y = Y / magnitude,
                Z = Z / magnitude
            };
        }

        public virtual T Dot(RootedVector3<T> other)
        {
            return X * other.X +
                   Y * other.Y +
                   Z * other.Z;
        }

        public virtual RootedVector3<T> CrossProduct(RootedVector3<T> other)
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
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
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
            return HashCode.Combine(X, Y, Z);
        }
    }

    public class Vector3<T>(T x, T y, T z) :
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

        public T X { get; set; } = x;
        public T Y { get; set; } = y;
        public T Z { get; set; } = z;

        public Vector3() : this(T.Zero, T.Zero, T.Zero) { }
        public Vector3(T value) : this(value, value, value) { }
        public Vector3(Vector2<T> xy, T z) : this(xy.X, xy.Y, z) { }

        public static Vector3<T> operator +(Vector3<T> left, Vector3<T> right)
        {
            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z
            };
        }

        public static Vector3<T> operator -(Vector3<T> left, Vector3<T> right)
        {
            return new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z
            };
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

        public static Vector3<T> operator -(Vector3<T> value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y,
                Z = -value.Z
            };
        }

        public static bool operator ==(Vector3<T>? left, Vector3<T>? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(Vector3<T>? left, Vector3<T>? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z;
        }

        public static bool operator >(Vector3<T> left, Vector3<T> right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector3<T> left, Vector3<T> right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector3<T> left, Vector3<T> right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector3<T> left, Vector3<T> right) { throw new InvalidOperationException(); }


        public virtual T Dot(Vector3<T> other)
        {
            return X * other.X +
                   Y * other.Y +
                   Z * other.Z;
        }

        public virtual Vector3<T> CrossProduct(Vector3<T> other)
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
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
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
            return HashCode.Combine(X, Y, Z);
        }
    }
}
