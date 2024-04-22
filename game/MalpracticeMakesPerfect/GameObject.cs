using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal abstract class GameObject
    {
        protected Texture2D asset;
        protected Rectangle position;
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Create game object
        /// </summary>
        /// <param name="asset">Object asset</param>
        /// <param name="position">Object dimensions/position</param>
        public GameObject(Texture2D asset, Rectangle position)
        {
            this.asset = asset;
            this.position = position;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, position, Color.White);
        }

        public abstract void Update();
    }
}
