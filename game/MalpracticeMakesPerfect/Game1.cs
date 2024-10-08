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
        private Texture2D slotTrash;
        private Texture2D joobi;
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
        private KeyboardState keyboardPrev;

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
        private SpriteFont notifFont;

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
        private Button pauseButton;
        private Button recipeBookButton;
        private Button pauseMenu;
        private bool isPaused;
        private Texture2D pauseArt;
        private Button mainMenuButton;

        //Hint variables
        private Button hintButton;
        private bool hintShown;
        private bool isClicked;
        private Dictionary<string, string> hints;

        //Reputation and Money
        private int maxRep = 1600;
        private int minRep = 0;
        private int reputation;
        private int repChange;


        private int Reputation
        {
            get { return reputation; }
            set
            {
                if (value > maxRep)
                {
                    reputation = maxRep;
                }
                else if (value < minRep)
                {
                    reputation = minRep;
                }
                else
                {
                    reputation = value;
                }
            }
        }

        private double money;

        //scenario List
        private Queue<Scenario> scenarioQueue;

        //misc fields
        private Vector2 path = new Vector2(10f, 400f);
        private float speed = 5.0f;

        private Random rng = new Random();


        private Slot newSnapBack;
        private Slot highlightedSlot;

        private List<Scenario> scenarios;

        private RecipeBook recipeBook;

        private List<Recipe> unlockedRecipes;

        private List<Item> newRecipeNotifItems;
        private float newRecipeNotifTimer;

        private Log myLog;
        

        

        /// <summary>
        /// Constructor
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            //Menu variables
            titlePos = new Vector2(100, 60);
            subtitlePos = new Vector2(450, 180);
            textBounceSpeed = 0.22f;

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

            Reputation = 800;
            money = 500;

            newRecipeNotifItems = new List<Item>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            //Loading textures
            slotSprite = Content.Load<Texture2D>("ui/slot");
            slotTrash = Content.Load<Texture2D>("ui/trash");
            joobi = Content.Load<Texture2D>("ui/joobi");
            
            sky = Content.Load<Texture2D>("background/sky");
            cloud = Content.Load<Texture2D>("background/cloud");
            ground = Content.Load<Texture2D>("background/grass");
            office = Content.Load<Texture2D>("background/Shop Pack V2 4");
            officeLocation = new Vector2(1320, 350);

            shopSlasset = Content.Load<Texture2D>("ui/shopslot1");
            shopSlassetB = Content.Load<Texture2D>("ui/shopslot2");

            //fonts
            itemAmountFont = Content.Load<SpriteFont>("fonts/item-amount");
            titleFont = Content.Load<SpriteFont>("fonts/TitleFont");
            subtitleFont = Content.Load<SpriteFont>("fonts/SubtitleFont");
            smallSubtitleFont = Content.Load<SpriteFont>("fonts/SmallerSubtitleFont");
            mediumFont = Content.Load<SpriteFont>("fonts/MediumFont");
            notifFont = Content.Load<SpriteFont>("fonts/Notif");

            star = Content.Load<Texture2D>("ui/star");

            //GET ITEMS

            allItems = DatabaseManager.GetItemsAndRecipes(Content, out allRecipes, out itemDict);


            //Load sky
            skyRect = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            skyRect2 = new Rectangle(_graphics.PreferredBackBufferWidth, 0, _graphics.PreferredBackBufferWidth + 1, _graphics.PreferredBackBufferHeight);
            cloudRect = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            cloudRect2 = new Rectangle(_graphics.PreferredBackBufferWidth, 0, _graphics.PreferredBackBufferWidth + 1, _graphics.PreferredBackBufferHeight);

            groundRect = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            myInventory = new Inventory(joobi, new Rectangle(700, 550, 588, 288), itemAmountFont, slotSprite, slotTrash, PickUpItem, PutDownItem, PutSingleItem, SetHighlighted);

            theMessenger = null;

            List<Item> shopItems = new List<Item>();
            foreach (Item i in allItems)
            {
                if (i.InShop)
                {
                    shopItems.Add(i);
                }
            }
            myShop = new Shop(joobi, shopSlasset, shopSlassetB, new Rectangle(700, 200, 600, 980), itemAmountFont, shopItems, PurchaseItem);

            //activity log
            myLog = new Log(slotSprite, new Rectangle(744, 694, 420, 100), itemAmountFont);


            // Scenarios
            scenarios = DatabaseManager.GetScenarios(Content, allItems, slotSprite, mediumFont, shopSlassetB, PickUpItem, PutDownItemScenario, SetHighlighted, UpdateStats);

            foreach (Scenario s in scenarios)
            {
                scenarioQueue.Enqueue(s);
            }

            //Setting up pause buttons
            isPaused = false;
            pauseArt = Content.Load<Texture2D>("PauseMenuPlaceHolderArt");
            pauseButton = new Button(sky, new Rectangle(1490, 25, 90, 50), smallSubtitleFont, "Help", Color.WhiteSmoke, Color.Maroon,Color.Yellow);
            pauseButton.OnLeftButton += Pause;
            pauseMenu = new Button(pauseArt, new Rectangle(220, 150, 1500, 800), smallSubtitleFont, "", Color.White, Color.White, Color.White);
            pauseMenu.OnLeftButton += Pause;

            mainMenuButton = new Button(sky, new Rectangle(1810, 25, 90, 50), smallSubtitleFont, "Quit", Color.WhiteSmoke, Color.Maroon, Color.Yellow);
            mainMenuButton.OnLeftButton += QuitToMenu;



            //setup recipe book
            recipeBook = new RecipeBook(slotSprite, new Rectangle(360, 203, 1200, 675), itemAmountFont, allItems, allRecipes);

            recipeBookButton = new Button(sky, new Rectangle(1600, 25, 190, 50), smallSubtitleFont, "Recipe Book", Color.WhiteSmoke, Color.Maroon, Color.Yellow);
            recipeBookButton.OnLeftButton += recipeBook.Show;

            unlockedRecipes = new List<Recipe>();


            //Setting up hints
            hintShown = false;
            isClicked = false;
            hints = new Dictionary<string, string>();
            hintButton = new Button(sky, new Rectangle(150, 150, 160, 50), smallSubtitleFont, "Hint: $20", Color.WhiteSmoke, Color.Maroon, Color.Yellow);
            hintButton.OnLeftButton += Hint;
            DatabaseManager.GetHintList(Content, hints);
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

                        myLog.Text += $"Bought 1 {bought.ItemName} for ${bought.Cost:N2}\n";
                        return;
                    }
                    else
                    {
                        myLog.Text += $"Not enough money for {bought.ItemName}!\n";
                        return;
                    }
                }
            }
            myLog.Text += $"Not enough room in inventory!\n";
        }

        /// <summary>
        /// Pick up item from slot
        /// </summary>
        /// <param name="mySlot">Slot to pick up item from</param>
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

        /// <summary>
        /// Put down stack of items into slot
        /// </summary>
        /// <param name="mySlot">Slot to place itemstack</param>
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

        /// <summary>
        /// Place down single item of itemstack in slot
        /// </summary>
        /// <param name="mySlot">Slot to place item</param>
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

        /// <summary>
        /// Combine held item with item in given slot
        /// </summary>
        /// <param name="mySlot">Slot with item to be combined</param>
        internal void CombineItems(Slot mySlot)
        {
            Item[] recipeInputs = new Item[2];

            bool existsRecipe = !(GetItemCombo(mySlot.Item, theMessenger.Item, out recipeInputs).Count == 0);

            if (existsRecipe)
            {
                //for recipebook
                NewRecipe(allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"]);


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
                myLog.Text += $"Created {outputAmount} {allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[0]}(s)";

                //add excess outputs
                for (int i = 1; i < allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs.Count; i++)
                {
                    //log excess items
                    myLog.Text += $", {outputAmount} {allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[i]}(s)";

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

                myLog.Text += "\n";
            }
        }

        /// <summary>
        /// Check if two items are a valid recipe, put them in the right order for the recipe
        /// </summary>
        /// <param name="item1">First item in recipe</param>
        /// <param name="item2">Second item in recipe</param>
        /// <param name="recipeInputs">Correct order of items for recipe</param>
        /// <returns>Outputs of recipe</returns>
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

        /// <summary>
        /// Unlock a new recipe
        /// </summary>
        /// <param name="recipe">Recipe to be unlocked</param>
        internal void NewRecipe(Recipe recipe)
        {
            if (!unlockedRecipes.Contains(recipe))
            {
                unlockedRecipes.Add(recipe);

                recipeBook.NewRecipe(recipe);

                newRecipeNotifItems = recipe.Outputs;
                newRecipeNotifTimer = 130;
            }
        }

        /// <summary>
        /// Place single item from itemstack
        /// </summary>
        /// <param name="mySlot">Slot for item to be placed</param>
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

        /// <summary>
        /// Set given slot as the highlighted slot
        /// </summary>
        /// <param name="mySlot">Given slot</param>
        internal void SetHighlighted(Slot mySlot)
        {
            highlightedSlot = mySlot;
        }

        /// <summary>
        /// Update reputation and money
        /// </summary>
        /// <param name="money">Money to be added</param>
        /// <param name="rep">Reputation to be added</param>
        internal void UpdateStats(double money, int rep)
        {
            this.money += money;
            Reputation += rep;
            repChange = rep;


        }

        /// <summary>
        /// makes it so that the pause menu either displays or doesnt
        /// </summary>
        private void Pause()
        {
            if (isPaused)
            {
                isPaused = false;
            }
            else
            {
                isPaused = true;
            }
        }

        /// <summary>
        /// Spends money to show a hint for the current scenario
        /// </summary>
        private void Hint()
        {
            if (hintShown)
            {
                hintShown = false;
            }
            else if (!hintShown && money >= 20)
            {
                hintShown = true;
                isClicked = true;
                money -= 20;
            }
        }

        /// <summary>
        /// Reset the game on win/game over for another round
        /// </summary>
        private void Reset()
        {
            Reputation = 800;
            money = 500;

            myInventory = new Inventory(joobi, new Rectangle(700, 550, 588, 288), itemAmountFont, slotSprite, slotTrash, PickUpItem, PutDownItem, PutSingleItem, SetHighlighted);

            theMessenger = null;

            scenarios = DatabaseManager.GetScenarios(Content, allItems, slotSprite, mediumFont, shopSlassetB, PickUpItem, PutDownItemScenario, SetHighlighted, UpdateStats);

            myLog.Clear();

            scenarioQueue.Clear();

            foreach (Scenario s in scenarios)
            {
                scenarioQueue.Enqueue(s);
            }

            isPaused = false;

            hintShown = false;
            isClicked = false;

            unlockedRecipes = new List<Recipe>();

            recipeBook = new RecipeBook(slotSprite, new Rectangle(360, 203, 1200, 675), itemAmountFont, allItems, allRecipes);
            recipeBookButton = new Button(sky, new Rectangle(1600, 25, 190, 50), smallSubtitleFont, "Recipe Book", Color.WhiteSmoke, Color.Maroon, Color.Yellow);
            recipeBookButton.OnLeftButton += recipeBook.Show;
        }

        private void QuitToMenu()
        {
            Reset();
            gameState = GameStates.TitleScreen;
        }
        

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.F) && keyboardPrev.IsKeyUp(Keys.F))
            {
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges();
            }

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
                    
                case GameStates.Instructions:
                    //changing to play state
                    if (mouseState.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
                    {
                        gameState = GameStates.GameScene;
                    }

                        break;

                case GameStates.GameScene:

                    highlightedSlot = null;

                    if (scenarioQueue.Count > 0 && scenarioQueue.Peek().state == Scenario.ScenarioState.Left)
                    {
                        scenarioQueue.Dequeue();

                        hintShown = false;
                        isClicked = false;
                    }

                    if (scenarioQueue.Count != 0)
                    {
                        Scenario currentScenario = scenarioQueue.Peek();
                        if (!isPaused)
                        {
                            currentScenario.Update();
                        }
                    }
                    else
                    {
                        if (reputation > 0)
                        {
                            gameState = GameStates.DayEnd;
                        }
                    }

                    //changing into game over state at the end of the day
                    if (scenarioQueue.Count == 0 && (Reputation <= maxRep/4 || money <= 0))
                    {
                        gameState = GameStates.GameOver;
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

                    //menus etc, things that will not function when paused
                    if (!isPaused)
                    {
                        //things that will not work when recipebook is shown
                        if (!recipeBook.IsShown)
                        {
                            myLog.Update();

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
                        }

                        hintButton.Update();

                        recipeBook.Update();

                        recipeBookButton.Update();
                    }
                 

                    //make recipe timer go away after a certain time
                    if (newRecipeNotifTimer > 0)
                    {
                        newRecipeNotifTimer--;
                    }



                    //Pause logic
                    pauseButton.Update();
                    if (isPaused)
                    {
                        pauseMenu.Update();
                        
                        
                    }
                    mainMenuButton.Update();

                    break;
                
                case GameStates.GameOver:
                    if (mouseState.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
                    {
                        gameState = GameStates.TitleScreen;

                        Reset();
                    }
                    break;

                case GameStates.DayEnd:
                    if (mouseState.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
                    {
                        gameState = GameStates.TitleScreen;

                        Reset();
                    }
                    break;
            }
            

            mousePrev = mouseState;
            keyboardPrev = keyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Crimson);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            switch (gameState)
            {
                case GameStates.TitleScreen://Main screen art
                    GraphicsDevice.Clear(Color.Maroon);
                    _spriteBatch.DrawString(titleFont, "MALPRACTICE MAKES PERFECT", titlePos, Color.Black);
                    _spriteBatch.DrawString(subtitleFont, "-Team Borderline Doctors-", new Vector2(subtitlePos.X - 2f, subtitlePos.Y - 2f), Color.Tomato);
                    _spriteBatch.DrawString(subtitleFont, "-Team Borderline Doctors-", new Vector2(subtitlePos.X + 3f, subtitlePos.Y + 3f), Color.Black);
                    _spriteBatch.DrawString(subtitleFont, "-Team Borderline Doctors-", subtitlePos, Color.Red);
                    _spriteBatch.DrawString(subtitleFont, "LEFT CLICK TO START", new Vector2(580, 850), Color.Tomato);
                    break;


                case GameStates.Instructions://Instructions text
                    GraphicsDevice.Clear(new Color(16,10,8));//Less harsh on the eyes
                    _spriteBatch.DrawString(titleFont, "We Need a Doctor!", new Vector2(103, 103), Color.Red);
                    _spriteBatch.DrawString(titleFont, "We Need a Doctor!", new Vector2(101, 101), Color.DarkRed);
                    _spriteBatch.DrawString(subtitleFont, "You are the only doctor in town", new Vector2(100, 250), Color.Maroon);
                    _spriteBatch.DrawString(mediumFont, "You need to find the best solutions for patients", new Vector2(100, 380), Color.Red);
                    _spriteBatch.DrawString(mediumFont, "Drag items from your inventory to customers" , new Vector2(100, 460), Color.Red);
                    _spriteBatch.DrawString(mediumFont, "Combine items by dragging one onto another", new Vector2(100, 540), Color.Red);
                    _spriteBatch.DrawString(mediumFont, "Bad solutions lower your reputation", new Vector2(100, 630), Color.Red);
                    _spriteBatch.DrawString(mediumFont, "Buy items from the shop on the right", new Vector2(100, 710), Color.Red);
                    _spriteBatch.DrawString(subtitleFont, "LEFT CLICK TO START THE DAY", new Vector2(100, 950), Color.DarkRed);
                    break;

                case GameStates.GameScene:

                    _spriteBatch.Draw(sky, skyRect, Color.White);
                    _spriteBatch.Draw(sky, skyRect2, Color.White);
                    _spriteBatch.Draw(cloud, cloudRect, Color.White);
                    _spriteBatch.Draw(cloud, cloudRect2, Color.White);
                    _spriteBatch.Draw(ground, groundRect, Color.White);

                    _spriteBatch.Draw(office, officeLocation, Color.White);

                    myInventory.DrawScene(_spriteBatch);

                    //INVENTORY DRAWING
                    myShop.Draw(_spriteBatch);

                    myInventory.DrawScene(_spriteBatch);

                    if (scenarioQueue.Count != 0)
                    {
                        scenarioQueue.Peek().Draw(_spriteBatch);
                    }

                    //draw console
                    myLog.Draw(_spriteBatch);

                    if (theMessenger != null)
                    {
                        theMessenger.Draw(_spriteBatch);
                    }

                    //draw reputation
                    _spriteBatch.DrawString(smallSubtitleFont, "Reputation:", new Vector2(10, 20), Color.Black);
                    _spriteBatch.Draw(sky, new Rectangle(190, 30, Reputation, 20), Color.Black);
                    _spriteBatch.DrawString(smallSubtitleFont, "Money:", new Vector2(10, 50), Color.Black);
                    _spriteBatch.DrawString(smallSubtitleFont, $"${money:N2}", new Vector2(111, 51), Color.DarkGoldenrod);
                    _spriteBatch.DrawString(smallSubtitleFont, $"${money:N2}", new Vector2(110, 50), Color.Gold);

                    if(Reputation <= 150)
                    {
                        _spriteBatch.Draw(sky, new Rectangle(190, 30, Reputation, 20), Color.Crimson);
                        
                    }
                    if(reputation == 0)
                    {
                        _spriteBatch.DrawString(smallSubtitleFont, "DANGER", new Vector2(200, 20), Color.Crimson);
                    }

                    

                    //draw notification for new recipe/item
                    if (newRecipeNotifItems != null && newRecipeNotifTimer > 0)
                    {
                        //change opacity as timer gets lower
                        MessageBox.DrawItemPreviews(_spriteBatch, newRecipeNotifItems, new Vector2(1600, 90), 60, Color.White * (newRecipeNotifTimer / 25.0F));
                        MessageBox.DrawMessage(_spriteBatch, joobi, notifFont, "New recipe unlocked!", new Vector2(1600, 155), Color.White, newRecipeNotifTimer / 25.0F);
                    }

                    //draw hoverover
                    if (theMessenger == null)
                    {
                        //hover over slot
                        if (highlightedSlot !=  null && !highlightedSlot.IsEmpty)
                        {
                            MessageBox.DrawMessage(_spriteBatch, joobi, itemAmountFont, highlightedSlot.ItemName, new Vector2(mouseState.X + 10, mouseState.Y + 10), Color.White);
                        }
                    }
                    //holding item
                    else
                    {
                        if (highlightedSlot == null || highlightedSlot.IsEmpty || highlightedSlot.ItemName == theMessenger.Item.ItemName)
                        {
                            MessageBox.DrawMessage(_spriteBatch, joobi, itemAmountFont, theMessenger.Item.ItemName, new Vector2(mouseState.X + 10, mouseState.Y + 10), Color.White);
                        }
                        else
                        {
                            if (GetItemCombo(theMessenger.Item, highlightedSlot.Item, out _).Count > 0)
                            {
                                MessageBox.DrawItemPreviews(_spriteBatch, GetItemCombo(theMessenger.Item, highlightedSlot.Item, out _), mouseState, Color.Black);
                            }
                            else
                            {
                                MessageBox.DrawMessage(_spriteBatch, joobi, itemAmountFont, "(incompatible)", new Vector2(mouseState.X + 10, mouseState.Y + 10), Color.White);
                            }
                        }
                    }


                    pauseButton.Draw(_spriteBatch);
                    recipeBookButton.Draw(_spriteBatch);

                    if (isClicked == false)
                    {
                        hintButton.Draw(_spriteBatch);
                    }
                    else
                    {
                        MessageBox.DrawMessage(_spriteBatch, sky, smallSubtitleFont, scenarioQueue.Peek().GetHint(hints), new Vector2(150, 150), Color.White);
                    }


                    recipeBook.Draw(_spriteBatch);

                    if (isPaused)
                    {
                        pauseMenu.Draw(_spriteBatch);

                    }
                    mainMenuButton.Draw(_spriteBatch);

                    if(scenarioQueue.Count >0 && scenarioQueue.Peek().IsLeaving)
                    {
                        if(repChange> 0)
                        {
                            _spriteBatch.Draw(sky, new Rectangle(290, 52, 170, 35), Color.Black);
                            _spriteBatch.DrawString(smallSubtitleFont, "Rep: +"+repChange, new Vector2(300, 50), Color.LightGreen);
                        }
                        else if(repChange <0)
                        {
                            _spriteBatch.Draw(sky, new Rectangle(290, 52, 170, 35), Color.Black);
                            _spriteBatch.DrawString(smallSubtitleFont, "Rep: " + repChange, new Vector2(300, 50), Color.Red);
                        }
                        else
                        {
                            _spriteBatch.Draw(sky, new Rectangle(290, 52, 230, 35), Color.Black);
                            _spriteBatch.DrawString(smallSubtitleFont, "No Rep change" , new Vector2(300, 50), Color.Yellow);
                        }
                    }
                   
                    break;

                case GameStates.GameOver://Game over screen art
                    GraphicsDevice.Clear(new Color(35, 18, 24));
                    for (int i = 0; i < starsLoc.Count; i++)
                    {
                        _spriteBatch.Draw(star, starsLoc[i], Color.LightSalmon);
                    }
                    _spriteBatch.DrawString(titleFont, "YOU GOT RUN OUT OF TOWN", new Vector2(100,140), Color.Red);
                    if (money <= 0)
                    {
                        _spriteBatch.DrawString(subtitleFont, "You're broke. The banks got ya.", new Vector2(105, 260), Color.Tomato);
                    }
                    else
                    {
                        _spriteBatch.DrawString(subtitleFont, "Your reputation sank too low. Pitchforked.", new Vector2(105, 260), Color.Tomato);
                    }
                    
                    _spriteBatch.DrawString(mediumFont, "Left Click To Try Again", new Vector2(105, 810), Color.Red);
                    _spriteBatch.DrawString(mediumFont, "Your Final Stats: Reputation: " + Reputation + $" Money: ${money:N2}", new Vector2(105, 865), Color.Tomato);

                    break;

                case GameStates.DayEnd://Day end art
                    GraphicsDevice.Clear(new Color(10,18,35));
                    for (int i = 0; i < starsLoc.Count; i++)
                    {
                        _spriteBatch.Draw(star, starsLoc[i], Color.White);
                    }
                    _spriteBatch.DrawString(titleFont, "The Day Is Over", new Vector2(100, 140), Color.Chocolate);
                    _spriteBatch.DrawString(subtitleFont, "Congrats You Survived The Day!", new Vector2(100, 260), Color.Goldenrod);
                    _spriteBatch.DrawString(mediumFont, "LEFT CLICK TO PLAY AGAIN", new Vector2(105, 810), Color.Gold);
                    _spriteBatch.DrawString(mediumFont, "Your Final Stats: Reputation: " + Reputation + $" Money: ${money:N2}"
                        , new Vector2(105, 865), Color.LightYellow);
                    break;

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}