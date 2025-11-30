namespace Lokspace
{
    partial class MainAdminForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelLateral = new System.Windows.Forms.Panel();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.btnGestionReservas = new System.Windows.Forms.Button();
            this.btnGestionEspacios = new System.Windows.Forms.Button();
            this.btnGestionCuentas = new System.Windows.Forms.Button();
            this.panelContenido = new System.Windows.Forms.Panel();
            this.panelLateral.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLateral
            // 
            this.panelLateral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLateral.Controls.Add(this.btnCerrarSesion);
            this.panelLateral.Controls.Add(this.btnGestionReservas);
            this.panelLateral.Controls.Add(this.btnGestionEspacios);
            this.panelLateral.Controls.Add(this.btnGestionCuentas);
            this.panelLateral.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLateral.Location = new System.Drawing.Point(0, 0);
            this.panelLateral.Name = "panelLateral";
            this.panelLateral.Size = new System.Drawing.Size(200, 631);
            this.panelLateral.TabIndex = 0;
            this.panelLateral.Paint += new System.Windows.Forms.PaintEventHandler(this.panelLateral_Paint);
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.Location = new System.Drawing.Point(12, 482);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Size = new System.Drawing.Size(168, 33);
            this.btnCerrarSesion.TabIndex = 3;
            this.btnCerrarSesion.Text = "Cerrar sesion";
            this.btnCerrarSesion.UseVisualStyleBackColor = true;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // 
            // btnGestionReservas
            // 
            this.btnGestionReservas.Location = new System.Drawing.Point(12, 210);
            this.btnGestionReservas.Name = "btnGestionReservas";
            this.btnGestionReservas.Size = new System.Drawing.Size(168, 33);
            this.btnGestionReservas.TabIndex = 2;
            this.btnGestionReservas.Text = "Gestionar reservas";
            this.btnGestionReservas.UseVisualStyleBackColor = true;
            this.btnGestionReservas.Click += new System.EventHandler(this.btnGestionReservas_Click);
            // 
            // btnGestionEspacios
            // 
            this.btnGestionEspacios.Location = new System.Drawing.Point(12, 117);
            this.btnGestionEspacios.Name = "btnGestionEspacios";
            this.btnGestionEspacios.Size = new System.Drawing.Size(168, 33);
            this.btnGestionEspacios.TabIndex = 1;
            this.btnGestionEspacios.Text = "Gestionar espacios";
            this.btnGestionEspacios.UseVisualStyleBackColor = true;
            this.btnGestionEspacios.Click += new System.EventHandler(this.btnGestionEspacios_Click);
            // 
            // btnGestionCuentas
            // 
            this.btnGestionCuentas.Location = new System.Drawing.Point(12, 22);
            this.btnGestionCuentas.Name = "btnGestionCuentas";
            this.btnGestionCuentas.Size = new System.Drawing.Size(168, 33);
            this.btnGestionCuentas.TabIndex = 0;
            this.btnGestionCuentas.Text = "Gestionar cuentas";
            this.btnGestionCuentas.UseVisualStyleBackColor = true;
            this.btnGestionCuentas.Click += new System.EventHandler(this.btnGestionCuentas_Click);
            // 
            // panelContenido
            // 
            this.panelContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContenido.Location = new System.Drawing.Point(200, 0);
            this.panelContenido.Name = "panelContenido";
            this.panelContenido.Size = new System.Drawing.Size(1493, 631);
            this.panelContenido.TabIndex = 0;
            this.panelContenido.Paint += new System.Windows.Forms.PaintEventHandler(this.panelContenido_Paint);
            // 
            // MainAdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1693, 631);
            this.Controls.Add(this.panelContenido);
            this.Controls.Add(this.panelLateral);
            this.Name = "MainAdminForm";
            this.Text = "MainAdminForm";
            this.Load += new System.EventHandler(this.MainAdminForm_Load);
            this.panelLateral.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLateral;
        private System.Windows.Forms.Panel panelContenido;
        private System.Windows.Forms.Button btnGestionCuentas;
        private System.Windows.Forms.Button btnGestionEspacios;
        private System.Windows.Forms.Button btnCerrarSesion;
        private System.Windows.Forms.Button btnGestionReservas;
    }
}