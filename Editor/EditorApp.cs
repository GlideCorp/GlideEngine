using Core.Logs;
using Engine;
using Engine.Rendering;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor
{
    public class EditorApp : Application
    {
        private ImGuiController? ImGuiController { get; set; }
        private IInputContext? InputContext { get; set; }

        public override void Startup()
        {
            base.Startup();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            InputContext = Window.CreateInput();
            ImGuiController = new ImGuiController(Context, Window, InputContext);

            Logger.Info($"{Context.GetStringS(GLEnum.Vendor)}\n{Context.GetStringS(GLEnum.Version)}\n");
        }

        protected override void OnUpdate(double deltaTime)
        {
            base.OnUpdate(deltaTime);
        }

        protected override void OnRender(double deltaTime)
        {
            Graphics.Clear();

            //Editor Render Loop
            ImGuiController?.Update((float)deltaTime);

            ImGui.ShowDemoWindow();

            ImGuiController?.Render();
        }
    }
}
