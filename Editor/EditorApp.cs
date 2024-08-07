using Core.Logs;
using Editor.Gui;
using Editor.Tools;
using Engine;
using Engine.Rendering;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Maths;

namespace Editor
{
    public class EditorApp : Application
    {
        private ImGuiRenderer? ImGuiRenderer { get; set; }
        private IInputContext? InputContext { get; set; }

        public override void Startup()
        {
            base.Startup();
            Window.Size = new Vector2D<int>(1280, 800);

            WindowManager.Register(new SceneInspector());
            WindowManager.Register(new TextureMemoryViewer());
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            WindowManager.LoadWindowsState();

            InputContext = Window.CreateInput();

            ImGuiRenderer = new ImGuiRenderer(Context, Window, InputContext);
            ImGuiRenderer.SetDefaultFont("resources\\fonts\\SplineSansMono-Medium.ttf", 16);

            ImGuiStylePtr imguiStyle = ImGui.GetStyle();
            imguiStyle.WindowRounding = 5;
            imguiStyle.FrameRounding = 2;
            imguiStyle.GrabRounding = 2;

            Logger.Info($"{Context.GetStringS(GLEnum.Vendor)}\n{Context.GetStringS(GLEnum.Version)}\n");
        }

        protected override void OnUpdate(double deltaTime)
        {
            base.OnUpdate(deltaTime);
        }

        protected override void OnRender(double deltaTime)
        {
            Graphics.Clear();

            //Editor Render Loop
            ImGuiRenderer?.BeginLayout(deltaTime);
            ImGui.DockSpaceOverViewport(ImGui.GetMainViewport());
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
