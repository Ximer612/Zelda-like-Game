using System;
using Aiv.Fast2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class NegativeFX : PostProcessingEffect
    {
        //GLSL: OpenGL Shading Language
        private static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

void main(){
    vec4 tex_color = texture(tex,uv);

    out_color = vec4(1 - tex_color.r, 1-tex_color.g,1-tex_color.b,tex_color.a);
}
";
        public NegativeFX() : base(fragmentShader)
        {
        }
    }
}