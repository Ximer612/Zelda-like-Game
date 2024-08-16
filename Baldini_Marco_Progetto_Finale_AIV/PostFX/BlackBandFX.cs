using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class BlackBandFX : PostProcessingEffect
    {
        //GLSL: OpenGL Shading Language
        private static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

/*
void main(){
    vec4 tex_color = texture(tex,uv);

    vec4 color = vec4(1.f,1.f,1.f,1.f);
    if(uv.x <= 0.3f)
{
    color = vec4(1.f,0.f,0.f,0.5f);
}
else
    if(uv.x >= 0.7f)
{
    color = vec4(0.f,0.f,1.f,0.5f);
}


    //out_color = tex_color * color;
    out_color = mix(tex_color,color,0.2f);
}
*/

void main(){
    vec4 tex_color = texture(tex,uv);

    float value = 0.1f;

    if(uv.y < value || uv.y > 1-value){
        out_color = vec4(0.f,0.f,0.f,0.f);
    } else {
        out_color = tex_color;
    }
}
";
        public BlackBandFX() : base(fragmentShader)
        {
        }
    }
}
