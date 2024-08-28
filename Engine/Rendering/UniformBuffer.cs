using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Engine.Rendering
{
    internal class UniformBuffer
    {
        public uint BufferID { get; private set; }

        internal unsafe UniformBuffer(int size, uint bindingPoint)
        {
            GL gl = Application.Context;
            BufferID = gl.CreateBuffer();
            //gl.BindBuffer(BufferTargetARB.UniformBuffer, BufferID);
            gl.NamedBufferData(BufferID, (nuint)size, null, VertexBufferObjectUsage.DynamicDraw);
            gl.BindBufferBase(BufferTargetARB.UniformBuffer, bindingPoint, BufferID);
            //gl.BindBuffer(BufferTargetARB.UniformBuffer, 0);
        }

        internal void Bind()
        {
            Application.Context.BindBuffer(BufferTargetARB.UniformBuffer, BufferID);
        }
        internal void Unbind()
        {
            Application.Context.BindBuffer(BufferTargetARB.UniformBuffer, 0);
        }

        internal void SetMatrix4(int offset, Matrix4X4<float> matrix)
        {
            Application.Context.NamedBufferSubData<Matrix4X4<float>>(BufferID, offset, 16*4, ref matrix);
        }
        internal void SetVec4(int offset, Vector4D<float> vector)
        {
            Application.Context.NamedBufferSubData<Vector4D<float>>(BufferID, offset, 16 , ref vector);
        }
    }
}
