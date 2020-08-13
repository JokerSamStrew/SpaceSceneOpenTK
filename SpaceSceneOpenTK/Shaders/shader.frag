#version 330


out vec4 outputColor;
in vec2 texCoord;
uniform sampler2D texture0;
uniform float time;
vec4 move;

void main()
{
    move = texture(texture0, texCoord);
	move.x = abs(sin(time));
	move.y = abs(cos(time));
	move.z = (cos(time));
	outputColor = move;
}