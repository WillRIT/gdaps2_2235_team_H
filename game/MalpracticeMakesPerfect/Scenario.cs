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
        private int slotNum;
        private Item[] itemSlots; //the slots that you can place items in
        private List<Solution> solutions;
        private Texture2D personSprite;
        private string godModeText;
        private Vector2 spawnPoint = new Vector2(0, 650);
        private Vector2 destinationPoint = new Vector2(800, 650);

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
        public Scenario(string sceneMessage, int slotNum, List<Solution> solutions, Texture2D personSprite, string godModeText)
        {
            this.sceneMessage = sceneMessage;
            this.slotNum = slotNum;
            this.solutions = solutions;
            this.personSprite = personSprite;
            this.godModeText = godModeText;
            itemSlots = new Item[slotNum];
        }

        public void Update()
        {
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

                    break;

                case ScenarioState.Leaving:

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

                    break;

                case ScenarioState.Leaving:

                    break;
            }
        }
    }
}
