using ImGuiNET;
using System.Numerics;

namespace Editor
{
    public class TextureMemoryViewer : EditorWindow
    {
        private int CurrentID;

        public TextureMemoryViewer() : base("Texture Viewer")
        {
        }

        protected override void ToolGui()
        {
            ImGui.SliderInt("Texture ID", ref CurrentID, 0, 32);

            Vector2 windowSize = ImGui.GetItemRectSize();
            float max = MathF.Max(windowSize.X, windowSize.Y);
            ImGui.Image(CurrentID, new Vector2(max, max));
        }
    }
}
