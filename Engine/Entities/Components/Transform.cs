using Core.Extensions;
using Silk.NET.Maths;

namespace Engine.Entities.Components
{
    public class Transform : Component
    {
        Vector3D<float> m_Position;
        Vector3D<float> m_Rotation;
        Vector3D<float> m_Size;

        Matrix4X4<float> m_ModelMatrix;

        bool IsDirty { get; set; }
        public Vector3D<float> Position
        {
            get => m_Position;
            set
            {
                m_Position = value;
                IsDirty |= true;
            }
        }
        public Vector3D<float> Rotation
        {
            get => m_Rotation;
            set
            {
                m_Rotation = value;
                IsDirty |= true;
            }
        }

        public Vector3D<float> Size
        {
            get => m_Size;
            set
            {
                m_Size = value;
                IsDirty |= true;
            }
        }

        public Matrix4X4<float> ModelMatrix
        {
            get
            {
                if(!IsDirty)
                {
                    return m_ModelMatrix;
                }

                Matrix4X4<float> translation = Matrix4X4.CreateTranslation(m_Position);
                Matrix4X4<float> rotoationMat = Matrix4X4.CreateFromQuaternion(m_Rotation.ToQuat());
                Matrix4X4<float> scaleMat = Matrix4X4.CreateScale(m_Size);
                IsDirty = false;

                return m_ModelMatrix = scaleMat * rotoationMat * translation;
            }
        }

        public Transform() : base("transform")
        {
            Position = Vector3D<float>.Zero;
            Rotation = Vector3D<float>.Zero;
            Size = Vector3D<float>.One;
            IsDirty = true;
        }

        public void Translate(Vector3D<float> amount)
        {
            Position += amount;
        }

        public void Rotate(Vector3D<float> amount)
        {
            Rotation = Rotation + amount * MathHelper.Deg2Rad;
        }

        public void Scale(Vector3D<float> amount)
        {
            Size *= amount;
        }

    }
}
