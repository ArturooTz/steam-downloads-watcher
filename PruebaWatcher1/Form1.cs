using System;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Management;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace PruebaWatcher1
{
    public partial class Form1 : Form
    {
        string path1 = "", path2 = "", path3 = "", path5 = "";
        bool exist1 = false, fin1 = false;
        System.Timers.Timer t = new System.Timers.Timer(TimeSpan.FromSeconds(2).TotalMilliseconds); // set the time


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            path2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string locat1 = path2 + "\\sdlocat.txt";
            label4.ForeColor = Color.Red;
            if (File.Exists(locat1))
            {
                using (StreamReader reader = new StreamReader(locat1))
                {
                    path3 = reader.ReadLine();
                    //exist1 = true;
                    AsignPathFromLocal();
                }
            }
            else
            {

            }
        }


        #region BOTONES

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AsignPathFromSearch(folderBrowserDialog1);
            //if (exist1 == false)
            //{

            //}
            //else
            //{

            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            path5 = textBox1.Text;

            label4.Text = "Watch is ON";
            label4.ForeColor = Color.Green;

            button3.Enabled = false;

            t.AutoReset = true;

            t.Elapsed += new System.Timers.ElapsedEventHandler(CheckFolder);
            
            t.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DirectoryCheckChanges();
            //if (IsDirectoryEmpty(path3) == true)
            //{
            //    label1.Text = "There are no downloads in progress.";
            //    label1.ForeColor = Color.Red;
            //    button3.Enabled = false;
            //}
            //else
            //{
            //    label1.Text = "There is a download in progress!";
            //    label1.ForeColor = Color.Green;
            //    button3.Enabled = true;
            //}
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion

       
        #region METODOS

        public void AsignPathFromSearch(FolderBrowserDialog kek1)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string locat = "";
                path1 = folderBrowserDialog1.SelectedPath;
                locat = path2 + "\\sdlocat.txt";
                if (path1.Contains("steamapps\\downloading"))
                {
                    
                    System.IO.File.WriteAllText(@locat, path1);
                    textBox1.Text = path1;
                    button4.Enabled = true;
                    if (IsDirectoryEmpty(path1) == true)
                    {
                        label1.Text = "There are no downloads in progress.";
                        label1.ForeColor = Color.Red;
                        button3.Enabled = false;
                    }
                    else
                    {
                        label1.Text = "There is a download in progress!";
                        label1.ForeColor = Color.Green;
                        button3.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("That is not a valid Steam Downloads folder.");
                }

            }

        }

        public void DirectoryCheckChanges()
        {
            if (IsDirectoryEmpty(path3) == true)
            {
                label1.Text = "There are no downloads in progress.";
                label1.ForeColor = Color.Red;
                button3.Enabled = false;
            }
            else
            {
                label1.Text = "There is a download in progress!";
                label1.ForeColor = Color.Green;
                button3.Enabled = true;
            }
        }

        public void AsignPathFromLocal()
        {
            textBox1.Text = path3;
            button4.Enabled = true;
            DirectoryCheckChanges();
            //if (IsDirectoryEmpty(path3) == true)
            //{
            //    label1.Text = "There are no downloads in progress.";
            //    label1.ForeColor = Color.Red;
            //    button3.Enabled = false;
            //}
            //else
            //{
            //    label1.Text = "There is a download in progress!";
            //    label1.ForeColor = Color.Green;
            //    button3.Enabled = true;
            //}

        }

        public bool IsDirectoryEmpty(string path)
        {
            path = textBox1.Text;
            return !Directory.EnumerateFileSystemEntries(path).Any();
            //IEnumerable<string> items = Directory.EnumerateFileSystemEntries(path);
            //using (IEnumerator<string> en = items.GetEnumerator())
            //{
            //    return !en.MoveNext();
            //}
        }

        public void CheckFolder(object sender, ElapsedEventArgs e)
        {
            //MessageBox.Show("Top Kek m8");
            
            if (IsDirectoryEmpty(path5))
            {
                if (t.Enabled == true)
                {
                    t.AutoReset = false;
                    t.Stop();
                    t.Enabled = false;
                    SetText("Downloads have finished, the system will now shutdown!");
                    
                }
     
               
                System.Timers.Timer j = new System.Timers.Timer(TimeSpan.FromSeconds(10).TotalMilliseconds); // set the time

                j.AutoReset = false;

                j.Elapsed += new System.Timers.ElapsedEventHandler(TurnOff1);

                j.Start();

                
            }
        }

        public void TurnOff1(object sender, ElapsedEventArgs e)
        {
            SetText4("Watch is OFF");

            if (radioButton1.Checked == true)
            {
                List<string> lel = new List<string>();
                Process[] localAll = Process.GetProcesses();
                int nProcess = localAll.Count();
                for (byte i = 0; nProcess > i; i++)
                {
                    if (localAll[i].ProcessName == "Steam") { localAll[i].Kill(); }
                }
                Process.Start("shutdown", "/s /t 0");
                Application.Exit();
                
            }
            else if (radioButton2.Checked == true)
            {
                SetSuspendState(false, true, true);
            }
            else if (radioButton3.Checked == true)
            {
                SetSuspendState(true, true, true);

            }
            //MessageBox.Show("SYSTEM OFF");
            
            
            //Application.Exit();
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label1.Text = text;
            }
        }

        private void SetText4(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText4);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label4.Text = text;
                this.label4.ForeColor = Color.Red;
            }
        }

        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);


        /////////////////// MOVER EL FORM SIN BORDE ///////////////////

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            
        }

        ///////////////////////////////////////////////////////////////
        


        #endregion

        

        
    }
}
