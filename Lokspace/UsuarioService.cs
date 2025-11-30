using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Lokspace
{
    internal class UsuarioService
    {
        private Database db = new Database();

        // Método para autenticar al usuario
        // Retorna un objeto Usuario si las credenciales son correctas, de lo contrario retorna null
        public Usuario Login(string email, string password)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT id_usuario, nombre1_usuario, nombre2_usuario, ap_usuario, am_usuario, email, password, id_rol
                                     FROM usuarios
                                     WHERE email = @email AND password = @password";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Usuario usuario = new Usuario
                            {
                                id_usuario = reader.GetInt32("id_usuario"),
                                nombre1_usuario = reader.GetString("nombre1_usuario"),
                                nombre2_usuario = reader.IsDBNull(reader.GetOrdinal("nombre2_usuario")) ? null : reader.GetString("nombre2_usuario"),
                                ap_usuario = reader.GetString("ap_usuario"),
                                am_usuario = reader.IsDBNull(reader.GetOrdinal("am_usuario")) ? null : reader.GetString("am_usuario"),
                                email = reader.GetString("email"),
                                password = reader.GetString("password"),
                                id_rol = reader.GetInt32("id_rol")
                            };
                            return usuario;
                        }
                        else
                        {
                            return null; // User not found
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}");
                return null;
            }
        }

        // Método para registrar un nuevo usuario
        // Retorna el ID del nuevo usuario si es exitoso, de lo contrario retorna -1
        public int RegistrarUsuario(Usuario usuario)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO usuarios 
                                    (nombre1_usuario, nombre2_usuario, ap_usuario, am_usuario, email, password, id_rol) 
                                    VALUES (@nombre1, @nombre2, @apellidoPaterno, @apellidoMaterno, @email, @password, @id_rol);
                                    SELECT LAST_INSERT_ID();";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre1", usuario.nombre1_usuario);
                    cmd.Parameters.AddWithValue("@nombre2", usuario.nombre2_usuario ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@apellidoPaterno", usuario.ap_usuario);
                    cmd.Parameters.AddWithValue("@apellidoMaterno", usuario.am_usuario ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@email", usuario.email);
                    cmd.Parameters.AddWithValue("@password", usuario.password);
                    cmd.Parameters.AddWithValue("@id_rol", usuario.id_rol);

                    // Ejecuta el insert y obtiene el ID generado
                    int nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
                    return nuevoId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar usuario: {ex.Message}");
                return -1;
            }
        }

        // Método para actualizar la información del usuario
        // Retorna true si la actualización fue exitosa, false en caso contrario
        public bool ActualizarUsuario(Usuario usuario)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE usuarios 
                                    SET nombre1_usuario = @nombre1, 
                                        nombre2_usuario = @nombre2, 
                                        ap_usuario = @apellidoPaterno, 
                                        am_usuario = @apellidoMaterno, 
                                        email = @email, 
                                        password = @password, 
                                        id_rol = @id_rol 
                                    WHERE id_usuario = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre1", usuario.nombre1_usuario);
                    cmd.Parameters.AddWithValue("@nombre2", usuario.nombre2_usuario ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@apellidoPaterno", usuario.ap_usuario);
                    cmd.Parameters.AddWithValue("@apellidoMaterno", usuario.am_usuario ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@email", usuario.email);
                    cmd.Parameters.AddWithValue("@password", usuario.password);
                    cmd.Parameters.AddWithValue("@id_rol", usuario.id_rol);
                    cmd.Parameters.AddWithValue("@id", usuario.id_usuario);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar usuario: {ex.Message}");
                return false;
            }
        }

        // Método para eliminar un usuario
        // Retorna true si la eliminación fue exitosa, false en caso contrario
        public bool EliminarUsuario(int idUsuario)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM usuarios WHERE id_usuario = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idUsuario);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar usuario: {ex.Message}");
                return false;
            }
        }

        // Método para obtener todos los usuarios
        public List<Usuario> ObtenerTodosUsuarios()
        {
            var usuarios = new List<Usuario>();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM usuarios ORDER BY id_usuario";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new Usuario
                            {
                                id_usuario = reader.GetInt32("id_usuario"),
                                nombre1_usuario = reader.GetString("nombre1_usuario"),
                                nombre2_usuario = reader.IsDBNull(reader.GetOrdinal("nombre2_usuario")) ? null : reader.GetString("nombre2_usuario"),
                                ap_usuario = reader.GetString("ap_usuario"),
                                am_usuario = reader.IsDBNull(reader.GetOrdinal("am_usuario")) ? null : reader.GetString("am_usuario"),
                                email = reader.GetString("email"),
                                password = reader.GetString("password"),
                                id_rol = reader.GetInt32("id_rol")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener usuarios: {ex.Message}");
            }

            return usuarios;
        }

        // Método para obtener un usuario por su ID
        // Retorna el usuario si lo encuentra, de lo contrario retorna null
        public Usuario ObtenerUsuarioPorId(int idUsuario)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM usuarios WHERE id_usuario = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idUsuario);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                id_usuario = reader.GetInt32("id_usuario"),
                                nombre1_usuario = reader.GetString("nombre1_usuario"),
                                nombre2_usuario = reader.IsDBNull(reader.GetOrdinal("nombre2_usuario")) ? null : reader.GetString("nombre2_usuario"),
                                ap_usuario = reader.GetString("ap_usuario"),
                                am_usuario = reader.IsDBNull(reader.GetOrdinal("am_usuario")) ? null : reader.GetString("am_usuario"),
                                email = reader.GetString("email"),
                                password = reader.GetString("password"),
                                id_rol = reader.GetInt32("id_rol")
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener usuario: {ex.Message}");
            }

            return null;
        }

        // Método para verificar si un email ya existe en la base de datos
        // Útil para evitar duplicados al registrar nuevos usuarios
        public bool VerificarEmailExistente(string email, int? idUsuarioExcluir = null)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM usuarios WHERE email = @email";

                    if (idUsuarioExcluir.HasValue)
                    {
                        query += " AND id_usuario != @idExcluir";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@email", email);

                    if (idUsuarioExcluir.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@idExcluir", idUsuarioExcluir.Value);
                    }

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar email: {ex.Message}");
                return false;
            }
        }

        // Método para cambiar la contraseña de un usuario
        public bool CambiarPassword(int idUsuario, string nuevaPassword)
        {
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE usuarios SET password = @password WHERE id_usuario = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@password", nuevaPassword);
                    cmd.Parameters.AddWithValue("@id", idUsuario);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar contraseña: {ex.Message}");
                return false;
            }
        }

        // Método para obtener usuarios por rol
        public List<Usuario> ObtenerUsuariosPorRol(int idRol)
        {
            var usuarios = new List<Usuario>();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM usuarios WHERE id_rol = @idRol ORDER BY ap_usuario, nombre1_usuario";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idRol", idRol);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new Usuario
                            {
                                id_usuario = reader.GetInt32("id_usuario"),
                                nombre1_usuario = reader.GetString("nombre1_usuario"),
                                nombre2_usuario = reader.IsDBNull(reader.GetOrdinal("nombre2_usuario")) ? null : reader.GetString("nombre2_usuario"),
                                ap_usuario = reader.GetString("ap_usuario"),
                                am_usuario = reader.IsDBNull(reader.GetOrdinal("am_usuario")) ? null : reader.GetString("am_usuario"),
                                email = reader.GetString("email"),
                                password = reader.GetString("password"),
                                id_rol = reader.GetInt32("id_rol")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener usuarios por rol: {ex.Message}");
            }

            return usuarios;
        }
    }
}