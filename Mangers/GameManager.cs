using Apos.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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

        public GameManager(Game game)
        {
            this.game = game;

            cameraManager = new(game);
            drawManager = new(game.GraphicsDevice, cameraManager.camera);
        }

        public void LoadContent()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            HandleExit(deltaTime);

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