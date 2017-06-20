using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace Interp2D
{
    using GdiColor = System.Drawing.Color;
    using MonoColor = Microsoft. Xna.Framework.Color;
    
    
    public partial class Form1 : Form
    {
        public Form1()
        {                              
            InitializeComponent();

            vertexColor1.SelectedIndex = 0;
            vertexColor2.SelectedIndex = 1;
            vertexColor3.SelectedIndex = 2; 
        }
        void graphicsControl_resizing(object sender, System.EventArgs e)
        {
            
        }
        void vertexColor_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int vertexIndex;

            if (sender == vertexColor1)
                vertexIndex = 0;
            else if (sender == vertexColor2)
                vertexIndex = 1;
            else if (sender == vertexColor3)
                vertexIndex = 2;
            else
                return;

            ComboBox combo = (ComboBox)sender;

            string colorName = combo.SelectedItem.ToString();

            GdiColor gdiColor = GdiColor.FromName(colorName);

            MonoColor monoColor = new MonoColor(gdiColor.R, gdiColor.G, gdiColor.B);

            spinningTriangleControl.Verticies[vertexIndex].Color = monoColor;
        }
        void layer_AddNew(object sender, System.EventArgs e)
        {

        }
    }

    interface ICommand
    {
        void Execute();
        void UnExecute();
    }
    public enum ModelingArrays
    {
        VertexInfo, 
        AvailableID,
        EditGroup
    }


    
}
