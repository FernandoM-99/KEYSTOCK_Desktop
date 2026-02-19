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
                string query = @"INSERT INTO ProductosProveedores (ProductoID, ProveedorID, CostoUltimaCompra, SKUProveedor) 
                                 VALUES (@pId, @prId, @costo, @skuProv)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@pId", productoId);
                cmd.Parameters.AddWithValue("@prId", proveedorId);
                cmd.Parameters.AddWithValue("@costo", costo);
                cmd.Parameters.AddWithValue("@skuProv", skuProv);

                conn.Open();
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
    }
}