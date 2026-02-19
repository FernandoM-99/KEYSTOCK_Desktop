using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEYSTOCK_Desktop.CapaDatos
{
    public class ProductoDAL
    {
        private Conexion conexion = new Conexion();

        // 1. LISTAR PRODUCTOS
        public DataTable Listar()
        {
            DataTable tabla = new DataTable();
            using (var conn = conexion.LeerConexion())
            {
                string query = "SELECT ProductoID, SKU, Nombre, Descripcion, StockActual FROM Productos";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                conn.Open();
                da.Fill(tabla);
            }
            return tabla;
        }

        // 2. INSERTAR PRODUCTO
        public bool Insertar(string sku, string nombre, string desc, int stock)
        {
            using (var conn = conexion.LeerConexion())
            {
                string query = "INSERT INTO Productos (SKU, Nombre, Descripcion, StockActual) VALUES (@sku, @nom, @desc, @stock)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sku", sku);
                cmd.Parameters.AddWithValue("@nom", nombre);
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@stock", stock);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        // 3. ACTUALIZAR PRODUCTO
        public bool Editar(int id, string sku, string nombre, string desc, int stock)
        {
            using (var conn = conexion.LeerConexion())
            {
                string query = "UPDATE Productos SET SKU=@sku, Nombre=@nom, Descripcion=@desc, StockActual=@stock WHERE ProductoID=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@sku", sku);
                cmd.Parameters.AddWithValue("@nom", nombre);
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@stock", stock);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 4. ELIMINAR PRODUCTO
        public bool Eliminar(int id)
        {
            using (var conn = conexion.LeerConexion())
            {
                string query = "DELETE FROM Productos WHERE ProductoID=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
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
    }
}
