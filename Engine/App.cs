using Core.Logs;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SilkWindow = Silk.NET.Windowing.Window;

namespace Engine
{
    public static class App
    {
        public static GL Gl { get; private set; }

        public static IWindow Window { get; private set; }

        
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
            Window.Run();
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

            //TODO: Rimuove una volta implementato sistema di logs
            Console.WriteLine($"{Gl?.GetStringS(GLEnum.Vendor)}\n{Gl?.GetStringS(GLEnum.Version)}\n");

            Shader sTest = new Shader(ShaderDefaults.Default);
        }

        private static void OnUpdate(double deltaTime) { }

        private static void OnRender(double deltaTime) { }
    }
}
