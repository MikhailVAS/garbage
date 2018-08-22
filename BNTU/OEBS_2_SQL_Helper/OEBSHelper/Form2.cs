using System;
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
            // notifyIcon1 = new NotifyIcon();
            //notifyIcon1.Icon = SystemIcons.Application;
            //notifyIcon1.Visible = true;
            //notifyIcon1.Click += notifyIcon1_Click;
            //notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = this.Icon;
            notifyIcon1.Visible = true;

        }
        // string path = Directory.GetCurrentDirectory();
        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            var wArea = Screen.PrimaryScreen.WorkingArea;
            this.Left = wArea.Width + wArea.Left - this.Width;
            this.Top = wArea.Height + wArea.Top - this.Height;

            AnimateWindow(this.Handle, 150, AnimateWindowFlags.AW_SLIDE | AnimateWindowFlags.AW_VER_NEGATIVE);
        }

        private void bunifuiOSSwitch1_Click(object sender, EventArgs e)
        {
            if (bunifuiOSSwitch1.Value)
            {
                timer2.Interval = 10000; // specify interval time as you want
                timer2.Tick += new EventHandler(timer2_Tick);
                bunifuCircleProgressbar1.Value = 50;
                bunifuCircleProgressbar1.animated = true;
                bunifuCircleProgressbar1.animationIterval = 5;
                bunifuCircleProgressbar1.animationSpeed = 5;
                ExecuteSQL("SELECT COUNT (1) FROM inv.mtl_material_transactions WHERE costed_flag = 'N'", false);
                timer2.Start();

                // MessageBox.Show("Yes", "Заголовок сообщения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                timer2.Stop();
                bunifuCircleProgressbar1.Value = 0;
                bunifuCircleProgressbar1.animated = false;
                textBox1.Text = "";

                //  MessageBox.Show("No", "Заголовок сообщения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

        }

        private void bunifuiOSSwitch2_Click(object sender, EventArgs e)
        {
            if (bunifuiOSSwitch2.Value)
            {
                timer1.Interval = 10000; // specify interval time as you want
                timer1.Tick += new EventHandler(timer1_Tick);
                bunifuCircleProgressbar3.Value = 50;
                bunifuCircleProgressbar3.animated = true;
                bunifuCircleProgressbar3.animationIterval = 5;
                bunifuCircleProgressbar3.animationSpeed = 5;
                ExecuteSQL("SELECT COUNT (1) FROM inv.mtl_material_transactions WHERE costed_flag = 'E'", true);
                timer1.Start();

                // MessageBox.Show("Yes", "Заголовок сообщения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                timer1.Stop();
                bunifuCircleProgressbar3.Value = 0;
                bunifuCircleProgressbar3.animated = false;
                textBox2.Text = "";
                notifyIcon1.Icon = this.Icon;
                notifyIcon1.Visible = true;

                //  MessageBox.Show("No", "Заголовок сообщения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void MailSend()
        {
            MailMessage message = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.life.com.by");
            message.From = new MailAddress(Form1.GlobalParam.email);
            message.To.Add("Mihail.Vasiljev@life.com.by");
            message.Subject = "Cost Manager Error";
            message.Body = "Cost Manager - Трагически пал в бою за справедливость";
            //MailAddress copy = new MailAddress("Dmitry.Guk@life.com.by");
            //message.CC.Add(copy);
            SmtpServer.Port = 2525;
            SmtpServer.Credentials = new System.Net.NetworkCredential(Form1.GlobalParam.email, Form1.GlobalParam.email_password);
            SmtpServer.EnableSsl = true;

            try
            {
                // MessageBox.Show("1", "YES", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                SmtpServer.Send(message);
                //  MessageBox.Show("2", "YES", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (SmtpException g)
            {
                MessageBox.Show(g.ToString(), "Erroror", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
                //// bunifuCustomLabel6.Text = "Error: " + ex;
                //bunifuCustomLabel6.Text = bunifuCustomLabel6.Text + ex.StackTrace;
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
                    //     if (RealCosting)
                    //       {

                    //         }
                    while (reader.Read())
                    {
                        if (RealCosting)
                        {

                            String LAST_UPDATED_BY = Convert.ToString(reader.GetValue(0));
                            // bunifuCircleProgressbar1.Value = Convert.ToInt32(LAST_UPDATED_BY);
                            textBox2.Text = LAST_UPDATED_BY;
                            if (Convert.ToInt32(LAST_UPDATED_BY) > 0)
                            {
                                notifyIcon1.Icon = SystemIcons.Error;
                                notifyIcon1.Visible = true;
                                PopupNotifier popup = new PopupNotifier();
                                //popup.Image = Properties.Settings;
                                popup.TitleText = "               ############# WARNING #############";
                                popup.ContentText = "\n                     Error    Cost    Manager               ";
                                popup.Popup();
                                MailSend();
                            }
                            else
                            {
                                notifyIcon1.Icon = SystemIcons.Application;
                                notifyIcon1.Visible = true;
                            }
                        }
                        else
                        {
                            String LAST_UPDATED_BY = Convert.ToString(reader.GetValue(0));
                            // bunifuCircleProgressbar1.Value = Convert.ToInt32(LAST_UPDATED_BY);
                            textBox1.Text = LAST_UPDATED_BY;
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
            // MessageBox.Show("bla-BLA0-LBA");
            ExecuteSQL("SELECT COUNT (1) FROM inv.mtl_material_transactions WHERE costed_flag = 'E'", true);
            // timer1.Stop();
            //plotdata();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            /// MessageBox.Show("T1");
            ExecuteSQL("SELECT COUNT (1) FROM inv.mtl_material_transactions WHERE costed_flag = 'N'", false);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Closing programm ? ", "OEBS Helper",
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
            //throw new NotImplementedException();
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
            {
                // MessageBox.Show("bla-BLA0-LBA");
                this.Show();

                //Form form2 = Application.OpenForms["UpdateWindow"];
                //if (form2 != null)
                //{
                //    form2.ShowDialog();
                //    //form2.ShowDialog();
                //}
                //else
                //{
                //    form2.Show();
                //}

            }
        }
    }
}




