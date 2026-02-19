using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEYSTOCK_Desktop.CapaDatos
{
    internal class MovimientoDAL
    {
        private Conexion conexion = new Conexion();
        public bool RegistrarMovimiento(int productoId, int usuarioId, int? proveedorId, string tipo, int cantidad, string referencia)
        {
            using (var conn = conexion.LeerConexion())
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    string queryMov = @"INSERT INTO MovimientosInventario (ProductoID, UsuarioID, ProveedorID, TipoMovimiento, Cantidad, FechaHora, Referencia) 
                                VALUES (@pId, @uId, @prId, @tipo, @cant, GETDATE(), @ref)";

                    SqlCommand cmdMov = new SqlCommand(queryMov, conn, tran);
                    cmdMov.Parameters.AddWithValue("@pId", productoId);
                    cmdMov.Parameters.AddWithValue("@uId", usuarioId);

                    // Si no hay proveedor (en caso de salidas), enviamos NULL a la DB
                    cmdMov.Parameters.AddWithValue("@prId", (object)proveedorId ?? DBNull.Value);

                    cmdMov.Parameters.AddWithValue("@tipo", tipo);
                    cmdMov.Parameters.AddWithValue("@cant", cantidad);
                    cmdMov.Parameters.AddWithValue("@ref", referencia);
                    cmdMov.ExecuteNonQuery();

                    // Lógica de actualización de Stock...
                    string operacion = (tipo == "Entrada") ? "+" : "-";
                    string queryStock = $"UPDATE Productos SET StockActual = StockActual {operacion} @cant WHERE ProductoID = @pId";
                    SqlCommand cmdStock = new SqlCommand(queryStock, conn, tran);
                    cmdStock.Parameters.AddWithValue("@cant", cantidad);
                    cmdStock.Parameters.AddWithValue("@pId", productoId);
                    cmdStock.ExecuteNonQuery();

                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex; // Re-lanzamos para ver el error en la UI
                }
            }
        }

        // Método para saber el stock antes de una salida
        public int ObtenerStockActual(int productoId)
        {
            using (var conn = conexion.LeerConexion())
            {
                string query = "SELECT StockActual FROM Productos WHERE ProductoID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", productoId);
                conn.Open();
                object resultado = cmd.ExecuteScalar();
                return (resultado != null) ? Convert.ToInt32(resultado) : 0;
            }
        }

        // Método para llenar el DataGridView de movimientos
        public DataTable ListarMovimientos()
        {
            DataTable tabla = new DataTable();
            using (var conn = conexion.LeerConexion())
            {
                // Consulta con JOIN para ver el nombre del producto en lugar de solo el ID
                string query = @"SELECT M.MovimientoID, P.Nombre as Producto, M.TipoMovimiento, 
                         M.Cantidad, M.FechaHora, M.Referencia 
                         FROM MovimientosInventario M 
                         INNER JOIN Productos P ON M.ProductoID = P.ProductoID 
                         ORDER BY M.FechaHora DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(tabla);
            }
            return tabla;
        }
    }
}
