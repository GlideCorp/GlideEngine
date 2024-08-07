using Core.Logs;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering
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

        public static uint CreateProgram(ShaderSource shaderSource)
        {
            uint ProgramID = Application.Context.CreateProgram();
            uint vs = CompileShader(GLEnum.VertexShader, shaderSource.VertexSource);
            uint fs = CompileShader(GLEnum.FragmentShader, shaderSource.FragmentSource);

            Application.Context.AttachShader(ProgramID, vs);
            Application.Context.AttachShader(ProgramID, fs);

            Application.Context.LinkProgram(ProgramID);
            Application.Context.ValidateProgram(ProgramID);

            Application.Context.DeleteShader(vs);
            Application.Context.DeleteShader(fs);

            return ProgramID;
        }

        private static uint CompileShader(GLEnum shaderType, string shaderSource)
        {
            uint id = Application.Context.CreateShader(shaderType);
            Application.Context.ShaderSource(id, shaderSource);
            Application.Context.CompileShader(id);

            //Check for errors
            string result = Application.Context.GetShaderInfoLog(id);

            if (!string.IsNullOrEmpty(result))
            {
                Logger.Error($"{shaderType}\n{result}");

                Application.Context.DeleteShader(id);
                return 0;
            }
            else
            {
                Logger.Info($"{shaderType} compiled succesfully!");
            }

            return id;
        }
    }
}
