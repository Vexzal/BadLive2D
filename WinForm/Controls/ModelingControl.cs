using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.PersonalTools;

namespace Interp2D
{
    class ModelingControl : GameControl
    {
        
        BasicEffect modelingEffect;
        BasicEffect lineEffect;

        Model backGroundModel;
        Rectangle backGroundBounds;
        Point BoundDimensions;
        Point BoundPosition;

        Texture2D backGroundTexture;
        Texture2D vertTexture;

        Matrix scaleToScreen;
        Matrix userScale;
        Matrix userTranslation;

        Matrix World;
        Matrix View;
        Matrix Projection;

        ModelContainer ActiveContainer; 

        VertexPositionColor[] vertexIndex;                       
        short[] lineIndex;
        short[] trisIndex;
      

        bool moving;

        string currentLayer;

        Dictionary<short, int> indicies = new Dictionary<short, int>();
        
        

        protected override void Initialize()
        {
            base.Initialize();

            modelingEffect = new BasicEffect(GraphicsDevice);
            Vector2[] CursorPositions = ProcGenTexTools.PositionArray(8, 8);

            vertTexture = ProcGenTexTools.CreateTexture(GraphicsDevice, 8, 8, pixel =>
            {
                Vector2 midpoint = new Vector2(4, 4);
                double pointDistance = ProcGenTexTools.calcDistance(CursorPositions[pixel], midpoint);
                if (pointDistance <= (int)(4))
                     return Color.DarkGray;
                else { return Color.Transparent; }
            });
            backGroundTexture = ProcGenTexTools.CreateTexture(GraphicsDevice, 1024, 1024, pixel => Color.Transparent);

            modelingEffect.VertexColorEnabled = true;
            
            vertexIndex = new VertexPositionColor[0];
            lineIndex = new short[0];
            trisIndex = new short[0];

            currentLayer = "";

            initBackModel();

            ActiveContainer.PopulateDataChange += new EventHandler<EventArgs>(modeling_Populate);
        }
        //properties 
        public string CurrentLayer
        {
            get { return currentLayer; }
            set { currentLayer = value; }
        }

        public void initBackModel()
        {
            VertexPositionTexture v1 = new VertexPositionTexture(new Vector3(0, 0, 0), new Vector2(0, 0));
            VertexPositionTexture v2 = new VertexPositionTexture(new Vector3(0, 1, 0), new Vector2(0, 1));
            VertexPositionTexture v3 = new VertexPositionTexture(new Vector3(1, 0, 0), new Vector2(1, 0));
            VertexPositionTexture v4 = new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 1));
            VertexPositionTexture[] Verts = new VertexPositionTexture[4] { v1, v2, v3, v4 };

            List<ModelMeshPart> partList = new List<ModelMeshPart>();
            List<ModelMesh> meshList = new List<ModelMesh>();
            List<ModelBone> boneList = new List<ModelBone>();

            short[] index = new short[6] { 0, 1, 2, 1, 2, 3 };

