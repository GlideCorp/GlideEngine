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

namespace Editor
{
    public class EditorApp : Application
    {
        private ImGuiRenderer? ImGuiRenderer { get; set; }
        private IInputContext? InputContext { get; set; }

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
            InputContext = Window.CreateInput();

            ImGuiRenderer = new ImGuiRenderer(Context, Window, InputContext);
            ImGuiRenderer.SetDefaultFont("resources\\fonts\\SplineSansMono-Medium.ttf", 16);

            ImGuiStylePtr imguiStyle = ImGui.GetStyle();
            imguiStyle.WindowRounding = 5;
            imguiStyle.FrameRounding = 2;
            imguiStyle.GrabRounding = 2;
            imguiStyle.Alpha = 0.8f;

            Logger.Info($"{Context.GetStringS(StringName.Vendor)}\n{Context.GetStringS(StringName.Version)}\n");


            //Robe di testing-----------------------------------------------------------------------

            transform = new Transform();
            camera = new Camera();
            camera.Position = new Vector3D<float>(2, 2, 3);
            camera.LookAt(Vector3D<float>.Zero);

            sTest = Shader.FromStream("resources/shaders/basic.vs", "resources/shaders/basic.fg");

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

            mTest = ModelLoader.Load("resources\\models\\test.glb");
            sTest.Use();

            sTest.SetMatrix4("uView", camera.View);
            sTest.SetMatrix4("uProjection", camera.Projection);

            //Fine robe di testing------------------------------------------------------------------

            WindowManager.Register(new SceneInspector(transform));
            WindowManager.Register(new TextureMemoryViewer());
            WindowManager.LoadWindowsState();
        }

        protected override void OnUpdate(double deltaTime)
        {
            base.OnUpdate(deltaTime);
        }

        protected override void OnRender(double deltaTime)
        {
            Graphics.Clear();

            sTest.SetMatrix4("uModel", transform.ModelMatrix);
            Graphics.Draw(mTest);
            
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
            if(ImGui.MenuItem(fpsMenuItem))
            {
                
            }
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
    }
}
