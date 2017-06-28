using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Interp2D
{
    class Layer
    {
        Texture2D layerTexture;
        ModelContainer LayerContainer;
        string Name;

        public Layer(Texture2D texture, string name)
        {
            layerTexture = texture;
            LayerContainer = new ModelContainer();
            Name = name;
        }
        public ModelContainer layerContainer
        {
            get { return LayerContainer; }
        }
        public Texture2D Texture
        {
            get { return layerTexture; }
        }
        public string name
        {
            get { return Name; }
        }

    }

    class BezierSurface
    {
        Vector2[] ControlPoints;
        int Height;
        int Width;
        

        public double BinomialCoefficient(int n, int i)
        {
            double nx;
            double ix;
            double nix; 

            for(int j = n-1; i>0; i--)
            {

            }

            return 0;
        }
    }
}
