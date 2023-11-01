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
    public class Hagrid : MotionSprite
    {
        #region Singleton Implementation
        private static Hagrid instance;
        private static object _lock = new object();
        public static Hagrid Current
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new Hagrid();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        public override float SpriteWidth { get { return 470; } }
        public override float SpriteHeight { get { return 777; } }

        protected override int BeginAlarmMin => throw new NotImplementedException();

        protected override int BeginAlarmMax => throw new NotImplementedException();

        public Hagrid() : base()
        {

        }
        public void Update()
        {

        }
        public void Draw()
        {
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName["hagrid"], // (Texture2D texture,
                    new Vector2(Xpos, Ypos), // Vector2 position,
                    null,               // Rectangle? sourceRectangle,
                    Color.White,        // Color color,
                    0,      // float rotation,
                    Vector2.Zero,       // Vector2 origin,
                    Scale,        // Vector2 scale,
                    Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // SpriteEffects effects,
                    1);                 // float layerDepth)
            Graphics.Current.SpriteB.End();
        }

        protected override void SetBeginLocation()
        {
            throw new NotImplementedException();
        }

        protected override void SetBeginAttributes()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateMotion()
        {
            throw new NotImplementedException();
        }

        protected override void CheckFinished()
        {
            throw new NotImplementedException();
        }
    }
}
