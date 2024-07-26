using Engine.Rendering;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Editor.ImGUI
{
    public class ImGuiRenderer
    {
        private ImGuiController? ImGuiController { get; set; }

        private ImFontPtr defaultFontPtr;

        public ImGuiRenderer(GL gl, IView view, IInputContext input)
        {
            ImGuiController = new ImGuiController(gl, view, input);
        }

        public void BeginLayout(double deltaTime)
        {
            ImGuiController?.Update((float)deltaTime);
            ImGui.PushFont(defaultFontPtr);
        }

        public void EndLayout()
        {
            ImGui.PopFont();
            ImGuiController?.Render();
        }

        public void SetDefaultFont(string fontPath, float fontPixelSize)
        {
            SetDefaultFont(fontPath, fontPixelSize, null, ImGui.GetIO().Fonts.GetGlyphRangesDefault());
        }
        public void SetDefaultFont(string fontPath, float fontPixelSize, ImFontConfigPtr configs)
        {
            SetDefaultFont(fontPath, fontPixelSize, configs, ImGui.GetIO().Fonts.GetGlyphRangesDefault());
        }

        public void SetDefaultFont(string fontPath, float fontPixelSize, ImFontConfigPtr configs, nint glyphRanges)
        {
            defaultFontPtr = ImGui.GetIO().Fonts.AddFontFromFileTTF(fontPath, fontPixelSize, configs, glyphRanges);

            RebuildFontAtlas();
        }

        //Ispirato a sta cosa: https://github.com/dovker/Monogame.ImGui/blob/master/MonoGame.ImGui/ImGUIRenderer.cs#L29
        public unsafe void RebuildFontAtlas()
        {
            // Get font texture from ImGui
            var io = ImGui.GetIO();
            io.Fonts.GetTexDataAsRGBA32(out byte* pixelData, out var width, out var height, out var bytesPerPixel);

            // Copy the data to a managed array
            var pixels = new byte[width * height * bytesPerPixel];
            Marshal.Copy(new IntPtr(pixelData), pixels, 0, pixels.Length);

            // Create and register the texture
            Texture2D texture = new(width, height);
            texture.SetData(pixels, TextureFormat.RGBA);

            // Let ImGui know where to find the texture
            io.Fonts.SetTexID((nint)texture.TextureID);
            io.Fonts.ClearTexData(); // Clears CPU side texture data
        }
    }
}
