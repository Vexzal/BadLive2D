using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interp2D
{
    class Tris
    {
        public short first;
        public short second;
        public short third;
        public short[] indices = new short[3];
       
        public Tris(short a, short b, short c)
        {
            first = a;
            second = b;
            third = c;

            indices[0] = first;
            indices[1] = second;
            indices[2] = third;
        }

        public short[] Incices
        {
            get { return indices; }
        }
    }
}