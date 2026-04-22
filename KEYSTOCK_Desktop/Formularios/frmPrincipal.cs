using KEYSTOCK_Desktop.CapaDatos;
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
            try 
            {
                // 1. Información del usuario desde la sesión
                lblUsuario.Text = $"Sesión: {UserSession.Nombre}";

                // 2. Obtener Nombre del Host (Equipo local) dinámicamente
                string nombreHost = Environment.MachineName;

                // 3. Obtener Nombre de la Base de Datos desde la cadena de conexión
                string nombreDB = "Desconocida";
                using (var conn = new Conexion().LeerConexion())
                {
                    // Database es una propiedad de SqlConnection que devuelve el Initial Catalog
                    nombreDB = conn.Database;
                }

                // 4. Asignación dinámica al label
                lblServer.Text = $"Host: {nombreHost} | DB: {nombreDB}";

                // 5. Inicializar fecha
                lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error crítico al inicializar Dashboard: " + ex.Message, 
                                "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            LlenarComboProductos();
            ConfigurarColumnasCarrito();
        }

        private void LlenarComboProductos()
        {
            ProductoDAL prodDal = new ProductoDAL();
            DataTable dt = prodDal.ObtenerListaSimple();

            if (dt != null && dt.Rows.Count > 0)
            {
                // Limpiar antes de asignar
                cmbProductos.DataSource = null;

                // Configurar mapeo
                cmbProductos.DisplayMember = "Nombre";
                cmbProductos.ValueMember = "ProductoID";

                // Asignar fuente de datos
                cmbProductos.DataSource = dt;
                cmbProductos.SelectedIndex = -1;
            }
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

        private void btnFinalizarVenta_Click(object sender, EventArgs e)
        {
            MovimientoDAL movDal = new MovimientoDAL();

            foreach (DataGridViewRow fila in dgvCarrito.Rows)
            {
                if (fila.Cells["ProductoID"].Value != null)
                {
                    int id = Convert.ToInt32(fila.Cells["ProductoID"].Value);
                    int cant = Convert.ToInt32(fila.Cells["Cantidad"].Value);
                    decimal precio = Convert.ToDecimal(fila.Cells["Precio"].Value);

                    // Se registra como Salida y con Referencia "VENTA POS"
                    movDal.RegistrarMovimiento(id, UserSession.UsuarioID, null, "Salida", cant, "VENTA POS", precio);
                }
            }

            dgvCarrito.Rows.Clear();
            MessageBox.Show("Venta registrada con éxito.");
        }

        private void puntoDeVentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LlenarComboProductos();

            if (pnlPOS.Visible == true)
            {
                pnlPOS.Visible = false;
            }
            else 
            {
                pnlPOS.Visible = true;
                //pnlPOS.BringToFront();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // 1. Validaciones básicas de campos vacíos
            if (cmbProductos.SelectedValue == null || string.IsNullOrWhiteSpace(txtCantidad.Text))
            {
                MessageBox.Show("Seleccione un producto y asigne una cantidad.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Obtener datos de los controles y validar formato numérico
            int idProd = Convert.ToInt32(cmbProductos.SelectedValue);
            string nombre = cmbProductos.Text;
            int cantAgregada;

            if (!int.TryParse(txtCantidad.Text, out cantAgregada) || cantAgregada <= 0)
            {
                MessageBox.Show("Por favor, ingrese una cantidad válida mayor a cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCantidad.Focus();
                return;
            }

            decimal precio;
            if (!decimal.TryParse(txtPrecio.Text, out precio) || precio < 0)
            {
                MessageBox.Show("Por favor, ingrese un precio válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return;
            }

            // =========================================================
            // 3. NUEVA VALIDACIÓN DE STOCK
            // =========================================================
            MovimientoDAL movDal = new MovimientoDAL();
            int stockDisponible = movDal.ObtenerStockActual(idProd); // Consultamos el stock real en SQL

            // Revisar si este producto ya fue agregado al carrito previamente para sumar su cantidad
            int cantidadYaEnCarrito = 0;
            foreach (DataGridViewRow fila in dgvCarrito.Rows)
            {
                if (fila.Cells["ProductoID"].Value != null && Convert.ToInt32(fila.Cells["ProductoID"].Value) == idProd)
                {
                    cantidadYaEnCarrito += Convert.ToInt32(fila.Cells["Cantidad"].Value);
                }
            }

            // Validar: (Lo que quiere agregar + Lo que ya está en el carrito) no debe superar el stock
            if ((cantAgregada + cantidadYaEnCarrito) > stockDisponible)
            {
                MessageBox.Show($"No hay stock suficiente para '{nombre}'.\n\n" +
                                $"Stock en inventario: {stockDisponible} unidades.\n" +
                                $"Cantidad en carrito: {cantidadYaEnCarrito} unidades.",
                                "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCantidad.Focus();
                return; // Detenemos el proceso para que no se agregue a la tabla
            }
            // =========================================================

            // 4. Calcular subtotal y agregar la fila si pasó la validación
            decimal subtotal = cantAgregada * precio;
            dgvCarrito.Rows.Add(idProd, nombre, cantAgregada, precio, subtotal);

            // 5. Limpiar campos para la siguiente entrada
            txtCantidad.Clear();
            txtPrecio.Clear();
            cmbProductos.SelectedIndex = -1;

            // (Opcional) Regresar el foco al combobox para seguir escaneando o buscando productos
            cmbProductos.Focus();
        }
        private void ConfigurarColumnasCarrito()
        {
            // Limpiamos cualquier residuo previo
            dgvCarrito.Columns.Clear();

            // Agregamos las columnas necesarias para la venta
            dgvCarrito.Columns.Add("ProductoID", "ID");
            dgvCarrito.Columns.Add("Nombre", "Producto");
            dgvCarrito.Columns.Add("Cantidad", "Cant.");
            dgvCarrito.Columns.Add("Precio", "Precio Unit.");
            dgvCarrito.Columns.Add("Subtotal", "Subtotal");

            // Opcional: Hacer que la columna ID no sea visible para el usuario
            dgvCarrito.Columns["ProductoID"].Visible = false;

            // Ajustar el diseño
            dgvCarrito.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
