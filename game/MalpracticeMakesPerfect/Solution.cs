using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal class Solution
    {
        protected List<Item> items;
        protected double score;

        /// <summary>
        /// Creates a solution
        /// </summary>
        /// <param name="items">Items needed for solution</param>
        /// <param name="score">Reputation score based on solution</param>
        public Solution(List<Item> items, double score)
        {
            this.items = items;
            this.score = score;
        }
    }
}
