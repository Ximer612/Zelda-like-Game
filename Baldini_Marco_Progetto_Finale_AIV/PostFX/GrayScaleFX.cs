using System;
using Aiv.Fast2D;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class GrayScaleFX : PostProcessingEffect
    {
        //GLSL: OpenGL Shading Language
        private static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

void main(){
    vec4 tex_color = texture(tex,uv);

    float gray = (tex_color.r + tex_color.b + tex_color.g) * 0.333f;

    out_color = vec4(gray,gray,gray,1);
}
";
        public GrayScaleFX() : base(fragmentShader)
        {
        }
    }
}
