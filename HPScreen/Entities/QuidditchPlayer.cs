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
    public class QuidditchPlayer : Follower
    {
        protected Color GryffindorColor { get { return new Color(166, 51, 46); } }
        protected Color HufflepuffColor { get { return new Color(239, 188, 47); } }
        protected Color RavenclawColor { get { return new Color(60, 78, 145); } }
        protected Color SlytherinColor { get { return new Color(54, 100, 71); } }
        public enum TeamValue
        {
            Gryffindor,
            Hufflepuff,
            RavenClaw,
            Slytherin
        }
        public QuidditchPlayer(TeamValue team) : base(Quaffle.Current)
        {
            Team = team;
            Scale = 0.5f;
            SetBasicCharacterAttributes();
            SetTeamAttributes();
            Visible = true;
        }
        public bool Visible { get; set; }
        public override float SpriteWidth { get { return 595.0f; } }
        public override float SpriteHeight { get { return 492.0f; } }
        protected override int LAG_ALARM_MIN { get { return 25; } }
        protected override int LAG_ALARM_MAX { get { return 180; } }
        protected TeamValue Team { get; set; }
        protected int Face { get; set; }
        protected int Hair { get; set; }
        protected bool HasBlush { get; set; }
        protected Color SkinColor { get; set; }
        protected Color PantColor { get; set; }
        protected Color JacketColor { get; set; }
        protected Color HairColor { get; set; }
        protected Color BristleColor { get; set; }
        protected Color BroomColor { get; set; }
        protected void SetBasicCharacterAttributes()
        {
            Color[] pantoptions = new Color[]
            {
                new Color(227, 225, 218), // off white
                new Color(245, 236, 206), // tan
                new Color(166, 151, 116), // dark tan
                new Color(30, 37, 56), // Navy
                new Color(9, 16, 33), // Dark navy
                new Color(3, 23, 6) // dark green
            };
            PantColor = pantoptions[Ran.Current.Next(0, pantoptions.Length - 1)];

            Color[] hairoptions = new Color[]
            {
                new Color(247, 228, 82), // bright blonde
                new Color(232, 221, 139), // dusty blonde
                new Color(105, 99, 62), // dark blonde
                new Color(201, 180, 135), // light brown
                new Color(112, 93, 53), // med brown
                new Color(54, 39, 9), // dark brown
                new Color(33, 23, 2), // very dark brown
                new Color(0, 0, 0), // black
                new Color(230, 126, 57), // Bright ginger
                new Color(158, 85, 36), // med ginger
                new Color(71, 32, 6), // dark ginger
                new Color(63, 8, 74), // purple
                new Color(10, 99, 10), // green
                new Color(9, 90, 130), // Aqua
                new Color(250, 250, 250) // white
            };
            HairColor = hairoptions[Ran.Current.Next(0, hairoptions.Length - 1)];

            Face = Ran.Current.Next(1, 3);
            Hair = Ran.Current.Next(1, 4);
            HasBlush = Ran.Current.Next(0, 4) == 0 ? true : false;
            SkinColor = Color.Lerp(Color.White, new Color(51, 28, 6), Ran.Current.Next(0f, 1f));

            Color[] bristleoptions = new Color[]
            {
                new Color(237, 155, 88), // orange
                new Color(135, 82, 38), // brown
                new Color(219, 179, 114), // tan
                new Color(214, 194, 161), // light tan
                new Color(153, 149, 142), // grey
                new Color(54, 31, 13), // dark brown
            };
            BristleColor = bristleoptions[Ran.Current.Next(0, bristleoptions.Length - 1)];

            Color[] broomoptions = new Color[]
            {
                new Color(189, 160, 115), // tan
                new Color(140, 112, 69), // dark tan
                new Color(89, 64, 25), // brown
                new Color(46, 29, 2), // dark brown
                new Color(122, 82, 61), // light mahagony
                new Color(89, 51, 31), // mahagony
                new Color(48, 21, 6) // dark mahogany
            };
            BroomColor = broomoptions[Ran.Current.Next(0, broomoptions.Length - 1)];
        }
        protected void SetTeamAttributes()
        {
            switch (this.Team)
            {
                case TeamValue.Gryffindor:
                    JacketColor = GryffindorColor;
                    break;

                case TeamValue.Hufflepuff:
                    JacketColor = HufflepuffColor;
                    break;

                case TeamValue.RavenClaw:
                    JacketColor = RavenclawColor;
                    break;

                case TeamValue.Slytherin:
                    JacketColor = SlytherinColor;
                    break;
            }
        }
        public void Draw()
        {
            if (!Following.InMotion) { return; }
            if (!Visible) { return; }

            Graphics.Current.SpriteB.Begin();

            // BASE
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName["player-base"], // (Texture2D texture,
                    new Vector2(Xpos, Ypos), // Vector2 position,
                    null,               // Rectangle? sourceRectangle,
                    Color.White,        // Color color,
                    0,      // float rotation,
                    new Vector2(SpriteWidth / 2, SpriteHeight / 2),       // Vector2 origin,
                    Scale,        // Vector2 scale,
                    Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // SpriteEffects effects,
                    1);                 // float layerDepth)

            // PANTS
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName["player-pants"], // (Texture2D texture,
                    new Vector2(Xpos, Ypos), // Vector2 position,
                    null,               // Rectangle? sourceRectangle,
                    PantColor,        // Color color,
                    0,      // float rotation,
                    new Vector2(SpriteWidth / 2, SpriteHeight / 2),       // Vector2 origin,
                    Scale,        // Vector2 scale,
                    Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // SpriteEffects effects,
                    1);                 // float layerDepth)
            
            // JACKET
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName["player-jacket"], // (Texture2D texture,
                    new Vector2(Xpos, Ypos), // Vector2 position,
                    null,               // Rectangle? sourceRectangle,
                    JacketColor,        // Color color,
                    0,      // float rotation,
                    new Vector2(SpriteWidth / 2, SpriteHeight / 2),       // Vector2 origin,
                    Scale,        // Vector2 scale,
                    Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // SpriteEffects effects,
                    1);                 // float layerDepth)

            // BRISTLE
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName["player-bristle"], // (Texture2D texture,
                    new Vector2(Xpos, Ypos), // Vector2 position,
                    null,               // Rectangle? sourceRectangle,
                    BristleColor,        // Color color,
                    0,      // float rotation,
                    new Vector2(SpriteWidth / 2, SpriteHeight / 2),       // Vector2 origin,
                    Scale,        // Vector2 scale,
                    Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // SpriteEffects effects,
                    1);                 // float layerDepth)

            // BROOM
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName["player-broom"], // (Texture2D texture,
                    new Vector2(Xpos, Ypos), // Vector2 position,
                    null,               // Rectangle? sourceRectangle,
                    BroomColor,        // Color color,
                    0,      // float rotation,
                    new Vector2(SpriteWidth / 2, SpriteHeight / 2),       // Vector2 origin,
                    Scale,        // Vector2 scale,
                    Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // SpriteEffects effects,
                    1);                 // float layerDepth)

            // FACE
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName[$"player-head{Face}"], // (Texture2D texture,
                    new Vector2(Xpos, Ypos), // Vector2 position,
                    null,               // Rectangle? sourceRectangle,
                    SkinColor,        // Color color,
                    0,      // float rotation,
                    new Vector2(SpriteWidth / 2, SpriteHeight / 2),       // Vector2 origin,
                    Scale,        // Vector2 scale,
                    Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // SpriteEffects effects,
                    1);                 // float layerDepth)

            // HAIR
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName[$"player-hair{Hair}"], // (Texture2D texture,
                    new Vector2(Xpos, Ypos), // Vector2 position,
                    null,               // Rectangle? sourceRectangle,
                    HairColor,        // Color color,
                    0,      // float rotation,
                    new Vector2(SpriteWidth / 2, SpriteHeight / 2),       // Vector2 origin,
                    Scale,        // Vector2 scale,
                    Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // SpriteEffects effects,
                    1);                 // float layerDepth)

            // BLUSH
            if (this.HasBlush)
            {
                Graphics.Current.SpriteB.Draw(
                    Graphics.Current.SpritesByName["player-blush"], // (Texture2D texture,
                        new Vector2(Xpos, Ypos), // Vector2 position,
                        null,               // Rectangle? sourceRectangle,
                        Color.White,        // Color color,
                        0,      // float rotation,
                        new Vector2(SpriteWidth / 2, SpriteHeight / 2),       // Vector2 origin,
                        Scale,        // Vector2 scale,
                        Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // SpriteEffects effects,
                        1);                 // float layerDepth)
            }

            Graphics.Current.SpriteB.End();
        }

        protected override void UpdateMotion()
        {
            int offsetY = Ran.Current.Next(-200, 200);
            int offsetX = Ran.Current.Next(-20, 20);
            this.TargetXPos = Following.Xpos + offsetX;
            this.TargetYPos = Following.Ypos + offsetY;
        }
    }
}
