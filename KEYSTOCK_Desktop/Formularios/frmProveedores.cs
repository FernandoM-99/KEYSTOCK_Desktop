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
    public partial class frmProveedores : Form
    {
        private int idProveedorSeleccionado = 0;
        ProveedorDAL dal = new ProveedorDAL();
        public frmProveedores()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmProveedores_Load(object sender, EventArgs e)
        {
            CargarGrid();
            btnEditar.Enabled = false;
        }
        private void CargarGrid()
        {
            // Listar todos los proveedores
            dgvProveedores.DataSource = dal.Listar();
            if (dgvProveedores.Columns.Contains("ProveedorID"))
                dgvProveedores.Columns["ProveedorID"].Visible = false;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidacionHelper.FormularioEsValido(this.Controls))
            {
                return; // Si NO es válido, detenemos la ejecución
            }

            if (dal.Insertar(txtEmpresa.Text, txtContacto.Text, txtEmail.Text, txtTelefono.Text))
            {
                MessageBox.Show("Proveedor registrado.");
                CargarGrid();
                Limpiar();
            }
        }
        private void Limpiar()
        {
            idProveedorSeleccionado = 0;
            txtEmpresa.Clear();
            txtContacto.Clear();
            txtEmail.Clear();
            txtTelefono.Clear();
            chkActivo.Checked = true;
        }

        private void dgvProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEditar.Enabled = true;
            if (e.RowIndex >= 0)
            {
                idProveedorSeleccionado = Convert.ToInt32(dgvProveedores.CurrentRow.Cells["ProveedorID"].Value);
                txtEmpresa.Text = dgvProveedores.CurrentRow.Cells["NombreEmpresa"].Value.ToString();
                txtContacto.Text = dgvProveedores.CurrentRow.Cells["NombreContacto"].Value.ToString();
                txtEmail.Text = dgvProveedores.CurrentRow.Cells["Email"].Value.ToString();
                txtTelefono.Text = dgvProveedores.CurrentRow.Cells["Telefono"].Value.ToString();
                //chkActivo.Checked = Convert.ToBoolean(dgvProveedores.CurrentRow.Cells["Activo"].Value);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idProveedorSeleccionado == 0) return;

            if (!ValidacionHelper.FormularioEsValido(this.Controls))
            {
                return; // Si NO es válido, detenemos la ejecución
            }

            if (dal.Editar(idProveedorSeleccionado, txtEmpresa.Text, txtContacto.Text, txtEmail.Text, txtTelefono.Text, chkActivo.Checked))
            {
                MessageBox.Show("Datos actualizados.");
                CargarGrid();
                Limpiar();
            }
            btnEditar.Enabled = false;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idProveedorSeleccionado == 0) return;

            DialogResult res = MessageBox.Show("¿Inactivar proveedor?", "Confirmar", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                if (dal.Inactivar(idProveedorSeleccionado))
                {
                    CargarGrid();
                    Limpiar();
                }
            }
        }

    }
}
