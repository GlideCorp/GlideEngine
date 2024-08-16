
using Core.Logs;
using Engine.Utilities;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SilkWindow = Silk.NET.Windowing.Window;

namespace Engine
{
    public class Application
    {
        const string WindowSettingsPath = "Settings/Window.json";

        private static Application? _instance = null;
        protected static Application Instance
        {
            get
            {
                if (_instance is null) { throw new NullReferenceException(); }
                return _instance;
            }
        }

        protected static IWindow Window
        {
            get
            {
                if (Instance.WindowPrivate is null)
                {
                    Logger.Error("Null context");
                    throw new NullReferenceException();
                }

                return Instance.WindowPrivate;
            }
        }

        public static GL Context
        {
            get
            {
                if (Instance.ContextPrivate is null)
                {
                    Logger.Error("Null context");
                    throw new NullReferenceException();
                }

                return Instance.ContextPrivate;
            }
        }

        public static Vector2D<int> FramebufferSize { get { return Instance.WindowPrivate.FramebufferSize; } }

        private IWindow WindowPrivate { get; init; }
        private GL? ContextPrivate { get; set; }

        public Application()
        {
            Logger.Startup();

            WindowOptions options = LoadWindowOptions();

            WindowPrivate = SilkWindow.Create(options);
            WindowPrivate.Load += OnLoad;
            WindowPrivate.Update += OnUpdate;
            WindowPrivate.Render += OnRender;
            WindowPrivate.Closing += OnClosing;
            WindowPrivate.FramebufferResize += OnFramebufferResize;

            ContextPrivate = null;

            _instance = this;
        }

        // TODO: rewrite WindowOptions to be serializable
        private static WindowOptions LoadWindowOptions()
        {
            /*
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
            */

            return WindowOptions.Default with
            {
                Size = new Vector2D<int>(800, 600),
                Title = "GlideEngine",
                API = new(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Debug, new(4, 6)),
                Samples = 4,
                VSync = false
            };
        }

        public virtual void Startup()
        {
        }

        public virtual void Run()
        {
            WindowPrivate.Run();
        }

        public virtual void Shutdown()
        {
            Logger.Shutdown();

            WindowPrivate.Dispose();
            _instance = null;
        }

        protected virtual void OnLoad()
        {
            ContextPrivate = WindowPrivate.CreateOpenGL();
            ContextPrivate.Enable(EnableCap.Multisample);
            ContextPrivate.Enable(EnableCap.DepthTest);
        }

        protected virtual void OnUpdate(double deltaTime)
        {
            Time.DeltaTime = (float)deltaTime;
        }

        protected virtual void OnRender(double deltaTime)
        {

        }

        protected virtual void OnClosing()
        {

        }

        protected virtual void OnFramebufferResize(Vector2D<int> newSize)
        {
            ContextPrivate.Viewport(newSize);
        }
    }
}
