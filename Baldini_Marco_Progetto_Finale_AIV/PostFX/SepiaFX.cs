using System;
using Aiv.Fast2D;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class SepiaFX : PostProcessingEffect
    {
        //GLSL: OpenGL Shading Language
        private static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

void main(){
    vec4 tex_color = texture(tex,uv);

/*
    float gray = (tex_color.r + tex_color.b + tex_color.g) * 0.333f;
    out_color = vec4(gray * 0.5f, gray * 0.6f, gray * 0.2f, 1);
*/

    //quando questi valori si discostano dal grigio
    float gray = dot(tex_color.rgb, vec3(0.299f, 0.587f, 0.114f));
    out_color = vec4(gray, gray * 0.95f, gray * 0.82f, tex_color.a);

}
";
        public SepiaFX() : base(fragmentShader)
        {
        }
    }
}