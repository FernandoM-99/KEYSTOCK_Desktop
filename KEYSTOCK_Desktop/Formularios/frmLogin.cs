using KEYSTOCK_Desktop.CapaDatos;
using KEYSTOCK_Desktop.Modelos;
using System;
using System.Data.SqlClient;
using System.Drawing;
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

            // 2. Validación de campos vacíos
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMensaje.Text = "⚠️ Los campos no pueden estar vacíos.";
                return;
            }

            try
            {
                // Hasheo de la contraseña ingresada para comparar con la base de datos
                string hashedInput = SecurityHelper.HashPassword(txtPassword.Text);

                using (SqlConnection conn = new Conexion().LeerConexion())
                {
                    // Consulta validando Nombre de Usuario, Contraseña Hasheada y Estado Activo
                    string query = @"SELECT UsuarioID, NombreCompleto, RoleID 
                           FROM Usuarios 
                           WHERE Username = @user AND PasswordHash = @pass AND Activo = 1";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", txtUser.Text.Trim());
                    cmd.Parameters.AddWithValue("@pass", hashedInput);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // 3. Éxito: Guardar datos en la sesión global
                            UserSession.UsuarioID = Convert.ToInt32(reader["UsuarioID"]);
                            UserSession.Nombre = reader["NombreCompleto"].ToString();
                            UserSession.RoleID = Convert.ToInt32(reader["RoleID"]);

                            // IMPORTANTE: Avisamos al Program.cs que el login fue correcto y cerramos este form
                            // Esto evita que el proceso muera al abrir el Dashboard
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            lblMensaje.Text = "❌ Usuario o contraseña incorrectos.";
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Manejo de errores de conexión técnica
                if (ex.Number == 26 || ex.Number == 2)
                    lblMensaje.Text = "⚠️ No se pudo localizar el servidor SQL.";
                else if (ex.Number == 4060)
                    lblMensaje.Text = "⚠️ La base de datos no es accesible.";
                else
                    lblMensaje.Text = "🚨 Error técnico: " + ex.Message;
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