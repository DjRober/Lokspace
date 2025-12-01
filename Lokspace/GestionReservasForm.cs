using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Lokspace
{
    public partial class GestionReservasForm : Form
    {
        private List<Reserva> reservas;
        private BindingList<Reserva> bindingList;
        private BindingSource bindingSource;
        private DataGridView dgvReservas;
        private Panel panelContenidoInterno;
        private FlowLayoutPanel flowCards;
        private bool usingCards = false;
        private Panel panelEstadisticas;
        private Panel panelFiltros;
        private ReservaService reservaService = new ReservaService();
        private UsuarioService usuarioService = new UsuarioService();
        private EspacioService espacioService = new EspacioService();
        private Usuario usuarioActual;

        // Filtros
        private ComboBox cmbFiltroEstado;
        private ComboBox cmbFiltroEspacio;
        private DateTimePicker dtpFiltroFecha;
        private CheckBox chkFiltroFecha;

        public GestionReservasForm(Usuario usuario)
        {
            this.usuarioActual = usuario;
            InicializarDatos();
            ConfigurarInterfaz();
            ConfigurarDataGridView();
            ConfigurarFlowCards();
            ConfigurarFiltros();
            this.Shown += (s, e) => EnsureGridBound();
        }

        private void InicializarDatos()
        {
            reservas = reservaService.ObtenerTodasReservas();
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Gestión de Reservas";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Padding = new Padding(12);

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F)); // Header
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));  // Filtros
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F)); // Estadísticas
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Contenido

            layout.Controls.Add(CrearHeader(), 0, 0);
            layout.Controls.Add(CrearPanelFiltros(), 0, 1);
            layout.Controls.Add(CrearPanelEstadisticas(), 0, 2);
            layout.Controls.Add(CrearPanelContenido(), 0, 3);

            this.Controls.Add(layout);
        }

        private Panel CrearHeader()
        {
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            var title = new Label
            {
                Text = "Gestión de Reservas",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(6, 10),
                AutoSize = true
            };

            var btnAgregar = new Button
            {
                Text = "➕ Nueva Reserva",
                Size = new Size(140, 36),
                Location = new Point(1000, 28),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAgregar.Click += (s, e) => AgregarReserva();

            var btnToggleView = new Button
            {
                Text = usingCards ? "📋 Vista Tabla" : "🃏 Vista Tarjetas",
                Size = new Size(120, 28),
                Location = new Point(860, 30)
            };
            btnToggleView.Click += (s, e) => {
                usingCards = !usingCards;
                btnToggleView.Text = usingCards ? "📋 Vista Tabla" : "🃏 Vista Tarjetas";
                RenderCurrentView();
            };

            var btnRefrescar = new Button
            {
                Text = "🔄 Refrescar",
                Size = new Size(100, 28),
                Location = new Point(740, 30)
            };
            btnRefrescar.Click += (s, e) => RefrescarDatos();

            panel.Controls.AddRange(new Control[] { title, btnAgregar, btnToggleView, btnRefrescar });
            return panel;
        }

        private Panel CrearPanelFiltros()
        {
            panelFiltros = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            return panelFiltros;
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

        private void ConfigurarFiltros()
        {
            panelFiltros.Controls.Clear();

            // Filtro por estado
            var lblEstado = new Label { Text = "Estado:", Location = new Point(10, 15), AutoSize = true };
            cmbFiltroEstado = new ComboBox
            {
                Location = new Point(60, 12),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFiltroEstado.Items.Add("Todos los estados");
            cmbFiltroEstado.SelectedIndex = 0;

            var estados = reservaService.ObtenerEstadosReserva();
            foreach (var estado in estados)
            {
                cmbFiltroEstado.Items.Add(new { Text = estado.nombre_estado, Value = estado.id_estado });
            }
            cmbFiltroEstado.DisplayMember = "Text";
            cmbFiltroEstado.ValueMember = "Value";
            cmbFiltroEstado.SelectedIndexChanged += (s, e) => AplicarFiltros();

            // Filtro por espacio
            var lblEspacio = new Label { Text = "Espacio:", Location = new Point(200, 15), AutoSize = true };
            cmbFiltroEspacio = new ComboBox
            {
                Location = new Point(260, 12),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFiltroEspacio.Items.Add("Todos los espacios");
            cmbFiltroEspacio.SelectedIndex = 0;

            var espacios = espacioService.ObtenerTodosEspaciosHijos();
            foreach (var espacio in espacios)
            {
                cmbFiltroEspacio.Items.Add(new { Text = espacio.nombre_espacio, Value = espacio.id_espacio });
            }
            cmbFiltroEspacio.DisplayMember = "Text";
            cmbFiltroEspacio.ValueMember = "Value";
            cmbFiltroEspacio.SelectedIndexChanged += (s, e) => AplicarFiltros();

            // Filtro por fecha
            chkFiltroFecha = new CheckBox { Text = "Fecha:", Location = new Point(430, 14), AutoSize = true };
            dtpFiltroFecha = new DateTimePicker
            {
                Location = new Point(490, 12),
                Width = 120,
                Enabled = false
            };
            chkFiltroFecha.CheckedChanged += (s, e) => {
                dtpFiltroFecha.Enabled = chkFiltroFecha.Checked;
                AplicarFiltros();
            };
            dtpFiltroFecha.ValueChanged += (s, e) => AplicarFiltros();

            // Botón limpiar filtros
            var btnLimpiar = new Button
            {
                Text = "Limpiar Filtros",
                Location = new Point(630, 11),
                Size = new Size(100, 23)
            };
            btnLimpiar.Click += (s, e) => LimpiarFiltros();

            panelFiltros.Controls.AddRange(new Control[] {
                lblEstado, cmbFiltroEstado,
                lblEspacio, cmbFiltroEspacio,
                chkFiltroFecha, dtpFiltroFecha,
                btnLimpiar
            });
        }

        private void ConfigurarDataGridView()
        {
            dgvReservas = new DataGridView
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
            dgvReservas.CellClick += ManejarClickCelda;

            panelContenidoInterno.Controls.Add(dgvReservas);
        }

        private void ConfigurarColumnasDataGrid()
        {
            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "id_reserva",
                HeaderText = "ID",
                DataPropertyName = "id_reserva",
                Width = 50
            });

            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NombreEspacio",
                HeaderText = "Espacio",
                DataPropertyName = "NombreEspacio",
                Width = 120
            });

            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "fecha_reserva",
                HeaderText = "Fecha",
                DataPropertyName = "fecha_reserva",
                Width = 90
            });

            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "hora_inicio",
                HeaderText = "Hora Inicio",
                DataPropertyName = "hora_inicio",
                Width = 80
            });

            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "hora_fin",
                HeaderText = "Hora Fin",
                DataPropertyName = "hora_fin",
                Width = 80
            });

            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NombreUsuario",
                HeaderText = "Solicitante",
                DataPropertyName = "NombreUsuario",
                Width = 120
            });

            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RolUsuario",
                HeaderText = "Rol",
                DataPropertyName = "RolUsuario",
                Width = 80
            });

            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EstadoReserva",
                HeaderText = "Estado",
                DataPropertyName = "EstadoReserva",
                Width = 90
            });

            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "proposito",
                HeaderText = "Propósito",
                DataPropertyName = "proposito"
            });

            // Botones de acción
            dgvReservas.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Editar",
                HeaderText = "",
                Text = "✏️ Editar",
                UseColumnTextForButtonValue = true,
                Width = 70
            });

            dgvReservas.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "CambiarEstado",
                HeaderText = "",
                Text = "🔄 Estado",
                UseColumnTextForButtonValue = true,
                Width = 70
            });

            dgvReservas.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Eliminar",
                HeaderText = "",
                Text = "🗑️ Eliminar",
                UseColumnTextForButtonValue = true,
                Width = 70
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
            dgvReservas.Visible = !usingCards;
            flowCards.Visible = usingCards;

            if (usingCards)
                RenderCards();
            else
                dgvReservas.Refresh();

            ActualizarEstadisticas();
        }

        private void RenderCards()
        {
            flowCards.SuspendLayout();
            flowCards.Controls.Clear();

            var datosFiltrados = ObtenerReservasFiltradas();
            foreach (var reserva in datosFiltrados)
            {
                flowCards.Controls.Add(CrearCard(reserva));
            }

            flowCards.ResumeLayout();
        }

        private Panel CrearCard(Reserva reserva)
        {
            var card = new Panel
            {
                Size = new Size(300, 200),
                Margin = new Padding(8),
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Título con el nombre del espacio
            card.Controls.Add(new Label
            {
                Text = reserva.NombreEspacio,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(280, 20)
            });

            // Fecha y hora
            card.Controls.Add(new Label
            {
                Text = $"{reserva.fecha_reserva.ToShortDateString()} {reserva.hora_inicio:hh\\:mm} - {reserva.hora_fin:hh\\:mm}",
                Location = new Point(10, 36),
                AutoSize = true
            });

            // Solicitante
            card.Controls.Add(new Label
            {
                Text = $"Solicitante: {reserva.NombreUsuario}",
                Location = new Point(10, 56),
                Size = new Size(280, 20),
                AutoEllipsis = true
            });

            // Estado
            card.Controls.Add(new Label
            {
                Text = $"Estado: {reserva.EstadoReserva}",
                Location = new Point(10, 76),
                AutoSize = true
            });

            // Propósito
            card.Controls.Add(new Label
            {
                Text = $"Propósito: {reserva.proposito}",
                Location = new Point(10, 96),
                Size = new Size(280, 40),
                AutoEllipsis = true
            });

            // Botones de acción
            var btnEditar = new Button
            {
                Text = "✏️ Editar",
                Size = new Size(70, 26),
                Location = new Point(10, 146),
                Tag = reserva
            };
            btnEditar.Click += (s, e) => EditarReserva((Reserva)((Button)s).Tag);

            var btnEstado = new Button
            {
                Text = "🔄 Estado",
                Size = new Size(70, 26),
                Location = new Point(90, 146),
                Tag = reserva
            };
            btnEstado.Click += (s, e) => CambiarEstadoReserva((Reserva)((Button)s).Tag);

            var btnEliminar = new Button
            {
                Text = "🗑️ Eliminar",
                Size = new Size(70, 26),
                Location = new Point(170, 146),
                Tag = reserva
            };
            btnEliminar.Click += (s, e) => EliminarReserva((Reserva)((Button)s).Tag);

            card.Controls.Add(btnEditar);
            card.Controls.Add(btnEstado);
            card.Controls.Add(btnEliminar);

            return card;
        }

        private void ActualizarEstadisticas()
        {
            panelEstadisticas.Controls.Clear();

            var datosFiltrados = ObtenerReservasFiltradas();

            var stats = new[]
            {
                ("Total Reservas", datosFiltrados.Count()),
                ("Pendientes", datosFiltrados.Count(r => r.id_estado_reserva == 3)),
                ("Aceptadas", datosFiltrados.Count(r => r.id_estado_reserva == 1)),
                ("Rechazadas", datosFiltrados.Count(r => r.id_estado_reserva == 2)),
                ("Canceladas", datosFiltrados.Count(r => r.id_estado_reserva == 4))
            };

            int x = 6;
            foreach (var (texto, valor) in stats)
            {
                panelEstadisticas.Controls.Add(CrearCardEstadistica(texto, valor, x, 180));
                x += 186;
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

        private void AplicarFiltros()
        {
            var datosFiltrados = ObtenerReservasFiltradas();
            bindingList = new BindingList<Reserva>(datosFiltrados.ToList());
            bindingSource.DataSource = bindingList;
            RenderCurrentView();
        }

        private IEnumerable<Reserva> ObtenerReservasFiltradas()
        {
            var query = reservas.AsEnumerable();

            // Filtro por estado
            if (cmbFiltroEstado.SelectedIndex > 0)
            {
                var estadoSeleccionado = (cmbFiltroEstado.SelectedItem as dynamic)?.Value;
                if (estadoSeleccionado != null)
                {
                    query = query.Where(r => r.id_estado_reserva == (int)estadoSeleccionado);
                }
            }

            // Filtro por espacio
            if (cmbFiltroEspacio.SelectedIndex > 0)
            {
                var espacioSeleccionado = (cmbFiltroEspacio.SelectedItem as dynamic)?.Value;
                if (espacioSeleccionado != null)
                {
                    query = query.Where(r => r.id_espacio == (int)espacioSeleccionado);
                }
            }

            // Filtro por fecha
            if (chkFiltroFecha.Checked)
            {
                query = query.Where(r => r.fecha_reserva.Date == dtpFiltroFecha.Value.Date);
            }

            return query;
        }

        private void LimpiarFiltros()
        {
            cmbFiltroEstado.SelectedIndex = 0;
            cmbFiltroEspacio.SelectedIndex = 0;
            chkFiltroFecha.Checked = false;
            AplicarFiltros();
        }

        private void RefrescarDatos()
        {
            InicializarDatos();
            EnsureGridBound();
            ConfigurarFiltros();
            AplicarFiltros();
        }

        private void AgregarReserva()
        {
            var nueva = new Reserva
            {
                fecha_reserva = DateTime.Today,
                hora_inicio = new TimeSpan(9, 0, 0),
                hora_fin = new TimeSpan(10, 0, 0),
                proposito = "Nueva reserva",
                fecha_solicitud = DateTime.Now,
                id_estado_reserva = 3, // Pendiente por defecto
                id_gestor = usuarioActual.id_usuario,
                id_usuario = usuarioActual.id_usuario
            };

            if (MostrarEditorReserva(nueva))
            {
                // Verificar disponibilidad
                if (!reservaService.VerificarDisponibilidad(nueva.id_espacio, nueva.fecha_reserva, nueva.hora_inicio, nueva.hora_fin))
                {
                    MessageBox.Show("El espacio no está disponible en el horario seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int nuevoId = reservaService.CrearReserva(nueva);
                if (nuevoId > 0)
                {
                    nueva.id_reserva = nuevoId;
                    // Recargar la reserva con toda la información
                    RefrescarDatos();
                }
                else
                {
                    MessageBox.Show("Error al crear la reserva", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EditarReserva(Reserva reserva)
        {
            if (MostrarEditorReserva(reserva))
            {
                // Verificar disponibilidad excluyendo la reserva actual
                if (!reservaService.VerificarDisponibilidad(reserva.id_espacio, reserva.fecha_reserva, reserva.hora_inicio, reserva.hora_fin, reserva.id_reserva))
                {
                    MessageBox.Show("El espacio no está disponible en el horario seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool exito = reservaService.ActualizarReserva(reserva);
                if (exito)
                {
                    RefrescarDatos();
                }
                else
                {
                    MessageBox.Show("Error al actualizar la reserva", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EliminarReserva(Reserva reserva)
        {
            if (MessageBox.Show($"¿Eliminar la reserva del espacio {reserva.NombreEspacio} para {reserva.fecha_reserva.ToShortDateString()}?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool exito = reservaService.EliminarReserva(reserva.id_reserva);
                if (exito)
                {
                    RefrescarDatos();
                }
                else
                {
                    MessageBox.Show("Error al eliminar la reserva", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CambiarEstadoReserva(Reserva reserva)
        {
            using (var form = new Form())
            {
                form.Text = "Cambiar Estado de Reserva";
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.Size = new Size(300, 200);
                form.MaximizeBox = form.MinimizeBox = false;

                var lblEstado = new Label { Text = "Nuevo estado:", Location = new Point(10, 20), AutoSize = true };
                var cmbEstado = new ComboBox { Location = new Point(100, 16), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

                var estados = reservaService.ObtenerEstadosReserva();
                foreach (var estado in estados)
                {
                    cmbEstado.Items.Add(new { Text = estado.nombre_estado, Value = estado.id_estado });
                }
                cmbEstado.DisplayMember = "Text";
                cmbEstado.ValueMember = "Value";
                cmbEstado.SelectedValue = reserva.id_estado_reserva;

                var btnOk = new Button { Text = "Guardar", DialogResult = DialogResult.OK, Location = new Point(100, 60), Size = new Size(90, 30) };
                var btnCancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(200, 60), Size = new Size(90, 30) };

                form.Controls.Add(lblEstado);
                form.Controls.Add(cmbEstado);
                form.Controls.Add(btnOk);
                form.Controls.Add(btnCancel);

                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    int nuevoEstado = (int)(cmbEstado.SelectedItem as dynamic).Value;
                    // Asumimos que el gestor es el usuario actual (deberías pasar el usuario actual al formulario)
                    int idGestor = 1; // Esto debería ser el ID del usuario administrador actual

                    bool exito = reservaService.CambiarEstadoReserva(reserva.id_reserva, nuevoEstado, idGestor);
                    if (exito)
                    {
                        RefrescarDatos();
                    }
                    else
                    {
                        MessageBox.Show("Error al cambiar el estado de la reserva", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ManejarClickCelda(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= bindingList.Count) return;

            var reserva = bindingList[e.RowIndex];
            var colName = dgvReservas.Columns[e.ColumnIndex].Name;

            if (colName == "Editar")
            {
                EditarReserva(reserva);
            }
            else if (colName == "CambiarEstado")
            {
                CambiarEstadoReserva(reserva);
            }
            else if (colName == "Eliminar")
            {
                EliminarReserva(reserva);
            }
        }

        public void EnsureGridBound()
        {
            if (bindingSource != null) return;

            bindingList = new BindingList<Reserva>(reservas);
            bindingSource = new BindingSource { DataSource = bindingList };
            dgvReservas.DataSource = bindingSource;

            bindingList.ListChanged += (s, e) => RenderCurrentView();
            RenderCurrentView();
        }

        private bool MostrarEditorReserva(Reserva reserva)
        {
            using (var form = new Form())
            {
                form.Text = reserva.id_reserva == 0 ? "Agregar Reserva" : "Editar Reserva";
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.Size = new Size(500, 400);
                form.MaximizeBox = form.MinimizeBox = false;

                // Controles del formulario
                var lblEspacio = new Label { Text = "Espacio:", Location = new Point(10, 20), AutoSize = true };
                var cmbEspacio = new ComboBox { Location = new Point(100, 16), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };

                var espacios = espacioService.ObtenerTodosEspaciosHijos();
                foreach (var espacio in espacios)
                {
                    cmbEspacio.Items.Add(new { Text = espacio.nombre_espacio, Value = espacio.id_espacio });
                }
                cmbEspacio.DisplayMember = "Text";
                cmbEspacio.ValueMember = "Value";
                if (reserva.id_espacio > 0)
                    cmbEspacio.SelectedValue = reserva.id_espacio;

                var lblFecha = new Label { Text = "Fecha:", Location = new Point(10, 60), AutoSize = true };
                var dtpFecha = new DateTimePicker { Value = reserva.fecha_reserva, Location = new Point(100, 56), Width = 120 };

                var lblHoraInicio = new Label { Text = "Hora Inicio:", Location = new Point(10, 100), AutoSize = true };
                var dtpHoraInicio = new DateTimePicker
                {
                    Value = DateTime.Today.Add(reserva.hora_inicio),
                    Location = new Point(100, 96),
                    Width = 80,
                    Format = DateTimePickerFormat.Time,
                    ShowUpDown = true
                };

                var lblHoraFin = new Label { Text = "Hora Fin:", Location = new Point(200, 100), AutoSize = true };
                var dtpHoraFin = new DateTimePicker
                {
                    Value = DateTime.Today.Add(reserva.hora_fin),
                    Location = new Point(260, 96),
                    Width = 80,
                    Format = DateTimePickerFormat.Time,
                    ShowUpDown = true
                };

                var lblProposito = new Label { Text = "Propósito:", Location = new Point(10, 140), AutoSize = true };
                var txtProposito = new TextBox { Text = reserva.proposito, Location = new Point(100, 136), Width = 300, Multiline = true, Height = 60 };

                var btnOk = new Button { Text = "Guardar", DialogResult = DialogResult.OK, Location = new Point(280, 220), Size = new Size(90, 30) };
                var btnCancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(380, 220), Size = new Size(90, 30) };

                form.Controls.Add(lblEspacio);
                form.Controls.Add(cmbEspacio);
                form.Controls.Add(lblFecha);
                form.Controls.Add(dtpFecha);
                form.Controls.Add(lblHoraInicio);
                form.Controls.Add(dtpHoraInicio);
                form.Controls.Add(lblHoraFin);
                form.Controls.Add(dtpHoraFin);
                form.Controls.Add(lblProposito);
                form.Controls.Add(txtProposito);
                form.Controls.Add(btnOk);
                form.Controls.Add(btnCancel);

                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    reserva.id_espacio = (int)(cmbEspacio.SelectedItem as dynamic).Value;
                    reserva.fecha_reserva = dtpFecha.Value;
                    reserva.hora_inicio = dtpHoraInicio.Value.TimeOfDay;
                    reserva.hora_fin = dtpHoraFin.Value.TimeOfDay;
                    reserva.proposito = txtProposito.Text.Trim();
                    return true;
                }
                return false;
            }
        }
    }
}