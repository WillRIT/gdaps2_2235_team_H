using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MalpracticeMakesPerfect
{
    /// <summary>
    /// Contains the game's mode
    /// </summary>
    enum GameMode
    {
        Menu, Game, GameOver
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D jam;
        private Texture2D Joobi;
        private SpriteFont menuFont;

        private Rectangle item;

        //KeyboardStates
        private KeyboardState currentKbState;
        private KeyboardState previousKbState;

        //GameMode
        private GameMode currentState;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            currentState = GameMode.Menu;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            menuFont = Content.Load<SpriteFont>("MenuFont");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            currentKbState = Keyboard.GetState();
           
            //switches between GameModes
            switch (currentState)
            {
                case GameMode.Menu:
                    if (SingleKeyPress(Keys.Space, currentKbState))
                    {
                        currentState = GameMode.Game;
                    }
                    break;
            }

            previousKbState = currentKbState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            
            
            switch (currentState)
            {
                //Main Menu Text
                case GameMode.Menu:
                    _spriteBatch.DrawString(menuFont, "Malpractice Makes Perfect", new Vector2(20, 20), Color.Black);
                    _spriteBatch.DrawString(menuFont, "Team Borderline Doctors", new Vector2(20, 50), Color.Black);
                    _spriteBatch.DrawString(menuFont, "Press space to begin", new Vector2(20, 70), Color.Black);

                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Checks for a single key press
        /// </summary>
        /// <param name="key"></param>
        /// <param name="kbState"></param>
        /// <returns></returns>
        private bool SingleKeyPress(Keys key, KeyboardState kbState)
        {
            kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(key) && previousKbState.IsKeyUp(key))
            {
                return true;
            }
            return false;
        }
    }
}