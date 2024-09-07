
using System;
using System.Numerics;

namespace Core.Maths.Vectors
{
    public struct Vector4Float(float x, float y, float z, float w) :
        IAdditionOperators<Vector4Float, Vector4Float, Vector4Float>,
        ISubtractionOperators<Vector4Float, Vector4Float, Vector4Float>,
        IMultiplyOperators<Vector4Float, float, Vector4Float>,
        IDivisionOperators<Vector4Float, float, Vector4Float>,
        IComparisonOperators<Vector4Float, Vector4Float, bool>,
        IUnaryNegationOperators<Vector4Float, Vector4Float>
    {
        public static Vector4Float Zero => new(value: 0);
        public static Vector4Float One => new(value: 1);

        public static Vector4Float UnitX => new(x: 1, y: 0, z: 0, w: 0);
        public static Vector4Float UnitY => new(x: 0, y: 1, z: 0, w: 0);
        public static Vector4Float UnitZ => new(x: 0, y: 0, z: 1, w: 0);
        public static Vector4Float UnitW => new(x: 0, y: 0, z: 0, w: 1);

        public float X { get; set; } = x;
        public float Y { get; set; } = y;
        public float Z { get; set; } = z;
        public float W { get; set; } = w;

        public Vector4Float() : this(0, 0, 0, 0) { }
        public Vector4Float(float value) : this(value, value, value, value) { }
        public Vector4Float(Vector3Float xyz, float w) : this(xyz.X, xyz.Y, xyz.Z, w) { }

        #region Arithmetic Operations
        public static Vector4Float operator +(Vector4Float left, Vector4Float right)
        {
            Vector4Float result = new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z,
                W = left.W + right.W
            };

            return result;
        }

        public static Vector4Float operator -(Vector4Float left, Vector4Float right)
        {
            Vector4Float result = new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z,
                W = left.W - right.W
            };

