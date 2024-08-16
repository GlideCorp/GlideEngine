#version 460 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aUV;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

out vec2 fragTexCoord;
out vec4 fragColor;
out vec3 fragNormal;

void main(void)
{
    // Send vertex attributes to fragment shader
    fragNormal = aNormal;
    gl_Position = uProjection * uView * uModel * vec4(aPosition, 1);
}