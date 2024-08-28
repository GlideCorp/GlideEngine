using Core.Logs;
using Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utilities
{
    public static class ShaderDatabase
    {
        static readonly string SHADERS_FOLDER = "resources\\shaders\\";
        static Dictionary<string, Shader> LoadedShaders;

        static ShaderDatabase()
        {
            LoadedShaders = new Dictionary<string, Shader>();
        }

        public static Shader Load(string shaderName)
        {
            if(LoadedShaders.TryGetValue(shaderName, out Shader? shader))
            {
                return shader;
            }

            if(!Directory.Exists(SHADERS_FOLDER + shaderName))
            {
                Logger.Warning($"Could not find {shaderName} shader.");
            }

            FileInfo fragmentShader = new FileInfo($"{SHADERS_FOLDER}{shaderName}\\{shaderName}.fg");
            FileInfo vertexShader = new FileInfo($"{SHADERS_FOLDER}{shaderName}\\{shaderName}.vs");

            Shader newShader = Shader.FromStream(vertexShader, fragmentShader);
            LoadedShaders.TryAdd(shaderName, newShader);
            return newShader;
        }

        public static void Clear()
        {
            LoadedShaders.Clear();
        }
    }
}
