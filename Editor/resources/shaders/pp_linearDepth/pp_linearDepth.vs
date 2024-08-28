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