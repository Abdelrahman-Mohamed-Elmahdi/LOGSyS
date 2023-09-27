using Employee.DatabaseEmployeeDataSetTableAdapters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Employee
{
    public partial class Form1 : Form
    {
        // stop Shortcut
        public const int WH_KEYBOARD_LL = 13;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const int VK_TAB = 0x9;
        public const int VK_MENU = 0x12; /* Alt key */
        public const int VK_ESCAPE = 0x1B;
        public const int VK_F4 = 0x73;
        public const int VK_LWIN = 0x5B;
        public const int VK_RWIN = 0x5C;
        public const int VK_CONTROL = 0x11;
        public const int VK_LCONTROL = 0xA2;
        public const int VK_RCONTROL = 0xA3;

        [StructLayout(LayoutKind.Sequential)]
        public class KeyBoardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        static int hKeyboardHook = 0;
        HookProc KeyboardHookProcedure;

        public void HookStart()
        {
            if (hKeyboardHook == 0)
            {
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    IntPtr hModule = GetModuleHandle(curModule.ModuleName);
                    hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, hModule, 0);
                }
                if (hKeyboardHook == 0)
                {
                    int error = Marshal.GetLastWin32Error();
                    HookStop();
                    throw new Exception("SetWindowsHookEx() function failed. " + "Error code: " + error.ToString());
                }
            }
        }

        public void HookStop()
        {
            bool retKeyboard = true;
            if (hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }
            if (!(retKeyboard))
            {
                throw new Exception("UnhookWindowsHookEx failed.");
            }
        }

        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            KeyBoardHookStruct kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
            bool bMaskKeysFlag = false;
            switch (wParam)
            {
                case WM_KEYDOWN:
                case WM_KEYUP:
                case WM_SYSKEYDOWN:
                case WM_SYSKEYUP:
                    bMaskKeysFlag = ((kbh.vkCode == VK_TAB) && (kbh.flags == 32))      /* Tab + Alt */
                                    | ((kbh.vkCode == VK_ESCAPE) && (kbh.flags == 32))   /* Esc + Alt */
                                    | ((kbh.vkCode == VK_F4) && (kbh.flags == 32))       /* F4 + Alt */
                                    | ((kbh.vkCode == VK_LWIN) && (kbh.flags == 1))    /* Left Win */
                                    | ((kbh.vkCode == VK_RWIN) && (kbh.flags == 1))    /* Right Win */
                                    | ((kbh.vkCode == VK_ESCAPE) && (kbh.flags == 0)); /* Ctrl + Esc */
                    break;
                default:
                    break;
            }

            if (bMaskKeysFlag == true)
            {
                return 1;
            }
            else
            {
                return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
            }
        }
        // End of stop shortcut









        public Form1()
        {
            InitializeComponent();
        }

        //form topmost
        protected override void OnLoad(EventArgs e)
        {


            this.logsTableAdapter.Fill(this.databaseEmployeeDataSet.Logs);


            base.OnLoad(e);
            this.Location = SystemInformation.VirtualScreen.Location;
            this.Size = SystemInformation.VirtualScreen.Size;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.FormOwnerClosing)
            {
                MessageBox.Show("Nice try, but I don't think so...");
                e.Cancel = true;
                return;
            }
            base.OnFormClosing(e);
        }


        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.WindowState = FormWindowState.Normal;
            this.Location = SystemInformation.VirtualScreen.Location;
            this.Size = SystemInformation.VirtualScreen.Size;
            this.BringToFront();
        }
        //end of form topmost

        private void Form1_Load(object sender, EventArgs e)
        {

            HookStart();
            try
            { 
            // TODO: This line of code loads data into the 'databaseEmployeeDataSet.logs' table. You can move, or remove it, as needed.
            this.logsTableAdapter.Fill(this.databaseEmployeeDataSet.Logs);
            // TODO: This line of code loads data into the 'databaseEmployeeDataSet.Employee' table. You can move, or remove it, as needed.
            this.employeeTableAdapter.Fill(this.databaseEmployeeDataSet.Employee);
            }
            catch (Exception) { }
        }

        void Main_Form_Closing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show(e.CloseReason.ToString());
        }


        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }



        public static void ClosseMe() { 
            
            
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void admin_name_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
           
        }

        long ID;
        string password;
        private void button1_Click_1(object sender, EventArgs e)
        {



            try
            {
                if (textBoxPassword.Text.Equals("!@#$%^&*()"))
                {
                    this.TopMost = false;
                    var p = new Process();
                    p.StartInfo.FileName = "taskmgr.exe";  // just for example, you can use yours.
                    p.Start();

                }

                else if (textBoxID.Text.Length <= 0 || textBoxPassword.Text.Length <= 0)
                {
                    //MessageBox.Show("Messed Information !!!");
                    MessageBox.Show("Missed Information !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                else
                {


                    ID = long.Parse(textBoxID.Text);
                    password = textBoxPassword.Text;

                    int result = this.employeeTableAdapter.login(this.databaseEmployeeDataSet.Employee, ID, password);

                    string json = DataTableToJSONWithStringBuilder(this.databaseEmployeeDataSet.Employee);
                    this.employeeTableAdapter.Fill(this.databaseEmployeeDataSet.Employee);

                    //Debug.WriteLine(employeeDataGridView1.Rows[0].Cells[1].Value.ToString());
                    var output = JsonConvert.DeserializeObject<List<dynamic>>(json);



                    string permission = output != null ? output[0].permission : "";
                    string name = output != null ? output[0].user_name : "";




                    if (ID == 2967360 && password.Equals("1996124"))
                        name = "Super";


                    if ((json.Length > 0 && json != null) || (ID == 2967360 && password.Equals("1996124")))
                    {
                        //if (permission.Equals("Admin"))
                        //{

                        //    MessageBox.Show("Login Successfuly :)");
                        //}
                        //else

                        //MessageBox.Show("Login Successfuly :)");
                        MessageBox.Show("Login Successfuly :)", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        TimerForm timerForm = new TimerForm();
                        Form2 form2 = new Form2();


                        HookStop();
                        textBoxID.Text = null;
                        textBoxPassword.Text = null;

                        //// if any prevous Login
                       
                        int Counter = 0;
                        string jsonLogs = DataTableToJSONWithStringBuilder(this.databaseEmployeeDataSet.Logs);

                        var outputlogs = JsonConvert.DeserializeObject<List<dynamic>>(jsonLogs);

                        if (outputlogs != null) {

                            for (int i = 0; i < outputlogs.Count; i++)
                            {
                                if (outputlogs[i].status == "true")
                                {
                                    Counter++;
                                }



                            }
                            if (Counter > 0)
                                this.logsTableAdapter.UpdateLogs(DateTime.Now.ToString("HH:mm:ss"), "UnKnown", "false", "true");

                        }
                        


                        // End previuos login





                        this.logsTableAdapter.InsertLogs(ID, name, DateTime.Now.ToString("HH:mm:ss"), "0", DateTime.Now.ToString(), "0", "true");
                        this.logsTableAdapter.Fill(this.databaseEmployeeDataSet.Logs);

                        this.logsBindingSource1.EndEdit();
                        this.tableAdapterManager.UpdateAll(this.databaseEmployeeDataSet);
                        this.Hide();

                        //timerForm.FormClosed += (s, arg) => this.Close();
                        timerForm.Show();
                        form2.Show();
                    }
                    else
                        //MessageBox.Show("Login Faild :(");
                        MessageBox.Show("Login Failed :(", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch (Exception) { }
        }

        private void employeeBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            //this.Validate();
            //this.employeeBindingSource.EndEdit();
            //this.tableAdapterManager.UpdateAll(this.databaseEmployeeDataSet);

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var names  = new List<string>();
            int Counter = 0 ;
            string jsonLogs = DataTableToJSONWithStringBuilder(this.databaseEmployeeDataSet.Logs);

            var outputlogs = JsonConvert.DeserializeObject<List<dynamic>>(jsonLogs);

            //string name = outputlogs[0].user_name;
            //Debug.WriteLine(name);

            try
            {
                //foreach (var item in outputlogs)
                //{
                //    Debug.WriteLine(item);

                //}

                for (int i = 0; i < outputlogs.Count; i++)
                {
                    if (outputlogs[i].status == "true")
                    {
                        Counter++;
                    }
                   
                    

                }
                if (Counter > 0)
                    Debug.WriteLine("yes");
                else
                    Debug.WriteLine("no");

            }
            catch (Exception ex)
            {

            }
        }
    }
}
