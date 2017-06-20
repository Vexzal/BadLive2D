using System;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.PersonalTools
{
    public class ArrayTools
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
                for (int i = index; i < array.Length - 1; i++)
                {
                    array[i] = array[i + 1];
                }
                T[] buffer = array;
                array = new T[array.Length - 1];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = buffer[i];
                }
            }
        }
    }
    public class ProcGenTexTools
    {
        public static double calcDistance(Vector2 pointA, Vector2 pointB)
        {
            return Math.Sqrt((Math.Pow(pointA.X - pointB.X, 2) + Math.Pow(pointA.Y - pointB.Y, 2)));
        }
        public static Vector2[] PositionArray(int Height, int Width)
        {
            int pixel = Height * Width;
            Vector2[] positions = new Vector2[pixel];
            for (int i = 0; i < pixel; i++)
            {
                positions[i] = new Vector2((i / Height) + 1, (i % Width) + 1);
            }
            return positions;
        }
        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
        {
            Texture2D texture = new Texture2D(device, width, height);

            Color[] data = new Color[width * height];
            for (int pixel = 0; pixel < data.Length; pixel++)
            {
                data[pixel] = paint(pixel);
            }

            texture.SetData(data);

            return texture;
        }

    }
}
