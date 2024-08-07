using Core.Extensions;
using Editor.Gui;
using Engine.Entities.Components;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Tools
{
    public class SceneInspector : Tool
    {
        Transform transform;

        public SceneInspector() : base($"{Lucide.GalleryHorizontalEnd} Scene Inspector")
        {

            transform = new Transform();
        }

        protected override void ToolGui()
        {
            Vector3 pos = transform.Position;
            Vector3 rotation = transform.Rotation.ToEuler() * MathHelper.Rad2Deg;
            Vector3 size = transform.Size;

            ImGui.SeparatorText("Transform");

            if (ImGui.DragFloat3($"{Lucide.Move3d} Position", ref pos))
                transform.Position = pos;

            if (ImGui.DragFloat3($"{Lucide.Rotate3d} Rotation", ref rotation))
                transform.Rotation = (rotation * MathHelper.Deg2Rad).ToQuat();

            if (ImGui.DragFloat3($"{Lucide.Scale3d} Size", ref size))
                transform.Size = size;

            ImGui.Separator();

            string mat = "";
            for (int i = 0; i < 4; i++) //Colonne
            {
                for (int j = 0; j < 4; j++) //Righe
                {
                    mat += transform.ModelMatrix[j, i] + ", ";
                }
                mat += "\n";
            }

            
            ImGui.Text(mat);
        }
    }
}
