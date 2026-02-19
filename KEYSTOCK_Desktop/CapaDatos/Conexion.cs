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
    }
}
