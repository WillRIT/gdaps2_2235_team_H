using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal abstract class Item
    {
        protected string itemDesc;
        protected enum itemAspects { };
        protected bool inInventory;
        protected Texture2D itemSprite;
    }
}
