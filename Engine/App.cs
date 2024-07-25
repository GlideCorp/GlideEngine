using Core.Logs;
using Engine.Rendering;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Shader = Engine.Rendering.Shader;
using SilkWindow = Silk.NET.Windowing.Window;

namespace Engine
{
    public static class App
    {
        public static GL Gl { get; private set; }

        public static IWindow Window { get; private set; }
        
        //TODO: Eventualmente spostare sta roba -------------------------------------------
        private static Shader sTest;
        private static Mesh mTest;

        private static Vector3D<float> CameraPosition = new(2.0f, 2.0f, 3.0f);
        private static Vector3D<float> CameraTarget = Vector3D<float>.Zero;
        private static Vector3D<float> CameraDirection = Vector3D.Normalize(CameraPosition - CameraTarget);
        private static Vector3D<float> CameraRight = Vector3D.Normalize(Vector3D.Cross(Vector3D<float>.UnitY, CameraDirection));
        private static Vector3D<float> CameraUp = Vector3D.Cross(CameraDirection, CameraRight);
        //---------------------------------------------------------------------------------

        public static void Initialize()
        {
            Logger.Startup();

            WindowOptions options = WindowOptions.Default with
            {
                Size = new Vector2D<int>(800, 600),
                Title = "GlideEngine",
                API = new(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Debug, new(4, 6))
            };

            Window = SilkWindow.Create(options);
            Window.Load += OnLoad;
            Window.Update += OnUpdate;
            Window.Render += OnRender;
        }

        public static void Run()
        {
            Window.Run();
        }

        public static void End()
        {
            Window.Dispose();
            Logger.Shutdown();
        }

        private static void OnLoad()
        {
            Gl = Window.CreateOpenGL();
            Console.WriteLine($"{Gl?.GetStringS(GLEnum.Vendor)}\n{Gl?.GetStringS(GLEnum.Version)}\n");

            //Da mettere in Sandbox per Testing
            sTest = new Shader("shaders/basic.vs", "shaders/basic.fg");

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

            var size = Window.FramebufferSize;
            Matrix4X4<float> view = Matrix4X4.CreateLookAt(CameraPosition, CameraTarget, CameraUp);
            Matrix4X4<float> proj = Matrix4X4.CreatePerspectiveFieldOfView(MathF.PI / 180f * 60.0f, (float)size.X / size.Y, 0.1f, 100.0f); //TODO: Create MathHelper con Deg2Rad(float deg)->float rads

            sTest.SetMatrix4("uModel", Matrix4X4<float>.Identity);
            sTest.SetMatrix4("uView", Matrix4X4<float>.Identity);
            sTest.SetMatrix4("uProjection", view * proj);
        }

        private static void OnUpdate(double deltaTime) 
        { 
        
        }

        private static void OnRender(double deltaTime)
        {
            Graphics.Clear();

            Graphics.Draw(mTest);
        }
    }
}
