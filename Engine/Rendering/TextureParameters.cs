using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering
{
    public struct TextureParameters
    {
        public TextureWrap VerticalWrap { get; set; }
        public TextureWrap HorizontalWrap { get; set; }

        public TextureFilter Minification { get; set; }
        public TextureFilter Magnification { get; set; }

        public bool Mipmaps { get; set; }

        public TextureParameters(TextureWrap verticalWrap, TextureWrap horizontalWrap,  TextureFilter minification, TextureFilter magification, bool mipmaps)
        {
            VerticalWrap = verticalWrap;
            VerticalWrap = horizontalWrap;

            Minification = minification;
            Magnification = magification;

            Mipmaps = mipmaps;
        }

        public void Apply(uint TextureID)
        {
            GL Gl = Application.Context;

            //TODO: Generalizzare sta cosa
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)HorizontalWrap);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)VerticalWrap);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)Minification.ToGLEnum(Mipmaps));
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)Magnification.ToGLEnum(Mipmaps));
        }

        public static TextureParameters Default = new(verticalWrap: TextureWrap.Clamp,
                                                               horizontalWrap: TextureWrap.Clamp,
                                                               minification: TextureFilter.Linear,
                                                               magification: TextureFilter.Linear,
                                                               mipmaps: false);
    }

    public enum TextureWrap
    {
        Repeat = GLEnum.Repeat,
        MirroredRepeat = GLEnum.MirroredRepeat,
        Clamp = GLEnum.ClampToEdge
    }

    public enum TextureFilter
    {
        Nearest,
        Linear
    }

    internal static class TextureFilterExtensions
    {
        public static GLEnum ToGLEnum(this TextureFilter filter, bool mipMap)
        {
            return filter switch
            {
                TextureFilter.Nearest => (mipMap) ? GLEnum.NearestMipmapNearest : GLEnum.Nearest,
                TextureFilter.Linear => (mipMap) ? GLEnum.LinearMipmapLinear : GLEnum.Linear,
                _ => GLEnum.Linear,
            };
        }
    }
}
