using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.PersonalTools;

namespace Interp2D
{
    class Layer
    {
        BasicEffect layertextureEffect;
        GraphicsDevice graphicsDevice;

        Model backGroundModel;
        
        string name; //layer
        VertexInfo[] verticies;
        VertexPositionColorTexture[] vertexIndex;
        int editSelectVert;
        int editLineEnd;
        short[] lineIndex;
        Texture2D texture;
        ModelMeshPart meshPart;
        
        Tris[] trisList;
        short[] trisIndex;
        short[] availableID;
        Dictionary<short, int> indicies = new Dictionary<short, int>();

        int drawOrder; //layer

        int[] editSelectGroup;

        VertexBuffer vertBuff; //layer
        IndexBuffer indexBuff; //layer

        bool selectState = false; //true means there is a selection false means otherwise;
        //constructors

        public Layer(string Name, GraphicsDevice GraphicsDevice)
        {
            graphicsDevice = GraphicsDevice;
            editSelectVert = -1;
            availableID = new short[1] { 0 };
            verticies = new VertexInfo[0];
            availableID[0] = (short)verticies.Length;
            vertexIndex = new VertexPositionColorTexture[0];
            lineIndex = new short[0];
            trisIndex = new short[0];
             trisList = new Tris[0];
            editSelectGroup = new int[1];
            name = Name;
        }

