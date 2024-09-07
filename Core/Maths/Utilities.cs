

using System.Numerics;

namespace Core.Maths
{
    public static class Utilities
    {
        public static T Module<T>(T x, T m)
            where T : INumber<T>, IModulusOperators<T, T, T>, IComparisonOperators<T, T, bool>
        {
            T r = x % m;
            if (r < T.Zero) { return r + m; }
            else { return r; }
        }
    }
}