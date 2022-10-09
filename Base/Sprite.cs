using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Platformer.Base
{
    public class Sprite
    {
        public Texture2D texture;

        public Vector2 position;
        public Vector2 size = Vector2.One * 50;

        public Color color = Color.White;

        public Rectangle? sourceRect;
        public Rectangle DrawRect {
            get { return new(position.ToPoint(), size.ToPoint()); }
        }

        public bool shouldDraw = true;

        public Sprite(Texture2D texture, Rectangle? sourceRect = null)
        {
            this.texture = texture;
            this.sourceRect = sourceRect;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(!shouldDraw) return;
            try { spriteBatch.Draw(texture, DrawRect, sourceRect, color); }
            catch(Exception) { throw new($"texture: {texture}, DrawRect: {DrawRect}, color: {color}"); }
        }
    }
}