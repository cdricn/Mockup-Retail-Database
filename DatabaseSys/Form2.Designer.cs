
namespace DatabaseSys
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_LoginHeader = new System.Windows.Forms.Label();
            this.button_Login = new System.Windows.Forms.Button();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.textBox_Username = new System.Windows.Forms.TextBox();
            this.label_Password = new System.Windows.Forms.Label();
            this.label_Username = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_LoginHeader
            // 
            this.label_LoginHeader.AutoSize = true;
            this.label_LoginHeader.Font = new System.Drawing.Font("Arial", 18F);
            this.label_LoginHeader.Location = new System.Drawing.Point(29, 22);
            this.label_LoginHeader.Name = "label_LoginHeader";
            this.label_LoginHeader.Size = new System.Drawing.Size(206, 27);
            this.label_LoginHeader.TabIndex = 14;
            this.label_LoginHeader.Text = "Employee System";
            // 
            // button_Login
            // 
            this.button_Login.Font = new System.Drawing.Font("Arial", 12F);
            this.button_Login.Location = new System.Drawing.Point(67, 299);
            this.button_Login.Name = "button_Login";
            this.button_Login.Size = new System.Drawing.Size(118, 39);
            this.button_Login.TabIndex = 13;
            this.button_Login.Text = "Login";
            this.button_Login.UseVisualStyleBackColor = true;
            this.button_Login.Click += new System.EventHandler(this.button_Login_Click);
            // 
            // textBox_Password
            // 
            this.textBox_Password.Location = new System.Drawing.Point(109, 248);
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.Size = new System.Drawing.Size(126, 20);
            this.textBox_Password.TabIndex = 12;
            // 
            // textBox_Username
            // 
            this.textBox_Username.Location = new System.Drawing.Point(109, 209);
            this.textBox_Username.Name = "textBox_Username";
            this.textBox_Username.Size = new System.Drawing.Size(126, 20);
            this.textBox_Username.TabIndex = 11;
            // 
            // label_Password
            // 
            this.label_Password.AutoSize = true;
            this.label_Password.Font = new System.Drawing.Font("Arial", 12F);
            this.label_Password.Location = new System.Drawing.Point(23, 248);
            this.label_Password.Name = "label_Password";
            this.label_Password.Size = new System.Drawing.Size(78, 18);
            this.label_Password.TabIndex = 10;
            this.label_Password.Text = "Password";
            // 
            // label_Username
            // 
            this.label_Username.AutoSize = true;
            this.label_Username.Font = new System.Drawing.Font("Arial", 12F);
            this.label_Username.Location = new System.Drawing.Point(23, 209);
            this.label_Username.Name = "label_Username";
            this.label_Username.Size = new System.Drawing.Size(80, 18);
            this.label_Username.TabIndex = 9;
            this.label_Username.Text = "Username";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 357);
            this.Controls.Add(this.label_LoginHeader);
            this.Controls.Add(this.button_Login);
            this.Controls.Add(this.textBox_Password);
            this.Controls.Add(this.textBox_Username);
            this.Controls.Add(this.label_Password);
            this.Controls.Add(this.label_Username);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_LoginHeader;
        private System.Windows.Forms.Button button_Login;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.TextBox textBox_Username;
        private System.Windows.Forms.Label label_Password;
        private System.Windows.Forms.Label label_Username;
    }
}