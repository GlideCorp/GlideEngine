
using System.Numerics;

namespace Core.Maths.Vectors
{
    public class RootedVector<T>(params T[] values) : Vector<T>(values)
        where T : INumber<T>, IRootFunctions<T>
    {
        public RootedVector(int size) : this() { }

        public T Magnitude()
        {
            T magnitudeSquared = Dot(this);
            return T.Sqrt(magnitudeSquared);
        }

        public virtual RootedVector<T> Normalize()
        {
            int size = Values.Length;
            T magnitude = Magnitude();
            RootedVector<T> result = new(size);

            for (int i = 0; i < size; i++) { result.Values[i] = Values[i] / magnitude; }

            return result;
        }
    }

    public class Vector<T>(params T[] values) :
        IAdditionOperators<Vector<T>, Vector<T>, Vector<T>>,
        ISubtractionOperators<Vector<T>, Vector<T>, Vector<T>>,
        IMultiplyOperators<Vector<T>, T, Vector<T>>,
        IDivisionOperators<Vector<T>, T, Vector<T>>,
        IComparisonOperators<Vector<T>, Vector<T>, bool>,
        IUnaryNegationOperators<Vector<T>, Vector<T>>
        where T : INumber<T>
    {
        public T[] Values { get; set; } = values;

        public Vector(int size) : this(size == 0 ? [] : new T[size]) { }

        public static Vector<T> operator +(Vector<T> left, Vector<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            int size = left.Values.Length;
            int numberOfOperations = System.Numerics.Vector<T>.Count;
            int remaining = size % numberOfOperations;
            Vector<T> result = new(size);

            Span<T> resultSpan = result.Values.AsSpan();
            ReadOnlySpan<T> leftSpan = left.Values.AsSpan();
            ReadOnlySpan<T> rightSpan = right.Values.AsSpan();

            for (int i = 0; i < size - remaining; i += numberOfOperations)
            {
                var v1 = new System.Numerics.Vector<T>(leftSpan.Slice(i, numberOfOperations));
                var v2 = new System.Numerics.Vector<T>(rightSpan.Slice(i, numberOfOperations));
                (v1 + v2).CopyTo(resultSpan.Slice(i, numberOfOperations));
            }

            for (int i = size - remaining; i < size; i++) { result.Values[i] = left.Values[i] + right.Values[i]; }

            return result;
        }

        public static Vector<T> operator -(Vector<T> left, Vector<T> right)
        {
            if (left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            int size = left.Values.Length;
            int numberOfOperations = System.Numerics.Vector<T>.Count;
            int remaining = size % numberOfOperations;
            Vector<T> result = new(size);

            Span<T> resultSpan = result.Values.AsSpan();
            ReadOnlySpan<T> leftSpan = left.Values.AsSpan();
            ReadOnlySpan<T> rightSpan = right.Values.AsSpan();

            for (int i = 0; i < size - remaining; i += numberOfOperations)
            {
                var v1 = new System.Numerics.Vector<T>(leftSpan.Slice(i, numberOfOperations));
                var v2 = new System.Numerics.Vector<T>(rightSpan.Slice(i, numberOfOperations));
                (v1 - v2).CopyTo(resultSpan.Slice(i, numberOfOperations));
            }

            for (int i = size - remaining; i < size; i++) { result.Values[i] = left.Values[i] - right.Values[i]; }

            return result;
        }

        public static Vector<T> operator *(Vector<T> vector, T scalar)
        {
            int size = vector.Values.Length;
            int numberOfOperations = System.Numerics.Vector<T>.Count;
            int remaining = size % numberOfOperations;
            Vector<T> result = new(size);

            Span<T> resultSpan = result.Values.AsSpan();
            ReadOnlySpan<T> leftSpan = vector.Values.AsSpan();

            for (int i = 0; i < size - remaining; i += numberOfOperations)
            {
                var v1 = new System.Numerics.Vector<T>(leftSpan.Slice(i, numberOfOperations));
                (v1 * scalar).CopyTo(resultSpan.Slice(i, numberOfOperations));
            }

            for (int i = size - remaining; i < size; i++) { result.Values[i] = vector.Values[i] * scalar; }

            return result;
        }

        public static Vector<T> operator *(T scalar, Vector<T> vector) { return vector * scalar; }

        public static Vector<T> operator /(Vector<T> vector, T scalar)
        {
            int size = vector.Values.Length;
            int numberOfOperations = System.Numerics.Vector<T>.Count;
            int remaining = size % numberOfOperations;
            Vector<T> result = new(size);

            Span<T> resultSpan = result.Values.AsSpan();
            ReadOnlySpan<T> leftSpan = vector.Values.AsSpan();

            for (int i = 0; i < size - remaining; i += numberOfOperations)
            {
                var v1 = new System.Numerics.Vector<T>(leftSpan.Slice(i, numberOfOperations));
                (v1 / scalar).CopyTo(resultSpan.Slice(i, numberOfOperations));
            }

            for (int i = size - remaining; i < size; i++) { result.Values[i] = vector.Values[i] / scalar; }

            return result;
        }

        public static Vector<T> operator /(T scalar, Vector<T> vector)
        {
            int size = vector.Values.Length;
            int numberOfOperations = System.Numerics.Vector<T>.Count;
            int remaining = size % numberOfOperations;
            Vector<T> result = new(size);

            Span<T> resultSpan = result.Values.AsSpan();
            ReadOnlySpan<T> leftSpan = vector.Values.AsSpan();

            for (int i = 0; i < size - remaining; i += numberOfOperations)
            {
                var v1 = new System.Numerics.Vector<T>(leftSpan.Slice(i, numberOfOperations));
                (v1 / scalar).CopyTo(resultSpan.Slice(i, numberOfOperations));
            }

            for (int i = size - remaining; i < size; i++) { result.Values[i] = scalar / vector.Values[i]; }

            return result;
        }

        public static bool operator ==(Vector<T>? left, Vector<T>? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            bool result = false;
            ReadOnlySpan<T> leftSpan = left.Values.AsSpan();
            ReadOnlySpan<T> rightSpan = right.Values.AsSpan();

            for (int i = 0; !result && i < left.Values.Length; i++) { result = leftSpan[i] == rightSpan[i]; }

            return result;
        }

        public static bool operator !=(Vector<T>? left, Vector<T>? right)
        {
            if (left is null || right is null ||
                left.Values.Length != right.Values.Length) { throw new InvalidOperationException(); }

            bool result = false;
            ReadOnlySpan<T> leftSpan = left.Values.AsSpan();
            ReadOnlySpan<T> rightSpan = right.Values.AsSpan();

            for (int i = 0; !result && i < left.Values.Length; i++) { result = leftSpan[i] != rightSpan[i]; }

            return result;
        }

        public static bool operator >(Vector<T> left, Vector<T> right) { throw new InvalidOperationException(); }
        public static bool operator >=(Vector<T> left, Vector<T> right) { throw new InvalidOperationException(); }
        public static bool operator <(Vector<T> left, Vector<T> right) { throw new InvalidOperationException(); }
        public static bool operator <=(Vector<T> left, Vector<T> right) { throw new InvalidOperationException(); }

        public static Vector<T> operator -(Vector<T> value)
        {
            Vector<T> result = new(value.Values.Length);
            ReadOnlySpan<T> valueSpan = value.Values.AsSpan();

            for (int i = 0; i < valueSpan.Length; i++) { result.Values[i] = -valueSpan[i]; }
            return result;
        }

        public T Dot(Vector<T> other)
        {
            if (Values.Length != other.Values.Length) { throw new InvalidOperationException(); }

            int size = Values.Length;
            T result = default!;

            ReadOnlySpan<T> leftSpan = Values.AsSpan();
            ReadOnlySpan<T> rightSpan = other.Values.AsSpan();

            for (int i = 0; i < size; i++) { result += leftSpan[i] * rightSpan[i]; }

            return result;
        }
        
        protected bool Equals(Vector<T> other)
        {
            return Values.Equals(other.Values);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Vector<T>)obj);
        }

        public override int GetHashCode()
        {
            return Values.GetHashCode();
        }
    }
}
