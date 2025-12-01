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
    public partial class FormReservaAlumno : Form
    {
        private int idEspacioSeleccionado;
        private ReservaService reservaService = new ReservaService();

        public int IdUsuarioActual { get; set; } // asignas desde tu login

        public FormReservaAlumno(int idEspacio)
        {
            InitializeComponent();
            idEspacioSeleccionado = idEspacio;
        }

        private void FormReservaAlumno_Load(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            try
            {
                // 1. Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(txtProposito.Text))
                {
                    MessageBox.Show("Debe ingresar un propósito para la reserva.",
                                    "Campo requerido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                // 2. Construir objeto Reserva
                Reserva nuevaReserva = new Reserva
                {
                    fecha_reserva = dtpFechaReserva.Value.Date,
                    hora_inicio = dtpHoraInicio.Value.TimeOfDay,
                    hora_fin = dtpHoraFin.Value.TimeOfDay,
                    proposito = txtProposito.Text.Trim(),
                    fecha_solicitud = DateTime.Today,

                    id_espacio = idEspacioSeleccionado,
                    id_usuario = IdUsuarioActual,
                    id_gestor = null,              // si aún no hay gestor
                    id_estado_reserva = 3

                };

                // 3. Guardar en base de datos
                ReservaService service = new ReservaService();
                int idGenerado = service.CrearReserva(nuevaReserva);

                if (idGenerado > 0)
                {
                    MessageBox.Show("La reserva ha sido registrada correctamente.",
                                    "Reserva creada",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    this.Close(); // Si quieres cerrar el formulario después
                }
                else
                {
                    MessageBox.Show("No se pudo guardar la reserva. Intente nuevamente.",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

    }
}


