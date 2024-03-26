﻿using Microsoft.Xna.Framework;
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
        GameScene,
        Instructions,
        GameShop,
        GameOver,
        DayEnd
    }

    /// <summary>
    /// the state of a moved item
    /// </summary>
    enum DragStates
    {
        Return,
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
        private Texture2D office;
        private Vector2 officeLocation;

        private Texture2D shopSlasset;
        private Texture2D shopSlassetB;


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
        private Dictionary<string, Item> itemDict;


        //Sky fields
        private Rectangle skyRect;
        private Rectangle skyRect2;

        //fonts
        private SpriteFont itemAmountFont;
        private SpriteFont titleFont;
        private SpriteFont subtitleFont;
        private SpriteFont smallSubtitleFont;
        private SpriteFont mediumFont;

        //item moving fields
        private List<Draggable> draggables = new List<Draggable>();
        private Draggable testDrag;
        private TempSlot testTemp;
        private Slot highlighted;
        private TempSlot theMessenger;
        private Slot snapBack;

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
        private Scenario JoobiScenario;
        private Texture2D adventurer;

        //scenario List
        private Queue<Scenario> scenarioQueue;

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

            DatabaseManager databaseManager = new DatabaseManager();

            allItems = databaseManager.GetItemsAndRecipes(Content, out allRecipes, out itemDict);

            //Loading textures
            slotSprite = Content.Load<Texture2D>("slot");
            diamondSprite = Content.Load<Texture2D>("diamond");
            itemAmountFont = Content.Load<SpriteFont>("item-amount");
            joobi = Content.Load<Texture2D>("joobi");
            adventurer = Content.Load<Texture2D>("better_patient");
            sky = Content.Load<Texture2D>("sky");
            office = Content.Load<Texture2D>("Shop Pack V2 4");
            officeLocation = new Vector2(320, 280);

            shopSlasset = Content.Load<Texture2D>("shopslot1");
            shopSlassetB = Content.Load<Texture2D>("shopslot2");


            //Load sky
            skyRect = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            skyRect2 = new Rectangle(_graphics.PreferredBackBufferWidth, 0, _graphics.PreferredBackBufferWidth + 1, _graphics.PreferredBackBufferHeight);

            myInventory = new Inventory(joobi, new Rectangle(500, 500, 500, 200), itemAmountFont, slotSprite);

            theMessenger = null;

            myShop = new Shop(joobi, shopSlasset, shopSlassetB, new Rectangle(1200, 300, 600, 980), allItems, PurchaseItem);


            //menu fonts
            titleFont = Content.Load<SpriteFont>("TitleFont");
            subtitleFont = Content.Load<SpriteFont>("SubtitleFont");
            smallSubtitleFont = Content.Load<SpriteFont>("SmallerSubtitleFont");
            mediumFont = Content.Load<SpriteFont>("MediumFont");
            star = Content.Load<Texture2D>("star.png");

            // Solutions
            List<Solution> solutionList = new List<Solution>();
            JoobiScenario = new Scenario("My Tongue is Green", 2, solutionList, adventurer, "Give me Green Paint", smallSubtitleFont);

            

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

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();

            switch (gameState)
            {
                case GameStates.TitleScreen:
                    reputation = 1600;
                    money = 1000;
                    titlePos.Y += textBounceSpeed;
                    subtitlePos.Y += textBounceSpeed;
                    if(titlePos.Y <= 55|| titlePos.Y >= 80)
                    {
                        textBounceSpeed = -textBounceSpeed;
                    }

                    //changing into Instructions
                    if (mouseState.LeftButton == ButtonState.Released && mousePrev.LeftButton == ButtonState.Pressed)
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
                    if (mouseState.LeftButton == ButtonState.Released && mousePrev.LeftButton == ButtonState.Pressed)
                    {
                        gameState = GameStates.GameScene;
                    }

                        break;

                case GameStates.GameScene:
                    //queuing scenarios
                    scenarioQueue.Enqueue(JoobiScenario);

                    JoobiScenario.Update();

                    //for testing rep decreases with right mouse button
                    if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        reputation -= 10;
                    }

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
                        /* while(scenarioQueue.Count > 0) 
                        {
                            if (scenarioQueue.Peek().Stopped == false)
                            {
                                scenarioQueue.Dequeue();
                            }
                             */
                    }
                    if (scenarioQueue.Count == 0 && reputation >0)
                    {
                        gameState = GameStates.DayEnd;

                    }


                    //moving sky background
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

                    //INVENTORY HANDLING!!
                    myInventory.Update();

                    myShop.Update();

                    //whether or not a slot is being highlighted
                    bool existsHighlight = false;
                    //the "return" state assures that the dragged item is returned to its original place
                    DragStates dragAction = DragStates.Return;
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
                                            dragAction = DragStates.Return;
                                        }
                                        //if dragged item has more
                                        else if (highlighted.Amount > theMessenger.Amount)
                                        {
                                            highlighted.Amount -= theMessenger.Amount;

                                            theMessenger.Item = allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs[0];
                                            outputAmount = theMessenger.Amount;
                                            dragAction = DragStates.Return;
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
                                                if (!placedExcess && ((s != snapBack && dragAction == DragStates.Return) || dragAction != DragStates.Return) && ((s.IsEmpty && !s.IsTrash) || s.IsTrash))
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
                                        dragAction = DragStates.Return;
                                    }

                                    break;
                            }
                        }

                    }

                    //right click to place one item of stack on empty slots or slots with the same as held item
                    if (mouseState.RightButton == ButtonState.Pressed && mousePrev.RightButton == ButtonState.Released && theMessenger != null && existsHighlight)
                    {
                        if ((highlighted.IsEmpty || highlighted.ItemName == theMessenger.Item.ItemName) && theMessenger.Amount > 1)
                        {
                            highlighted.Item = theMessenger.Item;
                            highlighted.Amount++;
                            theMessenger.Amount--;
                        }
                    }

                    if (mouseState.LeftButton == ButtonState.Released && theMessenger != null)
                    {
                        if (dragAction == DragStates.Return)
                        {
                            //assure recipe-output-snapback is not overriding a previously placed item
                            if (snapBack.ItemName == theMessenger.Item.ItemName || snapBack.IsEmpty)
                            {
                                snapBack.Item = theMessenger.Item;
                                snapBack.Amount += theMessenger.Amount;
                            }
                            else if (snapBack.IsTrash)
                            {
                                snapBack.Item = theMessenger.Item;
                                snapBack.Amount = theMessenger.Amount;
                            }
                            else
                            {
                                bool placedExcess = false;
                                foreach (Slot s in myInventory.Hotbar)
                                {
                                    //place in the first available empty slot (if dragged item will not be sent back to that slot) or in the trash
                                    if (!placedExcess && (s != snapBack) && ((s.IsEmpty && !s.IsTrash) || s.IsTrash))
                                    {
                                        s.Item = theMessenger.Item;
                                        s.Amount = theMessenger.Amount;
                                        placedExcess = true;
                                    }
                                }
                            }
                        }

                        theMessenger = null;
                    }

                    break;
                
                case GameStates.GameOver:
                    if (mouseState.LeftButton == ButtonState.Released && mousePrev.LeftButton == ButtonState.Pressed)
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
                    _spriteBatch.DrawString(subtitleFont, "Left Click to Start", new Vector2(600, 850), Color.Black);

                    
                    break;
                case GameStates.Instructions:
                    GraphicsDevice.Clear(Color.Black);
                    _spriteBatch.DrawString(titleFont, "We Need a Doctor!", new Vector2(103, 103), Color.WhiteSmoke);
                    _spriteBatch.DrawString(titleFont, "We Need a Doctor!", new Vector2(100, 100), Color.DarkRed);
                    _spriteBatch.DrawString(subtitleFont, "You are the only doctor in town", new Vector2(100, 250), Color.Maroon);
                    _spriteBatch.DrawString(mediumFont, "You need to find the ideal solution to all the towns ails", new Vector2(100, 350), Color.Firebrick);
                    _spriteBatch.DrawString(mediumFont, "Drag items from your inventory to customers," +
                        " Try and find the best solution from the items in your inventory", new Vector2(100, 410), Color.Red);
                    _spriteBatch.DrawString(mediumFont, "Combine items by dragging one item onto another", new Vector2(100, 470), Color.OrangeRed);
                    _spriteBatch.DrawString(mediumFont, "Bad solutions lower your reputation (to be added)", new Vector2(100, 530), Color.Orange);
                    _spriteBatch.DrawString(mediumFont, "Buy items from the shop on the right", new Vector2(100, 590), Color.SandyBrown);
                    _spriteBatch.DrawString(mediumFont, "Left Click to start the day", new Vector2(100, 650), Color.PeachPuff);


                    break;

                case GameStates.GameScene:

                    _spriteBatch.Draw(sky, skyRect, Color.White);
                    _spriteBatch.Draw(sky, skyRect2, Color.White);

                    _spriteBatch.Draw(office, officeLocation, Color.White);

                    myInventory.DrawScene(_spriteBatch);

                    _spriteBatch.DrawString(itemAmountFont, consoleLog, new Vector2(1500, 10), Color.Black);

                    JoobiScenario.Draw(_spriteBatch);

                    //INVENTORY DRAWING
                    myShop.Draw(_spriteBatch);

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
                    _spriteBatch.DrawString(smallSubtitleFont, $"${money:N2}", new Vector2(110, 50), Color.Goldenrod);

                    break;

                case GameStates.GameOver:
                    _spriteBatch.DrawString(titleFont, "you got run out of town", new Vector2(150,150), Color.Black);
                    _spriteBatch.DrawString(subtitleFont, "Left click to try again", new Vector2(150, 300), Color.Black);
                    break;

                case GameStates.DayEnd:
                    GraphicsDevice.Clear(Color.DarkBlue);
                    _spriteBatch.DrawString(titleFont, "The Day is Over", new Vector2(150, 150), Color.DarkGoldenrod);
                    _spriteBatch.DrawString(subtitleFont, "Congrats you survived the day!", new Vector2(150, 300), Color.Gold);
                    _spriteBatch.DrawString(subtitleFont, "LEFT CLICK TO PLAY AGAIN", new Vector2(150, 400), Color.Yellow);
                    _spriteBatch.DrawString(smallSubtitleFont, "Your Final Stats: Reputation: "+reputation+ $" Money: ${money:N2}", new Vector2(150, 500), Color.LightYellow);

                    
                    for(int i = 0; i < starsLoc.Count; i++)
                    {
                        _spriteBatch.Draw(star, starsLoc[i], Color.White);
                    }
                    break;


            }

            

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}