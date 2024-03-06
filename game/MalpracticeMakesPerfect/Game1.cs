using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

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

        private GameStates gameState;

        //Katies menu variables!
        private SpriteFont titleFont;
        private SpriteFont subtitleFont;
        private Vector2 titlePos;
        private Vector2 subtitlePos;
        private float textBounceSpeed;

        //Reputation and Money
        private int reputation;
        private double money;

        private string consoleLog;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            //Menu variables
            titlePos = new Vector2(255, 60);
            subtitlePos = new Vector2(450, 180);
            textBounceSpeed = 0.5f;

            //Reputation and Money
            reputation = 100;
            money = 100;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            consoleLog = string.Empty;

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

            List<Slot> slotList = new List<Slot>();

            for (int i = 0; i < 10; i++)
            {
                slotList.Add(new Slot(slotSprite, new Rectangle(), itemAmountFont, allItems[rng.Next(9)], rng.Next(1,4)));
            }

            myInventory = new Inventory(joobi, new Rectangle(500, 500, 500, 200), itemAmountFont, slotSprite, slotList);

            theMessenger = null;

            //menu fonts
            titleFont = Content.Load<SpriteFont>("TitleFont");
            subtitleFont = Content.Load<SpriteFont>("SubtitleFont");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            mouseState = Mouse.GetState();

            switch (gameState)
            {
                case GameStates.TitleScreen:
                    titlePos.Y += textBounceSpeed;
                    subtitlePos.Y += textBounceSpeed;
                    if(titlePos.Y <= 55|| titlePos.Y >= 80)
                    {
                        textBounceSpeed = -textBounceSpeed;
                    }


                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        gameState = GameStates.GameScene;
                    }

                    break;

                case GameStates.GameScene:

                    myInventory.Update();

                    //whether or not a slot is being highlighted
                    bool existsHighlight = false;
                    //"failed" assures that the dragged item is returned to its original place
                    DragStates dragAction = DragStates.Failed;
                    //check for item pickup
                    foreach (Slot s in myInventory.Hotbar)
                    {
                        if (s.Hovered)
                        {
                            existsHighlight = true;
                            //highlighted = the slot the dragged item would return to
                            highlighted = s;

                            if (mouseState.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released && !s.IsEmpty)
                            {
                                //messenger = dragged item
                                theMessenger = new TempSlot(highlighted.Position, itemAmountFont, highlighted.Item, highlighted.Amount);
                                snapBack = highlighted;
                                //remove item from the slot from which the item was dragged
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

                    //handle let go of click when item is being dragged
                    if (theMessenger != null && theMessenger.Placing && existsHighlight)
                    {
                        if (highlighted.IsTrash)
                        {
                            dragAction = DragStates.Empty;
                            highlighted.Item = theMessenger.Item;
                            highlighted.Amount = theMessenger.Amount;
                        }
                        else
                        {
                            dragAction = theMessenger.SnapIntersect(highlighted);

                            switch (dragAction)
                            {
                                //simple move item to 
                                case DragStates.Empty:
                                    highlighted.Item = theMessenger.Item;
                                    highlighted.Amount = theMessenger.Amount;

                                    break;

                                case DragStates.Combine:
                                    bool existsRecipe = false;

                                    Item[] recipeInputs = new Item[2];

                                    //check if there is a recipe
                                    if (allRecipes.ContainsKey($"{highlighted.Item},{theMessenger.Item}"))
                                    {
                                        recipeInputs = new Item[]
                                        {
                                            highlighted.Item,
                                            theMessenger.Item
                                        };
                                        existsRecipe = true;
                                    }
                                    else if (allRecipes.ContainsKey($"{theMessenger.Item},{highlighted.Item}"))
                                    {
                                        recipeInputs = new Item[]
                                        {
                                            theMessenger.Item,
                                            highlighted.Item
                                        };
                                        existsRecipe = true;
                                    }
                                    if (existsRecipe)
                                    {
                                        //TODO: add functionality for combining with slots with > 1 item, as well as
                                        //recipes with > 1 outputs
                                        int outputAmount = 0;

                                        if (highlighted.Amount == theMessenger.Amount)
                                        {
                                            highlighted.Item = allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[0];
                                            outputAmount = theMessenger.Amount;
                                        }
                                        else if (highlighted.Amount < theMessenger.Amount)
                                        {
                                            highlighted.Item = allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[0];
                                            outputAmount = highlighted.Amount;

                                            //return remaining items to original location
                                            theMessenger.Amount -= highlighted.Amount;
                                            dragAction = DragStates.Failed;
                                        }
                                        else if (highlighted.Amount > theMessenger.Amount)
                                        {
                                            highlighted.Amount -= theMessenger.Amount;

                                            theMessenger.Item = allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[0];
                                            outputAmount = theMessenger.Amount;
                                            dragAction = DragStates.Failed;
                                        }

                                        consoleLog += $"Created {outputAmount} {allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[0]}(s)";

                                        //add excess outputs
                                        for (int i = 1; i < allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs.Count; i++)
                                        {
                                            consoleLog += $", {outputAmount} {allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[i]}(s)";

                                            bool placedExcess = false;
                                            foreach (Slot s in myInventory.Hotbar)
                                            {
                                                if (!placedExcess && ((s != snapBack && dragAction == DragStates.Failed) || dragAction != DragStates.Failed) && ((s.IsEmpty && !s.IsTrash) || s.IsTrash))
                                                {
                                                    s.Item = allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[i];
                                                    s.Amount = outputAmount;
                                                    placedExcess = true;
                                                }
                                            }
                                        }

                                        consoleLog += "\n";
                                    }

                                    //item stacking
                                    if (theMessenger.Item.ItemName == highlighted.Item.ItemName)
                                    {
                                        existsRecipe = true;

                                        highlighted.Amount += theMessenger.Amount;
                                    }

                                    if (!existsRecipe)
                                    {
                                        dragAction = DragStates.Failed;
                                    }

                                    break;
                            }
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

                    break;
            }
            

            mousePrev = mouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Crimson);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            switch (gameState)
            {
                case GameStates.TitleScreen://Main screen art
                    GraphicsDevice.Clear(Color.Maroon);
                    _spriteBatch.DrawString(titleFont, "MAlPRACTICE MAKES PERFECT", titlePos, Color.Black);
                    _spriteBatch.DrawString(subtitleFont, "-Team Borderline Doctors-", new Vector2(subtitlePos.X - 1.5f, subtitlePos.Y - 1.5f), Color.White);
                    _spriteBatch.DrawString(subtitleFont, "-Team Borderline Doctors-", new Vector2(subtitlePos.X + 1.5f, subtitlePos.Y + 1.5f), Color.Black);
                    _spriteBatch.DrawString(subtitleFont, "-Team Borderline Doctors-", subtitlePos, Color.Red);
                    _spriteBatch.DrawString(subtitleFont, "Left Click to Start", new Vector2(600, 850), Color.Black);


                    break;

                case GameStates.GameScene:

                    myInventory.DrawScene(_spriteBatch);

                    _spriteBatch.DrawString(itemAmountFont, consoleLog, new Vector2(1500, 10), Color.Black);

                    if (theMessenger != null)
                    {
                        theMessenger.Draw(_spriteBatch);
                        //preview possible output(s) of combination
                        if (highlighted != null)
                        {
                            List<Item> combinePreview = new List<Item>();
                            if (allRecipes.ContainsKey($"{highlighted.Item},{theMessenger.Item}"))
                            {
                                combinePreview = allRecipes[$"{highlighted.Item},{theMessenger.Item}"].Outputs;
                            }
                            else if (allRecipes.ContainsKey($"{theMessenger.Item},{highlighted.Item}"))
                            {
                                combinePreview = allRecipes[$"{theMessenger.Item},{highlighted.Item}"].Outputs;
                            }

                            if (combinePreview.Count > 0)
                            {
                                for (int i = 0; i < combinePreview.Count; i++)
                                {
                                    _spriteBatch.Draw(combinePreview[i].Asset, new Rectangle(Mouse.GetState().X + i * 50, Mouse.GetState().Y, 50, 50), Color.Black);
                                }
                            }
                            else if (highlighted.IsEmpty || theMessenger.Item.ItemName == highlighted.Item.ItemName)
                            {
                                _spriteBatch.DrawString(itemAmountFont, theMessenger.Item.ToString(), new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.Black);
                            }
                            else
                            {
                                _spriteBatch.DrawString(itemAmountFont, "?????????????????????????????????????????", new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.Black);
                            }
                        }
                        else
                        {
                            _spriteBatch.DrawString(itemAmountFont, theMessenger.Item.ToString(), new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.Black);
                        }
                    }
                    else if (highlighted != null && !highlighted.IsEmpty)
                    {
                        _spriteBatch.DrawString(itemAmountFont, highlighted.ItemName, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.Black);
                    }

                    break;
            }

            

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}