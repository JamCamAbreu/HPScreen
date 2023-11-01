using HPScreen.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPScreen.Entities
{
    public abstract class MotionSprite : Sprite
    {
        #region Implement

        protected abstract int BeginAlarmMin { get; }
        protected abstract int BeginAlarmMax { get; }
        #endregion

        public MotionSprite() : base()
        {
            InMotion = false;
            ResetMotionBeginAlarm();
        }

        #region Interface
        public bool InMotion { get; set; }
        public virtual void Update()
        {
            if (InMotion)
            {

                this.Scale = Global.Ease(Scale, TargetScale, 0.02f);
                this.Xpos = Global.Ease(Xpos, TargetXPos, 0.05f);
                this.Ypos = Global.Ease(Ypos, TargetYPos, 0.05f);

                UpdateMotion();
                CheckFinished();
            }
            else
            {
                MotionBeginAlarm--;

                if (MotionBeginAlarm < 0)
                {
                    BeginLocation = (BeginLocationValue)Ran.Current.Next(0, 1);
                    SetBeginLocation();
                    SetBeginAttributes();
                    InMotion = true;
                }
            }
        }
        #endregion

        #region Internal
        protected virtual float GetOffscreenPad { get { return SpriteWidth * 2; } }
        protected int MotionBeginAlarm { get; set; }
        protected enum BeginLocationValue
        {
            LeftOffScreen,
            RightOffScreen,
            Position
        }
        protected BeginLocationValue BeginLocation { get; set; }
        protected virtual void SetBeginLocation()
        {
            int randomYPad = 200;
            int randomY = Ran.Current.Next(0 + randomYPad, Graphics.Current.ScreenHeight - randomYPad);
            if (BeginLocation == BeginLocationValue.LeftOffScreen)
            {
                Xpos = -GetOffscreenPad;
                Ypos = randomY;
                TargetXPos = Xpos;
                TargetYPos = Ypos;
                Flipped = false;
            }
            else if (BeginLocation == BeginLocationValue.RightOffScreen)
            {
                Xpos = Graphics.Current.ScreenWidth + GetOffscreenPad;
                Ypos = randomY;
                TargetXPos = Xpos;
                TargetYPos = Ypos;
                Flipped = true;
            }
        }
        protected virtual void SetBeginAttributes()
        {
            Scale = Ran.Current.Next(0.25f, 1.2f);
            TargetScale = Scale;
        }
        protected abstract void UpdateMotion();
        protected virtual void CheckFinished()
        {
            if (BeginLocation == BeginLocationValue.LeftOffScreen)
            {
                if (Xpos > Graphics.Current.ScreenWidth + GetOffscreenPad || Ypos < -GetOffscreenPad || Ypos > Graphics.Current.ScreenHeight + GetOffscreenPad)
                {
                    InMotion = false;
                    ResetMotionBeginAlarm();
                }
            }
            else if (BeginLocation == BeginLocationValue.RightOffScreen)
            {
                if (Xpos < -GetOffscreenPad || Ypos < -GetOffscreenPad || Ypos > Graphics.Current.ScreenHeight + GetOffscreenPad)
                {
                    InMotion = false;
                    ResetMotionBeginAlarm();
                }
            }
        }
        protected virtual void ResetMotionBeginAlarm()
        {
            MotionBeginAlarm = Ran.Current.Next(BeginAlarmMin, BeginAlarmMax);
        }
        #endregion
    }
}
