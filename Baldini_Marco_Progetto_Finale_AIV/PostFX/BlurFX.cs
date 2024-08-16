using System;
using Aiv.Fast2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class BlurFX : PostProcessingEffect
    {
        //GLSL: OpenGL Shading Language
        private static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

uniform float pixW, pixH;

void main(){
    vec4 tex_color = texture(tex,uv);

    vec2 uv_right = vec2(uv.x + pixW,uv.y);
    vec4 tex_right = texture(tex, uv_right);

    vec2 uv_left = vec2(uv.x - pixW,uv.y);
    vec4 tex_left = texture(tex, uv_left);

    vec2 uv_top = vec2(uv.x,uv.y + pixH);
    vec4 tex_top = texture(tex, uv_top);

    vec2 uv_bottom = vec2(uv.x,uv.y - pixH);
    vec4 tex_bottom = texture(tex, uv_bottom);

    vec2 uv_topr = vec2(uv.x + pixW,uv.y + pixH);
    vec4 tex_topr = texture(tex, uv_topr);

    vec2 uv_topl = vec2(uv.x - pixW,uv.y + pixH);
    vec4 tex_topl = texture(tex, uv_topl);

    vec2 uv_bottomr = vec2(uv.x + pixW,uv.y - pixH);
    vec4 tex_bottomr = texture(tex, uv_bottomr);

    vec2 uv_bottoml = vec2(uv.x - pixW,uv.y - pixH);
    vec4 tex_bottoml = texture(tex, uv_bottoml);

    out_color = (tex_color + tex_right + tex_left + tex_top + tex_bottom + tex_topr + tex_topl + tex_bottomr + tex_bottoml) * 0.1111f;

}
";
        public BlurFX(float blurAmount) : base(fragmentShader)
        {
            float pixelWidth = 1.0f / Game.Window.Width;
            float pixelHeight = 1.0f / Game.Window.Height;

            screenMesh.shader.SetUniform("pixW", pixelWidth* blurAmount);
            screenMesh.shader.SetUniform("pixH", pixelHeight* blurAmount);
        }
    }
}