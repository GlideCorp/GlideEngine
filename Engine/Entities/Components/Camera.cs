using Core.Extensions;
using Silk.NET.Maths;

namespace Engine.Entities.Components
{
    public class Camera : Component
    {
        Vector3D<float> m_Position;
        Vector3D<float> m_Direction;
        float m_Fov;

        Matrix4X4<float> m_Projection;
        Matrix4X4<float> m_View;

        public Vector3D<float> Position 
        { 
            get => m_Position; 
            set
            {
                m_Position = value;
                IsViewDirty = true;
            }
        }

        public Vector3D<float> Direction
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

        public Matrix4X4<float> Projection
        {
            get
            {
                if (!IsProjDirty)
                {
                    return m_Projection;
                }

                Vector2D<int> size = Application.FramebufferSize;
                m_Projection = Matrix4X4.CreatePerspectiveFieldOfView(Fov * MathHelper.Deg2Rad, (float)size.X/size.Y, 0.1f, 100.0f);

                IsProjDirty = false;
                return m_Projection;
            }
        }

        public Matrix4X4<float> View
        {
            get
            {
                if (!IsViewDirty)
                {
                    return m_View;
                }

                m_View = Matrix4X4.CreateLookAt(Position, Position + Direction, Vector3D<float>.UnitY);

                IsViewDirty = false;
                return m_View;
            }
        }

        private bool IsViewDirty { get; set; }
        private bool IsProjDirty { get; set; }

        public Camera() : base("camera")
        {
            Position = Vector3D<float>.Zero;
            Direction = Vector3D<float>.UnitZ;
            Fov = 60;

            IsViewDirty = true;
            IsProjDirty = true;
        }

        public void Refresh()
        {
            IsViewDirty = IsProjDirty = true;
        }

        public void LookAt(Vector3D<float> point)
        {
            Direction = Vector3D.Normalize(point - Position);
        }
    }
}
