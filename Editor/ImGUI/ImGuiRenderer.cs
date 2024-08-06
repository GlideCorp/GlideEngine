using Editor.ImGUI;
using Engine.Rendering;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Editor.ImGUI
{
    public class ImGuiRenderer
    {
        private ImGuiController? ImGuiController { get; set; }

        private ImFontPtr defaultFontPtr;

        private Texture2D FontAtlas;

        public ImGuiRenderer(GL gl, IView view, IInputContext input)
        {
            ImGuiController = new ImGuiController(gl, view, input);
            unsafe
            {
                var io = ImGui.GetIO();
                io.Fonts.GetTexDataAsRGBA32(out byte* pixelData, out var width, out var height, out var bytesPerPixel);

                var pixels = new byte[width * height * bytesPerPixel];
                Marshal.Copy(new IntPtr(pixelData), pixels, 0, pixels.Length);

                // Create and register the texture
                FontAtlas = new(width, height);
                FontAtlas.SetData(pixels, TextureFormat.RGBA);
            }
        }

        public void BeginLayout(double deltaTime)
        {
            ImGui.GetIO().DeltaTime = (float)deltaTime;
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

        public unsafe void SetDefaultFont(string fontPath, float fontPixelSize, ImFontConfigPtr configs, nint glyphRanges)
        {
            ImGui.GetIO().Fonts.AddFontFromFileTTF(fontPath, fontPixelSize, configs, glyphRanges);

            ImFontConfigPtr configuration = ImGuiNative.ImFontConfig_ImFontConfig();
            configuration.MergeMode = true;
            configuration.PixelSnapH = true;
            configuration.GlyphOffset = new Vector2(0, 3f);

            ushort[] IconRanges = new ushort[3];
            IconRanges[0] = Lucide.IconMin;
            IconRanges[1] = Lucide.IconMax;
            IconRanges[2] = 0;

            GCHandle rangeHandle = GCHandle.Alloc(IconRanges, GCHandleType.Pinned);
            try
            {
                defaultFontPtr = ImGui.GetIO().Fonts.AddFontFromFileTTF("resources\\fonts\\lucide-icon.ttf", fontPixelSize, configuration, rangeHandle.AddrOfPinnedObject());
            }
            finally
            {
                if (rangeHandle.IsAllocated)
                    rangeHandle.Free();
            }

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
            FontAtlas.Dispose();

            FontAtlas = new(width, height);
            FontAtlas.SetData(pixels, TextureFormat.RGBA);

            // Let ImGui know where to find the texture
            io.Fonts.SetTexID((nint)FontAtlas.TextureID);
            io.Fonts.ClearTexData(); // Clears CPU side texture data
        }
    }
}
