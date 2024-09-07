using Core.Logs;
using Silk.NET.OpenGL;
using StbImageSharp;

namespace Engine.Rendering
{
    public class Texture2D : Texture
    {
        public Texture2D(int width, int heigth, TextureParameters textureParameters, SizedInternalFormat internalFormat = SizedInternalFormat.Rgba16)
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

            TextureID = Gl.CreateTextures(GLEnum.Texture2D, 1); 
            //Bind();

            Params.Apply(TextureID);
            Gl.TextureStorage2D(TextureID, 1, internalFormat, (uint)Width, (uint)Height);
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
            Application.Context.BindTextureUnit(CurrentUnit, TextureID);
            //Application.Context.ActiveTexture((GLEnum)((uint)GLEnum.Texture0 + CurrentUnit));
            //Application.Context.BindTexture(TextureTarget.Texture2D, TextureID);
        }

        public static Texture2D FromStream(Stream stream)
        {
            return FromStream(stream, TextureParameters.Default);
        }

        public static Texture2D FromStream(Stream stream, PixelFormat format = PixelFormat.Rgba)
        {
            return FromStream(stream, TextureParameters.Default, format);
        }

        public static Texture2D FromStream(Stream stream, TextureParameters textureParameters, PixelFormat format = PixelFormat.Rgba)
        {
            ImageResult imageData = ImageResult.FromStream(stream);


            Texture2D texture = new(imageData.Width, imageData.Height, textureParameters);
            texture.SetData(imageData.Data, format);

            return texture;
        }
    }
}
