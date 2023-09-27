using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Employee
{
    public partial class TimerForm : Form
    {
        public TimerForm()
        {
            InitializeComponent();
        }

        int hour = 0;
        int minute = 0;
        int second = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            

            int secLabel = int.Parse(sec0.Text);
            int minLabel = int.Parse(min0.Text);
            int hourLabel = int.Parse(hour0.Text);


            if (secLabel < 59)
            {
                if(secLabel < 9)
                sec0.Text = "0"+(int.Parse(sec0.Text) + 1).ToString();
                else
                sec0.Text = (int.Parse(sec0.Text) + 1).ToString();
           
            
            }
            else if (minLabel < 59)
            {
                sec0.Text = "00";

                if(minLabel < 9)
                 min0.Text = "0"+(int.Parse(min0.Text) + 1).ToString();
                else
                 min0.Text = (int.Parse(min0.Text) + 1).ToString();

                minute = int.Parse(min0.Text);
            }
            else {
                sec0.Text = "00";
                min0.Text = "00";

                if (hourLabel < 9)
                    hour0.Text = "0"+(int.Parse(hour0.Text) + 1).ToString();
                else
                    hour0.Text = (int.Parse(hour0.Text) + 1).ToString();

                hour = int.Parse(hour0.Text);
            }







        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hour = " + hour + " minute = " + minute);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            //MessageBox.Show("Hour = " + hour + " minute = " + minute);
            try
            {
                Form1 form1 = new Form1();
            
                this.logsTableAdapter.UpdateLogs(DateTime.Now.ToString("HH:mm:ss"), hour + " hr and " + minute + " min", "false", "true");

                this.Hide();
              
                form1.Show();
            }catch(Exception ex) { }

        }
        private void Form1_Resize(object sender, EventArgs e)
        {
       
        }

        private void TimerForm_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.button2, "login to admin");
            try
            {
                // TODO: This line of code loads data into the 'databaseEmployeeDataSet.Logs' table. You can move, or remove it, as needed.
                this.logsTableAdapter.Fill(this.databaseEmployeeDataSet.Logs);
            }
            catch (Exception) { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            
        }

       
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
          
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void logsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            try { 
            this.Validate();
            this.logsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.databaseEmployeeDataSet);
            }
            catch (Exception) { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                Form1 form1 = new Form1();

                this.logsTableAdapter.UpdateLogs(DateTime.Now.ToString("HH:mm:ss"), hour + " hr and " + minute + " min", "false", "true");

                this.Hide();
                //form1.FormClosed += (s, arg) => this.Close();
                form1.Show();
            }
            catch (Exception ex) { }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            AdminLogin admin = new AdminLogin();
            
            admin.ShowDialog();
        }
    }
}
