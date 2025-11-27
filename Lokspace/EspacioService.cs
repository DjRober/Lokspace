using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lokspace
{
    internal class EspacioService
    {
        private Database db = new Database();

        // Metodo para obtener todos los espacios
        public List<Espacios> ObtenerTodosEspacios()
        {
            var espacios = new List<Espacios>();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM espacios";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            espacios.Add(new Espacios
                            {
                                id_espacio = reader.GetInt32("id_espacio"),
                                nombre_espacio = reader.GetString("nombre_espacio"),
                                estado_espacio = reader.GetString("estado_espacio"),
                                fecha_registro = reader.GetDateTime("fecha_registro").ToString("yyyy-MM-dd"), // Convertir Date a string
                                capacidad = reader.GetString("capacidad"),
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
    }
}
