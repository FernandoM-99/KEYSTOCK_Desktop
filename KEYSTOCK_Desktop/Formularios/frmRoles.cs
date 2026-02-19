using KEYSTOCK_Desktop.CapaDatos;
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
    public partial class frmRoles : Form
    {
        RolesDAL dal = new RolesDAL();
        private int idRolSeleccionado = 0;
        public frmRoles()
        {
            InitializeComponent();
        }

        private void txtNombreRol_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmRoles_Load(object sender, EventArgs e)
        {
            CargarGrid();
        }
        private void CargarGrid()
        {
            dgvRoles.DataSource = dal.ListarRoles();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreRol.Text)) return;

            if (dal.Insertar(txtNombreRol.Text.Trim()))
            {
                MessageBox.Show("Rol creado exitosamente.");
                CargarGrid();
                txtNombreRol.Clear();
            }
        }

        private void dgvRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idRolSeleccionado = Convert.ToInt32(dgvRoles.CurrentRow.Cells["RoleID"].Value);
                txtNombreRol.Text = dgvRoles.CurrentRow.Cells["NombreRol"].Value.ToString();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idRolSeleccionado == 0) return;

            if (dal.Editar(idRolSeleccionado, txtNombreRol.Text.Trim()))
            {
                MessageBox.Show("Rol actualizado.");
                CargarGrid();
                idRolSeleccionado = 0;
                txtNombreRol.Clear();
            }
        }
        private void Limpiar()
        {
            // Limpiamos el cuadro de texto del nombre del rol
            txtNombreRol.Clear();

            // Reseteamos el ID seleccionado a 0 para indicar que no hay edición activa
            idRolSeleccionado = 0;

            // Ponemos el foco en el TextBox para agilizar la entrada de datos
            txtNombreRol.Focus();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
    }
}
