using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Maths;

namespace Engine
{
    public static class App
    {

        public static GL Gl { get; private set; }

        public static IWindow Window { get; private set; }

        
        public static void Initialize()
        {
            WindowOptions options = WindowOptions.Default with
            {
                Size = new Vector2D<int>(800, 600),
                Title = "GlideEngine",
                API = new(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Debug, new(4, 6))
            };

            //TODO: Rivedere nomeclatura per le proprietà
            Window = Silk.NET.Windowing.Window.Create(options);
            Window.Load += OnLoad;
            Window.Update += OnUpdate;
            Window.Render += OnRender;
            Window.Run();
        }

        public static void Run()
        {
            Window.Run();
        }

        public static void End()
        {
            Window.Dispose();
        }

        private static void OnLoad()
        {
            Gl = Window.CreateOpenGL();

            //TODO: Rimuove una volta implementato sistema di logs
            Console.WriteLine($"{Gl?.GetStringS(GLEnum.Vendor)}\n{Gl?.GetStringS(GLEnum.Version)}\n");

            Shader sTest = new Shader(ShaderDefaults.Default);
        }

        private static void OnUpdate(double deltaTime) { }

        private static void OnRender(double deltaTime) { }
    }
}
