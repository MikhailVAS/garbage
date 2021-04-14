using System;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using OEBSHelper.SqlConn;
using System.Data.Common;


namespace OEBSHelper
{
    public partial class Form1 : Form
    {

        public static class GlobalParam
        {
            public static string host { get; set; }
            public static int port { get; set; }
            public static string sid { get; set; }
            public static string user { get; set; }
            public static string password { get; set; }
            public static string email_1 { get; set; }
            public static string email_password_1 { get; set; }
            public static string email_2 { get; set; }
            public static string smtp { get; set; }
            public static string smtp_port { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
        }
        

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            // 

            GlobalParam.host = bunifuMetroTextbox1.Text;
            GlobalParam.port = Convert.ToInt16(bunifuMetroTextbox2.Text);
            GlobalParam.sid  = bunifuMetroTextbox3.Text;
            GlobalParam.user  = bunifuMetroTextbox4.Text;
            GlobalParam.password  = bunifuMetroTextbox5.Text;
            GlobalParam.email_1 = bunifuMetroTextbox7.Text;
            GlobalParam.email_password_1 = bunifuMetroTextbox6.Text;
            GlobalParam.email_2 = bunifuMetroTextbox9.Text;
            GlobalParam.smtp = bunifuMetroTextbox11.Text;
            GlobalParam.smtp_port = bunifuMetroTextbox10.Text;
            // OracleConnection conn = DBUtils.GetDBConnection();
          /* !!!!!!!!   Debug comment
            
           OracleConnection conn = DBOracleUtils.GetDBConnection(GlobalParam.host, GlobalParam.port, GlobalParam.sid, GlobalParam.user, GlobalParam.password);

            bunifuCustomLabel7.Text = "Get Connection: " + conn;
            try
            {
                conn.Open();

                bunifuCustomLabel7.Text = conn.ConnectionString + "Successful Connection";
            }
            catch (Exception ex)
            {
                bunifuCustomLabel7.Text = "## ERROR: " + ex.Message;
                return;
            }
          */
            bunifuCustomLabel7.Text = "Connection successful!";
           this.Hide();
            Form2 f2 = new Form2();
            f2.Top = Screen.PrimaryScreen.WorkingArea.Height - f2.Height;
            f2.Left = Screen.PrimaryScreen.WorkingArea.Width - f2.Width;
            f2.Show(); //не блокируется
          //  f2.ShowDialog(); //блокируется основная форма
            // или f2.Show(); //не блокируется
        }

    }
} 
