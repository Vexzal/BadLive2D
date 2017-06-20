using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Interp2D
{
    class VertexInfo
    {


        Vector3 position;
        Vector3 normal;
        Vector2 textureCoord;
        //Color color1;
        //Color color2;
        //Color color3;
        Color[] colors;
        Byte4 blendIndicies;
        Vector4 blendWeight;



        //VertexPositionColorTexture vertex;

        BoundingSphere selectBounds;

        //what lines start with this vert

        // VertexInfo[] StartLines;

        //lines that end with this vert

        //VertexInfo[] EndLines;

        static int lineCount; //all lines that need to start with this line. 

        short iD;


        bool disposable = false;
        //public VertexInfo(VertexPositionColorTexture vpt, BoundingSphere bound, short id)
        //{
        //    vertex = vpt;
        //    selectBounds = bound;
        //    iD = id;
        //    StartLines = new VertexInfo[0];
        //    EndLines = new VertexInfo[0];
        //}
        public VertexInfo(short ID)
        {
            iD = ID;
            colors = new Color[4];
        }



        /**  
         * need methods for
         * adding a new element to start
         * being add to end line
         * adding to line count
         * deleting element
         * deleting lines 
         * checking for selection
         * moving element ??
         * accessors
         * **/
        //accesors

        //public VertexPositionColorTexture Vertex
        //{
        //    get { return vertex; }
        //    set { vertex = value; }
        //}
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector3 Normal
        {
            get { return normal; }
            set { normal = value; }
        }
        public Vector2 TextureCoord
        {
            get { return textureCoord; }
            set { textureCoord = value; }
        }
        public Color Color1
        {
            get { return colors[0]; }
            set { colors[0] = value; }
        }
        public Color Color2
        {
            get { return colors[1]; }
            set { colors[1] = value; }
        }
        public Color Color3
        {
            get { return colors[2]; }
            set { colors[2] = value; }
        }
        public Color Color4
        {
            get { return colors[3]; }
            set { colors[3] = value; }
        }

        public Byte4 BlendIndices
        {
            get { return blendIndicies; }
            set { blendIndicies = value; }
        }
        public Vector4 BlendWeights
        {
            get { return blendWeight; }
            set { blendWeight = value; }
        }
        public BoundingSphere SelectBounds
        {
            get { return selectBounds; }
            set { selectBounds = value; }
        }
        public static int LineCount
        {
            get { return lineCount; }
        }
        //public int Index
        //{
        //    get { return index; }
        //    set { index = value; }
        //}
        public short ID
        {
            get { return iD; }
        }
        //public VertexInfo[] startLines
        //{
        //    get { return StartLines; }
        //}
        public bool Disposable
        {
            get { return disposable; }
            set { disposable = value; }
        }
        #region Methods
        public VertexPosition getVertexPosition()
        {
            return new VertexPosition(position);
        }
        public VertexPositionColor getVertexPositionColor(int vertColor)
        {
            return new VertexPositionColor(position, colors[vertColor-1]);
        }
        public VertexPositionTexture getVertexPositionTexture()
        {
            return new VertexPositionTexture(position, textureCoord);
        }
        
        public VertexPositionColorTexture getVertexPositionColorTexture(int vertColor)
        {
            return new VertexPositionColorTexture(position, colors[vertColor-1], textureCoord);
        }
        public VertexPositionNormalTexture getVertexPositionNormalTexture()
        {
            return new VertexPositionNormalTexture(position, normal, textureCoord);
        }
        public object getGenericVertex<T>(Type type, int vertColor)
        {
            object outVal = new object();

            if(type  == typeof(VertexPosition))
            {
                outVal = getVertexPosition();
            }
            if(type == typeof(VertexPositionColor))
            {
                outVal = getVertexPositionColor(vertColor);
            }
            if(type == typeof(VertexPositionTexture))
            {
                outVal = getVertexPositionTexture();
            }
            if(type == typeof(VertexPositionColorTexture))
            {
                outVal = getVertexPositionColorTexture(vertColor);
            }
            if(type == typeof(VertexPositionNormalTexture))
            {
                outVal = getVertexPositionNormalTexture();
            }
            return outVal;               
        }
        public BoundingSphere getBoundingSPhere()
        {
            return new BoundingSphere(Position, 16f);
        }

        #endregion
        //new line that this is the start of
        //public void AddLine(VertexInfo endPoint)
        //{
        //    lineCount++;
        //    //ArrayTools.expandArrayOf<VertexInfo>(ref StartLines);
        //    StartLines[StartLines.Length - 1] = endPoint;
        //}
        //public void AddAsEndOfLine(VertexInfo startPoint)
        //{
        //   // ArrayTools.expandArrayOf<VertexInfo>(ref EndLines);
        //    EndLines[EndLines.Length - 1] = startPoint;
        //}

        //~VertexElement()
        //{
        //    clearReferences();
        //}
        //public void clearReferences()
        //{
        //     foreach(VertexInfo element in StartLines)
        //    {
        //        lineCount--;
        //        element.removeEndReference(this);
        //    }
        //    foreach(VertexInfo element in EndLines)
        //    {
        //        lineCount--; 
        //        element.RemoveStartReference(this);
        //    }
        //}

        //public void removeEndReference(VertexInfo Element)
        //{
        //    for(int i = 0; i<EndLines.Length;i++)
        //    {
        //        if( EndLines[i].ID == Element.ID)
        //        {
        //          //  ArrayTools.removeElement<VertexInfo>(ref EndLines, i);
        //        }         
        //    }

        //}

        //public void RemoveStartReference(VertexInfo Element)
        //{
        //    for(int i = 0; i<StartLines.Length;i++)
        //    {
        //        if(StartLines[i].ID == Element.ID)
        //        {
        //           // ArrayTools.removeElement<VertexInfo>(ref StartLines, i);
        //        }
        //    }
        //}

        //public VertexInfo[] fullReference()
        //{
        //    VertexInfo[] returnElement = new VertexInfo[StartLines.Length + EndLines.Length];
        //    for(int i = 0; i<startLines.Length;i++)
        //    {
        //        returnElement[i] = startLines[i];
        //    }
        //    for(int i =0;i<EndLines.Length;i++)
        //    {
        //        returnElement[i + startLines.Length] = EndLines[i];
        //    }
        //    return returnElement;
        //}

    }
}
