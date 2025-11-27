using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lokspace
{
    public partial class EspaciosUserControl : UserControl
    {
        // Lista de espacios (puedes cargarla desde una base de datos después)
        private List<Space> espacios = new List<Space>();

        public EspaciosUserControl()
        {
            InitializeComponent();
            InicializarDatos();
            RenderizarEspacios();
        }

        private void InitializeComponent()
        {
            this.panelContenedor = new Panel();
            this.flowLayoutPanel = new FlowLayoutPanel();
            this.lblTitulo = new Label();

            // 
            // panelContenedor
            // 
            this.panelContenedor.Dock = DockStyle.Fill;
            this.panelContenedor.AutoScroll = true;
            this.panelContenedor.BackColor = Color.White;

            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Dock = DockStyle.Fill;
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.WrapContents = true;
            this.flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            this.flowLayoutPanel.Padding = new Padding(10);

            // 
            // lblTitulo
            // 
            this.lblTitulo.Dock = DockStyle.Top;
            this.lblTitulo.Text = "🏢 Espacios Disponibles";
            this.lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblTitulo.Height = 50;
            this.lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            this.lblTitulo.Padding = new Padding(10, 0, 0, 0);

            // Agregar controles
            this.panelContenedor.Controls.Add(this.flowLayoutPanel);
            this.panelContenedor.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.panelContenedor);
        }

        // Clase Space (la misma que en GestionEspaciosForm)
        public class Space
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
            public int Capacity { get; set; }
            public string Features { get; set; }
            public string ParentId { get; set; }
            public int? Floor { get; set; }

            public string TipoDisplay => Type == "building" ? "🏢 Edificio" :
                                       Type == "classroom" ? "🏫 Aula" :
                                       Type == "sports" ? "⚽ Deportivo" : "❓ Desconocido";

            public string CapacidadDisplay => Capacity > 0 ? Capacity + " personas" : "N/A";
        }

        private void InicializarDatos()
        {
            // Los mismos datos de ejemplo que en GestionEspaciosForm
            espacios = new List<Space>
            {
                new Space { Id = "1", Name = "Edificio Central", Type = "building",
                           Description = "Edificio principal del campus universitario", Capacity = 1000 },
                new Space { Id = "2", Name = "Aula 101", Type = "classroom",
                           Description = "Aula equipada con proyector y aire acondicionado", Capacity = 40 },
                new Space { Id = "3", Name = "Aula 201", Type = "classroom",
                           Description = "Aula para clases magistrales", Capacity = 35 },
                new Space { Id = "4", Name = "Cancha de Fútbol", Type = "sports",
                           Description = "Cancha profesional de fútbol 11", Capacity = 200 },
                new Space { Id = "5", Name = "Edificio de Deportes", Type = "building",
                           Description = "Complejo deportivo universitario" },
                new Space { Id = "6", Name = "Gimnasio Principal", Type = "sports",
                           Description = "Gimnasio con equipamiento profesional", Capacity = 100 }
            };
        }

        private void RenderizarEspacios()
        {
            flowLayoutPanel.SuspendLayout();
            flowLayoutPanel.Controls.Clear();

            foreach (var espacio in espacios)
            {
                var card = CrearCardEspacio(espacio);
                flowLayoutPanel.Controls.Add(card);
            }

            flowLayoutPanel.ResumeLayout();
        }

        private Panel CrearCardEspacio(Space espacio)
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
                Text = espacio.Name,
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

            // Descripción
            var lblDescripcion = new Label
            {
                Text = espacio.Description,
                Font = new Font("Segoe UI", 9),
                Location = new Point(15, 90),
                Size = new Size(250, 60),
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            // Agregar controles a la card
            card.Controls.Add(lblNombre);
            card.Controls.Add(lblTipo);
            card.Controls.Add(lblCapacidad);
            card.Controls.Add(lblDescripcion);

            return card;
        }

        // Componentes
        private Panel panelContenedor;
        private FlowLayoutPanel flowLayoutPanel;
        private Label lblTitulo;
    }
}


