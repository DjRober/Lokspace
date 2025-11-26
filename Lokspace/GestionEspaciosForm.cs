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

            public string TipoDisplay => Type == "building" ? "🏢 Edificio" : Type == "classroom" ? "🏫 Aula" : Type == "sports" ? "⚽ Deportivo" : "❓ Desconocido";
            public string CapacidadDisplay => Capacity > 0 ? Capacity + " personas" : "N/A";
        }

        private List<Space> espacios;
        private BindingList<Space> bindingList;
        private BindingSource bindingSource;
        private DataGridView dgvEspacios;
        private Panel panelContenidoInterno;
        private FlowLayoutPanel flowCards;
        private bool usingCards = false;

        public GestionEspaciosForm()
        {
            InitializeComponent();
            InicializarDatos();
            ConfigurarInterfazOptimizada();
            ConfigurarDataGridViewOptimizado();
            ConfigurarFlowCards();
            this.Shown += (s, e) => EnsureGridBound();
        }

        // --- Data ---
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

        // --- Interfaz: modular y usando TableLayoutPanel ---
        private void ConfigurarInterfazOptimizada()
        {
            this.Text = "Gestión de Espacios Universitarios";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Padding = new Padding(12);

            var header = CrearHeaderOptimizado();
            var stats = CrearPanelEstadisticasOptimizado();

            panelContenidoInterno = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8), BackColor = Color.White };

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));   // header
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));   // stats
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));   // content

            layout.Controls.Add(header, 0, 0);
            layout.Controls.Add(stats, 0, 1);
            layout.Controls.Add(panelContenidoInterno, 0, 2);

            this.Controls.Add(layout);
        }

        private Panel CrearHeaderOptimizado()
        {
            var p = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            var title = new Label { Text = "Administrador de Espacios Universitarios", Font = new Font("Segoe UI", 18, FontStyle.Bold), Location = new Point(6, 10), AutoSize = true };
            var subtitle = new Label { Text = "Gestiona edificios, aulas y espacios deportivos", Font = new Font("Segoe UI", 10), Location = new Point(6, 44), AutoSize = true };

            var btnAgregar = new Button { Text = "➕ Agregar Espacio", Size = new Size(160, 36), Location = new Point(820, 28), BackColor = Color.FromArgb(59, 130, 246), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnAgregar.Click += (s, e) =>
            {
                var nuevo = new Space { Id = Guid.NewGuid().ToString(), Name = "Nuevo espacio", Type = "classroom", Capacity = 0 };
                if (MostrarEditor(nuevo))
                {
                    if (bindingList == null) EnsureGridBound();
                    bindingList.Add(nuevo);
                    RenderCurrentView();
                }
            };

            var btnToggleView = new Button { Text = "Cambiar vista", Size = new Size(100, 28), Location = new Point(660, 30) };
            btnToggleView.Click += (s, e) => { usingCards = !usingCards; RenderCurrentView(); };

            p.Controls.Add(title);
            p.Controls.Add(subtitle);
            p.Controls.Add(btnAgregar);
            p.Controls.Add(btnToggleView);
            return p;
        }

        private Panel CrearPanelEstadisticasOptimizado()
        {
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Name = "panelEstadisticas" };
            ActualizarEstadisticas(panel);
            return panel;
        }

        private void ActualizarEstadisticas(Panel panelEstadisticas)
        {
            panelEstadisticas.Controls.Clear();
            var totalEspacios = (bindingList != null) ? bindingList.Count : espacios.Count;
            var totalEdificios = (bindingList != null) ? bindingList.Count(s => s.Type == "building") : espacios.Count(s => s.Type == "building");
            var totalAulas = (bindingList != null) ? bindingList.Count(s => s.Type == "classroom") : espacios.Count(s => s.Type == "classroom");
            var totalDeportivos = (bindingList != null) ? bindingList.Count(s => s.Type == "sports") : espacios.Count(s => s.Type == "sports");

            int x = 6, w = 220, sp = 12;
            panelEstadisticas.Controls.Add(CrearCardEstadistica("Total Espacios", totalEspacios, x, w)); x += w + sp;
            panelEstadisticas.Controls.Add(CrearCardEstadistica("Edificios", totalEdificios, x, w)); x += w + sp;
            panelEstadisticas.Controls.Add(CrearCardEstadistica("Aulas", totalAulas, x, w)); x += w + sp;
            panelEstadisticas.Controls.Add(CrearCardEstadistica("Espacios Deportivos", totalDeportivos, x, w));
        }

        private Panel CrearCardEstadistica(string texto, int valor, int x, int width)
        {
            var card = new Panel { Size = new Size(width, 80), Location = new Point(x, 12), BackColor = Color.FromArgb(248, 250, 252), BorderStyle = BorderStyle.FixedSingle };
            card.Controls.Add(new Label { Text = texto, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(10, 8), AutoSize = true });
            card.Controls.Add(new Label { Text = valor.ToString(), Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.FromArgb(30, 30, 30), Location = new Point(10, 34), AutoSize = true });
            return card;
        }

        // --- Grid: modular, same tuning pero separado ---
        private void ConfigurarDataGridViewOptimizado()
        {
            dgvEspacios = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoGenerateColumns = true,
                ScrollBars = ScrollBars.Both,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
                AllowUserToResizeRows = false,
                RowTemplate = { Height = 34 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
            };

            dgvEspacios.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvEspacios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dgvEspacios.DefaultCellStyle.Padding = new Padding(6);

            dgvEspacios.CellClick += DgvEspacios_CellClick;
            dgvEspacios.DataBindingComplete += (s, e) =>
            {
                ConfigurarColumnasDespuesDeDataBinding();
                if (dgvEspacios.Rows.Count > 0) dgvEspacios.FirstDisplayedScrollingRowIndex = 0;
                dgvEspacios.ClearSelection();
            };

            panelContenidoInterno.Controls.Add(dgvEspacios);
        }

        // --- Cards UI (FlowLayoutPanel) ---
        private void ConfigurarFlowCards()
        {
            flowCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.White,
                Padding = new Padding(8)
            };
            // keep hidden by default; RenderCurrentView() will toggle
            panelContenidoInterno.Controls.Add(flowCards);
        }

        private void RenderCurrentView()
        {
            if (bindingList == null) EnsureGridBound();

            dgvEspacios.Visible = !usingCards;
            flowCards.Visible = usingCards;

            if (usingCards) RenderCards();
            else dgvEspacios.Refresh();

            // update stats
            foreach (Control c in this.Controls)
            {
                if (c is TableLayoutPanel tlp)
                {
                    foreach (Control inner in tlp.Controls)
                    {
                        if (inner is Panel p && p.Name == "panelEstadisticas") { ActualizarEstadisticas(p); }
                    }
                }
            }
        }

        private void RenderCards()
        {
            flowCards.SuspendLayout();
            flowCards.Controls.Clear();
            foreach (var s in bindingList)
            {
                var card = new Panel { Size = new Size(260, 140), Margin = new Padding(8), BackColor = Color.FromArgb(250, 250, 250), BorderStyle = BorderStyle.FixedSingle };
                card.Controls.Add(new Label { Text = s.Name, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(10, 10), AutoSize = false, Size = new Size(180, 20) });
                card.Controls.Add(new Label { Text = s.TipoDisplay, Location = new Point(10, 36), AutoSize = true });
                card.Controls.Add(new Label { Text = s.CapacidadDisplay, Location = new Point(10, 56), AutoSize = true });
                card.Controls.Add(new Label { Text = s.Description, Location = new Point(10, 76), Size = new Size(180, 40), AutoEllipsis = true });

                var btnEditar = new Button { Text = "Editar", Size = new Size(60, 26), Location = new Point(200, 10), Tag = s };
                btnEditar.Click += (se, ev) => { var sp = (Space)((Button)se).Tag; if (MostrarEditor(sp)) { bindingSource.ResetBindings(false); RenderCurrentView(); } };

                var btnEliminar = new Button { Text = "Eliminar", Size = new Size(60, 26), Location = new Point(200, 42), Tag = s };
                btnEliminar.Click += (se, ev) => { var sp = (Space)((Button)se).Tag; if (MessageBox.Show($"Eliminar {sp.Name}?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes) { bindingList.Remove(sp); RenderCurrentView(); } };

                card.Controls.Add(btnEditar); card.Controls.Add(btnEliminar);
                flowCards.Controls.Add(card);
            }
            flowCards.ResumeLayout();
        }

        // --- Binding and utility ---
        public void EnsureGridBound()
        {
            if (bindingSource != null) { bindingSource.ResetBindings(false); return; }

            bindingList = new BindingList<Space>(espacios);
            bindingSource = new BindingSource { DataSource = bindingList };
            dgvEspacios.BindingContext = this.BindingContext;
            dgvEspacios.DataSource = bindingSource;

            // Keep cards in sync
            bindingList.ListChanged += (s, e) => { RenderCurrentView(); };
            RenderCurrentView();
        }

        private void ConfigurarColumnasDespuesDeDataBinding()
        {
            if (dgvEspacios.Columns.Count == 0) return;
            foreach (DataGridViewColumn col in dgvEspacios.Columns)
            {
                if (col.Name == "Id" || col.Name == "Features" || col.Name == "ParentId" || col.Name == "Floor" || col.Name == "Type")
                    col.Visible = false;
            }

            if (dgvEspacios.Columns.Contains("Name")) { dgvEspacios.Columns["Name"].HeaderText = "Nombre"; dgvEspacios.Columns["Name"].Width = 220; }
            if (dgvEspacios.Columns.Contains("TipoDisplay")) { dgvEspacios.Columns["TipoDisplay"].HeaderText = "Tipo"; dgvEspacios.Columns["TipoDisplay"].Width = 120; }
            if (dgvEspacios.Columns.Contains("CapacidadDisplay")) { dgvEspacios.Columns["CapacidadDisplay"].HeaderText = "Capacidad"; dgvEspacios.Columns["CapacidadDisplay"].Width = 110; }
            if (dgvEspacios.Columns.Contains("Description")) { dgvEspacios.Columns["Description"].HeaderText = "Descripción"; dgvEspacios.Columns["Description"].Width = 300; }

            AgregarColumnasDeBotonesOptimizado();
            dgvEspacios.ClearSelection();
        }

        private void AgregarColumnasDeBotonesOptimizado()
        {
            if (!dgvEspacios.Columns.Contains("Editar"))
            {
                var btnEditar = new DataGridViewButtonColumn { Name = "Editar", HeaderText = "", Text = "Editar", UseColumnTextForButtonValue = true, Width = 80 };
                dgvEspacios.Columns.Add(btnEditar);
            }
            if (!dgvEspacios.Columns.Contains("Eliminar"))
            {
                var btnEliminar = new DataGridViewButtonColumn { Name = "Eliminar", HeaderText = "", Text = "Eliminar", UseColumnTextForButtonValue = true, Width = 80 };
                dgvEspacios.Columns.Add(btnEliminar);
            }
        }

        private void DgvEspacios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var espacio = (Space)bindingList[e.RowIndex];
            var colName = dgvEspacios.Columns[e.ColumnIndex].Name;
            if (colName == "Editar") { if (MostrarEditor(espacio)) bindingSource.ResetBindings(false); }
            else if (colName == "Eliminar") { if (MessageBox.Show($"Eliminar {espacio.Name}?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes) bindingList.RemoveAt(e.RowIndex); }
        }

        private bool MostrarEditor(Space espacio)
        {
            using (var f = new Form())
            {
                f.Text = "Editar espacio";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.Size = new Size(460, 360);
                f.MaximizeBox = false; f.MinimizeBox = false;

                var lblName = new Label { Text = "Nombre:", Location = new Point(10, 14), AutoSize = true };
                var txtName = new TextBox { Text = espacio.Name, Location = new Point(100, 10), Width = 320 };

                var lblType = new Label { Text = "Tipo:", Location = new Point(10, 54), AutoSize = true };
                var cmbType = new ComboBox { Location = new Point(100, 50), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbType.Items.AddRange(new[] { "building", "classroom", "sports" });
                cmbType.SelectedItem = espacio.Type ?? "classroom";

                var lblCapacity = new Label { Text = "Capacidad:", Location = new Point(10, 94), AutoSize = true };
                var nudCapacity = new NumericUpDown { Location = new Point(100, 90), Width = 120, Minimum = 0, Maximum = 10000, Value = Math.Max(0, espacio.Capacity) };

                var lblDesc = new Label { Text = "Descripción:", Location = new Point(10, 134), AutoSize = true };
                var txtDesc = new TextBox { Text = espacio.Description, Location = new Point(100, 130), Width = 320, Height = 100, Multiline = true, ScrollBars = ScrollBars.Vertical };

                var btnOk = new Button { Text = "Guardar", DialogResult = DialogResult.OK, Location = new Point(240, 250), Size = new Size(90, 30) };
                var btnCancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(340, 250), Size = new Size(90, 30) };

                f.Controls.Add(lblName); f.Controls.Add(txtName);
                f.Controls.Add(lblType); f.Controls.Add(cmbType);
                f.Controls.Add(lblCapacity); f.Controls.Add(nudCapacity);
                f.Controls.Add(lblDesc); f.Controls.Add(txtDesc);
                f.Controls.Add(btnOk); f.Controls.Add(btnCancel);

                f.AcceptButton = btnOk; f.CancelButton = btnCancel;

                var dr = f.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    espacio.Name = txtName.Text.Trim();
                    espacio.Type = cmbType.SelectedItem?.ToString() ?? espacio.Type;
                    espacio.Capacity = (int)nudCapacity.Value;
                    espacio.Description = txtDesc.Text.Trim();
                    return true;
                }
                return false;
            }
        }
    }
}
