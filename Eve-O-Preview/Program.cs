using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PreviewToy
{
    static class Program
    {
        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            Application.Run(new PreviewToyHandler());
        }
    }
}