        //accessors 
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public short[] LineIndex
        {
            get { return lineIndex; }            
        }
          public int[] SelectedVerts
        {
            get { return editSelectGroup; }

        } 
        public int EditSelectionVert
        {
            get { return editSelectVert; }
        }
        public VertexPositionColorTexture[] VertexIndex
        {
            get { return vertexIndex; }
        }
        //methods
        public void delete()
        {
            availableID = new short[0];
            if(!(verticies.Length > 0)) { return; }
            if(editSelectGroup[0] < 0) { return; }
            for(int i =0; i<editSelectGroup.Length;i++)
            {
                ArrayTools.expandArrayOf<short>(ref availableID);
                availableID[i] = verticies[editSelectGroup[i]].ID;
                verticies[editSelectGroup[i]].clearReferences();
                verticies[editSelectGroup[i]].Disposable = true;
                deleteTris(verticies[editSelectGroup[i]]); 
            }
            clearDisposable();
            
            populatePoints(verticies);
            populateLines(verticies);
            populateTris();
            initSelectGroup();
            if(editSelectVert >verticies.Length-1)
            {
                editSelectVert = verticies.Length - 1;
            }
            if(verticies.Length == 0)
            {
                editSelectVert = -1;
                selectState = false;
            }
            editSelectGroup[0] = editSelectVert;
        }
        //movement
        public void setMoveBuffer(ref VertexPositionColorTexture[] bufferVert,ref BoundingSphere[] bufferSphere)
        {
            Console.WriteLine("MoveGroupLenght: " + editSelectGroup.Length);
            for (int i = 0; i < editSelectGroup.Length; i++)
            {
                ArrayTools.expandArrayOf<VertexPositionColorTexture>(ref bufferVert);
                ArrayTools.expandArrayOf<BoundingSphere>(ref bufferSphere);
                bufferVert[i] = vertexIndex[editSelectGroup[i]];
                bufferSphere[i] = verticies[editSelectGroup[i]].SelectBounds;
            }
            //bufferVert = generatedPoints[selectedVert];
            //bufferSphere = verticies[selectedVert].SelectBounds;
            
        }
        public void cancelMove(ref VertexPositionColorTexture[] bufferVert, ref BoundingSphere[] bufferSphere)
        {
            Console.WriteLine("In function bufferVert: " + bufferVert.Length);
            for(int i = 0;i<editSelectGroup.Length;i++)
            {
                int index = editSelectGroup[i];
                vertexIndex[index] = bufferVert[i];
                verticies[index].Vertex = bufferVert[i];
                verticies[index].SelectBounds = bufferSphere[i];
                
            }
            bufferVert = new VertexPositionColorTexture[0];
            bufferSphere = new BoundingSphere[0];
        }
        public void updateMove(Point oldMousePos,Point newMousePos)
        {
            Point difference = newMousePos - oldMousePos;
            for(int i = 0;i<editSelectGroup.Length;i++)
            {
                int index = editSelectGroup[i];
                vertexIndex[index].Position += new Vector3(difference.X, difference.Y, 0);
                verticies[index].Vertex = vertexIndex[index];
                verticies[index].SelectBounds = new BoundingSphere(vertexIndex[index].Position, 16f);

            }
        }
        public void initSelectGroup()
        {
            editSelectGroup = new int[1];
        }
        //selection
        public void select(int x )
        {
            if (editSelectGroup.Length > 1) { initSelectGroup(); }
            cleanColor(Color.White);
            verticies[x].Color = Color.Orange;
            vertexIndex[x].Color = Color.Orange;
            editSelectVert = x;
            editSelectGroup[0] = editSelectVert;
            // Console.WriteLine("selected ID: " + verticies[x].ID);
            selectState = true;
        }
        public void selectGroup(int x)
        {
            for (int i = 0; i < editSelectGroup.Length; i++)
            {
                if (editSelectGroup[i] == x)
                {
                    editSelectVert = x;
                    
                    return;
                }
            }
            ArrayTools.expandArrayOf<int>(ref editSelectGroup);
            editSelectGroup[editSelectGroup.Length - 1] = x;
            verticies[x].Color = Color.Orange;
            vertexIndex[x].Color = Color.Orange;
            editSelectVert = x;
            selectState = true;
        }
        public void deselect()
        {
            if(selectState)
            {
                editSelectVert = -1;
                initSelectGroup();
                cleanColor(Color.White);
            }
            else
            {
                editSelectGroup = new int[verticies.Length];
                for(int i = 0;i<verticies.Length;i++)
                {
                    editSelectGroup[i] = i;
                    verticies[i].Color = Color.Orange;
                    VertexIndex[i].Color = Color.Orange;                    
                }
                editSelectVert = verticies.Length - 1;
            }
            selectState = !selectState;
        }
        //addition
        public void addVert(Point location)
        {
            //reset selection group 
            initSelectGroup();
            //check for selection
            int x = checkCursorAccess(location);
            //if more then initial vert
            if(editSelectVert>=0)
            {
                //if not existing vert
                if(x < 0)
                {
                    //reset color of verts;
                    cleanColor(Color.White);
                    //create vertex to pass to new element
                    VertexPositionColorTexture holding = new VertexPositionColorTexture(
                        new Vector3(location.X /*- Game1.halfDisplayWidth*/, location.Y /*- Game1.HalfDisplayHeight*/ , 0),
                        Color.Orange,
                        location.ToVector2());
                    //expand element to add new vert to;
                    ArrayTools.expandArrayOf<VertexInfo>(ref verticies);
                    //create new element
                    verticies[verticies.Length - 1] = new VertexInfo(holding,
                        new BoundingSphere(holding.Position, 16f),
                        availableID[0]);
                    
                    availableID[0] = (short)verticies.Length;
                    if(availableID.Length>1)
                    {
                        ArrayTools.removeElement<short>(ref availableID, 0);
                    }
                    //set index for vert at end of line
                    Console.WriteLine("vertListlength " + (verticies.Length-1));
                    editLineEnd = verticies.Length - 1;
                    populatePoints(verticies);
                }
                //if existing vert
                else
                {
                    //Console.WriteLine("Active");

                    //reset vert colors
                    cleanColor(Color.White);
                    //set selected vert 
                    verticies[x].Color = Color.Orange;
                    vertexIndex[x].Color = Color.Orange;
                    //set existing vert as end line
                    editLineEnd = x;
                    //check for tris;
                    //Console.WriteLine("new line end" + x);
                    //Console.WriteLine("selected vert ind" + editSelectVert);
                    initTris(verticies[editSelectVert], verticies[editLineEnd]);

                }
                //maybe get rid of 
                updateReferences();
                //set end of line vert as active vert
               
                editSelectVert = editLineEnd;
                editSelectGroup[0] = editSelectVert;
                //update lines; (shouldn't you update points first becasue you just added a vert;
                populateLines(verticies);
                
                populateTris();
            }
            //first virt
            else
            {
               // Console.WriteLine("init vert");
                //availableID = new short[1];
                VertexPositionColorTexture holding = new VertexPositionColorTexture(
                    new Vector3(location.X /*- Game1.halfDisplayWidth*/, location.Y/* - Game1.HalfDisplayHeight*/, 0),
                    Color.Orange,
                    location.ToVector2());

                ArrayTools.expandArrayOf<VertexInfo>(ref verticies);

                verticies[verticies.Length - 1] = new VertexInfo(holding, new BoundingSphere(holding.Position, 16f),availableID[0]);
                //Console.WriteLine("init id: " + verticies[verticies.Length - 1].ID);
                availableID[0] = (short)verticies.Length;
                if (availableID.Length > 1)
                {
                    ArrayTools.removeElement<short>(ref availableID, 0);
                }
                editSelectVert = verticies.Length - 1;
                //Console.WriteLine(editSelectVert);
                editSelectGroup[0] = editSelectVert;
                populatePoints(verticies);
            }
            selectState = true;                               
        }

        
        private void updateReferences()
        {
            verticies[editLineEnd].AddAsEndOfLine(verticies[editSelectVert]);
            verticies[editSelectVert].AddLine(verticies[editLineEnd]);
        }
         public void DrawLines(GraphicsDevice graphicsDevice)
        {
            if(lineIndex.Length > 0)
            {
                graphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(
                    PrimitiveType.LineList,
                    vertexIndex,
                    0,
                    vertexIndex.Length,
                    lineIndex,
                    0,
                    lineIndex.Length / 2
                    );
            }
        }
        public void DrawTris(GraphicsDevice graphicsDevice)
        {
            //if(trisIndex.Length > 0)
            //{
            //    vertBuff = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), vertexIndex.Length, BufferUsage.WriteOnly);
            //    vertBuff.SetData<VertexPositionColorTexture>(vertexIndex);
            //    indexBuff = new IndexBuffer(graphicsDevice, typeof(short), trisIndex.Length, BufferUsage.WriteOnly);
            //    indexBuff.SetData<short>(trisIndex);

            //    graphicsDevice.SetVertexBuffer(vertBuff);
            //    graphicsDevice.Indices = indexBuff;


            //    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, trisList.Length);
            //} 
    
            if (trisIndex.Length > 0)
            {

                graphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(
                    PrimitiveType.TriangleList,
                    vertexIndex,
                    0,
                    VertexIndex.Length,
                    trisIndex,
                    0,
                    trisIndex.Length / 3
                    );
            }
        }

