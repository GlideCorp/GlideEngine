using Core.Logs;
using Engine.Utilities;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Text;

namespace Engine.Rendering
{
    public struct ShaderSource
    {
        public string VertexSource { get; set; }
        public string FragmentSource { get; set; }

        public ShaderSource(string VertexSource, string FragmentSource)
        {
            this.VertexSource = VertexSource;
            this.FragmentSource = FragmentSource;
        }
    }

    public class Shader : IResource
    {
        private uint ProgramID { get; set; }

        private Dictionary<string, int> UniformLocationCache { get; set; }

        public Shader(ShaderSource shaderSource)
        {
            ProgramID = 0;

            UniformLocationCache = new();

            uint vs = ShaderBuilder.CompileShader(GLEnum.VertexShader, shaderSource.VertexSource, out string vertexError);
            uint fs = ShaderBuilder.CompileShader(GLEnum.FragmentShader, shaderSource.FragmentSource, out string fragmentError);

            if (vs == 0)
            {
                Logger.Error($"VertexShader:\n{vertexError}");
                vs = ShaderBuilder.CompileShader(GLEnum.VertexShader, ShaderBuilder.FallBack.VertexSource, out _);
            }

            if (fs == 0)
            {
                Logger.Error($"FragmentShader: \n{fragmentError}");
                fs = ShaderBuilder.CompileShader(GLEnum.FragmentShader, ShaderBuilder.FallBack.FragmentSource, out _);
            }

            ProgramID = ShaderBuilder.BuildProgram(vs, fs);

            if (ProgramID != 0)
                Logger.Info($"Successfuly created ShaderProgram #{ProgramID}");
        }

        private Shader(uint programID)
        {
            ProgramID = programID;
            UniformLocationCache = new();
        }

        ~Shader()
        {
            Dispose(false);
        }

        public int GetLocation(string uniformName)
        {
            int location;
            if (UniformLocationCache.TryGetValue(uniformName, out location))
            {
                return location;
            }

            location = Application.Context.GetUniformLocation(ProgramID, uniformName);

            if (location == -1)
            {
                Logger.Warning($"Cannot find {uniformName} in ShaderProgram #{ProgramID}");
            }

            UniformLocationCache[uniformName] = location;
            return location;
        }

        public void SetInt(string uniformName, int value)
        {
            Application.Context.Uniform1(GetLocation(uniformName), value);
        }
        public void SetInt(string uniformName, uint value)
        {
            SetInt(uniformName, (int)value);
        }

        public void SetFloat(string uniformName, float value)
        {
            Application.Context.Uniform1(GetLocation(uniformName), value);
        }

        public void SetVector2(string uniformName, Vector2D<float> vector)
        {
            Application.Context.Uniform2(GetLocation(uniformName), vector.ToSystem());
        }

        public void SetVector3(string uniformName, Vector3D<float> vector)
        {
            Application.Context.Uniform3(GetLocation(uniformName), vector.ToSystem());
        }

        public unsafe void SetMatrix4(string uniformName, Matrix4X4<float> matrix)
        {
            Application.Context.UniformMatrix4(GetLocation(uniformName), 1, false, (float*)&matrix);
        }

        public void Use()
        {
            Application.Context.UseProgram(ProgramID);
        }

        public static Shader FromStream(string vertexShaderFileName, string fragmentShaderFileName)
        {
            return FromStream(new FileInfo(vertexShaderFileName), new FileInfo(fragmentShaderFileName));
        }

        public static Shader FromStream(FileInfo vertexShaderFile, FileInfo fragmentShaderFile)
        {
            Logger.Info($"Creating ShaderProgram [vs: {vertexShaderFile.Name}, fg: {fragmentShaderFile.Name}]...");

            ShaderSource shaderSource = ShaderBuilder.Parse(vertexShaderFile, fragmentShaderFile);

            uint vs = ShaderBuilder.CompileShader(GLEnum.VertexShader, shaderSource.VertexSource, out string vertexError);
            uint fs = ShaderBuilder.CompileShader(GLEnum.FragmentShader, shaderSource.FragmentSource, out string fragmentError);

            if (vs == 0)
            {
                Logger.Error($"VertexShader: {vertexShaderFile.Name}\n{vertexError}");
                vs = ShaderBuilder.CompileShader(GLEnum.VertexShader, ShaderBuilder.FallBack.VertexSource, out _);
            }

            if (fs == 0)
            {
                Logger.Error($"FragmentShader: {fragmentShaderFile.Name}\n{fragmentError}");
                fs = ShaderBuilder.CompileShader(GLEnum.FragmentShader, ShaderBuilder.FallBack.FragmentSource, out _);
            }


            uint ID = ShaderBuilder.BuildProgram(vs, fs);

            return new Shader(ID);
        }

        //TODO: Implement better dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose)
        {

            if (ProgramID != 0)
            {
                Application.Context.DeleteProgram(ProgramID);
            }
        }

    }
}
