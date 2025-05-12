using Kurs_Tareeva_A_A_23VP2;
using System;
using System.Windows.Forms;

namespace Kursov_Tareeva23VP2
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Показываем стартовую форму как диалог
            using (FormStartcs startForm = new FormStartcs())
            {
                if (startForm.ShowDialog() == DialogResult.OK)
                {
                    // Если нажата кнопка "Начать" — открыть основную форму
                    Application.Run(new Form1());
                }
            }
        }
    }
}
