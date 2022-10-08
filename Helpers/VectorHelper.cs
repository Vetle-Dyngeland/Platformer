using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Platformer.Helpers
{
    public static class VectorHelper
    { 
        public static Vector2 Normalized(this Vector2 v)
        {
            if(v.X != 0) v.X /= v.Length();
            if(v.Y != 0) v.Y /= v.Length();
            return v;
        }

        public static Vector2 Average(this List<Vector2> v)
        {
            Vector2 sum = Vector2.Zero;
            foreach(var vector in v)
                sum += vector;
            return sum / v.Count;
        }
    }
}