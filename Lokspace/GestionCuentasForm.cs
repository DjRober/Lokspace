using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Lokspace
{
    public partial class GestionCuentasForm : Form
    {
        private List<Usuario> usuarios;
        private BindingList<Usuario> bindingList;
        private BindingSource bindingSource;
        private DataGridView dgvUsuarios;
        private Panel panelContenidoInterno;
        private FlowLayoutPanel flowCards;
        private bool usingCards = false;
        private Panel panelEstadisticas;
        private UsuarioService usuarioService = new UsuarioService();

        public GestionCuentasForm()
        {
            InicializarDatos();
            ConfigurarInterfaz();
            ConfigurarDataGridView();
            ConfigurarFlowCards();
            this.Shown += (s, e) => EnsureGridBound();
        }

        private void InicializarDatos()
        {
            usuarios = usuarioService.ObtenerTodosUsuarios();
        }

        // --- INTERFAZ PRINCIPAL ---
        private void ConfigurarInterfaz()
        {
            this.Text = "Gestión de Cuentas de Usuario";
            this.Size = new Size(1100, 700);
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
                Text = "Gestión de Cuentas de Usuario",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(6, 10),
                AutoSize = true
            };

            var btnAgregar = new Button
            {
                Text = "➕ Agregar Usuario",
                Size = new Size(160, 36),
                Location = new Point(820, 28),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAgregar.Click += (s, e) => AgregarUsuario();

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

        // --- DATA GRID VIEW PARA USUARIOS ---
        private void ConfigurarDataGridView()
        {
            dgvUsuarios = new DataGridView
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
            dgvUsuarios.CellClick += ManejarClickCelda;

            panelContenidoInterno.Controls.Add(dgvUsuarios);
        }

        private void ConfigurarColumnasDataGrid()
        {
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NombreCompleto",
                HeaderText = "Nombre Completo",
                DataPropertyName = "NombreCompleto"
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                HeaderText = "Email",
                DataPropertyName = "Email",
                Width = 200
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RolDisplay",
                HeaderText = "Rol",
                DataPropertyName = "RolDisplay",
                Width = 120
            });

            dgvUsuarios.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Editar",
                HeaderText = "",
                Text = "Editar",
                UseColumnTextForButtonValue = true,
                Width = 80
            });

            dgvUsuarios.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Eliminar",
                HeaderText = "",
                Text = "Eliminar",
                UseColumnTextForButtonValue = true,
                Width = 80
            });
        }

        // --- VISTA DE TARJETAS PARA USUARIOS ---
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

        // --- RENDERIZADO Y VISUALIZACIÓN ---
        private void RenderCurrentView()
        {
            dgvUsuarios.Visible = !usingCards;
            flowCards.Visible = usingCards;

            if (usingCards)
                RenderCards();
            else
                dgvUsuarios.Refresh();

            ActualizarEstadisticas();
        }

        private void RenderCards()
        {
            flowCards.SuspendLayout();
            flowCards.Controls.Clear();

            foreach (var usuario in bindingList)
            {
                flowCards.Controls.Add(CrearCard(usuario));
            }

            flowCards.ResumeLayout();
        }

        private Panel CrearCard(Usuario usuario)
        {
            var card = new Panel
            {
                Size = new Size(280, 160),
                Margin = new Padding(8),
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            card.Controls.Add(new Label
            {
                Text = usuario.NombreCompleto,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(200, 20)
            });

            card.Controls.Add(new Label
            {
                Text = $"📧 {usuario.email}",
                Location = new Point(10, 36),
                Size = new Size(200, 20),
                AutoEllipsis = true
            });

            card.Controls.Add(new Label
            {
                Text = $"👤 {usuario.RolDisplay}",
                Location = new Point(10, 56),
                AutoSize = true
            });

            card.Controls.Add(new Label
            {
                Text = $"🆔 ID: {usuario.id_usuario}",
                Location = new Point(10, 76),
                AutoSize = true
            });

            var btnEditar = new Button
            {
                Text = "Editar",
                Size = new Size(60, 26),
                Location = new Point(210, 10),
                Tag = usuario
            };
            btnEditar.Click += (s, e) => EditarUsuario((Usuario)((Button)s).Tag);

            var btnEliminar = new Button
            {
                Text = "Eliminar",
                Size = new Size(60, 26),
                Location = new Point(210, 42),
                Tag = usuario
            };
            btnEliminar.Click += (s, e) => EliminarUsuario((Usuario)((Button)s).Tag);

            card.Controls.Add(btnEditar);
            card.Controls.Add(btnEliminar);

            return card;
        }

        // --- ESTADÍSTICAS DE USUARIOS ---
        private void ActualizarEstadisticas()
        {
            panelEstadisticas.Controls.Clear();

            var datos = bindingList != null ? bindingList : usuarios.AsEnumerable();

            var stats = new[]
            {
                ("Total Usuarios", datos.Count()),
                ("Administradores", datos.Count(u => u.id_rol == 1)),
                ("Docentes", datos.Count(u => u.id_rol == 2)),
                ("Alumnos", datos.Count(u => u.id_rol == 3))
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

        // --- OPERACIONES CRUD ---
        private void AgregarUsuario()
        {
            var nuevo = new Usuario
            {
                id_usuario = ObtenerNuevoIdTemporal(),
                nombre1_usuario = "Nuevo",
                ap_usuario = "Usuario",
                email = "nuevo@email.com",
                password = "temp123",
                id_rol = 3 // Alumno por defecto
            };

            if (MostrarEditorUsuario(nuevo))
            {
                bindingList.Add(nuevo);
            }
        }

        private void EditarUsuario(Usuario usuario)
        {
            if (MostrarEditorUsuario(usuario))
            {
                bindingSource.ResetBindings(false);
                RenderCurrentView();
            }
        }

        private void EliminarUsuario(Usuario usuario)
        {
            if (MessageBox.Show($"¿Eliminar usuario {usuario.NombreCompleto}?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bindingList.Remove(usuario);
            }
        }

        private void ManejarClickCelda(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= bindingList.Count) return;

            var usuario = bindingList[e.RowIndex];
            var colName = dgvUsuarios.Columns[e.ColumnIndex].Name;

            if (colName == "Editar")
            {
                EditarUsuario(usuario);
            }
            else if (colName == "Eliminar")
            {
                EliminarUsuario(usuario);
            }
        }

        // --- BINDING Y DATOS ---
        public void EnsureGridBound()
        {
            if (bindingSource != null) return;

            bindingList = new BindingList<Usuario>(usuarios);
            bindingSource = new BindingSource { DataSource = bindingList };
            dgvUsuarios.DataSource = bindingSource;

            bindingList.ListChanged += (s, e) => RenderCurrentView();
            RenderCurrentView();
        }

        // --- EDITOR DE USUARIO ---
        private bool MostrarEditorUsuario(Usuario usuario)
        {
            using (var form = new Form())
            {
                form.Text = usuario.id_usuario < 0 ? "Agregar Usuario" : "Editar Usuario";
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.Size = new Size(500, 400);
                form.MaximizeBox = form.MinimizeBox = false;

                var controles = new[]
                {
                    CrearControlEditor("Primer Nombre:", new TextBox { Text = usuario.nombre1_usuario, Width = 300 }, 10),
                    CrearControlEditor("Segundo Nombre:", new TextBox { Text = usuario.nombre2_usuario ?? "", Width = 300 }, 50),
                    CrearControlEditor("Apellido Paterno:", new TextBox { Text = usuario.ap_usuario, Width = 300 }, 90),
                    CrearControlEditor("Apellido Materno:", new TextBox { Text = usuario.am_usuario ?? "", Width = 300 }, 130),
                    CrearControlEditor("Email:", new TextBox { Text = usuario.email, Width = 300 }, 170),
                    CrearControlEditor("Contraseña:", new TextBox { Text = usuario.password, Width = 300, UseSystemPasswordChar = true }, 210),
                    CrearControlEditor("Rol:", CrearComboRol(usuario.id_rol), 250)
                };

                var btnOk = new Button { Text = "Guardar", DialogResult = DialogResult.OK, Location = new Point(280, 300), Size = new Size(90, 30) };
                var btnCancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(380, 300), Size = new Size(90, 30) };

                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;
                form.Controls.AddRange(controles);
                form.Controls.Add(btnOk);
                form.Controls.Add(btnCancel);

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    usuario.nombre1_usuario = ((TextBox)controles[0].Controls[1]).Text.Trim();
                    usuario.nombre2_usuario = ((TextBox)controles[1].Controls[1]).Text.Trim();
                    usuario.ap_usuario = ((TextBox)controles[2].Controls[1]).Text.Trim();
                    usuario.am_usuario = ((TextBox)controles[3].Controls[1]).Text.Trim();
                    usuario.email = ((TextBox)controles[4].Controls[1]).Text.Trim();
                    usuario.password = ((TextBox)controles[5].Controls[1]).Text;
                    usuario.id_rol = (int)((ComboBox)controles[6].Controls[1]).SelectedValue;
                    return true;
                }
                return false;
            }
        }

        private Panel CrearControlEditor(string label, Control control, int y)
        {
            var panel = new Panel { Location = new Point(10, y), Size = new Size(470, 30) };
            panel.Controls.Add(new Label { Text = label, AutoSize = true, Location = new Point(0, 5) });
            panel.Controls.Add(control);
            control.Location = new Point(150, 0);
            return panel;
        }

        private ComboBox CrearComboRol(int rolActual)
        {
            var combo = new ComboBox { Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            // Roles hardcodeados según especificación
            var roles = new List<object>
            {
                new { Id = 1, Nombre = "👑 Administrador" },
                new { Id = 2, Nombre = "📚 Docente" },
                new { Id = 3, Nombre = "🎓 Alumno" }
            };

            combo.DataSource = roles;
            combo.DisplayMember = "Nombre";
            combo.ValueMember = "Id";
            combo.SelectedValue = rolActual;

            return combo;
        }

        private int ObtenerNuevoIdTemporal()
        {
            if (bindingList == null || !bindingList.Any())
                return -1;

            return bindingList.Min(u => u.id_usuario) - 1;
        }
    }

    // --- EXTENSIONES PARA LA CLASE USUARIO ---
    public partial class Usuario
    {
        // Propiedad calculada para nombre completo
        public string NombreCompleto
        {
            get
            {
                var nombres = new List<string> { nombre1_usuario };
                if (!string.IsNullOrEmpty(nombre2_usuario))
                    nombres.Add(nombre2_usuario);

                var apellidos = new List<string> { ap_usuario };
                if (!string.IsNullOrEmpty(am_usuario))
                    apellidos.Add(am_usuario);

                return $"{string.Join(" ", nombres)} {string.Join(" ", apellidos)}".Trim();
            }
        }

        // Propiedad calculada para mostrar el rol de forma amigable
        public string RolDisplay
        {
            get
            {
                switch (id_rol)
                {
                    case 1:
                        return "👑 Administrador";
                    case 2:
                        return "📚 Docente";
                    case 3:
                        return "🎓 Alumno";
                    default:
                        return $"❓ Rol {id_rol}";
                }
            }
        }
    }
}