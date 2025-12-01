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


        // ==================== MÉTODOS PARA ESPACIOS PADRE (TIPOS) ====================

        // Método para obtener todos los tipos de espacios
        public List<EspacioPadre> ObtenerTodosTiposEspacios()
        {
            var tipos = new List<EspacioPadre>();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM tipos_espacios ORDER BY nombre_tipo";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tipos.Add(new EspacioPadre
                            {
                                id_tipo_espacio = reader.GetInt32("id_tipo_espacio"),
                                nombre_tipo = reader.GetString("nombre_tipo")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener tipos de espacios: {ex.Message}");
            }

            return tipos;
        }

        // Método para agregar un nuevo tipo de espacio
        public int AgregarTipoEspacio(EspacioPadre tipoEspacio)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO tipos_espacios (nombre_tipo) 
                                    VALUES (@nombre);
                                    SELECT LAST_INSERT_ID();";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", tipoEspacio.nombre_tipo);

                    int nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
                    return nuevoId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar tipo de espacio: {ex.Message}");
                return -1;
            }
        }

        // Método para actualizar un tipo de espacio
        public bool ActualizarTipoEspacio(EspacioPadre tipoEspacio)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE tipos_espacios 
                                    SET nombre_tipo = @nombre 
                                    WHERE id_tipo_espacio = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", tipoEspacio.nombre_tipo);
                    cmd.Parameters.AddWithValue("@id", tipoEspacio.id_tipo_espacio);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar tipo de espacio: {ex.Message}");
                return false;
            }
        }

        // Método para eliminar un tipo de espacio
        public bool EliminarTipoEspacio(int idTipoEspacio)
        {
            try
            {
                // Primero verificamos si hay espacios hijos que dependen de este tipo
                if (TieneEspaciosHijos(idTipoEspacio))
                {
                    MessageBox.Show("No se puede eliminar el tipo de espacio porque tiene espacios asociados.");
                    return false;
                }

                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM tipos_espacios WHERE id_tipo_espacio = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idTipoEspacio);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar tipo de espacio: {ex.Message}");
                return false;
            }
        }

        // Método para verificar si un tipo de espacio tiene espacios hijos
        private bool TieneEspaciosHijos(int idTipoEspacio)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM espacios WHERE id_tipo_espacio = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idTipoEspacio);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar espacios hijos: {ex.Message}");
                return true; // Por seguridad, asumimos que sí tiene para evitar eliminaciones
            }
        }

        // ==================== MÉTODOS PARA ESPACIOS HIJO (ESPACIOS) ====================

        // Método para obtener todos los espacios
        public List<EspacioHijo> ObtenerTodosEspaciosHijos()
        {
            var espacios = new List<EspacioHijo>();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT e.*, te.nombre_tipo 
                                    FROM espacios e 
                                    LEFT JOIN tipos_espacios te ON e.id_tipo_espacio = te.id_tipo_espacio 
                                    ORDER BY e.nombre_espacio";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            espacios.Add(new EspacioHijo
                            {
                                id_espacio = reader.GetInt32("id_espacio"),
                                nombre_espacio = reader.GetString("nombre_espacio"),
                                estado_espacio = reader.GetString("estado_espacio"),
                                fecha_registro = reader.GetDateTime("fecha_registro"),
                                capacidad = reader.GetInt32("capacidad"),
                                id_tipo_espacio = reader.GetInt32("id_tipo_espacio"),

                            });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener espacios: {ex.Message}");
            }

            return espacios;
        }

        // Método para agregar un nuevo espacio
        public int AgregarEspacio(EspacioHijo espacio)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO espacios 
                                    (nombre_espacio, estado_espacio, fecha_registro, capacidad, id_tipo_espacio) 
                                    VALUES (@nombre, @estado, @fecha, @capacidad, @id_tipo);
                                    SELECT LAST_INSERT_ID();";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", espacio.nombre_espacio);
                    cmd.Parameters.AddWithValue("@estado", espacio.estado_espacio);
                    cmd.Parameters.AddWithValue("@fecha", espacio.fecha_registro);
                    cmd.Parameters.AddWithValue("@capacidad", espacio.capacidad);
                    cmd.Parameters.AddWithValue("@id_tipo", espacio.id_tipo_espacio);

                    int nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
                    return nuevoId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar espacio: {ex.Message}");
                return -1;
            }
        }

        // Método para actualizar un espacio
        public bool ActualizarEspacio(EspacioHijo espacio)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE espacios 
                                    SET nombre_espacio = @nombre, 
                                        estado_espacio = @estado, 
                                        fecha_registro = @fecha, 
                                        capacidad = @capacidad, 
                                        id_tipo_espacio = @id_tipo 
                                    WHERE id_espacio = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", espacio.nombre_espacio);
                    cmd.Parameters.AddWithValue("@estado", espacio.estado_espacio);
                    cmd.Parameters.AddWithValue("@fecha", espacio.fecha_registro);
                    cmd.Parameters.AddWithValue("@capacidad", espacio.capacidad);
                    cmd.Parameters.AddWithValue("@id_tipo", espacio.id_tipo_espacio);
                    cmd.Parameters.AddWithValue("@id", espacio.id_espacio);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar espacio: {ex.Message}");
                return false;
            }
        }

        // Método para eliminar un espacio
        public bool EliminarEspacio(int idEspacio)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM espacios WHERE id_espacio = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idEspacio);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar espacio: {ex.Message}");
                return false;
            }
        }

        // Método para obtener espacios por tipo
        public List<EspacioHijo> ObtenerEspaciosPorTipoR(int idTipoEspacio)
        {
            var espacios = new List<EspacioHijo>();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT e.*, te.nombre_tipo 
                                    FROM espacios e 
                                    LEFT JOIN tipos_espacios te ON e.id_tipo_espacio = te.id_tipo_espacio 
                                    WHERE e.id_tipo_espacio = @id_tipo 
                                    ORDER BY e.nombre_espacio";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id_tipo", idTipoEspacio);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            espacios.Add(new EspacioHijo
                            {
                                id_espacio = reader.GetInt32("id_espacio"),
                                nombre_espacio = reader.GetString("nombre_espacio"),
                                estado_espacio = reader.GetString("estado_espacio"),
                                fecha_registro = reader.GetDateTime("fecha_registro"),
                                capacidad = reader.GetInt32("capacidad"),
                                id_tipo_espacio = reader.GetInt32("id_tipo_espacio"),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener espacios por tipo: {ex.Message}");
            }

            return espacios;
        }

        // Método para obtener un espacio por su ID
        public EspacioHijo ObtenerEspacioPorId(int idEspacio)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT e.*, te.nombre_tipo 
                                    FROM espacios e 
                                    LEFT JOIN tipos_espacios te ON e.id_tipo_espacio = te.id_tipo_espacio 
                                    WHERE e.id_espacio = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idEspacio);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new EspacioHijo
                            {
                                id_espacio = reader.GetInt32("id_espacio"),
                                nombre_espacio = reader.GetString("nombre_espacio"),
                                estado_espacio = reader.GetString("estado_espacio"),
                                fecha_registro = reader.GetDateTime("fecha_registro"),
                                capacidad = reader.GetInt32("capacidad"),
                                id_tipo_espacio = reader.GetInt32("id_tipo_espacio"),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener espacio: {ex.Message}");
            }

            return null;
        }

        // Método para cambiar el estado de un espacio (activar/desactivar)
        public bool CambiarEstadoEspacio(int idEspacio, bool nuevoEstado)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE espacios SET estado_espacio = @estado WHERE id_espacio = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                    cmd.Parameters.AddWithValue("@id", idEspacio);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar estado del espacio: {ex.Message}");
                return false;
            }
        }

        // Método para verificar si existe un espacio con el mismo nombre
        public bool VerificarNombreEspacioExistente(string nombreEspacio, int? idEspacioExcluir = null)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM espacios WHERE nombre_espacio = @nombre";

                    if (idEspacioExcluir.HasValue)
                    {
                        query += " AND id_espacio != @idExcluir";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", nombreEspacio);

                    if (idEspacioExcluir.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@idExcluir", idEspacioExcluir.Value);
                    }

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar nombre de espacio: {ex.Message}");
                return false;
            }
        }

    }
}

