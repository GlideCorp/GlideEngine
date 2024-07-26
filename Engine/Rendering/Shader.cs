using Core.Logs;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Text;

namespace Engine.Rendering
{
    public struct ShaderSource
    {
        public string VertexSource { get; set; }
        public string FragmentSource { get; set; }
    }

    public class Shader
    {
        private uint ProgramID { get; set; }

        private FileInfo VertexFile { get; init; }
        private FileInfo FragmentFile { get; init; }

        private Dictionary<string, int> UniformLocationCache { get; set; }

        public Shader(string vertexShaderPath, string fragmentShaderPath)
            :this(new FileInfo(vertexShaderPath), new FileInfo(fragmentShaderPath))
        {
        }

        public Shader(FileInfo vertexShaderFile, FileInfo fragmentShaderFile)
        {
            ProgramID = 0;
            VertexFile = vertexShaderFile;
            FragmentFile = fragmentShaderFile;

            UniformLocationCache = new();

            if (!VertexFile.Exists)
            {
                Logger.Error($"ShaderFile {VertexFile.Name} does not exist!");
                return;
            }

            if (!FragmentFile.Exists)
            {
                Logger.Error($"ShaderFile {VertexFile.Name} does not exist!");
                return;
            }

            Logger.Info($"Creating ShaderProgram [vs: {VertexFile.Name}, fg: {FragmentFile.Name}]...");
            ShaderSource shaderSource = Parse();
            CreateProgram(shaderSource);



            if (ProgramID != 0)
                Logger.Info($"Successfuly created ShaderProgram #{ProgramID}");
        }


        ~Shader()
        {
            if (ProgramID != 0)
            {
                Application.Context.DeleteProgram(ProgramID);
            }
        }

        private ShaderSource Parse()
        {
            ShaderSource source = new();
            StringBuilder builder = new();

            // FEATURE: shader preprocessing
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
            ProgramID = Application.Context.CreateProgram();
            uint vs = CompileShader(GLEnum.VertexShader, shaderSource.VertexSource);
            uint fs = CompileShader(GLEnum.FragmentShader, shaderSource.FragmentSource);

            Application.Context.AttachShader(ProgramID, vs);
            Application.Context.AttachShader(ProgramID, fs);

            Application.Context.LinkProgram(ProgramID);
            Application.Context.ValidateProgram(ProgramID);

            Application.Context.DeleteShader(vs);
            Application.Context.DeleteShader(fs);
        }

        private uint CompileShader(GLEnum shaderType, string shaderSource)
        {
            uint id = Application.Context.CreateShader(shaderType);
            Application.Context.ShaderSource(id, shaderSource);
            Application.Context.CompileShader(id);

            //Check for errors
            string result = Application.Context.GetShaderInfoLog(id);

            if (!string.IsNullOrEmpty(result))
            {
                Logger.Error($"{(shaderType == GLEnum.VertexShader ? VertexFile.Name : FragmentFile.Name)}\n{result}");

                Application.Context.DeleteShader(id);
                return 0;
            }
            else
            {
                Logger.Info($"{(shaderType == GLEnum.VertexShader ? VertexFile.Name : FragmentFile.Name)} compiled succesfully!");
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

        #endregion

        public void Use()
        {
            Application.Context.UseProgram(ProgramID);
        }

        public void Reload()
        {
            Logger.Info($"Reloading ShaderProgram #{ProgramID}");

            uint oldId = ProgramID;
            Application.Context.DeleteProgram(ProgramID);

            ShaderSource shaderSource = Parse();
            CreateProgram(shaderSource);

            UniformLocationCache.Clear();
            Logger.Info($"Successfuly reloaded ShaderProgram #{oldId} -> #{ProgramID}");
        }
    }
}
