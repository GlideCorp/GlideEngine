using Core.Logs;
using Engine;
using Engine.Rendering;
using System.Numerics;
using System.Runtime.InteropServices;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using ImGuiNET;
using Shader = Engine.Rendering.Shader;
using System.Text.Unicode;
using System.Text;
using Silk.NET.Maths;
using Silk.NET.Assimp;
using Mesh = Engine.Rendering.Mesh;
using System.Drawing;

namespace Peek
{
    public class PeekApp : Application
    {
        private ImGuiController? ImGuiController { get; set; }
        private IInputContext? InputContext { get; set; }

        private Mesh quadMesh;
        private Shader mainShader;

        private FileInfo vertexShader;
        private FileInfo fragmentShader;

        private Vector3D<float> CameraPosition;
        private Vector3D<float> CameraTarget;
        private Vector3D<float> CameraDirection;
        private Vector3D<float> CameraRight;
        private Vector3D<float> CameraUp;

        private string fragmentSource = "";

        public override void Startup()
        {
            base.Startup();

            CameraPosition = new(0, 0, 1.0f);
            CameraTarget = Vector3D<float>.Zero;
            CameraDirection = Vector3D.Normalize(CameraPosition - CameraTarget);
            CameraRight = Vector3D.Normalize(Vector3D.Cross(Vector3D<float>.UnitY, CameraDirection));
            CameraUp = Vector3D.Cross(CameraDirection, CameraRight);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            InputContext = Window.CreateInput();

            ImGuiFontConfig fontConfig = new("resources\\fonts\\SplineSansMono-Medium.ttf", 16);
            ImGuiController = new ImGuiController(Context, Window, InputContext, fontConfig);

            var io = ImGui.GetIO();
            io.BackendFlags = ImGuiBackendFlags.None;
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

            ImGuiStylePtr imguiStyle = ImGui.GetStyle();
            imguiStyle.WindowRounding = 5;
            imguiStyle.FrameRounding = 2;
            imguiStyle.GrabRounding = 2;

            Logger.Info($"{Context.GetStringS(GLEnum.Vendor)}\n{Context.GetStringS(GLEnum.Version)}\n");

            //Create quadMesh where shader that will be rendered with shader
            List<Vertex> vertices = new List<Vertex>
            {
                new(){ Position = new(0.5f, 0.5f, 0.0f)},
                new(){ Position = new (0.5f, -0.5f, 0.0f)},
                new(){ Position = new (-0.5f, -0.5f, 0.0f)},
                new(){ Position =  new(-0.5f, 0.5f, 0.0f)}
            };

            List<uint> indices = new List<uint>
            {
                0, 1, 3,
                1, 2, 3
            };

            quadMesh = new Mesh(vertices, indices);

            //Shader stuff
            vertexShader = new FileInfo("shaders\\main.vs");
            fragmentShader = new FileInfo("shaders\\main.fg");

            using (var stream = fragmentShader.OpenText())
            {
                fragmentSource = stream.ReadToEnd();
            }

            mainShader = new Shader(vertexShader, fragmentShader);
            mainShader.Use();

        }

        protected override void OnUpdate(double deltaTime)
        {
            base.OnUpdate(deltaTime);

            var size = FramebufferSize;
            Matrix4X4<float> view = Matrix4X4.CreateLookAt(CameraPosition, CameraTarget, CameraUp);
            Matrix4X4<float> proj = Matrix4X4.CreatePerspectiveFieldOfView(MathF.PI / 180f * 60.0f, (float)size.X / size.Y, 0.1f, 100.0f); //TODO: Create MathHelper con Deg2Rad(float deg)->float rads

            mainShader.SetMatrix4("uView", view * proj);
        }

        protected override void OnRender(double deltaTime)
        {
            Graphics.Clear();

            Graphics.Draw(quadMesh, mainShader);

            //Editor Render Loop
            ImGuiController?.Update((float)deltaTime);

            ImGui.BeginMainMenuBar();
            ImGui.Text("Peek - Simple Shader Creator");

            if(ImGui.Button("Reload Shader"))
            {
                using (var stream = fragmentShader.OpenWrite())
                {
                    var data = new UTF8Encoding(false).GetBytes(fragmentSource);
                    stream.Write(data, 0, data.Length);
                }

                mainShader.Reload();
            }
            ImGui.EndMainMenuBar();

            ImGui.Begin("Shader Source");

            ImGui.InputTextMultiline("#source", ref fragmentSource, (uint)1024, ImGui.GetWindowSize(), ImGuiInputTextFlags.AllowTabInput);

            ImGui.ShowDemoWindow();
            ImGui.End();


            ImGuiController?.Render();
        }
    }
}
