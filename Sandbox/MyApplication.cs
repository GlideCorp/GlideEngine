using Core.Logs;
using Engine;
using Engine.Rendering;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Sandbox
{
    internal class MyApplication : Application
    {
        private Engine.Rendering.Shader sTest;
        private Mesh mTest;

        private Vector3D<float> CameraPosition;
        private Vector3D<float> CameraTarget;
        private Vector3D<float> CameraDirection;
        private Vector3D<float> CameraRight;
        private Vector3D<float> CameraUp;

        public MyApplication() : base()
        {
            CameraPosition = new(2.0f, 2.0f, 3.0f);
            CameraTarget = Vector3D<float>.Zero;
            CameraDirection = Vector3D.Normalize(CameraPosition - CameraTarget);
            CameraRight = Vector3D.Normalize(Vector3D.Cross(Vector3D<float>.UnitY, CameraDirection));
            CameraUp = Vector3D.Cross(CameraDirection, CameraRight);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            Logger.Info($"{Context.GetStringS(GLEnum.Vendor)}\n{Context.GetStringS(GLEnum.Version)}\n");

            //Da mettere in Sandbox per Testing
            sTest = new Engine.Rendering.Shader("shaders/basic.vs", "shaders/basic.fg");

            List<Vertex> vertices = new List<Vertex>
            {
                new(){ Position = new(0.5f, 0.5f, 0.0f)},
                new(){ Position = new (0.5f, -0.5f, 0.0f)},
                new(){ Position = new (-0.5f, -0.5f, 0.0f)},
                new(){ Position =  new(-0.5f, 0.5f, 0.0f)}
            };

            List<uint> indices = new List<uint>
            {
                0, 1, 3,  // first Triangle
                1, 2, 3   // second Triangle
            };

            mTest = new Mesh(vertices, indices);
            sTest.Use();

            var size = FramebufferSize;
            Matrix4X4<float> view = Matrix4X4.CreateLookAt(CameraPosition, CameraTarget, CameraUp);
            Matrix4X4<float> proj = Matrix4X4.CreatePerspectiveFieldOfView(MathF.PI / 180f * 60.0f, (float)size.X / size.Y, 0.1f, 100.0f); //TODO: Create MathHelper con Deg2Rad(float deg)->float rads

            sTest.SetMatrix4("uModel", Matrix4X4<float>.Identity);
            sTest.SetMatrix4("uView", Matrix4X4<float>.Identity);
            sTest.SetMatrix4("uProjection", view * proj);
        }

        protected override void OnRender(double deltaTime)
        {
            Graphics.Clear();
            Graphics.Draw(mTest);
        }
    }
}