        private void cleanColor(Color color)
        {
            for(int i = 0; i<verticies.Length;i++)
            {
                verticies[i].Color = color;
                vertexIndex[i].Color = color;
            }
        }
        private void clearDisposable()
        {
            for(int i = 0;i<verticies.Length;i++)
            {
                if(verticies[i].Disposable)
                {
                    ArrayTools.removeElement(ref verticies, i);
                    clearDisposable();
                    return;
                }
            }
        }
        private void populatePoints(VertexInfo[] element)
        {
            indicies.Clear();
            vertexIndex = new VertexPositionColorTexture[element.Length];
            for(int i = 0;i<vertexIndex.Length;i++)
            {
                
                //element[i].updateReferneces(i);
                //element[i].Index = i;
                 indicies.Add(element[i].ID, i);
                
                vertexIndex[i] = element[i].Vertex;
            }
        }
        private void populateLines(VertexInfo[] element)
        {
            int offset = 0;
            lineIndex = new short[VertexInfo.LineCount * 2];
            if(lineIndex.Length == 0) { return; }
            for(int iteration = 0; iteration<element.Length;iteration++)
            {
                if(iteration!=0)
                {
                    offset += element[iteration - 1].startLines.Length * 2;
                }
                for(int i = 0;i<element[iteration].startLines.Length;i++)
                {
                  
                    lineIndex[i * 2 + offset] = (short)indicies[element[iteration].ID];

                    lineIndex[i * 2 + 1 + offset] = (short)indicies[ element[iteration].startLines[i].ID];
                }
            }
        }
        public void populateTris()
        {
            
            trisIndex = new short[trisList.Length * 3];
            if(trisIndex.Length == 0) { return; }
            for(int iteration = 0;iteration <trisList.Length;iteration++)
            {
                //if(iteration!= 0) { offset += 3; }
                trisIndex[iteration * 3 + 0] = (short)indicies[ trisList[iteration].first.ID];
                trisIndex[iteration * 3 + 1] = (short)indicies[trisList[iteration].second.ID];
                trisIndex[iteration * 3 + 2] = (short)indicies[trisList[iteration].third.ID];             
            }            
        }
        
