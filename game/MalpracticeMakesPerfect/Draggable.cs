using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    public interface Draggable
    {
        Rectangle Rectangle { get; set; }
        Vector2 Position { get; set; }  
    }
}
