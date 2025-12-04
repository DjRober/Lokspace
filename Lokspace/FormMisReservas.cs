using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lokspace
{
    public partial class FormMisReservas : Form
    {
        private int usuarioId;
        public FormMisReservas(int idUsuario)
        {
            InitializeComponent();
            usuarioId = idUsuario;
            CargarReservas();
        }
        private void CargarReservas()
        {
            Database db = new Database();
            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = @"SELECT r.id_reserva AS 'Código',
                                            r.fecha_reserva AS 'Fecha',
                                            r.hora_inicio AS 'Inicio',
                                            r.hora_fin AS 'Fin',
                                            r.proposito AS 'Propósito',
                                            r.fecha_solicitud AS 'Solicitud',
                                            e.nombre_espacio AS 'Espacio',
                                            es.nombre_estado AS 'Estado'
                                     FROM reservas r
                                     INNER JOIN espacios e 
                                         ON r.id_espacio = e.id_espacio
                                     INNER JOIN estado_reserva es 
                                         ON r.id_estado_reserva = es.id_estado
                                     WHERE r.id_usuario = @idUsuario";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idUsuario", usuarioId);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvReservas.DataSource = dt;
                    dgvReservas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar reservas: " + ex.Message);
                }
            }
        }
        private void FormMisReservas_Load(object sender, EventArgs e)
        {

        }

        private void regresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