            return result;
        }

        public static Vector4Float operator *(Vector4Float vector, float scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y,
                Z = scalar * vector.Z,
                W = scalar * vector.W
            };
        }

        public static Vector4Float operator *(float scalar, Vector4Float vector) { return vector * scalar; }

        public static Vector4Float operator /(Vector4Float vector, float scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar,
                Z = vector.Z / scalar,
                W = vector.W / scalar
            };
        }

        public static Vector4Float operator /(float scalar, Vector4Float vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y,
                Z = scalar / vector.Z,
                W = scalar / vector.W
            };
        }
        public static Vector4Float operator -(Vector4Float value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y,
                Z = -value.Z,
                W = -value.W
            };
        }

        public static bool operator ==(Vector4Float left, Vector4Float right)
        {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z &&
                   left.W == right.W;
        }

        public static bool operator !=(Vector4Float left, Vector4Float right)
        {
            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z &&
                   left.W != right.W;
        }

        public static bool operator >(Vector4Float left, Vector4Float right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector4Float left, Vector4Float right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector4Float left, Vector4Float right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector4Float left, Vector4Float right) { throw new InvalidOperationException(); }
        #endregion

        #region Vector Operations
        public float Dot(Vector4Float other)
        {
            return X * other.X +
                   Y * other.Y +
                   Z * other.Z +
                   W * other.W;
        }

        public float Magnitude()
        {
            float magnitudeSquared = Dot(this);
            return MathF.Sqrt(magnitudeSquared);
        }

        public Vector4Float Normalize()
        {
            float magnitude = Magnitude();
            return new()
            {
               X = X/magnitude,
               Y = Y/magnitude,
               Z = Z/magnitude,
               W = W/magnitude
            };
        }
        #endregion

        private bool Equals(Vector4Float other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector4Float)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }
    }

    public struct Vector4Double(double x, double y, double z, double w) :
        IAdditionOperators<Vector4Double, Vector4Double, Vector4Double>,
        ISubtractionOperators<Vector4Double, Vector4Double, Vector4Double>,
        IMultiplyOperators<Vector4Double, double, Vector4Double>,
        IDivisionOperators<Vector4Double, double, Vector4Double>,
        IComparisonOperators<Vector4Double, Vector4Double, bool>,
        IUnaryNegationOperators<Vector4Double, Vector4Double>
    {
        public static Vector4Double Zero => new(value: 0);
        public static Vector4Double One => new(value: 1);

        public static Vector4Double UnitX => new(x: 1, y: 0, z: 0, w: 0);
        public static Vector4Double UnitY => new(x: 0, y: 1, z: 0, w: 0);
        public static Vector4Double UnitZ => new(x: 0, y: 0, z: 1, w: 0);
        public static Vector4Double UnitW => new(x: 0, y: 0, z: 0, w: 1);

        public double X { get; set; } = x;
        public double Y { get; set; } = y;
        public double Z { get; set; } = z;
        public double W { get; set; } = w;

        public Vector4Double() : this(0, 0, 0, 0) { }
        public Vector4Double(double value) : this(value, value, value, value) { }
        public Vector4Double(Vector3Double xyz, double w) : this(xyz.X, xyz.Y, xyz.Z, w) { }

        #region Arithmetic Operations
        public static Vector4Double operator +(Vector4Double left, Vector4Double right)
        {
            Vector4Double result = new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z,
                W = left.W + right.W
            };

            return result;
        }

        public static Vector4Double operator -(Vector4Double left, Vector4Double right)
        {
            Vector4Double result = new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z,
                W = left.W - right.W
            };

            return result;
        }

        public static Vector4Double operator *(Vector4Double vector, double scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y,
                Z = scalar * vector.Z,
                W = scalar * vector.W
            };
        }

        public static Vector4Double operator *(double scalar, Vector4Double vector) { return vector * scalar; }

        public static Vector4Double operator /(Vector4Double vector, double scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar,
                Z = vector.Z / scalar,
                W = vector.W / scalar
            };
        }

        public static Vector4Double operator /(double scalar, Vector4Double vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y,
                Z = scalar / vector.Z,
                W = scalar / vector.W
            };
        }
        public static Vector4Double operator -(Vector4Double value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y,
                Z = -value.Z,
                W = -value.W
            };
        }

        public static bool operator ==(Vector4Double left, Vector4Double right)
        {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z &&
                   left.W == right.W;
        }

        public static bool operator !=(Vector4Double left, Vector4Double right)
        {
            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z &&
                   left.W != right.W;
        }

        public static bool operator >(Vector4Double left, Vector4Double right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector4Double left, Vector4Double right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector4Double left, Vector4Double right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector4Double left, Vector4Double right) { throw new InvalidOperationException(); }
        #endregion

        #region Vector Operations
        public double Dot(Vector4Double other)
        {
            return X * other.X +
                   Y * other.Y +
                   Z * other.Z +
                   W * other.W;
        }

        public double Magnitude()
        {
            double magnitudeSquared = Dot(this);
            return Math.Sqrt(magnitudeSquared);
        }

        public Vector4Double Normalize()
        {
            double magnitude = Magnitude();
            return new()
            {
                X = X / magnitude,
                Y = Y / magnitude,
                Z = Z / magnitude,
                W = W / magnitude
            };
        }
        #endregion

        private bool Equals(Vector4Double other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector4Double)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }
    }

    public struct Vector4Int(int x, int y, int z, int w) :
        IAdditionOperators<Vector4Int, Vector4Int, Vector4Int>,
        ISubtractionOperators<Vector4Int, Vector4Int, Vector4Int>,
        IMultiplyOperators<Vector4Int, int, Vector4Int>,
        IDivisionOperators<Vector4Int, int, Vector4Int>,
        IComparisonOperators<Vector4Int, Vector4Int, bool>,
        IUnaryNegationOperators<Vector4Int, Vector4Int>
    {
        public static Vector4Int Zero => new(value: 0);
        public static Vector4Int One => new(value: 1);

        public static Vector4Int UnitX => new(x: 1, y: 0, z: 0, w: 0);
        public static Vector4Int UnitY => new(x: 0, y: 1, z: 0, w: 0);
        public static Vector4Int UnitZ => new(x: 0, y: 0, z: 1, w: 0);
        public static Vector4Int UnitW => new(x: 0, y: 0, z: 0, w: 1);

        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public int Z { get; set; } = z;
        public int W { get; set; } = w;

        public Vector4Int() : this(0, 0, 0, 0) { }
        public Vector4Int(int value) : this(value, value, value, value) { }
        public Vector4Int(Vector3Int xyz, int w) : this(xyz.X, xyz.Y, xyz.Z, w) { }

        #region Arithmetic Operations
        public static Vector4Int operator +(Vector4Int left, Vector4Int right)
        {
            Vector4Int result = new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z,
                W = left.W + right.W
            };

            return result;
        }

        public static Vector4Int operator -(Vector4Int left, Vector4Int right)
        {
            Vector4Int result = new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z,
                W = left.W - right.W
            };

            return result;
        }

        public static Vector4Int operator *(Vector4Int vector, int scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y,
                Z = scalar * vector.Z,
                W = scalar * vector.W
            };
        }

        public static Vector4Int operator *(int scalar, Vector4Int vector) { return vector * scalar; }

        public static Vector4Int operator /(Vector4Int vector, int scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar,
                Z = vector.Z / scalar,
                W = vector.W / scalar
            };
        }

        public static Vector4Int operator /(int scalar, Vector4Int vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y,
                Z = scalar / vector.Z,
                W = scalar / vector.W
            };
        }
        public static Vector4Int operator -(Vector4Int value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y,
                Z = -value.Z,
                W = -value.W
            };
        }

        public static bool operator ==(Vector4Int left, Vector4Int right)
        {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z &&
                   left.W == right.W;
        }

        public static bool operator !=(Vector4Int left, Vector4Int right)
        {
            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z &&
                   left.W != right.W;
        }

        public static bool operator >(Vector4Int left, Vector4Int right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector4Int left, Vector4Int right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector4Int left, Vector4Int right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector4Int left, Vector4Int right) { throw new InvalidOperationException(); }
        #endregion

        #region Vector Operations
        public int Dot(Vector4Int other)
        {
            return X * other.X +
                   Y * other.Y +
                   Z * other.Z +
                   W * other.W;
        }

        public float Magnitude()
        {
            int magnitudeSquared = Dot(this);
            return MathF.Sqrt(magnitudeSquared);
        }
        #endregion

        private bool Equals(Vector4Int other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector4Int)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }
    }

    public struct Vector4Byte(byte x, byte y, byte z, byte w) :
        IAdditionOperators<Vector4Byte, Vector4Byte, Vector4Byte>,
        ISubtractionOperators<Vector4Byte, Vector4Byte, Vector4Byte>,
        IMultiplyOperators<Vector4Byte, byte, Vector4Byte>,
        IDivisionOperators<Vector4Byte, byte, Vector4Byte>,
        IComparisonOperators<Vector4Byte, Vector4Byte, bool>,
        IUnaryNegationOperators<Vector4Byte, Vector4Byte>
    {
        public static Vector4Byte Zero => new(value: 0);
        public static Vector4Byte One => new(value: 1);

        public static Vector4Byte UnitX => new(x: 1, y: 0, z: 0, w: 0);
        public static Vector4Byte UnitY => new(x: 0, y: 1, z: 0, w: 0);
        public static Vector4Byte UnitZ => new(x: 0, y: 0, z: 1, w: 0);
        public static Vector4Byte UnitW => new(x: 0, y: 0, z: 0, w: 1);

        //Mi sa che questi dovrebbero essere sbyte... se mai dovesse diventare un problema dai ctrl+F byte -> sbyte
        public byte X { get; set; } = x;
        public byte Y { get; set; } = y;
        public byte Z { get; set; } = z;
        public byte W { get; set; } = w;

        public Vector4Byte() : this(0, 0, 0, 0) { }
        public Vector4Byte(byte value) : this(value, value, value, value) { }
        public Vector4Byte(Vector3Byte xyz, byte w) : this(xyz.X, xyz.Y, xyz.Z, w) { }

        #region Arithmetic Operations
        public static Vector4Byte operator +(Vector4Byte left, Vector4Byte right)
        {
            Vector4Byte result = new()
            {
                X = (byte)(left.X + right.X),
                Y = (byte)(left.Y + right.Y),
                Z = (byte)(left.Z + right.Z),
                W = (byte)(left.W + right.W)
            };

            return result;
        }

        public static Vector4Byte operator -(Vector4Byte left, Vector4Byte right)
        {
            Vector4Byte result = new()
            {
                X = (byte)(left.X - right.X),
                Y = (byte)(left.Y - right.Y),
                Z = (byte)(left.Z - right.Z),
                W = (byte)(left.W - right.W)
            };

            return result;
        }

        public static Vector4Byte operator *(Vector4Byte vector, byte scalar)
        {
            return new()
            {
                X = (byte)(scalar * vector.X),
                Y = (byte)(scalar * vector.Y),
                Z = (byte)(scalar * vector.Z),
                W = (byte)(scalar * vector.W)
            };
        }

        public static Vector4Byte operator *(byte scalar, Vector4Byte vector) { return vector * scalar; }

        public static Vector4Byte operator /(Vector4Byte vector, byte scalar)
        {
            return new()
            {
                X = (byte)(vector.X / scalar),
                Y = (byte)(vector.Y / scalar),
                Z = (byte)(vector.Z / scalar),
                W = (byte)(vector.W / scalar)
            };
        }

        public static Vector4Byte operator /(byte scalar, Vector4Byte vector)
        {
            return new()
            {
                X = (byte)(scalar / vector.X),
                Y = (byte)(scalar / vector.Y),
                Z = (byte)(scalar / vector.Z),
                W = (byte)(scalar / vector.W)
            };
        }
        public static Vector4Byte operator -(Vector4Byte value)
        {
            return new()
            {
                X = (byte)(-value.X),
                Y = (byte)(-value.Y),
                Z = (byte)(-value.Z),
                W = (byte)(-value.W)
            };
        }

        public static bool operator ==(Vector4Byte left, Vector4Byte right)
        {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z &&
                   left.W == right.W;
        }

        public static bool operator !=(Vector4Byte left, Vector4Byte right)
        {
            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z &&
                   left.W != right.W;
        }

        public static bool operator >(Vector4Byte left, Vector4Byte right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector4Byte left, Vector4Byte right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector4Byte left, Vector4Byte right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector4Byte left, Vector4Byte right) { throw new InvalidOperationException(); }
        #endregion

        #region Arithmetic Operations
        public byte Dot(Vector4Byte other)
        {
            return (byte)(X * other.X +
                           Y * other.Y +
                           Z * other.Z +
                           W * other.W);
        }

        public float Magnitude()
        {
            byte magnitudeSquared = Dot(this);
            return MathF.Sqrt(magnitudeSquared);
        }
        #endregion

        private bool Equals(Vector4Byte other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector4Byte)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }
    }

    /*
    public class RootedVector4<T>(T x, T y, T z, T w) :
        IAdditionOperators<RootedVector4<T>, RootedVector4<T>, RootedVector4<T>>,
        ISubtractionOperators<RootedVector4<T>, RootedVector4<T>, RootedVector4<T>>,
        IMultiplyOperators<RootedVector4<T>, T, RootedVector4<T>>,
        IDivisionOperators<RootedVector4<T>, T, RootedVector4<T>>,
        IComparisonOperators<RootedVector4<T>, RootedVector4<T>, bool>,
        IUnaryNegationOperators<RootedVector4<T>, RootedVector4<T>>
        where T : INumber<T>, IRootFunctions<T>
    {
        public static RootedVector4<T> Zero => new(value: T.Zero);
        public static RootedVector4<T> One => new(value: T.One);

        public static RootedVector4<T> UnitX => new(x: T.One, y: T.Zero, z: T.Zero, w: T.Zero);
        public static RootedVector4<T> UnitY => new(x: T.Zero, y: T.One, z: T.Zero, w: T.Zero);
        public static RootedVector4<T> UnitZ => new(x: T.Zero, y: T.Zero, z: T.One, w: T.Zero);
        public static RootedVector4<T> UnitW => new(x: T.Zero, y: T.Zero, z: T.Zero, w: T.One);

        public T X { get; set; } = x;
        public T Y { get; set; } = y;
        public T Z { get; set; } = z;
        public T W { get; set; } = w;


        public RootedVector4() : this(T.Zero, T.Zero, T.Zero, T.Zero) { }
        public RootedVector4(T value) : this(value, value, value, value) { }
        //public RootedVector4(Vector3<T> xyz, T w) : this(xyz.X, xyz.Y, xyz.Z, w) { }

        public static RootedVector4<T> operator +(RootedVector4<T> left, RootedVector4<T> right)
        {
            RootedVector4<T> result = new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z,
                W = left.W + right.W
            };

            return result;
        }

        public static RootedVector4<T> operator -(RootedVector4<T> left, RootedVector4<T> right)
        {
            RootedVector4<T> result = new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y,
                Z = left.Z - right.Z,
                W = left.W - right.W
            };

            return result;
        }

        public static RootedVector4<T> operator *(RootedVector4<T> vector, T scalar)
        {
            return new()
            {
                X = scalar * vector.X,
                Y = scalar * vector.Y,
                Z = scalar * vector.Z,
                W = scalar * vector.W
            };
        }

        public static RootedVector4<T> operator *(T scalar, RootedVector4<T> vector) { return vector * scalar; }

        public static RootedVector4<T> operator /(RootedVector4<T> vector, T scalar)
        {
            return new()
            {
                X = vector.X / scalar,
                Y = vector.Y / scalar,
                Z = vector.Z / scalar,
                W = vector.W / scalar
            };
        }

        public static RootedVector4<T> operator /(T scalar, RootedVector4<T> vector)
        {
            return new()
            {
                X = scalar / vector.X,
                Y = scalar / vector.Y,
                Z = scalar / vector.Z,
                W = scalar / vector.W
            };
        }

        public static bool operator ==(RootedVector4<T>? left, RootedVector4<T>? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z &&
                   left.W == right.W;
        }

        public static bool operator !=(RootedVector4<T>? left, RootedVector4<T>? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z &&
                   left.W != right.W;
        }

        public static bool operator >(RootedVector4<T> left, RootedVector4<T> right) { throw new InvalidOperationException(); }
        public static bool operator >=(RootedVector4<T> left, RootedVector4<T> right) { throw new InvalidOperationException(); }
        public static bool operator <(RootedVector4<T> left, RootedVector4<T> right) { throw new InvalidOperationException(); }
        public static bool operator <=(RootedVector4<T> left, RootedVector4<T> right) { throw new InvalidOperationException(); }

        public static RootedVector4<T> operator -(RootedVector4<T> value)
        {
            return new()
            {
                X = -value.X,
                Y = -value.Y,
                Z = -value.Z,
                W = -value.W
            };
        }

        public T Dot(RootedVector4<T> other)
        {
            return X * other.X +
                   Y * other.Y +
                   Z * other.Z +
                   W * other.W;
        }

        public T Magnitude()
        {
            T magnitudeSquared = Dot(this);
            return T.Sqrt(magnitudeSquared);
        }

        public RootedVector4<T> Normalize()
        {
            T magnitude = Magnitude();
            return new()
            {
               X = X/magnitude,
               Y = Y/magnitude,
               Z = Z/magnitude,
               W = W/magnitude
            };
        }
        protected bool Equals(RootedVector4<T> other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((RootedVector4<T>)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }
    }

    public class Vector4<T>(T x, T y, T z, T w) :
        IAdditionOperators<Vector4<T>, Vector4<T>, Vector4<T>>,
        ISubtractionOperators<Vector4<T>, Vector4<T>, Vector4<T>>,
        IMultiplyOperators<Vector4<T>, T, Vector4<T>>,
        IDivisionOperators<Vector4<T>, T, Vector4<T>>,
        IComparisonOperators<Vector4<T>, Vector4<T>, bool>,
        IUnaryNegationOperators<Vector4<T>, Vector4<T>>
        where T : INumber<T>
    {
        public static Vector4<T> Zero => new(value: T.Zero);
        public static Vector4<T> One => new(value: T.One);

        public static Vector4<T> UnitX => new(x: T.One, y: T.Zero, z: T.Zero, w: T.Zero);
        public static Vector4<T> UnitY => new(x: T.Zero, y: T.One, z: T.Zero, w: T.Zero);
        public static Vector4<T> UnitZ => new(x: T.Zero, y: T.Zero, z: T.One, w: T.Zero);
        public static Vector4<T> UnitW => new(x: T.Zero, y: T.Zero, z: T.Zero, w: T.One);

        public T X { get; set; } = x;
        public T Y { get; set; } = y;
        public T Z { get; set; } = z;
        public T W { get; set; } = w;


        public Vector4() : this(T.Zero, T.Zero, T.Zero, T.Zero) { }
        public Vector4(T value) : this(value, value, value, value) { }
        //public Vector4(Vector3<T> xyz, T w) : this(xyz.X, xyz.Y, xyz.Z, w) { }

        public static Vector4<T> operator +(Vector4<T> left, Vector4<T> right)
        {
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

        public static bool operator ==(Vector4<T>? left, Vector4<T>? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z &&
                   left.W == right.W;
        }

        public static bool operator !=(Vector4<T>? left, Vector4<T>? right)
        {
            if (left is null || right is null) { throw new InvalidOperationException(); }

            return left.X != right.X &&
                   left.Y != right.Y &&
                   left.Z != right.Z &&
                   left.W != right.W;
        }

        public static bool operator >(Vector4<T> left, Vector4<T> right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector4<T> left, Vector4<T> right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector4<T> left, Vector4<T> right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector4<T> left, Vector4<T> right) { throw new InvalidOperationException(); }

        public T Dot(Vector4<T> other)
        {
            return X * other.X +
                   Y * other.Y +
                   Z * other.Z +
                   W * other.W;
        }

        protected bool Equals(Vector4<T> other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector4<T>)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }
    }
    */
}