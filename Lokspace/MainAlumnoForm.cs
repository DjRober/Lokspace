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
            MostrarEspaciosEnPanel(); // Mostrar espacios al iniciar
        }

        // Clase Space para los espacios (igual que en GestionEspaciosForm)
        public class Space
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
            public int Capacity { get; set; }

            public string TipoDisplay => Type == "building" ? "🏢 Edificio" :
                                       Type == "classroom" ? "🏫 Aula" :
                                       Type == "sports" ? "⚽ Deportivo" : "❓ Desconocido";

            public string CapacidadDisplay => Capacity > 0 ? Capacity + " personas" : "N/A";
        }

        private void MainAlumnoForm_Load(object sender, EventArgs e)
        {

        }

        private void BtnCerrarSesion_Click(object sender, EventArgs e)
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

            // Crear FlowLayoutPanel para organizar las cards
            var flowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // Datos de ejemplo de espacios
            var espacios = new List<Space>
            {
                new Space {
                    Id = "1",
                    Name = "Edificio Central",
                    Type = "building",
                    Description = "Edificio principal del campus universitario",
                    Capacity = 1000
                },
                new Space {
                    Id = "2",
                    Name = "Aula 101",
                    Type = "classroom",
                    Description = "Aula equipada con proyector y aire acondicionado",
                    Capacity = 40
                },
                new Space {
                    Id = "3",
                    Name = "Aula 201",
                    Type = "classroom",
                    Description = "Aula para clases magistrales",
                    Capacity = 35
                },
                new Space {
                    Id = "4",
                    Name = "Cancha de Fútbol",
                    Type = "sports",
                    Description = "Cancha profesional de fútbol 11",
                    Capacity = 200
                },
                new Space {
                    Id = "5",
                    Name = "Edificio de Deportes",
                    Type = "building",
                    Description = "Complejo deportivo universitario",
                    Capacity = 0
                },
                new Space {
                    Id = "6",
                    Name = "Gimnasio Principal",
                    Type = "sports",
                    Description = "Gimnasio con equipamiento profesional",
                    Capacity = 100
                }
            };
            // Crear una card por cada espacio
            foreach (var espacio in espacios)
            {
                var card = CrearCardEspacio(espacio);
                flowLayoutPanel.Controls.Add(card);
            }

            // Agregar el FlowLayoutPanel al panel1
            panel1.Controls.Add(flowLayoutPanel);
        }
        private Panel CrearCardEspacio(Space espacio)
        {
            var card = new Panel
            {
                Size = new Size(280, 180),
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

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


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
