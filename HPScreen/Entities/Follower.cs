using HPScreen.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPScreen.Entities
{
    public abstract class Follower : MotionSprite
    {
        public MotionSprite Following { get; set; }
        public Follower(MotionSprite followingEntity)
        {
            Following = followingEntity;
            ResetLagAlarm();
        }

        public override void Update()
        {
            if (Following.InMotion)
            {
                Xpos = Global.Ease(Xpos, TargetXPos, 0.05f);
                Ypos = Global.Ease(Ypos, TargetYPos, 0.05f);
                Scale = Global.Ease(Scale, TargetScale, 0.02f);

                LagAlarm--;
                if (LagAlarm < 0)
                {
                    UpdateMotion();
                    ResetLagAlarm();
                }
            }
        }

        #region Internal
        protected int LagAlarm { get; set; }
        protected abstract int LAG_ALARM_MIN { get; }
        protected abstract int LAG_ALARM_MAX { get; }
        protected void ResetLagAlarm()
        {
            LagAlarm = (int)((Ran.Current.Next(LAG_ALARM_MIN, LAG_ALARM_MAX)) * (this.Scale * 0.75f));

        }
        protected override void UpdateMotion()
        {
            this.TargetXPos = Following.Xpos;
            this.TargetYPos = Following.Ypos;
        }
        #endregion

        #region Unused
        protected override int BeginAlarmMin { get { return 40; } } // unused because following
        protected override int BeginAlarmMax { get { return 300; } } // unused because following
        protected override void SetBeginLocation()
        {
            // unused because following
        }
        protected override void SetBeginAttributes()
        {
            // unused because following
        }
        protected override void CheckFinished()
        {
            // unused because following
        }
        #endregion
    }
}
