using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MalpracticeMakesPerfect
{
    enum GameStates
    {
        TitleScreen,
        GameScene,
        GameShop
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D jam;
        private Texture2D Joobi;

        private Rectangle itemPos;

        private Item diamond;
        private Slot diamondSlot;
        private Slot emptySlot;
        private Inventory myInventory;
        private Texture2D slotSprite;
        private Texture2D diamondSprite;
        private Texture2D joobi;

        private SpriteFont itemAmountFont;

        private Draggable testDrag;

        private Slot highlighted;
        


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

            slotSprite = Content.Load<Texture2D>("slot");
            diamondSprite = Content.Load<Texture2D>("diamond");
            itemAmountFont = Content.Load<SpriteFont>("item-amount");
            joobi = Content.Load<Texture2D>("joobi");


            diamond = new Item(diamondSprite, new Rectangle(0, 0, 50, 50),"Diamond", "Shiny", 90, true);
            diamondSlot = new Slot(slotSprite, new Rectangle(10, 10, 50, 50), itemAmountFont, diamond, 2);
            emptySlot = new Slot(slotSprite, new Rectangle(10, 100, 50, 50), itemAmountFont);

            myInventory = new Inventory(joobi, new Rectangle(500, 500, 500, 200), itemAmountFont, slotSprite);

            testDrag = new Draggable(joobi, new Rectangle(300, 100, 70, 70));

            


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            testDrag.Update();

            myInventory.Update();
            emptySlot.Update();
            diamondSlot.Update();

            bool existsHighlight = false;
            foreach (Slot s in myInventory.Hotbar)
            {
                if (s.Hovered)
                {
                    existsHighlight = true;
                    highlighted = s;
                }
            }
            if (!existsHighlight)
            {
                highlighted = null;
            }

            if (testDrag.Placing && existsHighlight)
            {
                testDrag.SnapIntersect(highlighted);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Crimson);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            diamondSlot.Draw(_spriteBatch);

            emptySlot.Position = new Rectangle(100, 100, 50, 50);
            emptySlot.Draw(_spriteBatch);

            myInventory.DrawScene(_spriteBatch);

            testDrag.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}