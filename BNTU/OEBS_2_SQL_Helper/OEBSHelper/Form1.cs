using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using OEBSHelper.SqlConn;

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

           // OracleConnection conn = DBUtils.GetDBConnection();
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
            bunifuCustomLabel7.Text = "Connection successful!";
           // this.Hide();
            Form2 f2 = new Form2();
            f2.ShowDialog(); //блокируется основная форма
            // или f2.Show(); //не блокируется
        }
    }
}
