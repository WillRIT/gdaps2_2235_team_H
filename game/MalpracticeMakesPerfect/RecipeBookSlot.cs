using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal class RecipeBookSlot : GameObject
    {
        public Item Item { get; private set; }
        public bool Unlocked { get; set; }
        public List<Recipe> Recipes { get; set; }

        public RecipeBookSlot(Rectangle position, Item item)
            : base(item.Asset, position)
        {
        }

        public override void Update()
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, new Rectangle((int)(position.X + position.Width * 0.1), (int)(position.Y + position.Width * 0.1), (int)(position.Width * 0.8), (int)(position.Width * 0.8)), (Unlocked) ? Color.White : Color.Green);
        }
    }
}
