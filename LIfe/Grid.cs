using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    public class Grid
    {
        public Grid(Int32 width, Int32 height)
        {
            Cells = new Int32[width,height];
            Width = width;
            Height = height;
        }

        public Int32[,] Cells { get; set; }

        public Int32 Width { get; private set; }

        public Int32 Height { get; private set; }
    }
}
