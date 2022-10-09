using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Platformer.Managers;

namespace Platformer.Base
{
    public class Tile : CollisionSprite
    {
        private static int tileSize = 32;
        private const int sourceTileSize = 16;

        public static Dictionary<string, Vector2> sourceRectDict;
        public static Dictionary<string, int> TextureIndexDict;

        public static Dictionary<string, float> bouncinessDict;
        public static Dictionary<string, float> frictionDict;

        public readonly string tileIndex;

        public Tile(string tileIndex, bool hasLoadedContent = false, int? _tileSize = null) : base(null, null, null)
        {
            this.tileIndex = tileIndex;
            if(_tileSize != null) tileSize = _tileSize.Value;

            if(sourceRectDict == null || TextureIndexDict == null || 
                bouncinessDict == null || frictionDict == null)
                InitializeDictionaries();

            sourceRect = new(sourceRectDict[tileIndex].ToPoint(), new(sourceTileSize));
            if(hasLoadedContent) LoadContent();

            friction = frictionDict[tileIndex];
            bounciness = bouncinessDict[tileIndex];

            size = Vector2.One * tileSize;

            isStatic = true;
            shouldCollide = true;
        }

        public void LoadContent()
            => texture ??= ContentLoader.textures[1][TextureIndexDict[tileIndex]];

        private static void InitializeDictionaries()
        {
            //Textures
            TextureIndexDict = new();
            for(int i = 0; i < 20; i++) 
                TextureIndexDict.Add($"Grass{i}", 0);

            //Source Rects
            sourceRectDict = new();

            int index = 0;
            index = AddRectangleToSourceRects("Grass", index, new(0, 0), new(2, 2));
            index = AddRectangleToSourceRects("Grass", index, new(3, 0), new(5, 2), hollow: true);
            index = AddRectangleToSourceRects("Grass", index, new(6, 3), new(8, 3));

            //Frictions
            frictionDict = new();
            for(int i = 0; i < 20; i++)
                frictionDict.Add($"Grass{i}", .94f);

            //Bouncinesses
            bouncinessDict = new();
            for(int i = 0; i < 20; i++)
                bouncinessDict.Add($"Grass{i}", 0);
        }

        private static int AddRectangleToSourceRects(string name, int startIndex, Vector2 startVector, Vector2 endVector, bool hollow = false)
        {
            int index = startIndex;
            Vector2 size = endVector - startVector;
            for(int y = 0; y <= size.Y; y++) 
                for(int x = 0; x <= size.X; x++) {
                    if(hollow && x == 1 && y == 1) continue;
                    sourceRectDict.Add($"{name}{index++}", (startVector + new Vector2(x, y)) * sourceTileSize);
                }

            return index;
        }
    }
}