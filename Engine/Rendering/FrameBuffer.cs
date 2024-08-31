using Core.Logs;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering
{
    public class FrameBuffer : IResource
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public uint FrameBufferID { get; private set; }

        public TextureParameters Params { get; private set; }

        public Texture Color { get; private set; }
        public Texture Depth { get; private set; }

        //TODO: Stencil Attachment if needed

        public FrameBuffer(int width, int heigth, TextureParameters textureParameters, bool multiSample = false)
        {
            if (width < 0 || heigth < 0)
            {
                Logger.Error($"Cannot create FrameBuffer of size {width}, {heigth}");
                throw new Exception();
            }

            Width = width;
            Height = heigth;
            Params = textureParameters;

            GL Gl = Application.Context;

            FrameBufferID = Gl.CreateFramebuffer();
            //Bind();

            if(multiSample)
            {
                Color = new Texture2DMultisample(Width, Height, textureParameters, 4, SizedInternalFormat.Rgb32f);
                Gl.NamedFramebufferTexture(FrameBufferID, GLEnum.ColorAttachment0, Color.TextureID, 0);
            }
            else
            {
                Color = new Texture2D(Width, Height, textureParameters, SizedInternalFormat.Rgb32f);
                //Color.SetData(new Span<byte>([]));
                Gl.NamedFramebufferTexture(FrameBufferID, GLEnum.ColorAttachment0, Color.TextureID, 0);
            }

            if (multiSample)
            {
                Depth = new Texture2DMultisample(Width, Height, textureParameters, 4, SizedInternalFormat.DepthComponent24);
                Gl.NamedFramebufferTexture(FrameBufferID, GLEnum.DepthAttachment, Depth.TextureID, 0);
            }
            else
            {
                Depth = new Texture2D(Width, Height, textureParameters, SizedInternalFormat.DepthComponent24);
                //Depth.SetData(new Span<byte>([]), PixelFormat.DepthComponent, PixelType.Float);
                Gl.NamedFramebufferTexture(FrameBufferID, GLEnum.DepthAttachment, Depth.TextureID, 0);
            }

            GLEnum status = Gl.CheckNamedFramebufferStatus(FrameBufferID, GLEnum.Framebuffer);
            CheckStatus(status);
        }

        public FrameBuffer(int width, int heigth, bool multiSample = false) : this(width, heigth, TextureParameters.Default, multiSample) { }

        ~FrameBuffer()
        {
            Dispose(false);
        }

        public void Bind()
        {
            if (FrameBufferID <= 0)
            {
                Logger.Error("Cannot bind not initialized FrameBuffer.");
                return;
            }

            Application.Context.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferID);
        }

        public void Unbind()
        {
            Application.Context.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        void CheckStatus(GLEnum status)
        {
            switch (status)
            {
                case GLEnum.FramebufferComplete: Logger.Info("FBO Complete"); break;
                case GLEnum.FramebufferIncompleteAttachment: Logger.Error("FBO Incomplete: Attachment"); break;
                case GLEnum.FramebufferIncompleteMissingAttachment: Logger.Error("FBO Incomplete: Missing Attachment"); break;
                case GLEnum.FramebufferIncompleteDrawBuffer: Logger.Error("FBO Incomplete: Draw Buffer"); break;
                case GLEnum.FramebufferIncompleteReadBuffer: Logger.Error("FBO Incomplete: Read Buffer"); break;
                case GLEnum.FramebufferIncompleteMultisample: Logger.Error("FBO Incomplete Multisample"); break;
                case GLEnum.FramebufferUnsupported: Logger.Error("FBO Unsupported"); break;
                case GLEnum.FramebufferUndefined: Logger.Error("FBO Unsupported"); break;
                default: Logger.Warning("Undefined FBO error"); break;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool dispose)
        {
            if (FrameBufferID != 0)
            {
                Color.Dispose();
                Depth.Dispose();

                Logger.Info($"Deleted FrameBuffer #{FrameBufferID}");
                Application.Context.DeleteFramebuffer(FrameBufferID);
            }
        }
    }
}
