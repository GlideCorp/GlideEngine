using Core.Logs;
using Editor.Gui;
using Editor.Tools;
using Engine;
using Engine.Rendering;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Maths;
using Shader = Engine.Rendering.Shader;
using Engine.Entities.Components;
using Engine.Utilities;
using System.Drawing;
using Core.Extensions;
using Engine.Rendering.PostProcessing;
using System.Numerics;

namespace Editor
{
    public class EditorApp : Application
    {
        private ImGuiRenderer? ImGuiRenderer { get; set; }

        Transform transform;
        Camera camera;
        Mesh mTest;
        Shader sTest;

        public override void Startup()
        {
            base.Startup();
            Window.Size = new Vector2D<int>(1280, 800);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            ImGuiRenderer = new ImGuiRenderer(Context, Window, InputContext);
            ImGuiRenderer.SetDefaultFont("resources\\fonts\\SplineSansMono-Medium.ttf", 16);

            Logger.Info($"{Context.GetStringS(StringName.Vendor)} \t {Context.GetStringS(StringName.Version)}");

            //Robe di testing-----------------------------------------------------------------------
            transform = new();
            camera = new()
            {
                Position = new Vector3D<float>(-2, 2, -3)
            };
            camera.LookAt(Vector3D<float>.Zero);

            sTest = Shader.FromStream("resources/shaders/basic.vs", "resources/shaders/basic.fg");
            mTest = ModelLoader.Load("resources\\models\\shapes.glb");

            PostProcessing.Push(new DrawDepthEffect());

            //Fine robe di testing------------------------------------------------------------------

            WindowManager.Register(new SceneInspector(transform));
            WindowManager.Register(new TextureMemoryViewer());
            WindowManager.Register(new InputTester());
            WindowManager.LoadWindowsState();
        }

        protected override void OnUpdate(double deltaTime)
        {
            base.OnUpdate(deltaTime);
        }

        protected override void OnRender(double deltaTime)
        {
            Context.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBuffer.FrameBufferID);
            Graphics.Clear();
            sTest.Use();
            sTest.SetMatrix4("uView", camera.View);
            sTest.SetMatrix4("uProjection", camera.Projection);
            sTest.SetMatrix4("uModel", transform.ModelMatrix);

            sTest.SetVector3("uCameraWorldPosition", camera.Position);

            sTest.SetVector3("uLigthPos", new Vector3D<float>(2, 2, 1));
            sTest.SetVector3("uLigthColor", new Vector3D<float>(1, 1, 1));
            sTest.SetVector3("uAmbientColor", new Vector3D<float>(0.79f, 0.94f, 0.97f));

            sTest.SetVector3("uBaseColor", new Vector3D<float>(1, 0, 0));
            sTest.SetFloat("uSmoothness", 0.5f);
            Graphics.Draw(mTest);

            PostProcessing.Execute();
            Context.BindFramebuffer(FramebufferTarget.Framebuffer, 0);


            //Editor Render Loop
            ImGuiRenderer?.BeginLayout(deltaTime);
            ImGui.DockSpaceOverViewport(ImGui.GetMainViewport(), ImGuiDockNodeFlags.PassthruCentralNode);
            ImGui.BeginMainMenuBar();
            ImGui.Text($"{Lucide.Wind} Glide Engine");
            ImGui.Separator();

            foreach (var window in WindowManager.Windows)
            {
                if (ImGui.MenuItem(window.Name))
                {
                    window.Toggle();
                }
            }

            string fpsMenuItem = $"{Lucide.Film} {Time.FPS:D} {Lucide.Dot} {Time.DeltaTime:N5}";
            ImGui.SameLine(ImGui.GetWindowWidth() - ImGui.CalcTextSize(fpsMenuItem).X - 20);

            Vector3 fpsColor = (Time.FPS >= 60 ? Color.LawnGreen : (Time.FPS < 30 ? Color.OrangeRed : Color.LightGoldenrodYellow)).ToVec3();
            fpsColor = Vector3.Normalize(fpsColor);

            ImGui.TextColored(new Vector4(fpsColor, 1)  , fpsMenuItem);
            ImGui.EndMainMenuBar();
            ImGui.DockSpaceOverViewport(ImGui.GetMainViewport(), ImGuiDockNodeFlags.PassthruCentralNode);

            ImGui.ShowDemoWindow();

            foreach (var window in WindowManager.Windows)
            {
                window.DrawGui();
            }

            ImGuiRenderer?.EndLayout();
        }

        protected override void OnClosing()
        {
            WindowManager.SaveWindowsState();
        }

        protected override void OnFramebufferResize(Vector2D<int> newSize)
        {
            camera.Refresh();
            base.OnFramebufferResize(newSize);
        }
    }
}
