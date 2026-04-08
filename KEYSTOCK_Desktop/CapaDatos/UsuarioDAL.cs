using KEYSTOCK_Desktop.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEYSTOCK_Desktop.CapaDatos
{
    internal class UsuarioDAL
    {
        private Conexion conexion = new Conexion();
        public bool InsertarUsuario(string nombre, string email, string pass, int roleId, bool activo, string username)
        {
            using (var conn = conexion.LeerConexion())
            {
                conn.Open();

                conexion.SetContextoSeguridad(conn, UserSession.Nombre, Environment.MachineName);
                string passwordHasheada = SecurityHelper.HashPassword(pass);

                string query = @"INSERT INTO Usuarios (NombreCompleto, Email, PasswordHash, RoleID, Activo) 
                         VALUES (@nom, @email, @pass, @role, @act)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nom", nombre);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@pass", passwordHasheada); // Recuerda usar Hashing en producción
                cmd.Parameters.AddWithValue("@role", roleId);
                cmd.Parameters.AddWithValue("@act", activo);
                cmd.Parameters.AddWithValue("@user", activo);
                
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public DataTable ListarRoles()
        {
            DataTable tabla = new DataTable();
            using (var conn = conexion.LeerConexion())
            {
                string query = "SELECT RoleID, NombreRol FROM Roles";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(tabla);
            }
            return tabla;
        }
        // Agrega esto a tu clase UsuarioDAL en la carpeta CapaDatos
        public DataTable ListarUsuarios()
        {
            DataTable tabla = new DataTable();
            using (var conn = conexion.LeerConexion())
            {
                // Consulta que une Usuarios con Roles para mayor claridad visual
                string query = @"SELECT U.UsuarioID, U.NombreCompleto, U.Email, R.NombreRol, U.Activo, u.Username 
                         FROM Usuarios U 
                         INNER JOIN Roles R ON U.RoleID = R.RoleID";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                try
                {
                    conn.Open();
                    da.Fill(tabla);
                }
                catch (Exception ex)
                {
                    // Manejo de errores de conexión hacia LAPTOP-ICAMTCRR\Ferna
                    Console.WriteLine("Error al listar usuarios: " + ex.Message);
                }
            }
            return tabla;
        }
        // Método para Modificar Usuario
        public bool EditarUsuario(int id, string nombre, string email, string pass, int roleId, bool activo, string username)
        {
            using (var conn = conexion.LeerConexion())
            {
                conn.Open();

                conexion.SetContextoSeguridad(conn, UserSession.Nombre, Environment.MachineName);
                // Usamos una lógica COALESCE o CASE en SQL: 
                // Si @pass es vacío o NULL, se queda la Contrasena actual.
                string passwordParaDB = string.IsNullOrEmpty(pass) ? "" : SecurityHelper.HashPassword(pass);

                string query = @"UPDATE Usuarios SET 
                         NombreCompleto=@nom, 
                         Email=@email, 
                         PasswordHash = CASE WHEN @pass = '' OR @pass IS NULL THEN PasswordHash ELSE @pass END,
                         RoleID=@rid, 
                         Activo=@act, 
                         Username=@user 
                         WHERE UsuarioID=@id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nom", nombre);
                cmd.Parameters.AddWithValue("@email", (object)email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@pass", (object)passwordParaDB ?? string.Empty);
                cmd.Parameters.AddWithValue("@rid", roleId);
                cmd.Parameters.AddWithValue("@act", activo);
                cmd.Parameters.AddWithValue("@user", username);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Método para Inactivar (Eliminar Lógico)
        public bool InactivarUsuario(int id)
        {
            using (var conn = conexion.LeerConexion())
            {
                conn.Open();

                conexion.SetContextoSeguridad(conn, UserSession.Nombre, Environment.MachineName);

                string query = "UPDATE Usuarios SET Activo = 0 WHERE UsuarioID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool ExisteUsuario(string username, int idActual = 0)
        {
            using (var conn = conexion.LeerConexion())
            {
                string query = "SELECT COUNT(*) FROM Usuarios WHERE Username = @user AND UsuarioID <> @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@id", idActual);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        // Listado filtrado por estado
        public DataTable ListarUsuarios(bool soloActivos = true)
        {
            DataTable tabla = new DataTable();
            using (var conn = conexion.LeerConexion())
            {
                string query = @"SELECT U.UsuarioID, U.NombreCompleto, U.Email, U.Username, 
                         R.NombreRol, U.Activo, U.RoleID 
                         FROM Usuarios U 
                         INNER JOIN Roles R ON U.RoleID = R.RoleID 
                         WHERE U.Activo = @estado 
                         ORDER BY U.NombreCompleto ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@estado", soloActivos ? 1 : 0);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }
            return tabla;
        }
    }

}
