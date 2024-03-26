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
    internal class Scenario
    {
        private string sceneMessage;
        private Slot slot;
        public Slot Slot
        {
            get { return slot; }
            set { slot = value; }
        }
        private List<Solution> solutions;
        private Texture2D personSprite;
        private string godModeText;
        private Vector2 spawnPoint = new Vector2(0, 300);
        private Vector2 destinationPoint = new Vector2(400, 300);
        private SpriteFont font;
        private MouseState mState;


        public enum ScenarioState
        {
            Walking,
            Waiting,
            Leaving
        }
        ScenarioState state = ScenarioState.Walking;


        /// <summary>
        /// Creates a scenario
        /// </summary>
        /// <param name="sceneMessage">The dialogue of the person</param>
        /// <param name="slotNum">The amount of slots available for items to give them</param>
        /// <param name="solutions">A list of solutions that could work.</param>
        /// <param name="personSprite">The sprite of the character</param>
        /// <param name="godModeText">Text explaining the solutions</param>
        public Scenario(Texture2D slotAsset,string sceneMessage, int slotNum, List<Solution> solutions, Texture2D personSprite, string godModeText, SpriteFont font)
        {
            this.sceneMessage = sceneMessage;
            this.solutions = solutions;
            this.personSprite = personSprite;
            this.godModeText = godModeText;
            this.font = font;

            slot = new Slot(slotAsset, new Rectangle(300, 400, 50, 50), font);
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
                    GiveCure();
                    //state = ScenarioState.Leaving;
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
                    sb.DrawString(font, sceneMessage,new Vector2 (320, 280), Color.Black);
                    slot.Draw(sb);
                    break;

                case ScenarioState.Leaving:
                    sb.Draw(personSprite, destinationPoint, Color.White);
                    if (destinationPoint == spawnPoint)
                    {
                        
                    }
                    break;
            }
        }

        /// <summary>
        /// Checks to see what items are in the slot, based on Dictionary
        /// Depending on the cure, says a small little hint or something,
        /// Then after the left mouse button gets clicked, remove or add to
        /// reputiation and money, change state.
        /// </summary>
        public void GiveCure()
        {

        }
    }
}
