using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.PersonalTools;

namespace Interp2D
{
    public enum ModelingCommands
    {
        SelectOne,
        SelectGroup,
        Move,
        Add    
    }

    class ModelingControl : GameControl
    {
        SpriteBatch spriteBatch;

        CommandManager commands;

        ModelingCommands activeCommand;

        System.Drawing.Point oldPoint;
        System.Drawing.Point newPoint;

        VertexPositionTexture[] testTriVert;
        short[] testTriIndex;

        int oldHeight;
        int newHeight;

        int oldWidth;
        int newWidth;

        BasicEffect modelingEffect;
        BasicEffect lineEffect;
        Effect customeModelEffect;
        Model backGroundModel;

        Texture2D backGroundTexture;
        Texture2D vertTexture;

        Matrix ScreenToPoint;
        Matrix BaseTransform;

        Matrix World;
        Matrix View;
        Matrix Projection;

        Matrix DrawOrder;

        ModelContainer ActiveContainer;

        VertexPositionColor[] vertPreview;
        short[] linePreview;
        VertexPositionColor[] vertexIndex;
        short[] lineIndex;
        short[] trisIndex;


        bool moving = false;
        bool Shift = false;
        Vector3 MoveVector = Vector3.Zero;

        string currentLayer;

        Dictionary<short, int> indicies = new Dictionary<short, int>();



        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            

            base.Initialize();

            //Content.RootDirectory = "Content";

            lineEffect = new BasicEffect(GraphicsDevice);
            lineEffect.VertexColorEnabled = true;
            

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
            backGroundTexture = ProcGenTexTools.CreateTexture(GraphicsDevice, 1024, 1024, pixel => Color.Gray);
            ScreenToPoint = Matrix.CreateScale(1.0f / this.Size.Width);
            
            //modelingEffect.VertexColorEnabled = true;

            vertexIndex = new VertexPositionColor[0];
            lineIndex = new short[0];
            trisIndex = new short[0];

            linePreview = new short[0];

            currentLayer = "";

            World = Matrix.Identity;
            View = Matrix.CreateLookAt(new Vector3(0, 0, -0.1f), Vector3.Zero, -Vector3.UnitY);
            Projection = Matrix.CreateOrthographic(this.Size.Width, this.Size.Height, -10, 100);
            BaseTransform = Matrix.CreateTranslation(new Vector3(new Vector2(-.5f, -.5f), 0));
            initBackModel();
            
            this.MouseClick += ModelingControl_MouseClick;
            this.MouseDown += ModelingControl_MouseDown;
            this.MouseUp += ModelingControl_MouseUp;
            this.MouseMove += ModelingControl_MouseMove;
            this.KeyDown += ModelingControl_KeyDown;
            this.KeyUp += ModelingControl_KeyUp;
            this.MouseWheel += ModelingControl_MouseWheel;

            //ActiveContainer.PopulateDataChange += new EventHandler<EventArgs>(modeling_Populate);
        }

        private void ModelingControl_MouseWheel(object sender, MouseEventArgs e)
        {
            Console.WriteLine(e.Delta);
            this.Height += (e.Delta);
            this.Width += (e.Delta );
            Console.WriteLine(this.Width);

            UpdateTransforms();
        }

        private System.Drawing.Point MouseDownLocation;

