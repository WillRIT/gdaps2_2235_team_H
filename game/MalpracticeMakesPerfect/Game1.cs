using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace MalpracticeMakesPerfect
{
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

        private Rectangle item;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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
            Joobi = this.Content.Load<Texture2D>("joobi");

            items = new List<Item>();
            recipes = new List<Recipe>();
            inventory = new Inventory(items, recipes);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //if ()
            inventory.GetItemsAndRecipes(filename, items, recipes);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            // TODO: Add your drawing code here

            _spriteBatch.Draw(Joobi, item, Color.AliceBlue);


            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}