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
    public class Voldy : Sprite
    {
        #region Singleton Implementation
        private static Voldy instance;
        private static object _lock = new object();
        public static Voldy Current
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new Voldy();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        public override float SpriteWidth { get { return 502; } }
        public override float SpriteHeight { get { return 529; } }
        public int HallucinateTimer { get; set; }
        public bool IsAnimating { get; set; }
        public int AnimationIndex { get; set; }
        public const int ANIMATION_INDEX_MAX = 50;
        public int AnimationIndexTimer { get; set; }
        public Vector2 AnimationOriginPoint { get; set; }
        public float SpiralPositionIndex { get; set; }
        public Voldy() : base()
        {
            ResetAnimation();
            IsAnimating = false;
            AnimationIndex = 0;
            AnimationIndexTimer = 0;
            AnimationOriginPoint = Vector2.Zero;
        }
        public void Update()
        {
            HallucinateTimer--;
            if (HallucinateTimer == 0)
            {
                IsAnimating = true;
                SpiralPositionIndex = 0;
                int ranX = Ran.Current.Next((int)(Graphics.Current.ScreenWidth * 0.3), (int)(Graphics.Current.ScreenWidth * 0.7));
                int ranY = Ran.Current.Next((int)(Graphics.Current.ScreenHeight * 0.3), (int)(Graphics.Current.ScreenHeight * 0.7));
                AnimationOriginPoint = new Vector2(ranX, ranY);
            }

            if (IsAnimating)
            {
                AnimationIndexTimer++;
                if (AnimationIndexTimer > 3)
                {
                    AnimationIndexTimer = 0;
                    AnimationIndex++;
                }
                if (AnimationIndex >= ANIMATION_INDEX_MAX)
                {
                    IsAnimating = false;
                    ResetAnimation();
                }
            }
        }
        public void Draw()
        {
            if (IsAnimating)
            {
                Graphics.Current.SpriteB.Begin();
                DrawVoldy();
                SpiralPositionIndex += 0.4f;

                Graphics.Current.SpriteB.End();
            }
        }
        protected void ResetAnimation()
        {
            HallucinateTimer = Ran.Current.Next(4500, 17000);
            //HallucinateTimer = Ran.Current.Next(85, 310);
            AnimationIndex = 0;
            AnimationIndexTimer = 0;
        }
        protected Vector2 GenerateSpiralPoint(Vector2 originPosition, int index)
        {
            float angle = index * 15;
            float radius = (float)Math.Sqrt(index) * 35.0f; // Adjust the growth factor for your desired size
            double radians = angle * Math.PI / 180.0;

            float x = originPosition.X + (float)(radius * Math.Cos(radians));
            float y = originPosition.Y + (float)(radius * Math.Sin(radians));

            return new Vector2(x, y);
        }
        protected void DrawVoldy()
        {
            int fadeFrames = 10;
            int actualIndex = (int)SpiralPositionIndex;
            int currentIndex = actualIndex - fadeFrames;
            int smallest = actualIndex - fadeFrames;
            while (currentIndex < actualIndex)
            {
                if (currentIndex < 0)
                {
                    currentIndex++;
                    continue;
                }

                float indexTransparency = 1 - ((float)actualIndex - currentIndex) / fadeFrames;
                DrawVoldyAtIndex(currentIndex, Color.White * indexTransparency);
                currentIndex++;
            }
        }
        protected void DrawVoldyAtIndex(int index, Color color)
        {
            Vector2 curvoldy = GenerateSpiralPoint(AnimationOriginPoint, index);
            //float scaleFactor = (float)(Scale * 5 * Math.Pow(AnimationIndex / ANIMATION_INDEX_MAX, 2));
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName["voldy"], // (Texture2D texture,
                    curvoldy, // Vector2 position,
                    null,               // Rectangle? sourceRectangle,
                    color,        // Color color,
                    0,      // float rotation,
                    new Vector2(SpriteWidth / 2, SpriteHeight / 2),       // Vector2 origin,
                    Vector2.One,        // Vector2 scale,
                    Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // SpriteEffects effects,
                    1);                 // float layerDepth)
        }
    }
}
