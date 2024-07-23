using Silk.NET.OpenGL;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Engine
{
    public struct ShaderSource
    {
        public string VertexSource { get; set; }
        public string FragmentSource { get; set; }
    }

    public class Shader
    {
        private uint ProgramID {  get; set; }

        private FileInfo VertexFile {  get; init; }
        private FileInfo FragmentFile { get; init; }

        private Dictionary<string, int> UniformLocationCache {  get; set; }
 
        public Shader(string vertexShaderPath, string fragmentShaderPath)
        {
            ProgramID = 0;
            VertexFile = new(vertexShaderPath);
            FragmentFile = new(fragmentShaderPath);

            UniformLocationCache = new();

            //TODO: TODO
            if (!VertexFile.Exists)
            {
                Console.WriteLine(Path.GetFullPath(vertexShaderPath));
                return;
            }

            if (!FragmentFile.Exists)
            {
                Console.WriteLine(Path.GetFullPath(fragmentShaderPath));
                return;
            }

            ShaderSource shaderSource = Parse();
            CreateProgram(shaderSource);

            //TODO: TODO
            Console.Write(shaderSource.VertexSource);
            Console.Write("\n\n\n\n\n\n");
            Console.Write(shaderSource.FragmentSource);
        }

        public Shader (ShaderSource shaderSource)
        {
            ProgramID = 0;
            UniformLocationCache = new();

            CreateProgram(shaderSource);

            //TODO: TODO
            Console.Write(shaderSource.VertexSource);
            Console.Write("\n\n\n\n\n\n");
            Console.Write(shaderSource.FragmentSource);
        }

        ~Shader()
        {
            if(ProgramID != 0)
            {
                GL Gl = App.Gl;
                Gl.DeleteProgram(ProgramID);
            }
        }

        private ShaderSource Parse()
        {
            ShaderSource source = new();
            StringBuilder builder = new();

            using (StreamReader stream = VertexFile.OpenText())
            {
                string s = "";
                while ((s = stream.ReadLine()) != null)
                {
                    builder.AppendLine(s);
                }

                source.VertexSource = builder.ToString();
            }

            builder.Clear();
            using (StreamReader stream = FragmentFile.OpenText())
            {
                string s = "";
                while ((s = stream.ReadLine()) != null)
                {
                    builder.AppendLine(s);
                }

                source.FragmentSource = builder.ToString();
            }

            return source;
        }

        private void CreateProgram(ShaderSource shaderSource)
        {
            GL Gl = App.Gl;

            ProgramID = Gl.CreateProgram();
            uint vs = CompileShader(GLEnum.VertexShader, shaderSource.VertexSource);
            uint fs = CompileShader(GLEnum.FragmentShader, shaderSource.FragmentSource);
        
            Gl.AttachShader(ProgramID, vs);
            Gl.AttachShader(ProgramID, fs);

            Gl.LinkProgram(ProgramID);
            Gl.ValidateProgram(ProgramID);

            Gl.DeleteShader(vs);
            Gl.DeleteShader(fs);
        }

        private uint CompileShader(GLEnum shaderType, string shaderSource)
        {
            GL Gl = App.Gl;

            uint id = Gl.CreateShader(shaderType);
            Gl.ShaderSource(id, shaderSource);
            Gl.CompileShader(id);

            //Check for errors
            string result = Gl.GetShaderInfoLog(id);

            if (!string.IsNullOrEmpty(result))
            {
                //Log Error

                Gl.DeleteShader(id);
                return 0;
            }
            else
            {
                //Log Info -> "Compiled successfully vertexShderFileName ecc...
            }

            return id;
        }

        #region Uniform Setters

        public int GetLocation(string uniformName)
        {
            int location;
            if (UniformLocationCache.TryGetValue(uniformName, out location))
            {
                return location;
            }

            GL Gl = App.Gl;
            location = Gl.GetUniformLocation(ProgramID, uniformName);

            if(location == -1)
            {
                //LogWarning -> Uniform blahblah in shaders pincoPallo.vs e pincoPallo.fg dosent exist!
            }

            UniformLocationCache[uniformName] = location;
            return location;
        }

        public void SetInt(string uniformName, int value)
        {
            GL Gl = App.Gl;
            Gl.Uniform1(GetLocation(uniformName), value);
        }

        public void SetFloat(string uniformName, float value)
        {
            GL Gl = App.Gl;
            Gl.Uniform1(GetLocation(uniformName), value);
        }

        public void SetVector2(string uniformName, Vector2 vector)
        {
            GL Gl = App.Gl;
            Gl.Uniform2(GetLocation(uniformName), vector);
        }

        public void SetVector3(string uniformName, Vector3 vector)
        {
            GL Gl = App.Gl;
            Gl.Uniform3(GetLocation(uniformName), vector);
        }

        public unsafe void SetMatrix4(string uniformName, Matrix4x4 matrix)
        {
            GL Gl = App.Gl;
            Gl.UniformMatrix4(GetLocation(uniformName), 1, false, (float*) &matrix);
        }

        #endregion

        public void Use()
        {
            GL Gl = App.Gl;
            Gl.UseProgram(ProgramID);
        }

        public void Reload()
        {
            if(VertexFile == null || FragmentFile == null)
            {
                //LogWarning -> Reloads are not possible for static Shaders
                return;
            }

            GL Gl = App.Gl;

            //LogInfo -> Reloading Shader ...

            Gl.DeleteProgram(ProgramID);

            ShaderSource shaderSource = Parse();
            CreateProgram(shaderSource);

            UniformLocationCache.Clear();
        }
    }
}
