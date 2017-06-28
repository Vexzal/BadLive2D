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
    using MonoColor = Microsoft.Xna.Framework.Color;
    public partial class Live2DForm : Form
    {
        NewLayerDialog newLayer;


        public static Dictionary<string, Microsoft.Xna.Framework.Graphics.Texture2D> modelTextures = new Dictionary<string, Microsoft.Xna.Framework.Graphics.Texture2D>();
        Dictionary<string, Layer> Layers = new Dictionary<string, Layer>();
        public Live2DForm()
        {
            
            InitializeComponent();
            newLayer = new NewLayerDialog();
            vertexColor1.SelectedIndex = 0;
            vertexColor2.SelectedIndex = 1;
            vertexColor3.SelectedIndex = 2;
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

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addLayerButton_Click(object sender, EventArgs e)
        {
            newLayer.graphicsDevice = modelingControl.GraphicsDevice;
            if(newLayer.ShowDialog() == DialogResult.OK)
            {
                string text = (newLayer.getTextBox() == "") ? DefaultLayerName("new layer") : newLayer.getTextBox();
                
                layerList.Items.Add(text);
                Layers.Add(text, new Layer(newLayer.returnTexture, text));
                newLayer.setDefaultTexture();
                modelingControl.SetLayer(Layers[text]);
                modelingControl.PrepPopulate();
                
            }
        }
        private string DefaultLayerName(string name)
        {
            int i = 1;
            while(true)
            {
                if(!layerList.Items.Contains(name + i.ToString()))
                {
                    return name + i;
                }
                i++;
            }
        }
        //protected override bool ProcessKeyPreview(ref Message m)
        //{
        //    modelingControl.ProcessKeyMessage(ref m);
        //    return base.ProcessKeyPreview(ref m);
        //}


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
