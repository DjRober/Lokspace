namespace Lokspace
{
    partial class ReservasPersonalesDocente
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
            this.label6 = new System.Windows.Forms.Label();
            this.btnNuevaReserva = new System.Windows.Forms.Button();
            this.listaReservasDocente = new System.Windows.Forms.DataGridView();
            this.btnCancelarReserva = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.listaReservasDocente)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label4.Location = new System.Drawing.Point(32, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(237, 29);
            this.label4.TabIndex = 3;
            this.label4.Text = "Gestion de Reservas";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label6.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label6.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label6.Location = new System.Drawing.Point(33, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(271, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Visualiza y administra tus reservas";
            // 
            // btnNuevaReserva
            // 
            this.btnNuevaReserva.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnNuevaReserva.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnNuevaReserva.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnNuevaReserva.Location = new System.Drawing.Point(852, 45);
            this.btnNuevaReserva.Name = "btnNuevaReserva";
            this.btnNuevaReserva.Size = new System.Drawing.Size(166, 49);
            this.btnNuevaReserva.TabIndex = 11;
            this.btnNuevaReserva.Text = "+ Nueva Reserva";
            this.btnNuevaReserva.UseVisualStyleBackColor = false;
            this.btnNuevaReserva.Click += new System.EventHandler(this.btnNuevaReserva_Click);
            // 
            // listaReservasDocente
            // 
            this.listaReservasDocente.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.listaReservasDocente.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.listaReservasDocente.Location = new System.Drawing.Point(37, 151);
            this.listaReservasDocente.Name = "listaReservasDocente";
            this.listaReservasDocente.RowHeadersWidth = 51;
            this.listaReservasDocente.RowTemplate.Height = 24;
            this.listaReservasDocente.Size = new System.Drawing.Size(981, 287);
            this.listaReservasDocente.TabIndex = 12;
            this.listaReservasDocente.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.listaReservasDocente_CellContentClick);
            // 
            // btnCancelarReserva
            // 
            this.btnCancelarReserva.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnCancelarReserva.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancelarReserva.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnCancelarReserva.Location = new System.Drawing.Point(852, 464);
            this.btnCancelarReserva.Name = "btnCancelarReserva";
            this.btnCancelarReserva.Size = new System.Drawing.Size(166, 49);
            this.btnCancelarReserva.TabIndex = 13;
            this.btnCancelarReserva.Text = "Cancelar Reserva";
            this.btnCancelarReserva.UseVisualStyleBackColor = false;
            this.btnCancelarReserva.Click += new System.EventHandler(this.btnCancelarReserva_Click);
            // 
            // ReservasPersonalesDocente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 558);
            this.Controls.Add(this.btnCancelarReserva);
            this.Controls.Add(this.listaReservasDocente);
            this.Controls.Add(this.btnNuevaReserva);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Name = "ReservasPersonalesDocente";
            this.Text = "ReservasPersonalesDocente";
            this.Load += new System.EventHandler(this.ReservasPersonalesDocente_Load);
            ((System.ComponentModel.ISupportInitialize)(this.listaReservasDocente)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnNuevaReserva;
        private System.Windows.Forms.DataGridView listaReservasDocente;
        private System.Windows.Forms.Button btnCancelarReserva;
    }
}