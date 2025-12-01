using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Lokspace
{
    internal class ReservaService
    {
        private Database db = new Database();

        // Método para obtener todas las reservas con información relacionada
        public List<Reserva> ObtenerTodasReservas()
        {
            var reservas = new List<Reserva>();

            string query = @"
                SELECT 
                    r.id_reserva,
                    r.fecha_reserva,
                    r.hora_inicio,
                    r.hora_fin,
                    r.proposito,
                    r.fecha_solicitud,
                    r.id_espacio,
                    r.id_usuario,
                    r.id_gestor,
                    r.id_estado_reserva,
                    e.nombre_espacio,
                    CONCAT(u.nombre1_usuario, ' ', u.ap_usuario) as nombre_usuario,
                    CONCAT(ug.nombre1_usuario, ' ', ug.ap_usuario) as nombre_gestor,
                    er.nombre_estado,
                    rl.tipo_rol as rol_usuario
                FROM reservas r
                LEFT JOIN espacios e ON r.id_espacio = e.id_espacio
                LEFT JOIN usuarios u ON r.id_usuario = u.id_usuario
                LEFT JOIN usuarios ug ON r.id_gestor = ug.id_usuario
                LEFT JOIN estado_reserva er ON r.id_estado_reserva = er.id_estado
                LEFT JOIN roles rl ON u.id_rol = rl.id_rol
                ORDER BY r.fecha_reserva DESC, r.hora_inicio DESC";

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
                            var reserva = new Reserva();

                            // Datos básicos de la reserva
                            reserva.id_reserva = reader.GetInt32("id_reserva");
                            reserva.fecha_reserva = reader.GetDateTime("fecha_reserva");
                            reserva.hora_inicio = reader.GetTimeSpan("hora_inicio");
                            reserva.hora_fin = reader.GetTimeSpan("hora_fin");
                            reserva.proposito = reader.GetString("proposito");
                            reserva.fecha_solicitud = reader.GetDateTime("fecha_solicitud");
                            reserva.id_espacio = reader.GetInt32("id_espacio");
                            reserva.id_usuario = reader.GetInt32("id_usuario");
                            if (!reader.IsDBNull(reader.GetOrdinal("id_gestor")))
                                reserva.id_gestor = reader.GetInt32("id_gestor");
                            reserva.id_estado_reserva = reader.GetInt32("id_estado_reserva");

                            // Información relacionada
                            if (!reader.IsDBNull(reader.GetOrdinal("nombre_espacio")))
                                reserva.NombreEspacio = reader.GetString("nombre_espacio");

                            if (!reader.IsDBNull(reader.GetOrdinal("nombre_usuario")))
                                reserva.NombreUsuario = reader.GetString("nombre_usuario");

                            if (!reader.IsDBNull(reader.GetOrdinal("nombre_gestor")))
                                reserva.NombreGestor = reader.GetString("nombre_gestor");

                            if (!reader.IsDBNull(reader.GetOrdinal("nombre_estado")))
                                reserva.EstadoReserva = reader.GetString("nombre_estado");

                            if (!reader.IsDBNull(reader.GetOrdinal("rol_usuario")))
                                reserva.RolUsuario = reader.GetString("rol_usuario");

                            reservas.Add(reserva);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener reservas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return reservas;
        }

        // Método para crear una nueva reserva
        public int CrearReserva(Reserva reserva)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO reservas 
                                    (fecha_reserva, hora_inicio, hora_fin, proposito, fecha_solicitud, id_espacio, id_usuario, id_gestor, id_estado_reserva) 
                                    VALUES (@fecha, @horaInicio, @horaFin, @proposito, @fechaSolicitud, @idEspacio, @idUsuario, @idGestor, @idEstado);
                                    SELECT LAST_INSERT_ID();";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@fecha", reserva.fecha_reserva);
                    cmd.Parameters.AddWithValue("@horaInicio", reserva.hora_inicio);
                    cmd.Parameters.AddWithValue("@horaFin", reserva.hora_fin);
                    cmd.Parameters.AddWithValue("@proposito", reserva.proposito);
                    cmd.Parameters.AddWithValue("@fechaSolicitud", reserva.fecha_solicitud);
                    cmd.Parameters.AddWithValue("@idEspacio", reserva.id_espacio);
                    cmd.Parameters.AddWithValue("@idUsuario", reserva.id_usuario);

                    cmd.Parameters.AddWithValue("@idGestor",
    (object)reserva.id_gestor ?? DBNull.Value);


                    cmd.Parameters.AddWithValue("@idEstado", reserva.id_estado_reserva);

                    int nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
                    return nuevoId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear reserva: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        // Método para actualizar una reserva
        public bool ActualizarReserva(Reserva reserva)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE reservas 
                                    SET fecha_reserva = @fecha, 
                                        hora_inicio = @horaInicio, 
                                        hora_fin = @horaFin, 
                                        proposito = @proposito, 
                                        id_espacio = @idEspacio,
                                        id_gestor = @idGestor,
                                        id_estado_reserva = @idEstado
                                    WHERE id_reserva = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@fecha", reserva.fecha_reserva);
                    cmd.Parameters.AddWithValue("@horaInicio", reserva.hora_inicio);
                    cmd.Parameters.AddWithValue("@horaFin", reserva.hora_fin);
                    cmd.Parameters.AddWithValue("@proposito", reserva.proposito);
                    cmd.Parameters.AddWithValue("@idEspacio", reserva.id_espacio);
                    cmd.Parameters.AddWithValue("@idGestor", reserva.id_gestor);
                    cmd.Parameters.AddWithValue("@idEstado", reserva.id_estado_reserva);
                    cmd.Parameters.AddWithValue("@id", reserva.id_reserva);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar reserva: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Método para eliminar una reserva
        public bool EliminarReserva(int idReserva)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM reservas WHERE id_reserva = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idReserva);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar reserva: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Método para cambiar el estado de una reserva
        public bool CambiarEstadoReserva(int idReserva, int nuevoEstado, int idGestor)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE reservas 
                                    SET id_estado_reserva = @estado, 
                                        id_gestor = @idGestor 
                                    WHERE id_reserva = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                    cmd.Parameters.AddWithValue("@idGestor", idGestor);
                    cmd.Parameters.AddWithValue("@id", idReserva);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar estado de reserva: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Método para verificar disponibilidad de espacio
        public bool VerificarDisponibilidad(int idEspacio, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, int? idReservaExcluir = null)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT COUNT(*) 
                        FROM reservas 
                        WHERE id_espacio = @idEspacio 
                          AND fecha_reserva = @fecha 
                          AND id_estado_reserva IN (1, 3) -- Aceptada o Pendiente
                          AND (
                            (hora_inicio < @horaFin AND hora_fin > @horaInicio)
                          )";

                    if (idReservaExcluir.HasValue)
                    {
                        query += " AND id_reserva != @idExcluir";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idEspacio", idEspacio);
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    cmd.Parameters.AddWithValue("@horaInicio", horaInicio);
                    cmd.Parameters.AddWithValue("@horaFin", horaFin);

                    if (idReservaExcluir.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@idExcluir", idReservaExcluir.Value);
                    }

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count == 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar disponibilidad: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Método para obtener reservas por usuario
        public List<Reserva> ObtenerReservasPorUsuario(int idUsuario)
        {
            var reservas = new List<Reserva>();

            string query = @"
                SELECT 
                    r.*,
                    e.nombre_espacio,
                    CONCAT(u.nombre1_usuario, ' ', u.ap_usuario) as nombre_usuario,
                    CONCAT(ug.nombre1_usuario, ' ', ug.ap_usuario) as nombre_gestor,
                    er.nombre_estado,
                    rl.tipo_rol as rol_usuario
                FROM reservas r
                LEFT JOIN espacios e ON r.id_espacio = e.id_espacio
                LEFT JOIN usuarios u ON r.id_usuario = u.id_usuario
                LEFT JOIN usuarios ug ON r.id_gestor = ug.id_usuario
                LEFT JOIN estado_reserva er ON r.id_estado_reserva = er.id_estado
                LEFT JOIN roles rl ON u.id_rol = rl.id_rol
                WHERE r.id_usuario = @idUsuario
                ORDER BY r.fecha_reserva DESC, r.hora_inicio DESC";

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var reserva = new Reserva();
                                reserva.id_reserva = reader.GetInt32("id_reserva");
                                reserva.fecha_reserva = reader.GetDateTime("fecha_reserva");
                                reserva.hora_inicio = reader.GetTimeSpan("hora_inicio");
                                reserva.hora_fin = reader.GetTimeSpan("hora_fin");
                                reserva.proposito = reader.GetString("proposito");
                                reserva.fecha_solicitud = reader.GetDateTime("fecha_solicitud");
                                reserva.id_espacio = reader.GetInt32("id_espacio");
                                reserva.id_usuario = reader.GetInt32("id_usuario");

                                reserva.id_gestor = reader.IsDBNull(reader.GetOrdinal("id_gestor"))
    ? (int?)null
    : reader.GetInt32("id_gestor");


                                reserva.id_estado_reserva = reader.GetInt32("id_estado_reserva");

                                if (!reader.IsDBNull(reader.GetOrdinal("nombre_espacio")))
                                    reserva.NombreEspacio = reader.GetString("nombre_espacio");

                                if (!reader.IsDBNull(reader.GetOrdinal("nombre_gestor")))
                                    reserva.NombreGestor = reader.GetString("nombre_gestor");

                                if (!reader.IsDBNull(reader.GetOrdinal("nombre_estado")))
                                    reserva.EstadoReserva = reader.GetString("nombre_estado");

                                reservas.Add(reserva);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener reservas del usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return reservas;
        }

        // Método para obtener todos los estados de reserva
        public List<EstadosReservas> ObtenerEstadosReserva()
        {
            var estados = new List<EstadosReservas>();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM estado_reserva ORDER BY id_estado";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            estados.Add(new EstadosReservas
                            {
                                id_estado = reader.GetInt32("id_estado"),
                                nombre_estado = reader.GetString("nombre_estado")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener estados de reserva: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return estados;
        }
    }
}
