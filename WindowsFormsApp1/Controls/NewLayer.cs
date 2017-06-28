using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.PersonalTools;
using Microsoft.Xna.Framework.Graphics;


namespace Interp2D
{
    class NewLayer : GraphicsDeviceControl
    {
        SpriteBatch spriteBatch;
        
        public Microsoft.Xna.Framework.Graphics.Texture2D GetTexture(System.IO.Stream mystream)
        {
            return Microsoft.Xna.Framework.Graphics.Texture2D.FromStream(GraphicsDevice, mystream);
        }

        protected override void Draw()
        {
            
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
    }
}
