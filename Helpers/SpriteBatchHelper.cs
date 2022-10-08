using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Platformer.Managers;
using System;

namespace Platformer.Helpers
{
    public static class SpriteBatchHelper
    {
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, float thickness = 1f, Color color = default)
        {
            if(color == default) color = Color.White;

            Rectangle r = new((int)p1.X, (int)p1.Y, (int)(p2 - p1).Length() + (int)thickness, (int)thickness);
            Vector2 v = Vector2.Normalize(p1 - p2);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if(p1.Y > p2.Y) angle = MathHelper.TwoPi - angle;

            spriteBatch.Draw(ContentLoader.textures[0][0], r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public static void DrawCircle(this SpriteBatch spriteBatch, Vector2 center, Vector2 size, Color color = default)
        {
            if(color == default) color = Color.White;

            Rectangle rect = new((center - size / 2).ToPoint(), size.ToPoint());
            spriteBatch.Draw(ContentLoader.textures[0][0], rect, color);
        }
    }
}