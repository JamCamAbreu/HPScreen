using HPScreen.Admin;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPScreen.Entities
{
    public class Potter : Follower
    {
        #region Singleton Implementation
        private static Potter instance;
        private static object _lock = new object();
        public static Potter Current
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new Potter(Snitch.Current);
                        }
                    }
                }
                return instance;
            }
        }
        public Potter(MotionSprite followingEntity) : base(followingEntity)
        {
            
        }
        #endregion
        public override float SpriteWidth { get { return 595.0f; } }
        public override float SpriteHeight { get { return 492.0f; } }
        protected override int LAG_ALARM_MIN { get { return 30; } }
        protected override int LAG_ALARM_MAX { get { return 70; } }
        public void Draw()
        {
            if (!Following.InMotion) { return; }

            Graphics.Current.SpriteB.Begin();
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName["potter"], // (Texture2D texture,
                    new Vector2(Xpos, Ypos), // Vector2 position,
                    null,               // Rectangle? sourceRectangle,
                    Color.White,        // Color color,
                    0,      // float rotation,
                    new Vector2(SpriteWidth/2, SpriteHeight/2),       // Vector2 origin,
                    Scale,        // Vector2 scale,
                    Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // SpriteEffects effects,
                    1);                 // float layerDepth)
            Graphics.Current.SpriteB.End();
        }
    }
}
