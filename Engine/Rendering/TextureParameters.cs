using Silk.NET.OpenGL;

namespace Engine.Rendering
{
    public struct TextureParameters
    {
        public TextureWrap VerticalWrap { get; set; }
        public TextureWrap HorizontalWrap { get; set; }

        public TextureFilter Minification { get; set; }
        public TextureFilter Magnification { get; set; }
            
        public TextureFilter Filters 
        { 
            set => Minification = Magnification = value;
        }

        public bool Mipmaps { get; set; }

        public TextureParameters(TextureWrap verticalWrap, TextureWrap horizontalWrap, TextureFilter minification, TextureFilter magification, bool mipmaps)
        {
            VerticalWrap = verticalWrap;
            HorizontalWrap = horizontalWrap;

            Minification = minification;
            Magnification = magification;

            Mipmaps = mipmaps;
        }

        public void Apply(uint TextureID)
        {
            GL Gl = Application.Context;

            Gl.TextureParameter(TextureID, GLEnum.TextureWrapS, (int)HorizontalWrap);
            Gl.TextureParameter(TextureID, GLEnum.TextureWrapT, (int)VerticalWrap);
            Gl.TextureParameter(TextureID, TextureParameterName.TextureMinFilter, (int)Minification.ToGLEnum(Mipmaps));
            Gl.TextureParameter(TextureID, TextureParameterName.TextureMagFilter, (int)Magnification.ToGLEnum(Mipmaps));
            /*
            //TODO: Generalizzare sta cosa
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)HorizontalWrap);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)VerticalWrap);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)Minification.ToGLEnum(Mipmaps));
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)Magnification.ToGLEnum(Mipmaps));
            */
        }
        
        public static TextureParameters Default = new(verticalWrap: TextureWrap.Clamp,
                                                               horizontalWrap: TextureWrap.Clamp,
                                                               minification: TextureFilter.Linear,
                                                               magification: TextureFilter.Linear,
                                                               mipmaps: false);

        //TODO: Forse è inutile BOOH
        public static TextureParameters DefaultFramBuffer = new(verticalWrap: TextureWrap.Clamp,
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
