using Core.Logs;
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
        private ImGuiController? ImGuiController { get; set; }
        private IInputContext? InputContext { get; set; }

        Texture2D? testTexture;

        public override void Startup()
        {
            base.Startup();

        }

        protected override void OnLoad()
        {
            base.OnLoad();

            InputContext = Window.CreateInput();
            ImGuiController = new ImGuiController(Context, Window, InputContext);

            var io = ImGui.GetIO();
            io.BackendFlags = ImGuiBackendFlags.None;
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

            FileStream stream = File.OpenRead("resources\\test.png");
            testTexture = Texture2D.FromStream(stream);

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
            ImGuiController?.Update((float)deltaTime);

            ImGui.BeginMainMenuBar();
            ImGui.Text("Glide Engine");
            ImGui.EndMainMenuBar();

            ImGui.ShowDemoWindow();

            ImGui.Begin("TextureTest");
            if(testTexture != null)
            {
                ImGui.Text($"Texture ID: {testTexture.TextureID}");
                //ImGui.SliderInt("TextureID", ref id, 1, 10);
                ImGui.Image((nint)testTexture.TextureID, new Vector2(256, 256));
            }
            ImGui.End();

            ImGuiController?.Render();
        }

    }
}
