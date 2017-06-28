using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MonoTexture = Microsoft.Xna.Framework.Graphics.Texture2D; 
namespace Interp2D
{
    
    public partial class NewLayerDialog : Form 
    {
        public Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice;   
       
        public string returnString;
        public MonoTexture returnTexture;
        public NewLayerDialog()
        {
            
            InitializeComponent();
            
        }
        private void ComboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            
            ComboBox combo = (ComboBox)sender;

            if(combo.SelectedItem.ToString() == "<No Texture>")
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
                returnTexture = Live2DForm.modelTextures[combo.SelectedItem.ToString()];
            }
        }
        private void Button3_Click(object sender, System.EventArgs e)
        {
           
            System.IO.Stream mystream = null;
            dialog.InitialDirectory = "c:\\";

            dialog.RestoreDirectory = true;
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if((mystream = dialog.OpenFile()) != null)
                    {
                        using (mystream)
                        {
                            
                            comboBox1.Items.Add(dialog.FileName);
                            Console.WriteLine("Combobox expanded");
                            returnTexture = MonoTexture.FromStream(graphicsDevice, mystream);
                            //Console.WriteLine("TextureSet");
                            Live2DForm.modelTextures.Add(dialog.FileName, returnTexture);
                            Console.WriteLine("Texture added to list");
                            comboBox1.SelectedIndex = comboBox1.FindStringExact(dialog.FileName);
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("error: could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
        public string getTextBox()
        {
            return this.maskedTextBox1.Text;
        }
        public void setDefaultTexture()
        {
            this.comboBox1.SelectedIndex = 0;
        }
        
    }
}
