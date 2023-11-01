using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPScreen.Entities
{
    public class Snape : MotionSprite
    {
        #region Singleton Implementation
        private static Snape instance;
        private static object _lock = new object();
        public static Snape Current
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new Snape();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        public override float SpriteWidth { get { return 287; } }
        public override float SpriteHeight { get { return 441; } }

        protected override int BeginAlarmMin => throw new NotImplementedException();

        protected override int BeginAlarmMax => throw new NotImplementedException();

        public Snape() : base()
        {

        }
        public void Update()
        {

        }
        public void Draw()
        {

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
