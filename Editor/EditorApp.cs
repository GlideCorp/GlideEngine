using Core.Logs;
using Editor.Gui;
using Editor.Tools;
using Engine;
using Engine.Rendering;
using ImGuiNET;
using Silk.NET.OpenGL;
using Engine.Entities.Components;
using Engine.Utilities;
using System.Drawing;
using Core.Extensions;
using System.Numerics;
using Editor.resources.materials;
using Core.Maths.Vectors;
using Silk.NET.Maths;

namespace Editor
{

    public class EditorApp : Application
    {
        private ImGuiRenderer? ImGuiRenderer { get; set; }

        Transform transform;
        Camera camera;
        Mesh mTest;
        Texture2D testTexture;
        BasicMaterial objMaterial;

        Vector2Float oldMousePos;
        Vector2Float startMousePos;

        public override void Startup()
        {
            base.Startup();
            Window.Size = new Vector2D<int>(1280, 800);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            Logger.Info($"{Context.GetStringS(StringName.Vendor)} \t {Context.GetStringS(StringName.Version)}");

            ImGuiRenderer = new ImGuiRenderer(Context, Window, InputContext);
            ImGuiRenderer.SetDefaultFont("resources\\fonts\\SplineSansMono-Medium.ttf", 16);


            //Robe di testing-----------------------------------------------------------------------
            transform = new();
            //transform.Scale(new Vector3D<float>(0.25f));
            camera = new()
            {
                Position = new Vector3Float(-2, 2, -3)
            };
            camera.LookAt(Vector3Float.Zero);

            objMaterial = new BasicMaterial
            {
                DiffuseColor = Color.Red,
                Shininess = 2
            };

            mTest = ModelLoader.Load("resources\\models\\cube.glb");

            oldMousePos = Vector2Float.Zero;

            FileStream stream = File.OpenRead("resources\\test.png");
            testTexture = Texture2D.FromStream(stream);
            //Fine robe di testing------------------------------------------------------------------

            WindowManager.Register(new SceneInspector(transform));
            //WindowManager.Register(new TextureMemoryViewer());
            //WindowManager.Register(new InputTester());
            WindowManager.Register(new PerformanceInspector());
            WindowManager.LoadWindowsState();
        }

        protected override void OnUpdate(double deltaTime)
        {
            base.OnUpdate(deltaTime);

            if(ImGui.GetIO().WantCaptureMouse) { return; }

            Vector2Float mouseDelta = Input.MousePosition() - oldMousePos;
            float scrollDelta = Input.MouseScroll();

            if(Input.MouseDown(0))
            {

                float alpha = mouseDelta.X * Time.DeltaTime * 8;
                float beta = -mouseDelta.Y * Time.DeltaTime * 8;

                Vector3Float newPosition = camera.Position.RotateAround(Vector3Float.Zero, alpha, beta);
                Vector3Float originToCamera = (newPosition - Vector3Float.Zero).Normalize();
                float closenessToAxis = originToCamera.Dot(Vector3Float.Zero);
                if (closenessToAxis >= 0.99 || closenessToAxis <= -0.99)
                {
                    return;
                }
                camera.Position = newPosition;
                camera.LookAt(Vector3Float.Zero);
                oldMousePos = Input.MousePosition();
            }
            else if (scrollDelta != 0)
            {
                camera.Position = camera.Position + (camera.Direction * scrollDelta);
            }
            oldMousePos = Input.MousePosition();
        }

        protected override void OnRender(double deltaTime)
        {
            Renderer.Begin(camera);

            Renderer.Draw(mTest, transform.ModelMatrix, objMaterial);

            Renderer.End();


            //Editor Render Loop
            ImGuiRenderer?.BeginLayout(deltaTime);
            ImGui.DockSpaceOverViewport(ImGui.GetMainViewport(), ImGuiDockNodeFlags.PassthruCentralNode);
            ImGui.BeginMainMenuBar();
            ImGui.Text($"{Lucide.Wind} Glide Engine");
            ImGui.Separator();

            foreach (var window in WindowManager.Windows)
            {
                if (ImGui.MenuItem(window.Name))
                {
                    window.Toggle();
                }
            }

            string fpsMenuItem = $"{Lucide.Film} {Time.FPS:D} {Lucide.Dot} {Time.DeltaTime * 1000:N5} ms";
            ImGui.SameLine(ImGui.GetWindowWidth() - ImGui.CalcTextSize(fpsMenuItem).X - 20);

            Vector3 fpsColor = (Time.FPS >= 60 ? Color.LawnGreen : (Time.FPS < 30 ? Color.OrangeRed : Color.LightGoldenrodYellow)).ToVec3();
            fpsColor = Vector3.Normalize(fpsColor);

            ImGui.TextColored(new Vector4(fpsColor, 1)  , fpsMenuItem);
            ImGui.EndMainMenuBar();
            ImGui.DockSpaceOverViewport(ImGui.GetMainViewport(), ImGuiDockNodeFlags.PassthruCentralNode);

            //ImGui.ShowDemoWindow();

            foreach (var window in WindowManager.Windows)
            {
                window.DrawGui();
            }

            ImGuiRenderer?.EndLayout();
        }

        protected override void OnClosing()
        {
            WindowManager.SaveWindowsState();
        }

        protected override void OnFramebufferResize(Vector2D<int> newSize)
        {
            camera.Refresh();
            base.OnFramebufferResize(newSize);
        }
    }
}
