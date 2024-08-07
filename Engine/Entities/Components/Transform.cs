using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities.Components
{
    public class Transform : Component
    {
        Vector3 m_Position;
        Quaternion m_Rotation;
        Vector3 m_Size;

        Matrix4x4 m_ModelMatrix;

        bool IsDirty { get; set; }
        public Vector3 Position
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

        public Vector3 Size
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

                Matrix4x4 translation = Matrix4x4.CreateTranslation(m_Position);
                Matrix4x4 rotoationMat = Matrix4x4.CreateFromQuaternion(m_Rotation);
                Matrix4x4 scaleMat = Matrix4x4.CreateScale(m_Size);
                IsDirty = false;

                return m_ModelMatrix = scaleMat * rotoationMat * translation;
            }
        }

        public Transform() : base("transform")
        {
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;
            Size = Vector3.One;
            IsDirty = true;
        }

        public void Translate(Vector3 amount)
        {
            Position += amount;
        }

        public void Rotate(Quaternion amount)
        {
            Rotation = Rotation * amount;
        }

        public void Scale(Vector3 amount)
        {
            Size *= amount;
        }

    }
}
