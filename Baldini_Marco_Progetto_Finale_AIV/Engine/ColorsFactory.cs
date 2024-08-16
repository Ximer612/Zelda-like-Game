using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
        enum ColorType { Black, Red, Green, Blue, Purple, Orange, White, Magenta, Gray, Yellow, Cyan, LAST }
    static class ColorsFactory
    {
        private static Vector4[] colors;

        static ColorsFactory()
        {
            colors = new Vector4[(int)ColorType.LAST];

            Vector4 color = Vector4.Zero;

            for (int i = 0; i < colors.Length; i++)
            {
                switch ((ColorType)i)
                {

                    case ColorType.Red:
                        color.X = 1;
                        color.Y = 0;
                        color.Z = 0;
                        break;
                    case ColorType.Magenta:
                        color.X = 1;
                        color.Y = 0;
                        color.Z = 1;
                        break;
                    case ColorType.Gray:
                        color.X = 0.2f;
                        color.Y = 0.2f;
                        color.Z = 0.2f;
                        break;
                    case ColorType.Yellow:
                        color.X = 1;
                        color.Y = 1;
                        color.Z = 0;
                        break;
                    case ColorType.Cyan:
                        color.X = 0;
                        color.Y = 1;
                        color.Z = 1;
                        break;
                    case ColorType.White:
                        color.X = 1;
                        color.Y = 1;
                        color.Z = 1;
                        break;
                    case ColorType.Green:
                        color.X = 0;
                        color.Y = 1;
                        color.Z = 0;
                        break;
                    case ColorType.Blue:
                        color.X = 0;
                        color.Y = 0;
                        color.Z = 1;
                        break;
                    case ColorType.Purple:
                        color.X = 0.7f;
                        color.Y = 0;
                        color.Z = 1;
                        break;
                    case ColorType.Orange:
                        color.X = 1;
                        color.Y = 0.6f;
                        color.Z = 0;
                        break;
                    default:
                        color.X = 0;
                        color.Y = 0;
                        color.Z = 0;
                        break;
                }

                color.W = 1;
                colors[i] = color;
            }
        }

        static public Vector4 GetColor(ColorType type)
        {
            return colors[(int)type];
        }

        static public Vector4 GetColor(int type)
        {
            return colors[type];
        }
    }
}
