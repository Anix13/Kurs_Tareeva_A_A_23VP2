namespace Kursov_Tareeva23VP2
{
    partial class FormStartcs
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        // Убедитесь, что здесь нет переопределения метода Dispose
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStartcs));
            this.btnStart = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(47, 120);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(355, 40);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Начать";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // labelInfo
            // 
            this.labelInfo.Location = new System.Drawing.Point(27, 37);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(400, 60);
            this.labelInfo.TabIndex = 0;
            this.labelInfo.Text = "Курсовая работа: Реестр курсов программирования\nВыполнила студентка Тареева Анаст" +
    "асия\nГруппа 23ВП2";
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelInfo.Click += new System.EventHandler(this.labelInfo_Click);
            // 
            // FormStartcs
            // 
            this.ClientSize = new System.Drawing.Size(464, 200);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.btnStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormStartcs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Стартовая форма";
            this.ResumeLayout(false);

        }

        #endregion

    }
}