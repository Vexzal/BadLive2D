using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Interp2D
{
    class VertexControl :Control
    {
        public VertexControl(short id)
        {
            ID = id;
        }
        public short ID;
    }
}
