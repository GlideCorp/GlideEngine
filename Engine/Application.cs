
using Core.Logs;
using Core.Serialization;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using Silk.NET.Windowing;

using SilkWindow = Silk.NET.Windowing.Window;

namespace Engine
{
    public class Application
    {
        const string WindowSettingsPath = "Settings/Window.json";

        protected static Application? _instance = null;
        protected static Application Instance
        {
            get
            {
                if (_instance is null) { throw new NullReferenceException(); }
                return _instance;
            }
        }

        public static GL Context
        {
            get
            {
                if (Instance.ContextInternal is null)
                {
                    Logger.Error("Null context");
                    throw new NullReferenceException();
                }

                return Instance.ContextInternal;
            }
        }
        protected static IWindow Window
        {
            get
            {
                if (Instance.WindowInternal is null)
                {
                    Logger.Error("Null context");
                    throw new NullReferenceException();
                }

                return Instance.WindowInternal;
            }
        }

        public static Vector2D<int> FramebufferSize { get { return Instance.WindowInternal.FramebufferSize; } }

        private IWindow WindowInternal { get; init; }
        private GL? ContextInternal { get; set; }

        protected Application()
        {
            Logger.Startup();

            WindowOptions options = LoadWindowOptions();

            WindowInternal = SilkWindow.Create(options);
            WindowInternal.Load += OnLoad;
            WindowInternal.Update += OnUpdate;
            WindowInternal.Render += OnRender;
            WindowInternal.FramebufferResize += OnFramebufferResize;

            ContextInternal = null;

            _instance = this;
        }

        private static WindowOptions LoadWindowOptions()
        {
            FileInfo fileInfo = new(WindowSettingsPath);
            DirectoryInfo directoryInfo = fileInfo.Directory!;

            if (!directoryInfo.Exists || !fileInfo.Exists ||
                !Serializer.Deserialize(fileInfo, out WindowOptions windowOptions))
            {
                windowOptions = WindowOptions.Default with
                {
                    Size = new Vector2D<int>(800, 600),
                    Title = "GlideEngine",
                    API = new(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Debug, new(4, 6))
                };
            }

            return windowOptions = WindowOptions.Default with
            {
                Size = new Vector2D<int>(800, 600),
                Title = "GlideEngine",
                API = new(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Debug, new(4, 6))
            }; ;
        }

        public virtual void Startup()
        {
            
        }

        public virtual void Run()
        {
            Instance.WindowInternal.Run();
        }

        public virtual void Shutdown()
        {
            Logger.Shutdown();

            WindowInternal.Dispose();
            _instance = null;
        }

        protected virtual void OnLoad()
        {
            ContextInternal = WindowInternal.CreateOpenGL();
        }

        protected virtual void OnUpdate(double deltaTime)
        {

        }

        protected virtual void OnRender(double deltaTime)
        {

        }

        private static void OnFramebufferResize(Vector2D<int> newSize)
        {
            Context.Viewport(newSize);
        }
    }
}
