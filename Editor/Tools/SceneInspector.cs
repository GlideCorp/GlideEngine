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
            
            if (ImGui.BeginTable("table", 2, ImGuiTableFlags.SizingFixedFit))
            {
                ImGui.TableSetupColumn("", ImGuiTableColumnFlags.WidthFixed);
                ImGui.TableSetupColumn("", ImGuiTableColumnFlags.WidthStretch);

                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Text($"{Lucide.Move3d} Position");
                ImGui.TableSetColumnIndex(1);

                if (ImGui.DragFloat3("", ref pos))
                    transform.Position = pos;

                ImGui.TableNextRow();

                ImGui.TableSetColumnIndex(0);
                ImGui.Text($"{Lucide.Rotate3d} Rotation");
                ImGui.TableSetColumnIndex(1);

                if (ImGui.DragFloat3("", ref rotation))
                    transform.Rotation = (rotation * MathHelper.Deg2Rad).ToQuat();

                ImGui.TableNextRow();

                ImGui.TableSetColumnIndex(0);
                ImGui.Text($"{Lucide.Scale3d} Size");
                ImGui.TableSetColumnIndex(1);

                if (ImGui.DragFloat3("", ref size))
                    transform.Size = size;

                ImGui.EndTable();
            }

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
