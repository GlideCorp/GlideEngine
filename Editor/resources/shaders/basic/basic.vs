#version 460 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aUV;

layout(std140, binding = 0) uniform BaseData
{
    mat4 uView;
    mat4 uProjection;

    vec4 uCameraWorldPos;
};

uniform mat4 uModel;

out vec3 fragWorldPos;
out vec3 fragNormal;
out vec2 fragTexCoord;

mat4 WorldToObject = inverse(uModel);

vec3 ObjectToWorldNormal(vec3 objectNormal)
{
    return normalize(
        WorldToObject[0].xyz * objectNormal.x +
        WorldToObject[1].xyz * objectNormal.y +
        WorldToObject[2].xyz * objectNormal.z
    );
}

void main(void)
{
    // Send vertex attributes to fragment shader
    vec4 verPosVS = uView * uModel * vec4(aPosition, 1.0);
    fragWorldPos = vec3(verPosVS);
    fragNormal = mat3(transpose(inverse(uModel))) * aNormal;

    gl_Position = uProjection * uView * uModel * vec4(aPosition, 1);
}