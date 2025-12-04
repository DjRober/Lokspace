using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lokspace
{
    public partial class EspaciosUserControles : UserControl
    {
        private List<Espacio> espacios = new List<Espacio>();
        private FlowLayoutPanel flowLayoutPanel;
        private Label lblTitulo;
        private EspacioService espacioService = new EspacioService();
        private int idUsuarioActual;

        public EspaciosUserControles(int idUsuario)
        {
            idUsuarioActual = idUsuario;

            InitializeComponent();
            InicializarComponentesAdicionales();
            CargarEspaciosDesdeBD();
        }

        private void InitializeComponent()
        {
            // Método generado automáticamente por el diseñador
        }

        private void InicializarComponentesAdicionales()
        {
            // Configurar componentes manualmente
            this.flowLayoutPanel = new FlowLayoutPanel();
            this.lblTitulo = new Label();

            // Configurar lblTitulo
            this.lblTitulo.Dock = DockStyle.Top;
            this.lblTitulo.Text = "🏢 Espacios Disponibles";
            this.lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblTitulo.Height = 50;
            this.lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            this.lblTitulo.Padding = new Padding(10, 0, 0, 0);

            // Configurar flowLayoutPanel
            this.flowLayoutPanel.Dock = DockStyle.Fill;
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.WrapContents = true;
            this.flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            this.flowLayoutPanel.Padding = new Padding(10);
            this.flowLayoutPanel.BackColor = Color.White;

            // Agregar controles
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.lblTitulo);
        }

        private void CargarEspaciosDesdeBD()
        {
            try
            {
                // Obtener espacios reales de la base de datos
                espacios = espacioService.ObtenerTodosEspacios();
                RenderizarEspacios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar espacios: {ex.Message}");
                // En caso de error, mostrar datos de ejemplo como fallback
                CargarDatosEjemplo();
            }
        }

        private void CargarDatosEjemplo()
        {
            // Solo como respaldo si hay error con la BD
            espacios = new List<Espacio>
            {
                new Espacio {
                    IdEspacio = 1,
                    NombreEspacio = "Edificio Central",
                    EstadoEspacio = "Activo",
                    FechaRegistro = DateTime.Now,
                    Capacidad = 1000,
                    NombreTipoEspacio = "Edificio"
                },
                new Espacio {
                    IdEspacio = 2,
                    NombreEspacio = "Aula 101",
                    EstadoEspacio = "Activo",
                    FechaRegistro = DateTime.Now,
                    Capacidad = 40,
                    NombreTipoEspacio = "Aula"
                }
            };
            RenderizarEspacios();
        }

        private void RenderizarEspacios()
        {
            flowLayoutPanel.SuspendLayout();
            flowLayoutPanel.Controls.Clear();

            if (espacios.Count == 0)
            {
                var lblMensaje = new Label
                {
                    Text = "No hay espacios disponibles",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.Gray
                };
                flowLayoutPanel.Controls.Add(lblMensaje);
            }
            else
            {
                foreach (var espacio in espacios)
                {
                    var card = CrearCardEspacio(espacio);
                    flowLayoutPanel.Controls.Add(card);
                }
            }

            flowLayoutPanel.ResumeLayout();
        }

        private Panel CrearCardEspacio(Espacio espacio)
        {
            var card = new Panel
            {
                Size = new Size(280, 180),
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            // Efecto hover
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(245, 245, 245);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            // Nombre del espacio
            var lblNombre = new Label
            {
                Text = espacio.NombreEspacio,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = false,
                Size = new Size(250, 25),
                ForeColor = Color.FromArgb(30, 30, 30)
            };

            // Tipo de espacio
            var lblTipo = new Label
            {
                Text = espacio.TipoDisplay,
                Font = new Font("Segoe UI", 9),
                Location = new Point(15, 45),
                AutoSize = true,
                ForeColor = Color.FromArgb(100, 100, 100)
            };

            // Capacidad
            var lblCapacidad = new Label
            {
                Text = $"👥 {espacio.CapacidadDisplay}",
                Font = new Font("Segoe UI", 9),
                Location = new Point(15, 65),
                AutoSize = true,
                ForeColor = Color.FromArgb(100, 100, 100)
            };

            // Descripción (con información de estado y fecha)
            var lblDescripcion = new Label
            {
                Text = $"Estado: {espacio.EstadoEspacio}\nRegistrado: {espacio.FechaRegistro:dd/MM/yyyy}",
                Font = new Font("Segoe UI", 8),
                Location = new Point(15, 90),
                Size = new Size(250, 60),
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            // Agregar controles a la card
            card.Controls.Add(lblNombre);
            card.Controls.Add(lblTipo);
            card.Controls.Add(lblCapacidad);
            card.Controls.Add(lblDescripcion);

            // SI EL ESPACIO ESTÁ DISPONIBLE, AGREGAR BOTÓN RESERVAR
            if (espacio.EstadoEspacio.ToLower() == "disponible")
            {
                Button btnReservar = new Button()
                {
                    Text = "Reservar",
                    BackColor = Color.FromArgb(46, 204, 113),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Width = 120,
                    Height = 30,
                    Cursor = Cursors.Hand,
                    Location = new Point(65, card.Height - 35)
                };

                btnReservar.FlatAppearance.BorderSize = 0;

                btnReservar.Click += (s, e) =>
                {
                    FormReservaAlumno f = new FormReservaAlumno(espacio.IdEspacio);
                    f.IdUsuarioActual = idUsuarioActual;
                    f.ShowDialog();
                };

                card.Controls.Add(btnReservar);
            }
            else
            {
                Label lblOcupado = new Label()
                {
                    Text = "Ocupado",
                    ForeColor = Color.Red,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(85, card.Height - 35)
                };

                card.Controls.Add(lblOcupado);
            }



            return card;
        }
    }
}
