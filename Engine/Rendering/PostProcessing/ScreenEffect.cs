using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering.PostProcessing
{
    public abstract class ScreenEffect
    {
        private ScreenMaterial MainMaterial { get; set; }

        protected ScreenEffect() 
        {
            ShaderSource screenDraw = new(GetVertex(), GetFragment());
            MainMaterial = new ScreenMaterial(new(screenDraw));
        }

        protected ScreenEffect(string vertexShader, string fragmentShader)
        {
            Shader shader = Shader.FromStream(vertexShader, fragmentShader);
            MainMaterial = new ScreenMaterial(shader);
        }

        public virtual void RenderImage(FrameBuffer source, FrameBuffer destination)
        {
            MainMaterial.ScreenBuffer = source;
            Graphics.Blit(source, destination, MainMaterial);
        }

        protected virtual string GetVertex()
        {
            return """
                    #version 460 core

                    layout (location = 0) in vec3 aPosition;
                    layout (location = 2) in vec2 aUV;

                    out vec2 fragTexCoord;

                    void main(void)
                    {
                        // Send vertex attributes to fragment shader
                        fragTexCoord = (aPosition.xy + 1.0) / 2.0;
                        gl_Position = vec4(aPosition.xy, 0.0, 1.0);
                    }

                """;
        }

        protected virtual string GetFragment()
        {
            return """
                #version 460 core

                in vec2 fragTexCoord;
                
                uniform sampler2D uColorBuffer;
                uniform sampler2D uDepthBuffer;
                
                out vec4 oColor;

                void main(void)
                {	
                    oColor = texture2D(uColorBuffer, fragTexCoord);
                }
                """;
        }
    }

    public class ScreenDrawEffect : ScreenEffect
    {
        protected override string GetFragment()
        {
            return """
                #version 460 core

                in vec2 fragTexCoord;

                uniform sampler2D uColorBuffer;
                uniform sampler2D uDepthBuffer;

                out vec4 oColor;

                void main(void)
                {
                    oColor = texture2D(uColorBuffer, fragTexCoord);
                }
                """;
        }
    }

    public class NegativeEffect : ScreenEffect
    {
        protected override string GetFragment()
        {
            return """
                #version 460 core

                in vec2 fragTexCoord;

                uniform sampler2D uColorBuffer;
                uniform sampler2D uDepthBuffer;

                out vec4 oColor;

                void main(void)
                {	
                    oColor = 1-texture2D(uColorBuffer, fragTexCoord);
                }
                """;
        }
    }
    public class DrawDepthEffect : ScreenEffect
    {
        protected override string GetFragment()
        {
            return """
                #version 460 core
                
                in vec2 fragTexCoord;
                
                uniform sampler2D uColorBuffer;
                uniform sampler2D uDepthBuffer;
                
                out vec4 oColor;
                
                float linearize_depth(float d,float zNear,float zFar)
                {
                    return (2.0 * zNear * zFar) / (zFar + zNear - d * (zFar - zNear));	
                }
                
                void main(void)
                {	
                    float depth = texture2D(uDepthBuffer, fragTexCoord).r * 2.0 - 1.0;
                    float linear = linearize_depth(depth, 0.1, 100) / 100;
                    oColor = vec4(vec3(linear.rrr), 1);//texture2D(uColorBuffer, fragTexCoord);
                }
                """;
        }
    }

    public class SimpleFogEffect : ScreenEffect
    {
        protected override string GetFragment()
        {
            return """
                #version 460 core
                
                in vec2 fragTexCoord;
                
                uniform sampler2D uColorBuffer;
                uniform sampler2D uDepthBuffer;
                
                out vec4 oColor;
                
                float linearize_depth(float d,float zNear,float zFar)
                {
                    return (2.0 * zNear * zFar) / (zFar + zNear - d * (zFar - zNear));	
                }
                
                vec4 fogColor = vec4(0.52, 0.81, 1, 1);

                void main(void)
                {	
                    float depth = texture2D(uDepthBuffer, fragTexCoord).r * 2.0 - 1.0;
                    float linearDepth = linearize_depth(depth, 0.1, 100)/100;

                    vec4 color = texture2D(uColorBuffer, fragTexCoord);
                    oColor = mix(color, fogColor, linearDepth);
                }
                """;
        }
    }
}
