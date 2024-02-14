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

        public Solution(List<Item> items, double score)
        {
            this.items = items;
            this.score = score;
        }
    }
}
