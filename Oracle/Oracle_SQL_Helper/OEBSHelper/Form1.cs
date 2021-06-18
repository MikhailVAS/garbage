using System;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using OEBSHelper.SqlConn;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace OEBSHelper
{
    public partial class Form1 : Form
    {
        public static string dirParameter = AppDomain.CurrentDomain.BaseDirectory + @"\set.dbs";
        public static int Key = 2;
        string publickey = "12345678";
        string secretkey = "87654321";
        byte[] secretkeyByte = { };
        
            byte[] publickeybyte = { };
        

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
            public static string monitor_count { get; set; }
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
            GlobalParam.sid = bunifuMetroTextbox3.Text;
            GlobalParam.user = bunifuMetroTextbox4.Text;
            GlobalParam.password = bunifuMetroTextbox5.Text;
            GlobalParam.email_1 = bunifuMetroTextbox7.Text;
            GlobalParam.email_password_1 = bunifuMetroTextbox6.Text;
            GlobalParam.email_2 = bunifuMetroTextbox9.Text;
            GlobalParam.smtp = bunifuMetroTextbox11.Text;
            GlobalParam.smtp_port = bunifuMetroTextbox10.Text;
            //GlobalParam.monitor_count = bunifuMetroTextbox8.Text;
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

        public void SaveEvent()
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
                    if (bunifuMetroTextbox1.Text != null
                        && bunifuMetroTextbox2.Text != null
                        && bunifuMetroTextbox3.Text != null
                        && bunifuMetroTextbox4.Text != null
                        && bunifuMetroTextbox5.Text != null
                        && bunifuMetroTextbox6.Text != null
                        && bunifuMetroTextbox7.Text != null
                        && bunifuMetroTextbox9.Text != null
                        && bunifuMetroTextbox10.Text != null
                        && bunifuMetroTextbox11.Text != null)
                    {
                        saveFile(bunifuMetroTextbox1.Text,
                            bunifuMetroTextbox2.Text,
                            bunifuMetroTextbox3.Text,
                            bunifuMetroTextbox4.Text,
                            bunifuMetroTextbox5.Text,
                            bunifuMetroTextbox6.Text,
                            bunifuMetroTextbox7.Text,
                            bunifuMetroTextbox9.Text,
                            bunifuMetroTextbox10.Text,
                            bunifuMetroTextbox11.Text);
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());
                }
            }
        }

        public void saveFile(string host, string port, string sid, string user_name,
                             string pas, string mail_pas, string mail_sender, string mail_cop,
                             string mail_port, string mail_smtp)
        {
            string Msg = host + ";" + port + ";" + sid + ";" + user_name + ";" + pas + ";"
                + mail_pas + ";" + mail_sender + ";" + mail_cop + ";" + mail_port + ";" + mail_smtp;


            /*  FileStream fParameter = new FileStream(dirParameter, FileMode.Create, FileAccess.Write);
              StreamWriter m_WriterParameter = new StreamWriter(fParameter);
              m_WriterParameter.BaseStream.Seek(0, SeekOrigin.End);
              m_WriterParameter.Write(Msg);
              m_WriterParameter.Flush();
              m_WriterParameter.Close(); */

            // Save Params to Encript File   
            try
            {
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                // Create a new DESCryptoServiceProvider object
                // to generate a key and initialization vector (IV).
                DESCryptoServiceProvider DESalg = new DESCryptoServiceProvider();

                // Create a string to encrypt.
                string sData = Msg;//"Here is some data to encrypt."

                // Encrypt text to a file using the file name, key, and IV.
                EncryptTextToFile(sData, secretkeyByte, publickeybyte);

                // Decrypt the text from a file using the file name, key, and IV.
              //  string Final = DecryptTextFromFile(FileName, DESalg.Key, DESalg.IV);

                // Display the decrypted string to the console.
               // Console.WriteLine(Final);
            }
            catch (Exception e)
            {
               // Console.WriteLine(e.Message);
            }
        }


        public void ReadFileDiaolg(bool OpenDialogOrLoadForm)
        {

            secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
            publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            
            openFileDialog1.InitialDirectory = "dirParameter";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (OpenDialogOrLoadForm == true)
            {


                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((myStream = openFileDialog1.OpenFile()) != null)
                        {

                            CryptoStream cStream = new CryptoStream(myStream,
                                new DESCryptoServiceProvider().CreateDecryptor(secretkeyByte, publickeybyte),
                                CryptoStreamMode.Read);
                            //  using (StreamReader sReader = new StreamReader(cStream))
                            {
                                // Create a StreamReader using the CryptoStream.
                                StreamReader sReader = new StreamReader(cStream);

                                string line = string.Empty;
                                while ((line = sReader.ReadLine()) != null)
                                {
                                    string[] tempArray = line.Split(';');
                                    bunifuMetroTextbox1.Text = tempArray[0];
                                    bunifuMetroTextbox2.Text = tempArray[1];
                                    bunifuMetroTextbox3.Text = tempArray[2];
                                    bunifuMetroTextbox4.Text = tempArray[3];
                                    bunifuMetroTextbox5.Text = tempArray[4];
                                    bunifuMetroTextbox6.Text = tempArray[5];
                                    bunifuMetroTextbox7.Text = tempArray[6];
                                    bunifuMetroTextbox9.Text = tempArray[7];
                                    bunifuMetroTextbox10.Text = tempArray[8];
                                    bunifuMetroTextbox11.Text = tempArray[9];
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
            }
            else
            {
                
                    if (File.Exists(dirParameter))
                    {
                    try
                    {
                        FileStream fStream = File.Open(dirParameter, FileMode.OpenOrCreate);

                        CryptoStream cStream = new CryptoStream(fStream,
                            new DESCryptoServiceProvider().CreateDecryptor(secretkeyByte, publickeybyte),
                            CryptoStreamMode.Read);
                        //using (StreamReader streamReader = new StreamReader(dirParameter))



                        StreamReader sReader = new StreamReader(cStream);
                        string line = string.Empty;

                        while ((line = sReader.ReadLine()) != null)
                        {
                            string[] tempArray = line.Split(';');
                            bunifuMetroTextbox1.Text = tempArray[0];
                            bunifuMetroTextbox2.Text = tempArray[1];
                            bunifuMetroTextbox3.Text = tempArray[2];
                            bunifuMetroTextbox4.Text = tempArray[3];
                            bunifuMetroTextbox5.Text = tempArray[4];
                            bunifuMetroTextbox6.Text = tempArray[5];
                            bunifuMetroTextbox7.Text = tempArray[6];
                            bunifuMetroTextbox9.Text = tempArray[7];
                            bunifuMetroTextbox10.Text = tempArray[8];
                            bunifuMetroTextbox11.Text = tempArray[9];
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
               
            }
        }

        // Encrypt data
        public static void EncryptTextToFile(String Data, byte[] Key, byte[] IV)
        {
            try
            {
                // Create or open the specified file.
                FileStream fStream = File.Open(dirParameter, FileMode.OpenOrCreate);

                // Create a CryptoStream using the FileStream
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(fStream,
                    new DESCryptoServiceProvider().CreateEncryptor(Key, IV),
                    CryptoStreamMode.Write);

                // Create a StreamWriter using the CryptoStream.
                StreamWriter sWriter = new StreamWriter(cStream);

                // Write the data to the stream
                // to encrypt it.
                sWriter.WriteLine(Data);

                // Close the streams and
                // close the file.
                sWriter.Close();
                cStream.Close();
                fStream.Close();
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("A file error occurred: {0}", e.Message);
            }
        }

  /*      public static string DecryptTextFromFile(byte[] Key, byte[] IV)
        {
            try
            {
                // Create or open the specified file.
                FileStream fStream = File.Open(dirParameter, FileMode.OpenOrCreate);

                // Create a CryptoStream using the FileStream
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(fStream,
                    new DESCryptoServiceProvider().CreateDecryptor(Key, IV),
                    CryptoStreamMode.Read);

                // Create a StreamReader using the CryptoStream.
                StreamReader sReader = new StreamReader(cStream);

                // Read the data from the stream
                // to decrypt it.
                string val = sReader.ReadLine();

                // Close the streams and
                // close the file.
                sReader.Close();
                cStream.Close();
                fStream.Close();

                // Return the string.
                return val;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("A file error occurred: {0}", e.Message);
                return null;
            }
        }*/

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            SaveEvent();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            ReadFileDiaolg(true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReadFileDiaolg(false); //ReadFile();
        }

        private void bunifuImageButton2_Click_1(object sender, EventArgs e)
        {
            SaveEvent();
        }
    }
}

