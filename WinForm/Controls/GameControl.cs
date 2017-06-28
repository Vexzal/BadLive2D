using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using XKeys = Microsoft.Xna.Framework.Input.Keys;

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
            ControlKeyboard.SetKeys(_keys);
            _gameTime = new GameTime(_timer.Elapsed, _timer.Elapsed - _elapsed);
            _elapsed = _timer.Elapsed;
            

            Update(_gameTime);
            Draw(_gameTime);
            Invalidate();
            
        }
        protected abstract void Update(GameTime gameTime);
        protected abstract void Draw(GameTime gameTime);

        #region input
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;

        private List<Microsoft.Xna.Framework.Input.Keys> _keys;

        internal new void ProcessKeyMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                XKeys xkey = KeyboardUtil.ToXna((Keys)m.WParam);
                if (!_keys.Contains(xkey))
                    _keys.Add(xkey);
            }
            else if (m.Msg == WM_KEYUP)
            {
                XKeys xkey = KeyboardUtil.ToXna((Keys)m.WParam);
                if (_keys.Contains(xkey))
                    _keys.Remove(xkey);
            }

        }
        #endregion
    }


}