        private void ModelingControl_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Middle)
            {
                MouseDownLocation = e.Location;
            }
        }
       

        private void ModelingControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (moving)
            {
                moving = false;
                if (MoveVector != Vector3.Zero)
                {
                    commands.ExecuteCommand(new ModelingMoveCommand(ActiveContainer, MoveVector));
                    ActiveContainer.CallPopulate();
                    MoveVector = Vector3.Zero;
                }
            }
        }

        private void ModelingControl_MouseMove(object sender, MouseEventArgs e)
        {

            oldPoint = newPoint;
            newPoint = e.Location;

            if(e.Button == MouseButtons.Middle)
            {
                this.Left = e.X + this.Left - MouseDownLocation.X;
                this.Top = e.Y + this.Top - MouseDownLocation.Y;
            }

            activeCommand = ModelingCommands.Add;
            if (ActiveContainer == null)
                return;
            
            UpdatePreview(MouseToVert(new Vector2(e.Location.X, e.Location.Y)));
           
            if(e.Button != MouseButtons.Left)
            {
                
                foreach (VertexInfo vert in ActiveContainer.Verticies)
                {
                    if (vert.getBoundingSphere().Contains(new Vector3(MouseToVert(new Vector2(newPoint.X, newPoint.Y)), 0)) == ContainmentType.Contains)
                    {

                        if (ActiveContainer.Selected(vert.ID))
                        {
                            activeCommand = ModelingCommands.Move;
                            Cursor.Current = Cursors.Cross;
                        }
                        else
                        {
                            activeCommand = ModelingCommands.SelectOne;
                            Cursor.Current = Cursors.Default;
                        }
                    }
                    else { activeCommand = ModelingCommands.Add; Cursor.Current = Cursors.Default; }
                }
            }
            
            
            if(Shift)
            {
                
                activeCommand = ModelingCommands.SelectGroup;
            }

            if(e.Button == MouseButtons.Left)
            {
                if (moving)
                {

                    System.Drawing.Point deltaPoint = new System.Drawing.Point((newPoint.X - oldPoint.X), (newPoint.Y - oldPoint.Y));
                    MoveVector += Vector3.Transform(new Vector3(new Vector2(deltaPoint.X,deltaPoint.Y), 0), ScreenToPoint);
                    foreach (int i in ActiveContainer.EditSelectGroup)
                    {
                        vertexIndex[i].Position += MoveVector;
                    }
                }
            }
        }
        private void ModelingControl_MouseClick(object sender, MouseEventArgs e)
        {
            
            if (ActiveContainer == null)
                return;
            Console.WriteLine(activeCommand);
            switch (activeCommand)
            {
                case ModelingCommands.Add:
                    //commands.ExecuteCommand(new AddVertCommand(ActiveContainer, ActiveContainer.Selection(newMState.Position.ToVector2()), newMState.Position.ToVector2()));
                    commands.ExecuteCommand(new AddVertCommand(ActiveContainer, ActiveContainer.Selection(MouseToVert(new Vector2(e.Location.X, e.Location.Y))),MouseToVert(new Vector2(e.Location.X, e.Location.Y))));
                    ActiveContainer.CallPopulate();
                    break;
                case ModelingCommands.SelectOne:
                    commands.ExecuteCommand(new ModelingSelectSingleCommand(ActiveContainer, ActiveContainer.Selection(MouseToVert(new Vector2(e.Location.X, e.Location.Y)))));
                    

                    break;
                case ModelingCommands.SelectGroup:
                    commands.ExecuteCommand(new ModelingSelectGroupCommand(ActiveContainer, ActiveContainer.Selection(MouseToVert(new Vector2(e.Location.X, e.Location.Y)))));
                    break;
                case ModelingCommands.Move:
                    moving = true;
                    break;
            }
        }
        private void ModelingControl_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == System.Windows.Forms.Keys.LShiftKey)
            {
                Shift = true;
            }
        }
        private void ModelingControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.LShiftKey)
            {
                Shift = false;
            }
        }

        
        #region properties
        public string CurrentLayer
        {
            get { return currentLayer; }
            set { currentLayer = value; }
        }
        #endregion 
        private void initBackModel()
        {
           
            VertexPositionTexture v1 = new VertexPositionTexture(new Vector3(0, 0, 0), new Vector2(0, 0));
            VertexPositionTexture v2 = new VertexPositionTexture(new Vector3(0, 1, 0), new Vector2(0, 1));
            VertexPositionTexture v3 = new VertexPositionTexture(new Vector3(1, 0, 0), new Vector2(1, 0));
            VertexPositionTexture v4 = new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 1));
            VertexPositionTexture[] Verts = new VertexPositionTexture[4] { v1, v2, v3, v4 };
            testTriVert = Verts;

            List<ModelMeshPart> partList = new List<ModelMeshPart>();
            List<ModelMesh> meshList = new List<ModelMesh>();
            List<ModelBone> boneList = new List<ModelBone>();

            short[] index = new short[6] { 0, 1, 2, 1, 2, 3 };
            testTriIndex = index;
            
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

            VertexPositionTexture[] getData = new VertexPositionTexture[4];


        }
        public void SetLayer(Layer layer)
        {
            commands = new CommandManager();
            
            ActiveContainer = layer.layerContainer;
            
            backGroundTexture = layer.Texture;
            
            currentLayer = layer.name;
        }
        public void PrepPopulate()
        {
            ActiveContainer.PopulateDataChange += new EventHandler<EventArgs>(modeling_Populate);
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

        private Vector2 MouseToVert(Vector2 Mouse)
        {
            return Vector2.Transform(Mouse, ScreenToPoint);
        }
        private Vector2 VertToControl(Vector2 VertPos)
        {
            return Vector2.Transform(VertPos, Matrix.Invert(ScreenToPoint));            
        }
        private void UpdateTransforms()
        {
            Console.WriteLine("UpdateTransforms");
            ScreenToPoint = Matrix.CreateScale(1f / this.Size.Width);
            View = Matrix.CreateLookAt(new Vector3(0, 0, -0.1f), Vector3.Zero, -Vector3.UnitY);
            Projection = Matrix.CreateOrthographic(this.Size.Width, this.Size.Height, -10, 100);
        }
        private void UpdatePreview(Vector2 mousePoint)
        {
            if (ActiveContainer == null)
                return;
            
            VertexPositionColor vert1 = new VertexPositionColor(new Vector3(mousePoint, 0), Color.Gray);
            VertexPositionColor vert2;
            if(ActiveContainer.EditSelectGroup[0] == -1)
            {
                vert2 = vert1;
                
                vertPreview = new VertexPositionColor[2] { vert1, vert2};
                linePreview = new short[2] { 0, 0 };
            }
            else
            {
                {
                    vert2 = new VertexPositionColor(ActiveContainer.Verticies[ActiveContainer.EditSelectGroup[ActiveContainer.EditSelectGroup.Length - 1]].Position, Color.Gray);
                    
                    vertPreview = new VertexPositionColor[2] { vert1, vert2};

                    linePreview = new short[2] { 0, 1 };

                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
           
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            foreach(ModelMesh mesh in backGroundModel.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.World = World * BaseTransform * Matrix.Invert(ScreenToPoint);
                    effect.View = View;
                    effect.Projection = Projection;
                    effect.Texture = backGroundTexture;
                    effect.TextureEnabled = true;
                }
                mesh.Draw();
            }
            


            lineEffect.World = World * BaseTransform* Matrix.Invert(ScreenToPoint);
            lineEffect.View = View;
            lineEffect.Projection = Projection;
            foreach(EffectPass pass in lineEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }
            if(linePreview.Length > 0)
            {
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList,
                    vertPreview,
                    0,
                    vertPreview.Length,
                    linePreview,
                    0,
                    linePreview.Length / 2);
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
            foreach(VertexPositionColor vert in vertexIndex)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(vertTexture, VertToControl(new Vector2(vert.Position.X , vert.Position.Y )) + new Vector2(-4,-4), Color.White);
                spriteBatch.End();
                
            }

            //throw new NotImplementedException();
            
        }
        public event EventHandler<EventArgs> ReturnToLayer;
       
        
         

        void modeling_Populate(object sender, EventArgs e)
        {
            vertexIndex = ActiveContainer.PopulatePoints<VertexPositionColor>();
            Console.WriteLine("VertPopulate");
            lineIndex = ActiveContainer.populateLines();
            trisIndex = ActiveContainer.populateTris();          
        } 
      
        
       
    }

    class ModelContainer
    {
        
        //contains all info needed for each control
        VertexInfo[] verticies;
        Lines[] lines;
        Tris[] tris;

        short[] availableID;
        int[] editSelectGroup;
        
        int editLineEnd;

        Dictionary<short, int> indicies = new Dictionary<short, int>();

        bool selectState = false;
            
        public ModelContainer()
        {
            verticies = new VertexInfo[0];
            lines = new Lines[0];
            tris = new Tris[0];

            availableID = new short[1] { 0 };
            editSelectGroup = new int[1] { -1 };
            

        }
        #region Properties
        public bool SelectState
        {
            get { return selectState; }
            set { selectState = value; }
        }
        public VertexInfo[] Verticies
        {
            get { return verticies; }
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

        #region externalMethods
        public int Selection(Vector2 loc)
        {
            foreach(VertexInfo vert in verticies)
            {
                if(vert.getBoundingSphere().Contains(new Vector3(loc, 0)) == ContainmentType.Contains)
                {
                    return indicies[vert.ID];
                }
            }
            return -1;
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
        public bool Selected(short ID)
        {
            for(int i = 0;i<editSelectGroup.Length;i++)
            {
                if(editSelectGroup[i] == indicies[ID])
                {
                    return true;
                }
            }
            return false;
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
                Console.WriteLine("Populate pop");
                PopulateDataChange(this, e);
        }
        public void CallPopulate()
        {
            Console.WriteLine("Populate");
            onPopulateDataChange(new EventArgs()); 
        }
        #endregion
    }

    #region Commands
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
            
            _container.SelectState = false;
            _container.EditSelectGroup[0] = -1;        
        }

        public void UnExecute()
        {
            _container.Verticies = oldvVerticies;
            _container.Lines = oldLines;
            _container.Tris = oldTris;
            _container.AvailableID = oldAvailableID;
            _container.EditSelectGroup = oldEditSelectGroup;
             

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
            _x = container;
            Selected = select;
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
            _x.Verticies = previousInfo;
            _x.EditSelectGroup = oldSelectGroup;
        }
    }
    class ModelingSelectGroupCommand : ICommand
    {
        ModelContainer x;
        int Selected;

        int[] oldSelection;
        VertexInfo[] oldInfo;

        public ModelingSelectGroupCommand(ModelContainer container, int selected)
        {
            x = container;
            Selected = selected;

            oldSelection = container.EditSelectGroup;
            oldInfo = container.Verticies;
        }
        public void Execute()
        {
            for(int i = 0;i<x.EditSelectGroup.Length; i++)
            {
                if(x.EditSelectGroup[i] == Selected)
                {
                    // editSelectVert = Selected;
                    x.RemoveArray(ModelingArrays.EditGroup, i);
                    return;
                }
            }
            x.ExpandArray(ModelingArrays.EditGroup);
            x.EditSelectGroup[x.EditSelectGroup.Length - 1] = Selected;
            x.Verticies[Selected].Color4 = Color.Orange;
        }
        public void UnExecute()
        {
            x.EditSelectGroup = oldSelection;
            x.Verticies = oldInfo;
        }
    }
    class ModelingDeselectCommand : ICommand
    {
        ModelContainer x;

        int[] oldSelection;

        public ModelingDeselectCommand(ModelContainer container)
        {
            x = container;
            oldSelection = container.EditSelectGroup;
        }
        public void Execute()
        {
            x.initSelectGroup();
            x.EditSelectGroup[0] = -1;
        }
        public void UnExecute()
        {
            x.EditSelectGroup = oldSelection;
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
    class AddVertCommand : ICommand
    {
        ModelContainer x;
        int access;        
        Vector2 loc;

        #region oldValues
        VertexInfo[] oldInfo;
        int[] oldSelect;
        short[] oldID;
        Lines[] oldLines;
        Tris[] oldTris;
        #endregion

        public AddVertCommand(ModelContainer container, int Access, Vector2 Location)
        {
            x = container;
            access = Access;
            loc = Location;

            oldInfo = container.Verticies;
            oldLines = container.Lines;
            oldTris = container.Tris;
            oldSelect = container.EditSelectGroup;
            oldID = container.AvailableID;

        }
        public void Execute()
        {
            Console.WriteLine("Execute add");
            if(x.EditSelectGroup[0] != -1)
            {
                if(access < 0)
                {
                    x.cleanColor(Color.White);
                    x.ExpandArray(ModelingArrays.VertexInfo);
                    x.Verticies[x.Verticies.Length - 1] = new VertexInfo(x.AvailableID[0]);
                    x.Verticies[x.Verticies.Length - 1].Position = new Vector3(loc, 0);
                    x.Verticies[x.Verticies.Length - 1].TextureCoord = loc;
                    x.Verticies[x.Verticies.Length - 1].Color4 = Color.Orange;

                    x.AvailableID[0] = (short)x.Verticies.Length;
                    if(x.AvailableID.Length >1)
                    {
                        x.RemoveArray(ModelingArrays.AvailableID, 0);
                    }
                    //editLineEnd = x.Verticies.Length - 1;
                    x.initLine(x.Verticies[x.EditSelectGroup[x.EditSelectGroup.Length - 1]], x.Verticies[x.Verticies.Length - 1]);
                    x.initSelectGroup();
                    x.EditSelectGroup[0] = x.Verticies.Length - 1;
                }
                else
                {
                    x.cleanColor(Color.White);

                    x.Verticies[access].Color4 = Color.Orange;
                    x.initLine(x.Verticies[x.EditSelectGroup[x.EditSelectGroup.Length - 1]], x.Verticies[access]);
                    x.initTris(x.Verticies[x.EditSelectGroup[x.EditSelectGroup.Length - 1]], x.Verticies[access]);
                    x.EditSelectGroup[0] = access;
                }


            }
            else
            {
                Console.WriteLine("First vert in chain");
                x.ExpandArray(ModelingArrays.VertexInfo);
                x.Verticies[x.Verticies.Length - 1] = new VertexInfo(x.AvailableID[0]);
                x.Verticies[x.Verticies.Length - 1].Position = new Vector3(loc, 0);
                x.Verticies[x.Verticies.Length - 1].TextureCoord = loc;
                x.Verticies[x.Verticies.Length - 1].Color4 = Color.Orange;
                x.AvailableID[0] = (short)x.Verticies.Length;
                if(x.AvailableID.Length >1)
                {
                    x.RemoveArray(ModelingArrays.AvailableID, 0);
                }
                x.EditSelectGroup[0] = x.Verticies.Length - 1;
                Console.WriteLine("Finished Adding");
            }
        }

        public void UnExecute()
        {
            x.Verticies = oldInfo;
            x.Lines = oldLines;
            x.Tris = oldTris;
            x.EditSelectGroup = oldSelect;
            x.AvailableID = oldID;
        }
    }
    #endregion
}
//moving added connected lines to 

