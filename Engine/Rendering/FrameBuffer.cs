﻿using Core.Logs;
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

        public Texture2D Color { get; private set; }
        public Texture2D Depth { get; private set; }

        //TODO: Stencil Attachment if needed

        public FrameBuffer(int width, int heigth, TextureParameters textureParameters)
        {
            if (width <= 0 || heigth <= 0)
            {
                Logger.Error($"Cannot create FrameBuffer of size {width}, {heigth}");
                throw new Exception();
            }

            Width = width;
            Height = heigth;
            Params = textureParameters;

            GL Gl = Application.Context;

            FrameBufferID = Gl.GenFramebuffer();
            Bind();

            Color = new Texture2D(Width, Height, textureParameters);
            Color.SetData(null, TextureFormat.RGB);
            Gl.FramebufferTexture2D(GLEnum.Framebuffer, GLEnum.ColorAttachment0, GLEnum.Texture2D, Color.TextureID, 0);
            
            Depth = new Texture2D(Width, Height, textureParameters);
            Depth.SetData(null, TextureFormat.DEPTH);
            Gl.FramebufferTexture2D(GLEnum.Framebuffer, GLEnum.DepthAttachment, GLEnum.Texture2D, Depth.TextureID, 0);

            GLEnum status = Gl.CheckFramebufferStatus(GLEnum.Framebuffer);
            CheckStatus(status);
            //Params.Apply(TextureID);
        }

        public FrameBuffer(int width, int heigth) : this(width, heigth, TextureParameters.Default) { }

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

        void CheckStatus(GLEnum status)
        {
            switch (status)
            {
                case GLEnum.FramebufferComplete: Logger.Info("FBO Complete"); break;
                case GLEnum.FramebufferIncompleteAttachment: Logger.Error("FBO Incomplete: Attachment"); break;
                case GLEnum.FramebufferIncompleteMissingAttachment: Logger.Error("FBO Incomplete: Missing Attachment"); break;
                case GLEnum.FramebufferIncompleteDrawBuffer: Logger.Error("FBO Incomplete: Draw Buffer"); break;
                case GLEnum.FramebufferIncompleteReadBuffer: Logger.Error("FBO Incomplete: Read Buffer"); break;
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
