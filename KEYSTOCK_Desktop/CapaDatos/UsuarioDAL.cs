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
        public bool InsertarUsuario(string nombre, string email, string pass, int roleId, bool activo)
        {
            using (var conn = conexion.LeerConexion())
            {
                string query = @"INSERT INTO Usuarios (NombreCompleto, Email, PasswordHash, RoleID, Activo) 
                         VALUES (@nom, @email, @pass, @role, @act)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nom", nombre);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@pass", pass); // Recuerda usar Hashing en producción
                cmd.Parameters.AddWithValue("@role", roleId);
                cmd.Parameters.AddWithValue("@act", activo);
                conn.Open();
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
                string query = @"SELECT U.UsuarioID, U.NombreCompleto, U.Email, R.NombreRol, U.Activo 
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
        public bool EditarUsuario(int id, string nombre, string email, int roleId, bool activo)
        {
            using (var conn = conexion.LeerConexion())
            {
                string query = @"UPDATE Usuarios 
                         SET NombreCompleto = @nom, Email = @email, RoleID = @role, Activo = @act 
                         WHERE UsuarioID = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nom", nombre);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@role", roleId);
                cmd.Parameters.AddWithValue("@act", activo);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }

}
