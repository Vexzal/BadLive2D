using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Interp2D
{
    class SpinningTriangleControl : GameControl
    {
        
        
        int oldWidth;
        int newWidth;

        int oldHeight;
        int newHeight;

        BasicEffect effect;
        Stopwatch timer;
        

        public readonly VertexPositionColor[] Verticies =
        {
            new VertexPositionColor(new Vector3(-1,-1,0),Color.Black),
            new VertexPositionColor(new Vector3(1,-1,0),Color.Black),
            new VertexPositionColor(new Vector3(0,1,0),Color.Black),
        };
        
        protected override void Initialize()
        {
            base.Initialize();

            effect = new BasicEffect(GraphicsDevice);

            effect.VertexColorEnabled = true;

            //start animation timer.
            timer = Stopwatch.StartNew();

            Application.Idle += delegate { Invalidate(); };
        }
        protected override void Update(GameTime gameTime)
        {
            
            
            
           // float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float time = (float)timer.Elapsed.TotalSeconds;
            Console.WriteLine(time);

            float yaw = time * 0.7f;
            float pitch = time * 0.8f;
            float roll = time * 0.6f;

            float aspect = GraphicsDevice.Viewport.AspectRatio;


            effect.World = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);

            effect.View = Matrix.CreateLookAt(new Vector3(0, 0, -5), Vector3.Zero, Vector3.Up);

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 10);

        }
        protected override void Draw(GameTime gameTime)
        {
                                              
            GraphicsDevice.Clear(Color.Beige);                                    

            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            effect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Verticies, 0, 1);
        }
    }
}
