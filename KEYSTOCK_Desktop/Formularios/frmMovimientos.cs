using KEYSTOCK_Desktop.CapaDatos;
using KEYSTOCK_Desktop.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KEYSTOCK_Desktop.Formularios
{
    public partial class frmMovimientos : Form
    {
        // Instancia global de la capa de datos
        MovimientoDAL dal = new MovimientoDAL();

        public frmMovimientos()
        {
            InitializeComponent();
        }

        // --- MÉTODOS QUE FALTABAN ---

        private int ObtenerStockActual(int id)
        {
            return dal.ObtenerStockActual(id);
        }

        private void CargarHistorial()
        {
            // dgvMovimientos es el nombre de tu DataGridView en este formulario
            dgvMovimientos.DataSource = dal.ListarMovimientos();
        }

        // ----------------------------

        private void frmMovimientos_Load(object sender, EventArgs e)
        {
            CargarHistorial();
            LlenarComboProductos();
            LlenarComboProveedores();
        }


        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            int idProd = Convert.ToInt32(cmbProductos.SelectedValue);
            string tipo = cmbTipo.SelectedItem.ToString();
            int cant = Convert.ToInt32(txtCantidad.Text);

            // Obtenemos el ID del proveedor si hay uno seleccionado
            int? idProv = null;
            if (cmbProveedores.SelectedValue != null)
                idProv = Convert.ToInt32(cmbProveedores.SelectedValue);

            // Validación exigida por tu Trigger de base de datos
            if (tipo == "Entrada" && idProv == null)
            {
                MessageBox.Show("Para una Entrada, debe seleccionar un proveedor.");
                return;
            }

            try
            {
                if (dal.RegistrarMovimiento(idProd, UserSession.UsuarioID, idProv, tipo, cant, txtReferencia.Text))
                {
                    MessageBox.Show("Movimiento registrado con éxito.");
                    CargarHistorial();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en transacción: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dgvMovimientos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmMovimientos_Load_1(object sender, EventArgs e)
        {
            CargarHistorial(); // Refresca el Grid
            LlenarComboProductos();
        }
        private void LlenarComboProductos()
        {
            ProductoDAL prodDal = new ProductoDAL();
            DataTable dt = prodDal.ObtenerListaSimple();

            cmbProductos.DataSource = dt;
            cmbProductos.DisplayMember = "Nombre";    // Lo que el usuario ve
            cmbProductos.ValueMember = "ProductoID";  // El valor real que guardaremos

            // Opcional: Que el combo empiece vacío
            cmbProductos.SelectedIndex = -1;
        }
        private void LlenarComboProveedores()
        {
            // Usamos la capa de datos que ya tienes creada
            ProveedorDAL provDal = new ProveedorDAL();
            DataTable dt = provDal.Listar(); // Este método ya lo creamos antes

            if (dt != null && dt.Rows.Count > 0)
            {
                cmbProveedores.DataSource = dt;

                // IMPORTANTE: Estos nombres deben coincidir EXACTAMENTE 
                // con las columnas de tu tabla en SQL Server
                cmbProveedores.DisplayMember = "NombreEmpresa";
                cmbProveedores.ValueMember = "ProveedorID";

                cmbProveedores.SelectedIndex = -1; // Empieza vacío
            }
        }

            private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}