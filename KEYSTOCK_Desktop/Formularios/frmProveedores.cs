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
        }
        private void CargarGrid()
        {
            dgvProveedores.DataSource = dal.Listar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmpresa.Text))
            {
                MessageBox.Show("El nombre de la empresa es obligatorio.");
                return;
            }

            if (dal.Insertar(txtEmpresa.Text, txtContacto.Text, txtEmail.Text, txtTelefono.Text))
            {
                MessageBox.Show("Proveedor registrado con éxito.");
                CargarGrid();
                Limpiar();
            }
        }
        private void Limpiar()
        {
            txtEmpresa.Clear();
            txtContacto.Clear();
            txtEmail.Clear();
            txtTelefono.Clear();
        }
    }
}
