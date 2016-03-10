using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
//using System.ComponentModel;
//using System.Text;
//using System.Threading.Tasks;
//using System.Drawing;
using System.Windows.Forms;

namespace aeroflot
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ViewElem();
        }

        Aeroflot st = new Aeroflot();
        List<Aeroflot> sts = new List<Aeroflot>(); //создание объекта Aeroflot

        //public класс, наследник от EventArgs
        //Используется как аргумент для передачи данных
        //в событиях

        public class UserEventArgs : EventArgs
        {
            //public readonly поле класса
            public readonly DataTable SendingTable;
            //Конструктор класса с параметром
            public UserEventArgs(DataTable dt)
            {
                SendingTable = dt;
            }
        }

        public class Aeroflot
        {
            //public readonly поле класса
            public string Name { get; set; }
            public int Reis { get; set; }
            public string Type { get; set; }
        }


        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            HideElem();
            tbName.Text = "";
            tbNReis.Text = "";
            tbType.Text = "";
        }

        void HideElem() // Спрятать элементы
        {
            this.ClientSize = new System.Drawing.Size(320, 150);
            dataGridView1.Visible = false;
            сервисToolStripMenuItem.Visible = false;
            правкаToolStripMenuItem.Visible = false;
            файлToolStripMenuItem.Visible = false;
            ButtonFind.Visible = false;
            label1.Visible = false;
            tbFind.Visible = false;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            tbName.Visible = true;
            tbNReis.Visible = true;
            tbType.Visible = true;
            buttonAddFlight.Visible = true;
            buttonCancel.Visible = true;
        }

        void ViewElem() // Отобразить элементы
        {
            this.ClientSize = new System.Drawing.Size(580, 370);
            dataGridView1.Visible = true;
            сервисToolStripMenuItem.Visible = true;
            правкаToolStripMenuItem.Visible = true;
            файлToolStripMenuItem.Visible = true;
            ButtonFind.Visible = true;
            label1.Visible = true;
            tbFind.Visible = true;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            tbName.Visible = false;
            tbNReis.Visible = false;
            tbType.Visible = false;
            buttonAddFlight.Visible = false;
            buttonCancel.Visible = false;
        }

        private void buttonAddFlight_Click(object sender, EventArgs e)// Кнопка Ок
        {
            if (tbName.Text != "")
            {
                if (tbNReis.Text != "")
                {
                    if (tbType.Text != "")
                    {
                        st.Name = tbName.Text;
                        st.Reis = Convert.ToInt32(tbNReis.Text);
                        st.Type = tbType.Text;
                        dataGridView1.Rows.Add(st.Name, st.Reis, st.Type);
                        sts.Add(st);
                        ViewElem();
                    }
                    else
                        MessageBox.Show("Вы не ввели тип самолёта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                    MessageBox.Show("Вы не ввели номер рейса.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
                MessageBox.Show("Вы не ввели наименование рейса.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        void buttonCanselClick(object sender, EventArgs e) //кнопка Отмена
        {
            ViewElem();

        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1[0, 0].Value != null)
            {
                int ind = dataGridView1.SelectedCells[0].RowIndex;
                dataGridView1.Rows.RemoveAt(ind);
            }

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    StreamWriter myWritet = new StreamWriter(myStream);
                    try
                    {
                        for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                        {
                            for (int j = 0; j < dataGridView1.ColumnCount; j++)
                                try
                                {
                                    myWritet.Write(dataGridView1.Rows[i].Cells[j].Value.ToString() + " ");
                                }
                                catch { }
                            myWritet.WriteLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        myWritet.Close();
                    }
                    myStream.Close();
                }
            }

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream mystr = null;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((mystr = openFileDialog1.OpenFile()) != null)
                {
                    StreamReader myread = new StreamReader(mystr);
                    string[] str;
                    int num = 0;
                    try
                    {
                        string[] str1 = myread.ReadToEnd().Split('\n');
                        num = str1.Count();
                        dataGridView1.RowCount = num;
                        for (int i = 0; i < num; i++)
                        {
                            str = str1[i].Split(' ');
                            for (int j = 0; j < dataGridView1.ColumnCount; j++)
                            {
                                try
                                {
                                    dataGridView1.Rows[i].Cells[j].Value = str[j];
                                }
                                catch { }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        myread.Close();
                    }
                }
            }

        }

        private void очиститьСписокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonFind_Click(object sender, EventArgs e) //Кнопка поиск
        {
            dataGridView2.Rows.Clear();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Selected = false;
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                        if (String.Compare(dataGridView1.Rows[i].Cells[j].Value.ToString(), tbFind.Text, true) == 0) // сравнение 2 строк
                        {
                            dataGridView1.Rows[i].Selected = true;
                            foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                            {
                                int index = dataGridView2.Rows.Add(r.Clone() as DataGridViewRow);
                                foreach (DataGridViewCell o in r.Cells)
                                {
                                    dataGridView2.Rows[index].Cells[o.ColumnIndex].Value = o.Value;
                                }
                            }
                            dataGridView1.Rows[i].Selected = false;
                        }
            }

        }

        private void информацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа студена гр.41703214 Васильев М.И", "О программе", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void подборToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Selected = false;
                if (dataGridView1.Rows[i].Cells[2].Value != null)
                    if (Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value) > 4)
                    {
                        dataGridView1.Rows[i].Selected = true;
                        foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                        {
                            int index = dataGridView2.Rows.Add(r.Clone() as DataGridViewRow);
                            foreach (DataGridViewCell o in r.Cells)
                            {
                                dataGridView2.Rows[index].Cells[o.ColumnIndex].Value = o.Value;
                            }
                        }
                        dataGridView1.Rows[i].Selected = false;
                    }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Stream mystr = null;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((mystr = openFileDialog1.OpenFile()) != null)
                {
                    StreamReader myread = new StreamReader(mystr);
                    string[] str;
                    int num = 0;
                    try
                    {
                        string[] str1 = myread.ReadToEnd().Split('\n');
                        num = str1.Count();
                        dataGridView1.RowCount = num;
                        for (int i = 0; i < num; i++)
                        {
                            str = str1[i].Split(' ');
                            for (int j = 0; j < dataGridView1.ColumnCount; j++)
                            {
                                try
                                {
                                    dataGridView1.Rows[i].Cells[j].Value = str[j];
                                }
                                catch { }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        myread.Close();
                    }
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            ViewElem();
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            добавитьToolStripMenuItem_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            сохранитьToolStripMenuItem_Click(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            удалитьToolStripMenuItem_Click(sender, e);
        }
    }

}

