using Apos.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Platformer.Base;
using System.Collections.Generic;

namespace Platformer.Managers
{
    public class GameManager
    {
        public readonly Game game;
        public DrawManager drawManager;
        public CameraManager cameraManager;

        private ICondition exitKeys = new AnyCondition(new KeyboardCondition(Keys.Escape));
        private float exitTimer;
        private const float exitTime = .001f;

        List<Tile> tiles;

        public GameManager(Game game)
        {
            this.game = game;

            cameraManager = new(game);
            drawManager = new(game.GraphicsDevice, cameraManager.camera);
        }

        public void LoadContent()
        {
            tiles = new();
            for(int i = 0; i < 20; i++) {
                tiles.Add(new("Grass" + i.ToString(), true) {
                    position = Vector2.UnitX * i * 32
                });
            }

            foreach(Sprite sprite in tiles)
                drawManager.AddPermaSpriteAtLayer(sprite, 1);
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            HandleExit(deltaTime);
            List<CollisionSprite> colTiles = new();
            foreach(CollisionSprite tile in tiles)
                colTiles.Add(tile);
            foreach(var tile in tiles)
                tile.Update(gameTime, colTiles);

            cameraManager.Update(gameTime);
        }

        private void HandleExit(float deltaTime)
        {
            if(exitKeys.Held()) {
                exitTimer += deltaTime;
                if(exitTimer >= exitTime) game.Exit();
            }
            else exitTimer = 0;
        }

        public void Draw(GameTime gameTime)
        {
            drawManager.Draw(gameTime);
        }
    }
}