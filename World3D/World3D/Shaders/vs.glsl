#version 330

in  vec3 vPosition;
out vec4 color;
uniform mat4 modelview;

void
main()
{
    gl_Position = modelview * vec4(vPosition, 1.0);
    color = vec4( vPosition, 1.0);
}