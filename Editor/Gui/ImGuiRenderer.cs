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

namespace Editor.Gui
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
            GlideEngineTheme();
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

        public void GlideEngineTheme()
        {
            RangeAccessor<Vector4> colors = ImGui.GetStyle().Colors;

            colors[(int)ImGuiCol.Text] = new Vector4(1.00f, 1.00f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.50f, 0.50f, 0.50f, 1.00f);
            colors[(int)ImGuiCol.WindowBg] = new Vector4(0.10f, 0.10f, 0.13f, 1.00f);
            colors[(int)ImGuiCol.ChildBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
            colors[(int)ImGuiCol.PopupBg] = new Vector4(0.10f, 0.10f, 0.13f, 0.92f);
            colors[(int)ImGuiCol.Border] = new Vector4(0.44f, 0.37f, 0.61f, 0.29f);
            colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.00f, 0.00f, 0.00f, 0.24f);
            colors[(int)ImGuiCol.FrameBg] = new Vector4(0.19f, 0.17f, 0.22f, 1.00f);
            colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.19f, 0.20f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.16f, 0.16f, 0.21f, 1.00f);
            colors[(int)ImGuiCol.TitleBg] = new Vector4(0.16f, 0.16f, 0.21f, 1.00f);
            colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.16f, 0.16f, 0.21f, 1.00f);
            colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.16f, 0.16f, 0.21f, 1.00f);
            colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.16f, 0.16f, 0.21f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.10f, 0.10f, 0.13f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.16f, 0.16f, 0.21f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.19f, 0.20f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.24f, 0.24f, 0.32f, 1.00f);
            colors[(int)ImGuiCol.CheckMark] = new Vector4(0.74f, 0.58f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.59f, 0.46f, 0.90f, 0.90f);
            colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.74f, 0.58f, 0.98f, 0.54f);
            colors[(int)ImGuiCol.Button] = new Vector4(0.16f, 0.16f, 0.21f, 1.00f);
            colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.24f, 0.25f, 0.31f, 1.00f);
            colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.44f, 0.39f, 0.64f, 1.00f);
            colors[(int)ImGuiCol.Header] = new Vector4(0.13f, 0.13f, 0.17f, 1.00f);
            colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.27f, 0.28f, 0.35f, 1.00f);
            colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.16f, 0.16f, 0.21f, 1.00f);
            colors[(int)ImGuiCol.Separator] = new Vector4(0.64f, 0.53f, 0.91f, 1.00f);
            colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.74f, 0.58f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.84f, 0.58f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.44f, 0.37f, 0.61f, 0.29f);
            colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.74f, 0.58f, 0.98f, 0.29f);
            colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.84f, 0.58f, 1.00f, 0.29f);
            colors[(int)ImGuiCol.TabHovered] = new Vector4(0.24f, 0.24f, 0.32f, 1.00f);
            colors[(int)ImGuiCol.Tab] = new Vector4(0.16f, 0.16f, 0.21f, 1.00f);
            colors[(int)ImGuiCol.TabActive] = new Vector4(0.20f, 0.22f, 0.27f, 1.00f);
            //colors[(int)ImGuiCol.TabSeleTTactedOverline] = new Vector4(0.26f, 0.59f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.16f, 0.16f, 0.21f, 1.00f);
            colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.16f, 0.16f, 0.21f, 1.00f);
            //colors[(int)ImGuiCol.TabDimmedSelectedOverline] = new Vector4(0.50f, 0.50f, 0.50f, 1.00f);
            colors[(int)ImGuiCol.DockingPreview] = new Vector4(0.44f, 0.37f, 0.61f, 1.00f);
            colors[(int)ImGuiCol.DockingEmptyBg] = new Vector4(0.20f, 0.20f, 0.20f, 1.00f);
            colors[(int)ImGuiCol.PlotLines] = new Vector4(0.61f, 0.61f, 0.61f, 1.00f);
            colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(1.00f, 0.43f, 0.35f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.90f, 0.70f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(1.00f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.TableHeaderBg] = new Vector4(0.19f, 0.19f, 0.20f, 1.00f);
            colors[(int)ImGuiCol.TableBorderStrong] = new Vector4(0.31f, 0.31f, 0.35f, 1.00f);
            colors[(int)ImGuiCol.TableBorderLight] = new Vector4(0.23f, 0.23f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
            colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(1.00f, 1.00f, 1.00f, 0.06f);
            //colors[(int)ImGuiCol.TextLink] = new Vector4(0.26f, 0.59f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.26f, 0.59f, 0.98f, 0.35f);
            colors[(int)ImGuiCol.DragDropTarget] = new Vector4(1.00f, 1.00f, 0.00f, 0.90f);
            colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.26f, 0.59f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(1.00f, 1.00f, 1.00f, 0.70f);
            colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.80f, 0.80f, 0.80f, 0.20f);
            colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.80f, 0.80f, 0.80f, 0.35f);
        }
    }
}
