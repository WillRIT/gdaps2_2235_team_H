using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal class Recipe
    {
        private Item[] inputs;
        private List<Item> outputs;
        public List<Item> Outputs
        {
            get { return outputs; }
        }

        /// <summary>
        /// Creates recipe
        /// </summary>
        /// <param name="inputs">The items being combined</param>
        /// <param name="outputs">The resulting item</param>
        public Recipe(Item[] inputs, List<Item> outputs)
        {
            this.inputs = inputs;
            this.outputs = outputs;
        }

        public override string ToString()
        {
            string recipeString = $"{inputs[0]} + {inputs[1]} = {outputs[0]}";

            for (int i = 1; i < outputs.Count; i++)
            {
                recipeString += $", {outputs[i]}";
            }

            return recipeString;
        }
    }
}
