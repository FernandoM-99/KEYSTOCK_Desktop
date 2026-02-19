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
    public partial class frmUsuarios : Form
    {
        public frmUsuarios()
        {
            InitializeComponent();
        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {
            LlenarComboRoles();
            CargarUsuarios();
        }

        private void LlenarComboRoles()
        {
            RolesDAL dalRoles = new RolesDAL();
            cmbRoles.DataSource = dalRoles.ListarRoles();
            cmbRoles.DisplayMember = "NombreRol";
            cmbRoles.ValueMember = "RoleID";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            UsuarioDAL dalUser = new UsuarioDAL();
            bool exito = dalUser.InsertarUsuario(
                txtNombre.Text,
                txtEmail.Text,
                txtPassword.Text,
                (int)cmbRoles.SelectedValue,
                chkActivo.Checked
            );

            if (exito)
            {
                MessageBox.Show("Usuario creado con éxito.");
                CargarUsuarios();
            }
        }
        // Esta es la función que te faltaba en el código anterior
        private void CargarUsuarios()
        {
            UsuarioDAL dal = new UsuarioDAL();
            // dgvUsuarios debe ser el nombre de tu DataGridView en el diseño
            dgvUsuarios.DataSource = dal.ListarUsuarios();

            // Opcional: Ajustar visualmente las columnas
            if (dgvUsuarios.Columns.Contains("UsuarioID"))
                dgvUsuarios.Columns["UsuarioID"].Visible = false; // Ocultamos el ID interno
        }

        private int idUsuarioSeleccionado = 0;

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idUsuarioSeleccionado = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["UsuarioID"].Value);
                txtNombre.Text = dgvUsuarios.CurrentRow.Cells["NombreCompleto"].Value.ToString();
                txtEmail.Text = dgvUsuarios.CurrentRow.Cells["Email"].Value.ToString();
                cmbRoles.Text = dgvUsuarios.CurrentRow.Cells["NombreRol"].Value.ToString();
                chkActivo.Checked = Convert.ToBoolean(dgvUsuarios.CurrentRow.Cells["Activo"].Value);
            }
        }
    }
}
