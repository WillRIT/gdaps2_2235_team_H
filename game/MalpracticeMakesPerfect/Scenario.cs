using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MalpracticeMakesPerfect
{
    internal abstract class Scenario
    {
        protected string sceneMessage;
        protected int slotNum;
        protected List<Solution> solutions;
        protected Texture2D personSprite;
        protected string godModeText;

    }
}
