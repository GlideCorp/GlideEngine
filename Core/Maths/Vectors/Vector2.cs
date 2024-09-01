﻿
using System.Numerics;

namespace Core.Maths.Vectors
{
    public class Vector2<T>() :
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

        public T[] Values { get; set; } = new T[2];

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

            Vector2<T> result = new()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y
            };

            return result;
        }

        public static Vector2<T> operator -(Vector2<T> left, Vector2<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            Vector2<T> result = new()
            {
                X = left.X - right.X,
                Y = left.Y - right.Y
            };

            return result;
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