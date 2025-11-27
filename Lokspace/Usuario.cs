using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Clase usuario adecuada a la base de datos

namespace Lokspace
{
    public class Usuario
    {
        public int id_usuario { get; set; }
        public string nombre1_usuario { get; set; }
        public string nombre2_usuario { get; set; }
        public string ap_usuario { get; set; }
        public string am_usuario { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int id_rol { get; set; }
    }
}
