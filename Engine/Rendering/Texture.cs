using Core.Logs;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering
{
    public enum TextureFormat
    {
        R = GLEnum.R8,
        RG = GLEnum.RG,
        RGB = GLEnum.Rgb,
        RGBA = GLEnum.Rgba,
        DEPTH = GLEnum.DepthComponent24,
        DEPTH_STENCIL = GLEnum.Depth24Stencil8
    }

    public abstract class Texture : IResource
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public uint TextureID { get; protected set; }
        public uint CurrentUnit { get; protected set; }
        public TextureParameters Params { get; protected set; }

        public virtual void Bind(uint textureUnit = 0)
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

        public virtual unsafe void SetData(Span<byte> data, TextureFormat internalFormat = TextureFormat.RGBA, PixelFormat format = PixelFormat.Rgba, PixelType pixelType = PixelType.UnsignedByte)
        {
            Bind();

            if (data.IsEmpty)
            {
                Application.Context.TexImage2D(GLEnum.Texture2D,
                                               0,
                                               (int)internalFormat,
                                               (uint)Width,
                                               (uint)Height,
                                               0,
                                               format,
                                               pixelType,
                                               (void*)null);
                return;
            }

            //fixed(byte* ptr = MemoryMarshal.AsBytes(data))
            fixed (byte* ptr = &data[0])
            {
                Application.Context.TexImage2D(GLEnum.Texture2D,
                                               0,
                                               (int)internalFormat,
                                               (uint)Width,
                                               (uint)Height,
                                               0,
                                               format,
                                               pixelType,
                                               (void*)ptr);
            }

            if (Params.Mipmaps)
            {
                Application.Context.GenerateMipmap(TextureTarget.Texture2D);
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (TextureID != 0)
            {
                Application.Context.DeleteTexture(TextureID);
                Logger.Info($"Deleted Texture #{TextureID}");
                TextureID = 0;
            }
        }
    }
}
