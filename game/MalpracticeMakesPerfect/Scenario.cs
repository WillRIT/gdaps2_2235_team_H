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
        protected string sceneMessage;
        protected int slotNum;
        protected Item[] itemSlots; //the slots that you can place items in
        protected List<Solution> solutions;
        protected Texture2D personSprite;
        protected string godModeText;

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
    }
}
