using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Employee
{
    public partial class Form2 : Form
    {

        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        internal struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        public Form2()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.Load += new EventHandler(Form2_Load);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;  // Turn on WS_EX_TOOLWINDOW
                return cp;
            }
        }
        struct DataParameter
        { 
         public int Process;
         public int Delay;
        }



        private DataParameter _inputparameter;

          
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int process = ((DataParameter)e.Argument).Process;
            int delay = ((DataParameter)e.Argument).Delay;
            int index = 1;

            try {
            
                for(int i = process; i > 0; i--)
                {
                    if (!backgroundWorker.CancellationPending)
                    {
                        backgroundWorker.ReportProgress(index++ * 100 / process, string.Format("Processing ... {0}", i));
                        Thread.Sleep(delay);


                        
                       


                    }
                }
                    

            }catch(Exception ex) {
                backgroundWorker.CancelAsync();
                MessageBox.Show(ex.Message,"Message",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                var lastInputInfo = new LASTINPUTINFO();
                lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);

                GetLastInputInfo(ref lastInputInfo);

                var lastInput = DateTime.Now.AddMilliseconds(
                    -(Environment.TickCount - lastInputInfo.dwTime));
                var current = DateTime.Now;
                var def = lastInput - current;
                progress.Value = e.ProgressPercentage;
                //percentLabel.Text = string.Format("Processing ... {0}%", e.ProgressPercentage);
                percentLabel.Text = lastInput.ToString() + "  current   " + current + " Defferent is" + def.ToString("mm");
                //MessageBox.Show(def.ToString());
                if (int.Parse(def.ToString("mm"))==15)
                {
                    backgroundWorker.CancelAsync();

                    //MessageBox.Show("Hellloooo :)");
                    Form1 form = new Form1();
                    TimerForm obj = (TimerForm)Application.OpenForms["TimerForm"];

                    this.logsTableAdapter.UpdateLogs(DateTime.Now.ToString("HH:mm:ss"), "auto logout", "false", "true");

                    obj.Hide();
                    this.Hide();

                    form.Show();
                    this.Close();
                }

                _inputparameter.Process += 1000;


                FormCollection fc = Application.OpenForms;
                bool isOpen = false;
                foreach (Form frm in fc)
                {
                    //iterate through
                    if (frm.Name == "TimerForm")
                    {
                        isOpen = true;
                    }
                }

                if (isOpen == false)
                {
                    Form1 form = new Form1();
                    backgroundWorker.CancelAsync();
                    this.Hide();

                    form.Show();
                }

                progress.Update();

            }catch (Exception ex) { }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show("Logged Out :).","Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy) {
                _inputparameter.Delay = 100;
                _inputparameter.Process = 1200;
                backgroundWorker.RunWorkerAsync(_inputparameter);
            }
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {

            if(backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseEmployeeDataSet.Logs' table. You can move, or remove it, as needed.
            this.logsTableAdapter.Fill(this.databaseEmployeeDataSet.Logs);
            this.Size = new Size(0, 0);
            if (!backgroundWorker.IsBusy)
            {
                _inputparameter.Delay = 100;
                _inputparameter.Process = 10000;
                backgroundWorker.RunWorkerAsync(_inputparameter);
            }
        }

        private void logsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.logsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.databaseEmployeeDataSet);

        }
    }
}
