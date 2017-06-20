using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Interp2D
{
    public abstract class GameControl : GraphicsDeviceControl
    {
        GameTime _gameTime;
        Stopwatch _timer;
        TimeSpan _elapsed;

        protected override void Initialize()
        {
            _timer = Stopwatch.StartNew();
            //_gameTime = new GameTime(_timer.Elapsed, _timer.Elapsed - _elapsed);
            Application.Idle += delegate { GameLoop(); };
            
        }

        protected override void Draw()
        {
           // Draw(_gameTime);
        }
        private void GameLoop()
        {
            _gameTime = new GameTime(_timer.Elapsed, _timer.Elapsed - _elapsed);
            _elapsed = _timer.Elapsed;
            
            Update(_gameTime);
            Draw(_gameTime);
            Invalidate();
            
        }
        protected abstract void Update(GameTime gameTime);
        protected abstract void Draw(GameTime gameTime);
    }


}
