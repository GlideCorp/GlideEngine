using Core.Logs;
using Engine.Rendering;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utilities
{
    public static class ShaderBuilder
    {
        /*
        ~Shader()
        {
            if (ProgramID != 0)
            {
                Application.Context.DeleteProgram(ProgramID);
            }
        }
        */

        public static ShaderSource Parse(FileInfo vertexShader, FileInfo fragmentShader)
        {
            if (!vertexShader.Exists)
            {
                Logger.Error($"ShaderFile {vertexShader.Name} does not exist!");
                return default;
            }

            if (!fragmentShader.Exists)
            {
                Logger.Error($"ShaderFile {fragmentShader.Name} does not exist!");
                return default;
            }

            ShaderSource source = new();
            StringBuilder builder = new();

            // FEATURE: shader preprocessing
            using (StreamReader stream = vertexShader.OpenText())
            {
                string s = "";
                while ((s = stream.ReadLine()) != null)
                {
                    builder.AppendLine(s);
                }

                source.VertexSource = builder.ToString();
            }

            builder.Clear();
            using (StreamReader stream = fragmentShader.OpenText())
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

        public static uint CompileShader(GLEnum shaderType, string shaderSource, out string shaderError)
        {
            uint id = Application.Context.CreateShader(shaderType);
            Application.Context.ShaderSource(id, shaderSource);
            Application.Context.CompileShader(id);

            //Check for errors
            shaderError = Application.Context.GetShaderInfoLog(id);

            if (!string.IsNullOrEmpty(shaderError))
            {
                Application.Context.DeleteShader(id);
                return 0;
            }
            else
            {
                return id;
            }
        }

        public static uint BuildProgram(uint vsId, uint fsId)
        {
            uint ProgramID = Application.Context.CreateProgram();

            Application.Context.AttachShader(ProgramID, vsId);
            Application.Context.AttachShader(ProgramID, fsId);

            Application.Context.LinkProgram(ProgramID);
            Application.Context.ValidateProgram(ProgramID);

            Application.Context.DeleteShader(vsId);
            Application.Context.DeleteShader(fsId);

            return ProgramID;
        }
    }
}
