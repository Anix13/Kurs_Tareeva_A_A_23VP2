using Kurs_Tareeva_A_A_23VP2;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kursov_Tareeva23VP2
{
    public partial class FormStartcs : Form
    {
        public FormStartcs()
        {
            InitializeComponent1();
        }

        private void InitializeComponent1()
        {
            this.labelInfo = new Label();
            this.btnStart = new Button();

            // Настройка Label с информацией
            this.labelInfo.Text = "Курсовая работа: Реестр курсов программирования\n" +
                                   "Выполнила студентка Тареева Анастасия\n" +
                                   "Группа 23ВП2";
            this.labelInfo.Location = new Point(55, 10);  // Позиция Label
            this.labelInfo.Size = new Size(400, 130);        // Размер Label
            this.labelInfo.TextAlign = ContentAlignment.MiddleCenter;
            this.labelInfo.Font = new Font(this.labelInfo.Font.FontFamily, 14); // Увеличиваем шрифт на labelInfo

            // Настройка кнопки "Начать"
            this.btnStart.Text = "Начать";
            this.btnStart.Location = new Point(160, 150);  // Позиция кнопки
            this.btnStart.Size = new Size(200, 40);  // Размер кнопки
            this.btnStart.Font = new Font(this.btnStart.Font.FontFamily, 16); // Увеличиваем шрифт на кнопке
            this.btnStart.Click += new EventHandler(this.btnStart_Click);

            // Настройка формы
            this.ClientSize = new Size(500, 250);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.btnStart);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Стартовая форма";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private Label labelInfo;
        private Button btnStart;

        private void labelInfo_Click(object sender, EventArgs e)
        {

        }
    }
}
