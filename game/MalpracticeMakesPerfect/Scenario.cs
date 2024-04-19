using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MalpracticeMakesPerfect
{
    internal delegate void UpdateStats(double money, int rep);

    internal class Scenario
    {
        private Random rng = new Random();

        private string[] unknownItemResponses = new string[]
        {
            "Yeah, no clue what I'm looking at right now.",
            "I have never seen this in my life.",
            "What?",
            "HUH??"
        };
        private string getUnknownResponse
        {
            get { return unknownItemResponses[rng.Next(unknownItemResponses.Length)];}
        }

        private string name;
        private double money;
        private string sceneMessage;
        private string shownMessage;
        private Slot slot;
        public Slot Slot
        {
            get { return slot; }
            set { slot = value; }
        }

        private Texture2D personSprite;
        private string godModeText;
        private Vector2 spawnPoint = new Vector2(0, 650);
        private Vector2 destinationPoint = new Vector2(400, 650);
        private SpriteFont font;
        private MouseState mState;
        private MouseState mPrev;
        private KeyboardState kState;
        private KeyboardState kPrev;
        private Rectangle buttonRect;
        private Texture2D buttonAsset;
        private Button button;
        private bool CureGiven;
        private bool godModeOn = false;

        public bool GodModeOn
        {
            get { return godModeOn; }
            set { godModeOn = value; }
        }

        Dictionary<string, string[]> cures = new Dictionary<string, string[]>();

        public enum ScenarioState
        {
            Walking,
            Waiting,
            Leaving,
            Left
        }
        public ScenarioState state = ScenarioState.Walking;

        public event UpdateStats UpdateStats;

        /// <summary>
        /// Creates a scenario
        /// </summary>
        /// <param name="sceneMessage">The dialogue of the person</param>
        /// <param name="slotNum">The amount of slots available for items to give them</param>
        /// <param name="solutions">A list of solutions that could work.</param>
        /// <param name="personSprite">The sprite of the character</param>
        /// <param name="godModeText">Text explaining the solutions</param>
        public Scenario(List<Item> items, Texture2D slotAsset, SpriteFont font, Texture2D buttonAsset, OnLeftPress pickUpItem, OnLeftRelease putDownItem, OnHover setHighlight, string name, string sceneMessage, string godModeText, Texture2D personSprite, double money, Dictionary<string, string[]> cures)
        {
            this.name = name;
            this.sceneMessage = sceneMessage;
            shownMessage = sceneMessage;
            this.personSprite = personSprite;
            this.godModeText = godModeText;
            this.money = money;
            this.font = font;
            this.buttonAsset = buttonAsset;
            this.cures = cures;


            // Fill the dictionary of cures!
            foreach (Item i in items)
            {
                if (!cures.ContainsKey(i.ItemName))
                {
                    cures.Add(i.ItemName, new string[] { "0", getUnknownResponse, "??? ok" });
                }
            }
            

            slot = new Slot(slotAsset, new Rectangle(625, 813, 50, 50), font);
            slot.PickUpItem += pickUpItem;
            slot.PutDownItem += putDownItem;
            slot.SetHighlighted += setHighlight;

            buttonRect = new Rectangle(570, 888, 170, 60);

            button = new Button(buttonAsset, buttonRect, font, "SUBMIT", Color.Black, Color.Red, Color.Green);
            button.OnLeftButton += GiveCure;

            CureGiven = false;
        }

        /// <summary>
        /// Checks to see what items are in the slot, based on Dictionary
        /// Depending on the cure, says a small little hint or something,
        /// Then after the left mouse button gets clicked, remove or add to
        /// reputiation and money, change state.
        /// </summary>
        public void GiveCure()
        {
            if (slot.Item != null && cures.ContainsKey(slot.ItemName))
            {
                shownMessage = cures[slot.ItemName][2];

                CureGiven = true;

                UpdateStats(money, int.Parse(cures[slot.ItemName][0]));
            }
            else
            {
                shownMessage = "Look, in the full game I wouldn't be so mad,\nbut you're DONE pal.";

                UpdateStats(0, -2000);
                CureGiven = true;
            }
            slot.Item = null;
        }

        public void Update()
        {
            mState = Mouse.GetState();
            kState = Keyboard.GetState();
            switch (state)
            {
                case ScenarioState.Walking:
                    spawnPoint += new Vector2(4, 0);
                    if (spawnPoint == destinationPoint)
                    {
                        state = ScenarioState.Waiting;
                    }
                    break;

                case ScenarioState.Waiting:
                    slot.Update();

                    // Check for G being Hit, swaps Godmode to the inverse of what it was
                    if (kState.IsKeyDown(Keys.G) && kPrev.IsKeyUp(Keys.G))
                    {
                        godModeOn = !godModeOn;
                    }

                    // Shows the message depending on the Effectiveness of the cure, if slot isn't empty
                    if (!slot.IsEmpty)
                    {
                        shownMessage = cures[slot.ItemName][1];
                    }
                    // Shows the God Mode Text 
                    else if (godModeOn)
                    {
                        shownMessage = godModeText;
                    }
                    // Shows the intital scene text
                    else
                    {
                        shownMessage = sceneMessage;
                    }
                    button.Update();


                    if (CureGiven == true)
                    {
                        state = ScenarioState.Leaving;
                    }

                    
                    break;

                case ScenarioState.Leaving:
                    destinationPoint -= new Vector2(4, 0); // Move off Screen

                    // 
                    if (mState.LeftButton == ButtonState.Pressed && mPrev.LeftButton == ButtonState.Released)
                    {
                        state = ScenarioState.Left;
                    }

                    break;

            }
            mPrev = mState;
            kPrev = kState;
        }

        public void Draw(SpriteBatch sb)
        {
            switch (state)
            {
                case ScenarioState.Walking:
                    sb.Draw(personSprite, spawnPoint, Color.White);
                    break;

                case ScenarioState.Waiting:
                    sb.Draw(personSprite, destinationPoint, Color.White);
                    MessageBox.DrawItemLabel(sb, buttonAsset, font, shownMessage, new Vector2 (220, 280), Color.White);
                    slot.Draw(sb);
                    button.Draw(sb);
                    break;

                case ScenarioState.Leaving:
                    sb.Draw(personSprite, destinationPoint, Color.White);
                    MessageBox.DrawItemLabel(sb, buttonAsset, font, shownMessage, new Vector2(290, 280), Color.White);

                    if (destinationPoint.X < 0)
                    {
                        MessageBox.DrawItemLabel(sb, buttonAsset, font, "(click to continue)", new Vector2(320, 380), Color.White);
                    }
                    break;
            }
        }
    }
}
