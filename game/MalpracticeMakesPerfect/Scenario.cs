﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MalpracticeMakesPerfect
{
    internal class Scenario
    {
        private string sceneMessage;
        private NewSlot slot;
        public NewSlot Slot
        {
            get { return slot; }
            set { slot = value; }
        }
        private List<Solution> solutions;
        private Texture2D personSprite;
        private string godModeText;
        private Vector2 spawnPoint = new Vector2(0, 650);
        private Vector2 destinationPoint = new Vector2(400, 650);
        private SpriteFont font;
        private MouseState mState;
        private Rectangle buttonRect;
        private Texture2D buttonAsset;
        private Button button;
        private bool CureGiven;

        Dictionary<Item, int> cures = new Dictionary<Item, int>();

        public enum ScenarioState
        {
            Walking,
            Waiting,
            Leaving
        }
        public ScenarioState state = ScenarioState.Walking;


        /// <summary>
        /// Creates a scenario
        /// </summary>
        /// <param name="sceneMessage">The dialogue of the person</param>
        /// <param name="slotNum">The amount of slots available for items to give them</param>
        /// <param name="solutions">A list of solutions that could work.</param>
        /// <param name="personSprite">The sprite of the character</param>
        /// <param name="godModeText">Text explaining the solutions</param>
        public Scenario(Texture2D slotAsset,string sceneMessage, int slotNum, List<Item> items, Item cure, Texture2D personSprite, string godModeText, SpriteFont font, Texture2D buttonAsset)
        {
            this.sceneMessage = sceneMessage;
            this.personSprite = personSprite;
            this.godModeText = godModeText;
            this.font = font;
            this.buttonAsset = buttonAsset;


            // Fill the dictionary of cures!
            foreach (Item item in items)
            {
                if (item == cure)
                {
                    cures.Add(cure, 10);
                }
                else
                {
                    cures.Add(item, -5);
                }
            }
            

            slot = new NewSlot(slotAsset, new Rectangle(600, 650, 50, 50), font);
            buttonRect = new Rectangle(600, 700, 100, 60);

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
            if (cures.ContainsKey(slot.Item))
            {
                if (cures[slot.Item] > 0)
                {
                    sceneMessage = $"Oh! A {slot}! Thank you, this looks like it'll work!";
                    CureGiven = true;
                }
                else
                {
                    sceneMessage = $"What kind of Quack doctor are you!??";
                }
            }
            slot.Item = null;
        }

        public void Update()
        {
            mState = Mouse.GetState();
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
                    button.Update();

                    if (!slot.IsEmpty)
                    {
                        sceneMessage = slot.ToString();

                    }
                    if (CureGiven == true)
                    {
                        state = ScenarioState.Leaving;
                    }
                    break;

                case ScenarioState.Leaving:
                    destinationPoint -= new Vector2(4, 0);
                    break;

            }
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
                    MessageBox.DrawItemLabel(sb, buttonAsset, font, sceneMessage, new Vector2 (320, 280), Color.White);
                    slot.Draw(sb);
                    button.Draw(sb);
                    break;

                case ScenarioState.Leaving:
                    sb.Draw(personSprite, destinationPoint, Color.White);
                 
                    break;
            }
        }
    }
}
