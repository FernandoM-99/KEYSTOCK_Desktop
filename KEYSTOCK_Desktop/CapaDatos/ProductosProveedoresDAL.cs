using KEYSTOCK_Desktop.Modelos;
using System;
using System.Data;
using System.Data.SqlClient;

namespace KEYSTOCK_Desktop.CapaDatos
{
    public class ProductosProveedoresDAL
    {
        private Conexion conexion = new Conexion();

        // Inserta la relación en la tabla intermedia
        public bool Vincular(int productoId, int proveedorId, decimal costo, string skuProv)
        {
            using (var conn = conexion.LeerConexion())
            {
                conn.Open();

                conexion.SetContextoSeguridad(conn, UserSession.Nombre, Environment.MachineName);
                string query = @"INSERT INTO ProductosProveedores (ProductoID, ProveedorID, CostoUltimaCompra, SKUProveedor) 
                                 VALUES (@pId, @prId, @costo, @skuProv)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@pId", productoId);
                cmd.Parameters.AddWithValue("@prId", proveedorId);
                cmd.Parameters.AddWithValue("@costo", costo);
                cmd.Parameters.AddWithValue("@skuProv", skuProv);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Lista los proveedores de un producto específico (para un DataGridView)
        public DataTable ListarPorProducto(int productoId)
        {
            DataTable tabla = new DataTable();
            using (var conn = conexion.LeerConexion())
            {
                string query = @"SELECT Pr.NombreEmpresa, PP.CostoUltimaCompra, PP.SKUProveedor 
                                 FROM ProductosProveedores PP
                                 INNER JOIN Proveedores Pr ON PP.ProveedorID = Pr.ProveedorID
                                 WHERE PP.ProductoID = @pId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@pId", productoId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }
            return tabla;
        }

        // Agrega este método para obtener el ID del proveedor actual del producto
        public int ObtenerProveedorDeProducto(int productoId)
        {
            using (var conn = conexion.LeerConexion())
            {
                string query = "SELECT TOP 1 ProveedorID FROM ProductosProveedores WHERE ProductoID = @pId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@pId", productoId);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        // Agrega este método para actualizar el vínculo al editar el producto
        public void ActualizarVinculo(int productoId, int proveedorId, decimal costo, string skuProv)
        {
            using (var conn = conexion.LeerConexion())
            {
                conn.Open();
                conexion.SetContextoSeguridad(conn, UserSession.Nombre, Environment.MachineName);

                // 1. Eliminamos el vínculo anterior para evitar duplicados
                string queryDelete = "DELETE FROM ProductosProveedores WHERE ProductoID = @pId";
                SqlCommand cmdDel = new SqlCommand(queryDelete, conn);
                cmdDel.Parameters.AddWithValue("@pId", productoId);
                cmdDel.ExecuteNonQuery();

                // 2. Insertamos el nuevo vínculo
                string queryInsert = @"INSERT INTO ProductosProveedores (ProductoID, ProveedorID, CostoUltimaCompra, SKUProveedor) 
                               VALUES (@pId, @prId, @costo, @skuProv)";
                SqlCommand cmdIns = new SqlCommand(queryInsert, conn);
                cmdIns.Parameters.AddWithValue("@pId", productoId);
                cmdIns.Parameters.AddWithValue("@prId", proveedorId);
                cmdIns.Parameters.AddWithValue("@costo", costo);
                cmdIns.Parameters.AddWithValue("@skuProv", skuProv);
                cmdIns.ExecuteNonQuery();
            }
        }
    }
}