namespace Lokspace
{
    partial class NuevaReservaDocente
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
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnNuvaReserva = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCampoProposito = new System.Windows.Forms.TextBox();
            this.cmbEspacio = new System.Windows.Forms.ComboBox();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.dtpHoraInicio = new System.Windows.Forms.DateTimePicker();
            this.dtpHoraFin = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label4.Location = new System.Drawing.Point(135, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(243, 29);
            this.label4.TabIndex = 4;
            this.label4.Text = "Crear Nueva Reserva";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(31, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Espacio: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(45, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Fecha: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(351, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Hora inicio: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label5.Location = new System.Drawing.Point(372, 172);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Hora fin: ";
            // 
            // btnNuvaReserva
            // 
            this.btnNuvaReserva.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnNuvaReserva.Location = new System.Drawing.Point(355, 386);
            this.btnNuvaReserva.Name = "btnNuvaReserva";
            this.btnNuvaReserva.Size = new System.Drawing.Size(138, 46);
            this.btnNuvaReserva.TabIndex = 9;
            this.btnNuvaReserva.Text = "Guardar";
            this.btnNuvaReserva.UseVisualStyleBackColor = true;
            this.btnNuvaReserva.Click += new System.EventHandler(this.btnNuvaReserva_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnCancelar.Location = new System.Drawing.Point(542, 386);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(138, 46);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label6.Location = new System.Drawing.Point(20, 233);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "Proposito: ";
            // 
            // txtCampoProposito
            // 
            this.txtCampoProposito.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtCampoProposito.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtCampoProposito.Location = new System.Drawing.Point(140, 233);
            this.txtCampoProposito.Multiline = true;
            this.txtCampoProposito.Name = "txtCampoProposito";
            this.txtCampoProposito.Size = new System.Drawing.Size(510, 57);
            this.txtCampoProposito.TabIndex = 16;
            this.txtCampoProposito.TextChanged += new System.EventHandler(this.txtCampoProposito_TextChanged);
            // 
            // cmbEspacio
            // 
            this.cmbEspacio.FormattingEnabled = true;
            this.cmbEspacio.Location = new System.Drawing.Point(116, 168);
            this.cmbEspacio.Name = "cmbEspacio";
            this.cmbEspacio.Size = new System.Drawing.Size(200, 24);
            this.cmbEspacio.TabIndex = 17;
            this.cmbEspacio.SelectedIndexChanged += new System.EventHandler(this.cmbEspacio_SelectedIndexChanged);
            // 
            // dtpFecha
            // 
            this.dtpFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.dtpFecha.Location = new System.Drawing.Point(116, 114);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(200, 23);
            this.dtpFecha.TabIndex = 18;
            this.dtpFecha.ValueChanged += new System.EventHandler(this.dtpFecha_ValueChanged);
            // 
            // dtpHoraInicio
            // 
            this.dtpHoraInicio.CustomFormat = "hh:mm tt";
            this.dtpHoraInicio.Location = new System.Drawing.Point(457, 111);
            this.dtpHoraInicio.Name = "dtpHoraInicio";
            this.dtpHoraInicio.Size = new System.Drawing.Size(200, 22);
            this.dtpHoraInicio.TabIndex = 19;
            this.dtpHoraInicio.ValueChanged += new System.EventHandler(this.dtpHoraInicio_ValueChanged);
            // 
            // dtpHoraFin
            // 
            this.dtpHoraFin.CustomFormat = "hh:mm tt";
            this.dtpHoraFin.Location = new System.Drawing.Point(457, 174);
            this.dtpHoraFin.Name = "dtpHoraFin";
            this.dtpHoraFin.Size = new System.Drawing.Size(200, 22);
            this.dtpHoraFin.TabIndex = 20;
            this.dtpHoraFin.ValueChanged += new System.EventHandler(this.dtpHoraFin_ValueChanged);
            // 
            // NuevaReservaDocente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 481);
            this.Controls.Add(this.dtpHoraFin);
            this.Controls.Add(this.dtpHoraInicio);
            this.Controls.Add(this.dtpFecha);
            this.Controls.Add(this.cmbEspacio);
            this.Controls.Add(this.txtCampoProposito);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnNuvaReserva);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Name = "NuevaReservaDocente";
            this.Text = "NuevaReservaDocente";
            this.Load += new System.EventHandler(this.NuevaReservaDocente_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnNuvaReserva;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCampoProposito;
        private System.Windows.Forms.ComboBox cmbEspacio;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.DateTimePicker dtpHoraInicio;
        private System.Windows.Forms.DateTimePicker dtpHoraFin;
    }
}