using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using OEBSHelper.SqlConn;
using Oracle.ManagedDataAccess.Client;
using OEBSHelper;
using System.IO;
using Bunifu.DataViz;

namespace OEBSHelper
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        string path = Directory.GetCurrentDirectory();

        private void Form2_Load(object sender, EventArgs e)
        {
            bunifuCustomLabel4.Text = "";
            FileFoundList.Clear();
            GetFFiles(path+"\\sql", "*");
        }

        private void bunifuiOSSwitch1_Click(object sender, EventArgs e)
        {
            if (bunifuiOSSwitch1.Value)
            {
                bunifuCircleProgressbar1.Value = 50;
                bunifuCircleProgressbar1.animated = true;
                bunifuCircleProgressbar1.animationIterval = 5;
                bunifuCircleProgressbar1.animationSpeed = 5;
                ExecuteSQL("SELECT COUNT (1) FROM inv.mtl_material_transactions WHERE CREATED_BY = '-1'AND CREATION_DATE >= TO_DATE('17.02.2018', 'dd.mm.yyyy')", false);


                // MessageBox.Show("Yes", "Заголовок сообщения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                bunifuCircleProgressbar1.Value = 0;
                bunifuCircleProgressbar1.animated = false;
                bunifuCustomLabel4.Text = "";

                //  MessageBox.Show("No", "Заголовок сообщения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

        }

        private void ExecuteSQL(String SQL, Boolean Costing)
        {
            // Получить объект Connection для подключения к DB.
            // OracleConnection conn = DBUtils.GetDBConnection();
            OracleConnection conn = DBOracleUtils.GetDBConnection(Form1.GlobalParam.host, Form1.GlobalParam.port, Form1.GlobalParam.sid, Form1.GlobalParam.user, Form1.GlobalParam.password);

            conn.Open();
            try
            {
                QueryEmployee(conn, SQL, Costing);
            }
            catch (Exception ex)
            {
                bunifuCustomLabel6.Text = "Error: " + ex;
                bunifuCustomLabel6.Text = bunifuCustomLabel6.Text + ex.StackTrace;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            Console.Read();
        }

        private void QueryEmployee(OracleConnection conn, String sqlquery, Boolean RealCosting)
        {
            string sql = sqlquery;
            // Создать объект Command.
            OracleCommand cmd = new OracleCommand();

            // Сочетать Command с Connection.
            cmd.Connection = conn;
            cmd.CommandText = sql;

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    if (RealCosting)
                    {

                        // выводим названия столбцов
                        //bunifuCustomDataGrid1.Columns
                        //    bunifuCustomLabel6.Text = "{0}\t{1}\t{2}" + reader.GetName(0) + reader.GetName(1) + reader.GetName(2);
                    }
                    while (reader.Read())
                    {
                        if (RealCosting)
                        {
                            bunifuCustomDataGrid1.ColumnCount = reader.FieldCount;

                            for (int ColumnCount = 0; ColumnCount < reader.FieldCount; ColumnCount++)
                            {

                                for (int RowCount = 0; RowCount <= reader.Depth; RowCount++)
                                {
                                    // bunifuCustomDataGrid1.Rows.Add
                                    //  bunifuCustomDataGrid1.Rows.Add(reader.GetValue(RowCount));
                                    // string id = reader.GetString(RowCount);
                                    //   bunifuCustomLabel6.Text = reader.GetValue(1);
                                    bunifuCustomDataGrid1.Rows[RowCount].Cells[ColumnCount].Value = reader.GetValue(ColumnCount);
                                }
                            }
                            // object id = reader.GetValue(0);
                            // object name = reader.GetValue(1);
                            //  object age = reader.GetValue(2);

                            // bunifuCustomLabel6.Text = "{0} \t{1} \t{2}" + id + name + age;
                        }
                        else
                        {
                            String LAST_UPDATED_BY = Convert.ToString(reader.GetValue(0));//reader.GetString(2);
                                                                                          // bunifuCircleProgressbar1.Value = Convert.ToInt32(LAST_UPDATED_BY);
                            bunifuCustomLabel4.Text = LAST_UPDATED_BY;
                        }

                    }
                }
            }

        }
        //private void ReadData()
        //{
        //    FileStream fs = new FileStream(@"c:\\sql\\", FileMode.Open, FileAccess.Read);
        //    StreamReader strm = new StreamReader(fs);
        //    strm.BaseStream.Seek(0, SeekOrigin.Begin);
        //    string str = strm.ReadLine();
        //    while (str != null)
        //    {
        //        FileFoundList.Items.Add(str);
        //        str = strm.ReadLine();
        //    }
        //    strm.Close();
        //    fs.Close();
        //}




        private void bunifuImageButton11_Click(object sender, EventArgs e)
        {
            ExecuteSQL("SELECT 1 as FIRST_C,2 as SECOND_C,3 as Tree FROM  DUAL UNION ALL SELECT 4 as FIRST_C,5 as SECOND_C,6 as Tree FROM  DUAL ", true);

            //for (int i = 0; i < 30; i++)
            //{
            //    bunifuCustomDataGrid1.Rows.Add("Twitter", "Today", "December", "$7,000.00");
            //}
        }
        private void bunifuMetroTextbox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //  ReadData();
            //if (bunifuMetroTextbox1.Text != "")
            //{
            //ProcessDirectory();// (directories[0], bunifuMetroTextbox1.Text);
            FileFoundList.Clear();
            GetFFiles(path + "\\sql", bunifuMetroTextbox1.Text);
            //}
        }

        private void bunifuImageButton11_Click_1(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton8_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuiOSSwitch1_OnValueChange(object sender, EventArgs e)
        {

        }
        private bool _IsMaximized;
        private Size _LastSize, _ScreenSize;
        private Point _LastLocation, _ZeroZero;
        //string[] directories = Directory.GetDirectories("C:\\sql");


        private void bunifuImageButton7_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void bunifuImageButton10_Click_1(object sender, EventArgs e)
        {
            Screen currentScreen = Screen.FromControl(this);
            _ScreenSize = currentScreen.WorkingArea.Size;


            if (this.Size == _ScreenSize)
            {//is maximized
                _IsMaximized = false;
                this.Location = _LastLocation;
                this.Size = _LastSize;
                panel1.Height = this.Size.Height;
                panel2.Height = this.Size.Height-100;
                FileFoundList.Height = this.Size.Height-110;
                // BtnMax.Image = Properties.Resources.Maximize;
                //    BtnMax.Text = "Maximize";
            }
            else
            {
                _IsMaximized = true;
                _LastSize = this.Size;
                _LastLocation = this.Location;
                this.Location = currentScreen.WorkingArea.Location;
                ;//_ZeroZero;
                this.Size = _ScreenSize;
                panel1.Height = this.Size.Height-10;
                panel2.Height = this.Size.Height;
                FileFoundList.Height = this.Size.Height-110;

                //  BtnMax.Image = Properties.Resources.Restore;
                //   BtnMax.Text = "Restore";
            }
        }

        private void FileFoundList_SelectedIndexChanged(object sender, EventArgs e)
        {
            String fullPath = path + "\\" + FileFoundList.FocusedItem.Text;
            bunifuCustomLabel6.Text = fullPath;
            //Process.Start(fullPath);
        }

        private void bunifuImageButton11_Click_2(object sender, EventArgs e)
        {
            ExecuteSQL("SELECT 1 as FIRST_C,2 as SECOND_C,3 as Tree FROM  DUAL UNION ALL SELECT 4 as FIRST_C,5 as SECOND_C,6 as Tree FROM  DUAL ", true);

        }

        //public void plotdata()
        //{
        //    var canvas1 = new Bunifu.DataViz.Canvas();
        //    var datapoints1 = new Bunifu.DataViz.DataPoint(Bunifu.DataViz.BunifuDataViz._type.Bunifu_spline);
        //    datapoints1.addLabely("Apr", "700");
        //    datapoints1.addLabely("May", "430");
        //    datapoints1.addLabely("Jun", "440");
        //    datapoints1.addLabely("Jul", "750");
        //    datapoints1.addLabely("Aug", "770");
        //    datapoints1.addLabely("Sep", "600");
        //    datapoints1.addLabely("Oct", "560");
        //    datapoints1.addLabely("Nov", "200");
        //    datapoints1.addLabely("Dec", "750");

        //    //add the datapoints to the canvas
        //    canvas1.addData(datapoints1);
        //    //render te chart
        //    bunifuDataViz1.Render(canvas1);

        //}



        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    timer1.Stop();
        //    plotdata();
        //}

        private void bunifuDataViz1_Load(object sender, EventArgs e)
        {
           
        }

  

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
            
        }

        public class ListViewItem
        {
            public string Name { get; set; }
            public string PathName { get; set; }
        }

        public void GetFFiles(string DirectoryPath, string findstring)
        {
            DirectoryInfo Dinfo = new DirectoryInfo(DirectoryPath);
            List<ListViewItem> ListViewItemFiles = new List<ListViewItem>();
            List<FileInfo> ListOfFiles = Dinfo.GetFiles("*"+findstring+"*.sql", SearchOption.TopDirectoryOnly).ToList();
            foreach (FileInfo directory in ListOfFiles)
            {
                //ListViewItemFiles.Add(new ListViewItem() { Name = directory.Name, PathName = directory.FullName });
                FileFoundList.Items.Add( Convert.ToString(directory.Name));
            }
            //LocalFilesView.ItemsSource = ListViewItemFiles;
        
      }
    }
}
    



