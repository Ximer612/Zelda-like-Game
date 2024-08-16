﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class TextObject
    {
        protected List<TextChar> sprites;
        protected string text;
        protected bool isActive;
        protected Font font;
        protected int hSpace;

        protected Vector2 position;
        public Vector2 Position { get => position; set => position = value; }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; UpdateCharStatus(value); }
        }

        public string Text
        {
            get { return text; }
            set { SetText(value); }
        }

        public TextObject(Vector2 position, string textString = "", Font font = null, int horizontalSpace = 0)
        {
            this.position = position;
            hSpace = horizontalSpace;

            if (font == null)
            {
                //get default font
                font = FontMngr.GetFont();
            }

            this.font = font;

            sprites = new List<TextChar>();

            if (textString != "")
            {
                SetText(textString);
            }

            CameraMngr.TextsObjects.Add(this);
        }

        public void SetText(string newText)
        {
            if (newText != text)
            {
                text = newText;
                int numChars = text.Length;
                float charX = position.X;
                float charY = position.Y;

                for (int i = 0; i < numChars; i++)
                {
                    char c = text[i];//string as char array

                    if (i > sprites.Count - 1)//i is greater than last char index
                    {
                        TextChar tc = new TextChar(new Vector2(charX, charY), c, font);
                        tc.IsActive = true;
                        sprites.Add(tc);
                    }
                    else if (c != sprites[i].Character)
                    {
                        //different from previous
                        sprites[i].Character = c;
                    }

                    charX += sprites[i].HalfWidth * 2f + hSpace;
                }

                if (sprites.Count > text.Length)
                {
                    int count = sprites.Count - text.Length;
                    int startCut = text.Length;

                    //destroy GameObjects
                    for (int i = startCut; i < sprites.Count; i++)
                    {
                        sprites[i].Destroy();
                    }

                    sprites.RemoveRange(startCut, count);
                }
            }
        }
        public virtual void SetScaleByZoom(float amount)
        {
            Vector2 pos = Position * amount;

            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Scale = new Vector2(amount);
                sprites[i].Position = pos;
                pos.X += (sprites[i].Width + hSpace) * amount;
            }
        }
        public virtual void SetScale(float amount)
        {
            Vector2 pos = Position;
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Scale = new Vector2(amount);
                sprites[i].Position = pos;
                pos.X += (sprites[i].Width + hSpace) * amount;
            }
        }
        protected virtual void UpdateCharStatus(bool activeStatus)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].IsActive = activeStatus;
            }
        }
        public virtual void Clear()
        {
            if(sprites != null)
            {
                for (int i = 0; i < sprites.Count; i++)
                {
                    sprites[i].Destroy();
                }

                sprites = null;
            }

            text = null;
            CameraMngr.TextsObjects.Remove(this);
        }
        public virtual void Draw()
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Draw();
            }
        }
    }
}
