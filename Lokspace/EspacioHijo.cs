using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lokspace
{
    public class EspacioHijo
    {
        public int id_espacio {  get; set; }
        public string nombre_espacio { get; set; }
        public bool estado_espacio { get; set; }
        public DateTime fecha_registro { get; set; }
        public int capacidad {  get; set; }
        public int id_tipo_espacio { get; set; }
    }
}
