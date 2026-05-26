using KEYSTOCK_Desktop.CapaDatos;
using KEYSTOCK_Desktop.Modelos;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing;

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
            // Usamos Listar() en lugar de ObtenerListaSimple para tener todas las columnas
            DataTable dt = prodDal.Listar();

            if (dt != null && dt.Rows.Count > 0)
            {
                cmbProductos.DataSource = null;
                cmbProductos.DisplayMember = "Nombre";
                cmbProductos.ValueMember = "ProductoID";
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
            if (dgvCarrito.Rows.Count == 0)
            {
                MessageBox.Show("El carrito está vacío.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MovimientoDAL movDal = new MovimientoDAL();

            foreach (DataGridViewRow fila in dgvCarrito.Rows)
            {
                if (fila.Cells["ProductoID"].Value != null)
                {
                    int id = Convert.ToInt32(fila.Cells["ProductoID"].Value);
                    int cant = Convert.ToInt32(fila.Cells["Cantidad"].Value);
                    decimal precio = Convert.ToDecimal(fila.Cells["Precio"].Value);

                    movDal.RegistrarMovimiento(id, UserSession.UsuarioID, null, "Salida", cant, "VENTA POS", precio);
                }
            }

            // MANDAR A IMPRIMIR
            ImprimirTicket();

            // Limpiar carrito y reiniciar total
            dgvCarrito.Rows.Clear();
            CalcularTotal();
            MessageBox.Show("Venta registrada y ticket enviado a la impresora.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // =======================================================
        // LÓGICA DE IMPRESIÓN DEL TICKET
        // =======================================================
        private void ImprimirTicket()
        {
            PrintDocument pd = new PrintDocument();
            // Nos suscribimos al evento que dibuja el contenido de la página
            pd.PrintPage += new PrintPageEventHandler(DibujarTicket);

            try
            {
                // pd.Print() envía directo a la impresora predeterminada de Windows.
                // Si no hay impresora física, suele abrir "Guardar como PDF".
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar imprimir: " + ex.Message, "Error de Impresión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DibujarTicket(object sender, PrintPageEventArgs e)
        {
            Graphics graficos = e.Graphics;
            Font fuenteNormal = new Font("Courier New", 10);
            Font fuenteNegrita = new Font("Courier New", 12, FontStyle.Bold);

            int startX = 10;
            int startY = 10;
            int offset = 40;

            // Encabezado
            graficos.DrawString("  KEYSTOCK DESKTOP  ", fuenteNegrita, Brushes.Black, startX + 50, startY);
            graficos.DrawString("   TICKET DE VENTA  ", fuenteNormal, Brushes.Black, startX + 50, startY + 20);
            graficos.DrawString($"Fecha: {DateTime.Now}", fuenteNormal, Brushes.Black, startX, startY + offset);
            graficos.DrawString($"Cajero: {UserSession.Nombre}", fuenteNormal, Brushes.Black, startX, startY + offset + 20);
            graficos.DrawString("----------------------------------------", fuenteNormal, Brushes.Black, startX, startY + offset + 40);

            offset += 60;
            graficos.DrawString("CANT  PRODUCTO             SUBTOTAL", fuenteNegrita, Brushes.Black, startX, startY + offset);
            offset += 20;

            // Filas del carrito
            decimal granTotal = 0;
            foreach (DataGridViewRow fila in dgvCarrito.Rows)
            {
                if (fila.Cells["ProductoID"].Value == null) continue;

                string cant = fila.Cells["Cantidad"].Value.ToString().PadRight(5);

                string prod = fila.Cells["Nombre"].Value.ToString();
                // Recortamos el nombre si es muy largo para que no rompa el formato del ticket
                if (prod.Length > 20) prod = prod.Substring(0, 20);
                prod = prod.PadRight(21);

                decimal subtotal = Convert.ToDecimal(fila.Cells["Subtotal"].Value);
                granTotal += subtotal;

                string sub = subtotal.ToString("C2");

                graficos.DrawString($"{cant} {prod} {sub}", fuenteNormal, Brushes.Black, startX, startY + offset);
                offset += 20;
            }

            // Pie de ticket
            graficos.DrawString("----------------------------------------", fuenteNormal, Brushes.Black, startX, startY + offset);
            offset += 20;
            graficos.DrawString($"TOTAL A PAGAR: {granTotal:C2}", fuenteNegrita, Brushes.Black, startX, startY + offset);
            offset += 30;
            graficos.DrawString("   ¡Gracias por su preferencia!   ", fuenteNormal, Brushes.Black, startX + 20, startY + offset);
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
            // 3. VALIDACIÓN DE STOCK
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
            cmbProductos.SelectedIndex = -1;
            // Nota: txtPrecio y lblProveedor se limpian solos gracias al evento SelectedIndexChanged del ComboBox

            // 6. Calcular el Gran Total
            CalcularTotal();

            // Regresar el foco al combobox para seguir escaneando o buscando productos
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

        private void cmbProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Si hay un producto seleccionado, extraemos su data de la fila seleccionada
            if (cmbProductos.SelectedIndex != -1 && cmbProductos.SelectedItem is DataRowView row)
            {
                txtPrecio.Text = row["PrecioUnitario"].ToString();

                // Suponiendo que agregaste el label lblProveedor a tu diseño:
                txtProveedor.Text = $"{row["Proveedor"]}";
            }
            else
            {
                txtPrecio.Clear();
                if (txtProveedor != null) txtProveedor.Text = "Proveedor: ---";
            }
        }

        private void CalcularTotal()
        {
            decimal granTotal = 0;
            foreach (DataGridViewRow fila in dgvCarrito.Rows)
            {
                if (fila.Cells["Subtotal"].Value != null)
                {
                    granTotal += Convert.ToDecimal(fila.Cells["Subtotal"].Value);
                }
            }
            // Suponiendo que agregaste el label lblTotal a tu diseño:
            txtTotal.Text = $"TOTAL: {granTotal:C2}";
        }
    }
}
