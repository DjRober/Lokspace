using System;

namespace Lokspace
{
    public class Reserva
    {
        public int id_reserva { get; set; }
        public DateTime fecha_reserva { get; set; }
        public TimeSpan hora_inicio { get; set; }
        public TimeSpan hora_fin { get; set; }
        public string proposito { get; set; }
        public DateTime fecha_solicitud { get; set; }
        public int id_espacio { get; set; }
        public int id_usuario { get; set; }
        public int? id_gestor { get; set; }

        public int id_estado_reserva { get; set; }

        // Propiedades de navegación (para mostrar información relacionada)
        public string NombreEspacio { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreGestor { get; set; }
        public string EstadoReserva { get; set; }
        public string RolUsuario { get; set; }
    }
}
