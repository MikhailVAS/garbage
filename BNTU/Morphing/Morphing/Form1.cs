using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Morphing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Image img = Image.FromFile(@"d:\Git\garbage\BNTU\Morphing\Morphing\IronMan.jpg");
            Bitmap bit_map = new Bitmap(img);
            pic_original1.Image = img;
            pic_processed.Image = effect(bit_map, 0.3);
        }

        private Byte transformation(Byte color, double p)
        {
            Byte result;

            //Gamma correction
            result = (byte)(Math.Pow(((double)color / 255), p) * 255);
            return result;
        }


        private Bitmap effect(Bitmap Image, double p)
        {
            int Height = Image.Height;
            int Width = Image.Width;

            Bitmap result = Image;

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Color Pixel = Image.GetPixel(j, i);
                    Byte r = Pixel.R;
                    Byte g = Pixel.G;
                    Byte b = Pixel.B;

                    Byte new_r, new_g, new_b;
                    new_r = transformation(r, p);
                    new_g = transformation(g, p);
                    new_b = transformation(b, p);
                    Color new_pixel = Color.FromArgb(255, new_r, new_g, new_b);
                    result.SetPixel(j, i, new_pixel);
                }

            }
            return result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Image img1 = Image.FromFile(@"d:\Git\garbage\BNTU\Morphing\Morphing\IronMan.jpg");
            Bitmap bit_map1 = new Bitmap(img1);
            pic_original1.Image = img1;
            Image img2 = Image.FromFile(@"d:\Git\garbage\BNTU\Morphing\Morphing\Joke.jpg");
            Bitmap bit_map2 = new Bitmap(img2);
            pic_original2.Image = img2;
            pic_processed.Image = bit_map1;
        }
    }
}
