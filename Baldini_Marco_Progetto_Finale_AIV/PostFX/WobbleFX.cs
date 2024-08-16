using System;
using Aiv.Fast2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class WobbleFX : PostProcessingEffect
    {
        //GLSL: OpenGL Shading Language
        private static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

uniform float time;

void main(){
    vec2 ux_copy = uv;
    // WAVE FORM: y = A * sin(B * (x + C) ) + D

    float A = 1.f / 100;    //amplitude
    float B = 20.f;         //frequency
    float C = time / 50.f;         //phase
    float D = 0f;         //vertical shift

    ux_copy.x += A * sin(B * (ux_copy.y + C) ) + D;

    vec4 tex_color = texture(tex,ux_copy);
    out_color = tex_color;
}
";
        private float time;
        private float speed;

        public WobbleFX() : base(fragmentShader)
        {
            speed = 10.0f;
        }

        public override void Update(Window window)
        {
            time += window.DeltaTime * speed;
            screenMesh.shader.SetUniform("time", time); //uniform = nell'esecuzione della shader non cambia am può essere cambiata da fuori
        }
    }
}