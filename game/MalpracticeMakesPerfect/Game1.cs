using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace MalpracticeMakesPerfect
{
    /// <summary>
    /// Contains the game's mode
    /// </summary>
    enum GameMode
    {
        Menu, Game, GameOver, Pause
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D jam;
        private Texture2D Joobi;

        private string filename = "../../../ITEMS.JSON";
        private Inventory inventory;
        private List<Item> items;
        private List<Recipe> recipes;
        private SpriteFont menuFont;
        private Vector2 patientPath;

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

            MouseState mouseState = new MouseState();

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

            Joobi = this.Content.Load<Texture2D>("joobi");


            items = new List<Item>();
            recipes = new List<Recipe>();
            inventory = new Inventory(items, recipes);

            List<Solution> ScenarioTest = new List<Solution>();

            Scenario JoobiSick = new Scenario("My arms are green! Help!", 2, ScenarioTest, Joobi, "Give me Apple");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //if ()
            inventory.GetItemsAndRecipes(filename, items, recipes);
            
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
                case GameMode.Game:


                    break;
                case GameMode.GameOver:
                    if (SingleKeyPress(Keys.Space, currentKbState))
                    {
                        currentState = GameMode.Menu;
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
                case GameMode.Game:
                    
                    break;
                case GameMode.GameOver:
                    _spriteBatch.DrawString(menuFont, "loss :[", new Vector2(20, 20), Color.Black);
                    break;
            }

            // TODO: Add your drawing code here

            _spriteBatch.Draw(Joobi, item, Color.AliceBlue);

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