        public int checkCursorAccess(Point pos) //renamed to checkForSelect, checkCursor Access checks bounds of rectangle 
        {
            for(int i=0; i<verticies.Length;i++)
            {
                if(verticies[i].SelectBounds.Contains(new Vector3(pos.X /*- Game1.halfDisplayWidth*/,pos.Y /*- Game1.HalfDisplayHeight*/,0)) == ContainmentType.Contains)
                {
                    return i;
                }
            }
            return -1;
        }
  
        public void initTris(VertexInfo a, VertexInfo b)
        {
            VertexInfo[] aBuffer = a.fullReference();
            VertexInfo[] bBuffer = b.fullReference();


            for(int ai = 0;ai<aBuffer.Length;ai++ )
            {
                for(int bi = 0; bi<bBuffer.Length;bi++)
                {
                    if(aBuffer[ai].ID == bBuffer[bi].ID )
                    {
                        ArrayTools.expandArrayOf<Tris>(ref trisList);
                        trisList[trisList.Length - 1] = new Tris(a, b, bBuffer[bi]);                        
                    }
                }
            }
        }
        public void deleteTris(VertexInfo removedElement) 
        {
            for(int i = 0;i<trisList.Length;i++)
            {
                for(int j= 0; j<trisList[i].Incices.Length;j++)
                {
                    if(trisList[i].Incices[j].ID == removedElement.ID)
                    {
                        ArrayTools.removeElement<Tris>(ref trisList, i);
                        deleteTris(removedElement);
                        return;
                    }
                }
            }
        }
        public void initBackModel(GraphicsDevice graphicsDevice)
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

            VertexBuffer vBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), Verts.Length,BufferUsage.WriteOnly);
            IndexBuffer iBuffer = new IndexBuffer(graphicsDevice, typeof(short), 6, BufferUsage.WriteOnly);
            vBuffer.SetData<VertexPositionTexture>(Verts);
            iBuffer.SetData<short>(index);

            ModelMeshPart part = new ModelMeshPart();
            partList.Add(part);
            ModelMesh mesh = new ModelMesh(graphicsDevice, partList);
            backGroundModel = new Model(graphicsDevice, boneList, meshList);
            backGroundModel.Meshes[0].MeshParts[0].VertexBuffer = vBuffer;
            backGroundModel.Meshes[0].MeshParts[0].IndexBuffer = iBuffer;
            backGroundModel.Meshes[0].MeshParts[0].NumVertices = Verts.Length;
            backGroundModel.Meshes[0].MeshParts[0].StartIndex = 0;
            backGroundModel.Meshes[0].MeshParts[0].PrimitiveCount = 0;
            backGroundModel.Meshes[0].MeshParts[0].Effect = layertextureEffect;
        }
        public void DrawLayerBack(Matrix world, Matrix view, Matrix projection)
        {
            foreach(ModelMesh mesh in backGroundModel.Meshes )
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.Texture = texture;
                    effect.TextureEnabled = true;

                }
            }
        }      
            
        
    }
}
