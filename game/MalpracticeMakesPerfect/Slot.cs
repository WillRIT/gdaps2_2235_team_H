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


        public Slot()
        {
            item = null;
            amount = 0;
        }

        public Slot(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }

        public void AddItem(Item item, int amount)
        {
            if (this.item != null)
            {
                this.item = item;
                this.amount = amount;
            }
        }
    }
}
