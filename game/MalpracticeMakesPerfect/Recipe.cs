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
    }
}
