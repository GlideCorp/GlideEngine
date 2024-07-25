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

            ImGuiFontConfig fontConfig = new("resources\\fonts\\Arvo-Regular.ttf", 15);
            ImGuiController = new ImGuiController(Context,Window, InputContext, fontConfig);

            var io = ImGui.GetIO();
            io.BackendFlags = ImGuiBackendFlags.None;
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

            //io.Fonts.AddFontFromFileTTF("resources\\fonts\\Arvo-Regular.ttf", 15.0f, null, io.Fonts.GetGlyphRangesDefault());
            //RebuildFontAtlas();

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

        //Sposta in una classe a parte, idealmente una che gestisce in modo più specifico DearImgui nel contesto di un Editor
        //Forse the cherno aveva un video al riguardo boh :)
        //Senno una cosa simile: https://github.com/dovker/Monogame.ImGui/blob/master/MonoGame.ImGui/ImGUIRenderer.cs#L29
        unsafe void RebuildFontAtlas()
        {     
            // Get font texture from ImGui
            var io = ImGui.GetIO();
            io.Fonts.GetTexDataAsRGBA32(out byte* pixelData, out var width, out var height, out var bytesPerPixel);

            // Copy the data to a managed array
            var pixels = new byte[width * height * bytesPerPixel];
            Marshal.Copy(new IntPtr(pixelData), pixels, 0, pixels.Length);

            // Create and register the texture
            Texture2D texture = new(width, height);
            texture.SetData(pixels);

            // Let ImGui know where to find the texture
            io.Fonts.SetTexID((nint)texture.TextureID);
            io.Fonts.ClearTexData(); // Clears CPU side texture data
        }

    }
}
