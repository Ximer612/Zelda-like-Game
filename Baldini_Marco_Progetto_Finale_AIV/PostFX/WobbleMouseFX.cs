using System;
using Aiv.Fast2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class WobbleMouseFX : PostProcessingEffect
    {
        //GLSL: OpenGL Shading Language
        private static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

uniform float time;
uniform vec3 mouse;

void main(){
    vec2 ux_copy = uv;

    vec2 mouse_diff = mouse.xy - uv.xy;
    float mouse_dist = length(mouse_diff) * 0.5f;

    float ray = 1.f - clamp(mouse_dist * 10.f, 0.f,1.f);

    // WAVE FORM: y = A * sin(B * (x + C) ) + D

    float A = 1.f / 100;    //amplitude
    float B = 20.f * ray;         //frequency
    float C = time / 50.f;         //phase
    float D = 0f;         //vertical shift

    ux_copy.x += A * sin(B * (ux_copy.y + C) ) + D;

    vec4 tex_color = texture(tex,ux_copy);
    //out_color = tex_color * ray;
    out_color = tex_color;
}
";
        private float time;
        private float speed;

        public WobbleMouseFX() : base(fragmentShader)
        {
            speed = 5.0f;
        }

        public override void Update(Window window)
        {
            time += window.DeltaTime * speed;
            screenMesh.shader.SetUniform("time", time); //uniform = nell'esecuzione della shader non cambia am può essere cambiata da fuori

            Vector2 mouse = window.MousePosition;
            mouse.X /= window.OrthoWidth;
            mouse.Y = 1.0f - (mouse.Y / window.OrthoHeight);
            screenMesh.shader.SetUniform("mouse", new Vector3(mouse.X,mouse.Y,0)); //uniform = nell'esecuzione della shader non cambia am può essere cambiata da fuori

        }
    }
}