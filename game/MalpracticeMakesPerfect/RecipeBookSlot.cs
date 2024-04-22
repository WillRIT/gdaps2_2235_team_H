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
        public Dictionary<Recipe, bool> Recipes { get; set; }

        /// <summary>
        /// Create recipebook slot
        /// </summary>
        /// <param name="position">Dimensions/position of slot</param>
        /// <param name="item">Item in slot</param>
        public RecipeBookSlot(Rectangle position, Item item)
            : base(item.Asset, position)
        {
            Recipes = new Dictionary<Recipe, bool>();
            this.Item = item;
        }

        public override string ToString()
        {
            return $"{Item.ItemName}: " + (Unlocked ? "Unlocked" : "Locked") + $", {Recipes.Count} recipe(s)";
        }

        public override void Update()
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, new Rectangle((int)(position.X + position.Width * 0.1), (int)(position.Y + position.Width * 0.1), (int)(position.Width * 0.8), (int)(position.Width * 0.8)), (Unlocked) ? Color.White : Color.Black);
        }
    }
}
