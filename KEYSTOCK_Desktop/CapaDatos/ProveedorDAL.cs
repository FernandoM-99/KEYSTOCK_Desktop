using KEYSTOCK_Desktop.CapaDatos;
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
            string query = @"INSERT INTO Proveedores (NombreEmpresa, NombreContacto, Email, Telefono) 
                             VALUES (@emp, @cont, @email, @tel)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@emp", empresa);
            cmd.Parameters.AddWithValue("@cont", contacto);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@tel", tel);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}