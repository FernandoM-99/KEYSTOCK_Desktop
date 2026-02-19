using KEYSTOCK_Desktop.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KEYSTOCK_Desktop.Formularios
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            // 1. Llenamos la información del usuario desde la sesión global
            // Usamos la información que guardamos durante el Login
            lblUsuario.Text = $"Sesión: {UserSession.Nombre}";

            // 2. Información del servidor (LAPTOP-ICAMTCRR\Ferna)
            lblServer.Text = "Host: LAPTOP-ICAMTCRR | DB: Sistema_Gestion_Inventarios";

            // 3. Inicializamos la fecha para que no aparezca vacía al inicio
            lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void tmrReloj_Tick(object sender, EventArgs e)
        {
            // Actualiza el reloj cada segundo
            lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x is frmProductos);

            frmProductos hijo = new frmProductos();
            hijo.MdiParent = this; // Se mantiene dentro del contenedor principal
            hijo.Show();
        }

        private void movimientosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verificamos si ya está abierto para no duplicarlo
            Form frm = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x is frmMovimientos);

            if (frm != null)
            {
                frm.BringToFront();
            }
            else
            {
                frmMovimientos hijo = new frmMovimientos();
                hijo.MdiParent = this; // Se establece como hijo del principal
                hijo.Show();
            }
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x is frmProveedores);

            frmProveedores hijo = new frmProveedores();
            hijo.MdiParent = this;
            hijo.Show();
        }

        private void productosProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verificamos si el formulario ya está abierto para no duplicar la ventana
            Form frm = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x is frmVinculoProv);
            frmVinculoProv hijo = new frmVinculoProv();
            hijo.MdiParent = this;
            hijo.Show();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verificamos si el formulario ya está abierto para no duplicar la ventana
            Form frm = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x is frmUsuarios);
            if (UserSession.RoleID == 1) // Suponiendo que 1 es 'Administrador'
            {
                frmUsuarios hijo = new frmUsuarios();
                hijo.MdiParent = this;
                hijo.Show();
            }
            else
            {
                MessageBox.Show("Acceso denegado. Se requieren permisos de administrador.");
            }
        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verificamos si el formulario ya está abierto para no duplicar la ventana
            Form frm = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x is frmRoles);
            // Solo permitimos el acceso si el RoleID guardado en la sesión es 1 (Admin)
            if (UserSession.RoleID == 1)
            {
                frmRoles hijo = new frmRoles();
                hijo.MdiParent = this;
                hijo.Show();
            }
            else
            {
                MessageBox.Show("Acceso restringido. Solo el administrador puede gestionar perfiles.",
                                "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Preguntamos al usuario para evitar cierres accidentales
            DialogResult resultado = MessageBox.Show("¿Está seguro que desea salir de KEYSOTCK Desktop?",
                                                    "Confirmar Salida",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                Application.Exit(); // Cierra todos los formularios y termina el proceso
            }
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Desea cerrar la sesión actual?",
                                            "Cerrar Sesión",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Information);

            if (resultado == DialogResult.Yes)
            {
                // 1. Limpiamos los datos de la sesión global por seguridad
                UserSession.UsuarioID = 0;
                UserSession.Nombre = string.Empty;
                UserSession.RoleID = 0;

                // 2. Reiniciamos la aplicación para volver al punto de entrada (Login)
                // Esto ejecutará de nuevo el bloque Main en Program.cs
                Application.Restart();
            }
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Solo preguntamos si el cierre fue iniciado por el usuario (la X o Alt+F4)
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult res = MessageBox.Show("¿Seguro que desea cerrar el sistema?", "KEYSOTCK",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.No)
                {
                    e.Cancel = true; // Cancela el evento de cierre
                }
            }
        }
    }
}
