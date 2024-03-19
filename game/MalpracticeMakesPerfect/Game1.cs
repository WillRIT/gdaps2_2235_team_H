using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace MalpracticeMakesPerfect
{
    /// <summary>
    ///whether the game is in play and if its in the shop in play or in game
    /// </summary>
    enum GameStates
    {
        TitleScreen,
        GameScene,
        GameShop,
        GameOver
    }

    /// <summary>
    /// the state of a moved item
    /// </summary>
    enum DragStates
    {
        Failed,
        Empty,
        Combine
    }

    public class Game1 : Game
    {
        //fields

        //Graphics and sprites
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D slotSprite;
        private Texture2D diamondSprite;
        private Texture2D joobi;
        private Texture2D patient;
        private Texture2D sky;

        //input managers
        private MouseState mouseState;
        private MouseState mousePrev;

        //items and slots
        private Rectangle itemPos;
        private Item diamond;
        private Slot diamondSlot;
        private Slot emptySlot;
        private Inventory myInventory;

        private List<Item> allItems;
        private Dictionary<string, Recipe> allRecipes;


        //Sky fields
        private Rectangle skyRect;
        private Rectangle skyRect2;

        //fonts
        private SpriteFont itemAmountFont;
        private SpriteFont titleFont;
        private SpriteFont subtitleFont;

        //item moving fields
        private List<Draggable> draggables = new List<Draggable>();
        private Draggable testDrag;
        private TempSlot testTemp;
        private Slot highlighted;
        private TempSlot theMessenger;
        private Slot snapBack;

        //States
        private GameStates gameState;

        //Katies menu variables!
        private Vector2 titlePos;
        private Vector2 subtitlePos;
        private float textBounceSpeed;
        private SpriteFont smallSubtitleFont;

        //Reputation and Money
        private int reputation;
        private double money;

        //for testing
        private Scenario JoobiScenario;
        private Texture2D adventurer;


        //misc fields
        private Vector2 path = new Vector2(10f, 400f);
        private float speed = 5.0f;

        private string consoleLog;


        private Random rng = new Random();

        /// <summary>
        /// Constructor
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            //Menu variables
            titlePos = new Vector2(255, 60);
            subtitlePos = new Vector2(450, 180);
            textBounceSpeed = 0.5f;   
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
            adventurer = Content.Load<Texture2D>("adventurer_03_1");

            sky = Content.Load<Texture2D>("sky");
            skyRect = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            skyRect2 = new Rectangle(_graphics.PreferredBackBufferWidth, 0, _graphics.PreferredBackBufferWidth + 1, _graphics.PreferredBackBufferHeight);

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
            smallSubtitleFont = Content.Load<SpriteFont>("SmallerSubtitleFont");

            // Solution list and adding solutions to it
            List<Solution> solutionList = new List<Solution>();

            

            JoobiScenario = new Scenario("I am Joobi", 2, solutionList, adventurer, "I am special Joobi");

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
                    reputation = 1600;
                    money = 100;
                    titlePos.Y += textBounceSpeed;
                    subtitlePos.Y += textBounceSpeed;
                    if(titlePos.Y <= 55|| titlePos.Y >= 80)
                    {
                        textBounceSpeed = -textBounceSpeed;
                    }


                    if (mouseState.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
                    {
                        gameState = GameStates.GameScene;
                    }

                    break;

                case GameStates.GameScene:

                   if(mouseState.RightButton == ButtonState.Pressed)
                    {
                        reputation -= 10;
                    }
                   if(reputation <= 0)
                    {
                        gameState = GameStates.GameOver;
                    }

                    myInventory.Update();

                    skyRect.X--;
                    skyRect2.X--;
                    if (skyRect.X < 0 - _graphics.PreferredBackBufferWidth)
                    {
                        skyRect.X = _graphics.PreferredBackBufferWidth;
                    }
                    if (skyRect2.X < 0 - _graphics.PreferredBackBufferWidth)
                    {
                        skyRect2.X = _graphics.PreferredBackBufferWidth;
                    }

                    if (JoobiScenario.Stopped == false)
                    {
                        JoobiScenario.ScenarioStart();
                    }

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
                        //item in trash is overwritten
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
                                        int outputAmount = 0; //handles quantity of the created items

                                        //if dragged item is same quantity
                                        if (highlighted.Amount == theMessenger.Amount)
                                        {
                                            highlighted.Item = allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[0];
                                            outputAmount = theMessenger.Amount;
                                        }
                                        //if dragged item has less
                                        else if (highlighted.Amount < theMessenger.Amount)
                                        {
                                            highlighted.Item = allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[0];
                                            outputAmount = highlighted.Amount;

                                            //return remaining items to original location
                                            theMessenger.Amount -= highlighted.Amount;
                                            dragAction = DragStates.Failed;
                                        }
                                        //if dragged item has more
                                        else if (highlighted.Amount > theMessenger.Amount)
                                        {
                                            highlighted.Amount -= theMessenger.Amount;

                                            theMessenger.Item = allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[0];
                                            outputAmount = theMessenger.Amount;
                                            dragAction = DragStates.Failed;
                                        }

                                        //log when items are created
                                        consoleLog += $"Created {outputAmount} {allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[0]}(s)";

                                        //add excess outputs
                                        for (int i = 1; i < allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs.Count; i++)
                                        {
                                            //log excess items
                                            consoleLog += $", {outputAmount} {allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[i]}(s)";

                                            bool placedExcess = false;
                                            foreach (Slot s in myInventory.Hotbar)
                                            {
                                                //place in the first available empty slot (if dragged item will not be sent back to that slot) or in the trash
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
                
                case GameStates.GameOver:
                    if (mouseState.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
                    {
                        gameState = GameStates.TitleScreen;
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
                    _spriteBatch.DrawString(titleFont, "MALPRACTICE MAKES PERFECT", titlePos, Color.Black);
                    _spriteBatch.DrawString(subtitleFont, "-Team Borderline Doctors-", new Vector2(subtitlePos.X - 1.5f, subtitlePos.Y - 1.5f), Color.White);
                    _spriteBatch.DrawString(subtitleFont, "-Team Borderline Doctors-", new Vector2(subtitlePos.X + 1.5f, subtitlePos.Y + 1.5f), Color.Black);
                    _spriteBatch.DrawString(subtitleFont, "-Team Borderline Doctors-", subtitlePos, Color.Red);
                    _spriteBatch.DrawString(subtitleFont, "Left Click to Start", new Vector2(600, 850), Color.Black);

                    
                    break;

                case GameStates.GameScene:

                    _spriteBatch.Draw(sky, skyRect, Color.White);
                    _spriteBatch.Draw(sky, skyRect2, Color.White);

                    myInventory.DrawScene(_spriteBatch);

                    _spriteBatch.DrawString(itemAmountFont, consoleLog, new Vector2(1500, 10), Color.Black);

                    JoobiScenario.Draw(_spriteBatch);

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
                    _spriteBatch.DrawString(smallSubtitleFont,"Reputation:",new Vector2(10,20), Color.Black);
                    _spriteBatch.Draw(joobi,new Rectangle(190,30,reputation,20),Color.Black);
                    _spriteBatch.DrawString(smallSubtitleFont,"Money:", new Vector2(10,50),Color.Black);
                    _spriteBatch.DrawString(smallSubtitleFont, ""+money, new Vector2(110, 50), Color.Goldenrod);

                    break;

                case GameStates.GameOver:
                    _spriteBatch.DrawString(titleFont, "you got run out of town", new Vector2(150,150), Color.Black);
                    _spriteBatch.DrawString(subtitleFont, "Left click to try again", new Vector2(150, 300), Color.Black);
                    break;

            }

            

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}