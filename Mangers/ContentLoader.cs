using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Platformer.Managers
{
    public static class ContentLoader
    {
        private static ContentManager Content { get; set; }

        public readonly static List<List<Texture2D>> textures = new();
        public readonly static List<List<SpriteFont>> fonts = new();

        public static Dictionary<string, int> fontDict;
        public static Dictionary<string, int> textureDict;

        public static void LoadContent(ContentManager content)
        {
            Content = content;
            InitializeDictionaries();

            for(int i = 24; i < 35; i += 2) 
                LoadFont("Arial", i);
            LoadFont("Arial", 60);

            LoadTexture("Test", "whitePixel");
            LoadTexture("Test", "circle");

            LoadTexture("Tiles", "tileset");
        }

        private static void InitializeDictionaries()
        {
            fontDict = new() {
                { "Arial", 0 }
            };

            textureDict = new() {
                { "Test", 0 },
                { "Tiles", 1 }
            };
        }

        private static void LoadTexture(string folderName, string textureName)
        {
            int listIndex = textureDict[folderName];
            while(listIndex >= textures.Count) textures.Add(new());
            textures[listIndex].Add(Content.Load<Texture2D>($"Sprites/{folderName}/{textureName}"));
        }

        private static void LoadFont(string fontName, float size)
        {
            int listIndex = fontDict[fontName];
            while(listIndex >= fonts.Count) fonts.Add(new());
            try { fonts[listIndex].Add(Content.Load<SpriteFont>($"Fonts/{fontName}/{fontName}{MathF.Round(size, 4)}")); }
            catch(Exception) { fonts[listIndex].Add(Content.Load<SpriteFont>($"Fonts/{fontName}/{MathF.Round(size, 4)}")); }
        }
    }
}