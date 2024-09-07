using Core.Extensions;
using Core.Maths;
using Core.Maths.Vectors;
using Core.Maths.Matrices;

namespace Engine.Entities.Components
{
    public class Camera : Component
    {
        private Vector3Float m_Position;
        private Vector3Float m_Direction;
        private float m_Fov;

        private Matrix4x4 m_Projection;
        private Matrix4x4 m_View;

        public Vector3Float Position 
        { 
            get => m_Position; 
            set
            {
                m_Position = value;
                IsViewDirty = true;
            }
        }

        public Vector3Float Direction
        {
            get => m_Direction;
            set
            {
                m_Direction = value;
                IsViewDirty = true;
            }
        }

        public float Fov
        {
            get => m_Fov;
            set
            {
                m_Fov = value;
                IsProjDirty = true;
            }
        }

        public Matrix4x4 Projection
        {
            get
            {
                if (!IsProjDirty)
                {
                    return m_Projection;
                }

                Vector2Int size = Application.FramebufferSize;
                m_Projection = Matrix4x4.Perspective(Fov * MathHelper.Deg2Rad, (float)size.X/size.Y, 0.1f, 100.0f);

                IsProjDirty = false;
                return m_Projection;
            }
        }

        public Matrix4x4 View
        {
            get
            {
                if (!IsViewDirty)
                {
                    return m_View;
                }

                m_View = Matrix4x4.LookAt(Position, Position + Direction, Vector3Float.UnitY);

                IsViewDirty = false;
                return m_View;
            }
        }

        private bool IsViewDirty { get; set; }
        private bool IsProjDirty { get; set; }

        public Camera() : base("camera")
        {
            Position = Vector3Float.Zero;
            Direction = Vector3Float.UnitZ;
            Fov = 60;

            IsViewDirty = true;
            IsProjDirty = true;
        }

        public void Refresh()
        {
            IsViewDirty = IsProjDirty = true;
        }

        public void LookAt(Vector3Float point)
        {
            Direction = (point - Position).Normalize();
        }
    }
}
