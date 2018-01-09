using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

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
            for (double step=0; step<=1.01; step += 0.05)
            {
                effect((Bitmap)pic_original1.Image.Clone(), (Bitmap)pic_original2.Image.Clone(), step);
            }
        }

        private Byte transformation(Byte color1, Byte color2, double step)
        {
            Byte result;
            result = (byte)(step * color2 + (1 - step) * color1);
            return result;
        }


        private void effect(Bitmap Image1, Bitmap Image2, double step)
        {
            int Height = Image1.Height;
            int Width = Image1.Width;

            Bitmap result = (Bitmap)pic_processed.Image.Clone();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Color Pixel1 = Image1.GetPixel(j, i);
                    Byte r1 = Pixel1.R;
                    Byte g1 = Pixel1.G;
                    Byte b1 = Pixel1.B;

                    Color Pixel2 = Image2.GetPixel(j, i);
                    Byte r2 = Pixel2.R;
                    Byte g2 = Pixel2.G;
                    Byte b2 = Pixel2.B;

                    Byte new_r, new_g, new_b;
                    new_r = transformation(r1, r2, step);
                    new_g = transformation(g1, g2, step);
                    new_b = transformation(b1, b2, step);
                    Color new_pixel = Color.FromArgb(255, new_r, new_g, new_b);
                    result.SetPixel(j, i, new_pixel);
                }

            }
            pic_processed.Image = (Bitmap)result.Clone();
            pic_processed.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Image img1 = Image.FromFile(@"d:\Git\garbage\BNTU\Morphing\Morphing\IronMan.jpg");
            Bitmap bit_map1 = new Bitmap(img1);
            pic_original1.Image = (Bitmap)bit_map1.Clone();
            pic_processed.Image = (Bitmap)bit_map1.Clone();
            Image img2 = Image.FromFile(@"d:\Git\garbage\BNTU\Morphing\Morphing\Joke.jpg");
            Bitmap bit_map2 = new Bitmap(img2);
            bit_map2 = new Bitmap(bit_map2, bit_map1.Size);
            pic_original2.Image = bit_map2;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
