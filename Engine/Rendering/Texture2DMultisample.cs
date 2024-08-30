using Core.Logs;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering
{
    public class Texture2DMultisample : Texture
    {
        public Texture2DMultisample(int width, int heigth, TextureParameters textureParameters, int samples = 4, SizedInternalFormat internalFormat = SizedInternalFormat.Rgba16)
        {
            if (width <= 0 || heigth <= 0)
            {
                Logger.Warning($"Cannot create texture of size {width}, {heigth}. Size was rescaled to <1, 1>s");

                width = 1;
                heigth = 1;
                //throw new Exception();
            }

            Width = width;
            Height = heigth;
            Params = textureParameters;
            CurrentUnit = 0;

            GL Gl = Application.Context;

            TextureID = Gl.CreateTexture(GLEnum.Texture2DMultisample);
            //Bind();

            //Gl.TextureParameter(TextureID, GLEnum.TextureWrapS, (int)Params.HorizontalWrap);
            //Gl.TextureParameter(TextureID, GLEnum.TextureWrapT, (int)Params.VerticalWrap);
            Gl.TextureStorage2DMultisample(TextureID, (uint)samples, internalFormat, (uint)Width, (uint)Height, true);
            //Gl.TexImage2DMultisample(TextureTarget.Texture2DMultisample, (uint)samples, internalFormat, (uint)Width, (uint)Height, true);
        }

        public Texture2DMultisample(int width, int heigth) : this(width, heigth, TextureParameters.Default) { }

        ~Texture2DMultisample()
        {
            Dispose(false);
        }

        public override void Bind(uint textureUnit = 0)
        {
            if (TextureID <= 0)
            {
                Logger.Error("Cannot bind not initialized Texture");
                return;
            }

            CurrentUnit = textureUnit;
            Application.Context.BindTextureUnit(CurrentUnit, TextureID);
            //Application.Context.ActiveTexture((GLEnum)((uint)GLEnum.Texture0 + CurrentUnit));
            //Application.Context.BindTexture(TextureTarget.Texture2DMultisample, TextureID);
        }
    }
}
