using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Platformer.Managers;

namespace Platformer.Base
{
    public class CollisionSprite : Sprite
    {
        public Vector2 velocity;
        public Rectangle hitbox;

        public float bounciness = .2f;

        public bool shouldCollide = true;

        public bool isStatic;

        public readonly bool[] sidesTouching = new bool[4];

        public CollisionSprite(Texture2D texture, Rectangle? sourceRect, Rectangle? hitbox) : base(texture, sourceRect)
        {
            this.hitbox = hitbox == null ? DrawRect : hitbox.Value;
        }

        public CollisionSprite(Sprite sprite) : base(sprite.texture, sprite.sourceRect)
        {
            position = sprite.position;
            size = sprite.size;
            color = sprite.color;
            shouldDraw = sprite.shouldDraw;
            hitbox = sprite.DrawRect;
        }

        public virtual void Update(GameTime gameTime, List<CollisionSprite> colSprites)
        {
            if(isStatic || !shouldCollide) return;

            for(int i = 0; i < sidesTouching.Length; i++)
                sidesTouching[i] = false;

            
            foreach(var sprite in colSprites)
                if(Touching(sprite) && sprite.shouldCollide) Collide(sprite.hitbox, sprite.bounciness);
        }

        #region Collision
        public virtual bool Touching(Rectangle rect)
        {
            if(!shouldCollide) return false;
            if(Vector2.Distance(position, rect.Location.ToVector2()) > (size + rect.Size.ToVector2()).Length() + 1)
                return false;

            if(rect.Location.X < position.X) if(IsTouchingRight(rect)) return true;
            if(rect.Location.X > position.X) if(IsTouchingLeft(rect)) return true;
            if(rect.Location.Y > position.Y) if(IsTouchingTop(rect)) return true;
            if(rect.Location.Y < position.Y) if(IsTouchingBottom(rect)) return true;
            return false;
        }

        public virtual bool Touching(CollisionSprite colSprite) => Touching(colSprite.hitbox);

        protected virtual void Collide(Rectangle rect, float bounciness = 0f)
        {
            if(IsTouchingTop(rect)) {
                velocity.Y *= -bounciness;
                position.Y = rect.Top - hitbox.Size.Y - 1;
                sidesTouching[0] = true;
            }
            if(IsTouchingBottom(rect)) {
                velocity.Y *= -bounciness;
                position.Y = rect.Bottom + 1;
                sidesTouching[1] = true;
            }
            if(IsTouchingLeft(rect)) {
                velocity.X *= -bounciness;
                position.X = rect.Left - hitbox.Size.Y - 1;
                sidesTouching[2] = true;
            }
            if(IsTouchingRight(rect)) {
                velocity.X *= -bounciness;
                position.X = rect.Right + 1;
                sidesTouching[3] = true;
            }
        }

        public virtual bool IsTouchingLeft(Rectangle rect)
        {
            if(!shouldCollide) return false;

            return hitbox.Right + velocity.X > rect.Left &&
                    hitbox.Left < rect.Left &&
                    hitbox.Bottom > rect.Top &&
                    hitbox.Top < rect.Bottom;
        }

        public virtual bool IsTouchingRight(Rectangle rect)
        {
            if(!shouldCollide) return false;

            return hitbox.Left + velocity.X < rect.Right &&
                    hitbox.Right > rect.Right &&
                    hitbox.Bottom > rect.Top &&
                    hitbox.Top < rect.Bottom;
        }

        public virtual bool IsTouchingTop(Rectangle rect)
        {
            if(!shouldCollide) return false;

            return hitbox.Bottom + velocity.Y > rect.Top &&
                    hitbox.Top < rect.Top &&
                    hitbox.Right > rect.Left &&
                    hitbox.Left < rect.Right;
        }

        public virtual bool IsTouchingBottom(Rectangle rect)
        {
            if(!shouldCollide) return false;

            return hitbox.Top + velocity.Y < rect.Bottom &&
                    hitbox.Bottom > rect.Bottom &&
                    hitbox.Right > rect.Left &&
                    hitbox.Left < rect.Right;
        }
        #endregion Collision
    }
}