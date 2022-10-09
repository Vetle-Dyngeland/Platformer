using Apos.Input;
using Microsoft.Xna.Framework;
using Platformer.Managers;

namespace Platformer.System
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private GameManager gameManager;

        public Game1()
        {
            graphics = new(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            ScreenManager.Initialize(this, graphics);
        }

        protected override void Initialize()
        {
            gameManager = new(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            InputHelper.Setup(this);
            ContentLoader.LoadContent(Content);
            gameManager.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            InputHelper.UpdateSetup();

            ScreenManager.Update();
            gameManager.Update(gameTime);

            base.Update(gameTime);
            InputHelper.UpdateCleanup();
        }

        protected override void Draw(GameTime gameTime)
        {
            gameManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}