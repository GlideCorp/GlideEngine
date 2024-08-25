using Core.Logs;
using Silk.NET.OpenGL;
using StbImageSharp;
using System.IO;
using System.Runtime.InteropServices;

namespace Engine.Rendering
{
    public class Texture2D : Texture
    {
        public Texture2D(int width, int heigth, TextureParameters textureParameters)
        {
            if(width <= 0 || heigth <= 0)
            {
                Logger.Warning($"Cannot create texture of size {width}, {heigth}. Size was rescaled to <1, 1>");
                
                width = 1;
                heigth = 1;
                //throw new Exception();
            }

            Width = width;
            Height = heigth;
            Params = textureParameters;
            CurrentUnit = 0;

            GL Gl = Application.Context;

            TextureID = Gl.GenTexture();
            Bind();
            
            Params.Apply(TextureID);
        }

        public Texture2D(int width, int heigth) : this(width, heigth, TextureParameters.Default) { }

        ~Texture2D()
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
            Application.Context.ActiveTexture((GLEnum)((uint)GLEnum.Texture0 + CurrentUnit));
            Application.Context.BindTexture(TextureTarget.Texture2D, TextureID);
        }

        public static Texture2D FromStream(Stream stream)
        {
            return FromStream(stream, TextureParameters.Default);
        }

        public static Texture2D FromStream(Stream stream, TextureFormat format)
        {
            return FromStream(stream, TextureParameters.Default, format);
        }

        public static Texture2D FromStream(Stream stream, TextureParameters textureParameters, TextureFormat format = TextureFormat.RGB)
        {
            ImageResult imageData = ImageResult.FromStream(stream);


            Texture2D texture = new(imageData.Width, imageData.Height, textureParameters);
            texture.SetData(imageData.Data, format);

            return texture;
        }
    }
}
