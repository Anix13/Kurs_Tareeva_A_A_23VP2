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

            // Заменить запуск основной формы на SplashForm
            Application.Run(new FormStartcs());
        }
    }
}
