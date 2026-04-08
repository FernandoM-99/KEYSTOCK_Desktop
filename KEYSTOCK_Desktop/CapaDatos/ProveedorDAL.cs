using KEYSTOCK_Desktop.CapaDatos;
using KEYSTOCK_Desktop.Modelos;
using System;
using System.Data;
using System.Data.SqlClient;

public class ProveedorDAL
{
    private Conexion conexion = new Conexion();

    public DataTable Listar()
    {
        DataTable tabla = new DataTable();
        using (var conn = conexion.LeerConexion())
        {
            string query = "SELECT * FROM Proveedores ORDER BY NombreEmpresa ASC";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            da.Fill(tabla);
        }
        return tabla;
    }

    public bool Insertar(string empresa, string contacto, string email, string tel)
    {
        using (var conn = conexion.LeerConexion())
        {
            conn.Open();

            conexion.SetContextoSeguridad(conn, UserSession.Nombre, Environment.MachineName);

            string query = @"INSERT INTO Proveedores (NombreEmpresa, NombreContacto, Email, Telefono) 
                             VALUES (@emp, @cont, @email, @tel)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@emp", empresa);
            cmd.Parameters.AddWithValue("@cont", contacto);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@tel", tel);

            return cmd.ExecuteNonQuery() > 0;
        }
    }

    // 3. ACTUALIZAR PROVEEDOR
    public bool Editar(int id, string empresa, string contacto, string email, string tel, bool activo)
    {
        using (var conn = conexion.LeerConexion())
        {
            conn.Open();
            // Contexto para la bitácora
            conexion.SetContextoSeguridad(conn, UserSession.Nombre, Environment.MachineName);

            string query = @"UPDATE Proveedores SET NombreEmpresa=@emp, NombreContacto=@cont, 
                         Email=@email, Telefono=@tel, Activo=@act WHERE ProveedorID=@id";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@emp", empresa);
            cmd.Parameters.AddWithValue("@cont", contacto);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@tel", tel);
            cmd.Parameters.AddWithValue("@act", activo);

            return cmd.ExecuteNonQuery() > 0;
        }
    }

    // 4. ELIMINACIÓN LÓGICA (Inactivar)
    public bool Inactivar(int id)
    {
        using (var conn = conexion.LeerConexion())
        {
            conn.Open();
            conexion.SetContextoSeguridad(conn, UserSession.Nombre, Environment.MachineName);

            string query = "UPDATE Proveedores SET Activo = 0 WHERE ProveedorID = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}