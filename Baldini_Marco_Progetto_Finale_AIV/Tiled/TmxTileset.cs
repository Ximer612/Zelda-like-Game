﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    struct TileOffset
    {
        public int X;
        public int Y;

        public TileOffset(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class TmxTileset
    {
        public int Width  { get; }
        public int Height { get; }

        private TileOffset[] tiles;
        public string TextureName { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }

        private int spacing;
        private int margin;
        private int cols;
        private int rows;

        public TmxTileset(string textureName, int cols, int rows, int tileW, int tileH, int spacing, int margin)
        {
            tiles = new TileOffset[cols * rows];

            TextureName = textureName;
            TileWidth = tileW;
            TileHeight = tileH;
            this.cols = cols;
            this.rows = rows;
            this.spacing = spacing;
            this.margin = margin;


            Width = tileW * cols;
            Height = tileH * rows;

            int xOff = margin;
            int yOff = margin;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    tiles[i * cols + j] = new TileOffset(xOff, yOff);

                    xOff += TileWidth + spacing;
                }

                xOff = margin;
                yOff += TileHeight + spacing;
            }
        }

        public TileOffset GetAtIndex(int index)
        {
            return tiles[index - 1];
        }
    }
}
