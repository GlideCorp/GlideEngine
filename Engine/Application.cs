﻿using Core.Logs;
using Core.Maths.Vectors;
using Engine.Rendering;
using Engine.Utilities;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Drawing;
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

        public static IInputContext InputContext
        {
            get
            {
                if (Instance.InputPrivate is null)
                {
                    Logger.Error("Null context");
                    throw new NullReferenceException();
                }

                return Instance.InputPrivate;
            }
        }

        public static Vector2Int FramebufferSize 
        { 
            get 
            {
                Vector2D<int> size = Instance.WindowPrivate.FramebufferSize;
                return new(size.X, size.Y); 
            } 
        }

        private IWindow WindowPrivate { get; init; }
        private GL? ContextPrivate { get; set; }
        private IInputContext? InputPrivate { get; set; }

        private DebugProc DebugMessageDelegate { get; set; }

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

            DebugMessageDelegate = GlLogMessageCallback;
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
            ContextPrivate.Enable(EnableCap.CullFace);
            ContextPrivate.DebugMessageCallback(DebugMessageDelegate, in IntPtr.Zero);

            InputPrivate = WindowPrivate.CreateInput();
            Input.Keyboard = InputPrivate.Keyboards[0];
            Input.Mouse = InputPrivate.Mice[0];

            Renderer.Startup();
            Graphics.ClearColor = Color.LightSkyBlue;
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
            ContextPrivate?.Viewport(newSize);

            Renderer.ResizeMainBuffer(new Vector2Int(newSize.X, newSize.Y));
        }

        private void GlLogMessageCallback(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message, nint userParam)
        {
            switch (severity)
            {
                case GLEnum.DebugSeverityLow:
                    Logger.Info($"API Low Severity: {type}, {message}");
                    break;

                case GLEnum.DebugSeverityMedium:
                    Logger.Warning($"API Medium Severity: {type}, {message}");
                    break;

                case GLEnum.DebugSeverityHigh:
                    Logger.Warning($"API High Severity: {type}, {message}");
                    break;

                default:
                    return;
            }
        }
    }
}
