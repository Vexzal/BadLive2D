using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interp2D
{
    static class ArrayTools
    {
        public static void expandArrayOf<T>(ref T[] array, int extension = 1)
        {
            T[] buffer = array;
            array = new T[array.Length + extension];
            for (int i = 0; i < buffer.Length; i += extension)
            {
                array[i] = buffer[i];
            }

        }
        public static void removeElement<T>(ref T[] array, int index)
        {            
            {
                for (int i = index; i < array.Length -1; i++)
                {
                    array[i] = array[i + 1];
                }
                T[] buffer = array;
                array = new T[array.Length - 1];
                for(int i = 0; i < array.Length;i++)
                {
                    array[i] = buffer[i];
                }
            }
        }

        //


    }
}
