namespace Storage_Viewer___Optimizer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DiskList = new ListView();
            btnOpti = new Button();
            SuspendLayout();
            // 
            // DiskList
            // 
            DiskList.Location = new Point(6, 3);
            DiskList.Name = "DiskList";
            DiskList.Size = new Size(299, 83);
            DiskList.TabIndex = 0;
            DiskList.UseCompatibleStateImageBehavior = false;
            // 
            // btnOpti
            // 
            btnOpti.Location = new Point(6, 92);
            btnOpti.Name = "btnOpti";
            btnOpti.Size = new Size(299, 28);
            btnOpti.TabIndex = 1;
            btnOpti.Text = "Opti";
            btnOpti.UseVisualStyleBackColor = true;
            btnOpti.Click += btnOpti_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(310, 127);
            Controls.Add(btnOpti);
            Controls.Add(DiskList);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private ListView DiskList;
        private Button btnOpti;
    }
}
