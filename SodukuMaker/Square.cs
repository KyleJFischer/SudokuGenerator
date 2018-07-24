using System;
using System.Collections.Generic;
using System.Text;

namespace SodukuMaker
{
    [Serializable]
    class Square
    {
        public int? value;
        public bool cleared = false;
        public int x;
        public int y;

        public Square()
        {
            value = null;
        }
    }
}
