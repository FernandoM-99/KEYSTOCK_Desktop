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

namespace KEYSTOCK_Desktop
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // 1. Limpiar mensajes previos
            lblMensaje.Text = "";
            lblMensaje.ForeColor = Color.Red;

            // 2. Validación de campos vacíos (Interfaz)
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMensaje.Text = "⚠️ Los campos no pueden estar vacíos.";
                return;
            }

            try
            {
                using (SqlConnection conn = new Conexion().LeerConexion())
                {
                    // Consulta basada en tu diagrama: Usuarios -> Roles
                    string query = "SELECT UsuarioID, NombreCompleto, RoleID FROM Usuarios " +
                                   "WHERE Email = @email AND PasswordHash = @pass AND Activo = 1";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@pass", txtPassword.Text.Trim());

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Éxito: Guardar sesión y entrar
                        UserSession.UsuarioID = (int)reader["UsuarioID"];
                        UserSession.Nombre = reader["NombreCompleto"].ToString();
                        UserSession.RoleID = (int)reader["RoleID"];

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        // 3. Error de Credenciales
                        lblMensaje.Text = "❌ Correo o contraseña incorrectos.";
                    }
                }
            }
            catch (SqlException ex)
            {
                // 4. Manejo del Error de Conexión (El error 26 que te apareció)
                if (ex.Number == 26 || ex.Number == 2)
                    lblMensaje.Text = "⚠️ No se pudo localizar el servidor SQL.";
                else if (ex.Number == 4060)
                    lblMensaje.Text = "⚠️ La base de datos no es accesible.";
                else
                    lblMensaje.Text = "🚨 Error técnico: " + ex.Message;

                // Log para el desarrollador en la consola
                Console.WriteLine("Error Detallado: " + ex.ToString());
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            lblMensaje.Text = "";
        }
    }
}
