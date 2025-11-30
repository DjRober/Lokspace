using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lokspace
{
    public partial class MainAdminForm : Form
    {
        private Usuario usuario;
        private Form formularioActivo;

        public MainAdminForm(Usuario usuario)
        {
            this.usuario = usuario;
            InitializeComponent();
            MostrarDashboardInicial();
            
        }

        private void MostrarDashboardInicial()
        {
            MostrarFormularioEnPanel(new GestionCuentasForm());
            ResaltarBotonActivo(btnGestionCuentas);
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
            panelContenido.Controls.Clear();
            panelContenido.Controls.Add(formulario);

            // Mostrar y guardar referencia
            formulario.Show();
            formularioActivo = formulario;
        }

        private void ResaltarBotonActivo(Button botonActivo)
        {
            // Lista de todos los botones del menu
            var botones = new[] { btnGestionCuentas, btnGestionEspacios, btnGestionReservas};

            foreach (var btn in botones)
            {
                if (btn == botonActivo)
                {
                    btn.BackColor = Color.LightBlue; // Color resaltado
                    btn.ForeColor = Color.White;
                }
                else
                {
                    btn.BackColor = SystemColors.Control; // Color por defecto
                    btn.ForeColor = SystemColors.ControlText;
                }
            }
        }

        private void MainAdminForm_Load(object sender, EventArgs e)
        {

        }

        // Botones del panel de control (Panel lateral)
        private void btnGestionCuentas_Click(object sender, EventArgs e)
        {
            MostrarFormularioEnPanel(new GestionCuentasForm());
            ResaltarBotonActivo((Button)sender);
        }

        private void btnGestionEspacios_Click(object sender, EventArgs e)
        {
            MostrarFormularioEnPanel(new GestionEspaciosForm());
            ResaltarBotonActivo((Button)sender);
        }

        private void btnGestionReservas_Click(object sender, EventArgs e)
        {
            MostrarFormularioEnPanel(new GestionReservasForm());
            ResaltarBotonActivo((Button)sender);
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Estás seguro de que deseas cerrar sesión?", "Confirmar cierre de sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                var loginForm = new Login();
                loginForm.Show();
            }

        }

        private void panelContenido_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelLateral_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
