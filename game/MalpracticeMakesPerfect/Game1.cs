<<<<<<< HEAD
﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
=======
﻿using Microsoft.Xna.Framework;
>>>>>>> parent of cecf2b6 (Starting to mess around with Drawing)
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MalpracticeMakesPerfect
{
    enum GameStates
    {
<<<<<<< HEAD
        TitleScreen,
        GameScene,
        GameShop
=======
        Menu, Game, GameOver
>>>>>>> parent of cecf2b6 (Starting to mess around with Drawing)
    }

    enum DragStates
    {
<<<<<<< HEAD
        Failed,
        Empty,
        Combine
    }
=======
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

            if ()

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

            _spriteBatch.Begin();
            // TODO: Add your drawing code here

            _spriteBatch.Draw(Joobi, item, Color.AliceBlue);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
>>>>>>> parent of cecf2b6 (Starting to mess around with Drawing)

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

            private SpriteFont menuFont;
            private Vector2 patientPath;

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

                List<Slot> slotList = new List<Slot>();

                for (int i = 0; i < 10; i++)
                {
                    slotList.Add(new Slot(slotSprite, new Rectangle(), itemAmountFont, allItems[rng.Next(9)], rng.Next(1, 4)));
                }

                myInventory = new Inventory(joobi, new Rectangle(500, 500, 500, 200), itemAmountFont, slotSprite, slotList);

                theMessenger = null;

                menuFont = Content.Load<SpriteFont>("MenuFont");

                joobi = this.Content.Load<Texture2D>("joobi");

                List<Solution> ScenarioTest = new List<Solution>();

                Scenario JoobiSick = new Scenario("My arms are green! Help!", 2, ScenarioTest, joobi, "Give me Apple");
                // TODO: use this.Content to load your game content here
            }

            protected override void Update(GameTime gameTime)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();


                // TODO: Add your update logic here
                mouseState = Mouse.GetState();

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

                                    //add excess outputs
                                    for (int i = 1; i < allRecipes[$"{recipeInputs[0]},{recipeInputs[1]}"].Outputs.Count; i++)
                                    {
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

                mousePrev = mouseState;


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
                GraphicsDevice.Clear(Color.Crimson);


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
                _spriteBatch.Begin();

                myInventory.DrawScene(_spriteBatch);

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
}