using Apos.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Managers
{
    public static class ScreenManager
    {
        public static GraphicsDeviceManager graphics;
        public static Game game;

        private static Vector2 prefferedScreenSize;
        private static Vector2 fullScreenSize;
       
        public static bool isFullScreen;

        private static readonly ICondition fullScreenCondition = new KeyboardCondition(Keys.F11);

        public static Vector2 ScreenSize {
            get { return isFullScreen ? fullScreenSize : prefferedScreenSize; }
            set { prefferedScreenSize = value; SetScreenSettings(); }
        }

        public static void Initialize(Game game_, GraphicsDeviceManager graphics_)
        {
            game = game_;
            graphics = graphics_;

            int x = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            fullScreenSize = new(x, y);
            prefferedScreenSize = new(1600, 900);

            SetScreenSettings();
        }

        public static void SetScreenSettings()
        {
            graphics.PreferredBackBufferWidth = (int)ScreenSize.X;
            graphics.PreferredBackBufferHeight = (int)ScreenSize.Y;
            graphics.IsFullScreen = isFullScreen;
            graphics.ApplyChanges();
        }

        public static void Update()
        {
            if(fullScreenCondition.Pressed()) {
                isFullScreen = !isFullScreen;
                SetScreenSettings();
            }
        }
    }
}