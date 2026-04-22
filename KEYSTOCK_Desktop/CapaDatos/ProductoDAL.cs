using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KEYSTOCK_Desktop.Modelos;

namespace KEYSTOCK_Desktop.CapaDatos
{
    public class ProductoDAL
    {
        private Conexion conexion = new Conexion();

        // 1. LISTAR PRODUCTOS
        // 1. LISTAR PRODUCTOS (Actualizado con nombre de Proveedor)
        public DataTable Listar()
        {
            DataTable tabla = new DataTable();
            using (var conn = conexion.LeerConexion())
            {
                // Utilizamos LEFT JOIN para que si un producto aún no tiene proveedor, 
                // de todos modos aparezca en la lista (mostrando 'Sin Proveedor')
                string query = @"
            SELECT 
                P.ProductoID, 
                P.SKU, 
                P.Nombre, 
                P.Descripcion, 
                P.StockActual, 
                P.PrecioUnitario,
                ISNULL(Pr.NombreEmpresa, 'Sin Proveedor') AS Proveedor
            FROM Productos P
            LEFT JOIN ProductosProveedores PP ON P.ProductoID = PP.ProductoID
            LEFT JOIN Proveedores Pr ON PP.ProveedorID = Pr.ProveedorID
            ORDER BY P.Nombre ASC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                conn.Open();
                da.Fill(tabla);
            }
            return tabla;
        }

        // 2. INSERTAR PRODUCTO (Actualizado para retornar ID)
        public int Insertar(string sku, string nombre, string desc, int stock, decimal precio)
        {
            using (var conn = conexion.LeerConexion())
            {
                conn.Open();
                conexion.SetContextoSeguridad(conn, UserSession.Nombre, Environment.MachineName);

                // Usamos SCOPE_IDENTITY() para obtener el ID recién creado
                string query = @"INSERT INTO Productos (SKU, Nombre, Descripcion, StockActual, PrecioUnitario) 
                         VALUES (@sku, @nom, @desc, @stock, @precio);
                         SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sku", sku);
                cmd.Parameters.AddWithValue("@nom", nombre);
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@precio", precio);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // 3. ACTUALIZAR PRODUCTO
        public bool Editar(int id, string sku, string nombre, string desc, int stock, decimal precio)
        {
            using (var conn = conexion.LeerConexion())
            {
                conn.Open();
                conexion.SetContextoSeguridad(conn, UserSession.Nombre, Environment.MachineName);

                string query = @"UPDATE Productos SET SKU=@sku, Nombre=@nom, Descripcion=@desc, 
                         StockActual=@stock, PrecioUnitario=@precio WHERE ProductoID=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@sku", sku);
                cmd.Parameters.AddWithValue("@nom", nombre);
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@precio", precio);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 4. ELIMINAR PRODUCTO
        public bool Eliminar(int id)
        {
            using (var conn = conexion.LeerConexion())
            {
                conn.Open();

                conexion.SetContextoSeguridad(conn, UserSession.Nombre, Environment.MachineName);
                string query = "DELETE FROM Productos WHERE ProductoID=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Agrega esto a tu clase ProductoDAL
        public DataTable ObtenerListaSimple()
        {
            DataTable tabla = new DataTable();
            using (var conn = conexion.LeerConexion())
            {
                string query = "SELECT ProductoID, Nombre FROM Productos ORDER BY Nombre ASC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(tabla);
            }
            return tabla;
        }

        // Agrega este método dentro de tu clase ProductoDAL
        public bool ExisteSKU(string sku, int idActual = 0)
        {
            using (var conn = conexion.LeerConexion())
            {
                // Cuenta cuántos productos tienen ese SKU, excluyendo el ID del producto que estamos editando (si aplica)
                string query = "SELECT COUNT(*) FROM Productos WHERE SKU = @sku AND ProductoID <> @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sku", sku);
                cmd.Parameters.AddWithValue("@id", idActual);

                conn.Open();
                int coincidencias = Convert.ToInt32(cmd.ExecuteScalar());

                return coincidencias > 0; // Retorna true si ya existe
            }
        }
    }
}
