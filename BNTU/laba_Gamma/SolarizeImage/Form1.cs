using System;
using System.Drawing;
using System.Windows.Forms;

namespace SolarizeImage
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Image img = Image.FromFile(@"d:\laba_Gamma\Pic.jpg", false);
            Bitmap bit_map = new Bitmap(img);
            pic_original.Image = img;

            String user_median = textBox1.Text; // Получение введеного значения в компонент
            double user_median_value = Convert.ToDouble(user_median);

            if (user_median_value > 255) // Защита от дурака
            {
                user_median_value = 0.9;
            }
            else if (user_median_value > 1)
            {
                user_median_value = user_median_value / 255;
            }
            
            else if (user_median_value < 0)
            {
                user_median_value = 0.001;
            }
            
            pic_processed.Image = effect(bit_map, user_median_value);
        }


        private double[] get_values(double[] arr, int size) {

            double[] new_arr = new double[arr.Length];
            Array.Copy(arr, 0, new_arr, 0, arr.Length); // Клонирование массива 

            double[] result = new double[3];
            int median_index = (int)(size / 2); // Получение индекса медианного значения 
            Array.Sort(new_arr);
            result[0] = new_arr[0];  // Lmin
            result[1] = new_arr[median_index];// Median
            result[2] = new_arr[size-1];// Lmax

            return result;

        }


        private double calculate_gamma(double user_median, double image_median, double image_min, double image_max)
        {

            return Math.Log(image_median, ((user_median - image_min) / (image_max - image_min)));// Вычисление гаммы
        }


        private Bitmap effect(Bitmap Image, double user_median)
        {
            int Height = Image.Height;
            int Width = Image.Width;

            Bitmap result = Image;

            double[] pixels_values = new double[Height * Width];
            double[] values = new double[3];
            double min, max, median;

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Color Pixel = Image.GetPixel(j, i);
                    Byte value = Pixel.R;
                    pixels_values[i * Width + j] = ((((double)(value)))/255); // Нормализация значения пикселя 
                }
            }
            
            values = get_values(pixels_values, Width * Height);
            min = values[0];
            median = values[1];
            max = values[2];
            double gamma = calculate_gamma(user_median, median, min, max);

            for (int i = 0; i < Width * Height; i++)
            {
                pixels_values[i] = (Math.Pow(pixels_values[i], gamma)) * 255; // Расчет результирующего значения пикселей
            }

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Byte color = (Byte)(pixels_values[i * Width + j]);
                    Color new_pixel = Color.FromArgb(255, color, color, color);
                    result.SetPixel(j, i, new_pixel);
                }

            }

            return result;
        }


        private void Form1_Load(object sender, EventArgs e) //Загрузка изображения в компаненты
        {
            Image img = Image.FromFile(@"d:\laba_Gamma\Pic.jpg", false);
            Bitmap bit_map = new Bitmap(img);
            pic_original.Image = img;
            pic_processed.Image = bit_map;
        }
    }
}
