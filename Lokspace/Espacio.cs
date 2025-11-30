using System;

namespace Lokspace
{
    public class Espacio
    {
        public int IdEspacio { get; set; }
        public string NombreEspacio { get; set; }
        public string EstadoEspacio { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int Capacidad { get; set; }
        public int IdTipoEspacio { get; set; }
        public string NombreTipoEspacio { get; set; }

        // Propiedades calculadas para compatibilidad con el código existente
        public string TipoDisplay
        {
            get
            {
                // Mapear nombres de tipos a los emojis usados anteriormente
                switch (NombreTipoEspacio.ToLower())
                {
                    case "edificio":
                    case "building":
                        return "🏢 Edificio";
                    case "aula":
                    case "classroom":
                        return "🏫 Aula";
                    case "deportivo":
                    case "sports":
                        return "⚽ Deportivo";
                    default:
                        return "❓ " + NombreTipoEspacio;
                }
            }
        }

        public string CapacidadDisplay
        {
            get
            {
                return Capacidad > 0 ? Capacidad + " personas" : "N/A";
            }
        }

        // Para compatibilidad con el código existente que usa "Description"
        public string Description
        {
            get
            {
                return $"Estado: {EstadoEspacio} | Registrado: {FechaRegistro:dd/MM/yyyy}";
            }
        }

        // Para compatibilidad con el código existente que usa "Name"
        public string Name
        {
            get
            {
                return NombreEspacio;
            }
        }

        // Para compatibilidad con el código existente que usa "Type"
        public string Type
        {
            get
            {
                switch (NombreTipoEspacio.ToLower())
                {
                    case "edificio":
                    case "building":
                        return "building";
                    case "aula":
                    case "classroom":
                        return "classroom";
                    case "deportivo":
                    case "sports":
                        return "sports";
                    default:
                        return "unknown";
                }
            }
        }
    }
}