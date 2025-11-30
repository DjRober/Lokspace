namespace Lokspace
{
    partial class MainDocenteForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnConfiguracion = new System.Windows.Forms.Button();
            this.btnUsuarios = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCalendario = new System.Windows.Forms.Button();
            this.btnSolicitudes = new System.Windows.Forms.Button();
            this.btnReservas = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelDocente = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnConfiguracion);
            this.panel1.Controls.Add(this.btnUsuarios);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnCalendario);
            this.panel1.Controls.Add(this.btnSolicitudes);
            this.panel1.Controls.Add(this.btnReservas);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(203, 535);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnConfiguracion
            // 
            this.btnConfiguracion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnConfiguracion.Location = new System.Drawing.Point(43, 378);
            this.btnConfiguracion.Name = "btnConfiguracion";
            this.btnConfiguracion.Size = new System.Drawing.Size(123, 30);
            this.btnConfiguracion.TabIndex = 12;
            this.btnConfiguracion.Text = "Configuracion";
            this.btnConfiguracion.UseVisualStyleBackColor = true;
            // 
            // btnUsuarios
            // 
            this.btnUsuarios.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnUsuarios.Location = new System.Drawing.Point(43, 327);
            this.btnUsuarios.Name = "btnUsuarios";
            this.btnUsuarios.Size = new System.Drawing.Size(123, 32);
            this.btnUsuarios.TabIndex = 11;
            this.btnUsuarios.Text = "Usuarios";
            this.btnUsuarios.UseVisualStyleBackColor = true;
            this.btnUsuarios.Click += new System.EventHandler(this.button4_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label2.Location = new System.Drawing.Point(11, 290);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Administracion";
            // 
            // btnCalendario
            // 
            this.btnCalendario.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnCalendario.Location = new System.Drawing.Point(43, 214);
            this.btnCalendario.Name = "btnCalendario";
            this.btnCalendario.Size = new System.Drawing.Size(123, 32);
            this.btnCalendario.TabIndex = 9;
            this.btnCalendario.Text = "Calendario";
            this.btnCalendario.UseVisualStyleBackColor = true;
            this.btnCalendario.Click += new System.EventHandler(this.btnCalendario_Click);
            // 
            // btnSolicitudes
            // 
            this.btnSolicitudes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnSolicitudes.Location = new System.Drawing.Point(43, 165);
            this.btnSolicitudes.Name = "btnSolicitudes";
            this.btnSolicitudes.Size = new System.Drawing.Size(123, 33);
            this.btnSolicitudes.TabIndex = 8;
            this.btnSolicitudes.Text = "Solicitudes ";
            this.btnSolicitudes.UseVisualStyleBackColor = true;
            this.btnSolicitudes.Click += new System.EventHandler(this.btnEspacios_Click);
            // 
            // btnReservas
            // 
            this.btnReservas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnReservas.Location = new System.Drawing.Point(43, 120);
            this.btnReservas.Name = "btnReservas";
            this.btnReservas.Size = new System.Drawing.Size(123, 30);
            this.btnReservas.TabIndex = 7;
            this.btnReservas.Text = "Mis Reservas";
            this.btnReservas.UseVisualStyleBackColor = true;
            this.btnReservas.Click += new System.EventHandler(this.btnReservas_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label3.Location = new System.Drawing.Point(11, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Navegacion principal";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "LokSpace";
            // 
            // panelDocente
            // 
            this.panelDocente.Location = new System.Drawing.Point(213, 3);
            this.panelDocente.Name = "panelDocente";
            this.panelDocente.Size = new System.Drawing.Size(956, 535);
            this.panelDocente.TabIndex = 1;
            this.panelDocente.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDocente_Paint);
            // 
            // MainDocenteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 539);
            this.Controls.Add(this.panelDocente);
            this.Controls.Add(this.panel1);
            this.Name = "MainDocenteForm";
            this.Text = "MainDocenteForm";
            this.Load += new System.EventHandler(this.MainDocenteForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConfiguracion;
        private System.Windows.Forms.Button btnUsuarios;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCalendario;
        private System.Windows.Forms.Button btnSolicitudes;
        private System.Windows.Forms.Button btnReservas;
        private System.Windows.Forms.Panel panelDocente;
    }
}