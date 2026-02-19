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
    public partial class frmVinculoProv : Form
    {
        ProductosProveedoresDAL dalRelacion = new ProductosProveedoresDAL();
        ProductoDAL dalProd = new ProductoDAL();
        ProveedorDAL dalProv = new ProveedorDAL();

        public frmVinculoProv()
        {
            InitializeComponent();
        }

        private void frmVinculoProv_Load(object sender, EventArgs e)
        {
            // Llenar combos al iniciar
            cmbProductos.DataSource = dalProd.ObtenerListaSimple();
            cmbProductos.DisplayMember = "Nombre";
            cmbProductos.ValueMember = "ProductoID";

            cmbProveedores.DataSource = dalProv.Listar(); // Usamos el método Listar que ya tenías
            cmbProveedores.DisplayMember = "NombreEmpresa";
            cmbProveedores.ValueMember = "ProveedorID";
        }

        private void btnVincular_Click(object sender, EventArgs e)
        {
            if (cmbProductos.SelectedValue == null || cmbProveedores.SelectedValue == null) return;

            int pId = Convert.ToInt32(cmbProductos.SelectedValue);
            int prId = Convert.ToInt32(cmbProveedores.SelectedValue);
            decimal costo = decimal.Parse(txtCosto.Text);
            string skuProv = txtSKUProveedor.Text;

            if (dalRelacion.Vincular(pId, prId, costo, skuProv))
            {
                MessageBox.Show("Vínculo establecido con éxito.");
                CargarGrid(pId);
            }
        }

        private void cmbProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProductos.SelectedValue is int pId)
            {
                CargarGrid(pId);
            }
        }

        private void CargarGrid(int id)
        {
            dgvVinculos.DataSource = dalRelacion.ListarPorProducto(id);
        }

    private void txtCosto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }   
        }

        private void cmbProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
