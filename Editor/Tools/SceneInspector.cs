using Core.Extensions;
using Core.Maths;
using Core.Maths.Vectors;
using Editor.Gui;
using Engine.Entities.Components;
using Engine.Rendering;
using ImGuiNET;
using Silk.NET.Maths;
using System.Numerics;
using Quaternion = Core.Maths.Quaternion;

namespace Editor.Tools
{
    public class SceneInspector : Tool
    {
        Transform transform;
        public SceneInspector(Transform t) : base($"{Lucide.GalleryHorizontalEnd} Scene Inspector")
        {
            transform = t;
        }

        //TODO: Esplorare l'idea per un metodo ToolSceneDraw o simili, che permette a un tool di disegnare sulla scena
        //      cose come gizmos o simili.
        protected override void ToolGui()
        {
            
            Vector3 pos = transform.Position.ToSystem();
            Vector3 rotation = transform.Rotation.ToEuler().ToSystem() * MathHelper.Rad2Deg;
            Vector3 size = transform.Size.ToSystem();

            ImGui.SeparatorText("Transform");

            if (ImGui.DragFloat3($"{Lucide.Move3d} Position", ref pos))
                transform.Position = new Vector3Float(pos.X, pos.Y, pos.Z);

            
            if (ImGui.DragFloat3($"{Lucide.Rotate3d} Rotation", ref rotation))
            {
                Vector3D<float> r = new Vector3D<float>(rotation.X, rotation.Y, rotation.Z);
                transform.Rotation = Quaternion.FromEuler(rotation);
            }

            if (ImGui.DragFloat3($"{Lucide.Scale3d} Size", ref size))
                transform.Size = new Vector3Float(size.X, size.Y, size.Z);

            ImGui.Separator();

            string mat = "";
            for (int i = 0; i < 4; i++) //Colonne
            {
                for (int j = 0; j < 4; j++) //Righe
                {
                    mat += transform.ModelMatrix.GetRow(j).ToString();
                }
                mat += "\n";
            }
            
            ImGui.Text(mat);
        }
    }
}
