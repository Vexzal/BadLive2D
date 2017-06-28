using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Interp2D
{
    public static class ControlKeyboard
    {
        static Keys[] _currentKeys = new Keys[0];
        static Dictionary<int, Keys[]> _arrayCache = new Dictionary<int, Keys[]>();

        public static KeyboardState GetState()
        {
            return new KeyboardState(_currentKeys);
        }

        public static KeyboardState GetState(PlayerIndex playerIndex)
        {
            return new KeyboardState(_currentKeys);
        }

        internal static void SetKeys(List<Keys> keys)
        {
            if(!_arrayCache.TryGetValue(keys.Count, out _currentKeys))
            {
                _currentKeys = new Keys[keys.Count];
                _arrayCache.Add(keys.Count, _currentKeys);
            }

            keys.CopyTo(_currentKeys);
        }

        
    }

}

