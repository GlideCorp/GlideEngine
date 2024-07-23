using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class ShaderDefaults
    {
        public static ShaderSource Default = new()
        {
            VertexSource = """
            #version 460 core 
            layout (location = 0) in vec3 aPosition;

            void main(void)
            {
                gl_Position = vec4(aPosition, 1);
            }
            """,
            
            FragmentSource = """
            #version 460 core
            out vec4 oColor;

            void main(void)
            {
                oColor = vec4(1, 0, 0, 1);
            }
            """
        };
    }
}
