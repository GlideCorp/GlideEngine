using Core.Extensions;
using Core.Maths;
using Core.Maths.Vectors;
using Core.Maths.Matrices;

namespace Engine.Entities.Components
{
    public class Transform : Component
    {
        Vector3Float m_Position;
        Quaternion m_Rotation;
        Vector3Float m_Size;

        Matrix4x4 m_ModelMatrix;

        bool IsDirty { get; set; }
        public Vector3Float Position
        {
            get => m_Position;
            set
            {
                m_Position = value;
                IsDirty |= true;
            }
        }
        public Quaternion Rotation
        {
            get => m_Rotation;
            set
            {
                m_Rotation = value;
                IsDirty |= true;
            }
        }

        public Vector3Float Size
        {
            get => m_Size;
            set
            {
                m_Size = value;
                IsDirty |= true;
            }
        }

        public Matrix4x4 ModelMatrix
        {
            get
            {
                if(!IsDirty)
                {
                    return m_ModelMatrix;
                }

                Matrix4x4 translation = Matrix4x4.Translate(m_Position);
                Matrix4x4 rotationMat = Matrix4x4.Rotate(m_Rotation);
                Matrix4x4 scaleMat = Matrix4x4.Scale(m_Size);
                IsDirty = false;

                return m_ModelMatrix = rotationMat * scaleMat * translation;
            }
        }

        public Transform() : base("transform")
        {
            Position = Vector3Float.Zero;
            Rotation = Quaternion.Identity;
            Size = Vector3Float.One;
            IsDirty = true;
        }

        public void Translate(Vector3Float amount)
        {
            Position += amount;
        }

        public void Rotate(Quaternion rotation)
        {
            Rotation = Rotation * rotation;
        }

        public void Scale(Vector3Float amount)
        {
            Size = new(amount.X * Size.X, amount.Y * Size.Y, amount.Z * Size.Z);
        }

    }
}
