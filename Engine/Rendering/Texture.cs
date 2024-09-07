using Core.Logs;
using Silk.NET.OpenGL;

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
            Application.Context.BindTextureUnit(CurrentUnit, TextureID);
            //Application.Context.ActiveTexture((GLEnum)((uint)GLEnum.Texture0 + CurrentUnit));
            //Application.Context.BindTexture(TextureTarget.Texture2D, TextureID);
        }

        public virtual unsafe void SetData(Span<byte> data, PixelFormat format = PixelFormat.Rgba, PixelType pixelType = PixelType.UnsignedByte)
        {
            //Bind();

            if (data.IsEmpty)
            {
                Application.Context.TextureSubImage2D(TextureID,
                                            0,
                                            0,
                                            0,
                                            (uint)Width,
                                            (uint)Height,
                                            format,
                                            pixelType,
                                            (void*)null);

                return;
            }

            //fixed(byte* ptr = MemoryMarshal.AsBytes(data))
            fixed (byte* ptr = &data[0])
            {
                Application.Context.TextureSubImage2D(TextureID,
                                               0,
                                               0,
                                               0,
                                               (uint)Width,
                                               (uint)Height,
                                               format,
                                               pixelType,
                                               (void*)ptr);
            }

            if (Params.Mipmaps && !data.IsEmpty)
            {
                Application.Context.GenerateTextureMipmap(TextureID);
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
