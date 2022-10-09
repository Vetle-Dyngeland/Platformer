using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Threading;
using System;
using Apos.Input;
using Microsoft.Xna.Framework.Input;

namespace Platformer.Base
{
    public class CollisionSprite : Sprite
    {
        public Vector2 velocity;
        public Rectangle hitbox;

        public float bounciness = .2f;
        public float friction = .95f;
        public bool shouldCollide = true;
        public bool isStatic;
        public bool useGravity = true;
        public readonly bool[] sidesTouching = new bool[4];
        public readonly float[] sideFrictions = new float[2];
        public const float gravity = 350;

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
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(useGravity && !isStatic)
                velocity.Y += gravity * deltaTime;

            if(shouldCollide)
                Collision(colSprites);

            position += velocity * deltaTime;
        }

        #region Collision

        protected virtual void Collision(List<CollisionSprite> colSprites)
        {
            for(int i = 0; i < sidesTouching.Length; i++)
                sidesTouching[i] = false;
            for(int i = 0; i < sideFrictions.Length; i++)
                sideFrictions[i] = 1f;

            foreach(var sprite in colSprites)
                if(Touching(sprite) && sprite.shouldCollide) Collide(sprite.hitbox, sprite.bounciness, sprite.friction);

            velocity.Y *= sideFrictions[0];
            velocity.X *= sideFrictions[1];
        }
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

        protected virtual void Collide(Rectangle rect, float bounciness = 0f, float friction = 1f)
        {
            if(IsTouchingTop(rect)) sidesTouching[0] = true;
            if(IsTouchingBottom(rect)) sidesTouching[1] = true;
            if(IsTouchingLeft(rect)) sidesTouching[2] = true;
            if(IsTouchingRight(rect)) sidesTouching[3] = true;

            //X collision
            if(sidesTouching[2] || sidesTouching[3]) {
                if(!isStatic) {
                    velocity.X *= -bounciness;
                    if(sideFrictions[1] > friction) sideFrictions[1] = friction;
                }

                if(IsTouchingLeft(rect)) position.X = rect.Left - hitbox.Size.X - 1;
                if(IsTouchingRight(rect)) position.X = rect.Right + 1;
            }

            //Y collision
            if(sidesTouching[0] || sidesTouching[1]) {
                if(!isStatic) {
                    velocity.Y *= -bounciness;
                    if(sideFrictions[0] > friction) sideFrictions[0] = friction;
                }

                if(IsTouchingTop(rect)) position.Y = rect.Top - hitbox.Size.Y - 1;
                if(IsTouchingBottom(rect)) position.Y = rect.Bottom + 1;
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