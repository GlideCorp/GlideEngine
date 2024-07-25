using Core.Logs;
using Silk.NET.OpenGL;
using StbImageSharp;
using System.IO;
using System.Runtime.InteropServices;

namespace Engine.Rendering
{
    //https://stackoverflow.com/questions/538060/proper-use-of-the-idisposable-interface dicono di usarlo
    public class Texture2D : IResource
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public uint TextureID { get; private set; }

        //TODO: Add Format
        public Texture2D(int width, int heigth)
        {
            if(width <= 0 || heigth <= 0)
            {
                Logger.Error($"Cannot create texture of size {width}, {heigth}");
                throw new Exception();
            }

            Width = width;
            Height = heigth;

            GL Gl = Application.Context;

            TextureID = Gl.GenTexture();
            Bind();

            //TODO: Generalizzare sta cosa
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Nearest);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Nearest);
        }

        ~Texture2D()
        {
            Dispose(false);
        }

        public unsafe void SetData(Span<byte> data)
        {
            Bind();

            //fixed(byte* ptr = MemoryMarshal.AsBytes(data))
            fixed(byte* ptr = &data[0])
            {
                Application.Context.TexImage2D(GLEnum.Texture2D,
                                               0,
                                               InternalFormat.Rgba,
                                               (uint)Width,
                                               (uint)Height,
                                               0,
                                               PixelFormat.Rgba,
                                               GLEnum.UnsignedByte,
                                               (void*)ptr);
            }
        }

        public void Bind(TextureUnit textureUnit = TextureUnit.Texture0)
        {
            if(TextureID <= 0)
            {
                Logger.Error("Cannot bind not initialized Texture2D.");
                return;
            }

            Application.Context.ActiveTexture(textureUnit);
            Application.Context.BindTexture(TextureTarget.Texture2D, TextureID);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose)
        {

        }

        public static Texture2D FromStream(Stream stream)
        {
            ImageResult imageData = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

            Texture2D texture = new Texture2D(imageData.Width, imageData.Height);
            texture.SetData(imageData.Data);

            return texture;
        }
    }
}
