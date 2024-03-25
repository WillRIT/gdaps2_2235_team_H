using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal class Shop : GameObject
    {
        private List<Item> items; // list of all items
        private SpriteFont font;

        public Shop(Texture2D asset, Rectangle position, List<Item> items)
            : base(asset, position)
        {
            this.items = items;

            //make slot of every item
            foreach (Item i in items)
            {
            }
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