            VertexBuffer vBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture), Verts.Length, BufferUsage.WriteOnly);
            IndexBuffer iBuffer = new IndexBuffer(GraphicsDevice, typeof(short), 6, BufferUsage.WriteOnly);
            vBuffer.SetData(Verts);
            iBuffer.SetData(index);

            ModelMeshPart part = new ModelMeshPart();
            partList.Add(part);
            ModelMesh mesh = new ModelMesh(GraphicsDevice, partList);
            meshList.Add(mesh);
            backGroundModel = new Model(GraphicsDevice, boneList, meshList);
            backGroundModel.Meshes[0].MeshParts[0].VertexBuffer = vBuffer;
            backGroundModel.Meshes[0].MeshParts[0].IndexBuffer = iBuffer;
            backGroundModel.Meshes[0].MeshParts[0].NumVertices = Verts.Length;
            backGroundModel.Meshes[0].MeshParts[0].StartIndex = 0;
            backGroundModel.Meshes[0].MeshParts[0].PrimitiveCount = 2;
            backGroundModel.Meshes[0].MeshParts[0].Effect = modelingEffect;
        }

        /** 
         * replacing vertexinfo with lines
         * need to manage creating, deleting, and forming tris
         * 
         * creating and deleting should be straight forward
         * as should deleting
         * and it lets me break down vertex info into something more useable 
         * for getting tris creat an array of IDs, check each try for association with inputs, 
         *      then take them and store them in arrays of ids, 
         *      check for comparrisons and populate the tris using the dictionary
         *      works better if I pass the vertex info group instead of just the ID
         * **/

        //commands
       

        
        protected override void Update(GameTime gameTime)
        {
            
        }

        protected override void Draw(GameTime gameTime)
        {
            foreach(ModelMesh mesh in backGroundModel.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.World = World * scaleToScreen * userScale * userTranslation;
                    effect.View = View;
                    effect.Projection = Projection;
                    effect.Texture = backGroundTexture;
                    effect.TextureEnabled = true;
                }
                mesh.Draw();
            }
            if(lineIndex.Length > 0)
            {
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList,
                    vertexIndex,
                    0,
                    vertexIndex.Length,
                    lineIndex,
                    0,
                    lineIndex.Length / 2);
            }

            //throw new NotImplementedException();
            
        }
        public event EventHandler<EventArgs> ReturnToLayer;
       
        
         

        void modeling_Populate(object sender, EventArgs e)
        {
            vertexIndex = ActiveContainer.PopulatePoints<VertexPositionColor>();
            lineIndex = ActiveContainer.populateLines();
            trisIndex = ActiveContainer.populateTris();

        } 
    }

    class ModelContainer
    {
        GraphicsDevice GraphicsDevice;
        //contains all info needed for each control
        VertexInfo[] verticies;
        Lines[] lines;
        Tris[] tris;

        short[] availableID;
        int[] editSelectGroup;
        int editSelectVert;
        int editLineEnd;

        Dictionary<short, int> indicies = new Dictionary<short, int>();

        bool selectState = false;
            
        public ModelContainer(GraphicsDevice graphicsDevice)
        {
            verticies = new VertexInfo[0];
            lines = new Lines[0];
            tris = new Tris[0];

            availableID = new short[1] { 0 };
            editSelectGroup = new int[1];
            editSelectVert = -1;

        }
        #region Properties
        public bool SelectState
        {
            get { return selectState; }
            set { selectState = value; }
        }
        public VertexInfo[] Verticies
        {
            get { return Verticies; }
            set { verticies = value; }
        }
        public Lines[] Lines
        {
            get { return lines; }
            set { lines = value; }
        }
        public Tris[] Tris
        {
            get { return tris; }
            set { tris = value; }

        }
        public short[] AvailableID
        {
            get { return availableID; }
            set { availableID = value; }
        }   
        public int[] EditSelectGroup
        {
            get { return editSelectGroup; }
            set { editSelectGroup = value; }
        }
        public int EditSelectVert
        {
            get { return editSelectVert; }
            set { editSelectVert = value; }
        }
        public int EditLineEnd
        {
            get { return editLineEnd; }
            set { editLineEnd = value; }
        }

        #endregion

        #region ArrayManage
        public void ExpandArray(ModelingArrays key)
        {
            switch(key)
            {
                case (ModelingArrays.VertexInfo):
                    ArrayTools.expandArrayOf(ref verticies);
                    break;                
                case (ModelingArrays.AvailableID):
                    ArrayTools.expandArrayOf(ref availableID);
                    break;
                case (ModelingArrays.EditGroup):
                    ArrayTools.expandArrayOf(ref editSelectGroup);
                    break;
                    
            }
        }
        public void RemoveArray(ModelingArrays key, int index)
        {
            switch(key)
            {
                case (ModelingArrays.VertexInfo):
                    ArrayTools.removeElement(ref verticies, index);
                    break;
                case (ModelingArrays.AvailableID):
                    ArrayTools.removeElement(ref availableID, index);
                    break;
                case (ModelingArrays.EditGroup):
                    ArrayTools.removeElement(ref editSelectGroup, index);
                    break;
            }
        }
        #endregion

        #region internalMethods

        #region generalInternal
        public void cleanColor(Color color)
        {
            for (int i = 0; i < verticies.Length; i++)
            {
                verticies[i].Color4 = color;
            }
        }
        public void initSelectGroup()
        {            
            editSelectGroup = new int[1] { -1 };
        }
        public float CrossProduct(Vector3 point1, Vector3 point2, Vector2 cursor)
        {
            Vector2 truncatedPoint1 = new Vector2(point1.X, point1.Y);
            Vector2 truncatedPoint2 = new Vector2(point2.X, point2.Y);
            float dxc = cursor.X - truncatedPoint1.X;
            float dyc = cursor.Y - truncatedPoint1.Y;

            float dxl = point2.X - point2.X;
            float dyl = point2.Y - point2.Y;


            return dxc * dyl - dyc * dxl;
        }

        #endregion

        #region internalDelete
        public void clearDisposable()
        {
            {
                for (int i = 0; i < verticies.Length; i++)
                {
                    if (verticies[i].Disposable)
                    {
                        ArrayTools.removeElement(ref verticies, i);
                        clearDisposable();
                        return;
                    }
                }
            }
        }
        public void deleteLine(VertexInfo removedElement)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Indices.Length; j++)
                {
                    if (lines[i].Indices[j] == removedElement.ID)
                    {
                        ArrayTools.removeElement(ref lines, i);
                        deleteLine(removedElement);
                        return;
                    }
                }
            }
            return;
        }
        public void deleteTris(VertexInfo removedElement)
        {
            for (int i = 0; i < tris.Length; i++)
            {
                for (int j = 0; j < tris[i].Incices.Length; j++)
                {
                    if (tris[i].Incices[j] == removedElement.ID)
                    {
                        ArrayTools.removeElement(ref tris, i);
                        deleteTris(removedElement);
                        return;
                    }
                }
            }
            return;
        }
        #endregion

        #region populateArrays
        private short[] fullLineReference(VertexInfo info)
        {
           short buffValue = -1;
            short[] bufferArray = new short[0];
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(info.ID, ref buffValue))
                {
                    ArrayTools.expandArrayOf(ref bufferArray);
                    bufferArray[bufferArray.Length - 1] = buffValue;
                }
            }
            return bufferArray;
        }
        public void initLine(VertexInfo a, VertexInfo b)
        {
            ArrayTools.expandArrayOf(ref lines);
            lines[lines.Length - 1] = new Lines(a.ID, b.ID);
        }
        public void initTris(VertexInfo a, VertexInfo b)
        {
            short[] abuffer = fullLineReference(a);
            short[] bBuffer = fullLineReference(b);

            for (int ai = 0; ai < abuffer.Length; ai++)
            {
                for (int bi = 0; bi < bBuffer.Length; bi++)
                {
                    if (abuffer[ai] == bBuffer[bi])
                    {
                        ArrayTools.expandArrayOf(ref tris);
                        tris[tris.Length - 1] = new Tris(a.ID, b.ID, abuffer[ai]);
                    }
                }
            }
        }
        public T[] PopulatePoints<T>()
        {    
            T[] Points = new T[verticies.Length];
            indicies.Clear();
            for(int i = 0; i< Points.Length;i++)
            {
                indicies.Add(verticies[i].ID, i);
                Points[i] = (T)verticies[i].getGenericVertex<T>(typeof(T), 4);
            }
            return Points;
                
        }        
        public short[] populateLines()
        {
            short[] buffIndex = new short[lines.Length * 2];
            if (buffIndex.Length == 0) { return new short[0]; }
            for (int iteration = 0; iteration < lines.Length; iteration++)
            {
                buffIndex[iteration * 2 + 0] = (short)indicies[lines[iteration].first];
                buffIndex[iteration * 2 + 1] = (short)indicies[lines[iteration].second];
            }
            return buffIndex;
        }        
        public short[] populateTris()
        {
            short[] buffIndex = new short[tris.Length * 3];
            if(buffIndex.Length == 0) { return new short[0]; }
            for(int iteration = 0; iteration <tris.Length;iteration++)
            {
                buffIndex[iteration * 3 + 0] = (short)indicies[tris[iteration].first];
                buffIndex[iteration * 3 + 1] = (short)indicies[tris[iteration].second];
                buffIndex[iteration * 3 + 2] = (short)indicies[tris[iteration].third];
            }
            return buffIndex;

        }
        #endregion


        #endregion

        #region Events
        public event EventHandler<EventArgs> PopulateDataChange;
        protected virtual void onPopulateDataChange(EventArgs e)
        {
            if (PopulateDataChange != null)
                PopulateDataChange(this, e);
        }
        public void CallPopulate()
        {
            onPopulateDataChange(new EventArgs()); 
        }
        #endregion
    }


    class ModelingDeleteCommand : ICommand
    {
        ModelContainer _container;
        short[] oldAvailableID;
        int[] oldEditSelectGroup;
        VertexInfo[] oldvVerticies;
        Lines[] oldLines;
        Tris[] oldTris;


        public ModelingDeleteCommand(ModelContainer container)
        {
            _container = container;
            oldAvailableID = _container.AvailableID;
            oldEditSelectGroup = _container.EditSelectGroup;
            oldvVerticies = _container.Verticies;
            oldLines = _container.Lines;
            oldTris = _container.Tris;
        }

        public void Execute()
        {
            if (_container.AvailableID.Length < 1)
            {
                _container.AvailableID = new short[1];
            }
            int IDoffset = _container.AvailableID.Length - 1;
            if (!(_container.Verticies.Length > 0)) { return; }
            if (_container.EditSelectGroup[0] < 0) { return; }
            for (int i = 0; i < _container.EditSelectGroup.Length; i++)
            {
                
                _container.ExpandArray(ModelingArrays.AvailableID);
                _container.AvailableID[IDoffset + i] = _container.Verticies[_container.EditSelectGroup[i]].ID;

                _container.Verticies[_container.EditSelectGroup[i]].Disposable = true;
                _container.deleteLine(_container.Verticies[_container.EditSelectGroup[i]]);
                _container.deleteTris(_container.Verticies[_container.EditSelectGroup[i]]);
            }
            _container.clearDisposable();
            _container.CallPopulate();
            _container.initSelectGroup();
            _container.EditSelectVert = -1;
            _container.SelectState = false;
            _container.EditSelectGroup = _container.EditSelectGroup;           
        }

        public void UnExecute()
        {
            _container.Verticies = oldvVerticies;
            _container.Lines = oldLines;
            _container.Tris = oldTris;
            _container.AvailableID = oldAvailableID;
            _container.EditSelectGroup = oldEditSelectGroup;
            _container.EditSelectVert = oldEditSelectGroup[oldEditSelectGroup.Length - 1];      

        }
    }

    class ModelingSelectSingleCommand : ICommand
    {
        ModelContainer _x;
        VertexInfo[] previousInfo;
        int Selected;
        int previouslySelected;
        int[] oldSelectGroup;

        public ModelingSelectSingleCommand(ModelContainer container, int select )
        {

        }

        public void Execute()
        {
            _x.initSelectGroup();
            _x.cleanColor(Color.White);
            _x.Verticies[Selected].Color4 = Color.Orange;
            _x.EditSelectGroup[0] = Selected;

        }
        public void UnExecute()
        {

        }
    }
    class ModelingSelectGroupCommand : ICommand
    {
        ModelContainer x;
        int Selected;
        int editSelectVert;
        public void Execute()
        {
            for(int i = 0;i<x.EditSelectGroup.Length; i++)
            {
                if(x.EditSelectGroup[i] == Selected)
                {
                    editSelectVert = Selected;
                    return;
                }
            }
        }
        public void UnExecute()
        {

        }
    }
    class ModelingMoveCommand : ICommand
    {
        ModelContainer _container;
        Vector3 moveOffset;

        public ModelingMoveCommand(ModelContainer container, Vector3 offset)
        {
            _container = container;
            moveOffset = offset;
        }

        public void Execute()
        {
            for(int i = 0; i<_container.Verticies.Length;i++)
            {
                _container.Verticies[i].Position += moveOffset;                
            }
            _container.CallPopulate();
        }
        public void UnExecute()
        {
            for(int i = 0; i<_container.Verticies.Length;i++)
            {
                _container.Verticies[i].Position -= moveOffset;
            }
            _container.CallPopulate();
        }
    }
    class AddDisconnectedVert : ICommand
    {
        ModelContainer _container;
        Vector3 Position;
        short[] oldAvailableID;
        
        public AddDisconnectedVert(ModelContainer container, Vector3 position)
        {
            _container = container;
            oldAvailableID = _container.AvailableID;   
            Position = position;
        }

        public void Execute()
        {
            _container.ExpandArray(ModelingArrays.VertexInfo);
            _container.Verticies[_container.Verticies.Length - 1] = new VertexInfo(_container.AvailableID[0]);
            _container.Verticies[_container.Verticies.Length - 1].Position = Position;
            _container.Verticies[_container.Verticies.Length - 1].Color4 = Color.Orange;
            _container.Verticies[_container.Verticies.Length - 1].TextureCoord =new Vector2(Position.X, Position.Y);
            _container.AvailableID[0] = (short)_container.Verticies.Length;
            if(_container.AvailableID.Length>1)
            {
                _container.RemoveArray(ModelingArrays.AvailableID, 0);
            }
            _container.EditSelectGroup[0] = _container.Verticies.Length - 1;
            _container.CallPopulate();
            _container.SelectState = true;

        }
        public void UnExecute()
        {
            _container.RemoveArray(ModelingArrays.VertexInfo, _container.Verticies.Length - 1);
            _container.AvailableID = oldAvailableID;                
            _container.EditSelectGroup[0] = -1;
            _container.SelectState = false;
            _container.CallPopulate();
        }
    }
    class AddConnectedVert : ICommand
    {
        ModelContainer x;
        Vector3 Position;



        public void Execute()
        {
            
            x.cleanColor(Color.White);
            x.ExpandArray(ModelingArrays.VertexInfo);
            x.Verticies[x.Verticies.Length - 1] = new VertexInfo(x.AvailableID[0]);
            x.Verticies[x.Verticies.Length - 1].Position = Position;
            x.Verticies[x.Verticies.Length - 1].Color4 = Color.Orange;
            x.Verticies[x.Verticies.Length - 1].TextureCoord = new Vector2(Position.X, Position.Y);
            

        }
        public void UnExecute()
        {

        }
    }
    class ConnectExistingVert : ICommand
    {
        public void Execute()
        {

        }
        public void UnExecute()
        {

        }
    }
}

