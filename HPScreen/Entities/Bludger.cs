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
    public class Bludger : MotionSprite
    {
        public override float SpriteWidth { get { return 157; } }
        public override float SpriteHeight { get { return 143; } }
        protected override int BeginAlarmMin { get { return 40; } }
        protected override int BeginAlarmMax { get { return 450; } }
        protected float HorizontalSpeed { get; set; }
        protected float VerticalSpeed { get; set; }
        public Bludger() : base()
        {
                
        }
        public void Draw()
        {
            Graphics.Current.SpriteB.Begin();
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName["bludger"], // (Texture2D texture,
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
        protected override void SetBeginAttributes()
        {
            base.SetBeginAttributes();
            HorizontalSpeed = Ran.Current.Next(80, 120);
            VerticalSpeed = Ran.Current.Next(-5, 5);
        }


        protected override void UpdateMotion()
        {
            if (BeginLocation == BeginLocationValue.LeftOffScreen) { TargetXPos += HorizontalSpeed; }
            else if (BeginLocation == BeginLocationValue.RightOffScreen) { TargetXPos -= HorizontalSpeed; }

            TargetYPos += VerticalSpeed;
        }
    }
}
