
#pragma warning disable CA2260
namespace Core.Maths.Vectors
{
    public class Vector2 : RootedVector2<float> { }
    public class Vector2Double : RootedVector2<double> { }
    public class Vector2Int : Vector2<int> { }
    public class Vector2Byte : Vector2<byte> { }

    public class Vector3 : RootVector3<float> { }
    public class Vector3Double : RootVector3<double> { }
    public class Vector3Int : Vector3<int> { }
    public class Vector3Byte : Vector3<byte> { }

    public class Vector4 : RootVector4<float> { }
    public class Vector4Double : RootVector4<double> { }
    public class Vector4Int : Vector4<int> { }
    public class Vector4Byte : Vector4<byte> { }
}
