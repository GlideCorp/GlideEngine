
using System.Numerics;

namespace Core.Maths.Vectors
{
    public class Vector3() : RootVector3<float>(),
        IAdditionOperators<Vector3, Vector3, Vector3>,
        ISubtractionOperators<Vector3, Vector3, Vector3>,
        IMultiplyOperators<Vector3, float, Vector3>,
        IDivisionOperators<Vector3, float, Vector3>,
        IComparisonOperators<Vector3, Vector3, bool>,
        IUnaryNegationOperators<Vector3, Vector3>
    {
        public static Vector3 Zero => new(value: 0);
        public static Vector3 One => new(value: 1);

        public static Vector3 UnitX => new(x: 1, y: 0, z: 0);
        public static Vector3 UnitY => new(x: 0, y: 1, z: 0);
        public static Vector3 UnitZ => new(x: 0, y: 0, z: 1);

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

        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z
            };
        }

        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z
            };
        }

        public static Vector3 operator *(Vector3 vector, float scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y,
                Z = scalar * vector.Z
            };
        }

        public static Vector3 operator *(float scalar, Vector3 vector) { return vector * scalar; }

        public static Vector3 operator /(Vector3 vector, float scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar,
                Z = vector.Z / scalar
            };
        }

        public static Vector3 operator /(float scalar, Vector3 vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y,
                Z = scalar / vector.Z
            };
        }

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

        public static bool operator >(Vector3 left, Vector3 right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector3 left, Vector3 right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector3 left, Vector3 right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector3 left, Vector3 right) { throw new InvalidOperationException(); }

        public static Vector3 operator -(Vector3 value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y,
                Z = -value.Z
            };
        }

        protected bool Equals(Vector3 other)
        {
            return Values.Equals(other.Values);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector3)obj);
        }

        public override int GetHashCode()
        {
            return Values.GetHashCode();
        }
    }

    public class Vector3Double() : RootVector3<double>(),
        IAdditionOperators<Vector3Double, Vector3Double, Vector3Double>,
        ISubtractionOperators<Vector3Double, Vector3Double, Vector3Double>,
        IMultiplyOperators<Vector3Double, double, Vector3Double>,
        IDivisionOperators<Vector3Double, double, Vector3Double>,
        IComparisonOperators<Vector3Double, Vector3Double, bool>,
        IUnaryNegationOperators<Vector3Double, Vector3Double>
    {
        public static Vector3Double Zero => new(value: 0);
        public static Vector3Double One => new(value: 1);

        public static Vector3Double UnitX => new(x: 1, y: 0, z: 0);
        public static Vector3Double UnitY => new(x: 0, y: 1, z: 0);
        public static Vector3Double UnitZ => new(x: 0, y: 0, z: 1);

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

        public static Vector3Double operator +(Vector3Double left, Vector3Double right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z
            };
        }

        public static Vector3Double operator -(Vector3Double left, Vector3Double right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

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
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(Vector3Double? left, Vector3Double? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

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

        protected bool Equals(Vector3Double other)
        {
            return Values.Equals(other.Values);
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
            return Values.GetHashCode();
        }
    }

    public class Vector3Int() : Vector3<int>(),
        IAdditionOperators<Vector3Int, Vector3Int, Vector3Int>,
        ISubtractionOperators<Vector3Int, Vector3Int, Vector3Int>,
        IMultiplyOperators<Vector3Int, int, Vector3Int>,
        IDivisionOperators<Vector3Int, int, Vector3Int>,
        IComparisonOperators<Vector3Int, Vector3Int, bool>,
        IUnaryNegationOperators<Vector3Int, Vector3Int>
    {
        public static Vector3Int Zero => new(value: 0);
        public static Vector3Int One => new(value: 1);

        public static Vector3Int UnitX => new(x: 1, y: 0, z: 0);
        public static Vector3Int UnitY => new(x: 0, y: 1, z: 0);
        public static Vector3Int UnitZ => new(x: 0, y: 0, z: 1);

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

        public static Vector3Int operator +(Vector3Int left, Vector3Int right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = (byte)(left.X + right.X),
                Y = (byte)(left.Y + right.Y),
                Z = (byte)(left.Z + right.Z)
            };
        }

        public static Vector3Int operator -(Vector3Int left, Vector3Int right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

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
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(Vector3Int? left, Vector3Int? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

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
            return Values.Equals(other.Values);
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
            return Values.GetHashCode();
        }
    }

    public class Vector3Byte() : Vector3<byte>(),
        IAdditionOperators<Vector3Byte, Vector3Byte, Vector3Byte>,
        ISubtractionOperators<Vector3Byte, Vector3Byte, Vector3Byte>,
        IMultiplyOperators<Vector3Byte, byte, Vector3Byte>,
        IDivisionOperators<Vector3Byte, byte, Vector3Byte>,
        IComparisonOperators<Vector3Byte, Vector3Byte, bool>,
        IUnaryNegationOperators<Vector3Byte, Vector3Byte>
    {
        public static Vector3Byte Zero => new(value: 0);
        public static Vector3Byte One => new(value: 1);
                      
        public static Vector3Byte UnitX => new(x: 1, y: 0, z: 0);
        public static Vector3Byte UnitY => new(x: 0, y: 1, z: 0);
        public static Vector3Byte UnitZ => new(x: 0, y: 0, z: 1);

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

        public static Vector3Byte operator +(Vector3Byte left, Vector3Byte right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return new()
            {
                X = (byte)(left.X + right.X),
                Y = (byte)(left.Y + right.Y),
                Z = (byte)(left.Z + right.Z)
            };
        }

        public static Vector3Byte operator -(Vector3Byte left, Vector3Byte right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

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
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }

        public static bool operator !=(Vector3Byte? left, Vector3Byte? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

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
            return Values.Equals(other.Values);
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
            return Values.GetHashCode();
        }
    }

    public class RootVector3<T>() : RootedVector<T>(size: 3),
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

            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z
            };
        }

        public static RootVector3<T> operator -(RootVector3<T> left, RootVector3<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

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

        public override T Magnitude()
        {
            T magnitudeSquared = Dot(this);
            return T.Sqrt(magnitudeSquared);
        }

        public override RootVector3<T> Normalize()
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

        public T Dot(RootVector3<T> other)
        {
            return X * other.X +
                   Y * other.Y +
                   Z * other.Z;
        }

        public Vector3<T> CrossProduct(RootVector3<T> other)
        {
            return new()
            {
                X = Y * other.Z - Z * other.Y,
                Y = Z * other.X - X * other.Z,
                Z = X * other.Y - Y * other.X
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

    public class Vector3<T>() : Vector<T>(size: 3),
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

            return new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z
            };
        }

        public static Vector3<T> operator -(Vector3<T> left, Vector3<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

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
