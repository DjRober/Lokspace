using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Servicios del usuario utilizado para conectar con la base de datos
// para modificar, agregar, eliminar y solicitar usuarios de la base de datos

namespace Lokspace
{
    internal class UsuarioService
    {
        private Database db = new Database();

        // Metodo para autenticar al usuario
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
            } catch(Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}");
                return null;
            }
        }

        // Metodo para registrar un nuevo usuario
        // Pendiente

        // Metodo para actualizar la informacion del usuario
        // Pendiente

        // Metodo para eliminar un usuario
        // Pendiente

        // Metodo para obtener todos los usuarios
        public List<Usuario> ObtenerTodosUsuarios()
        {
            var usuarios = new List<Usuario>();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM usuarios";
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
    }
}
