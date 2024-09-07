
using Core.Maths.Vectors;
using System;
using System.Numerics;

namespace Core.Maths.Vectors
{
    public class Vector3Float : RootVector3<float>,
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

        public Vector3Float()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public Vector3Float(float value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3Float(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

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
            if (left is null || right is null ) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(Vector3Float? left, Vector3Float? right)
        {
            if (left is null || right is null ) { throw new InvalidOperationException(); }

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

    public class Vector3Double : RootVector3<double>,
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
        public Vector3Double()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public Vector3Double(double value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3Double(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

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
            if (left is null || right is null ) { throw new InvalidOperationException(); }

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

    public class Vector3Int : Vector3<int>,
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

        public Vector3Int()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public Vector3Int(int value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3Int(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

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
            if (left is null || right is null ) { throw new InvalidOperationException(); }

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

    public class Vector3Byte : Vector3<byte>,
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

        public Vector3Byte()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public Vector3Byte(byte value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3Byte(byte x, byte y, byte z)
        {
            X = x;
            Y = y;
            Z = z;
        }

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
            if (left is null || right is null ) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(Vector3Byte? left, Vector3Byte? right)
        {
            if (left is null || right is null ) { throw new InvalidOperationException(); }

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

    public class RootVector3<T>:
        IAdditionOperators<RootVector3<T>, RootVector3<T>, RootVector3<T>>,
        ISubtractionOperators<RootVector3<T>, RootVector3<T>, RootVector3<T>>,
        IMultiplyOperators<RootVector3<T>, T, RootVector3<T>>,
        IDivisionOperators<RootVector3<T>, T, RootVector3<T>>,
        IComparisonOperators<RootVector3<T>, RootVector3<T>, bool>,
        IUnaryNegationOperators<RootVector3<T>, RootVector3<T>>
        where T : INumber<T>, IRootFunctions<T>
    {
        public static RootVector3<T> Zero => new(value: T.Zero);
        public static RootVector3<T> One => new(value: T.One);

        public static RootVector3<T> UnitX => new(x: T.One, y: T.Zero, z: T.Zero);
        public static RootVector3<T> UnitY => new(x: T.Zero, y: T.One, z: T.Zero);
        public static RootVector3<T> UnitZ => new(x: T.Zero, y: T.Zero, z: T.One);


        public T X { get; set; }

        public T Y { get; set; }

        public T Z { get; set; }

        public RootVector3()
        {
            X = T.Zero;
            Y = T.Zero;
            Z = T.Zero;
        }

        public RootVector3(T value)
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
            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z
            };
        }

        public static RootVector3<T> operator -(RootVector3<T> left, RootVector3<T> right)
        {
            return new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z
            };
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
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(RootVector3<T>? left, RootVector3<T>? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

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

        public virtual RootVector3<T> Normalize()
        {
            T magnitude = Magnitude();
            return new()
            {
                X = X/magnitude,
                Y = Y/magnitude, 
                Z = Z/magnitude
            };
        }

        public virtual T Dot(RootVector3<T> other)
        {
            return X * other.X +
                   Y * other.Y +
                   Z * other.Z;
        }

        public virtual RootVector3<T> CrossProduct(RootVector3<T> other)
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

    public class Vector3<T>:
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


        public T X { get; set; }

        public T Y { get; set; }

        public T Z { get; set; }

        public Vector3()
        {
            X = T.Zero;
            Y = T.Zero;
            Z = T.Zero;
        }

        public Vector3(T value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3(Vector2<T> xy, T z)
        {
            X = xy.X;
            Y = xy.Y;
            Z = z;
        }

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
