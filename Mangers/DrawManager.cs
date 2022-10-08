using Apos.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Base;
using System.Collections.Generic;

namespace Platformer.Managers
{
    public class DrawManager
    {
        private readonly GraphicsDevice device;

        private readonly SpriteBatch spriteBatch;

        public readonly List<List<Sprite>> sprites = new();
        public readonly List<Sprite> bgSprites = new();

        public readonly List<List<Sprite>> permaSprites = new();
        public readonly List<Sprite> permaBgSprites = new();

        private readonly Camera camera;

        private readonly RasterizerState rasterizer = RasterizerState.CullNone;
        private readonly SamplerState samplerState = SamplerState.PointClamp;

        public Color bgColor = Color.CornflowerBlue;

        public DrawManager(GraphicsDevice device, Camera camera)
        {
            this.device = device;
            this.camera = camera;
            spriteBatch = new(device);
        }

        public void Draw(GameTime gameTime)
        {
            device.Clear(bgColor);

            DrawBackground();
            DrawNormals();

            UpdateLists();

            if(gameTime != default) return;
        }

        private void DrawBackground()
        {
            spriteBatch.Begin(transformMatrix: camera.GetView(-1), rasterizerState: rasterizer, samplerState: samplerState);

            foreach(var sprite in bgSprites)
                sprite.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void DrawNormals()
        {
            spriteBatch.Begin(transformMatrix: camera.View, rasterizerState: rasterizer, samplerState: samplerState);

            foreach(var layer in sprites)
                foreach(var sprite in layer)
                    sprite.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void UpdateLists()
        {
            bgSprites.Clear();
            foreach(var sprite in permaBgSprites) bgSprites.Add(sprite);

            sprites.Clear();
            for(int i = 0; i < permaSprites.Count; i++)
                for(int j = 0; j < permaSprites[i].Count; j++)
                    AddSpriteAtLayer(permaSprites[i][j], i);
        }

        public void AddSpriteAtLayer(Sprite sprite, int layer)
        {
            while(layer >= sprites.Count) sprites.Add(new());
            sprites[layer].Add(sprite);
        }

        public void AddPermaSpriteAtLayer(Sprite sprite, int layer)
        {
            while(layer >= permaSprites.Count) permaSprites.Add(new());
            permaSprites[layer].Add(sprite);
        }
    }
}