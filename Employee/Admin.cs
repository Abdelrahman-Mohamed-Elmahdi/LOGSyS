using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Employee
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void radioAdmin_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void labelID_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBoxID_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelPassword_Click(object sender, EventArgs e)
        {

        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void radioUser_CheckedChanged(object sender, EventArgs e)
        {

        }
        long ID;
        string name, permission, password;
        private void Login_name_Click(object sender, EventArgs e)
        {


            if (validateFields() == 1)
                MessageBox.Show("Missed Information !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            else if (textBoxID.Text.Length < 5 || textBoxID.Text.Length > 11) {
                MessageBox.Show("Invalid ID !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {


                try
                {
                    ID = long.Parse(textBoxID.Text);
                    name = textBoxName.Text;
                    permission = "";
                    bool isChecked = radioAdmin.Checked;

                    if (isChecked)
                        permission = radioAdmin.Text;
                    else
                        permission = radioUser.Text;
                    password = textBoxPassword.Text;

                    employeeDataGridView.AllowUserToAddRows = true;


                    this.employeeTableAdapter.InsertEmployee(
                         ID, name, permission, password);



                    this.employeeTableAdapter.Fill(this.databaseEmployeeDataSet.Employee);

                    this.employeeBindingSource.EndEdit();
                    this.tableAdapterManager.UpdateAll(this.databaseEmployeeDataSet);

                    MessageBox.Show("Inserted Successfully :)", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                   
                }
                catch (Exception ex)
                {
                   
                    MessageBox.Show("ID is already exist :(", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                textBoxName.Text = string.Empty;
                textBoxID.Text = string.Empty;
                textBoxPassword.Text = string.Empty;
                radioAdmin.Checked = false;
                radioUser.Checked = false;



            }

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

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ID = long.Parse(textBoxID.Text);


                this.employeeTableAdapter.DeleteEmployee(ID);

                this.employeeTableAdapter.Fill(this.databaseEmployeeDataSet.Employee);

                this.employeeBindingSource.EndEdit();
                this.tableAdapterManager.UpdateAll(this.databaseEmployeeDataSet);

                MessageBox.Show("Deleted Successfully :)", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

               

                textBoxName.Text = string.Empty;
                textBoxID.Text = string.Empty;
                textBoxPassword.Text = string.Empty;
                radioAdmin.Checked = false;
                radioUser.Checked = false;
            }
            catch (Exception ex)
            {

            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (validateFields() == 1)
                MessageBox.Show("Missed Information !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            else if (textBoxID.Text.Length < 4 || textBoxID.Text.Length > 11)
            {
                MessageBox.Show("Invalid ID !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {


                ID = long.Parse(textBoxID.Text);
                name = textBoxName.Text;
                permission = "";
                bool isChecked = radioAdmin.Checked;

                if (isChecked)
                    permission = radioAdmin.Text;
                else
                    permission = radioUser.Text;
                password = textBoxPassword.Text;

                try
                {
                    this.employeeTableAdapter.UpdateEmployee(ID, name, permission, password, ID);


                    this.employeeTableAdapter.Fill(this.databaseEmployeeDataSet.Employee);
                    this.employeeBindingSource.EndEdit();
                    this.tableAdapterManager.UpdateAll(this.databaseEmployeeDataSet);

                    MessageBox.Show("Updated Successfully :)", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);


                }
                catch (Exception ex)
                {

                }
            }
            textBoxName.Text = string.Empty;
                textBoxID.Text = string.Empty;
                textBoxPassword.Text = string.Empty;
                radioAdmin.Checked = false;
                radioUser.Checked = false;
            
        }

        private void employeeDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void employeeDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        int validateFields() {

           
            if(textBoxID.TextLength == 0 || 
                textBoxName.TextLength == 0 || 
                textBoxPassword.TextLength == 0 || 
                (radioAdmin.Checked == false && radioUser.Checked == false))

            return 1;

            else

            return 0;   

            
        }
        private void employeeDataGridView_Click(object sender, EventArgs e)
        {
          

        }

        private void employeeDataGridView_RowHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            try { 
            string value = employeeDataGridView.CurrentRow.Cells[1].Value.ToString();

               textBoxID.Text = employeeDataGridView.CurrentRow.Cells[0].Value.ToString();
               textBoxName.Text = employeeDataGridView.CurrentRow.Cells[1].Value.ToString();
               permission = employeeDataGridView.CurrentRow.Cells[2].Value.ToString();
            if (permission.Equals("Admin"))
                radioAdmin.Checked = true;
            else
                radioUser.Checked = true;

            textBoxPassword.Text = employeeDataGridView.CurrentRow.Cells[3].Value.ToString();


            }
            catch (Exception) { }
        }

       

        private void Admin_Load(object sender, EventArgs e)
        {
            try { 

            this.Size = Screen.PrimaryScreen.WorkingArea.Size;



            // TODO: This line of code loads data into the 'databaseEmployeeDataSet.logs' table. You can move, or remove it, as needed.
            //this.logsTableAdapter.Fill(this.databaseEmployeeDataSet.logs);


            // TODO: This line of code loads data into the 'databaseEmployeeDataSet.Employee' table. You can move, or remove it, as needed.
            this.employeeTableAdapter.Fill(this.databaseEmployeeDataSet.Employee);
            this.logsTableAdapter1.Fill(this.databaseEmployeeDataSet.Logs);

            }
            catch (Exception) { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try { 
            AdminLogin form1 = (AdminLogin)Application.OpenForms["AdminLogin"];



                //Form1 form1 = new Form1();
                form1.Hide();
                this.Hide();

          
            }
            catch (Exception) { }

        }

        private void textBoxSearchID_TextChanged(object sender, EventArgs e)
        {

            try
            {
                this.logsTableAdapter1.getByID(this.databaseEmployeeDataSet.Logs, long.Parse(textBoxSearchID.Text));
            }catch(Exception ex)
            {

            }
        }

        private void dateTimePickerSearchDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.logsTableAdapter1.getByDate(this.databaseEmployeeDataSet.Logs, dateTimePickerSearchDate.Value.ToString("yyyy-MM-dd"));
            }
            catch(Exception ex)
            {

            }
            }

        private void employeeDataGridView_RowHeaderCellChanged(object sender, DataGridViewRowEventArgs e)
        {

        }


        private void copyAlltoClipboard()
        {
            logsDataGridView.SelectAll();
            DataObject dataObj = logsDataGridView.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = true;
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Microsoft.Office.Interop.Excel.Worksheet sheet1 = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                int StartCol = 1;
                int StartRow = 1;
                int j = 0, i = 0;

                //Write Headers
                for (j = 0; j < logsDataGridView.Columns.Count; j++)
                {
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[StartRow, StartCol + j];
                    myRange.Value2 = logsDataGridView.Columns[j].HeaderText;
                }

                StartRow++;

                //Write datagridview content
                for (i = 0; i < logsDataGridView.Rows.Count; i++)
                {
                    for (j = 0; j < logsDataGridView.Columns.Count; j++)
                    {
                        try
                        {
                            Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[StartRow + i, StartCol + j];
                            myRange.Value2 = logsDataGridView[j, i].Value == null ? "" : logsDataGridView[j, i].Value.ToString();
                        }
                        catch
                        {
                            ;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.Show();
        }

        private void Admin_Shown(Object sender, EventArgs e)
        {

            MessageBox.Show("You are in the Form.Shown event.");
        }   
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
