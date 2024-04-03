using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        Instructions,
        GameScene,
        GameShop,
        GameOver,
        DayEnd
    }

    public class Game1 : Game
    {
        //fields

        //Graphics and sprites
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D slotSprite;
        private Texture2D joobi;
        private Texture2D patient;
        private Texture2D sky;
        private Texture2D cloud;
        private Texture2D ground;
        private Texture2D office;
        private Vector2 officeLocation;

        private Texture2D shopSlasset;
        private Texture2D shopSlassetB;


        //input managers
        private MouseState mouseState;
        private MouseState mousePrev;

        private KeyboardState keyboardState;

        //items and slots
        private Inventory myInventory;

        private List<Item> allItems;
        private Dictionary<string, Recipe> allRecipes;
        private Dictionary<string, Item> itemDict;


        //Sky fields
        private Rectangle skyRect;
        private Rectangle skyRect2;
        private Rectangle cloudRect;
        private Rectangle cloudRect2;

        private Rectangle groundRect;

        //fonts
        private SpriteFont itemAmountFont;
        private SpriteFont titleFont;
        private SpriteFont subtitleFont;
        private SpriteFont smallSubtitleFont;
        private SpriteFont mediumFont;

        //item moving fields
        private TempSlot theMessenger;

        private Shop myShop;

        //States
        private GameStates gameState;

        //Katies menu variables!
        private Vector2 titlePos;
        private Vector2 subtitlePos;
        private float textBounceSpeed;
        private Texture2D star;
        private List<Rectangle> starsLoc;

        //Reputation and Money
        private int reputation;
        private double money;

        //for testing
        private Scenario GreenScenario;
        private Texture2D adventurer;

        //scenario List
        private Queue<Scenario> scenarioQueue;

        //misc fields
        private Vector2 path = new Vector2(10f, 400f);
        private float speed = 5.0f;

        private string consoleLog;

        private Random rng = new Random();


        private Slot newSnapBack;
        private Slot highlightedSlot;

        private List<Scenario> scenarios;

        /// <summary>
        /// Constructor
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            //Menu variables
            titlePos = new Vector2(150, 60);
            subtitlePos = new Vector2(450, 180);
            textBounceSpeed = 0.2f;

            scenarioQueue = new Queue<Scenario>();
            starsLoc = new List<Rectangle>();
            


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


            //Loading textures
            slotSprite = Content.Load<Texture2D>("ui/slot");
            joobi = Content.Load<Texture2D>("ui/joobi");
            
            sky = Content.Load<Texture2D>("background/sky");
            cloud = Content.Load<Texture2D>("background/cloud");
            ground = Content.Load<Texture2D>("background/grass");
            office = Content.Load<Texture2D>("background/Shop Pack V2 4");
            officeLocation = new Vector2(1180, 350);

            //people
            adventurer = Content.Load<Texture2D>("people/green_man");

            shopSlasset = Content.Load<Texture2D>("ui/shopslot1");
            shopSlassetB = Content.Load<Texture2D>("ui/shopslot2");

            //fonts
            itemAmountFont = Content.Load<SpriteFont>("fonts/item-amount");
            titleFont = Content.Load<SpriteFont>("fonts/TitleFont");
            subtitleFont = Content.Load<SpriteFont>("fonts/SubtitleFont");
            smallSubtitleFont = Content.Load<SpriteFont>("fonts/SmallerSubtitleFont");
            mediumFont = Content.Load<SpriteFont>("fonts/MediumFont");

            star = Content.Load<Texture2D>("ui/star");

            //GET ITEMS
            DatabaseManager databaseManager = new DatabaseManager();

            allItems = databaseManager.GetItemsAndRecipes(Content, out allRecipes, out itemDict);


            //Load sky
            skyRect = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            skyRect2 = new Rectangle(_graphics.PreferredBackBufferWidth, 0, _graphics.PreferredBackBufferWidth + 1, _graphics.PreferredBackBufferHeight);
            cloudRect = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            cloudRect2 = new Rectangle(_graphics.PreferredBackBufferWidth, 0, _graphics.PreferredBackBufferWidth + 1, _graphics.PreferredBackBufferHeight);

            groundRect = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            myInventory = new Inventory(joobi, new Rectangle(700, 500, 500, 200), itemAmountFont, slotSprite, PickUpItem, PutDownItem, PutSingleItem, SetHighlighted);

            theMessenger = null;

            List<Item> shopItems = new List<Item>();
            foreach (Item i in allItems)
            {
                if (i.InShop)
                {
                    shopItems.Add(i);
                }
            }
            myShop = new Shop(joobi, shopSlasset, shopSlassetB, new Rectangle(1200, 300, 600, 980), itemAmountFont, shopItems, PurchaseItem);




            // Scenarios
            //fix this shit
            scenarios = DatabaseManager.GetScenarios(Content, allItems, slotSprite, mediumFont, shopSlassetB, PickUpItem, PutDownItemScenario, SetHighlighted);

        }

        /// <summary>
        /// Purchase item from shop and add it to the inventory
        /// </summary>
        /// <param name="bought">The purchased item</param>
        private void PurchaseItem(Item bought)
        {
            //check for empty slot
            foreach (Slot s in myInventory.Hotbar)
            {
                //if same name or empty and not trash
                if ((s.IsEmpty || s.ItemName == bought.ItemName) && !s.IsTrash)
                {
                    //test if sufficient funds
                    if (money >= bought.Cost)
                    {
                        s.AddItem(bought, 1);
                        money -= bought.Cost;

                        consoleLog += $"Bought 1 {bought.ItemName} for ${bought.Cost:N2}\n";
                        return;
                    }
                    else
                    {
                        consoleLog += $"Not enough money for {bought.ItemName}!\n";
                        return;
                    }
                }
            }
            consoleLog += $"Not enough room in inventory!\n";
        }

        internal void PickUpItem(Slot mySlot) {
            if (theMessenger == null)
            {
                //snapback assures item will be put back if player releases mouse button
                newSnapBack = mySlot;

                //messenger is a temporary slot that moves items
                theMessenger = new TempSlot(mySlot.Position, itemAmountFont, mySlot.Item, mySlot.Amount);

                mySlot.Clear();
            }
        }

        internal void PutDownItem(Slot mySlot)
        {
            if (theMessenger != null)
            {
                //overwrite trash item
                if (mySlot.IsTrash || mySlot.IsEmpty)
                {
                    mySlot.Item = theMessenger.Item;
                    mySlot.Amount = theMessenger.Amount;

                    theMessenger = null;
                }
                else if (mySlot.ItemName == theMessenger.Item.ItemName)
                {
                    mySlot.Amount += theMessenger.Amount;

                    theMessenger = null;
                }
                else
                {
                    CombineItems(mySlot);
                }
            }
        }

        internal void PutDownItemScenario(Slot mySlot)
        {
            if (theMessenger != null)
            {
                if (mySlot.IsEmpty)
                {
                    mySlot.AddItem(theMessenger.Item, 1);
                    theMessenger.Amount--;
                }
            }
        }

        internal void CombineItems(Slot mySlot)
        {
            Item[] recipeInputs = new Item[2];

            bool existsRecipe = !(GetItemCombo(mySlot.Item, theMessenger.Item, out recipeInputs).Count == 0);

            if (existsRecipe)
            {
                int outputAmount = 0; //handles quantity of the created items

                //if dragged item is same quantity
                if (mySlot.Amount == theMessenger.Amount)
                {
                    mySlot.Item = allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[0];
                    outputAmount = theMessenger.Amount;

                    theMessenger = null;
                }

                //if dragged item has more
                else if (mySlot.Amount < theMessenger.Amount)
                {
                    mySlot.Item = allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[0];
                    outputAmount = mySlot.Amount;

                    //return remaining items to original location
                    theMessenger.Amount -= mySlot.Amount;
                }
                //if dragged item has less
                else if (mySlot.Amount > theMessenger.Amount)
                {
                    mySlot.Amount -= theMessenger.Amount;

                    theMessenger.Item = allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[0];
                    outputAmount = theMessenger.Amount;
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
                        //place in the first available empty slot (if dragged item will not be sent back to original slot) or in the trash
                        if (!placedExcess && ((s != newSnapBack && theMessenger != null) || theMessenger == null) && ((s.IsEmpty && !s.IsTrash) || s.IsTrash))
                        {
                            s.Item = allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[i];
                            s.Amount = outputAmount;
                            placedExcess = true;
                        }
                    }
                }

                consoleLog += "\n";
            }
        }

        internal List<Item> GetItemCombo(Item item1, Item item2, out Item[] recipeInputs)
        {
            //check if there is a recipe
            if (allRecipes.ContainsKey($"{item1},{item2}"))
            {
                recipeInputs = new Item[]
                {
                    item1,
                    item2
                };
                return allRecipes[$"{item1},{item2}"].Outputs;
            }
            else if (allRecipes.ContainsKey($"{item2},{item1}"))
            {
                recipeInputs = new Item[]
                {
                    item2,
                    item1
                };
                return allRecipes[$"{item2},{item1}"].Outputs;
            }

            recipeInputs = null;

            return new List<Item>();
        }

        internal void PutSingleItem(Slot mySlot)
        {
            if (theMessenger != null)
            {
                if (theMessenger.Amount > 1)
                {
                    if (mySlot.AddItem(theMessenger.Item, 1))
                    {
                        theMessenger.Amount -= 1;
                    }
                }
            }
        }

        internal void SetHighlighted(Slot mySlot)
        {
            highlightedSlot = mySlot;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.F))
            {
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges();
            }

            mouseState = Mouse.GetState();

            switch (gameState)
            {
                case GameStates.TitleScreen:
                    reputation = 1600;
                    money = 1000;
                    myInventory.Clear();
                    titlePos.Y += textBounceSpeed;
                    subtitlePos.Y += textBounceSpeed;
                    if(titlePos.Y <= 55|| titlePos.Y >= 80)
                    {
                        textBounceSpeed = -textBounceSpeed;
                    }

                    //changing into Instructions
                    if (mouseState.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
                    {
                        gameState = GameStates.Instructions;
                        starsLoc.Clear();
                        for (int i = 0; i < 18; i++)
                        {
                            starsLoc.Add(new Rectangle(rng.Next(100, 1820), rng.Next(100, 980), 40, 40));
                        }
                    }

                    break;
                    //changing to play state
                case GameStates.Instructions:
                    if (mouseState.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
                    {
                        gameState = GameStates.GameScene;
                    }

                        break;

                case GameStates.GameScene:

                    highlightedSlot = null;

                    //queuing scenarios

                    scenarioQueue.Enqueue(GreenScenario);
                    Scenario currentScenario = scenarioQueue.Peek();

                    if (currentScenario.state == Scenario.ScenarioState.Leaving)
                    {
                        scenarioQueue.Dequeue();
                    }
                    if (scenarioQueue.Count == 0)
                    {
                        gameState = GameStates.DayEnd;
                    }

                    currentScenario.Update();
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        money -= 10;
                    }

                    //changing into game over state
                   if(reputation <= 0)
                    {
                        gameState = GameStates.GameOver;
                    }
                    if (money <= 0)
                    {
                        money = 0;
                        gameState = GameStates.GameOver;
                    }
                    if (scenarioQueue.Count == 0 && reputation > 0)
                    {
                        gameState = GameStates.DayEnd;

                    }


                    //moving sky background
                    skyRect.X--;
                    skyRect2.X--;
                    cloudRect.X--;
                    cloudRect2.X--;
                    if (skyRect.X < 0 - _graphics.PreferredBackBufferWidth)
                    {
                        skyRect.X = _graphics.PreferredBackBufferWidth;
                        cloudRect.X = _graphics.PreferredBackBufferWidth;
                    }
                    if (skyRect2.X < 0 - _graphics.PreferredBackBufferWidth)
                    {
                        skyRect2.X = _graphics.PreferredBackBufferWidth;
                        cloudRect2.X = _graphics.PreferredBackBufferWidth;
                    }

                    //inventory handling
                    myShop.Update();

                    myInventory.Update();

                    if (theMessenger != null)
                    {
                        theMessenger.Update();

                        //places item in its original spot
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            newSnapBack.AddItem(theMessenger.Item, theMessenger.Amount);

                            //removes messenger
                            theMessenger = null;
                        }
                    }

                    

                    break;
                
                case GameStates.GameOver:
                    if (mouseState.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
                    {
                        gameState = GameStates.TitleScreen;
                    }
                    break;

                case GameStates.DayEnd:
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

            _spriteBatch.Begin();

            switch (gameState)
            {
                case GameStates.TitleScreen://Main screen art
                    GraphicsDevice.Clear(Color.Maroon);
                    _spriteBatch.DrawString(titleFont, "MALPRACTICE MAKES PERFECT", titlePos, Color.Black);
                    _spriteBatch.DrawString(subtitleFont, "-Team Borderline Doctors-", new Vector2(subtitlePos.X - 1.5f, subtitlePos.Y - 1.5f), Color.White);
                    _spriteBatch.DrawString(subtitleFont, "-Team Borderline Doctors-", new Vector2(subtitlePos.X + 1.5f, subtitlePos.Y + 1.5f), Color.Black);
                    _spriteBatch.DrawString(subtitleFont, "-Team Borderline Doctors-", subtitlePos, Color.Red);
                    _spriteBatch.DrawString(subtitleFont, "LEFT CLICK TO START", new Vector2(580, 850), Color.OrangeRed);
                    break;


                case GameStates.Instructions://Instructions text
                    GraphicsDevice.Clear(Color.Black);
                    _spriteBatch.DrawString(titleFont, "We Need a Doctor!", new Vector2(103, 103), Color.WhiteSmoke);
                    _spriteBatch.DrawString(titleFont, "We Need a Doctor!", new Vector2(100, 100), Color.DarkRed);
                    _spriteBatch.DrawString(subtitleFont, "You are the only doctor in town", new Vector2(100, 250), Color.Maroon);
                    _spriteBatch.DrawString(mediumFont, "You need to find the ideal solution to all the towns ails", new Vector2(100, 370), Color.Crimson);
                    _spriteBatch.DrawString(mediumFont, "Drag items from your inventory to customers," +
                        " Try and find the best solution from the items in your inventory", new Vector2(100, 430), Color.Crimson);
                    _spriteBatch.DrawString(mediumFont, "Combine items by dragging one item onto another", new Vector2(100, 490), Color.Crimson);
                    _spriteBatch.DrawString(mediumFont, "Bad solutions lower your reputation (to be added)", new Vector2(100, 550), Color.Crimson);
                    _spriteBatch.DrawString(mediumFont, "Buy items from the shop on the right", new Vector2(100, 610), Color.Crimson);
                    _spriteBatch.DrawString(subtitleFont, "LEFT CLICK TO START THE DAY", new Vector2(100, 950), Color.Maroon);
                    break;

                case GameStates.GameScene:

                    _spriteBatch.Draw(sky, skyRect, Color.White);
                    _spriteBatch.Draw(sky, skyRect2, Color.White);
                    _spriteBatch.Draw(cloud, cloudRect, Color.White);
                    _spriteBatch.Draw(cloud, cloudRect2, Color.White);
                    _spriteBatch.Draw(ground, groundRect, Color.White);

                    _spriteBatch.Draw(office, officeLocation, Color.White);

                    myInventory.DrawScene(_spriteBatch);

                    GreenScenario.Draw(_spriteBatch);

                    //INVENTORY DRAWING
                    myShop.Draw(_spriteBatch);

                    myInventory.DrawScene(_spriteBatch);

                    if (theMessenger != null)
                    {
                        theMessenger.Draw(_spriteBatch);
                    }

                    //draw reputation
                    _spriteBatch.DrawString(smallSubtitleFont, "Reputation:", new Vector2(10, 20), Color.Black);
                    _spriteBatch.Draw(joobi, new Rectangle(190, 30, reputation, 20), Color.Black);
                    _spriteBatch.DrawString(smallSubtitleFont, "Money:", new Vector2(10, 50), Color.Black);
                    _spriteBatch.DrawString(smallSubtitleFont, $"${money:N2}", new Vector2(111, 51), Color.DarkGoldenrod);
                    _spriteBatch.DrawString(smallSubtitleFont, $"${money:N2}", new Vector2(110, 50), Color.Gold);

                    //draw console
                    _spriteBatch.DrawString(itemAmountFont, consoleLog, new Vector2(1500, 10), Color.Black);

                    //draw hoverover
                    if (theMessenger == null)
                    {
                        //hover over slot
                        if (highlightedSlot !=  null && !highlightedSlot.IsEmpty)
                        {
                            MessageBox.DrawItemLabel(_spriteBatch, joobi, itemAmountFont, highlightedSlot.ItemName, new Vector2(mouseState.X + 10, mouseState.Y + 10), Color.White);
                        }
                    }
                    //holding item
                    else
                    {
                        if (highlightedSlot == null || highlightedSlot.IsEmpty || highlightedSlot.ItemName == theMessenger.Item.ItemName)
                        {
                            MessageBox.DrawItemLabel(_spriteBatch, joobi, itemAmountFont, theMessenger.Item.ItemName, new Vector2(mouseState.X + 10, mouseState.Y + 10), Color.White);
                        }
                        else
                        {
                            if (GetItemCombo(theMessenger.Item, highlightedSlot.Item, out _).Count > 0)
                            {
                                MessageBox.DrawItemPreviews(_spriteBatch, GetItemCombo(theMessenger.Item, highlightedSlot.Item, out _), mouseState, Color.Black);
                            }
                            else
                            {
                                MessageBox.DrawItemLabel(_spriteBatch, joobi, itemAmountFont, "(incompatible)", new Vector2(mouseState.X + 10, mouseState.Y + 10), Color.White);
                            }
                        }
                    }

                    break;

                case GameStates.GameOver://Game over screen art
                    GraphicsDevice.Clear(Color.Black);
                    for (int i = 0; i < starsLoc.Count; i++)
                    {
                        _spriteBatch.Draw(star, starsLoc[i], Color.OrangeRed);
                    }
                    _spriteBatch.DrawString(titleFont, "YOU GOT RUN OUT OF TOWN", new Vector2(100,140), Color.Red);
                    _spriteBatch.DrawString(subtitleFont, "Your Reputation Sank Too Low", new Vector2(105, 260), Color.Tomato);
                    _spriteBatch.DrawString(mediumFont, "Left Click To Try Again", new Vector2(105, 360), Color.Tomato);
                    _spriteBatch.DrawString(mediumFont, "Your Final Stats: Reputation: " + reputation + $" Money: ${money:N2}", new Vector2(105, 415), Color.Tomato);

                    break;

                case GameStates.DayEnd://Day end art
                    GraphicsDevice.Clear(Color.DarkBlue);
                    for (int i = 0; i < starsLoc.Count; i++)
                    {
                        _spriteBatch.Draw(star, starsLoc[i], Color.White);
                    }
                    _spriteBatch.DrawString(titleFont, "The Day Is Over", new Vector2(150, 150), Color.DarkGoldenrod);
                    _spriteBatch.DrawString(subtitleFont, "Congrats You Survived The Day!", new Vector2(150, 300), Color.Gold);
                    _spriteBatch.DrawString(subtitleFont, "LEFT CLICK TO PLAY AGAIN", new Vector2(150, 400), Color.Yellow);
                    _spriteBatch.DrawString(smallSubtitleFont, "Your Final Stats: Reputation: " + reputation + $" Money: ${money:N2}"
                        , new Vector2(150, 500), Color.LightYellow);
                    break;

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}