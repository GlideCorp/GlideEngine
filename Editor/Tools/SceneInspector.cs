using Editor.ImGUI;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Tools
{
    public class SceneInspector : Tool
    {
        public SceneInspector() : base($"{Lucide.GalleryHorizontalEnd} Scene Inspector")
        {
        }

        protected override void ToolGui()
        {
            base.ToolGui();
        }
    }
}
