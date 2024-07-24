using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SilkWindow = Silk.NET.Windowing.Window;

namespace Engine
{
    public static class App
    {
        public static GL Gl { get; private set; }

        public static IWindow Window { get; private set; }
        
        //TODO: Eventualmente spostare sta roba ------------------------------------------
        private static Shader sTest;
        private static Mesh mTest;

        private static Vector3 CameraPosition = new Vector3(2.0f, 2.0f, 3.0f);
        private static Vector3 CameraTarget = Vector3.Zero;
        private static Vector3 CameraDirection = Vector3.Normalize(CameraPosition - CameraTarget);
        private static Vector3 CameraRight = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, CameraDirection));
        private static Vector3 CameraUp = Vector3.Cross(CameraDirection, CameraRight);
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
            Matrix4x4 view = Matrix4x4.CreateLookAt(CameraPosition, CameraTarget, CameraUp);
            Matrix4x4 proj = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 180f * 60.0f, (float)size.X / size.Y, 0.1f, 100.0f); //TODO: Create MathHelper con Deg2Rad(float deg)->float rads

            sTest.SetMatrix4("uModel", Matrix4x4.Identity);
            sTest.SetMatrix4("uView", view);
            sTest.SetMatrix4("uProjection", proj);
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
