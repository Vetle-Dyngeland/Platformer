using Apos.Camera;
using Apos.Input;
using Microsoft.Xna.Framework;
using Platformer.Base;
using Platformer.Helpers;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace Platformer.Managers
{
    public class CameraManager
    {
        public Camera camera;

        public bool useDebugMovement = true;

        public CollisionSprite followSprite;
        private readonly List<Vector2> oldPositions = new();
        public float lookaheadTime = .2f;
        public int lookaheadSmoothing = 25;
        private Vector2 oldPositionAverage = Vector2.Zero;

        private readonly KeyboardCondition[] moveConditions = new KeyboardCondition[] {
            new(Keys.W), new(Keys.S), new(Keys.A), new(Keys.D)
        };
        private const float debugMoveSpeed = 500;

        public CameraManager(Game game)
        {
            IVirtualViewport defaultViewport = new DefaultViewport(game.GraphicsDevice, game.Window);
            camera = new(defaultViewport);
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(useDebugMovement) DebugMovement(deltaTime);
            else FollowSprite(deltaTime);
        }

        private void DebugMovement(float deltaTime)
        {
            Vector2 moveVector = new(
                Convert.ToInt16(moveConditions[2].Held()) - Convert.ToInt16(moveConditions[3].Held()),
                Convert.ToInt16(moveConditions[0].Held()) - Convert.ToInt16(moveConditions[1].Held()));

            List<Vector2> movementAverageList = new() { -moveVector.Normalized(), -moveVector };
            camera.XY += movementAverageList.Average() * debugMoveSpeed * deltaTime;
        }

        private void FollowSprite(float deltaTime)
        {
            oldPositions.Add(followSprite.position + followSprite.velocity * deltaTime * lookaheadTime);
            if(oldPositions.Count > lookaheadSmoothing) oldPositions.RemoveAt(0);

            Vector2 addPos = oldPositions.Average() - oldPositionAverage;
            camera.XY += addPos;
            oldPositionAverage = oldPositions.Average();
        }
    }
}