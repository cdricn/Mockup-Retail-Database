using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseSys
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void button_ProceedCForm_Click(object sender, EventArgs e)
        {
            // Hide first page on click
            groupBox_Customer.Hide();
            label_F1Header2.Hide();

            // Show second page on click
            ShowSecondPage(button_SalesForm, button_ServiceForm, button_ReturnToFirstPage, groupBox_Sales, groupBox_Transaction);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Hide second page on load
            HideSecondPage(button_SalesForm, button_ServiceForm, button_ReturnToFirstPage, groupBox_Service, groupBox_Sales, groupBox_Transaction);

            // Avoid group box overlap for sales and service form
            groupBox_Customer.Location = new System.Drawing.Point(18, 90);
            groupBox_Service.Location = new System.Drawing.Point(18, 90);
            groupBox_Sales.Location = new System.Drawing.Point(18, 90);
        }

        private void button_SalesForm_Click(object sender, EventArgs e)
        {
            groupBox_Sales.Show();
            groupBox_Service.Hide();
        }
        private void button_ServiceForm_Click(object sender, EventArgs e)
        {
            groupBox_Service.Show();
            groupBox_Sales.Hide();
        }
        private void button_ReturnToFirstPage_Click(object sender, EventArgs e)
        {
            HideSecondPage(button_SalesForm, button_ServiceForm, button_ReturnToFirstPage, groupBox_Service, groupBox_Sales, groupBox_Transaction);
            groupBox_Customer.Show();
            label_F1Header2.Show();
        }



        //------FUNCTIONS-------

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

    }
}
