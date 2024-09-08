using Core.Extensions;
using Core.Logs;
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
        Vector3Float eulerAngles;
        Quaternion rotation;
        public SceneInspector(Transform t) : base($"{Lucide.GalleryHorizontalEnd} Scene Inspector")
        {
            transform = t;
            //t.Rotate(Quaternion.FromEuler(new Vector3Float(0.0f, 90.0f, 0.0f) * MathHelper.Deg2Rad));

            //Quaternion test = Quaternion.FromEuler(new Vector3Float(90.0f, 0.0f, 90.0f) * MathHelper.Deg2Rad);
            //Logger.Info($"{t.Rotation}");
            //Logger.Info($"{t.Rotation.ToEuler() * MathHelper.Rad2Deg}");
        }

        //TODO: Esplorare l'idea per un metodo ToolSceneDraw o simili, che permette a un tool di disegnare sulla scena
        //      cose come gizmos o simili.
        protected override void ToolGui()
        {
            Vector3 rot = transform.Rotation.ToEuler().ToSystem() * MathHelper.Rad2Deg;
            /*
            if (ImGui.DragFloat3($"{Lucide.Rotate3d} Rotation", ref rot))
            {
                Vector3Float r = new Vector3Float(rot.X, rot.Y, rot.Z);
                eulerAngles = r;
                rotation = Quaternion.FromEuler(eulerAngles * MathHelper.Deg2Rad);
            }
            */
            Vector3 pos = transform.Position.ToSystem();
            if (ImGui.DragFloat3($"{Lucide.Move3d} Position", ref pos))
                transform.Position = new Vector3Float(pos.X, pos.Y, pos.Z);

            /*
            Vector3 pos = transform.Position.ToSystem();
            Vector3 rotation = transform.Rotation.ToEuler().ToSystem() * MathHelper.Rad2Deg;
            Vector3 size = transform.Size.ToSystem();

            ImGui.SeparatorText("Transform");

            if (ImGui.DragFloat3($"{Lucide.Move3d} Position", ref pos))
                transform.Position = new Vector3Float(pos.X, pos.Y, pos.Z);


            if (ImGui.DragFloat3($"{Lucide.Rotate3d} Rotation", ref rotation))
            {
                Vector3Float r = new Vector3Float(rotation.X, rotation.Y , rotation.Z );
                transform.Rotation = Quaternion.FromEuler(r * MathHelper.Deg2Rad);
            }

            if (ImGui.DragFloat3($"{Lucide.Scale3d} Size", ref size))
                transform.Size = new Vector3Float(size.X, size.Y, size.Z);

            ImGui.Separator();

            string mat = "";
            for (int j = 0; j < 4; j++) //Righe
            {
                mat += transform.ModelMatrix.GetRow(j).ToString() + "\n";
            }
            ImGui.Text(mat);
            */

        }
    }
}
