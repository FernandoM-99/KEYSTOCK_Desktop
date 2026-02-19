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
    public partial class frmProductos : Form
    {
        ProductoDAL objetoDAL = new ProductoDAL();
        public frmProductos()
        {
            InitializeComponent();
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            RefrescarGrid();
        }
        private void RefrescarGrid()
        {
            dgvProductos.DataSource = objetoDAL.Listar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validación básica
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.");
                return;
            }

            bool exito = objetoDAL.Insertar(
                txtSKU.Text,
                txtNombre.Text,
                txtDescripcion.Text,
                Convert.ToInt32(txtStock.Text)
            );

            if (exito)
            {
                MessageBox.Show("Producto registrado correctamente.");
                RefrescarGrid();
                LimpiarCampos();
            }
        }

        private void LimpiarCampos()
        {
            txtSKU.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtStock.Text = "0";
        }

        private void txtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private int idSeleccionado = 0;
        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idSeleccionado = Convert.ToInt32(dgvProductos.CurrentRow.Cells["ProductoID"].Value);
                txtSKU.Text = dgvProductos.CurrentRow.Cells["SKU"].Value.ToString();
                txtNombre.Text = dgvProductos.CurrentRow.Cells["Nombre"].Value.ToString();
                txtDescripcion.Text = dgvProductos.CurrentRow.Cells["Descripcion"].Value.ToString();
                txtStock.Text = dgvProductos.CurrentRow.Cells["StockActual"].Value.ToString();
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0) return;

            if (objetoDAL.Editar(idSeleccionado, txtSKU.Text, txtNombre.Text, txtDescripcion.Text, Convert.ToInt32(txtStock.Text)))
            {
                MessageBox.Show("Producto actualizado.");
                RefrescarGrid();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0) return;

            DialogResult resp = MessageBox.Show("¿Seguro que desea eliminar?", "Confirmar", MessageBoxButtons.YesNo);
            if (resp == DialogResult.Yes)
            {
                if (objetoDAL.Eliminar(idSeleccionado))
                {
                    RefrescarGrid();
                    LimpiarCampos();
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}
