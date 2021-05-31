﻿using System;
using System.Data.Common;
using System.Drawing;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using OEBSHelper.SqlConn;
using Oracle.ManagedDataAccess.Client;
using OEBSHelper;
using Tulpep.NotificationWindow;
using System.Net.Mail;
//using MetroFramework.Forms;


namespace OEBSHelper
{
    public partial class Form2 : Form
    {

        [Flags]
        enum AnimateWindowFlags
        {
            AW_HOR_POSITIVE = 0x00000001,
            AW_HOR_NEGATIVE = 0x00000002,
            AW_VER_POSITIVE = 0x00000004,
            AW_VER_NEGATIVE = 0x00000008,
            AW_CENTER = 0x00000010,
            AW_HIDE = 0x00010000,
            AW_ACTIVATE = 0x00020000,
            AW_SLIDE = 0x00040000,
            AW_BLEND = 0x00080000
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool AnimateWindow(IntPtr hWnd, int time, AnimateWindowFlags flags);

        public Form2()
        {
            InitializeComponent();
            notifyIcon1.Icon = this.Icon;
            notifyIcon1.Visible = true;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = "0";
          //  textBox20.Text = "";
            var wArea = Screen.PrimaryScreen.WorkingArea;
            this.Left = wArea.Width + wArea.Left - this.Width;
            this.Top = wArea.Height + wArea.Top - this.Height;
            bunifuMaterialTextbox1.Text = bunifuMaterialTextbox1.Text + " " + Form1.GlobalParam.monitor_count;
            //  AnimateWindow(this.Handle, 150, AnimateWindowFlags.AW_SLIDE | AnimateWindowFlags.AW_VER_NEGATIVE);
        }

         private void ControlInit(BunifuiOSSwitch Switch, String SQL,BunifuCircleProgressbar CircleProgressbar,
                                  BunifuMetroTextbox ms, Timer Timer_sec,TextBox CountBox)
        {
            MessageBox.Show(Switch.Name, "Заголовок сообщения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            if (Switch.Value is true)
            {
                Timer_sec.Interval = Convert.ToInt32(ms.Text) * 1000; // specify interval time as you want
                Timer_sec.Tick += new EventHandler(timer1_Tick);
                CircleProgressbar.Value = 50;
                CircleProgressbar.animated = true;
                CircleProgressbar.animationIterval = 5;
                CircleProgressbar.animationSpeed = 5;
                Timer_sec.Start();
                MessageBox.Show("Yes", "Заголовок сообщения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                MessageBox.Show(SQL, "Заголовок сообщения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                notifyIcon1.Icon = this.Icon;
                notifyIcon1.Visible = true;
                // ExecuteSQL(textEditorControl1.Text, false);
            }
            else
            {
                Timer_sec.Stop();
                Timer_sec.Tick -= new EventHandler(timer1_Tick);
                CircleProgressbar.Value = 0;
                CircleProgressbar.animated = false;
                CountBox.Text = "";
                MessageBox.Show("NO", "Заголовок сообщения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                notifyIcon1.Icon = this.Icon;
                notifyIcon1.Visible = true;
            }

        }

        private void MailSend()
        {
            MailMessage message = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(Form1.GlobalParam.smtp); 
            message.From = new MailAddress(Form1.GlobalParam.email_1);
            message.To.Add(Form1.GlobalParam.email_1);
            message.Subject = "Cost Manager Error";
            message.Body = "Cost Manager - Трагически пал в бою за справедливость";
            if (Form1.GlobalParam.email_2.Length > 0  )
              {
                MailAddress copy = new MailAddress(Form1.GlobalParam.email_2); 
                message.CC.Add(copy);
              }
            SmtpServer.Port = Convert.ToInt32(Form1.GlobalParam.smtp_port);
            SmtpServer.Credentials = new System.Net.NetworkCredential(Form1.GlobalParam.email_1, Form1.GlobalParam.email_password_1);
            SmtpServer.EnableSsl = true;

            try
            {
                SmtpServer.Send(message);
            }
            catch (SmtpException g)
            {
                MessageBox.Show(g.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        private void ExecuteSQL(String SQL, Boolean Costing)
        {
            // Получить объект Connection для подключения к DB.
             //OracleConnection conn = DBUtils.GetDBConnection();
            OracleConnection conn = DBOracleUtils.GetDBConnection(Form1.GlobalParam.host, Form1.GlobalParam.port, Form1.GlobalParam.sid, Form1.GlobalParam.user, Form1.GlobalParam.password);

            conn.Open();
            try
            {
                QueryEmployee(conn, SQL, Costing);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
                    while (reader.Read())
                    {
                        if (RealCosting)
                        {

                            String LAST_UPDATED_BY = Convert.ToString(reader.GetValue(0));
                            // bunifuCircleProgressbar1.Value = Convert.ToInt32(LAST_UPDATED_BY);
                        //    textBox20.Text = LAST_UPDATED_BY;
                            if (Convert.ToInt32(LAST_UPDATED_BY) > 0)
                            {
                                notifyIcon1.Icon = SystemIcons.Error;
                                notifyIcon1.Visible = true;
                                PopupNotifier popup = new PopupNotifier();
                                popup.TitleText = "               ############# WARNING #############";
                                popup.ContentText = "\n                     Error    Cost    Manager               ";
                                popup.Popup();
                                MailSend();
                            }
                            else
                            {
                                notifyIcon1.Icon = this.Icon;
                                notifyIcon1.Visible = true;
                            }
                        }
                        else
                        {
                            String LAST_UPDATED_BY = Convert.ToString(reader.GetValue(0));
                            textBox1.Text = LAST_UPDATED_BY;
                            notifyIcon1.Icon = this.Icon;
                            notifyIcon1.Visible = true;
                        }

                    }
                }
            }
        }

        private void bunifuImageButton8_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        private void bunifuImageButton7_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            //Close();
            // проверяем наше окно, и если оно было свернуто, делаем событие        
            //if (WindowState == FormWindowState.Minimized)
            //{
            //    // прячем наше окно из панели
            //    this.ShowInTaskbar = false;
            //    // делаем нашу иконку в трее активной
            //    notifyIcon1.Visible = true;
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MessageBox.Show("T1");
         //   ExecuteSQL("SELECT COUNT (1) FROM inv.mtl_material_transactions WHERE costed_flag = 'E'", true);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
             MessageBox.Show("T2");
            //ExecuteSQL("SELECT COUNT (1) FROM inv.mtl_material_transactions WHERE costed_flag = 'N'", false);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Close programm ? ", "DataBase Helper",
         MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                closing = false;
                Application.Exit();
            }
        }

        private void qweToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.Show(); //не блокируется
        }

        private bool closing = true;
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = closing;
            this.Hide();
        }

        private void notifyIcon1_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
            {
                // MessageBox.Show("bla-BLA0-LBA");
                this.Show();
            }
        }

        private void bunifuiOSSwitch1_OnValueChange(object sender, EventArgs e)
        {
            ControlInit(bunifuiOSSwitch1, textEditorControl1.Text, bunifuCircleProgressbar1, bunifuMetroTextbox11,
     timer1, textBox1);
        }
    }
}



