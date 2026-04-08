using KEYSTOCK_Desktop.Modelos;
using System;
using System.Data;
using System.Data.SqlClient;

namespace KEYSTOCK_Desktop.CapaDatos
{
    public class RolesDAL
    {
        private Conexion conexion = new Conexion();

        // Obtener todos los roles
        public DataTable ListarRoles()
        {
            DataTable tabla = new DataTable();
            using (var conn = conexion.LeerConexion())
            {
                string query = "SELECT RoleID, NombreRol FROM Roles ORDER BY RoleID ASC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(tabla);
            }
            return tabla;
        }

        // Insertar un nuevo rol
        public bool Insertar(string nombreRol)
        {
            using (var conn = conexion.LeerConexion())
            {
                conn.Open();

                conexion.SetContextoSeguridad(conn, UserSession.Nombre, Environment.MachineName);

                string query = "INSERT INTO Roles (NombreRol) VALUES (@nombre)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", nombreRol);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Editar un rol existente
        public bool Editar(int id, string nombreRol)
        {
            using (var conn = conexion.LeerConexion())
            {
                conn.Open();

                conexion.SetContextoSeguridad(conn, UserSession.Nombre, Environment.MachineName);

                string query = "UPDATE Roles SET NombreRol = @nombre WHERE RoleID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nombre", nombreRol);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}