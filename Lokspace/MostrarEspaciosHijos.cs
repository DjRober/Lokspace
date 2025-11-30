using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Lokspace
{
    public partial class MostrarEspaciosHijos : Form
    {
        private EspacioPadre espacioPadre;
        private List<EspacioHijo> espaciosHijos;
        private BindingList<EspacioHijo> bindingList;
        private BindingSource bindingSource;
        private DataGridView dgvEspaciosHijos;
        private Panel panelContenidoInterno;
        private FlowLayoutPanel flowCards;
        private bool usingCards = false;
        private Panel panelEstadisticas;
        private EspacioService espacioService = new EspacioService();

        public MostrarEspaciosHijos(EspacioPadre espacioPadre)
        {
            this.espacioPadre = espacioPadre;
            InicializarDatos();
            ConfigurarInterfaz();
            ConfigurarDataGridView();
            ConfigurarFlowCards();
            this.Shown += (s, e) => EnsureGridBound();
        }

        private void InicializarDatos()
        {
            // Obtener los espacios hijos del espacio padre actual
            espaciosHijos = espacioService.ObtenerEspaciosPorTipoR(espacioPadre.id_tipo_espacio);
        }

        private void ConfigurarInterfaz()
        {
            this.Text = $"Espacios: {espacioPadre.nombre_tipo}";
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
                Text = $"Espacios: {espacioPadre.nombre_tipo}",
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
            btnAgregar.Click += (s, e) => AgregarEspacioHijo();

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
            dgvEspaciosHijos = new DataGridView
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
            dgvEspaciosHijos.CellClick += ManejarClickCelda;

            panelContenidoInterno.Controls.Add(dgvEspaciosHijos);
        }

        private void ConfigurarColumnasDataGrid()
        {
            dgvEspaciosHijos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "id_espacio",
                HeaderText = "ID",
                DataPropertyName = "id_espacio",
                Width = 60
            });

            dgvEspaciosHijos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nombre_espacio",
                HeaderText = "Nombre",
                DataPropertyName = "nombre_espacio"
            });

            dgvEspaciosHijos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "estado_espacio",
                HeaderText = "Estado",
                DataPropertyName = "estado_espacio",
                Width = 100
            });

            dgvEspaciosHijos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "capacidad",
                HeaderText = "Capacidad",
                DataPropertyName = "capacidad",
                Width = 100
            });

            dgvEspaciosHijos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "fecha_registro",
                HeaderText = "Fecha Registro",
                DataPropertyName = "fecha_registro",
                Width = 120
            });

            dgvEspaciosHijos.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Editar",
                HeaderText = "",
                Text = "✏️ Editar",
                UseColumnTextForButtonValue = true,
                Width = 80
            });

            dgvEspaciosHijos.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Eliminar",
                HeaderText = "",
                Text = "🗑️ Eliminar",
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
            dgvEspaciosHijos.Visible = !usingCards;
            flowCards.Visible = usingCards;

            if (usingCards)
                RenderCards();
            else
                dgvEspaciosHijos.Refresh();

            ActualizarEstadisticas();
        }

        private void RenderCards()
        {
            flowCards.SuspendLayout();
            flowCards.Controls.Clear();

            foreach (var espacioHijo in bindingList)
            {
                flowCards.Controls.Add(CrearCard(espacioHijo));
            }

            flowCards.ResumeLayout();
        }

        private Panel CrearCard(EspacioHijo espacioHijo)
        {
            var card = new Panel
            {
                Size = new Size(280, 180),
                Margin = new Padding(8),
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            card.Controls.Add(new Label
            {
                Text = espacioHijo.nombre_espacio,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(180, 20)
            });

            card.Controls.Add(new Label
            {
                Text = $"ID: {espacioHijo.id_espacio}",
                Location = new Point(10, 36),
                AutoSize = true
            });

            card.Controls.Add(new Label
            {
                Text = $"Estado: {espacioHijo.estado_espacio}",
                Location = new Point(10, 56),
                AutoSize = true
            });

            card.Controls.Add(new Label
            {
                Text = $"Capacidad: {espacioHijo.capacidad}",
                Location = new Point(10, 76),
                AutoSize = true
            });

            card.Controls.Add(new Label
            {
                Text = $"Fecha: {espacioHijo.fecha_registro.ToShortDateString()}",
                Location = new Point(10, 96),
                AutoSize = true
            });

            var btnEditar = new Button
            {
                Text = "✏️ Editar",
                Size = new Size(80, 26),
                Location = new Point(10, 126),
                Tag = espacioHijo
            };
            btnEditar.Click += (s, e) => EditarEspacioHijo((EspacioHijo)((Button)s).Tag);

            var btnEliminar = new Button
            {
                Text = "🗑️ Eliminar",
                Size = new Size(80, 26),
                Location = new Point(100, 126),
                Tag = espacioHijo
            };
            btnEliminar.Click += (s, e) => EliminarEspacioHijo((EspacioHijo)((Button)s).Tag);

            card.Controls.Add(btnEditar);
            card.Controls.Add(btnEliminar);

            return card;
        }

        private void ActualizarEstadisticas()
        {
            panelEstadisticas.Controls.Clear();

            var datos = bindingList != null ? bindingList : espaciosHijos.AsEnumerable();

            var stats = new[]
            {
                ("Total Espacios", datos.Count()),
                ("Disponibles", datos.Count(e => e.estado_espacio == "Disponible")),
                ("Ocupados", datos.Count(e => e.estado_espacio == "Ocupado"))
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

        private void AgregarEspacioHijo()
        {
            var nuevo = new EspacioHijo
            {
                nombre_espacio = "Nuevo espacio",
                estado_espacio = "Disponible",
                fecha_registro = DateTime.Now,
                capacidad = 0,
                id_tipo_espacio = espacioPadre.id_tipo_espacio
            };

            if (MostrarEditorEspacioHijo(nuevo))
            {
                int nuevoId = espacioService.AgregarEspacio(nuevo);
                if (nuevoId > 0)
                {
                    nuevo.id_espacio = nuevoId;
                    bindingList.Add(nuevo);
                }
                else
                {
                    MessageBox.Show("Error al agregar el espacio");
                }
            }
        }

        private void EditarEspacioHijo(EspacioHijo espacioHijo)
        {
            if (MostrarEditorEspacioHijo(espacioHijo))
            {
                bool exito = espacioService.ActualizarEspacio(espacioHijo);
                if (exito)
                {
                    bindingSource.ResetBindings(false);
                    RenderCurrentView();
                }
                else
                {
                    MessageBox.Show("Error al actualizar el espacio");
                }
            }
        }

        private void EliminarEspacioHijo(EspacioHijo espacioHijo)
        {
            if (MessageBox.Show($"¿Eliminar el espacio '{espacioHijo.nombre_espacio}'?",
                "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool exito = espacioService.EliminarEspacio(espacioHijo.id_espacio);
                if (exito)
                {
                    bindingList.Remove(espacioHijo);
                }
                else
                {
                    MessageBox.Show("Error al eliminar el espacio");
                }
            }
        }

        private void ManejarClickCelda(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= bindingList.Count) return;

            var espacioHijo = bindingList[e.RowIndex];
            var colName = dgvEspaciosHijos.Columns[e.ColumnIndex].Name;

            if (colName == "Editar")
            {
                EditarEspacioHijo(espacioHijo);
            }
            else if (colName == "Eliminar")
            {
                EliminarEspacioHijo(espacioHijo);
            }
        }

        public void EnsureGridBound()
        {
            if (bindingSource != null) return;

            bindingList = new BindingList<EspacioHijo>(espaciosHijos);
            bindingSource = new BindingSource { DataSource = bindingList };
            dgvEspaciosHijos.DataSource = bindingSource;

            bindingList.ListChanged += (s, e) => RenderCurrentView();
            RenderCurrentView();
        }

        private bool MostrarEditorEspacioHijo(EspacioHijo espacioHijo)
        {
            using (var form = new Form())
            {
                form.Text = espacioHijo.id_espacio == 0 ? "Agregar Espacio" : "Editar Espacio";
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.Size = new Size(400, 300);
                form.MaximizeBox = form.MinimizeBox = false;

                var lblNombre = new Label { Text = "Nombre:", Location = new Point(10, 20), AutoSize = true };
                var txtNombre = new TextBox { Text = espacioHijo.nombre_espacio, Location = new Point(100, 16), Width = 250 };

                var lblEstado = new Label { Text = "Estado:", Location = new Point(10, 60), AutoSize = true };
                var cmbEstado = new ComboBox { Location = new Point(100, 56), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbEstado.Items.AddRange(new[] { "Disponible", "Ocupado" });
                cmbEstado.SelectedItem = espacioHijo.estado_espacio;

                var lblCapacidad = new Label { Text = "Capacidad:", Location = new Point(10, 100), AutoSize = true };
                var nudCapacidad = new NumericUpDown { Value = espacioHijo.capacidad, Location = new Point(100, 96), Width = 120, Minimum = 0, Maximum = 10000 };

                var lblFecha = new Label { Text = "Fecha Registro:", Location = new Point(10, 140), AutoSize = true };
                var dtpFecha = new DateTimePicker { Value = espacioHijo.fecha_registro, Location = new Point(100, 136), Width = 150 };

                var btnOk = new Button { Text = "Guardar", DialogResult = DialogResult.OK, Location = new Point(200, 180), Size = new Size(90, 30) };
                var btnCancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(300, 180), Size = new Size(90, 30) };

                form.Controls.Add(lblNombre);
                form.Controls.Add(txtNombre);
                form.Controls.Add(lblEstado);
                form.Controls.Add(cmbEstado);
                form.Controls.Add(lblCapacidad);
                form.Controls.Add(nudCapacidad);
                form.Controls.Add(lblFecha);
                form.Controls.Add(dtpFecha);
                form.Controls.Add(btnOk);
                form.Controls.Add(btnCancel);

                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    espacioHijo.nombre_espacio = txtNombre.Text.Trim();
                    espacioHijo.estado_espacio = cmbEstado.SelectedItem?.ToString();
                    espacioHijo.capacidad = (int)nudCapacidad.Value;
                    espacioHijo.fecha_registro = dtpFecha.Value;
                    return true;
                }
                return false;
            }
        }
    }
}
