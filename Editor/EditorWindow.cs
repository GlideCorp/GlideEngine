using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Editor
{
    public abstract class EditorWindow
    {
        public string Name { get; private set; }

        bool windowOpen;
        public bool Open { get => windowOpen; set => windowOpen = value; }

        public EditorWindow(string windowName)
        {
            Name = windowName;
            Open = false;
        }

        public void DrawGui()
        {
            if (!Open) return;
                
            ImGui.Begin(Name, ref windowOpen);
            ToolGui();
            ImGui.End();
        }

        protected virtual void ToolGui()
        {
            var samples = new float[100];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = (float)Math.Sin(n * 0.2f + ImGui.GetTime() * 1.5f);
            ImGui.PlotLines("TestPlot", ref samples[0], 100);
        }

        public void Toggle()
        {
            Open = !Open;
        }
    }
}
