using HPScreen.Admin;
using HPScreen.Entities;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;

namespace HPScreen
{
    public class HPScreen : Game
    {
        private GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private int loadFrames = 0;
        private const int LOAD_FRAMES_THRESH = 10;

        protected Bludger bludger1 { get; set; }
        protected Bludger bludger2 { get; set; }
        protected bool RunSetup { get; set; }
        public HPScreen()
        {
            Graphics.Current.GraphicsDM = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            // Set the following property to false to ensure alternating Step() and Draw() functions
            // Set the property to true to (hopefully) improve game smoothness by ignoring some draw calls if needed.
            IsFixedTimeStep = false;

            RunSetup = true;
        }
        protected override void Initialize()
        {
            // Note: This takes places BEFORE LoadContent()
            Graphics.Current.Init(this.GraphicsDevice, this.Window, true);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            Graphics.Current.SpriteB = new Microsoft.Xna.Framework.Graphics.SpriteBatch(Graphics.Current.GraphicsDM.GraphicsDevice);

            Graphics.Current.SpritesByName.Add("pitch", Content.Load<Texture2D>("Sprites/pitch"));
            Graphics.Current.SpritesByName.Add("potter", Content.Load<Texture2D>("Sprites/potter"));
            Graphics.Current.SpritesByName.Add("snitch", Content.Load<Texture2D>("Sprites/snitch"));
            Graphics.Current.SpritesByName.Add("bludger", Content.Load<Texture2D>("Sprites/bludger"));
            Graphics.Current.SpritesByName.Add("hagrid", Content.Load<Texture2D>("Sprites/hagrid"));
            Graphics.Current.SpritesByName.Add("quaffle", Content.Load<Texture2D>("Sprites/quaffle"));
            Graphics.Current.SpritesByName.Add("snape", Content.Load<Texture2D>("Sprites/snape"));
            Graphics.Current.SpritesByName.Add("voldy", Content.Load<Texture2D>("Sprites/voldy"));

            Graphics.Current.SpritesByName.Add("player-base", Content.Load<Texture2D>("Sprites/QuidditchPlayerBase"));
            Graphics.Current.SpritesByName.Add("player-pants", Content.Load<Texture2D>("Sprites/QuidditchPlayerPants"));
            Graphics.Current.SpritesByName.Add("player-jacket",Content.Load<Texture2D>("Sprites/QuidditchPlayerJacket"));
            Graphics.Current.SpritesByName.Add("player-head1", Content.Load<Texture2D>("Sprites/QuidditchPlayerHead1"));
            Graphics.Current.SpritesByName.Add("player-head2", Content.Load<Texture2D>("Sprites/QuidditchPlayerHead2"));
            Graphics.Current.SpritesByName.Add("player-head3", Content.Load<Texture2D>("Sprites/QuidditchPlayerHead3"));
            Graphics.Current.SpritesByName.Add("player-hair1", Content.Load<Texture2D>("Sprites/QuidditchPlayerHair1"));
            Graphics.Current.SpritesByName.Add("player-hair2", Content.Load<Texture2D>("Sprites/QuidditchPlayerHair2"));
            Graphics.Current.SpritesByName.Add("player-hair3", Content.Load<Texture2D>("Sprites/QuidditchPlayerHair3"));
            Graphics.Current.SpritesByName.Add("player-hair4", Content.Load<Texture2D>("Sprites/QuidditchPlayerHair4"));
            Graphics.Current.SpritesByName.Add("player-blush", Content.Load<Texture2D>("Sprites/QuidditchPlayerBlush"));
            Graphics.Current.SpritesByName.Add("player-bristle", Content.Load<Texture2D>("Sprites/QuidditchPlayerBristle"));
            Graphics.Current.SpritesByName.Add("player-broom", Content.Load<Texture2D>("Sprites/QuidditchPlayerBroom"));

            Graphics.Current.Fonts = new Dictionary<string, SpriteFont>();
            Graphics.Current.Fonts.Add("arial-48", Content.Load<SpriteFont>($"Fonts/arial_48"));
            Graphics.Current.Fonts.Add("arial-72", Content.Load<SpriteFont>($"Fonts/arial_72"));
            Graphics.Current.Fonts.Add("arial-96", Content.Load<SpriteFont>($"Fonts/arial_96"));
            Graphics.Current.Fonts.Add("arial-144", Content.Load<SpriteFont>($"Fonts/arial_144"));
        }
        protected override void Update(GameTime gameTime)
        {
            CheckInput();

            if (RunSetup) { Setup(); }

            //ScoreBoard.Current.Update();
            Quaffle.Current.Update();
            QuidditchTeam.Current.Update();
            //Snape.Current.Update();
            //Hagrid.Current.Update();
            Voldy.Current.Update();
            Snitch.Current.Update();
            Potter.Current.Update();
            bludger1.Update();
            bludger2.Update();
            QuoteDisplay.Current.Update();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            DrawBackground();
            //ScoreBoard.Current.Draw();
            Quaffle.Current.Draw();
            QuidditchTeam.Current.Draw();
            //Hagrid.Current.Draw();
            Potter.Current.Draw();
            Snitch.Current.Draw();
            bludger1.Draw();
            bludger2.Draw();
            //Snape.Current.Draw();
            QuoteDisplay.Current.Draw();
            Voldy.Current.Draw();
            base.Draw(gameTime);
        }

        protected void Setup()
        {
            bludger1 = new Bludger();
            bludger2 = new Bludger();

            RunSetup = false;
        }

        protected void DrawBackground()
        {
            Rectangle destinationRectangle = new Rectangle(0, 0, Graphics.Current.ScreenWidth, Graphics.Current.ScreenHeight);
            Graphics.Current.SpriteB.Begin();
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName["pitch"],  // Sprite: (texture2d)
                destinationRectangle,
                Color.White
            );
            Graphics.Current.SpriteB.End();
        }
        protected void CheckInput()
        {
            loadFrames++;
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            if (loadFrames >= LOAD_FRAMES_THRESH)
            {
                // Check if any key was pressed
                if (_currentKeyboardState.GetPressedKeys().Length > 0 && _previousKeyboardState.GetPressedKeys().Length == 0)
                {
                    Exit();
                }

                // Check if the mouse has moved
                Vector2 currentPos = new Vector2(_currentMouseState.Position.X, _currentMouseState.Position.Y);
                Vector2 previousPos = new Vector2(_previousMouseState.Position.X, _previousMouseState.Position.Y);
                if (Global.ApproxDist(currentPos, previousPos) >= 1)
                {
                    Exit();
                }
            }
        }
    }
}