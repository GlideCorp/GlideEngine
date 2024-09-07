using Core.Logs;
using Core.Maths.Vectors;
using Engine.Utilities;
using ImGuiNET;

namespace Editor.Tools
{
    public class InputTester : Tool
    {
        Vector2Float previousMousePosition;

        public InputTester() : base("Input Tester")
        {
            previousMousePosition = Vector2Float.Zero;
        }

        protected override void ToolGui()
        {
            base.ToolGui();

            ImGui.Text($"Current Keyboard: {Input.Keyboard.Name}");
            ImGui.Text($"Current Clipboard: {Input.Keyboard.ClipboardText}");

            ImGui.Separator();
            ImGui.Text($"Current Mouse: {Input.Mouse.Name}");
            ImGui.Text($"Mouse Position: {Input.MousePosition()}");
            ImGui.Text($"Mouse Delta: {Input.MousePosition() - previousMousePosition}");
            ImGui.Text($"Scroll Value: {Input.MouseScroll()}");
            ImGui.Spacing();
            ImGui.Text($"LeftMousePressed {Input.MousePressed(0)}");
            ImGui.Text($"RigthMousePressed {Input.MousePressed(1)}");
            ImGui.Text($"MiddleMousePressed {Input.MousePressed(2)}");
            ImGui.Spacing();
            ImGui.Text($"LeftMouseDown {Input.MouseDown(0)}");
            ImGui.Text($"RigthMouseDown {Input.MouseDown(1)}");
            ImGui.Text($"MiddleMouseDown {Input.MouseDown(2)}");
            ImGui.Spacing();
            ImGui.Text($"LeftMouseReleased {Input.MouseReleased(0)}");
            ImGui.Text($"RigthMouseReleased {Input.MouseReleased(1)}");
            ImGui.Text($"MiddleMouseReleased {Input.MouseReleased(2)}");

            previousMousePosition = Input.MousePosition();
        }
    }
}
