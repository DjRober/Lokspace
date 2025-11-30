using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Lokspace
{
    public partial class GestionEspaciosForm : Form
    {
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
            public string CapacidadDisplay => Capacity > 0 ? $"{Capacity} personas" : "N/A";
        }

        private List<Space> espacios;
        private BindingList<Space> bindingList;
        private BindingSource bindingSource;
        private DataGridView dgvEspacios;
        private Panel panelContenidoInterno;
        private FlowLayoutPanel flowCards;
        private bool usingCards = false;
        private Panel panelEstadisticas;

        public GestionEspaciosForm()
        {
            InicializarDatos();
            ConfigurarInterfaz();
            ConfigurarDataGridView();
            ConfigurarFlowCards();
            this.Shown += (s, e) => EnsureGridBound();
        }

        private void InicializarDatos()
        {
            espacios = new List<Space>
            {
                new Space { Id = "1", Name = "Edificio Central", Type = "building", Description = "Edificio principal", Capacity = 1000 },
                new Space { Id = "2", Name = "Aula 101", Type = "classroom", ParentId = "1", Floor = 1, Capacity = 40 },
                new Space { Id = "3", Name = "Aula 201", Type = "classroom", ParentId = "1", Floor = 2, Capacity = 35 },
                new Space { Id = "4", Name = "Cancha de Fútbol", Type = "sports", Capacity = 200 },
                new Space { Id = "5", Name = "Edificio de Deportes", Type = "building" },
                new Space { Id = "6", Name = "Gimnasio Principal", Type = "sports", ParentId = "5", Capacity = 100 }
            };
        }

        private void ConfigurarInterfaz()
        {
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Padding = new Padding(12);

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            layout.Controls.Add(CrearHeader(), 0, 0);
            layout.Controls.Add(CrearPanelEstadisticas(), 0, 1);
            layout.Controls.Add(CrearPanelContenido(), 0, 2);

            this.Controls.Add(layout);
        }

        private Panel CrearHeader()
        {
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            var title = new Label
            {
                Text = "Administrador de Espacios Universitarios",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(6, 10),
                AutoSize = true
            };

            var btnAgregar = new Button
            {
                Text = "➕ Agregar Espacio",
                Size = new Size(160, 36),
                Location = new Point(820, 28),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAgregar.Click += (s, e) => AgregarEspacio();

            var btnToggleView = new Button
            {
                Text = usingCards ? "📋 Vista Tabla" : "🃏 Vista Tarjetas",
                Size = new Size(120, 28),
                Location = new Point(660, 30)
            };
            btnToggleView.Click += (s, e) => {
                usingCards = !usingCards;
                btnToggleView.Text = usingCards ? "📋 Vista Tabla" : "🃏 Vista Tarjetas";
                RenderCurrentView();
            };

            panel.Controls.AddRange(new Control[] { title, btnAgregar, btnToggleView });
            return panel;
        }

        private Panel CrearPanelEstadisticas()
        {
            panelEstadisticas = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            return panelEstadisticas;
        }

        private Panel CrearPanelContenido()
        {
            panelContenidoInterno = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            return panelContenidoInterno;
        }

        private void ConfigurarDataGridView()
        {
            dgvEspacios = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoGenerateColumns = false,
                ScrollBars = ScrollBars.Vertical,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowTemplate = { Height = 34 }
            };

            ConfigurarColumnasDataGrid();
            dgvEspacios.CellClick += ManejarClickCelda;

            panelContenidoInterno.Controls.Add(dgvEspacios);
        }

        private void ConfigurarColumnasDataGrid()
        {
            dgvEspacios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Nombre",
                DataPropertyName = "Name"
            });

            dgvEspacios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TipoDisplay",
                HeaderText = "Tipo",
                DataPropertyName = "TipoDisplay",
                Width = 120
            });

            dgvEspacios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CapacidadDisplay",
                HeaderText = "Capacidad",
                DataPropertyName = "CapacidadDisplay",
                Width = 110
            });

            dgvEspacios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Descripción",
                DataPropertyName = "Description"
            });

            dgvEspacios.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Editar",
                HeaderText = "",
                Text = "Editar",
                UseColumnTextForButtonValue = true,
                Width = 80
            });

            dgvEspacios.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Eliminar",
                HeaderText = "",
                Text = "Eliminar",
                UseColumnTextForButtonValue = true,
                Width = 80
            });
        }

        private void ConfigurarFlowCards()
        {
            flowCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                BackColor = Color.White,
                Padding = new Padding(8)
            };
            panelContenidoInterno.Controls.Add(flowCards);
        }

        private void RenderCurrentView()
        {
            dgvEspacios.Visible = !usingCards;
            flowCards.Visible = usingCards;

            if (usingCards)
                RenderCards();
            else
                dgvEspacios.Refresh();

            ActualizarEstadisticas();
        }

        private void RenderCards()
        {
            flowCards.SuspendLayout();
            flowCards.Controls.Clear();

            foreach (var espacio in bindingList)
            {
                flowCards.Controls.Add(CrearCard(espacio));
            }

            flowCards.ResumeLayout();
        }

        private Panel CrearCard(Space espacio)
        {
            var card = new Panel
            {
                Size = new Size(260, 140),
                Margin = new Padding(8),
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            card.Controls.Add(new Label
            {
                Text = espacio.Name,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(180, 20)
            });

            card.Controls.Add(new Label
            {
                Text = espacio.TipoDisplay,
                Location = new Point(10, 36),
                AutoSize = true
            });

            card.Controls.Add(new Label
            {
                Text = espacio.CapacidadDisplay,
                Location = new Point(10, 56),
                AutoSize = true
            });

            card.Controls.Add(new Label
            {
                Text = espacio.Description ?? "",
                Location = new Point(10, 76),
                Size = new Size(180, 40),
                AutoEllipsis = true
            });

            var btnEditar = new Button
            {
                Text = "Editar",
                Size = new Size(60, 26),
                Location = new Point(200, 10),
                Tag = espacio
            };
            btnEditar.Click += (s, e) => EditarEspacio((Space)((Button)s).Tag);

            var btnEliminar = new Button
            {
                Text = "Eliminar",
                Size = new Size(60, 26),
                Location = new Point(200, 42),
                Tag = espacio
            };
            btnEliminar.Click += (s, e) => EliminarEspacio((Space)((Button)s).Tag);

            card.Controls.Add(btnEditar);
            card.Controls.Add(btnEliminar);

            return card;
        }

        private void ActualizarEstadisticas()
        {
            panelEstadisticas.Controls.Clear();

            // CORRECCIÓN: Usar la lista correcta según el contexto
            var datos = bindingList != null ? bindingList : espacios.AsEnumerable();

            var stats = new[]
            {
                ("Total Espacios", datos.Count()),
                ("Edificios", datos.Count(s => s.Type == "building")),
                ("Aulas", datos.Count(s => s.Type == "classroom")),
                ("Espacios Deportivos", datos.Count(s => s.Type == "sports"))
            };

            int x = 6;
            foreach (var (texto, valor) in stats)
            {
                panelEstadisticas.Controls.Add(CrearCardEstadistica(texto, valor, x, 220));
                x += 232;
            }
        }

        private Panel CrearCardEstadistica(string texto, int valor, int x, int width)
        {
            var card = new Panel
            {
                Size = new Size(width, 80),
                Location = new Point(x, 12),
                BackColor = Color.FromArgb(248, 250, 252),
                BorderStyle = BorderStyle.FixedSingle
            };

            card.Controls.Add(new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(10, 8),
                AutoSize = true
            });

            card.Controls.Add(new Label
            {
                Text = valor.ToString(),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30),
                Location = new Point(10, 34),
                AutoSize = true
            });

            return card;
        }

        private void AgregarEspacio()
        {
            var nuevo = new Space
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Nuevo espacio",
                Type = "classroom",
                Capacity = 0
            };

            if (MostrarEditor(nuevo))
            {
                bindingList.Add(nuevo);
            }
        }

        private void EditarEspacio(Space espacio)
        {
            if (MostrarEditor(espacio))
            {
                bindingSource.ResetBindings(false);
                RenderCurrentView();
            }
        }

        private void EliminarEspacio(Space espacio)
        {
            if (MessageBox.Show($"¿Eliminar {espacio.Name}?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bindingList.Remove(espacio);
            }
        }

        private void ManejarClickCelda(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= bindingList.Count) return;

            var espacio = bindingList[e.RowIndex];
            var colName = dgvEspacios.Columns[e.ColumnIndex].Name;

            if (colName == "Editar")
            {
                EditarEspacio(espacio);
            }
            else if (colName == "Eliminar")
            {
                EliminarEspacio(espacio);
            }
        }

        public void EnsureGridBound()
        {
            if (bindingSource != null) return;

            bindingList = new BindingList<Space>(espacios);
            bindingSource = new BindingSource { DataSource = bindingList };
            dgvEspacios.DataSource = bindingSource;

            bindingList.ListChanged += (s, e) => RenderCurrentView();
            RenderCurrentView();
        }

        private bool MostrarEditor(Space espacio)
        {
            using (var form = new Form())
            {
                form.Text = "Editar espacio";
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.Size = new Size(460, 360);
                form.MaximizeBox = form.MinimizeBox = false;

                var controles = new[]
                {
                    CrearControlEditor("Nombre:", new TextBox { Text = espacio.Name, Width = 320 }, 10),
                    CrearControlEditor("Tipo:", CrearComboTipo(espacio.Type), 50),
                    CrearControlEditor("Capacidad:", new NumericUpDown { Value = Math.Max(0, espacio.Capacity), Width = 120 }, 90),
                    CrearControlEditor("Descripción:", new TextBox { Text = espacio.Description, Multiline = true, Height = 100, ScrollBars = ScrollBars.Vertical, Width = 320 }, 130)
                };

                var btnOk = new Button { Text = "Guardar", DialogResult = DialogResult.OK, Location = new Point(240, 250), Size = new Size(90, 30) };
                var btnCancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(340, 250), Size = new Size(90, 30) };

                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;
                form.Controls.AddRange(controles);
                form.Controls.Add(btnOk);
                form.Controls.Add(btnCancel);

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    espacio.Name = ((TextBox)controles[0].Controls[1]).Text.Trim();
                    espacio.Type = ((ComboBox)controles[1].Controls[1]).SelectedItem?.ToString() ?? "classroom";
                    espacio.Capacity = (int)((NumericUpDown)controles[2].Controls[1]).Value;
                    espacio.Description = ((TextBox)controles[3].Controls[1]).Text.Trim();
                    return true;
                }
                return false;
            }
        }

        private Panel CrearControlEditor(string label, Control control, int y)
        {
            var panel = new Panel { Location = new Point(10, y), Size = new Size(430, 30) };
            panel.Controls.Add(new Label { Text = label, AutoSize = true, Location = new Point(0, 5) });
            panel.Controls.Add(control);
            control.Location = new Point(100, 0);
            return panel;
        }

        private ComboBox CrearComboTipo(string tipoActual)
        {
            var combo = new ComboBox { Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            combo.Items.AddRange(new[] { "building", "classroom", "sports" });
            combo.SelectedItem = tipoActual ?? "classroom";
            return combo;
        }
    }
}