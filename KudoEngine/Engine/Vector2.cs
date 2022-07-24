using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudoEngine.Engine
{
    /// <summary>
    /// <see langword="Type"/>
    /// Contains <see langword="X"/> and <see langword="Y"/> values
    /// </summary>
    public class Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        /// <summary>
        /// Initialize a new Vector2 with values 0, 0
        /// </summary>
        public Vector2()
        {
            X = Zero().X;
            Y = Zero().Y;
        }

        /// <summary>
        /// Initialize a new Vector2 with values
        /// </summary>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Returns <see langword="X"/> and <see langword="Y"/> as 0
        /// </summary>
        public static Vector2 Zero()
        {
            return new(0, 0);
        }
    }
}
