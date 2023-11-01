using System;
using HPScreen.Admin;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPScreen.Entities
{
    public class Quaffle : MotionSprite
    {
        #region Singleton Implementation
        private static Quaffle instance;
        private static object _lock = new object();
        public static Quaffle Current
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new Quaffle();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        public override float SpriteWidth { get { return 105; } }
        public override float SpriteHeight { get { return 95; } }
        protected override int BeginAlarmMin { get { return 40; } }
        protected override int BeginAlarmMax { get { return 160; } }
        protected override float GetOffscreenPad { get { return 2800; } }
        protected float HorizontalSpeed { get; set; }
        protected float VerticalSpeed { get; set; }
        public Quaffle() : base()
        {
            BeginLocation = BeginLocationValue.RightOffScreen;
        }
        public void Draw()
        {
            Graphics.Current.SpriteB.Begin();
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName["quaffle"], // (Texture2D texture,
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
        #region Override
        protected override void SetBeginLocation()
        {
            base.SetBeginLocation();
            QuidditchTeam.Current.SetAbsolutePosition(Xpos, Ypos);
            QuidditchTeam.Current.SetFlipped(this.Flipped);
        }
        protected override void SetBeginAttributes()
        {
            base.SetBeginAttributes();
            QuidditchTeam.Current.SetAbsoluteScale(Scale);
            HorizontalSpeed = Ran.Current.Next(15, 35) * (Scale * 0.75f);
            VerticalSpeed = Ran.Current.Next(-4, 4) * (Scale * 0.75f);
            QuidditchTeam.Current.RandomizeVisibility();
        }
        protected override void UpdateMotion()
        {
            if (BeginLocation == BeginLocationValue.LeftOffScreen) { TargetXPos += HorizontalSpeed; }
            else if (BeginLocation == BeginLocationValue.RightOffScreen) { TargetXPos -= HorizontalSpeed; }

            TargetYPos += VerticalSpeed;
        }
        #endregion
    }
}
