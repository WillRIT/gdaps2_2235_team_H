using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        private Random rng = new Random();
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
        private Dictionary<string, Recipe> allRecipes;
        


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

            List<Slot> slotList = new List<Slot>
            {
                new Slot(slotSprite, new Rectangle(), itemAmountFont, allItems[rng.Next(allItems.Count)], 1),
                new Slot(slotSprite, new Rectangle(), itemAmountFont, allItems[rng.Next(allItems.Count)], 1),
                new Slot(slotSprite, new Rectangle(), itemAmountFont, allItems[rng.Next(allItems.Count)], 1),
                new Slot(slotSprite, new Rectangle(), itemAmountFont, allItems[rng.Next(allItems.Count)], 1),
                new Slot(slotSprite, new Rectangle(), itemAmountFont, allItems[rng.Next(allItems.Count)], 1),
                new Slot(slotSprite, new Rectangle(), itemAmountFont, allItems[rng.Next(allItems.Count)], 1),
                new Slot(slotSprite, new Rectangle(), itemAmountFont, allItems[rng.Next(allItems.Count)], 1)
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

                    case DragStates.Combine:
                        bool existsRecipe = false;

                        if (allRecipes.ContainsKey($"{highlighted.Item},{theMessenger.Item}"))
                        {
                            existsRecipe = true;
                            
                            //TODO: add functionality for combining with slots with > 1 item, as well as
                            //recipes with > 1 outputs
                            if (highlighted.Amount == 1)
                            {
                                highlighted.Item = allRecipes[$"{highlighted.Item},{theMessenger.Item}"].Outputs[0];
                            }
                        }
                        else if (allRecipes.ContainsKey($"{theMessenger.Item},{highlighted.Item}"))
                        {
                            existsRecipe = true;

                            //TODO: add functionality for combining with slots with > 1 item, as well as
                            //recipes with > 1 outputs
                            if (highlighted.Amount == 1)
                            {
                                highlighted.Item = allRecipes[$"{theMessenger.Item},{highlighted.Item}"].Outputs[0];
                            }
                        }

                        if (!existsRecipe)
                        {
                            dragAction = DragStates.Failed;
                        }

                        break;
                }
            }

            if (mouseState.LeftButton == ButtonState.Released && theMessenger != null)
            {
                if (dragAction == DragStates.Failed)
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