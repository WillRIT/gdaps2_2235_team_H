using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MalpracticeMakesPerfect
{
    enum GameStates
    {
        TitleScreen,
        GameScene,
        GameShop
    }

    enum DragStates
    {
        Failed,
        Empty,
        Combine
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private MouseState mouseState;
        private MouseState mousePrev;
        private Texture2D jam;

        private Rectangle itemPos;

        private Item diamond;
        private Slot diamondSlot;
        private Slot emptySlot;
        private Inventory myInventory;
        private Texture2D slotSprite;
        private Texture2D diamondSprite;
        private Texture2D joobi;

        private SpriteFont itemAmountFont;

        private List<Draggable> draggables = new List<Draggable>();
        private Draggable testDrag;
        private TempSlot testTemp;

        private Slot highlighted;

        private TempSlot theMessenger;
        private Slot snapBack;

        private List<Item> allItems;
        private List<Recipe> allRecipes;
        


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            DatabaseManager databaseManager = new DatabaseManager();

            allItems = databaseManager.GetItemsAndRecipes(Content, out allRecipes);

            slotSprite = Content.Load<Texture2D>("slot");
            diamondSprite = Content.Load<Texture2D>("diamond");
            itemAmountFont = Content.Load<SpriteFont>("item-amount");
            joobi = Content.Load<Texture2D>("joobi");
            Texture2D mealyAppleSprite = Content.Load<Texture2D>("Mealy Apple");

            Item mealyApple = new Item(mealyAppleSprite, new Rectangle(0, 0, 50, 50), "Mealy Apple", "a very mealy apple", 60.80, false);

            List<Slot> slotList = new List<Slot>
            {
                new Slot(slotSprite, new Rectangle(0, 0, 50, 50), itemAmountFont, mealyApple, 6),
                new Slot(slotSprite, new Rectangle(0, 0, 0, 0), itemAmountFont),
                new Slot(slotSprite, new Rectangle(0, 0, 0, 0), itemAmountFont, mealyApple, 5)
            };

            myInventory = new Inventory(joobi, new Rectangle(500, 500, 500, 200), itemAmountFont, slotSprite, slotList);

            theMessenger = null;

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            mouseState = Mouse.GetState();

            myInventory.Update();

            bool existsHighlight = false;
            DragStates dragAction = DragStates.Failed;
            foreach (Slot s in myInventory.Hotbar)
            {
                if (s.Hovered)
                {
                    existsHighlight = true;
                    highlighted = s;

                    if (mouseState.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released && !s.IsEmpty)
                    {
                        theMessenger = new TempSlot(highlighted.Position, itemAmountFont, highlighted.Item, highlighted.Amount);
                        snapBack = highlighted;
                        highlighted.Item = null;
                    }
                }
            }
            if (!existsHighlight)
            {
                highlighted = null;
            }

            if (theMessenger != null)
            {
                theMessenger.Update();
            }

            if (theMessenger != null && theMessenger.Placing && existsHighlight)
            {
                dragAction = theMessenger.SnapIntersect(highlighted);

                switch (dragAction)
                {
                    case DragStates.Empty:
                        highlighted.Item = theMessenger.Item;
                        highlighted.Amount = theMessenger.Amount;

                        break;
                }
            }

            if (mouseState.LeftButton == ButtonState.Released && theMessenger != null)
            {
                if (dragAction != DragStates.Empty)
                {
                    snapBack.Item = theMessenger.Item;
                    snapBack.Amount = theMessenger.Amount;
                }

                theMessenger = null;
            }

            mousePrev = mouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Crimson);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            myInventory.DrawScene(_spriteBatch);

            if (theMessenger != null)
            {
                theMessenger.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}