using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DatabaseSys
{
    public partial class Form1 : Form
    {
        string CreateCustomerQuery; // Insert customer query for final transaction
        string SalesFinishQuery;
        string ServiceSaveQuery; // Save service query for receiving jobs
        string ServiceFinishQuery; // Finish service query for finished jobs
        public TextBox textBox1;
        String CustID = "";

        public Form1()
        {
            InitializeComponent();
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'aDV_DBDataSet.TB_Customers' table. You can move, or remove it, as needed.
            //this.tB_CustomersTableAdapter.Fill(this.aDV_DBDataSet.TB_Customers);
            label_DateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            // Hide second page on load
            HideSecondPage(button_SalesForm, button_ServiceForm, button_ReturnToFirstPage, groupBox_Service, groupBox_Sales, groupBox_Transaction);

            // Avoid group box overlap for sales and service form
            groupBox_Customer.Location = new System.Drawing.Point(18, 90);
            groupBox_Service.Location = new System.Drawing.Point(18, 90);
            groupBox_Sales.Location = new System.Drawing.Point(18, 90); 
            groupBox_Transaction.Location = new System.Drawing.Point(498, 90);

            // Combobox Items
            comboBox_SeRepairType.Items.Add("Repair");
            comboBox_SeRepairType.Items.Add("Deep Cleaning");
            comboBox_SeRepairType.Items.Add("Hardware Replacement");
            comboBox_SeRepairType.Items.Add("Hardware Replacement & Repair");

            // Temp Table for Items
            try
            {
                SqlConnQuery($"CREATE TABLE TempTable(ItemID float, ItemName nvarchar(255), Price float, Count int, TotalPrice float);"); //Sales Table
            }
            catch
            {

            }
            // Temp Table for Sales & Service Transaction
            try
            {
                SqlConnQuery($"CREATE TABLE TempTableSTrans(ItemID float, ItemName nvarchar(255), Price float, Count int, TotalPrice float);"); //Service Table
            }
            catch
            {

            }
            try
            {
                SqlConnQuery($"CREATE TABLE TempTableSeTrans(ItemID float, ItemName nvarchar(255), Price float, Count int, TotalPrice float);"); //Service Table
            }
            catch
            {

            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SqlConnQuery($"DROP TABLE TempTable");
            SqlConnQuery($"DROP TABLE TempTableSTrans");
            SqlConnQuery($"DROP TABLE TempTableSeTrans");
        }

        // Customer Page
        private void textBox_CCustomerSearch_TextChanged(object sender, EventArgs e)
        {
            string query = $"Select CustomerID, FirstName, LastName, Email, Phone, TempAddress FROM TB_Customers WHERE FirstName LIKE '%{textBox_CCustomerSearch.Text}%' OR LastName LIKE '%{textBox_CCustomerSearch.Text}%'";
            SqlConnQuery_DataTable(query, dataGridView_CCustomerList);
        }
        private void dataGridView_CCustomerList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Int32 index = dataGridView_CCustomerList.Rows.Count - 1; // Row count of current rows in grid view
            if (e.RowIndex > -1)
            {
                CustID = dataGridView_CCustomerList.Rows[e.RowIndex].Cells[0].Value.ToString();
                dataGridView_Transaction(CustID);

                String FName = dataGridView_CCustomerList.Rows[e.RowIndex].Cells[1].Value.ToString();
                String LName = dataGridView_CCustomerList.Rows[e.RowIndex].Cells[2].Value.ToString();
                
                label_CDetailsName.Text = FName + " " + LName;
                label_SeFullName.Text = FName + " " + LName;

                label_CDetailsEmail.Text = dataGridView_CCustomerList.Rows[e.RowIndex].Cells[3].Value.ToString();
                label_CDetailsPhone.Text = dataGridView_CCustomerList.Rows[e.RowIndex].Cells[4].Value.ToString();
                label_CDetailsAddress.Text = dataGridView_CCustomerList.Rows[e.RowIndex].Cells[5].Value.ToString();

                CreateCustomerQuery = $"UPDATE TB_Customers SET FirstName='{FName}', LastName='{LName}', Email='{label_CDetailsEmail.Text}', Phone='{label_CDetailsPhone.Text}', TempAddress='{label_CDetailsAddress.Text}' WHERE CustomerID = {CustID}";
                labelTEST.Text = CreateCustomerQuery;
            }
            if (e.RowIndex == index)
            {
                CreateCustomerQuery = null;
                labelTEST.Text = CreateCustomerQuery;
                dataGridView_Transaction("null");
            }
        }
        private void button_CClearDetails_Click(object sender, EventArgs e)
        {
            label_CDetailsName.Text = "---";
            label_CDetailsPhone.Text = "---";
            label_CDetailsAddress.Text = "---";
            label_CDetailsEmail.Text = "---";
            textBox_CCustomerSearch.Text = "";
            label_SeFullName.Text = "";
            CreateCustomerQuery = null;

            // Clear Transaction History view
            dataGridView_Transaction("null"); // String search because query is finding numbers
        }
        private void dataGridView_Transaction(String CustID)
        {
            string query = $"Select OrderID, Details, Quantity FROM TB_ItemOrders WHERE CustomerID LIKE '%{CustID}%'";
            SqlConnQuery_DataTable(query, dataGridView_CTransactionHistory);

        }
        private void button_CAddCustomer_Click(object sender, EventArgs e)
        {
            String empty = "";
            String fn = textBox_CFName.Text;
            String ln = textBox_CLName.Text;
            String email = textBox_CEmail.Text;
            String phone = textBox_CPhone.Text;
            String address = textBox_CAddress.Text;
            String ID = "CustomerID";
            if (textBox_CFName.Text == empty || textBox_CLName.Text == empty || textBox_CEmail.Text == empty || textBox_CPhone.Text == empty)
            {
                label_CEmptyFieldWarning.Show();
            }
            else
            {
                String NewQuery = "SELECT CustomerID FROM TB_Customers WHERE CustomerID = (SELECT max(CustomerID) FROM TB_Customers)";
                float CustomerID = float.Parse(SqlConnQueryReturn(NewQuery, ID));
                CustomerID++;

                try
                {
                    double a = double.Parse(textBox_CPhone.Text);
                    label_CEmptyFieldWarning.Hide();

                    if (textBox_CPhone.TextLength != 11)
                    {
                        label_CEmptyFieldWarning.Text = "Invalid Phone Input";
                    }
                    else
                    {
                        label_CEmptyFieldWarning.Hide();
                        CreateCustomerQuery = $"INSERT INTO TB_Customers(CustomerID, FirstName, LastName, Email, Phone, TempAddress) VALUES({CustomerID}, '{fn}', '{ln}', '{email}', {phone}, '{address}')";
                        labelTEST.Text = CreateCustomerQuery; // Save query for later
                        label_SeFullName.Text = textBox_CFName.Text + " " + textBox_CLName.Text; // Show full name in service form

                        CustID = CustomerID.ToString();
                    }
                }
                catch
                {
                    label_CEmptyFieldWarning.Show();
                    label_CEmptyFieldWarning.Text = "Invalid Phone Input";
                }
            }
        }
        private void button_CClearForm_Click(object sender, EventArgs e)
        {
            textBox_CFName.Text = "";
            textBox_CLName.Text = "";
            textBox_CEmail.Text = "";
            textBox_CPhone.Text = "";
            textBox_CAddress.Text = "";
            CreateCustomerQuery = null;
        }
        private void button_ProceedCForm_Click_1(object sender, EventArgs e)
        {
            if (CreateCustomerQuery == "" || CreateCustomerQuery == null)
            {
                label_CEmptyQueryWarning.Show();
            }
            else if (label_CEmptyFieldWarning.Visible == true)
            {
                label_CEmptyQueryWarning.Show();
            }
            else
            {
                // Hide first page on click
                groupBox_Customer.Hide();
                label_F1Header2.Hide();

                // Show second page on click
                label_CEmptyQueryWarning.Hide();
                ShowSecondPage(button_SalesForm, button_ServiceForm, button_ReturnToFirstPage, groupBox_Sales, groupBox_Transaction);
            }
        }

        // Sales Page
        private void textBox_SItemSearch_TextChanged(object sender, EventArgs e)
        {
            string query = $"SELECT ItemID, ItemName, Details, Price, Stock FROM TB_Items WHERE ItemName LIKE '%{textBox_SItemSearch.Text}%'";
            SqlConnQuery_DataTable(query, dataGridView_SItemList);
        }
        private void dataGridView_SItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Int32 index = dataGridView_SItemList.Rows.Count - 1; // Row count of current rows in grid view
            if (e.RowIndex > -1)
            {
                label_SItemID.Text = dataGridView_SItemList.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox_SItemName.Text = dataGridView_SItemList.Rows[e.RowIndex].Cells[1].Value.ToString();
                label_SPrice.Text = dataGridView_SItemList.Rows[e.RowIndex].Cells[3].Value.ToString();
                label_SStockCount.Text = dataGridView_SItemList.Rows[e.RowIndex].Cells[4].Value.ToString();
                try
                {
                    numericUpDown_SCount.Maximum = Int32.Parse(label_SStockCount.Text);
                    numericUpDown_SCount.Minimum = 1;

                    if (Int32.Parse(label_SStockCount.Text) <= 0)
                    {
                        label_SAvailableBool.Text = "NO";
                    }
                    else
                    {
                        label_SAvailableBool.Text = "YES";
                    }
                }
                catch
                {

                }
            }
            if (e.RowIndex == index)
            {
                label_SItemID.Text = "---";
                textBox_SItemName.Text = "";
                label_SPrice.Text = "---";
                label_SStockCount.Text = "---";
            }
        }
        private void button_SAddItem_Click(object sender, EventArgs e)
        {
            if (label_SAvailableBool.Text == "NO")
            {
                label5.Text = "No stock, cannot add.";
                label5.Show();
            }
            else {
                try
                {
                    float ItemID = float.Parse(label_SItemID.Text);
                    float SalePrice = float.Parse(label_SPrice.Text);
                    float Count = float.Parse(numericUpDown_SCount.Text);
                    float TotalPrice = SalePrice * Count;

                    string AddItemQuery = $"INSERT INTO TempTable (ItemID, ItemName, Price, Count, TotalPrice) VALUES ('{label_SItemID.Text}', '{textBox_SItemName.Text}', '{SalePrice}', {Count}, '{TotalPrice}')";
                    SqlConnQuery(AddItemQuery);

                    string query = $"SELECT * FROM TempTable";
                    SqlConnQuery_DataTable(query, dataGridView_SCurrentItems);
                }
                catch
                {

                }
            }
        }
        private void dataGridView_SCurrentItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                label_SCurrItemID.Text = dataGridView_SCurrentItems.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
        }
        private void button_SRemove_Click(object sender, EventArgs e)
        {
            Int32 index = dataGridView_SCurrentItems.Rows.Count - 1;
            // Create a connection to the database.
            try
            {
                string DelItemQuery = $"DELETE FROM TempTable WHERE ItemID = {float.Parse(label_SCurrItemID.Text)}";
                SqlConnQuery(DelItemQuery);
            }
            catch
            {

            }
            string query = $"SELECT * FROM TempTable";
            SqlConnQuery_DataTable(query, dataGridView_SCurrentItems);
        }
        private void button_SClear_Click(object sender, EventArgs e)
        {
            string DelRecordsQuery = $"DELETE FROM TempTable;";
            SqlConnQuery(DelRecordsQuery);

            string query = $"SELECT * FROM TempTable";
            SqlConnQuery_DataTable(query, dataGridView_SCurrentItems);
        }
        private void button_SFinish_Click(object sender, EventArgs e)
        {
            string DelRecordsQuery = $"DELETE FROM TempTableSTrans;"; // Refresh Trans Table Per Click
            SqlConnQuery(DelRecordsQuery);

            string query = "INSERT INTO TempTableSTrans (ItemID, ItemName, Price, Count, TotalPrice) SELECT ItemID, ItemName, Price, Count, TotalPrice FROM TempTable;";
            SqlConnQuery(query);

            string query2 = "SELECT * FROM TempTableSTrans";

            SqlConnQuery_DataTable(query2, dataGridView_STrans);
            GetTotalPrice();
        }
        private void GetTotalPrice()
        {
            double ServicePrice = ServiceType(comboBox_SeRepairType.Text);
            double TotalPrice = SqlConnQueryPrice("SELECT SUM(TotalPrice) AS total FROM TempTableSTrans");
            double FinalPrice = ServicePrice + TotalPrice;
            label_TTotalPrice.Text = FinalPrice.ToString();
            label_SServicePrice.Text = ServicePrice.ToString();
        }

        // Service Page
        private void richTextBox_SeRepairDetailsTextBox_Click(object sender, EventArgs e)
        {
            richTextBox_SeRepairDetailsTextBox.ForeColor = Color.Black;
        }
        private void comboBox_SeRepairType_Click(object sender, EventArgs e)
        {
            comboBox_SeRepairType.ForeColor = Color.Black;
        }
        private void button_SeSave_Click(object sender, EventArgs e)
        {
            String type = "Save";
            ServiceQuery(type);
        }
        private void button_SeFinish_Click(object sender, EventArgs e)
        {
            button_SePartsFinish_Click(sender, e);
            String type = "Service";
            ServiceQuery(type);
        }
        private void ServiceQuery(String type)
        {
            string ID = "JobID";
            string[] SeComboBoxText = { "Repair", "Deep Cleaning", "Hardware Replacement", "Hardware Replacement & Repair" };
            if (richTextBox_SeRepairDetailsTextBox.Text == null || richTextBox_SeRepairDetailsTextBox.Text == "")
            {
                richTextBox_SeRepairDetailsTextBox.ForeColor = Color.Red;
                richTextBox_SeRepairDetailsTextBox.Text = "Please write details.";
            }
            else if (!SeComboBoxText.Contains(comboBox_SeRepairType.Text))
            {
                comboBox_SeRepairType.ForeColor = Color.Red;
                comboBox_SeRepairType.Text = "Please choose type.";
            }
            else
            {
                if (type == "Save")
                {
                    // Get JobID
                    String query = "SELECT JobID FROM TB_ShopService WHERE JobID = (SELECT max(JobID) FROM TB_ShopService)";
                    label_SeJobID.Text = SqlConnQueryReturn(query, ID);
                    // Adding JobID with 1 for ID continuity 
                    float JobIDCount = 1 + float.Parse(label_SeJobID.Text); // Or convert to float; depends on jobid value in db
                    
                    ServiceSaveQuery = $"INSERT INTO TB_ShopService (JobID, DateReceived, Details, CustomerID) VALUES({JobIDCount}, '{label_DateTime.Text}', '{richTextBox_SeRepairDetailsTextBox.Text}', {CustID}); ";
                    SqlConnQuery(ServiceSaveQuery);
                    label4.Text = ServiceSaveQuery;
                }
                else
                {
                    // Get JobID
                    String query = "SELECT JobID FROM TB_RepairDetails WHERE JobID = (SELECT max(JobID) FROM TB_RepairDetails)";
                    label_SeJobID.Text = SqlConnQueryReturn(query, ID);
                    // Adding JobID with 1 for ID continuity 
                    float JobIDCount = 1 + float.Parse(label_SeJobID.Text); // Or convert to float; depends on jobid value in db
                    
                    float ServicePrice = ToSingle(ServiceType(comboBox_SeRepairType.Text));
                    ServiceFinishQuery = $"INSERT INTO TB_RepairDetails (DateReturned, Details, Price, JobID, PO_ID) VALUES('{label_DateTime.Text}', '{richTextBox_SeRepairDetailsTextBox.Text}', {ServicePrice}, {JobIDCount}, {100});";
                    label4.Text = ServiceFinishQuery;
                }
            }
        }
        private double ServiceType(String type)
        {
            int Price;
            if (type == "Repair")
            {
                Price = 2000;
            }
            else if (type == "Deep Cleaning")
            {
                Price = 600;
            }
            else if (type == "Hardware Replacement")
            {
                Price = 500;
            }
            else if (type == "Hardware Replacement & Repair")
            {
                Price = 2500;
            }
            else
            {
                Price = 0;
            }
            return Price;
        }

        private void button_SeShowReplacement_Click(object sender, EventArgs e)
        {
            if (groupBox_SeReplacementForm.Visible == false)
            {
                groupBox_SeReplacementForm.Show();
            }
            else
            {
                groupBox_SeReplacementForm.Hide();
            }
        }
        private void textBox_SeItemSearch_TextChanged(object sender, EventArgs e)
        {
            string query = $"SELECT ItemID, ItemName, Details, Price, Stock FROM TB_Items WHERE ItemName LIKE '%{textBox_SeItemSearch.Text}%'";
            SqlConnQuery_DataTable(query, dataGridView_SeItemList);
        }
        private void dataGridView_SeItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Int32 index = dataGridView_SeItemList.Rows.Count - 1; // Row count of current rows in grid view
            if (e.RowIndex > -1)
            {
                label_SItemID.Text = dataGridView_SeItemList.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox_SItemName.Text = dataGridView_SeItemList.Rows[e.RowIndex].Cells[1].Value.ToString();
                label_SPrice.Text = dataGridView_SeItemList.Rows[e.RowIndex].Cells[3].Value.ToString();
                label_SStockCount.Text = dataGridView_SeItemList.Rows[e.RowIndex].Cells[4].Value.ToString();
                try
                {
                    numericUpDown_SCount.Maximum = Int32.Parse(label_SStockCount.Text);
                    numericUpDown_SCount.Minimum = 1;

                    if (Int32.Parse(label_SStockCount.Text) <= 0)
                    {
                        label_SAvailableBool.Text = "NO";
                    }
                    else
                    {
                        label_SAvailableBool.Text = "YES";
                    }
                }
                catch
                {

                }
            }
            if (e.RowIndex == index)
            {
                label_SItemID.Text = "---";
                textBox_SItemName.Text = "";
                label_SPrice.Text = "---";
                label_SStockCount.Text = "---";
            }
        }
        private void button_SeAddItem_Click(object sender, EventArgs e)
        {
            if (label_SAvailableBool.Text == "NO")
            {
                label5.Text = "No stock, cannot add.";
                label5.Show();
            }
            else
            {
                try
                {
                    float ItemID = float.Parse(label_SItemID.Text);
                    float SalePrice = float.Parse(label_SPrice.Text);
                    float Count = float.Parse(numericUpDown_SeCount.Text);
                    float TotalPrice = SalePrice * Count;

                    string AddItemQuery = $"INSERT INTO TempTable (ItemID, ItemName, Price, Count, TotalPrice) VALUES ('{label_SItemID.Text}', '{textBox_SItemName.Text}', '{SalePrice}', {Count}, '{TotalPrice}')";
                    SqlConnQuery(AddItemQuery);

                    string query = $"SELECT * FROM TempTable";
                    SqlConnQuery_DataTable(query, dataGridView_SeCurrentItems);
                }
                catch
                {

                }
            }
        }
        private void dataGridView_SeCurrentItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                label_SCurrItemID.Text = dataGridView_SeCurrentItems.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
        }
        private void button_SeRemove_Click(object sender, EventArgs e)
        {
            Int32 index = dataGridView_SeCurrentItems.Rows.Count - 1;
            try
            {
                string DelItemQuery = $"DELETE FROM TempTable WHERE ItemID = {float.Parse(label_SCurrItemID.Text)}";
                SqlConnQuery(DelItemQuery);
            }
            catch
            {

            }
            string query = $"SELECT * FROM TempTable";
            SqlConnQuery_DataTable(query, dataGridView_SeCurrentItems);
        }
        private void button_SeClearReplacement_Click(object sender, EventArgs e)
        {
            string DelRecordsQuery = $"DELETE FROM TempTable;";
            SqlConnQuery(DelRecordsQuery);

            string query = $"SELECT * FROM TempTable";
            SqlConnQuery_DataTable(query, dataGridView_SeCurrentItems);
        }
        private void button_SePartsFinish_Click(object sender, EventArgs e)
        {
            button_SFinish_Click(sender, e);
        }

        // Transaction Page
        private void dataGridView_STrans_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                label_TransID.Text = dataGridView_STrans.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
        }
        private void button_TRemove_Click(object sender, EventArgs e)
        {
            Int32 index = dataGridView_STrans.Rows.Count - 1;
            try
            {
                string DelItemQuery = $"DELETE FROM TempTableSTrans WHERE ItemID = {float.Parse(label_TransID.Text)}";
                SqlConnQuery(DelItemQuery);
            }
            catch
            {
            }
            string query = $"SELECT * FROM TempTableSTrans";
            SqlConnQuery_DataTable(query, dataGridView_STrans);

            GetTotalPrice();
        }
        private void button_TClear_Click(object sender, EventArgs e)
        {
            string DelRecordsQuery = $"DELETE FROM TempTableSTrans;";
            SqlConnQuery(DelRecordsQuery);

            string query = $"SELECT * FROM TempTableSTrans";
            SqlConnQuery_DataTable(query, dataGridView_STrans);

            label_TTotalPrice.Text = "";
        }
        private void button_TProcess_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnQuery(CreateCustomerQuery);
                if (groupBox_Sales.Visible == true)
                {
                    //"INSERT INTO TB_ItemOrders (OrderID, Details, ItemID, Quantity, Price, CustomerID) SELECT Order, Details, ItemID, Count, Price, Customer FROM TempTableSTrans WHERE Order = @Order and Customer = @Customer"
                    String query = "INSERT INTO TB_ItemOrders (OrderID, Details, ItemID, Quantity, Price, CustomerID) SELECT Order, Details, ItemID, Quantity, Price, Customer FROM TempTableSTrans WHERE Order = @Order and Customer = @Customer";
                    SqlConnQuerySalesFinal(query);
                }
                else
                {
                    SqlConnQuery(ServiceFinishQuery);
                }
            }
            catch
            {
            }
        }

        // 2nd Page Menu
        private void button_SalesForm_Click(object sender, EventArgs e)
        {
            if (groupBox_Sales.Visible == false)
            {
                label_SItemID.Text = "---";
                textBox_SItemName.Text = "";
                label_SPrice.Text = "---";
                label_SStockCount.Text = "---";
                label_SCurrItemID.Text = null;
            }

            groupBox_Sales.Show();
            groupBox_Service.Hide();
        }
        private void button_ServiceForm_Click(object sender, EventArgs e)
        {
            if (groupBox_Service.Visible == false)
            {
                label_SCurrItemID.Text = null;
            }

            groupBox_Service.Show();
            groupBox_Sales.Hide();
            groupBox_SeReplacementForm.Hide();
        }
        private void button_ReturnToFirstPage_Click(object sender, EventArgs e)
        {
            HideSecondPage(button_SalesForm, button_ServiceForm, button_ReturnToFirstPage, groupBox_Service, groupBox_Sales, groupBox_Transaction);
            groupBox_Customer.Show();
            label_F1Header2.Show();

            if (CreateCustomerQuery == "")
            {
                // clear customer form
            }
            else if (CreateCustomerQuery != "")
            {
                // clear customer details
            }
        }

        //------FUNCTIONS------
        private void SqlConnQuerySalesFinal(String query)
        {
            String ID = "OrderID";
            // Create a connection to the database.
            string connectionString = @"Data Source=DESKTOP-ATDS9TK;Initial Catalog=ADV_DB;Integrated Security=true;MultipleActiveResultSets=true";
            SqlConnection cnn = new SqlConnection(connectionString);

            cnn.Open();

            string selectQuery = "SELECT ItemID, ItemName, Price, Count, TotalPrice FROM TempTable";
            SqlCommand selectCommand = new SqlCommand(selectQuery, cnn);

            SqlDataReader reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                string insertQuery = "INSERT INTO TB_ItemOrders (OrderID, Details, CustomerID, ItemID, Quantity, Price) VALUES (@OrderID, @Details, @CustomerID, @ItemID, @Quantity, @Price)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, cnn);

                String NewQuery = "SELECT OrderID FROM TB_ItemOrders WHERE OrderID = (SELECT max(OrderID) FROM TB_ItemOrders)";
                float OrderID = float.Parse(SqlConnQueryReturn(NewQuery, ID));
                OrderID++;

                insertCommand.Parameters.AddWithValue("@OrderID", OrderID);
                insertCommand.Parameters.AddWithValue("@CustomerID", CustID);
                insertCommand.Parameters.AddWithValue("@Details", reader["ItemName"]);
                insertCommand.Parameters.AddWithValue("@ItemID", reader["ItemID"]);
                insertCommand.Parameters.AddWithValue("@Quantity", reader["Count"]);
                insertCommand.Parameters.AddWithValue("@Price", reader["TotalPrice"]);
                insertCommand.ExecuteNonQuery();
                // selectCommand.Parameters.Clear();
                insertCommand.Parameters.Clear();
            }

            reader.Close();
            cnn.Close();
        }
        // Sql Connection Function for queries
        private void SqlConnQuery(String query)
        {
            string connectionString = @"Data Source=DESKTOP-ATDS9TK;Initial Catalog=ADV_DB;Integrated Security=true";
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Open();
                using (SqlCommand command = new SqlCommand(query, cnn))
                    command.ExecuteNonQuery();
                cnn.Close();
            }
        }
        // Sql Connection for Total Price
        private double SqlConnQueryPrice(String query)
        {
            double sum = 0;
            try
            {
                string connectionString = @"Data Source=DESKTOP-ATDS9TK;Initial Catalog=ADV_DB;Integrated Security=true";
                SqlConnection cnn = new SqlConnection(connectionString);

                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                sum = (double)command.ExecuteScalar();
                cnn.Close();
            }
            catch
            {

            }

            return sum;
        }
        // Sql Connection to get ID
        private String SqlConnQueryReturn(String query, String ID)
        {
            string connectionString = @"Data Source=DESKTOP-ATDS9TK;Initial Catalog=ADV_DB;Integrated Security=true";
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand(query, cnn);
                cmd.Parameters.AddWithValue($"@{ID}", query);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader[ID].ToString();
                    }
                    return null;
                }
            }
        }
        // Sql Connection Function for Data Tables
        private void SqlConnQuery_DataTable(String query, DataGridView dataGridView)
        {
            string connectionString = @"Data Source=DESKTOP-ATDS9TK;Initial Catalog=ADV_DB;Integrated Security=true";
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, cnn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView.DataSource = dt;
                cnn.Close();
            }
        }

        // Function to show second page
        static void ShowSecondPage(Control buttonS, Control buttonSe, Control buttonR, Control groupBoxS, Control groupBoxT)
        {
            buttonS.Show();
            buttonSe.Show();
            buttonR.Show();
            groupBoxS.Show();
            groupBoxT.Show();
        }
        // Function to hide second page
        static void HideSecondPage(Control buttonS, Control buttonSe, Control buttonR, Control groupBoxS, Control groupBoxSe, Control groupBoxT)
        {
            buttonS.Hide();
            buttonSe.Hide();
            buttonR.Hide();
            groupBoxS.Hide();
            groupBoxSe.Hide();
            groupBoxT.Hide();
        }
        // Double to float
        public static float ToSingle(double value)
        {
            return (float)value;
        }
    }
}
