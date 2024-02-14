using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal abstract class Recipe
    {
        protected Item[] inputs = new Item[2];
        protected List<Item> outputs = new List<Item>();
    }
}
