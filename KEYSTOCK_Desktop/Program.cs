using KEYSTOCK_Desktop.Formularios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KEYSTOCK_Desktop
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmLogin login = new frmLogin();
            // Mostramos el login como un diálogo
            if (login.ShowDialog() == DialogResult.OK)
            {
                // Si el login fue exitoso, lanzamos el Dashboard como el proceso principal
                Application.Run(new frmPrincipal());
            }
        }
    }
}
