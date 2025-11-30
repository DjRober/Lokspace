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
    public partial class MainDocenteForm : Form
    {
        private Usuario usuario;

        private Form formularioActivo;

        public MainDocenteForm(Usuario usuario)
        {
            this.usuario = usuario;
            InitializeComponent();
        }


        private void MostrarFormularioEnPanel(Form formulario)
        {
            try
            {
                // Cerrar formulario anterior si existe
                if (formularioActivo != null && !formularioActivo.IsDisposed)
                {
                    formularioActivo.Close();
                    formularioActivo.Dispose();
                }

                this.formularioActivo = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cerrando formulario: {ex.Message}");
            }

            // Configurar nuevo formulario
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;

            // Limpiar panel y agregar nuevo formulario
            panelDocente.Controls.Clear();
            panelDocente.Controls.Add(formulario);

            // Mostrar y guardar referencia
            formulario.Show();
            this.formularioActivo = formulario;
        }


        private void MainDocenteForm_Load(object sender, EventArgs e)
        {
            //Muestra la vista de Reservas del Docente al iniciar
            MostrarFormularioEnPanel(new ReservasPersonalesDocente());
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnReservas_Click(object sender, EventArgs e)
        {
            MostrarFormularioEnPanel(new ReservasPersonalesDocente());
        }

        private void btnEspacios_Click(object sender, EventArgs e)
        {
            MostrarFormularioEnPanel(new SolicitudesPendientesDocente());
        }

        private void btnCalendario_Click(object sender, EventArgs e)
        {

        }

        private void btnNuevaReserva_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panelDocente_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
