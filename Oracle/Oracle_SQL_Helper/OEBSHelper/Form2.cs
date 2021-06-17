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
using System.IO;
//using System.Windows.Forms;
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
        private Boolean ReversParam = true;
        public static string dirParameter = AppDomain.CurrentDomain.BaseDirectory + @"\Monitors.dbs";
        //this.Cursor = Cursors.Default;



        public Form2()
        {
            InitializeComponent();
            notifyIcon1.Icon = this.Icon;
            notifyIcon1.Visible = true;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            var wArea = Screen.PrimaryScreen.WorkingArea;
            this.Left = wArea.Width + wArea.Left - this.Width;
            this.Top = wArea.Height + wArea.Top - this.Height;
            bunifuMaterialTextbox1.Text = bunifuMaterialTextbox1.Text + " " + Form1.GlobalParam.monitor_count;
            ReadMonitor();

            //  AnimateWindow(this.Handle, 150, AnimateWindowFlags.AW_SLIDE | AnimateWindowFlags.AW_VER_NEGATIVE);
        }

        private void ControlInit(BunifuiOSSwitch MonitorSwitch, String SQL, BunifuCircleProgressbar CircleProgressbar,
                                 BunifuMetroTextbox TimerTextBox, Timer Timer_sec, TextBox CountBoxs,
                                 EventHandler Event, BunifuiOSSwitch WarningHintSwitch,
                                 BunifuMaterialTextbox MonitorName)
        {
            //MessageBox.Show(MonitorSwitch.Name, "Заголовок сообщения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
           // EventHandler a = Event;
            if (MonitorSwitch.Value is true)
            {
                Timer_sec.Interval = Convert.ToInt32(TimerTextBox.Text) * 1000; // specify interval time as you want
                Timer_sec.Tick += Event;
                CircleProgressbar.Value = 50;
                CircleProgressbar.animated = true;
                CircleProgressbar.animationIterval = 5;
                CircleProgressbar.animationSpeed = 5;
                Timer_sec.Start();
               // MessageBox.Show("Yes", "Заголовок сообщения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
               // MessageBox.Show(SQL, "Заголовок сообщения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                notifyIcon1.Icon = this.Icon;
                notifyIcon1.Visible = true;
                 ExecuteSQL(SQL, CountBoxs, MonitorName, WarningHintSwitch);
            }
            else
            {
                Timer_sec.Stop();
                Timer_sec.Tick -= Event;
                CircleProgressbar.Value = 0;
                CircleProgressbar.animated = false;
                CountBoxs.Text = "";
               // MessageBox.Show("NO", "Заголовок сообщения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                notifyIcon1.Icon = this.Icon;
                notifyIcon1.Visible = true;
            }

        }

        private void MailSend(BunifuMaterialTextbox MonitorName)
        {
            MailMessage message = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(Form1.GlobalParam.smtp);
            message.From = new MailAddress(Form1.GlobalParam.email_1);
            message.To.Add(Form1.GlobalParam.email_1);
            message.Subject = MonitorName.Text + " Error";
            message.Body = MonitorName.Text + "Error or not valid";
            if (Form1.GlobalParam.email_2.Length > 0)
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
        private void ExecuteSQL(String SQL, TextBox CountTextBox, BunifuMaterialTextbox MonitorName,
                                BunifuiOSSwitch WarningHintSwitch) 
        {
            // Получить объект Connection для подключения к DB.
            //OracleConnection conn = DBUtils.GetDBConnection();
            OracleConnection conn = DBOracleUtils.GetDBConnection(Form1.GlobalParam.host, Form1.GlobalParam.port, Form1.GlobalParam.sid, Form1.GlobalParam.user, Form1.GlobalParam.password);

            conn.Open();
            try
            {
                QueryEmployee(conn, SQL, WarningHintSwitch, CountTextBox, MonitorName);
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

        private void QueryEmployee(OracleConnection conn, String sqlquery, BunifuiOSSwitch WarningHintSwitch, TextBox CountBox, BunifuMaterialTextbox MonitorName)
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
                        if (WarningHintSwitch.Value is true)
                        {

                            String LAST_UPDATED_BY = Convert.ToString(reader.GetValue(0));
                            // BunifuCircleProgressbar.Value = Convert.ToInt32(LAST_UPDATED_BY);
                            CountBox.Text = LAST_UPDATED_BY;
                            if (Convert.ToInt32(LAST_UPDATED_BY) > 0)
                            {
                                notifyIcon1.Icon = SystemIcons.Error;
                                notifyIcon1.Visible = true;
                                PopupNotifier popup = new PopupNotifier();
                                popup.TitleText = "    ############# WARNING #############";
                                popup.ContentText = "\n     " + MonitorName.Text + "               ";
                                popup.Popup();
                                MailSend(MonitorName);
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
                            CountBox.Text = LAST_UPDATED_BY;
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

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            if (this.ReversParam)
            {
                this.Size = new Size(this.Size.Width - 480, this.Size.Height);
                bunifuImageButton1.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
                bunifuImageButton1.Refresh();
                bunifuImageButton7.Left = 213;
                this.ReversParam = false;
                panel1.Size = new Size(230, 185);
                panel2.Size = new Size(230, 185);
                panel3.Size = new Size(230, 185);
                panel4.Size = new Size(230, 185);
                panel5.Size = new Size(230, 185);
                panel6.Size = new Size(230, 185);
                // this.Location = new Point(0, 0);
                //this.Location = new Point(resolution.Right - this.Width, resolution.Bottom);
                this.Location = new Point(resolution.Width - Size.Width,480);
                /*for (int i = 1; i <= 6; i++) 
                   {
                      this.Controls["panel" + i.ToString()].Size = new Size(230, 185);

                   } */
            }
            else
            {
                //  MessageBox.Show("T2");
                this.Size = new Size(this.Size.Width + 480, this.Size.Height);
                bunifuImageButton1.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
                bunifuImageButton1.Refresh();
                bunifuImageButton7.Left = 697;
                panel1.Width = 706;
                panel2.Width = 706;
                panel3.Width = 706;
                panel4.Width = 706;
                panel5.Width = 706;
                panel6.Width = 706;
                this.ReversParam = true;
                // this.Location = new Point(0, 0);
                this.Location = new Point(resolution.Width - Size.Width, 480);
            }
        }

        public void SaveMonitor()
        {
            DialogResult result;
            result = MessageBox.Show("Do you want to save file?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question); if (result == DialogResult.No)
            {
                return;
            }
            if (result == DialogResult.Yes)
            {
                try
                {
                    if (bunifuMaterialTextbox1.Text != null
                        && bunifuMaterialTextbox2.Text != null
                        && bunifuMaterialTextbox3.Text != null
                        && bunifuMaterialTextbox4.Text != null
                        && bunifuMaterialTextbox5.Text != null
                        && textEditorControl1.Text != null
                        && textEditorControl2.Text != null
                        && textEditorControl3.Text != null
                        && textEditorControl4.Text != null
                        && textEditorControl5.Text != null)
                    {
                        saveFile(bunifuMaterialTextbox1.Text,
                            bunifuMaterialTextbox2.Text,
                            bunifuMaterialTextbox3.Text,
                            bunifuMaterialTextbox4.Text,
                            bunifuMaterialTextbox5.Text,
                            textEditorControl1.Text,
                            textEditorControl2.Text,
                            textEditorControl3.Text,
                            textEditorControl4.Text,
                            textEditorControl5.Text);
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());
                }
            }
        }

        public void saveFile(string MonitName1, string MonitName2, string MonitName3, string MonitName4,
                              string MonitName5, string SQL1, string SQL2, string SQL3, string SQL4, string SQL5)
        {
            string Msg = MonitName1 + ";" + MonitName2 + ";" + MonitName3 + ";" + MonitName4 + ";" + MonitName5
                + ";" + SQL1 + ";" + SQL2 + ";" + SQL3 + ";" + SQL4 + ";" + SQL5;
            // Save File to .txt  
            FileStream fParameter = new FileStream(dirParameter, FileMode.Create, FileAccess.Write);
            StreamWriter m_WriterParameter = new StreamWriter(fParameter);
            m_WriterParameter.BaseStream.Seek(0, SeekOrigin.End);
            m_WriterParameter.Write(Msg);
            m_WriterParameter.Flush();
            m_WriterParameter.Close();
        }

        public void ReadMonitor()
        {
            if (File.Exists(dirParameter))
                using (StreamReader streamReader = new StreamReader(dirParameter))

                {

                    string line = string.Empty;
                    while ((line = streamReader.ReadToEnd()) != null)
                    {
                        string[] tempArray = line.Split(';');
                        bunifuMaterialTextbox1.Text = tempArray[0];
                        bunifuMaterialTextbox2.Text = tempArray[1];
                        bunifuMaterialTextbox3.Text = tempArray[2];
                        bunifuMaterialTextbox4.Text = tempArray[3];
                        bunifuMaterialTextbox5.Text = tempArray[4];
                        textEditorControl1.Text = tempArray[5];
                        textEditorControl2.Text = tempArray[6];
                        textEditorControl3.Text = tempArray[7];
                        textEditorControl4.Text = tempArray[8];
                        textEditorControl5.Text = tempArray[9];
                        break;
                    }
                }
        }

        private void bunifuiOSSwitch1_OnValueChange(object sender, EventArgs e)
        {
            ControlInit(bunifuiOSSwitch1, textEditorControl1.Text, bunifuCircleProgressbar1, TimeTextbox1,
     timer1, textBox1, timer1_Tick, WarningHintSwitch1, bunifuMaterialTextbox1);
        }


        private void bunifuiOSSwitch2_OnValueChange(object sender, EventArgs e)
        {
            ControlInit(bunifuiOSSwitch2, textEditorControl2.Text, bunifuCircleProgressbar2, TimeTextbox2,
       timer2, textBox2, timer2_Tick, WarningHintSwitch2, bunifuMaterialTextbox2);
        }

        private void bunifuiOSSwitch3_OnValueChange(object sender, EventArgs e)
        {
            ControlInit(bunifuiOSSwitch3, textEditorControl3.Text, bunifuCircleProgressbar3, TimeTextbox3,
     timer3, textBox3, timer3_Tick, WarningHintSwitch3, bunifuMaterialTextbox3);
        }

        private void bunifuiOSSwitch4_OnValueChange(object sender, EventArgs e)
        {
            ControlInit(bunifuiOSSwitch4, textEditorControl4.Text, bunifuCircleProgressbar4, TimeTextbox4,
                 timer4, textBox4, timer4_Tick, WarningHintSwitch4, bunifuMaterialTextbox4);
        }

        private void bunifuiOSSwitch5_OnValueChange(object sender, EventArgs e)
        {
            ControlInit(bunifuiOSSwitch5, textEditorControl5.Text, bunifuCircleProgressbar5, TimeTextbox5,
                 timer5, textBox5, timer5_Tick, WarningHintSwitch4, bunifuMaterialTextbox5);
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
           // MessageBox.Show("T1");
            ExecuteSQL(textEditorControl1.Text, textBox1, bunifuMaterialTextbox1, WarningHintSwitch1);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("T2");
            ExecuteSQL(textEditorControl2.Text, textBox2, bunifuMaterialTextbox2, WarningHintSwitch2);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("T3");
            ExecuteSQL(textEditorControl3.Text, textBox3, bunifuMaterialTextbox3, WarningHintSwitch3);
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("T4");
            ExecuteSQL(textEditorControl4.Text, textBox4, bunifuMaterialTextbox4, WarningHintSwitch4);
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("T5");
            ExecuteSQL(textEditorControl5.Text, textBox5, bunifuMaterialTextbox5, WarningHintSwitch5);
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            SaveMonitor();
        }

    }
}



