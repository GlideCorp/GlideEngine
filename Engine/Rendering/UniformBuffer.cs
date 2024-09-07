using Silk.NET.OpenGL;
using Core.Maths.Vectors;
using Core.Maths.Matrices;

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

        internal void SetMatrix4(int offset, Matrix4x4 matrix)
        {
            Application.Context.NamedBufferSubData(BufferID, offset, 16*4, ref matrix);
        }
        internal void SetVec4(int offset, Vector4Float vector)
        {
            Application.Context.NamedBufferSubData(BufferID, offset, 16 , ref vector);
        }
    }
}
