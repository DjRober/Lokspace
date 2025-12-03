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
    public partial class ReservasPersonalesDocente : Form
    {
        private Usuario docente; //objeto para guardar informacion del docente logueado

        private ReservaService reservaService = new ReservaService(); //servicio para interactuar con las bds

        public ReservasPersonalesDocente(Usuario docente)
        {
            InitializeComponent();
            this.docente = docente; //asigna el usuario q se paso desde el MainDocente
        }


        private void ReservasPersonalesDocente_Load(object sender, EventArgs e)
        {
            CargarReservas(); //carga las reservas del docente al iniciar el formulario
        }


        //metodo para cargar y mostrar reservas en el dataGridView
        private void CargarReservas()
        {
            try
            {
                List<Reserva> reservas = reservaService.ObtenerReservasPorUsuario(this.docente.id_usuario);

                listaReservasDocente.DataSource = reservas;

                //ocultar columnas q son id
                listaReservasDocente.Columns["id_reserva"].Visible = false;
                listaReservasDocente.Columns["id_espacio"].Visible = false;
                listaReservasDocente.Columns["id_estado_reserva"].Visible = false;
                listaReservasDocente.Columns["id_usuario"].Visible = false;
                listaReservasDocente.Columns["id_gestor"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro al cargar reservas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnNuevaReserva_Click(object sender, EventArgs e)
        {
            using (var formNuevaReserva = new NuevaReservaDocente(this.docente.id_usuario))
            {
                if(formNuevaReserva.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Reserva creada con exito.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarReservas(); //carga nuevamente la lista para mostrar la nueva reserva
                }
            }
        }

        private void listaReservasDocente_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnCancelarReserva_Click(object sender, EventArgs e)
        {
            //verifica si hay una fila seleccionada en el dataGriedView
            if (listaReservasDocente.SelectedRows.Count > 0)
            {
                Reserva reservaSeleccionada = listaReservasDocente.SelectedRows[0].DataBoundItem as Reserva; //obtiene la reserva seleccionada

                if (reservaSeleccionada != null)
                {
                    DialogResult result = MessageBox.Show($"Esta seguro de cancelar la reserva?");

                    if (result == DialogResult.OK)
                    {
                        //llama al servicio para cambiar el estado
                        int idEstadoCancelado = 200; //valor de 200 = id de "cancelada" en la bds

                        if (reservaService.CancelarReservaPorUsuario(reservaSeleccionada.id_reserva, idEstadoCancelado, this.docente.id_usuario))
                        {
                            MessageBox.Show("Reserva cancelada con exito.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargarReservas(); //nuevamente cargar la lista (datagridview) para actualizar el cambio
                        }
                        else
                        {
                            MessageBox.Show("Error al cancelar la reserva.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione una reserva para cancelar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
