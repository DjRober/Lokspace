using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Lokspace
{
    internal class EspacioService
    {
        // Usa la clase Database que ya existe en tu proyecto
        private Database db = new Database();

        public List<Espacio> ObtenerTodosEspacios()
        {
            var lista = new List<Espacio>();

            string query = @"
                SELECT 
                    e.id_espacio,
                    e.nombre_espacio,
                    e.estado_espacio,
                    e.fecha_registro,
                    e.capacidad,
                    e.id_tipo_espacio,
                    t.nombre_tipo
                FROM espacios e
                LEFT JOIN tipos_espacios t ON e.id_tipo_espacio = t.id_tipo_espacio;
            ";

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var esp = new Espacio();

                            // IdEspacio (int)
                            if (!reader.IsDBNull(reader.GetOrdinal("id_espacio")))
                                esp.IdEspacio = reader.GetInt32("id_espacio");

                            // NombreEspacio (string)
                            if (!reader.IsDBNull(reader.GetOrdinal("nombre_espacio")))
                                esp.NombreEspacio = reader.GetString("nombre_espacio");

                            // EstadoEspacio (string)
                            if (!reader.IsDBNull(reader.GetOrdinal("estado_espacio")))
                                esp.EstadoEspacio = reader.GetString("estado_espacio");

                            // FechaRegistro (DateTime) - fallback a DateTime.MinValue si es null
                            if (!reader.IsDBNull(reader.GetOrdinal("fecha_registro")))
                                esp.FechaRegistro = reader.GetDateTime("fecha_registro");
                            else
                                esp.FechaRegistro = DateTime.MinValue;

                            // Capacidad (int) - fallback 0
                            if (!reader.IsDBNull(reader.GetOrdinal("capacidad")))
                                esp.Capacidad = reader.GetInt32("capacidad");
                            else
                                esp.Capacidad = 0;

                            // IdTipoEspacio (int) - fallback 0
                            if (!reader.IsDBNull(reader.GetOrdinal("id_tipo_espacio")))
                                esp.IdTipoEspacio = reader.GetInt32("id_tipo_espacio");
                            else
                                esp.IdTipoEspacio = 0;

                            // NombreTipoEspacio (string) - fallback "Desconocido"
                            if (!reader.IsDBNull(reader.GetOrdinal("nombre_tipo")))
                                esp.NombreTipoEspacio = reader.GetString("nombre_tipo");
                            else
                                esp.NombreTipoEspacio = "Desconocido";

                            lista.Add(esp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Para debugging rápido mientras pruebas:
                MessageBox.Show($"Error en ObtenerTodosEspacios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return lista;
        }

        // Método para obtener espacios por tipo, con parámetro aplicado correctamente
        public List<Espacio> ObtenerEspaciosPorTipo(int idTipoEspacio)
        {
            var lista = new List<Espacio>();

            string query = @"
                SELECT 
                    e.id_espacio,
                    e.nombre_espacio,
                    e.estado_espacio,
                    e.fecha_registro,
                    e.capacidad,
                    e.id_tipo_espacio,
                    t.nombre_tipo
                FROM espacios e
                LEFT JOIN tipos_espacios t ON e.id_tipo_espacio = t.id_tipo_espacio
                WHERE e.id_tipo_espacio = @idTipoEspacio;
            ";

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idTipoEspacio", idTipoEspacio);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var esp = new Espacio();

                                if (!reader.IsDBNull(reader.GetOrdinal("id_espacio")))
                                    esp.IdEspacio = reader.GetInt32("id_espacio");
                                if (!reader.IsDBNull(reader.GetOrdinal("nombre_espacio")))
                                    esp.NombreEspacio = reader.GetString("nombre_espacio");
                                if (!reader.IsDBNull(reader.GetOrdinal("estado_espacio")))
                                    esp.EstadoEspacio = reader.GetString("estado_espacio");
                                if (!reader.IsDBNull(reader.GetOrdinal("fecha_registro")))
                                    esp.FechaRegistro = reader.GetDateTime("fecha_registro");
                                if (!reader.IsDBNull(reader.GetOrdinal("capacidad")))
                                    esp.Capacidad = reader.GetInt32("capacidad");
                                if (!reader.IsDBNull(reader.GetOrdinal("id_tipo_espacio")))
                                    esp.IdTipoEspacio = reader.GetInt32("id_tipo_espacio");
                                if (!reader.IsDBNull(reader.GetOrdinal("nombre_tipo")))
                                    esp.NombreTipoEspacio = reader.GetString("nombre_tipo");
                                else
                                    esp.NombreTipoEspacio = "Desconocido";

                                lista.Add(esp);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en ObtenerEspaciosPorTipo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return lista;
        }
    }
}
