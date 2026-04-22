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
            LlenarComboProveedores();

            btnActualizar.Enabled = false;
        }
        private void RefrescarGrid()
        {
            dgvProductos.DataSource = objetoDAL.Listar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // 1. Validar que los campos obligatorios no estén vacíos
            if (!ValidacionHelper.FormularioEsValido(this.Controls))
            {
                return;
            }

            ProductoDAL objetoDAL = new ProductoDAL();
            ProductosProveedoresDAL vinculoDAL = new ProductosProveedoresDAL();

            // 2. Validar que el SKU no esté duplicado
            if (objetoDAL.ExisteSKU(txtSKU.Text))
            {
                MessageBox.Show("El SKU ingresado ya existe en otro producto. Por favor, asigne un SKU diferente.",
                                "Validación de SKU", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSKU.Focus();
                return;
            }

            // 3. Validar el formato del precio
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("Ingrese un precio válido (Ej: 150.50).");
                txtPrecio.Focus();
                return;
            }

            // 4. Ejecutar inserción y obtener el ID del nuevo producto
            int nuevoProductoID = objetoDAL.Insertar(
                txtSKU.Text,
                txtNombre.Text,
                txtDescripcion.Text,
                Convert.ToInt32(txtStock.Text),
                precio
            );

            if (nuevoProductoID > 0)
            {
                // 5. Vincular con el proveedor si se seleccionó uno en el ComboBox
                if (cmbProveedores.SelectedValue != null)
                {
                    int proveedorID = (int)cmbProveedores.SelectedValue;
                    // Se vincula con el costo inicial y el SKU del producto
                    vinculoDAL.Vincular(nuevoProductoID, proveedorID, precio, txtSKU.Text);
                }

                MessageBox.Show("Producto registrado y vinculado correctamente.");
                RefrescarGrid();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Ocurrió un error al intentar registrar el producto.");
            }
        }

        private void LimpiarCampos()
        {
            txtSKU.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
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
                // 1. Obtener el ID seleccionado
                idSeleccionado = Convert.ToInt32(dgvProductos.CurrentRow.Cells["ProductoID"].Value);

                // 2. Cargar los datos básicos
                txtSKU.Text = dgvProductos.CurrentRow.Cells["SKU"].Value.ToString();
                txtNombre.Text = dgvProductos.CurrentRow.Cells["Nombre"].Value.ToString();
                txtDescripcion.Text = dgvProductos.CurrentRow.Cells["Descripcion"].Value.ToString();
                txtStock.Text = dgvProductos.CurrentRow.Cells["StockActual"].Value.ToString();

                // 3. Cargar el Precio Unitario
                txtPrecio.Text = dgvProductos.CurrentRow.Cells["PrecioUnitario"].Value.ToString();

                // 4. Cargar el Proveedor en el ComboBox
                ProductosProveedoresDAL vinculoDAL = new ProductosProveedoresDAL();
                int proveedorID = vinculoDAL.ObtenerProveedorDeProducto(idSeleccionado);

                if (proveedorID > 0)
                {
                    cmbProveedores.SelectedValue = proveedorID;
                }
                else
                {
                    cmbProveedores.SelectedIndex = -1; // Lo deja en blanco si no tiene proveedor asignado
                }

                btnActualizar.Enabled = true; // Habilitamos el botón de edición
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            // 1. Validar campos vacíos
            if (!ValidacionHelper.FormularioEsValido(this.Controls))
            {
                return;
            }

            if (idSeleccionado == 0) return;

            ProductoDAL objetoDAL = new ProductoDAL();
            ProductosProveedoresDAL vinculoDAL = new ProductosProveedoresDAL();

            // 2. Validar que el SKU no esté duplicado en OTRO producto distinto al actual
            if (objetoDAL.ExisteSKU(txtSKU.Text, idSeleccionado))
            {
                MessageBox.Show("El SKU ingresado ya está asignado a otro producto. Por favor, asigne un SKU diferente.",
                                "Validación de SKU", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSKU.Focus();
                return;
            }

            // 3. Validar el formato del precio
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("Ingrese un precio válido (Ej: 150.50).");
                txtPrecio.Focus();
                return;
            }

            // 4. Ejecutar la edición del producto
            bool exito = objetoDAL.Editar(
                idSeleccionado,
                txtSKU.Text,
                txtNombre.Text,
                txtDescripcion.Text,
                Convert.ToInt32(txtStock.Text),
                precio
            );

            if (exito)
            {
                // 5. Actualizar el vínculo con el proveedor si hay uno seleccionado
                if (cmbProveedores.SelectedValue != null)
                {
                    int proveedorID = (int)cmbProveedores.SelectedValue;
                    vinculoDAL.ActualizarVinculo(idSeleccionado, proveedorID, precio, txtSKU.Text);
                }

                MessageBox.Show("Producto actualizado correctamente.");
                // RefrescarGrid();
                // LimpiarCampos();
                btnActualizar.Enabled = false;
                idSeleccionado = 0; // Limpiar ID en memoria
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

        // Método genérico para validar que no haya campos vacíos en el formulario
        private bool ValidarCamposRequeridos(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                // Validar TextBoxes
                if (control is TextBox txt && string.IsNullOrWhiteSpace(txt.Text))
                {
                    MessageBox.Show($"El campo {txt.Name.Replace("txt", "")} no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt.Focus();
                    return false;
                }

                // Validar ComboBoxes (Que tengan un elemento seleccionado)
                if (control is ComboBox cmb && cmb.SelectedIndex == -1)
                {
                    MessageBox.Show($"Debe seleccionar una opción en {cmb.Name.Replace("cmb", "")}.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmb.Focus();
                    return false;
                }

                // Si hay paneles o GroupBoxes, aplicamos recursividad para revisar sus controles hijos
                if (control.HasChildren)
                {
                    if (!ValidarCamposRequeridos(control.Controls))
                        return false;
                }
            }
            return true; // Todo está correcto
        }

        private void LlenarComboProveedores()
        {
            ProveedorDAL dalProv = new ProveedorDAL();
            cmbProveedores.DataSource = dalProv.Listar(); // Asumiendo que retorna DataTable
            cmbProveedores.DisplayMember = "NombreEmpresa";
            cmbProveedores.ValueMember = "ProveedorID";
            cmbProveedores.SelectedIndex = -1;
        }
    }
}
