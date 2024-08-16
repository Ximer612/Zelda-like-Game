using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class BlackBandUnderGUIFX : PostProcessingEffect
    {
        //GLSL: OpenGL Shading Language
        private static string fragmentShader = @"
            #version 330 core

            in vec2 uv;
            uniform sampler2D tex;
            out vec4 out_color;

            void main(){

                vec4 color1 = texture (tex, uv) * max (0, sign(0.9 - uv.y));
                
                out_color = vec4(color1.rgb,1);
                }
            ";
        public BlackBandUnderGUIFX() : base(fragmentShader)
        {
        }
    }
}
