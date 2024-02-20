using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal class Slot
    {
        private Item item;
        private int amount;

        public Slot(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }
}
