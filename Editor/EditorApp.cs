using Core.Logs;
using Editor.ImGUI;
using Engine;
using Engine.Rendering;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Editor
{
    public class EditorApp : Application
    {
        private ImGuiRenderer? ImGuiRenderer { get; set; }
        private IInputContext? InputContext { get; set; }

        List<EditorWindow> EditorWindows{ get; set; }

        Texture2D? testTexture;

        public override void Startup()
        {
            base.Startup();
            EditorWindows = new();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            InputContext = Window.CreateInput();

            ImGuiRenderer = new ImGuiRenderer(Context,Window, InputContext);
            ImGuiRenderer.SetDefaultFont("resources\\fonts\\SplineSansMono-Medium.ttf", 16);

            var io = ImGui.GetIO();
            io.BackendFlags = ImGuiBackendFlags.None;
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

            ImGuiStylePtr imguiStyle = ImGui.GetStyle();
            imguiStyle.WindowRounding = 5;
            imguiStyle.FrameRounding = 2;
            imguiStyle.GrabRounding = 2;

            FileStream stream = File.OpenRead("resources\\test.png");
            testTexture = Texture2D.FromStream(stream, TextureParameters.Default with { Filters = TextureFilter.Nearest });

            EditorWindows.Add(new TestWindow());

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
            ImGuiRenderer?.BeginLayout((float)deltaTime);

            ImGui.BeginMainMenuBar();
            ImGui.Text("Glide Engine");

            if (ImGui.BeginMenu("Tools"))
            {
                foreach (var window in EditorWindows)
                {
                    if (ImGui.MenuItem(window.Name))
                    {
                        window.Open = true;
                    }
                }
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
            ImGui.DockSpaceOverViewport(ImGui.GetMainViewport(), ImGuiDockNodeFlags.PassthruCentralNode);

            foreach (var window in EditorWindows)
            {
                window.DrawGui();
            }

            ImGui.Begin("TextureTest");
            if(testTexture != null)
            {
                ImGui.Text($"Texture ID: {testTexture.TextureID}");
                //ImGui.SliderInt("TextureID", ref id, 1, 10);
                ImGui.Image((nint)testTexture.TextureID, new Vector2(256, 256));
            }
            ImGui.End();

            ImGuiRenderer?.EndLayout();
        }


    }
}
