using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace KEYSTOCK_Desktop.CapaDatos
{
    public class Conexion
    {
        // CAMBIA ESTA LÍNEA:
        private string cadena = ConfigurationManager.ConnectionStrings["KeystockConn"].ConnectionString;
        public SqlConnection LeerConexion()
        {
            return new SqlConnection(cadena);
        }

        public void SetContextoSeguridad(SqlConnection conn, string usuario, string equipo)
        {
            // Verificamos que la conexión esté abierta antes de ejecutar
            if (conn.State != System.Data.ConnectionState.Open) conn.Open();

            string query = "SET CONTEXT_INFO @info";

            // Formateamos la cadena de contexto
            string contexto = $"{usuario}|{equipo}";
            byte[] info = System.Text.Encoding.UTF8.GetBytes(contexto);

            // El Context_info requiere exactamente 128 bytes
            Array.Resize(ref info, 128);

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@info", info);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
