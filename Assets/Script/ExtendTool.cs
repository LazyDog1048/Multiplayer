using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameThing
{
    public static class ExtendTool
    {
        public static Sprite ToSprite( this Texture2D self )
        {
            var rect      = new Rect( 0, 0, self.width, self.height );
            var pivot     = Vector2.one * 0.5f;
            var newSprite = Sprite.Create( self, rect, pivot );

            return newSprite;
        }
    }
    
}
