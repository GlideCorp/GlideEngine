using Core.Logs;
using Silk.NET.OpenGL;
using StbImageSharp;
using System.IO;
using System.Runtime.InteropServices;

namespace Engine.Rendering
{
    public enum TextureFormat
    {
        R = GLEnum.R8,
        RG = GLEnum.RG,
        RGB = GLEnum.Rgb,
        RGBA = GLEnum.Rgba,
        DEPTH = GLEnum.Depth,
        DEPTH_STENCIL = GLEnum.DepthStencil
    }

    //https://stackoverflow.com/questions/538060/proper-use-of-the-idisposable-interface dicono di usarlo
    public class Texture2D : IResource
    {

        public int Width { get; private set; }
        public int Height { get; private set; }

        public uint TextureID { get; private set; }

        public TextureParameters Params { get; private set; }

        //TODO: Add Format
        public Texture2D(int width, int heigth, TextureParameters textureParameters)
        {
            if(width <= 0 || heigth <= 0)
            {
                Logger.Error($"Cannot create texture of size {width}, {heigth}");
                throw new Exception();
            }

            Width = width;
            Height = heigth;
            Params = textureParameters;

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

        public unsafe void SetData(Span<byte> data, TextureFormat format = TextureFormat.RGB)
        {
            Bind();

            //fixed(byte* ptr = MemoryMarshal.AsBytes(data))
            fixed(byte* ptr = &data[0])
            {
                Application.Context.TexImage2D(GLEnum.Texture2D,
                                               0,
                                               (int)format,
                                               (uint)Width,
                                               (uint)Height,
                                               0,
                                               (GLEnum)PixelFormat.Rgba,
                                               GLEnum.UnsignedByte,
                                               (void*)ptr);
            }

            if(Params.Mipmaps)
            {
                Application.Context.GenerateMipmap(TextureTarget.Texture2D);
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
            if(TextureID != 0)
            {
                Application.Context.DeleteTexture(TextureID);
                Logger.Info($"Deleted Texture #{TextureID}");
                TextureID = 0;
            }
        }

        public static Texture2D FromStream(Stream stream)
        {
            ImageResult imageData = ImageResult.FromStream(stream);

            Texture2D texture = new Texture2D(imageData.Width, imageData.Height);
            texture.SetData(imageData.Data);

            return texture;
        }
    }
}
