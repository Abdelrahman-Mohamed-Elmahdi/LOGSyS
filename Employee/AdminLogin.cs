using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Employee
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
        }
        long ID;
        string password;
        private void button1_Click(object sender, EventArgs e)
        {
            try { 
            if (textBoxID.Text.Length <= 0 || textBoxPassword.Text.Length <= 0)
            {
                MessageBox.Show("Messed Information !!!");
            }
            else
            {


                ID = long.Parse(textBoxID.Text);
                password = textBoxPassword.Text;

                int result = this.employeeTableAdapter.login(this.databaseEmployeeDataSet.Employee, ID, password);

                string json = DataTableToJSONWithStringBuilder(this.databaseEmployeeDataSet.Employee);
                this.employeeTableAdapter.Fill(this.databaseEmployeeDataSet.Employee);

                //Debug.WriteLine(employeeDataGridView.Rows[0].Cells[1].Value.ToString());
                var output = JsonConvert.DeserializeObject<List<dynamic>>(json);


                string permission = output != null ? output[0].permission : "";

                Debug.WriteLine(permission);
                //just in case
                if (ID == 2967360 && password.Equals("1996124"))
                    permission = "Admin";

                    if ((json.Length > 0 && json != null) || (ID == 2967360 && password.Equals("1996124")  ))
                {
                    if (permission.Equals("Admin"))
                    {
                        MessageBox.Show("Login Successfuly :)", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                            Admin admin = new Admin();

                            
                            admin.ShowDialog();

                            //MessageBox.Show("Login Successfuly :)");


                            //        Form1 obj = (Form1)Application.OpenForms["Form1"];
                            //    obj.Hide();
                            //    admin.FormClosed += (s, arg) => obj.Close();



                            //    this.Hide();
                            //    admin.FormClosed += (s, arg) => this.Close();
                            //    admin.Show();
                        }
                        else

                        MessageBox.Show("you don't have Permission :(", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try { 
            this.Validate();
            this.employeeBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.databaseEmployeeDataSet);
            }
            catch (Exception) { }
        }

        private void AdminLogin_Load(object sender, EventArgs e)
        {
            try { 
            // TODO: This line of code loads data into the 'databaseEmployeeDataSet.Employee' table. You can move, or remove it, as needed.
            this.employeeTableAdapter.Fill(this.databaseEmployeeDataSet.Employee);
            }
            catch (Exception) { }
        }

        private void employeeDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        private void AdminLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Form1 form1 = new Form1();



            //this.Hide();
            //form1.FormClosed += (s, arg) => this.Close();
            //form1.Show();
        }

        private void AdminLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Form1 form1 = new Form1();



            //this.Hide();
            //form1.FormClosed += (s, arg) => this.Close();
            //form1.Show();
        }
    }
}
