using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interp2D
{
    class Lines
    {
        public short first;
        public short second;
        short[] indices = new short[2];

        public Lines(short a, short b)
        {
            first = a;
            second = b;
            
            indices[0] = first;
            indices[1] = second;
        }

        public short[] Indices
        {
            get { return indices; }
        }
        public bool Contains(short id, ref short outID)
        {
            if(first == id)
            {
                outID = second;
                return true;
            }
            if(second== id)
            {
                outID = first;
                return true;
            }
            return false;
        }

    }
}
