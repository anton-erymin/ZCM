using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace ZCM
{
    static class Program
    {

        private static bool mQuit;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 f1 = new Form1();
            f1.FormClosed += QuitLoop;
            f1.Show();

            do {
                Application.DoEvents();

                f1.Solve(0.05);


                f1.DrawAxis();
                f1.Draw();

                


                System.Threading.Thread.Sleep(1);
            } while (!mQuit);
        }


        private static void QuitLoop(object sender, FormClosedEventArgs e)
        {
            mQuit = true;
        }
        
    }
}
