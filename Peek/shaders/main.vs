#version 460 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;

uniform mat4 uView;

void main(void)
{
    gl_Position = uView * vec4(aPosition, 1);
}