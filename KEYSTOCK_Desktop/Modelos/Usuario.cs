using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEYSTOCK_Desktop.Modelos
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string NombreCompleto { get; set; }
        public int RoleID { get; set; }
        public bool Activo { get; set; }
    }
}
