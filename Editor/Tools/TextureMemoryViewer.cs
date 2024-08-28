using Editor.Gui;
using ImGuiNET;
using System.Numerics;

namespace Editor
{
    public class TextureMemoryViewer : Tool
    {
        private int CurrentID;

        public TextureMemoryViewer() : base($"{Lucide.Image} Texture Viewer")
        {
        }

        protected override void ToolGui()
        {
            ImGui.SliderInt($"{Lucide.Hash}Texture ID", ref CurrentID, 0, 32);

            Vector2 windowSize = ImGui.GetItemRectSize();
            float max = MathF.Max(windowSize.X, windowSize.Y);
            ImGui.Image(CurrentID, new Vector2(max, max));
        }
    }
}
