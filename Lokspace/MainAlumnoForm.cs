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
    public partial class MainAlumnoForm : Form
    {
        private Usuario usuario;
        private Form formularioActivo;

        public MainAlumnoForm(Usuario usuario)
        {
            this.usuario = usuario;
            InitializeComponent();
            MostrarEspaciosEnPanel(); // Mostrar espacios reales al iniciar
        }

        private void MainAlumnoForm_Load(object sender, EventArgs e)
        {
            // El contenido ya se carga en el constructor
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Estás seguro de que deseas cerrar sesión?",
                                       "Confirmar cierre de sesión",
                                       MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                var loginForm = new Login();
                loginForm.Show();
            }
        }

        private void MostrarEspaciosEnPanel()
        {
            // Limpiar el panel
            panel1.Controls.Clear();

            // Crear y agregar el UserControl de espacios (que ahora usa datos reales)
            var espaciosUC = new EspaciosUserControles();
            espaciosUC.Dock = DockStyle.Fill;

            panel1.Controls.Add(espaciosUC);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Método del evento paint del panel
        }
    }
}