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
        private Rectangle patient;
        

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

            patient = new Rectangle(0, 0, personSprite.Width, personSprite.Height);

            itemSlots = new Item[slotNum];
        }


        /// <summary>
        /// 
        /// </summary>
        public void ScenarioSetup()
        {

        }

        /// <summary>
        /// Draws the patient in thier rectangle and square
        /// </summary>
        /// <param name="sb"></param> Just a Spritebatch
        public void DrawScenario(SpriteBatch sb)
        {
            sb.Draw(personSprite, patient, Color.White);
        }
    }
}
