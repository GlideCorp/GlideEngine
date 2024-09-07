using Engine.Rendering.Effects;
using Engine.Entities.Components;
using Core.Logs;
using Core.Maths.Vectors;
using Core.Maths.Matrices;


namespace Engine.Rendering
{
    public static class Renderer
    {
        public static FrameBuffer? FrameBuffer { get; private set; }

        static UniformBuffer GlobalUniformBuffer;
        static Matrix4x4 ViewMatrix;
        static Matrix4x4 ProjectionMatrix;

        //                              Questa tripla orribile è temporanea, una volta stabilito un entity component system cambierà
        static Dictionary<Shader, List<(Mesh, Material, Matrix4x4)>> DrawCalls;

        public static void Startup()
        {
            Vector2Int frameBufferSize = Application.FramebufferSize;
            FrameBuffer = new FrameBuffer(frameBufferSize.X, frameBufferSize.Y, true);

            GlobalUniformBuffer = new UniformBuffer((16 * 4 * 2) + 16, 0); //2 di 4 Vector4D
        }

        public static void Begin(Camera camera) => Begin(camera.View, camera.Projection);
        public static void Begin(Matrix4x4 view, Matrix4x4 projection)
        {
            if(FrameBuffer == null)
            {
                Logger.Error("Renderer was not started!");
                throw new Exception("UninitializedRenderer");
            }

            ViewMatrix = view;
            ProjectionMatrix = projection;
            DrawCalls.Clear();
        }

        public static void Draw(Mesh mesh, Matrix4x4 transform, Material material)
        {
            if(DrawCalls.TryGetValue(material.Shader, out var drawCalls))
            {
                drawCalls.Add((mesh, material, transform));
                return;
            }

            DrawCalls[material.Shader] = [(mesh, material, transform)];
        }

        public static void End()
        {
            if (FrameBuffer == null)
            {
                Logger.Warning("Renderer was not started!");
                return;
            }
            FrameBuffer.Bind();
            Graphics.Clear();

            //TODO: Migliorare radicalmente come viene settato lo uniform buffer. Non è una priorità alta sta cosa perchè tanto vengono usati solo qui.
            GlobalUniformBuffer.SetMatrix4(0, ViewMatrix);
            GlobalUniformBuffer.SetMatrix4(16*4, ProjectionMatrix);

            Vector4Float camPos = ViewMatrix.GetRow(3);
            GlobalUniformBuffer.SetVec4(16*4*2, camPos);

            //Sta cosa al momento non ha senso ma ok!
            foreach ((Shader shad, List<(Mesh, Material, Matrix4x4)> objs) in DrawCalls)
            {
                shad.Use();

                shad.SetVector3("uLigthPos", new Vector3Float(2, 2, 1));
                shad.SetVector3("uLigthColor", new Vector3Float(1, 1, 1));
                shad.SetVector3("uAmbientColor", new Vector3Float(0.79f, 0.94f, 0.97f));

                foreach ((Mesh mesh, Material material, Matrix4x4 transform) in objs)
                {
                    shad.SetMatrix4("uModel", transform);
                    material.ApplyProperties();
                    Graphics.Draw(mesh);
                }
            }

            PostProcessing.Execute();
            FrameBuffer.Unbind();

            ViewMatrix = Matrix4x4.Identity;
            ProjectionMatrix = Matrix4x4.Identity;
        }

        internal static void ResizeMainBuffer(Vector2Int size)
        {
            FrameBuffer?.Dispose();
            FrameBuffer = new FrameBuffer(size.X, size.Y, true);
            PostProcessing.ResizeBuffers(size);
        }

        static Renderer()
        {
            DrawCalls = new Dictionary<Shader, List<(Mesh, Material, Matrix4x4)>>();
        }
    }
}
