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
                chkActivo.Checked,
                txtUsuario.Text
            );

            if (exito)
            {
                MessageBox.Show("Usuario creado con éxito.");
                CargarUsuarios();
            }
        }
        // Esta es la función que te faltaba en el código anterior
        private void CargarUsuarios(bool soloActivos = true)
        {
            UsuarioDAL dal = new UsuarioDAL();
            dgvUsuarios.DataSource = dal.ListarUsuarios(soloActivos);

            if (dgvUsuarios.Columns.Contains("UsuarioID"))
                dgvUsuarios.Columns["UsuarioID"].Visible = false;
            if (dgvUsuarios.Columns.Contains("RoleID"))
                dgvUsuarios.Columns["RoleID"].Visible = false;
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
                txtUsuario.Text = dgvUsuarios.CurrentRow.Cells["Username"].Value.ToString();

                // Limpiamos el campo de password por seguridad y para indicar que es opcional al editar
                txtPassword.Clear();
            }
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            UsuarioDAL dalUser = new UsuarioDAL();

            // VALIDACIÓN: Nombre de usuario único
            if (dalUser.ExisteUsuario(txtUsuario.Text))
            {
                MessageBox.Show("El nombre de usuario ya existe. Elija otro.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool exito = dalUser.InsertarUsuario(
                txtNombre.Text, txtEmail.Text, txtPassword.Text,
                (int)cmbRoles.SelectedValue, chkActivo.Checked, txtUsuario.Text
            );

            if (exito)
            {
                MessageBox.Show("Usuario creado.");
                CargarUsuarios(!chkVerInactivos.Checked);
                LimpiarCampos();
            }
        }

        // Botón MODIFICAR: Actualiza el registro seleccionado en el Grid
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (idUsuarioSeleccionado == 0) return;

            UsuarioDAL dal = new UsuarioDAL();

            // 1. Validación de nombre de usuario único (excluyendo al actual)
            if (dal.ExisteUsuario(txtUsuario.Text, idUsuarioSeleccionado))
            {
                MessageBox.Show("Este nombre de usuario ya está ocupado.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Ejecutar edición
            // Si txtPassword.Text está vacío, la DAL mantendrá la contraseña anterior
            bool exito = dal.EditarUsuario(
                idUsuarioSeleccionado,
                txtNombre.Text,
                txtEmail.Text,
                txtPassword.Text, // Se envía tal cual (vacío o con texto nuevo)
                (int)cmbRoles.SelectedValue,
                chkActivo.Checked,
                txtUsuario.Text
            );

            if (exito)
            {
                MessageBox.Show("Usuario actualizado con éxito.");
                CargarUsuarios(!chkVerInactivos.Checked);
                LimpiarCampos();
            }
        }

        // Botón ELIMINAR: Ventana emergente e inactivación
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idUsuarioSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un usuario para eliminar.");
                return;
            }

            DialogResult res = MessageBox.Show("¿Está seguro que desea eliminar este usuario? (Se marcará como inactivo)",
                                              "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (res == DialogResult.Yes)
            {
                UsuarioDAL dal = new UsuarioDAL();
                if (dal.InactivarUsuario(idUsuarioSeleccionado))
                {
                    MessageBox.Show("Usuario inactivado con éxito.");
                    CargarUsuarios();
                    LimpiarCampos();
                }
            }
        }

        // Función auxiliar para limpiar la interfaz
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            txtUsuario.Clear();
            cmbRoles.SelectedIndex = -1;
            chkActivo.Checked = true;
            idUsuarioSeleccionado = 0;
        }

        // Evento del CheckBox para filtrar el Grid
        private void chkVerInactivos_CheckedChanged(object sender, EventArgs e)
        {
            // Si chkVerInactivos está marcado, pedimos activos = false
            CargarUsuarios(!chkVerInactivos.Checked);
        }
    }
}
