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
        private List<EspacioPadre> espaciosPadre;
        private BindingList<EspacioPadre> bindingList;
        private BindingSource bindingSource;
        private DataGridView dgvEspacios;
        private Panel panelContenidoInterno;
        private FlowLayoutPanel flowCards;
        private bool usingCards = false;
        private Panel panelEstadisticas;
        private EspacioService espacioService = new EspacioService();

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
            // Usar el servicio para obtener datos reales de la base de datos
            espaciosPadre = espacioService.ObtenerTodosTiposEspacios();
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Gestión de Tipos de Espacios";
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
                Text = "Gestión de Tipos de Espacios",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(6, 10),
                AutoSize = true
            };

            var btnAgregar = new Button
            {
                Text = "➕ Agregar Tipo",
                Size = new Size(160, 36),
                Location = new Point(820, 28),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAgregar.Click += (s, e) => AgregarEspacioPadre();

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
                Name = "id_tipo_espacio",
                HeaderText = "ID",
                DataPropertyName = "id_tipo_espacio",
                Width = 60
            });

            dgvEspacios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nombre_tipo",
                HeaderText = "Nombre del Tipo",
                DataPropertyName = "nombre_tipo"
            });

            dgvEspacios.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Visualizar",
                HeaderText = "",
                Text = "👁️ Visualizar",
                UseColumnTextForButtonValue = true,
                Width = 100
            });

            dgvEspacios.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Editar",
                HeaderText = "",
                Text = "✏️ Editar",
                UseColumnTextForButtonValue = true,
                Width = 80
            });

            dgvEspacios.Columns.Add(new DataGridViewButtonColumn
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

            foreach (var espacioPadre in bindingList)
            {
                flowCards.Controls.Add(CrearCard(espacioPadre));
            }

            flowCards.ResumeLayout();
        }

        private Panel CrearCard(EspacioPadre espacioPadre)
        {
            var card = new Panel
            {
                Size = new Size(280, 140),
                Margin = new Padding(8),
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            card.Controls.Add(new Label
            {
                Text = espacioPadre.nombre_tipo,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(180, 20)
            });

            card.Controls.Add(new Label
            {
                Text = $"ID: {espacioPadre.id_tipo_espacio}",
                Location = new Point(10, 36),
                AutoSize = true
            });

            var btnVisualizar = new Button
            {
                Text = "👁️ Visualizar",
                Size = new Size(80, 26),
                Location = new Point(10, 70),
                Tag = espacioPadre
            };
            btnVisualizar.Click += (s, e) => VisualizarEspaciosHijos((EspacioPadre)((Button)s).Tag);

            var btnEditar = new Button
            {
                Text = "✏️ Editar",
                Size = new Size(80, 26),
                Location = new Point(100, 70),
                Tag = espacioPadre
            };
            btnEditar.Click += (s, e) => EditarEspacioPadre((EspacioPadre)((Button)s).Tag);

            var btnEliminar = new Button
            {
                Text = "🗑️ Eliminar",
                Size = new Size(80, 26),
                Location = new Point(190, 70),
                Tag = espacioPadre
            };
            btnEliminar.Click += (s, e) => EliminarEspacioPadre((EspacioPadre)((Button)s).Tag);

            card.Controls.Add(btnVisualizar);
            card.Controls.Add(btnEditar);
            card.Controls.Add(btnEliminar);

            return card;
        }

        private void ActualizarEstadisticas()
        {
            panelEstadisticas.Controls.Clear();

            var datos = bindingList != null ? bindingList : espaciosPadre.AsEnumerable();

            var stats = new[]
            {
                ("Total Tipos de Espacios", datos.Count())
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

        private void AgregarEspacioPadre()
        {
            var nuevo = new EspacioPadre
            {
                nombre_tipo = "Nuevo tipo de espacio"
            };

            if (MostrarEditorEspacioPadre(nuevo))
            {
                int nuevoId = espacioService.AgregarTipoEspacio(nuevo);
                if (nuevoId > 0)
                {
                    nuevo.id_tipo_espacio = nuevoId;
                    bindingList.Add(nuevo);
                }
                else
                {
                    MessageBox.Show("Error al agregar el tipo de espacio");
                }
            }
        }

        private void EditarEspacioPadre(EspacioPadre espacioPadre)
        {
            if (MostrarEditorEspacioPadre(espacioPadre))
            {
                bool exito = espacioService.ActualizarTipoEspacio(espacioPadre);
                if (exito)
                {
                    bindingSource.ResetBindings(false);
                    RenderCurrentView();
                }
                else
                {
                    MessageBox.Show("Error al actualizar el tipo de espacio");
                }
            }
        }

        private void EliminarEspacioPadre(EspacioPadre espacioPadre)
        {
            if (MessageBox.Show($"¿Eliminar el tipo de espacio '{espacioPadre.nombre_tipo}'?",
                "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool exito = espacioService.EliminarTipoEspacio(espacioPadre.id_tipo_espacio);
                if (exito)
                {
                    bindingList.Remove(espacioPadre);
                }
                else
                {
                    MessageBox.Show("Error al eliminar el tipo de espacio");
                }
            }
        }

        private void VisualizarEspaciosHijos(EspacioPadre espacioPadre)
        {
            // Pendiente: Abrir formulario MostrarEspaciosHijos
            MessageBox.Show($"Visualizar espacios hijos para: {espacioPadre.nombre_tipo}\n\n" +
                          $"Esta función abrirá el formulario MostrarEspaciosHijos con ID: {espacioPadre.id_tipo_espacio}",
                          "Función Pendiente", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ManejarClickCelda(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= bindingList.Count) return;

            var espacioPadre = bindingList[e.RowIndex];
            var colName = dgvEspacios.Columns[e.ColumnIndex].Name;

            if (colName == "Visualizar")
            {
                VisualizarEspaciosHijos(espacioPadre);
            }
            else if (colName == "Editar")
            {
                EditarEspacioPadre(espacioPadre);
            }
            else if (colName == "Eliminar")
            {
                EliminarEspacioPadre(espacioPadre);
            }
        }

        public void EnsureGridBound()
        {
            if (bindingSource != null) return;

            bindingList = new BindingList<EspacioPadre>(espaciosPadre);
            bindingSource = new BindingSource { DataSource = bindingList };
            dgvEspacios.DataSource = bindingSource;

            bindingList.ListChanged += (s, e) => RenderCurrentView();
            RenderCurrentView();
        }

        private bool MostrarEditorEspacioPadre(EspacioPadre espacioPadre)
        {
            using (var form = new Form())
            {
                form.Text = espacioPadre.id_tipo_espacio == 0 ? "Agregar Tipo de Espacio" : "Editar Tipo de Espacio";
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.Size = new Size(400, 150);
                form.MaximizeBox = form.MinimizeBox = false;

                var lblNombre = new Label { Text = "Nombre del tipo:", Location = new Point(10, 20), AutoSize = true };
                var txtNombre = new TextBox { Text = espacioPadre.nombre_tipo, Location = new Point(120, 16), Width = 250 };

                var btnOk = new Button { Text = "Guardar", DialogResult = DialogResult.OK, Location = new Point(200, 60), Size = new Size(90, 30) };
                var btnCancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(300, 60), Size = new Size(90, 30) };

                form.Controls.Add(lblNombre);
                form.Controls.Add(txtNombre);
                form.Controls.Add(btnOk);
                form.Controls.Add(btnCancel);

                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    espacioPadre.nombre_tipo = txtNombre.Text.Trim();
                    return true;
                }
                return false;
            }
        }
    }
}