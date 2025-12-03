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
    public partial class NuevaReservaDocente : Form
    {
        private int id_docente;
        private ReservaService reservaService = new ReservaService();
        private EspacioService espacioService = new EspacioService();

        public NuevaReservaDocente(int id_docente)
        {
            InitializeComponent();
            this.id_docente = id_docente;
        }

        private void NuevaReservaDocente_Load(object sender, EventArgs e)
        {
            //cambiar formato de tiempo de fecha a hora en los dtp HoraInicio y HoraFin
            dtpHoraInicio.Format = DateTimePickerFormat.Custom;
            dtpHoraInicio.ShowUpDown = true;

            dtpHoraFin.Format = DateTimePickerFormat.Custom;
            dtpHoraFin.ShowUpDown = true;

            CargarEspacios();
        }


        private void CargarEspacios()
        {
            try
            {
                List<Espacio> espacios = espacioService.ObtenerTodosEspacios();

                //si la lista de espacios esta vacia muestra una advertancia
                if (espacios == null || espacios.Count == 0)
                {
                    MessageBox.Show("No se encontraron espacios disponibles para reservar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                cmbEspacio.DataSource = espacios;
                cmbEspacio.DisplayMember = "NombreEspacio"; //propiedad para mostrar
                cmbEspacio.ValueMember = "IdEspacio";       //propiedad para usar como valor

                cmbEspacio.SelectedIndex = -1; //no selecciona ninguno por defecto
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar espacios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnCancelar_Click(object sender, EventArgs e)
        {
            //establece el resultado del dialogo y cierra el formulario
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void txtCampoFecha_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCampoHoraInicio_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCampoHoraFin_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCampoProposito_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnNuvaReserva_Click(object sender, EventArgs e)
        {
            int idEspacioSeleccionado;
            //recoleccion y validacion de datps
            if (cmbEspacio.SelectedValue == null || !int.TryParse(cmbEspacio.SelectedValue.ToString(), out idEspacioSeleccionado))  
            {
                MessageBox.Show("Seleccione un espacio", "Error de Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCampoProposito.Text))
            {
                MessageBox.Show("Ingrese el proposito de la reserva", "Error de Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime fechaReserva = dtpFecha.Value.Date;
            TimeSpan horaInicio = dtpHoraInicio.Value.TimeOfDay; 
            TimeSpan horaFin = dtpHoraFin.Value.TimeOfDay;      
            
            if (horaInicio >= horaFin)
            {
                MessageBox.Show("La hora de inicio debe ser anterior a la hora de fin", "Error de Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            //creacion del objeto reserva
            Reserva nuevaReserva = new Reserva
            {
                fecha_reserva = fechaReserva,
                hora_inicio = horaInicio,
                hora_fin = horaFin,
                id_espacio = idEspacioSeleccionado,
                proposito = txtCampoProposito.Text,
                fecha_solicitud = DateTime.Now,
                id_usuario = this.id_docente, //id del docente que hace la reserva
                id_gestor = null,
                id_estado_reserva = 100
            };
            


            //guardar la reserva
            int idNuevaReserva = reservaService.CrearReserva(nuevaReserva);

            if (idNuevaReserva > 0)
            {
                //si la reserva se creo devuelve un id > 0
                this.DialogResult = DialogResult.OK; 
                this.Close();
            }
            else
            {
                MessageBox.Show("No se pudo crear la reserva. Revise los datos e intente de nuevo.", "Error al Guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
            }
            

        }

        private void cmbEspacio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpHoraInicio_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpHoraFin_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
