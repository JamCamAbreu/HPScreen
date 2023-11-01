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
    public class Snitch : MotionSprite
    {
        #region Singleton Implementation
        private static Snitch instance;
        private static object _lock = new object();
        public static Snitch Current
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new Snitch();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        public override float SpriteWidth { get { return 139; } }
        public override float SpriteHeight { get { return 89; } }
        protected override int BeginAlarmMin { get { return 560; } }
        protected override int BeginAlarmMax { get { return 1750; } }

        public Snitch() : base()
        {
            BeginLocation = BeginLocationValue.LeftOffScreen;
            MotionType = SnitchMotionTypeValue.zippity;
            ZippityAlarm = 0;
        }

        #region Interface
        public void Draw()
        {
            Graphics.Current.SpriteB.Begin();
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName["snitch"], // (Texture2D texture,
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
        public enum SnitchMotionTypeValue
        {
            constant,
            zippity
        }
        public SnitchMotionTypeValue MotionType { get; set; }
        #endregion

        #region Override
        protected override void SetBeginLocation()
        {
            base.SetBeginLocation();
            Potter.Current.SetAbsolutePosition(Xpos, Ypos);
            Potter.Current.Flipped = this.Flipped;
        }
        protected override void SetBeginAttributes()
        {
            base.SetBeginAttributes();
            Potter.Current.Scale = Scale;
            Potter.Current.TargetScale = Scale;
            MotionType = (SnitchMotionTypeValue)Ran.Current.Next(0, 1);
            ConstantMotionSpeed = Ran.Current.Next(15, 25) * (Scale * 0.75f);
            ResetZippityAlarm();
        }
        protected override void CheckFinished()
        {
            base.CheckFinished();
        }
        #endregion

        #region Internal
        protected float ConstantMotionSpeed { get; set; }
        protected int ZippityAlarm { get; set; }
        
        protected const int ZIPPITY_ALARM_MIN = 2;
        protected const int ZIPPITY_ALARM_MAX = 28;
        protected const int ZIPPITY_WIDTH_MIN = -80;
        protected const int ZIPPITY_WIDTH_MAX = 500;
        protected const int ZIPPITY_HEIGHT_MIN = -300;
        protected const int ZIPPITY_HEIGHT_MAX = 300;
        protected void ResetZippityAlarm()
        {
            ZippityAlarm = Ran.Current.Next(ZIPPITY_ALARM_MIN, ZIPPITY_ALARM_MAX);
        }
        protected override void UpdateMotion()
        {
            switch (MotionType)
            {
                case SnitchMotionTypeValue.constant:
                    if (BeginLocation == BeginLocationValue.LeftOffScreen) { TargetXPos += ConstantMotionSpeed; }
                    else if (BeginLocation == BeginLocationValue.RightOffScreen) { TargetXPos -= ConstantMotionSpeed; }
                    break;

                case SnitchMotionTypeValue.zippity:
                    ZippityAlarm--;
                    if (ZippityAlarm < 0)
                    {
                        ResetZippityAlarm();
                        if (BeginLocation == BeginLocationValue.LeftOffScreen)
                        {
                            TargetXPos += Ran.Current.Next(ZIPPITY_WIDTH_MIN, ZIPPITY_WIDTH_MAX);
                            TargetYPos += Ran.Current.Next(ZIPPITY_HEIGHT_MIN, ZIPPITY_HEIGHT_MAX);
                        }
                        else if (BeginLocation == BeginLocationValue.RightOffScreen)
                        {
                            TargetXPos -= Ran.Current.Next(ZIPPITY_WIDTH_MIN, ZIPPITY_WIDTH_MAX);
                            TargetYPos -= Ran.Current.Next(ZIPPITY_HEIGHT_MIN, ZIPPITY_HEIGHT_MAX);
                        }
                        RollForScale();
                    }

                    break;
            }
        }
        protected void RollForScale()
        {
            float scalechange = Ran.Current.Next(0.05f, 0.5f);
            int scalechance = Ran.Current.Next(0, 100);
            if (scalechance <= 20)
            {
                this.TargetScale -= scalechange;
            }
            else if (scalechance >= 80)
            {
                this.TargetScale += scalechange;
            }
            Potter.Current.TargetScale = this.TargetScale;
        }
        #endregion
    }
}
